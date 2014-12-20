using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF;

public partial class Reports_New : System.Web.UI.Page
{
    #region Initialization
    commonClass objcls = new commonClass();
    static string strConnection;
    OdbcConnection con = new OdbcConnection();
    clsCommon obj = new clsCommon();
    DateTime dt;
    DateTime Adate, Bdate, Rdate5; DateTime Actual1; DateTime Blk; DateTime Res;
    DataRow dr1; DateTime ADate;
    Decimal rrent = 0, rrent1 = 0, rdeposit = 0, rdeposit1 = 0, gtr, gtd;
    string d, y, m, g, rr, dde, pprt;
    int id;
    string strsql3, countr;
    string remarks;
    string name, place, building, room, indate, rents, deposits, num, stat, rec, outdate, states, dist, allocfrom, reason;
    int no = 0, transno;
    DateTime indat, outdat;
    string alloctype, passno, mpass;
    string rrr;
    string ind, outd, it, ot, build;
    string reporttime, report, Sname, f1; 
    string number;
    int slno = 0, seasonID;
    int firstrec, lastrec, totrec, misrec, miss, nrec;
    int useid;
    string frmdate,toodate,f;
    DateTime fromdate, todate;
    string idproof;
    string collectioncurrent,collectionprev,datecurrent,dateprev;
    double variations;
    DataTable dt_stseason = new DataTable();
    clsCommon obc = new clsCommon();
    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                useid = int.Parse(Session["userid"].ToString());
            }
            catch
            {
            }
            ViewState["action"] = "NILL";
            Title = "Tsunami ARMS - Reports Test";
            ViewState["click"] = "no"; 
            ViewState["clkremit"] = "no";
             strConnection = obj.ConnectionString();

            #region current date selection
            try
            {
                OdbcCommand cmd46 = new OdbcCommand();
                cmd46.Parameters.AddWithValue("tblname", "t_dayclosing");
                cmd46.Parameters.AddWithValue("attribute", "closedate_start");
                cmd46.Parameters.AddWithValue("conditionv", "daystatus='open'");
                DataTable dtt46 = new DataTable();
                dtt46 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd46);
                dt = DateTime.Parse(dtt46.Rows[0][0].ToString());
                string dtdd = dt.ToString("yyyy/MM/dd");
                Session["dayend"] = dtdd.ToString();
                txtdate.Text = dt.ToString("dd/MM/yyyy");
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Current date not set ...Please set current date.");
            }
            #endregion

            try
            {
                OdbcCommand cmd2051 = new OdbcCommand();
                cmd2051.CommandType = CommandType.StoredProcedure;
                cmd2051.Parameters.AddWithValue("tblname", "m_sub_counter");
                cmd2051.Parameters.AddWithValue("attribute", "counter_id,counter_ip");
                cmd2051.Parameters.AddWithValue("conditionv", "  rowstatus<>2");
                DataTable dtt2051 = new DataTable();
                dtt2051 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2051);
                if (dtt2051.Rows.Count > 0)
                {
                    DataRow dtt2051row3 = dtt2051.NewRow();
                    dtt2051row3["counter_ip"] = "All";
                    dtt2051row3["counter_id"] = "-1";
                    dtt2051.Rows.InsertAt(dtt2051row3, 0);
                    cmbcounter.DataSource = dtt2051;
                    cmbcounter.DataBind();
                }
                else
                {
                    okmessage("Tsunami ARMS - Warning", "No counter is set");
                    return;
                }
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "No counter is set");
                return;
            }

            OdbcCommand ddh = new OdbcCommand();
            ddh.Parameters.AddWithValue("tblname", "m_sub_season s,m_season m");
            ddh.Parameters.AddWithValue("attribute", "distinct  s.season_sub_id, s.seasonname");
            ddh.Parameters.AddWithValue("conditionv", "s.rowstatus <> 2 and s.season_sub_id=m.season_sub_id");
            DataTable dttf = new DataTable();
            dttf = objcls.SpDtTbl("call selectcond(?,?,?)", ddh);
            cmbseasoncomp.DataSource = dttf;
            cmbseasoncomp.DataBind();
            ddlseason.DataSource = dttf;
            ddlseason.DataBind();
            
        OdbcCommand cmdS = new OdbcCommand();
        cmdS.Parameters.AddWithValue("tblname", "m_season");
        cmdS.Parameters.AddWithValue("attribute", "season_id,season_sub_id, DATE_FORMAT(CAST(startdate AS CHAR(12)),'%d/%m/%Y' ) AS 'startdate',DATE_FORMAT(CAST(enddate AS CHAR(12)),'%d/%m/%Y' ) AS 'enddate'");
        cmdS.Parameters.AddWithValue("conditionv", "curdate() between  startdate and enddate and is_current=" + 1 + " and rowstatus<>" + 2 + "");
        DataTable dtS = new DataTable();
        dtS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdS);
        if (dtS.Rows.Count > 0)
        {
            int curseason1 = int.Parse(dtS.Rows[0]["season_id"].ToString());
            Session["season"] = curseason1.ToString();
            Session["seasonid"] = dtS.Rows[0]["season_id"].ToString();
            Session["seasonsubid"] = dtS.Rows[0]["season_sub_id"].ToString();
        
            DataTable dt_seas = objcls.DtTbl("SELECT season_sub_id,seasonname FROM  m_sub_season WHERE season_sub_id = '" + dtS.Rows[0]["season_sub_id"].ToString() + "'");
            if (dt_seas.Rows.Count > 0)
            {
                cmbseasoncomp.SelectedValue = dt_seas.Rows[0][0].ToString();
                ddlseason.SelectedValue = dt_seas.Rows[0][0].ToString();
            }
        }
        else
        {
            obc.ShowAlertMessage(this,"No season foun");
        }

         
            //string stseason = @"SELECT season_sub_id FROM  m_season WHERE enddate>=CURDATE()";
            //DataTable dt_stseason = objcls.DtTbl(stseason);
            //if (dt_stseason.Rows.Count > 0)
            //{
            //    cmbseasoncomp.SelectedValue = dt_stseason.Rows[0][0].ToString();
            //}



            OdbcCommand malyear = new OdbcCommand();
            malyear.Parameters.AddWithValue("tblname", "t_settings ");
            malyear.Parameters.AddWithValue("attribute", "mal_year,mal_year_id");
            //malyear.Parameters.AddWithValue("conditionv", "end_eng_date>=curdate()");
            DataTable dtyear = objcls.SpDtTbl("call selectdata(?,?)", malyear);
            //DataTable dtyear = objcls.SpDtTbl("call selectcond(?,?,?)", malyear);
            //curdate() between start_eng_date  and end_eng_date
            cmbyearcomp.DataSource = dtyear;
            cmbyearcomp.DataBind();

            string st = @"SELECT mal_year_id FROM  t_settings WHERE   end_eng_date>=CURDATE()";
            DataTable dt_st = objcls.DtTbl(st);
            if (dt_st.Rows.Count > 0)
            {
                cmbyearcomp.SelectedValue = dt_st.Rows[0][0].ToString();
            }
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

    protected void btnOk_Click(object sender, EventArgs e)
    {

    }
    
    #region Button Yes
    protected void btnYes_Click(object sender, EventArgs e)
    {
        string counter = cmbcounter.SelectedItem.ToString();
        string frm = " ", cond = " ";
        if (counter != "All")
        {
            frm = " INNER JOIN m_sub_counter ON alloc.counter_id = m_sub_counter.counter_id ";
            cond = " AND m_sub_counter.counter_ip = '" + cmbcounter.SelectedItem.ToString() + "' ";
        }
        DateTime rdate = DateTime.Now;
        string repdate = rdate.ToString("yyyy/MM/dd");
        string reptime = rdate.ToShortTimeString();

        #region half print include full report

        if (ViewState["action"].ToString() == "Half Print include  on full report")
        {
            string strsql1 = "m_room as room,"
           + "m_sub_building as build,"
           + "t_roomallocation as alloc"
           + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
           + " Left join m_sub_district as dist on alloc.district_id=dist.district_id "
           + " left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id " + frm + "";
            string strsql2 = " alloc.alloc_id,"
                           + "alloc.alloc_no,"
                             + "alloc.pass_id,"
                           + "alloc.place,"
                           + "alloc.phone,"
                           + "alloc.idproof,"
                           + "alloc.idproofno,"
                           + "alloc.noofinmates,"
                           + "alloc.numberofunit,"
                           + "alloc.advance,"
                           + "alloc.reason,"
                           + "alloc.othercharge,"
                           + "alloc.adv_recieptno,"
                           + "alloc.swaminame,"
                           + "build.buildingname,"
                           + "room.roomno,"
                           + "alloc.allocdate,"
                           + "alloc.exp_vecatedate,"
                           + "alloc.roomrent,"
                           + "alloc.state_id,"
                           + "alloc.district_id,"
                           + "alloc.deposit,"
                           + "alloc.alloc_type,"
                           + "alloc.totalcharge,"
                           + "alloc.realloc_from,"
                           + "alloc.reason_id,"
                           + "actualvecdate";
            strsql3 = "alloc.room_id=room.room_id"
                  + " and room.build_id=build.build_id"
                  + " and alloc.dayend='" + Session["ledgerDate"].ToString() + "' " + cond + " order by alloc.alloc_id asc";
            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", strsql1);
            cmd350.Parameters.AddWithValue("attribute", strsql2);
            cmd350.Parameters.AddWithValue("conditionv", strsql3);
            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
            int cont = dtt350.Rows.Count;
            int hp = cont % 20;
            Session["cont"] = cont.ToString();
            Session["hp"] = hp.ToString();
            Session["rep"] = "full";
            if (dtt350.Rows.Count == 0)
            {
                okmessage("Tsunami ARMS - Warning", "No details found");
                return;
            }
            DateTime reporttime = DateTime.Now;
            report = "Ledger Report" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

            PdfPTable table1 = new PdfPTable(10);
            float[] colWidths1 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
            table1.SetWidths(colWidths1);

            string repdates = rdate.ToString("dd/MM/yyyy");
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
            string dateee = ss.ToString("dd-MMMM-yyyy");

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "", fontLB)));
            cell500.Colspan = 9;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 5;
            cell501.Border = 0;
            cell501.HorizontalAlignment = 0;
            table1.AddCell(cell501);

            PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
            cell502.Colspan = 4;
            cell502.Border = 0;
            cell502.HorizontalAlignment = 2;
            table1.AddCell(cell502);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell2);

            PdfPCell cell3ee = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
            table1.AddCell(cell3ee);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
            table1.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell5);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
            table1.AddCell(cell7);

            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
            table1.AddCell(cell8);

            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
            table1.AddCell(cell9);

            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
            table1.AddCell(cell10);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
            table1.AddCell(cell11);

            doc.Add(table1);
            int i = 0;
            for (int ii = 0; ii < cont; ii++)
            {
                if (i > 26)
                {
                    doc.NewPage();
                    PdfPTable table4 = new PdfPTable(10);
                    float[] colWidths4 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                    table4.SetWidths(colWidths4);

                    PdfPTable table3 = new PdfPTable(10);
                    float[] colWidths3 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                    table3.SetWidths(colWidths3);

                    PdfPCell cell500p = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "", fontLB)));
                    cell500p.Colspan = 9;
                    cell500p.Border = 1;
                    cell500p.HorizontalAlignment = 1;
                    table3.AddCell(cell500p);

                    PdfPCell cell501p = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                    cell501p.Colspan = 5;
                    cell501p.Border = 0;
                    cell501p.HorizontalAlignment = 0;
                    table3.AddCell(cell501p);

                    PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                    cell502p.Colspan = 4;
                    cell502p.Border = 0;
                    cell502p.HorizontalAlignment = 2;
                    table3.AddCell(cell502p);

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
                    table3.AddCell(cell10);

                    PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                    table3.AddCell(cell11p);

                    PdfPCell cell12q = new PdfPCell(new Phrase(new Chunk("ID Proof:", font9)));
                    table3.AddCell(cell12q);

                    i = 0;
                    doc.Add(table3);
                }
                PdfPTable table = new PdfPTable(10);
                float[] colWidths = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                table.SetWidths(colWidths);
                transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                num = dtt350.Rows[ii]["alloc_no"].ToString();
                Session["num"] = num.ToString();
                name = dtt350.Rows[ii]["swaminame"].ToString();
                place = dtt350.Rows[ii]["place"].ToString();
                states = dtt350.Rows[ii]["state_id"].ToString();
                dist = dtt350.Rows[ii]["district_id"].ToString();
                rec = dtt350.Rows[ii]["adv_recieptno"].ToString();
                allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                reason = dtt350.Rows[ii]["reason_id"].ToString();
                alloctype = dtt350.Rows[ii]["alloc_type"].ToString();

                #region extent remark&alter remark
                if (allocfrom != "")
                {
                    if (reason != "")
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                        if (dtallocfr.Rows.Count > 0)
                        {
                            remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }
                    }
                    else
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
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
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());
                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor Paid Allocation")
                {
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());
                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor multiple pass")
                {                    
                    int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                    mpass = "";
                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                    cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                    cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
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

                build = "";
                building = dtt350.Rows[ii]["buildingname"].ToString();
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
                room = dtt350.Rows[ii]["roomno"].ToString();
                Session["rec"] = rec.ToString();
                Session["tno"] = transno.ToString();
                indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                ind = indat.ToString("dd-MMM");
                it = indat.ToString("hh:mm:tt");
                indate = it + "       " + ind;
                if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                {
                    outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;
                }
                else
                {
                    outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;
                }
                rents = dtt350.Rows[ii]["roomrent"].ToString();
                deposits = dtt350.Rows[ii]["deposit"].ToString();
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

                PdfPCell cell32 = new PdfPCell(new Phrase(new Chunk(idproof, font8)));
                table.AddCell(cell32);

                doc.Add(table);
                i++;
                if ((i == 27) || (ii == cont - 1))
                {
                    PdfPTable table2 = new PdfPTable(10);
                    float[] colWidths2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                    table2.SetWidths(colWidths2);

                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                    cell41.Colspan = 6;
                    table2.AddCell(cell41);

                    PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                    table2.AddCell(cell49);

                    gtr = gtr + decimal.Parse(rr.ToString());
                    gtd = gtd + decimal.Parse(dde.ToString());

                    PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                    table2.AddCell(cell50);

                    PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table2.AddCell(cell51);

                    doc.Add(table2);
                    rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                }
                if (ii == cont - 1)
                {
                    PdfPTable table10 = new PdfPTable(10);
                    float[] colWidths10 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                    table10.SetWidths(colWidths10);

                    PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500p10.Colspan = 9;
                    cell500p10.Border = 0;
                    cell500p10.HorizontalAlignment = 1;
                    table10.AddCell(cell500p10);
                    /////////////////////
                    PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
                    cell500p12.Colspan = 2;
                    cell500p12.Border = 0;
                    cell500p12.HorizontalAlignment = 0;
                    table10.AddCell(cell500p12);

                    PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
                    cell500p13.Colspan = 3;
                    cell500p13.Border = 0;
                    cell500p13.HorizontalAlignment = 0;
                    table10.AddCell(cell500p13);

                    PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("Deposit : ", font10)));
                    cell500p15.Colspan = 2;
                    cell500p15.Border = 0;
                    cell500p15.HorizontalAlignment = 0;
                    table10.AddCell(cell500p15);

                    PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), font10)));
                    cell500p11.Colspan = 2;
                    cell500p11.Border = 1;
                    cell500p11.HorizontalAlignment = 1;
                    table10.AddCell(cell500p11);
                    ///////////////////
                    PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", font10)));
                    cell500p14.Colspan = 2;
                    cell500p14.Border = 1;
                    cell500p14.HorizontalAlignment = 1;
                    table10.AddCell(cell500p14);

                    //NumberToEnglish n = new NumberToEnglish();
                    string re = objcls.NumberToTextWithLakhs(Int64.Parse(gtr.ToString()));
                    re = re + " Only";
                    PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, font10)));
                    cell500p16.Colspan = 7;
                    cell500p16.Border = 1;
                    cell500p16.HorizontalAlignment = 1;
                    table10.AddCell(cell500p16);

                    PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("Deposit: ", font10)));
                    cell500p17.Colspan = 2;
                    cell500p17.Border = 1;
                    cell500p17.HorizontalAlignment = 1;
                    table10.AddCell(cell500p17);

                    string de = objcls.NumberToTextWithLakhs(Int64.Parse(gtd.ToString()));
                    de = de + " Only";
                    PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk(de, font10)));
                    cell500p18.Colspan = 7;
                    cell500p18.Border = 1;
                    cell500p18.HorizontalAlignment = 1;
                    table10.AddCell(cell500p18);
                    gtr = 0;
                    gtd = 0;

                    PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb1.PaddingLeft = 20;
                    cellfb1.Colspan = 9;
                    cellfb1.MinimumHeight = 30;
                    cellfb1.Border = 0;
                    table10.AddCell(cellfb1);

                    PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                    cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb.PaddingLeft = 20;
                    cellfb.Colspan = 9;
                    cellfb.MinimumHeight = 30;
                    cellfb.Border = 0;
                    table10.AddCell(cellfb);

                    PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                    cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellf1b.PaddingLeft = 20;
                    cellf1b.Colspan = 9;
                    cellf1b.Border = 0;

                    table10.AddCell(cellf1b);

                    PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                    cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cellh2.PaddingLeft = 20;
                    cellh2.Border = 0;
                    cellh2.Colspan = 9;
                    table10.AddCell(cellh2);
                    doc.Add(table10);
                }
            }
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            try
            {
                OdbcCommand cmd901 = new OdbcCommand();
                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                cmd901.Parameters.AddWithValue("attribute", "max(slno)");

                DataTable dtt901 = new DataTable();
                dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                id = int.Parse(dtt901.Rows[0][0].ToString());
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd25 = new OdbcCommand();
                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                int q1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
            }
            catch
            {
                id = 1;
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd589 = new OdbcCommand();
                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                int q2 = objcls.Procedures("CALL savedata(?,?)", cmd589);
            }
        }
        #endregion

        #region Half print not full report
        if (ViewState["action"].ToString() == "Half Print not full report")
        {
            string strsql1 = "m_room as room,"
         + "m_sub_building as build,"
         + "t_roomallocation as alloc"
         + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
         + " Left join m_sub_district as dist on alloc.district_id=dist.district_id"
         + " left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id " + frm + "";
            string strsql2 = "alloc.alloc_id,"
                           + "alloc.alloc_no,"
                           + "alloc.place,"
                           + "alloc.phone,"
                           + "alloc.adv_recieptno,"
                             + "alloc.pass_id,"
                           + "alloc.idproof,"
                           + "alloc.idproofno,"
                           + "alloc.noofinmates,"
                           + "alloc.numberofunit,"
                           + "alloc.advance,"
                           + "alloc.reason,"
                           + "alloc.othercharge,"
                           + "alloc.adv_recieptno,"
                           + "alloc.swaminame,"
                           + "build.buildingname,"
                           + "room.roomno,"
                           + "alloc.allocdate,"
                           + "alloc.exp_vecatedate,"
                           + "alloc.roomrent,"
                           + "alloc.state_id,"
                           + "alloc.district_id,"
                           + "alloc.deposit,"
                           + "alloc.alloc_type,"
                           + "alloc.totalcharge,"
                           + "alloc.realloc_from,"
                           + "alloc.reason_id,"
                           + "actualvecdate";
            int s = int.Parse(Session["tno"].ToString());
            strsql3 = "alloc.room_id=room.room_id"
             + " and room.build_id=build.build_id"
             + " and alloc.dayend='" + Session["ledgerDate"].ToString() + "'"
             + " and alloc.alloc_id>" + s + " " + cond + " order by alloc.alloc_id asc";
            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", strsql1);
            cmd350.Parameters.AddWithValue("attribute", strsql2);
            cmd350.Parameters.AddWithValue("conditionv", strsql3);
            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
            int cont = dtt350.Rows.Count;
            int hp = cont % 20;
            Session["cont"] = cont.ToString();
            Session["hp"] = hp.ToString();
            DateTime reporttime = DateTime.Now;
            report = "Ledger Report" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            PdfPTable table1 = new PdfPTable(10);
            float[] colWidths1 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
            table1.SetWidths(colWidths1);

            string repdates = rdate.ToString("dd/MM/yyyy");
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
            string dateee = ss.ToString("dd-MMMM-yyyy");

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "", fontLB)));
            cell500.Colspan = 9;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 5;
            cell501.Border = 0;
            cell501.HorizontalAlignment = 0;
            table1.AddCell(cell501);

            PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
            cell502.Colspan = 4;
            cell502.Border = 0;
            cell502.HorizontalAlignment = 2;
            table1.AddCell(cell502);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell2);


            PdfPCell cell3rr = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
            table1.AddCell(cell3rr);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
            table1.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell5);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
            table1.AddCell(cell7);

            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
            table1.AddCell(cell8);

            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
            table1.AddCell(cell9);

            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
            table1.AddCell(cell10);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
            table1.AddCell(cell11);

            PdfPCell cell11w = new PdfPCell(new Phrase(new Chunk("Id Proof:", font9)));
            table1.AddCell(cell11w);

            doc.Add(table1);

            int i = 0;

            for (int ii = 0; ii < cont; ii++)
            {
                if (i > 26)
                {
                    doc.NewPage();
                    PdfPTable table4 = new PdfPTable(10);
                    float[] colWidths4 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                    table4.SetWidths(colWidths4);


                    PdfPTable table3 = new PdfPTable(9);
                    float[] colWidths3 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                    table3.SetWidths(colWidths3);


                    PdfPCell cell500p = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "", fontLB)));
                    cell500p.Colspan = 9;
                    cell500p.Border = 1;
                    cell500p.HorizontalAlignment = 1;
                    table3.AddCell(cell500p);

                    PdfPCell cell501p = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                    cell501p.Colspan = 5;
                    cell501p.Border = 0;
                    cell501p.HorizontalAlignment = 0;
                    table3.AddCell(cell501p);

                    PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                    cell502p.Colspan = 4;
                    cell502p.Border = 0;
                    cell502p.HorizontalAlignment = 2;
                    table3.AddCell(cell502p);

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
                    table3.AddCell(cell10);

                    PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                    table3.AddCell(cell11p);

                    PdfPCell cell11wq = new PdfPCell(new Phrase(new Chunk("Id Proof:", font9)));
                    table1.AddCell(cell11wq);

                    i = 0;
                    doc.Add(table3);
                }

                PdfPTable table = new PdfPTable(10);
                float[] colWidths = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                table.SetWidths(colWidths);

                transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                num = dtt350.Rows[ii]["alloc_no"].ToString();
                Session["num"] = num.ToString();
                name = dtt350.Rows[ii]["swaminame"].ToString();
                place = dtt350.Rows[ii]["place"].ToString();
                states = dtt350.Rows[ii]["state_id"].ToString();
                dist = dtt350.Rows[ii]["district_id"].ToString();
                rec = dtt350.Rows[ii]["adv_recieptno"].ToString();
                allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                reason = dtt350.Rows[ii]["reason_id"].ToString();
                alloctype = dtt350.Rows[ii]["alloc_type"].ToString();
                idproof = dtt350.Rows[ii]["idproof"].ToString();

                #region extent remark&alter remark
                if (allocfrom != "")
                {
                    if (reason != "")
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
                        if (dtallocfr.Rows.Count > 0)
                        {
                            remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }
                    }
                    else
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
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
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());
                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor Paid Allocation")
                {
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor multiple pass")
                {
                    int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                    mpass = "";
                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                    cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                    cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
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

                build = "";
                building = dtt350.Rows[ii]["buildingname"].ToString();
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
                room = dtt350.Rows[ii]["roomno"].ToString();
                Session["rec"] = rec.ToString();
                Session["tno"] = transno.ToString();
                indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                ind = indat.ToString("dd-MMM");
                it = indat.ToString("hh:mm:tt");
                indate = it + "       " + ind;
                if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                {
                    outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;
                }
                else
                {
                    outdat = DateTime.Parse(dtt350.Rows[ii]["actualdate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;
                }
                rents = dtt350.Rows[ii]["roomrent"].ToString();
                deposits = dtt350.Rows[ii]["deposit"].ToString();
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

                PdfPCell cell31a = new PdfPCell(new Phrase(new Chunk(idproof, font8)));
                table.AddCell(cell31a);

                doc.Add(table);
                i++;

                if ((i == 27) || (ii == cont - 1))
                {
                    PdfPTable table2 = new PdfPTable(10);
                    float[] colWidths2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                    table2.SetWidths(colWidths2);

                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                    cell41.Colspan = 6;
                    table2.AddCell(cell41);

                    PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                    table2.AddCell(cell49);

                    gtr = gtr + decimal.Parse(rr.ToString());
                    gtd = gtd + decimal.Parse(dde.ToString());

                    PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                    table2.AddCell(cell50);

                    PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table2.AddCell(cell51);

                    doc.Add(table2);
                    rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                }
                if (ii == cont - 1)
                {
                    PdfPTable table10 = new PdfPTable(10);
                    float[] colWidths10 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                    table10.SetWidths(colWidths10);

                    PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500p10.Colspan = 9;
                    cell500p10.Border = 0;
                    cell500p10.HorizontalAlignment = 1;
                    table10.AddCell(cell500p10);
            
                    PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
                    cell500p12.Colspan = 2;
                    cell500p12.Border = 0;
                    cell500p12.HorizontalAlignment = 0;
                    table10.AddCell(cell500p12);

                    PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
                    cell500p13.Colspan = 3;
                    cell500p13.Border = 0;
                    cell500p13.HorizontalAlignment = 0;
                    table10.AddCell(cell500p13);

                    PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("Deposit : ", font10)));
                    cell500p15.Colspan = 2;
                    cell500p15.Border = 0;
                    cell500p15.HorizontalAlignment = 0;
                    table10.AddCell(cell500p15);

                    PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), font10)));
                    cell500p11.Colspan = 2;
                    cell500p11.Border = 1;
                    cell500p11.HorizontalAlignment = 1;
                    table10.AddCell(cell500p11);
                
                    PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", font10)));
                    cell500p14.Colspan = 2;
                    cell500p14.Border = 1;
                    cell500p14.HorizontalAlignment = 1;
                    table10.AddCell(cell500p14);

                    //NumberToEnglish n = new NumberToEnglish();
                    string re = objcls.NumberToTextWithLakhs(Int64.Parse(gtr.ToString()));
                    re = re + " Only";
                    PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, font10)));
                    cell500p16.Colspan = 7;
                    cell500p16.Border = 1;
                    cell500p16.HorizontalAlignment = 1;
                    table10.AddCell(cell500p16);

                    PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("Deposit: ", font10)));
                    cell500p17.Colspan = 2;
                    cell500p17.Border = 1;
                    cell500p17.HorizontalAlignment = 1;
                    table10.AddCell(cell500p17);

                    string de = objcls.NumberToTextWithLakhs(Int64.Parse(gtd.ToString()));
                    de = de + " Only";
                    PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk(de, font10)));
                    cell500p18.Colspan = 7;
                    cell500p18.Border = 1;
                    cell500p18.HorizontalAlignment = 1;
                    table10.AddCell(cell500p18);
                    gtr = 0;
                    gtd = 0;

                    PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb1.PaddingLeft = 20;
                    cellfb1.Colspan = 9;
                    cellfb1.MinimumHeight = 30;
                    cellfb1.Border = 0;
                    table10.AddCell(cellfb1);

                    PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                    cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb.PaddingLeft = 20;
                    cellfb.Colspan = 9;
                    cellfb.MinimumHeight = 30;
                    cellfb.Border = 0;
                    table10.AddCell(cellfb);

                    PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                    cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellf1b.PaddingLeft = 20;
                    cellf1b.Colspan = 9;
                    cellf1b.Border = 0;

                    table10.AddCell(cellf1b);

                    PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                    cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cellh2.PaddingLeft = 20;
                    cellh2.Border = 0;
                    cellh2.Colspan = 9;
                    table10.AddCell(cellh2);

                    doc.Add(table10);
                }
            }
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

            try
            {
                OdbcCommand cmd901 = new OdbcCommand();
                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                DataTable dtt901 = new DataTable();
                dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                id = int.Parse(dtt901.Rows[0][0].ToString());

                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd25 = new OdbcCommand();
                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                int aq1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
            }
            catch
            {
                id = 1;
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();

                OdbcCommand cmd589 = new OdbcCommand();
                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                int aq2 = objcls.Procedures("CALL savedata(?,?)", cmd589);
            }
        }
        #endregion

        #region  half print
        if (ViewState["action"].ToString() == "Half Print")
        {
            string strsql1 = "m_room as room,"
                  + "m_sub_building as build,"
                  + "t_roomallocation as alloc"
                  + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                  + " Left join m_sub_district as dist on alloc.district_id=dist.district_id left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id " + frm + "";
            string strsql2 = "alloc.alloc_id,"
                           + "alloc.alloc_no,"
                           + "alloc.place,"
                           + "alloc.adv_recieptno,"
                           + "alloc.phone,"
                             + "alloc.pass_id,"
                           + "alloc.idproof,"
                           + "alloc.idproofno,"
                           + "alloc.noofinmates,"
                           + "alloc.numberofunit,"
                           + "alloc.advance,"
                           + "alloc.reason,"
                           + "alloc.othercharge,"
                           + "alloc.adv_recieptno,"
                           + "alloc.swaminame,"
                           + "build.buildingname,"
                           + "room.roomno,"
                           + "alloc.allocdate,"
                           + "alloc.exp_vecatedate,"
                           + "alloc.roomrent,"
                           + "alloc.state_id,"
                           + "alloc.district_id,"
                           + "alloc.deposit,"
                           + "alloc.alloc_type,"
                           + "alloc.totalcharge,"
                           + "alloc.realloc_from,"
                           + "alloc.reason_id,"
                           + "actualvecdate";
            int s = int.Parse(Session["tno"].ToString());
            strsql3 = "alloc.room_id=room.room_id"
             + " and room.build_id=build.build_id"
             + " and alloc.dayend='" + Session["ledgerDate"].ToString() + "'"
             + " and alloc.alloc_id>" + s + "  " + cond + " order by alloc.alloc_id asc";
            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", strsql1);
            cmd350.Parameters.AddWithValue("attribute", strsql2);
            cmd350.Parameters.AddWithValue("conditionv", strsql3);
            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
            int cont = dtt350.Rows.Count;
            int hp = cont % 20;
            Session["cont"] = cont.ToString();
            Session["hp"] = hp.ToString();
            DateTime reporttime = DateTime.Now;
            report = "Ledger Report" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

            PdfPTable table1 = new PdfPTable(10);
            float[] colWidths1 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
            table1.SetWidths(colWidths1);

            string repdates = rdate.ToString("dd/MM/yyyy");
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
            string dateee = ss.ToString("dd-MMMM-yyyy");

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "", fontLB)));
            cell500.Colspan = 9;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 5;
            cell501.Border = 0;
            cell501.HorizontalAlignment = 0;
            table1.AddCell(cell501);

            PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
            cell502.Colspan = 4;
            cell502.Border = 0;
            cell502.HorizontalAlignment = 2;
            table1.AddCell(cell502);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell2);

            PdfPCell cell3ff = new PdfPCell(new Phrase(new Chunk("rec", font9)));
            table1.AddCell(cell3ff);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
            table1.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell5);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
            table1.AddCell(cell7);

            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
            table1.AddCell(cell8);

            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
            table1.AddCell(cell9);

            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
            table1.AddCell(cell10);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
            table1.AddCell(cell11);

            PdfPCell cell11r = new PdfPCell(new Phrase(new Chunk("Id Proof:", font9)));
            table1.AddCell(cell11r);

            doc.Add(table1);

            int i = 0;

            for (int ii = 0; ii < cont; ii++)
            {
                if (i > 26)
                {
                    doc.NewPage();
                    PdfPTable table4 = new PdfPTable(10);
                    float[] colWidths4 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                    table4.SetWidths(colWidths4);

                    PdfPTable table3 = new PdfPTable(9);
                    float[] colWidths3 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                    table3.SetWidths(colWidths3);

                    PdfPCell cell500p = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "", fontLB)));
                    cell500p.Colspan = 9;
                    cell500p.Border = 1;
                    cell500p.HorizontalAlignment = 1;
                    table3.AddCell(cell500p);

                    PdfPCell cell501p = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                    cell501p.Colspan = 5;
                    cell501p.Border = 0;
                    cell501p.HorizontalAlignment = 0;
                    table3.AddCell(cell501p);

                    PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                    cell502p.Colspan = 4;
                    cell502p.Border = 0;
                    cell502p.HorizontalAlignment = 2;
                    table3.AddCell(cell502p);

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
                    table3.AddCell(cell10);

                    PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                    table3.AddCell(cell11p);

                    PdfPCell cell11pq = new PdfPCell(new Phrase(new Chunk("Id Proof:", font9)));
                    table3.AddCell(cell11pq);

                    i = 0;
                    doc.Add(table3);
                }

                PdfPTable table = new PdfPTable(10);
                float[] colWidths = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                table.SetWidths(colWidths);

                transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                num = dtt350.Rows[ii]["alloc_no"].ToString();
                Session["num"] = num.ToString();
                name = dtt350.Rows[ii]["swaminame"].ToString();
                place = dtt350.Rows[ii]["place"].ToString();
                states = dtt350.Rows[ii]["state_id"].ToString();
                dist = dtt350.Rows[ii]["district_id"].ToString();
                rec = dtt350.Rows[ii]["adv_recieptno"].ToString();

                allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                reason = dtt350.Rows[ii]["reason_id"].ToString();
                alloctype = dtt350.Rows[ii]["alloc_type"].ToString();

                #region extent remark&alter remark
                if (allocfrom != "")
                {
                    if (reason != "")
                    {

                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
                        if (dtallocfr.Rows.Count > 0)
                        {
                            remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }


                    }
                    else
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
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
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    //dacnt115.Fill(dtt115);
                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor Paid Allocation")
                {
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor multiple pass")
                {
                    //

                    int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                    mpass = "";

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                    cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                    cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

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

                build = "";
                building = dtt350.Rows[ii]["buildingname"].ToString();
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
                room = dtt350.Rows[ii]["roomno"].ToString();
                Session["rec"] = rec.ToString();
                Session["tno"] = transno.ToString();
                indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                ind = indat.ToString("dd-MMM");
                it = indat.ToString("hh:mm:tt");
                indate = it + "       " + ind;
                if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                {
                    outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;
                }
                else
                {
                    outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;
                }
                rents = dtt350.Rows[ii]["roomrent"].ToString();
                deposits = dtt350.Rows[ii]["deposit"].ToString();
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

                PdfPCell cell31u = new PdfPCell(new Phrase(new Chunk(idproof, font8)));
                table.AddCell(cell31u);

                doc.Add(table);
                i++;

                if ((i == 27) || (ii == cont - 1))
                {
                    PdfPTable table2 = new PdfPTable(10);
                    float[] colWidths2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                    table2.SetWidths(colWidths2);

                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                    cell41.Colspan = 6;
                    table2.AddCell(cell41);

                    PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                    table2.AddCell(cell49);

                    gtr = gtr + decimal.Parse(rr.ToString());
                    gtd = gtd + decimal.Parse(dde.ToString());

                    PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                    table2.AddCell(cell50);

                    PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table2.AddCell(cell51);

                    doc.Add(table2);
                    rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                }
                if (ii == cont - 1)
                {
                    PdfPTable table10 = new PdfPTable(10);
                    float[] colWidths10 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                    table10.SetWidths(colWidths10);

                    PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", fontLB)));
                    cell500p10.Colspan = 9;
                    cell500p10.Border = 0;
                    cell500p10.HorizontalAlignment = 1;
                    table10.AddCell(cell500p10);
                    /////////////////////
                    PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", fontLB)));
                    cell500p12.Colspan = 2;
                    cell500p12.Border = 0;
                    cell500p12.HorizontalAlignment = 0;
                    table10.AddCell(cell500p12);

                    PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), fontLB)));
                    cell500p13.Colspan = 3;
                    cell500p13.Border = 0;
                    cell500p13.HorizontalAlignment = 0;
                    table10.AddCell(cell500p13);

                    PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("Deposit : ", fontLB)));
                    cell500p15.Colspan = 2;
                    cell500p15.Border = 0;
                    cell500p15.HorizontalAlignment = 0;
                    table10.AddCell(cell500p15);


                    PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), fontLB)));
                    cell500p11.Colspan = 2;
                    cell500p11.Border = 1;
                    cell500p11.HorizontalAlignment = 1;
                    table10.AddCell(cell500p11);
                    ///////////////////
                    PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", fontLB)));
                    cell500p14.Colspan = 2;
                    cell500p14.Border = 1;
                    cell500p14.HorizontalAlignment = 1;
                    table10.AddCell(cell500p14);

                    // NumberToEnglish n = new NumberToEnglish();
                    string re = objcls.NumberToTextWithLakhs(Int64.Parse(gtr.ToString()));
                    re = re + " Only";
                    PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, fontLB)));
                    cell500p16.Colspan = 7;
                    cell500p16.Border = 1;
                    cell500p16.HorizontalAlignment = 1;
                    table10.AddCell(cell500p16);

                    PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("Deposit: ", fontLB)));
                    cell500p17.Colspan = 2;
                    cell500p17.Border = 1;
                    cell500p17.HorizontalAlignment = 1;
                    table10.AddCell(cell500p17);

                    string de = objcls.NumberToTextWithLakhs(Int64.Parse(gtd.ToString()));
                    de = de + " Only";
                    PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk(de, fontLB)));
                    cell500p18.Colspan = 7;
                    cell500p18.Border = 1;
                    cell500p18.HorizontalAlignment = 1;
                    table10.AddCell(cell500p18);
                    gtr = 0;
                    gtd = 0;

                    PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb1.PaddingLeft = 20;
                    cellfb1.Colspan = 9;
                    cellfb1.MinimumHeight = 30;
                    cellfb1.Border = 0;
                    table10.AddCell(cellfb1);

                    PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                    cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb.PaddingLeft = 20;
                    cellfb.Colspan = 9;
                    cellfb.MinimumHeight = 30;
                    cellfb.Border = 0;
                    table10.AddCell(cellfb);

                    PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                    cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellf1b.PaddingLeft = 20;
                    cellf1b.Colspan = 9;
                    cellf1b.Border = 0;
                    table10.AddCell(cellf1b);

                    PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                    cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cellh2.PaddingLeft = 20;
                    cellh2.Border = 0;
                    cellh2.Colspan = 9;
                    table10.AddCell(cellh2);
                    doc.Add(table10);
                }
            }

            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

            try
            {
                OdbcCommand cmd901 = new OdbcCommand();
                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                DataTable dtt901 = new DataTable();
                dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                id = int.Parse(dtt901.Rows[0][0].ToString());

                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd25 = new OdbcCommand();
                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                int sq1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
            }
            catch
            {
                id = 1;
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();

                OdbcCommand cmd589 = new OdbcCommand();
                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                int aq2 = objcls.Procedures("CALL savedata(?,?)", cmd589);
            }

        }
        #endregion

        #region fullprint
        if (ViewState["action"].ToString() == "Full Report")
        {
            string strsql1 = "m_room as room,"
            + "m_sub_building as build,"
            + "t_roomallocation as alloc"
            + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
            + " Left join m_sub_district as dist on alloc.district_id=dist.district_id left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id " + frm + "";
            string strsql2 = "alloc.alloc_id,"
                           + "alloc.alloc_no,"
                           + "alloc.place,"
                           + "alloc.adv_recieptno,"
                             + "alloc.pass_id,"
                           + "alloc.phone,"
                           + "alloc.idproof,"
                           + "alloc.idproofno,"
                           + "alloc.noofinmates,"
                           + "alloc.numberofunit,"
                           + "alloc.advance,"
                           + "alloc.reason,"
                           + "alloc.othercharge,"
                           + "alloc.adv_recieptno,"
                           + "alloc.swaminame,"
                           + "build.buildingname,"
                           + "room.roomno,"
                           + "alloc.allocdate,"
                           + "alloc.exp_vecatedate,"
                           + "alloc.roomrent,"
                           + "alloc.state_id,"
                           + "alloc.district_id,"
                           + "alloc.deposit,"
                           + "alloc.alloc_type,"
                           + "alloc.totalcharge,"
                           + "alloc.realloc_from,"
                           + "alloc.reason_id,"
                           + "actualvecdate";
            strsql3 = "alloc.room_id=room.room_id"
                  + " and room.build_id=build.build_id"
                  + " and alloc.dayend='" + Session["ledgerDate"].ToString() + "' " + cond + " order by alloc.alloc_id asc";
            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", strsql1);
            cmd350.Parameters.AddWithValue("attribute", strsql2);
            cmd350.Parameters.AddWithValue("conditionv", strsql3);
            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
            int cont = dtt350.Rows.Count;
            int hp = cont % 20;
            Session["cont"] = cont.ToString();
            Session["hp"] = hp.ToString();
            Session["rep"] = "full";
            Session["num"] = 1;
            if (dtt350.Rows.Count == 0)
            {
                okmessage("Tsunami ARMS - Warning", "No details found");
                return;
            }
            if (hp < 18)
            {
                lblMsg.Text = "Including half Print?";
                ViewState["action"] = "Half Print include  on full report";
                pnlOk.Visible = false;
                pnlYesNo.Visible = true;
                ModalPopupExtender1.Show();
                this.ScriptManager1.SetFocus(btnYes);
            }
            else
            {
                DateTime reporttime = DateTime.Now;
                report = "Ledger Report" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
                string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;
                Font font8 = FontFactory.GetFont("ARIAL", 9);
                Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
                Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
                Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
                pdfPage page = new pdfPage();
                page.strRptMode = "Allocation";
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;

                doc.Open();

                PdfPTable table1 = new PdfPTable(10);
                float[] colWidths1 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                table1.SetWidths(colWidths1);
                string repdates = rdate.ToString("dd/MM/yyyy");
                string dt1 = dt.ToString("dd/MM/yyyy");
                DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
                string dateee = ss.ToString("dd-MMMM-yyyy");

                PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter: " + cmbcounter.SelectedItem.ToString() + "", fontLB)));
                cell500.Colspan = 9;
                cell500.Border = 1;
                cell500.HorizontalAlignment = 1;
                table1.AddCell(cell500);

                PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                cell501.Colspan = 5;
                cell501.Border = 0;
                cell501.HorizontalAlignment = 0;
                table1.AddCell(cell501);

                PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                cell502.Colspan = 4;
                cell502.Border = 0;
                cell502.HorizontalAlignment = 2;
                table1.AddCell(cell502);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table1.AddCell(cell2);

                PdfPCell cellff3 = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
                table1.AddCell(cellff3);

                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
                table1.AddCell(cell3);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell5);

                PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
                table1.AddCell(cell7);

                PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
                table1.AddCell(cell8);

                PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
                table1.AddCell(cell9);

                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
                table1.AddCell(cell10);

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                table1.AddCell(cell11);

                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("ID Proof:", font9)));
                table1.AddCell(cell12);

                doc.Add(table1);

                int i = 0;

                for (int ii = 0; ii < cont; ii++)
                {
                    if (i > 26)
                    {
                        doc.NewPage();
                        PdfPTable table4 = new PdfPTable(10);
                        float[] colWidths4 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                        table4.SetWidths(colWidths4);

                        PdfPTable table3 = new PdfPTable(10);
                        float[] colWidths3 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                        table3.SetWidths(colWidths3);

                        PdfPCell cell500p = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter: " + cmbcounter.SelectedItem.ToString() + "", fontLB)));
                        cell500p.Colspan = 9;
                        cell500p.Border = 1;
                        cell500p.HorizontalAlignment = 1;
                        table3.AddCell(cell500p);

                        PdfPCell cell501p = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                        cell501p.Colspan = 5;
                        cell501p.Border = 0;
                        cell501p.HorizontalAlignment = 0;
                        table3.AddCell(cell501p);

                        PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                        cell502p.Colspan = 4;
                        cell502p.Border = 0;
                        cell502p.HorizontalAlignment = 2;
                        table3.AddCell(cell502p);

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
                        table3.AddCell(cell10);

                        PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                        table3.AddCell(cell11p);

                        PdfPCell cell12p = new PdfPCell(new Phrase(new Chunk("ID Proof:", font9)));
                        table3.AddCell(cell12p);
                        i = 0;

                        doc.Add(table3);
                    }

                    PdfPTable table = new PdfPTable(10);
                    float[] colWidths = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                    table.SetWidths(colWidths);

                    transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                    num = dtt350.Rows[ii]["alloc_no"].ToString();
                    Session["num"] = num.ToString();
                    name = dtt350.Rows[ii]["swaminame"].ToString();
                    place = dtt350.Rows[ii]["place"].ToString();
                    states = dtt350.Rows[ii]["state_id"].ToString();
                    dist = dtt350.Rows[ii]["district_id"].ToString();
                    rec = dtt350.Rows[ii]["adv_recieptno"].ToString();
                    allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                    reason = dtt350.Rows[ii]["reason_id"].ToString();
                    alloctype = dtt350.Rows[ii]["alloc_type"].ToString();

                    #region extent remark&alter remark
                    if (allocfrom != "")
                    {
                        if (reason != "")
                        {

                            OdbcCommand cmdallocfr = new OdbcCommand();
                            cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                            cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                            cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                            DataTable dtallocfr = new DataTable();
                            dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
                            if (dtallocfr.Rows.Count > 0)
                            {
                                remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                            }


                        }
                        else
                        {
                            OdbcCommand cmdallocfr = new OdbcCommand();
                            cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                            cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                            cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                            DataTable dtallocfr = new DataTable();
                            dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
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
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                    else if (alloctype == "Donor Paid Allocation")
                    {
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                    else if (alloctype == "Donor multiple pass")
                    {
                        //

                        int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                        mpass = "";

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                        cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                        cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");
                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
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

                    build = "";
                    building = dtt350.Rows[ii]["buildingname"].ToString();
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
                    room = dtt350.Rows[ii]["roomno"].ToString();
                    Session["rec"] = rec.ToString();
                    Session["tno"] = transno.ToString();
                    indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                    ind = indat.ToString("dd-MMM");
                    it = indat.ToString("hh:mm:tt");
                    indate = it + "       " + ind;
                    if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                    {
                        outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                        outd = outdat.ToString("dd-MMM");
                        ot = outdat.ToString("hh:mm:tt");
                        outdate = ot + "       " + outd;
                    }
                    else
                    {
                        outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                        outd = outdat.ToString("dd-MMM");
                        ot = outdat.ToString("hh:mm:tt");
                        outdate = ot + "       " + outd;

                    }
                    rents = dtt350.Rows[ii]["roomrent"].ToString();
                    deposits = dtt350.Rows[ii]["deposit"].ToString();
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

                    if ((i == 27) || (ii == cont - 1))
                    {
                        PdfPTable table2 = new PdfPTable(10);
                        float[] colWidths2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                        table2.SetWidths(colWidths2);

                        PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                        cell41.Colspan = 6;
                        table2.AddCell(cell41);

                        PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                        table2.AddCell(cell49);

                        gtr = gtr + decimal.Parse(rr.ToString());
                        gtd = gtd + decimal.Parse(dde.ToString());

                        PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                        table2.AddCell(cell50);

                        PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        table2.AddCell(cell51);
                        doc.Add(table2);
                        rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                    }

                    if (ii == cont - 1)
                    {
                        PdfPTable table10 = new PdfPTable(10);
                        float[] colWidths10 = { 60, 65, 130, 75, 85, 85, 75, 75, 50, 70};
                        table10.SetWidths(colWidths10);

                        PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", fontLB)));
                        cell500p10.Colspan = 9;
                        cell500p10.Border = 0;
                        cell500p10.HorizontalAlignment = 1;
                        table10.AddCell(cell500p10);
                        /////////////////////
                        PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", fontLB)));
                        cell500p12.Colspan = 2;
                        cell500p12.Border = 0;
                        cell500p12.HorizontalAlignment = 0;
                        table10.AddCell(cell500p12);

                        PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), fontLB)));
                        cell500p13.Colspan = 3;
                        cell500p13.Border = 0;
                        cell500p13.HorizontalAlignment = 0;
                        table10.AddCell(cell500p13);

                        PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("Deposit : ", fontLB)));
                        cell500p15.Colspan = 2;
                        cell500p15.Border = 0;
                        cell500p15.HorizontalAlignment = 0;
                        table10.AddCell(cell500p15);

                        PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), fontLB)));
                        cell500p11.Colspan = 2;
                        cell500p11.Border = 1;
                        cell500p11.HorizontalAlignment = 1;
                        table10.AddCell(cell500p11);
                        ///////////////////
                        PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", fontLB)));
                        cell500p14.Colspan = 2;
                        cell500p14.Border = 1;
                        cell500p14.HorizontalAlignment = 1;
                        table10.AddCell(cell500p14);

                        //NumberToEnglish n = new NumberToEnglish();
                        string re = objcls.NumberToTextWithLakhs(Int64.Parse(gtr.ToString()));
                        re = re + " Only";
                        PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, fontLB)));
                        cell500p16.Colspan = 7;
                        cell500p16.Border = 1;
                        cell500p16.HorizontalAlignment = 1;
                        table10.AddCell(cell500p16);

                        PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("Deposit: ", fontLB)));
                        cell500p17.Colspan = 2;
                        cell500p17.Border = 1;
                        cell500p17.HorizontalAlignment = 1;
                        table10.AddCell(cell500p17);

                        string de = objcls.NumberToTextWithLakhs(Int64.Parse(gtd.ToString()));
                        de = de + " Only";
                        PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk(de, fontLB)));
                        cell500p18.Colspan = 7;
                        cell500p18.Border = 1;
                        cell500p18.HorizontalAlignment = 1;
                        table10.AddCell(cell500p18);
                        gtr = 0;
                        gtd = 0;

                        PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellfb1.PaddingLeft = 20;
                        cellfb1.Colspan = 9;
                        cellfb1.MinimumHeight = 30;
                        cellfb1.Border = 0;
                        table10.AddCell(cellfb1);

                        PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                        cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellfb.PaddingLeft = 20;
                        cellfb.Colspan = 9;
                        cellfb.MinimumHeight = 30;
                        cellfb.Border = 0;
                        table10.AddCell(cellfb);

                        PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                        cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellf1b.PaddingLeft = 20;
                        cellf1b.Colspan = 9;
                        cellf1b.Border = 0;

                        table10.AddCell(cellf1b);

                        PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                        cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        cellh2.PaddingLeft = 20;
                        cellh2.Border = 0;
                        cellh2.Colspan = 9;
                        table10.AddCell(cellh2);
                        doc.Add(table10);
                    }
                }
                doc.Close();
                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);

                try
                {
                    OdbcCommand cmd901 = new OdbcCommand();
                    cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                    cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                    DataTable dtt901 = new DataTable();
                    dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                    id = int.Parse(dtt901.Rows[0][0].ToString());

                    int tno = int.Parse(Session["tno"].ToString());
                    string ct = Session["num"].ToString();
                    OdbcCommand cmd25 = new OdbcCommand();
                    cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                    cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                    cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                    int sd1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
                }
                catch
                {
                    id = 1;
                    int tno = int.Parse(Session["tno"].ToString());
                    string ct = Session["num"].ToString();

                    OdbcCommand cmd589 = new OdbcCommand();
                    cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                    cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                    int asq1 = objcls.Procedures("CALL savedata(?,?)", cmd589);
                }
            }
        #endregion

        }

    }
    #endregion

    #region Button No
    protected void btnNo_Click(object sender, EventArgs e)
    {
        DateTime rdate = DateTime.Now;
        string repdate = rdate.ToString("yyyy/MM/dd");
        string reptime = rdate.ToShortTimeString();

        #region Half Print not inlclude full report

        if (ViewState["action"].ToString() == "Half Print include  on full report")// not inlclude full report
        {
            try
            {
                string strsql1 = "m_room as room,"
              + "m_sub_building as build,"
              + "t_roomallocation as alloc"
              + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
              + " Left join m_sub_district as dist on alloc.district_id=dist.district_id "
              + " left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";

                string strsql2 = "alloc.alloc_id,"
                               + "alloc.alloc_no,"
                               + "alloc.place,"
                               + "alloc.phone,"
                               + "alloc.adv_recieptno,"
                               + "alloc.idproof,"
                               + "alloc.pass_id,"
                               + "alloc.idproofno,"
                               + "alloc.noofinmates,"
                               + "alloc.numberofunit,"
                               + "alloc.advance,"
                               + "alloc.reason,"
                               + "alloc.othercharge,"
                               + "alloc.adv_recieptno,"
                               + "alloc.swaminame,"
                               + "build.buildingname,"
                               + "room.roomno,"
                               + "alloc.allocdate,"
                               + "alloc.exp_vecatedate,"
                               + "alloc.roomrent,"
                               + "alloc.state_id,"
                               + "alloc.district_id,"
                               + "alloc.deposit,"
                               + "alloc.alloc_type,"
                               + "alloc.totalcharge,"
                               + "alloc.realloc_from,"
                               + "alloc.reason_id,"
                               + "actualvecdate";


                try
                {
                    int s = int.Parse(Session["tno"].ToString());
                }
                catch
                {
                }
                strsql3 = "alloc.room_id=room.room_id"
                 + " and room.build_id=build.build_id"
                 + " and alloc.dayend='" + Session["ledgerDate"].ToString() + "' order by alloc.alloc_id asc";

                OdbcCommand cmd350 = new OdbcCommand();
                cmd350.Parameters.AddWithValue("tblname", strsql1);
                cmd350.Parameters.AddWithValue("attribute", strsql2);
                cmd350.Parameters.AddWithValue("conditionv", strsql3);
                OdbcDataAdapter dacnt350 = new OdbcDataAdapter(cmd350);
                DataTable dtt350 = new DataTable();
                dacnt350.Fill(dtt350);

                int cont = dtt350.Rows.Count;
                int hp = cont % 20;
                Session["cont"] = cont.ToString();
                Session["hp"] = hp.ToString();

                cont = int.Parse(Session["cont"].ToString());
                hp = int.Parse(Session["hp"].ToString());
                cont = cont - hp;

                DateTime reporttime = DateTime.Now;
                report = "Ledger Report" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
                string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;
                Font font8 = FontFactory.GetFont("ARIAL", 9);
                Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
                Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
                Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
                pdfPage page = new pdfPage();
                page.strRptMode = "Allocation";
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;

                doc.Open();

                PdfPTable table1 = new PdfPTable(9);
                float[] colWidths1 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                table1.SetWidths(colWidths1);


                string repdates = rdate.ToString("dd/MM/yyyy");
                string dt1 = dt.ToString("dd/MM/yyyy");

                DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
                string dateee = ss.ToString("dd-MMMM-yyyy");

                PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
                cell500.Colspan = 9;
                cell500.Border = 1;
                cell500.HorizontalAlignment = 1;
                table1.AddCell(cell500);

                PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                cell501.Colspan = 5;
                cell501.Border = 0;
                cell501.HorizontalAlignment = 0;
                table1.AddCell(cell501);

                PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                cell502.Colspan = 4;
                cell502.Border = 0;
                cell502.HorizontalAlignment = 2;
                table1.AddCell(cell502);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table1.AddCell(cell2);

                PdfPCell cell3e = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
                table1.AddCell(cell3e);


                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
                table1.AddCell(cell3);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell5);

                PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
                table1.AddCell(cell7);

                PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
                table1.AddCell(cell8);

                PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
                table1.AddCell(cell9);

                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
                table1.AddCell(cell10);

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                table1.AddCell(cell11);

                doc.Add(table1);

                int i = 0;

                for (int ii = 0; ii < cont; ii++)
                {
                    if (i > 26)
                    {
                        doc.NewPage();
                        PdfPTable table4 = new PdfPTable(9);
                        float[] colWidths4 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                        table4.SetWidths(colWidths4);


                        PdfPTable table3 = new PdfPTable(9);
                        float[] colWidths3 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                        table3.SetWidths(colWidths3);


                        PdfPCell cell500p = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
                        cell500p.Colspan = 9;
                        cell500p.Border = 1;
                        cell500p.HorizontalAlignment = 1;
                        table3.AddCell(cell500p);

                        PdfPCell cell501p = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                        cell501p.Colspan = 5;
                        cell501p.Border = 0;
                        cell501p.HorizontalAlignment = 0;
                        table3.AddCell(cell501p);

                        PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                        cell502p.Colspan = 4;
                        cell502p.Border = 0;
                        cell502p.HorizontalAlignment = 2;
                        table3.AddCell(cell502p);

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
                        table3.AddCell(cell10);

                        PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                        table3.AddCell(cell11p);

                        i = 0;

                        doc.Add(table3);
                    }

                    PdfPTable table = new PdfPTable(9);
                    float[] colWidths = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table.SetWidths(colWidths);


                    transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                    num = dtt350.Rows[ii]["alloc_no"].ToString();
                    Session["num"] = num.ToString();
                    name = dtt350.Rows[ii]["swaminame"].ToString();
                    place = dtt350.Rows[ii]["place"].ToString();
                    states = dtt350.Rows[ii]["state_id"].ToString();
                    dist = dtt350.Rows[ii]["district_id"].ToString();
                    rec = dtt350.Rows[ii]["adv_recieptno"].ToString();

                    allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                    reason = dtt350.Rows[ii]["reason_id"].ToString();
                    alloctype = dtt350.Rows[ii]["alloc_type"].ToString();


                    #region extent remark&alter remark
                    if (allocfrom != "")
                    {
                        if (reason != "")
                        {

                            OdbcCommand cmdallocfr = new OdbcCommand();
                            cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                            cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                            cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                            DataTable dtallocfr = new DataTable();
                            dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
                            if (dtallocfr.Rows.Count > 0)
                            {
                                remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                            }


                        }
                        else
                        {
                            OdbcCommand cmdallocfr = new OdbcCommand();
                            cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                            cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                            cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                            DataTable dtallocfr = new DataTable();
                            dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

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
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                    else if (alloctype == "Donor Paid Allocation")
                    {
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                    else if (alloctype == "Donor multiple pass")
                    {
                        //

                        int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                        mpass = "";

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                        cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                        cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");
                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
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


                    build = "";
                    building = dtt350.Rows[ii]["buildingname"].ToString();
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

                    room = dtt350.Rows[ii]["roomno"].ToString();

                    Session["rec"] = rec.ToString();
                    Session["tno"] = transno.ToString();

                    indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                    ind = indat.ToString("dd-MMM");
                    it = indat.ToString("hh:mm:tt");
                    indate = it + "       " + ind;

                    if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                    {
                        outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                        outd = outdat.ToString("dd-MMM");
                        ot = outdat.ToString("hh:mm:tt");
                        outdate = ot + "       " + outd;
                    }
                    else
                    {
                        outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                        outd = outdat.ToString("dd-MMM");
                        ot = outdat.ToString("hh:mm:tt");
                        outdate = ot + "       " + outd;

                    }

                    rents = dtt350.Rows[ii]["roomrent"].ToString();
                    deposits = dtt350.Rows[ii]["deposit"].ToString();


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

                    if ((i == 27) || (ii == cont - 1))
                    {
                        PdfPTable table2 = new PdfPTable(9);
                        float[] colWidths2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                        table2.SetWidths(colWidths2);

                        PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                        cell41.Colspan = 6;
                        table2.AddCell(cell41);

                        PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                        table2.AddCell(cell49);

                        gtr = gtr + decimal.Parse(rr.ToString());
                        gtd = gtd + decimal.Parse(dde.ToString());

                        PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                        table2.AddCell(cell50);

                        PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        table2.AddCell(cell51);

                        doc.Add(table2);

                        rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                    }

                    if (ii == cont - 1)
                    {
                        PdfPTable table10 = new PdfPTable(9);
                        float[] colWidths10 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                        table10.SetWidths(colWidths10);


                        PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", fontLB)));
                        cell500p10.Colspan = 9;
                        cell500p10.Border = 0;
                        cell500p10.HorizontalAlignment = 1;
                        table10.AddCell(cell500p10);
                        /////////////////////
                        PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
                        cell500p12.Colspan = 2;
                        cell500p12.Border = 0;
                        cell500p12.HorizontalAlignment = 0;
                        table10.AddCell(cell500p12);

                        PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
                        cell500p13.Colspan = 3;
                        cell500p13.Border = 0;
                        cell500p13.HorizontalAlignment = 0;
                        table10.AddCell(cell500p13);

                        PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("Deposit : ", font10)));
                        cell500p15.Colspan = 2;
                        cell500p15.Border = 0;
                        cell500p15.HorizontalAlignment = 0;
                        table10.AddCell(cell500p15);


                        PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), font10)));
                        cell500p11.Colspan = 2;
                        cell500p11.Border = 1;
                        cell500p11.HorizontalAlignment = 1;
                        table10.AddCell(cell500p11);
                        ///////////////////
                        PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", font10)));
                        cell500p14.Colspan = 2;
                        cell500p14.Border = 1;
                        cell500p14.HorizontalAlignment = 1;
                        table10.AddCell(cell500p14);

                        //NumberToEnglish n = new NumberToEnglish();
                        string re = objcls.NumberToTextWithLakhs(Int64.Parse(gtr.ToString()));
                        re = re + " Only";
                        PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, font10)));
                        cell500p16.Colspan = 7;
                        cell500p16.Border = 1;
                        cell500p16.HorizontalAlignment = 1;
                        table10.AddCell(cell500p16);

                        PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("Deposit: ", font10)));
                        cell500p17.Colspan = 2;
                        cell500p17.Border = 1;
                        cell500p17.HorizontalAlignment = 1;
                        table10.AddCell(cell500p17);

                        string de = objcls.NumberToTextWithLakhs(Int64.Parse(gtd.ToString()));
                        de = de + " Only";
                        PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk(de, font10)));
                        cell500p18.Colspan = 7;
                        cell500p18.Border = 1;
                        cell500p18.HorizontalAlignment = 1;
                        table10.AddCell(cell500p18);
                        gtr = 0;
                        gtd = 0;

                        PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellfb1.PaddingLeft = 20;
                        cellfb1.Colspan = 9;
                        cellfb1.MinimumHeight = 30;
                        cellfb1.Border = 0;
                        table10.AddCell(cellfb1);


                        PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                        cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellfb.PaddingLeft = 20;
                        cellfb.Colspan = 9;
                        cellfb.MinimumHeight = 30;
                        cellfb.Border = 0;
                        table10.AddCell(cellfb);

                        PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                        cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellf1b.PaddingLeft = 20;
                        cellf1b.Colspan = 9;
                        cellf1b.Border = 0;

                        table10.AddCell(cellf1b);

                        PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                        cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        cellh2.PaddingLeft = 20;
                        cellh2.Border = 0;
                        cellh2.Colspan = 9;
                        table10.AddCell(cellh2);

                        doc.Add(table10);
                    }
                }

                doc.Close();

                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);

                try
                {
                    OdbcCommand cmd901 = new OdbcCommand();
                    cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                    cmd901.Parameters.AddWithValue("attribute", "max(slno)");

                    DataTable dtt901 = new DataTable();
                    dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);

                    id = int.Parse(dtt901.Rows[0][0].ToString());

                    int tno = int.Parse(Session["tno"].ToString());
                    string ct = Session["num"].ToString();
                    OdbcCommand cmd25 = new OdbcCommand();
                    cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                    cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                    cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                    int re1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
                }
                catch
                {
                    id = 1;
                    int tno = int.Parse(Session["tno"].ToString());
                    string ct = Session["num"].ToString();

                    OdbcCommand cmd589 = new OdbcCommand();
                    cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                    cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                    int saw1 = objcls.Procedures("CALL savedata(?,?)", cmd589);
                }
            }
            catch
            {

            }
        }
        #endregion

        #region not Half Print include - not full report

        if (ViewState["action"].ToString() == "Half Print not full report")// not Half Print include - not full report
        {
            string strsql1 = "m_room as room,"
         + "m_sub_building as build,"
         + "t_roomallocation as alloc"
         + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
         + " Left join m_sub_district as dist on alloc.district_id=dist.district_id"
         + " left join t_roomvacate vac on alloc.alloc_id=vac.alloc_id";

            string strsql2 = "alloc.alloc_id,"
                           + "alloc.alloc_no,"
                           + "alloc.place,"
                           + "alloc.pass_id,"
                           + "alloc.phone,"
                           + "alloc.adv_recieptno,"
                           + "alloc.idproof,"
                           + "alloc.idproofno,"
                           + "alloc.noofinmates,"
                           + "alloc.numberofunit,"
                           + "alloc.advance,"
                           + "alloc.reason,"
                           + "alloc.othercharge,"
                           + "alloc.adv_recieptno,"
                           + "alloc.swaminame,"
                           + "build.buildingname,"
                           + "room.roomno,"
                           + "alloc.allocdate,"
                           + "alloc.exp_vecatedate,"
                           + "alloc.roomrent,"
                           + "alloc.state_id,"
                           + "alloc.district_id,"
                           + "alloc.deposit,"
                           + "alloc.alloc_type,"
                           + "alloc.totalcharge,"
                           + "alloc.realloc_from,"
                           + "alloc.reason_id,"
                           + "actualvecdate";


            int s = int.Parse(Session["tno"].ToString());
            strsql3 = "alloc.room_id=room.room_id"
             + " and room.build_id=build.build_id"
             + " and alloc.dayend='" + Session["ledgerDate"].ToString() + "'"
             + " and alloc.alloc_id>" + s + " order by alloc.alloc_id asc";

            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", strsql1);
            cmd350.Parameters.AddWithValue("attribute", strsql2);
            cmd350.Parameters.AddWithValue("conditionv", strsql3);
            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);

            int cont = dtt350.Rows.Count;
            int hp = cont % 20;
            Session["cont"] = cont.ToString();
            Session["hp"] = hp.ToString();

            cont = int.Parse(Session["cont"].ToString());
            hp = int.Parse(Session["hp"].ToString());
            cont = cont - hp;

            DateTime reporttime = DateTime.Now;
            report = "Ledger Report" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            PdfPTable table1 = new PdfPTable(9);
            float[] colWidths1 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
            table1.SetWidths(colWidths1);


            string repdates = rdate.ToString("dd/MM/yyyy");
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
            string dateee = ss.ToString("dd-MMMM-yyyy");

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
            cell500.Colspan = 9;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 5;
            cell501.Border = 0;
            cell501.HorizontalAlignment = 0;
            table1.AddCell(cell501);

            PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
            cell502.Colspan = 4;
            cell502.Border = 0;
            cell502.HorizontalAlignment = 2;
            table1.AddCell(cell502);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell2);

            PdfPCell cell3rry = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
            table1.AddCell(cell3rry);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
            table1.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell5);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
            table1.AddCell(cell7);

            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
            table1.AddCell(cell8);

            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
            table1.AddCell(cell9);

            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
            table1.AddCell(cell10);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
            table1.AddCell(cell11);

            doc.Add(table1);

            int i = 0;

            for (int ii = 0; ii < cont; ii++)
            {
                if (i > 26)
                {
                    doc.NewPage();
                    PdfPTable table4 = new PdfPTable(9);
                    float[] colWidths4 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table4.SetWidths(colWidths4);


                    PdfPTable table3 = new PdfPTable(9);
                    float[] colWidths3 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table3.SetWidths(colWidths3);


                    PdfPCell cell500p = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
                    cell500p.Colspan = 9;
                    cell500p.Border = 1;
                    cell500p.HorizontalAlignment = 1;
                    table3.AddCell(cell500p);

                    PdfPCell cell501p = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                    cell501p.Colspan = 5;
                    cell501p.Border = 0;
                    cell501p.HorizontalAlignment = 0;
                    table3.AddCell(cell501p);

                    PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                    cell502p.Colspan = 4;
                    cell502p.Border = 0;
                    cell502p.HorizontalAlignment = 2;
                    table3.AddCell(cell502p);

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
                    table3.AddCell(cell10);

                    PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                    table3.AddCell(cell11p);

                    i = 0;

                    doc.Add(table3);
                }

                PdfPTable table = new PdfPTable(9);
                float[] colWidths = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                table.SetWidths(colWidths);

                transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                num = dtt350.Rows[ii]["alloc_no"].ToString();
                Session["num"] = num.ToString();
                name = dtt350.Rows[ii]["swaminame"].ToString();
                place = dtt350.Rows[ii]["place"].ToString();
                states = dtt350.Rows[ii]["state_id"].ToString();
                dist = dtt350.Rows[ii]["district_id"].ToString();
                rec = dtt350.Rows[ii]["adv_recieptno"].ToString();

                allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                reason = dtt350.Rows[ii]["reason_id"].ToString();
                alloctype = dtt350.Rows[ii]["alloc_type"].ToString();


                #region extent remark&alter remark
                if (allocfrom != "")
                {
                    if (reason != "")
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
                        if (dtallocfr.Rows.Count > 0)
                        {
                            remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }

                    }
                    else
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
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
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor Paid Allocation")
                {
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor multiple pass")
                {
                    int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                    mpass = "";

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                    cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                    cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
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


                build = "";
                building = dtt350.Rows[ii]["buildingname"].ToString();
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

                room = dtt350.Rows[ii]["roomno"].ToString();

                Session["rec"] = rec.ToString();
                Session["tno"] = transno.ToString();

                indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                ind = indat.ToString("dd-MMM");
                it = indat.ToString("hh:mm:tt");
                indate = it + "       " + ind;

                if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                {

                    outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;
                }
                else
                {

                    outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;
                }
                rents = dtt350.Rows[ii]["roomrent"].ToString();
                deposits = dtt350.Rows[ii]["deposit"].ToString();


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

                if ((i == 27) || (ii == cont - 1))
                {
                    PdfPTable table2 = new PdfPTable(9);
                    float[] colWidths2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table2.SetWidths(colWidths2);

                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                    cell41.Colspan = 6;
                    table2.AddCell(cell41);

                    PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                    table2.AddCell(cell49);

                    gtr = gtr + decimal.Parse(rr.ToString());
                    gtd = gtd + decimal.Parse(dde.ToString());

                    PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                    table2.AddCell(cell50);

                    PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table2.AddCell(cell51);

                    doc.Add(table2);

                    rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                }

                if (ii == cont - 1)
                {
                    PdfPTable table10 = new PdfPTable(9);
                    float[] colWidths10 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table10.SetWidths(colWidths10);


                    PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500p10.Colspan = 9;
                    cell500p10.Border = 0;
                    cell500p10.HorizontalAlignment = 1;
                    table10.AddCell(cell500p10);
                    /////////////////////
                    PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
                    cell500p12.Colspan = 2;
                    cell500p12.Border = 0;
                    cell500p12.HorizontalAlignment = 0;
                    table10.AddCell(cell500p12);

                    PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
                    cell500p13.Colspan = 3;
                    cell500p13.Border = 0;
                    cell500p13.HorizontalAlignment = 0;
                    table10.AddCell(cell500p13);

                    PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("Deposit : ", font10)));
                    cell500p15.Colspan = 2;
                    cell500p15.Border = 0;
                    cell500p15.HorizontalAlignment = 0;
                    table10.AddCell(cell500p15);


                    PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), font10)));
                    cell500p11.Colspan = 2;
                    cell500p11.Border = 1;
                    cell500p11.HorizontalAlignment = 1;
                    table10.AddCell(cell500p11);
                    ///////////////////
                    PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", font10)));
                    cell500p14.Colspan = 2;
                    cell500p14.Border = 1;
                    cell500p14.HorizontalAlignment = 1;
                    table10.AddCell(cell500p14);

                    // NumberToEnglish n = new NumberToEnglish();
                    string re = objcls.NumberToTextWithLakhs(Int64.Parse(gtr.ToString()));
                    re = re + " Only";
                    PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, font10)));
                    cell500p16.Colspan = 7;
                    cell500p16.Border = 1;
                    cell500p16.HorizontalAlignment = 1;
                    table10.AddCell(cell500p16);

                    PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("Deposit: ", font10)));
                    cell500p17.Colspan = 2;
                    cell500p17.Border = 1;
                    cell500p17.HorizontalAlignment = 1;
                    table10.AddCell(cell500p17);

                    string de = objcls.NumberToTextWithLakhs(Int64.Parse(gtd.ToString()));
                    de = de + " Only";
                    PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk(de, font10)));
                    cell500p18.Colspan = 7;
                    cell500p18.Border = 1;
                    cell500p18.HorizontalAlignment = 1;
                    table10.AddCell(cell500p18);
                    gtr = 0;
                    gtd = 0;

                    PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb1.PaddingLeft = 20;
                    cellfb1.Colspan = 9;
                    cellfb1.MinimumHeight = 30;
                    cellfb1.Border = 0;
                    table10.AddCell(cellfb1);


                    PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                    cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb.PaddingLeft = 20;
                    cellfb.Colspan = 9;
                    cellfb.MinimumHeight = 30;
                    cellfb.Border = 0;
                    table10.AddCell(cellfb);

                    PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                    cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellf1b.PaddingLeft = 20;
                    cellf1b.Colspan = 9;
                    cellf1b.Border = 0;

                    table10.AddCell(cellf1b);

                    PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                    cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cellh2.PaddingLeft = 20;
                    cellh2.Border = 0;
                    cellh2.Colspan = 9;
                    table10.AddCell(cellh2);

                    doc.Add(table10);
                }
            }

            doc.Close();

            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

            try
            {
                OdbcCommand cmd901 = new OdbcCommand("CALL selectdata(?,?)", con);
                cmd901.CommandType = CommandType.StoredProcedure;
                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                OdbcDataAdapter dacnt901 = new OdbcDataAdapter(cmd901);
                DataTable dtt901 = new DataTable();
                dacnt901.Fill(dtt901);
                id = int.Parse(dtt901.Rows[0][0].ToString());

                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd25 = new OdbcCommand();
                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                int eew1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
            }
            catch
            {
                id = 1;
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();

                OdbcCommand cmd589 = new OdbcCommand();
                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                int wq1 = objcls.Procedures("CALL savedata(?,?)", cmd589);
            }
        }
        #endregion

        #region nnnn
        #region half print
        if (ViewState["action"].ToString() == "Half Print")
        {
            string strsql1 = "m_room as room,"
            + "m_sub_building as build,"
            + "t_roomallocation as alloc"
            + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
            + " Left join m_sub_district as dist on alloc.district_id=dist.district_id"
            + " left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";

            string strsql2 = "alloc.alloc_id,"
                           + "alloc.alloc_no,"
                           + "alloc.place,"
                           + "alloc.phone,"
                           + "alloc.idproof,"
                           + "alloc.adv_recieptno,"
                           + "alloc.pass_id,"
                           + "alloc.idproofno,"
                           + "alloc.noofinmates,"
                           + "alloc.numberofunit,"
                           + "alloc.advance,"
                           + "alloc.reason,"
                           + "alloc.othercharge,"
                           + "alloc.adv_recieptno,"
                           + "alloc.swaminame,"
                           + "build.buildingname,"
                           + "room.roomno,"
                           + "alloc.allocdate,"
                           + "alloc.exp_vecatedate,"
                           + "alloc.roomrent,"
                           + "alloc.state_id,"
                           + "alloc.district_id,"
                           + "alloc.deposit,"
                           + "alloc.alloc_type,"
                           + "alloc.totalcharge,"
                           + "alloc.realloc_from,"
                           + "alloc.reason_id,"
                           + "actualvecdate";


            int s = int.Parse(Session["tno"].ToString());
            strsql3 = "alloc.room_id=room.room_id"
             + " and room.build_id=build.build_id"
             + " and alloc.dayend='" + Session["ledgerDate"].ToString() + "'"
             + " and alloc.alloc_id>" + s + " order by alloc.alloc_id asc";

            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", strsql1);
            cmd350.Parameters.AddWithValue("attribute", strsql2);
            cmd350.Parameters.AddWithValue("conditionv", strsql3);
            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);

            int cont = dtt350.Rows.Count;
            int hp = cont % 20;
            Session["cont"] = cont.ToString();
            Session["hp"] = hp.ToString();

            cont = int.Parse(Session["cont"].ToString());
            hp = int.Parse(Session["hp"].ToString());
            cont = cont - hp;

            DateTime reporttime = DateTime.Now;
            report = "Ledger Report" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 9, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            PdfPTable table1 = new PdfPTable(9);
            float[] colWidths1 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
            table1.SetWidths(colWidths1);

            string repdates = rdate.ToString("dd/MM/yyyy");
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
            string dateee = ss.ToString("dd-MMMM-yyyy");

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
            cell500.Colspan = 9;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 5;
            cell501.Border = 0;
            cell501.HorizontalAlignment = 0;
            table1.AddCell(cell501);

            PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
            cell502.Colspan = 4;
            cell502.Border = 0;
            cell502.HorizontalAlignment = 2;
            table1.AddCell(cell502);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell2);
            PdfPCell cell3fg = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
            table1.AddCell(cell3fg);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
            table1.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell5);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
            table1.AddCell(cell7);

            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
            table1.AddCell(cell8);

            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
            table1.AddCell(cell9);

            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
            table1.AddCell(cell10);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
            table1.AddCell(cell11);

            doc.Add(table1);

            int i = 0;

            for (int ii = 0; ii < cont; ii++)
            {
                if (i > 26)
                {
                    doc.NewPage();
                    PdfPTable table4 = new PdfPTable(9);
                    float[] colWidths4 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table4.SetWidths(colWidths4);


                    PdfPTable table3 = new PdfPTable(9);
                    float[] colWidths3 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table3.SetWidths(colWidths3);


                    PdfPCell cell500p = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
                    cell500p.Colspan = 9;
                    cell500p.Border = 1;
                    cell500p.HorizontalAlignment = 1;
                    table3.AddCell(cell500p);

                    PdfPCell cell501p = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                    cell501p.Colspan = 5;
                    cell501p.Border = 0;
                    cell501p.HorizontalAlignment = 0;
                    table3.AddCell(cell501p);

                    PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                    cell502p.Colspan = 4;
                    cell502p.Border = 0;
                    cell502p.HorizontalAlignment = 2;
                    table3.AddCell(cell502p);

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
                    table3.AddCell(cell10);

                    PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                    table3.AddCell(cell11p);

                    i = 0;

                    doc.Add(table3);
                }

                PdfPTable table = new PdfPTable(9);
                float[] colWidths = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                table.SetWidths(colWidths);

                transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                num = dtt350.Rows[ii]["alloc_no"].ToString();
                Session["num"] = num.ToString();
                name = dtt350.Rows[ii]["swaminame"].ToString();
                place = dtt350.Rows[ii]["place"].ToString();
                states = dtt350.Rows[ii]["state_id"].ToString();
                dist = dtt350.Rows[ii]["district_id"].ToString();
                rec = dtt350.Rows[ii]["adv_recieptno"].ToString();

                allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                reason = dtt350.Rows[ii]["reason_id"].ToString();
                alloctype = dtt350.Rows[ii]["alloc_type"].ToString();


                #region extent remark&alter remark
                if (allocfrom != "")
                {
                    if (reason != "")
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
                        if (dtallocfr.Rows.Count > 0)
                        {
                            remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }

                    }
                    else
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
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
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor Paid Allocation")
                {
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor multiple pass")
                {
                    //

                    int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                    mpass = "";

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                    cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                    cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
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


                build = "";
                building = dtt350.Rows[ii]["buildingname"].ToString();
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

                room = dtt350.Rows[ii]["roomno"].ToString();

                Session["rec"] = rec.ToString();
                Session["tno"] = transno.ToString();

                indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                ind = indat.ToString("dd-MMM");
                it = indat.ToString("hh:mm:tt");
                indate = it + "       " + ind;

                if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                {

                    outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;
                }
                else
                {

                    outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;

                }

                rents = dtt350.Rows[ii]["roomrent"].ToString();
                deposits = dtt350.Rows[ii]["deposit"].ToString();


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

                if ((i == 27) || (ii == cont - 1))
                {
                    PdfPTable table2 = new PdfPTable(9);
                    float[] colWidths2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table2.SetWidths(colWidths2);

                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                    cell41.Colspan = 6;
                    table2.AddCell(cell41);

                    PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                    table2.AddCell(cell49);

                    gtr = gtr + decimal.Parse(rr.ToString());
                    gtd = gtd + decimal.Parse(dde.ToString());

                    PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                    table2.AddCell(cell50);

                    PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table2.AddCell(cell51);

                    doc.Add(table2);

                    rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                }

                if (ii == cont - 1)
                {
                    PdfPTable table10 = new PdfPTable(9);
                    float[] colWidths10 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table10.SetWidths(colWidths10);

                    PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", fontLB)));
                    cell500p10.Colspan = 9;
                    cell500p10.Border = 0;
                    cell500p10.HorizontalAlignment = 1;
                    table10.AddCell(cell500p10);
                    /////////////////////
                    PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
                    cell500p12.Colspan = 2;
                    cell500p12.Border = 0;
                    cell500p12.HorizontalAlignment = 0;
                    table10.AddCell(cell500p12);

                    PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
                    cell500p13.Colspan = 3;
                    cell500p13.Border = 0;
                    cell500p13.HorizontalAlignment = 0;
                    table10.AddCell(cell500p13);

                    PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("Deposit : ", font10)));
                    cell500p15.Colspan = 2;
                    cell500p15.Border = 0;
                    cell500p15.HorizontalAlignment = 0;
                    table10.AddCell(cell500p15);


                    PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), font10)));
                    cell500p11.Colspan = 2;
                    cell500p11.Border = 1;
                    cell500p11.HorizontalAlignment = 1;
                    table10.AddCell(cell500p11);
                    ///////////////////
                    PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", font10)));
                    cell500p14.Colspan = 2;
                    cell500p14.Border = 1;
                    cell500p14.HorizontalAlignment = 1;
                    table10.AddCell(cell500p14);

                    // NumberToEnglish n = new NumberToEnglish();
                    string re = objcls.NumberToTextWithLakhs(Int64.Parse(gtr.ToString()));
                    re = re + " Only";
                    PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, font10)));
                    cell500p16.Colspan = 7;
                    cell500p16.Border = 1;
                    cell500p16.HorizontalAlignment = 1;
                    table10.AddCell(cell500p16);

                    PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("Deposit: ", font10)));
                    cell500p17.Colspan = 2;
                    cell500p17.Border = 1;
                    cell500p17.HorizontalAlignment = 1;
                    table10.AddCell(cell500p17);

                    string de = objcls.NumberToTextWithLakhs(Int64.Parse(gtd.ToString()));
                    de = de + " Only";
                    PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk(de, font10)));
                    cell500p18.Colspan = 7;
                    cell500p18.Border = 1;
                    cell500p18.HorizontalAlignment = 1;
                    table10.AddCell(cell500p18);
                    gtr = 0;
                    gtd = 0;

                    PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb1.PaddingLeft = 20;
                    cellfb1.Colspan = 9;
                    cellfb1.MinimumHeight = 30;
                    cellfb1.Border = 0;
                    table10.AddCell(cellfb1);


                    PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                    cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb.PaddingLeft = 20;
                    cellfb.Colspan = 9;
                    cellfb.MinimumHeight = 30;
                    cellfb.Border = 0;
                    table10.AddCell(cellfb);

                    PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                    cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellf1b.PaddingLeft = 20;
                    cellf1b.Colspan = 9;
                    cellf1b.Border = 0;

                    table10.AddCell(cellf1b);

                    PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                    cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cellh2.PaddingLeft = 20;
                    cellh2.Border = 0;
                    cellh2.Colspan = 9;
                    table10.AddCell(cellh2);

                    doc.Add(table10);
                }
            }
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            try
            {
                OdbcCommand cmd901 = new OdbcCommand();
                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                DataTable dtt901 = new DataTable();
                dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                id = int.Parse(dtt901.Rows[0][0].ToString());
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd25 = new OdbcCommand();
                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                int re1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
            }
            catch
            {
                id = 1;
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd589 = new OdbcCommand();
                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                int saq1 = objcls.Procedures("CALL savedata(?,?)", cmd589);
            }
        }
        #endregion

        #region full print
        if (ViewState["action"].ToString() == "Full Report")
        {
            string strsql1 = "m_room as room,"
             + "m_sub_building as build,"
             + "t_roomallocation as alloc"
             + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
             + " Left join m_sub_district as dist on alloc.district_id=dist.district_id"
             + " left join t_roomvacate vac on alloc.alloc_id=vac.alloc_id";
            string strsql2 = "alloc.alloc_id,"
                           + "alloc.alloc_no,"
                           + "alloc.place,"
                           + "alloc.adv_recieptno,"
                           + "alloc.pass_id,"
                           + "alloc.phone,"
                           + "alloc.idproof,"
                           + "alloc.idproofno,"
                           + "alloc.noofinmates,"
                           + "alloc.numberofunit,"
                           + "alloc.advance,"
                           + "alloc.reason,"
                           + "alloc.othercharge,"
                           + "alloc.adv_recieptno,"
                           + "alloc.swaminame,"
                           + "build.buildingname,"
                           + "room.roomno,"
                           + "alloc.allocdate,"
                           + "alloc.exp_vecatedate,"
                           + "alloc.roomrent,"
                           + "alloc.state_id,"
                           + "alloc.district_id,"
                           + "alloc.deposit,"
                           + "alloc.alloc_type,"
                           + "alloc.totalcharge,"
                           + "alloc.realloc_from,"
                           + "alloc.reason_id,"
                           + "actualvecdate";
            int s = int.Parse(Session["tno"].ToString());
            strsql3 = "alloc.room_id=room.room_id"
             + " and room.build_id=build.build_id"
             + " and alloc.dayend='" + Session["ledgerDate"].ToString() + "'"
             + " and alloc.alloc_id>" + s + " order by alloc.alloc_id asc";
            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", strsql1);
            cmd350.Parameters.AddWithValue("attribute", strsql2);
            cmd350.Parameters.AddWithValue("conditionv", strsql3);
            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
            int cont = dtt350.Rows.Count;
            int hp = cont % 20;
            Session["cont"] = cont.ToString();
            Session["hp"] = hp.ToString();
            Session["rep"] = "half";
            if (dtt350.Rows.Count == 0)
            {
                okmessage("Tsunami ARMS - Warning", "No details found");
                return;
            }
            if (hp < 18)
            {
                lblMsg.Text = "Including half Print?";
                ViewState["action"] = "Half Print not full report";
                pnlOk.Visible = false;
                pnlYesNo.Visible = true;
                ModalPopupExtender1.Show();
                this.ScriptManager1.SetFocus(btnYes);
            }
            else
            {
                DateTime reporttime = DateTime.Now;
                report = "Ledger Report" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
                string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;
                Font font8 = FontFactory.GetFont("ARIAL", 9);
                Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
                Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
                Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
                pdfPage page = new pdfPage();
                page.strRptMode = "Allocation";
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;

                doc.Open();

                PdfPTable table1 = new PdfPTable(9);
                float[] colWidths1 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                table1.SetWidths(colWidths1);

                string repdates = rdate.ToString("dd/MM/yyyy");
                string dt1 = dt.ToString("dd/MM/yyyy");

                DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
                string dateee = ss.ToString("dd-MMMM-yyyy");

                PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
                cell500.Colspan = 9;
                cell500.Border = 1;
                cell500.HorizontalAlignment = 1;
                table1.AddCell(cell500);

                PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                cell501.Colspan = 5;
                cell501.Border = 0;
                cell501.HorizontalAlignment = 0;
                table1.AddCell(cell501);

                PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                cell502.Colspan = 4;
                cell502.Border = 0;
                cell502.HorizontalAlignment = 2;
                table1.AddCell(cell502);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table1.AddCell(cell2);

                PdfPCell celld3 = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
                table1.AddCell(celld3);

                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
                table1.AddCell(cell3);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell5);

                PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
                table1.AddCell(cell7);

                PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
                table1.AddCell(cell8);

                PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
                table1.AddCell(cell9);

                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
                table1.AddCell(cell10);

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                table1.AddCell(cell11);

                doc.Add(table1);
                int i = 0;
                for (int ii = 0; ii < cont; ii++)
                {
                    if (i > 26)
                    {
                        doc.NewPage();
                        PdfPTable table4 = new PdfPTable(9);
                        float[] colWidths4 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                        table4.SetWidths(colWidths4);

                        PdfPTable table3 = new PdfPTable(9);
                        float[] colWidths3 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                        table3.SetWidths(colWidths3);

                        PdfPCell cell500p = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
                        cell500p.Colspan = 9;
                        cell500p.Border = 1;
                        cell500p.HorizontalAlignment = 1;
                        table3.AddCell(cell500p);

                        PdfPCell cell501p = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                        cell501p.Colspan = 5;
                        cell501p.Border = 0;
                        cell501p.HorizontalAlignment = 0;
                        table3.AddCell(cell501p);

                        PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                        cell502p.Colspan = 4;
                        cell502p.Border = 0;
                        cell502p.HorizontalAlignment = 2;
                        table3.AddCell(cell502p);

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
                        table3.AddCell(cell10);

                        PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                        table3.AddCell(cell11p);

                        i = 0;
                        doc.Add(table3);
                    }

                    PdfPTable table = new PdfPTable(9);
                    float[] colWidths = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table.SetWidths(colWidths);
                    transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                    num = dtt350.Rows[ii]["alloc_no"].ToString();
                    Session["num"] = num.ToString();
                    name = dtt350.Rows[ii]["swaminame"].ToString();
                    place = dtt350.Rows[ii]["place"].ToString();
                    states = dtt350.Rows[ii]["state_id"].ToString();
                    dist = dtt350.Rows[ii]["district_id"].ToString();
                    rec = dtt350.Rows[ii]["adv_recieptno"].ToString();
                    allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                    reason = dtt350.Rows[ii]["reason_id"].ToString();
                    alloctype = dtt350.Rows[ii]["alloc_type"].ToString();

                    #region extent remark&alter remark
                    if (allocfrom != "")
                    {
                        if (reason != "")
                        {
                            OdbcCommand cmdallocfr = new OdbcCommand();
                            cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                            cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                            cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                            DataTable dtallocfr = new DataTable();
                            dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
                            if (dtallocfr.Rows.Count > 0)
                            {
                                remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                            }
                        }
                        else
                        {
                            OdbcCommand cmdallocfr = new OdbcCommand();
                            cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                            cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                            cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                            DataTable dtallocfr = new DataTable();
                            dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
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
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());
                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                    else if (alloctype == "Donor Paid Allocation")
                    {
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());
                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                    else if (alloctype == "Donor multiple pass")
                    {
                        int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                        mpass = "";
                        OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                        cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                        cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");
                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
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

                    build = "";
                    building = dtt350.Rows[ii]["buildingname"].ToString();
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
                    room = dtt350.Rows[ii]["roomno"].ToString();
                    Session["rec"] = rec.ToString();
                    Session["tno"] = transno.ToString();
                    indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                    ind = indat.ToString("dd-MMM");
                    it = indat.ToString("hh:mm:tt");
                    indate = it + "       " + ind;
                    if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                    {
                        outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                        outd = outdat.ToString("dd-MMM");
                        ot = outdat.ToString("hh:mm:tt");
                        outdate = ot + "       " + outd;
                    }
                    else
                    {
                        outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                        outd = outdat.ToString("dd-MMM");
                        ot = outdat.ToString("hh:mm:tt");
                        outdate = ot + "       " + outd;
                    }
                    rents = dtt350.Rows[ii]["roomrent"].ToString();
                    deposits = dtt350.Rows[ii]["deposit"].ToString();
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

                    if ((i == 27) || (ii == cont - 1))
                    {
                        PdfPTable table2 = new PdfPTable(9);
                        float[] colWidths2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                        table2.SetWidths(colWidths2);

                        PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                        cell41.Colspan = 6;
                        table2.AddCell(cell41);

                        PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                        table2.AddCell(cell49);

                        gtr = gtr + decimal.Parse(rr.ToString());
                        gtd = gtd + decimal.Parse(dde.ToString());

                        PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                        table2.AddCell(cell50);

                        PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        table2.AddCell(cell51);

                        doc.Add(table2);

                        rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                    }
                    if (ii == cont - 1)
                    {
                        PdfPTable table10 = new PdfPTable(9);
                        float[] colWidths10 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                        table10.SetWidths(colWidths10);

                        PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell500p10.Colspan = 9;
                        cell500p10.Border = 0;
                        cell500p10.HorizontalAlignment = 1;
                        table10.AddCell(cell500p10);
                        /////////////////////
                        PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
                        cell500p12.Colspan = 2;
                        cell500p12.Border = 0;
                        cell500p12.HorizontalAlignment = 0;
                        table10.AddCell(cell500p12);

                        PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
                        cell500p13.Colspan = 3;
                        cell500p13.Border = 0;
                        cell500p13.HorizontalAlignment = 0;
                        table10.AddCell(cell500p13);

                        PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("Deposit : ", font10)));
                        cell500p15.Colspan = 2;
                        cell500p15.Border = 0;
                        cell500p15.HorizontalAlignment = 0;
                        table10.AddCell(cell500p15);

                        PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), font10)));
                        cell500p11.Colspan = 2;
                        cell500p11.Border = 1;
                        cell500p11.HorizontalAlignment = 1;
                        table10.AddCell(cell500p11);
                        ///////////////////
                        PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", font10)));
                        cell500p14.Colspan = 2;
                        cell500p14.Border = 1;
                        cell500p14.HorizontalAlignment = 1;
                        table10.AddCell(cell500p14);

                        string re = objcls.NumberToTextWithLakhs(Int64.Parse(gtr.ToString()));
                        re = re + " Only";
                        PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, font10)));
                        cell500p16.Colspan = 7;
                        cell500p16.Border = 1;
                        cell500p16.HorizontalAlignment = 1;
                        table10.AddCell(cell500p16);

                        PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("Deposit: ", font10)));
                        cell500p17.Colspan = 2;
                        cell500p17.Border = 1;
                        cell500p17.HorizontalAlignment = 1;
                        table10.AddCell(cell500p17);

                        string de = objcls.NumberToTextWithLakhs(Int64.Parse(gtd.ToString()));
                        de = de + " Only";
                        PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk(de, font10)));
                        cell500p18.Colspan = 7;
                        cell500p18.Border = 1;
                        cell500p18.HorizontalAlignment = 1;
                        table10.AddCell(cell500p18);
                        gtr = 0;
                        gtd = 0;

                        PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellfb1.PaddingLeft = 20;
                        cellfb1.Colspan = 9;
                        cellfb1.MinimumHeight = 30;
                        cellfb1.Border = 0;
                        table10.AddCell(cellfb1);

                        PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                        cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellfb.PaddingLeft = 20;
                        cellfb.Colspan = 9;
                        cellfb.MinimumHeight = 30;
                        cellfb.Border = 0;
                        table10.AddCell(cellfb);

                        PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                        cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellf1b.PaddingLeft = 20;
                        cellf1b.Colspan = 9;
                        cellf1b.Border = 0;

                        table10.AddCell(cellf1b);

                        PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                        cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        cellh2.PaddingLeft = 20;
                        cellh2.Border = 0;
                        cellh2.Colspan = 9;
                        table10.AddCell(cellh2);

                        doc.Add(table10);
                    }
                }

                doc.Close();
                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);
                try
                {
                    OdbcCommand cmd901 = new OdbcCommand();
                    cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                    cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                    DataTable dtt901 = new DataTable();
                    dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                    id = int.Parse(dtt901.Rows[0][0].ToString());

                    int tno = int.Parse(Session["tno"].ToString());
                    string ct = Session["num"].ToString();
                    OdbcCommand cmd25 = new OdbcCommand();
                    cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                    cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                    cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                    int er1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
                }
                catch
                {
                    id = 1;
                    int tno = int.Parse(Session["tno"].ToString());
                    string ct = Session["num"].ToString();
                    OdbcCommand cmd589 = new OdbcCommand();
                    cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                    cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                    int req1 = objcls.Procedures("CALL savedata(?,?)", cmd589);
                }
            }

        }
        #endregion

        #endregion
    }
    #endregion

    #region Excel Function
    public void GetExcel(DataTable dt, string Heading)
    {
        DataTable myReader = new DataTable();
        myReader = dt;
        DateTime dth = DateTime.Now;
        string S_head = Heading + dth.ToString("dd-MM-yyyy hh:mm:ss");
        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        string sep = "";
        string MH = "TRAVANCORE DEVASWOM BOARD";
        Response.Write("\t\t\t" + MH);
        Response.Write("\n\n");
        Response.Write("\t\t\t" + S_head);
        Response.Write("\n\n");
        foreach (DataColumn c in myReader.Columns)
        {
            string hd = c.ColumnName.ToUpper();

            Response.Write(sep + hd);
            sep = "\t";
        }
        Response.Write("\n");
        int i;
        Response.Write("\n");
        foreach (DataRow dr in myReader.Rows)
        {
            sep = "";
            for (i = 0; i < myReader.Columns.Count; i++)
            {
                Response.Write(sep + dr[i].ToString());
                sep = "\t";
            }
            Response.Write("\n");
        }
        Response.End();
    }
    #endregion

    protected void lnktotalallocseasonreport_Click(object sender, EventArgs e)
    {

        #region ledger from to
        try
        {
            miss = 0;
            if ((txtfromd.Text == "") || (txttod.Text == ""))
            {
                okmessage("Tsunami ARMS - Warning", "Enter dates");
                return;
            }

            string str1 = objcls.yearmonthdate(txtfromd.Text.ToString());

            string str2 = objcls.yearmonthdate(txttod.Text.ToString());

            DateTime ind = DateTime.Parse(str1);
            DateTime outd = DateTime.Parse(str2);
            if (outd < ind)
            {
                okmessage("Tsunami ARMS - Warning", "Check the dates");
                return;
            }

            DateTime rdate = DateTime.Now;
            string repdate = rdate.ToString("yyyy/MM/dd");
            string reptime = rdate.ToShortTimeString();

            int no = 0, i = 0;
            int currentyear = rdate.Year;


            string datf = objcls.yearmonthdate(txtfromd.Text);
            string datt = objcls.yearmonthdate(txttod.Text);

            DateTime daf1 = DateTime.Parse(datf.ToString());
            DateTime dat1 = DateTime.Parse(datt.ToString());

            string g1 = daf1.ToString("yyyy-MM-dd");
            string g2 = dat1.ToString("yyyy-MM-dd");


            string g3 = daf1.ToString("dd/MM/yy");
            string g4 = dat1.ToString("dd/MM/yy");
            string strsql1 = "m_room as room,"
                    + "m_sub_building as build,"
                    + "t_roomallocation as alloc"
                    + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                    + " Left join m_sub_district as dist on alloc.district_id=dist.district_id  left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";

            string strsql2 = "alloc.alloc_id,"
                           + "alloc.alloc_no,"
                           + "alloc.pass_id,"
                           + "alloc.place,"
                           + "alloc.phone,"
                           + "alloc.idproof,"
                           + "alloc.alloc_type,"
                           + "alloc.idproofno,"
                           + "alloc.noofinmates,"
                           + "alloc.numberofunit,"
                           + "alloc.advance,"
                           + "alloc.reason,"
                           + "alloc.othercharge,"
                           + "alloc.adv_recieptno,"
                           + "alloc.swaminame,"
                           + "build.buildingname,"
                           + "room.roomno,"
                           + "alloc.allocdate,"
                           + "alloc.exp_vecatedate,"
                           + "alloc.roomrent,"
                           + "alloc.state_id,"
                           + "alloc.district_id,"
                           + "alloc.deposit,"
                           + "alloc.totalcharge,"
                           + "alloc.realloc_from,"
                           + "alloc.reason_id,"
                           + "actualvecdate,"
                           + "alloc.is_plainprint,"
                           + "alloc.counter_id";

            strsql3 = "alloc.room_id=room.room_id"
              + " and room.build_id=build.build_id"
              + " and alloc.dayend >= '" + datf + "' and alloc.dayend <= '" + datt + "' order by alloc.alloc_id asc";


            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", strsql1);
            cmd350.Parameters.AddWithValue("attribute", strsql2);
            cmd350.Parameters.AddWithValue("conditionv", strsql3);
            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);

            int cont = dtt350.Rows.Count;
            if (dtt350.Rows.Count == 0)
            {
                okmessage("Tsunami ARMS - Warning", "No details found");
                return;
            }

            firstrec = int.Parse(dtt350.Rows[0]["adv_recieptno"].ToString());
            lastrec = int.Parse(dtt350.Rows[cont - 1]["adv_recieptno"].ToString());
            int totrec = lastrec - firstrec + 1;

            DateTime reporttime = DateTime.Now;
            report = "Ledger From-To " + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;

            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font80 = FontFactory.GetFont("ARIAL", 7);
            Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);

            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();
            PdfPTable table1 = new PdfPTable(9);
            float[] colWidths1 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
            table1.SetWidths(colWidths1);

            string repdates = rdate.ToString("dd/MM/yyyy");

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
            cell500.Colspan = 9;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 5;
            cell501.Border = 0;
            cell501.HorizontalAlignment = 0;
            table1.AddCell(cell501);

            if (txtfromd.Text.ToString() == txttod.Text.ToString())
            {
                PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + g3.ToString(), font10)));
                cell502.Colspan = 4;
                cell502.Border = 0;
                cell502.HorizontalAlignment = 2;
                table1.AddCell(cell502);
            }
            else
            {
                PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + g3 + "-" + g4, font10)));
                cell502.Colspan = 4;
                cell502.Border = 0;
                cell502.HorizontalAlignment = 2;
                table1.AddCell(cell502);
            }


            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell2);

            PdfPCell cell2d = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
            table1.AddCell(cell2d);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
            table1.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell5);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
            table1.AddCell(cell7);

            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
            table1.AddCell(cell8);

            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
            table1.AddCell(cell9);

            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
            table1.AddCell(cell10);

            PdfPCell cell11e = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
            table1.AddCell(cell11e);


            doc.Add(table1);
            i = 0;

            slno = 1;
            for (int ii = 0; ii < cont; ii++)
            {
                if (i > 26)
                {
                    doc.NewPage();

                    PdfPTable table2 = new PdfPTable(9);
                    float[] colWidths2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table2.SetWidths(colWidths2);

                    PdfPCell cell500d = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
                    cell500d.Colspan = 9;
                    cell500d.Border = 1;
                    cell500d.HorizontalAlignment = 1;
                    table2.AddCell(cell500d);

                    PdfPCell cell501d = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                    cell501d.Colspan = 5;
                    cell501d.Border = 0;
                    cell501d.HorizontalAlignment = 0;
                    table2.AddCell(cell501d);

                    if (txtfromd.Text.ToString() == txttod.Text.ToString())
                    {
                        PdfPCell cell502d = new PdfPCell(new Phrase(new Chunk("Date: " + g3.ToString(), font10)));
                        cell502d.Colspan = 4;
                        cell502d.Border = 0;
                        cell502d.HorizontalAlignment = 2;
                        table2.AddCell(cell502d);
                    }
                    else
                    {
                        PdfPCell cell502e = new PdfPCell(new Phrase(new Chunk("Date: " + g3 + "-" + g4, font10)));
                        cell502e.Colspan = 4;
                        cell502e.Border = 0;
                        cell502e.HorizontalAlignment = 2;
                        table2.AddCell(cell502e);
                    }

                    PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table2.AddCell(cell22);

                    PdfPCell cell2df = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
                    table1.AddCell(cell2df);

                    PdfPCell cell32 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
                    table2.AddCell(cell32);

                    PdfPCell cell52 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table2.AddCell(cell52);

                    PdfPCell cell72 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
                    table2.AddCell(cell72);

                    PdfPCell cell82 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
                    table2.AddCell(cell82);

                    PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
                    table2.AddCell(cell92);

                    PdfPCell cell102 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
                    table2.AddCell(cell102);

                    PdfPCell cell112e = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                    table1.AddCell(cell112e);
                    i = 0;
                    doc.Add(table2);
                }

                PdfPTable table = new PdfPTable(9);
                float[] colWidths = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                table.SetWidths(colWidths);

                #region Receipt no correction1

                if (pprt == null)
                {
                    pprt = "";
                }
                if (rec == null)
                {
                    rec = "99999999";
                }
                int no1 = int.Parse(rec);
                if (countr == null)
                {
                    countr = "0";
                }
                string pprt1 = pprt;
                pprt = dtt350.Rows[ii]["is_plainprint"].ToString();
                int ctr1 = int.Parse(countr);
                countr = dtt350.Rows[ii]["counter_id"].ToString();
                #endregion

                rec = dtt350.Rows[ii]["adv_recieptno"].ToString();
                transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                num = dtt350.Rows[ii]["alloc_no"].ToString();
                Session["num"] = num.ToString();
                name = dtt350.Rows[ii]["swaminame"].ToString();
                place = dtt350.Rows[ii]["place"].ToString();
                states = dtt350.Rows[ii]["state_id"].ToString();
                dist = dtt350.Rows[ii]["district_id"].ToString();
                allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                reason = dtt350.Rows[ii]["reason_id"].ToString();
                alloctype = dtt350.Rows[ii]["alloc_type"].ToString();

                #region Receipt no correction2

                string pprt2 = pprt;
                int no2 = int.Parse(rec);
                int ctr2 = int.Parse(countr);
                int diff = no2 - no1;

                if (diff > 1)
                {
                    for (int i1 = no1 + 1; i1 < no2; i1++)
                    {
                        if (pprt1 == pprt2)
                        {
                            if (ctr1 == ctr2)
                            {
                                //string saq1 = "Select count(*) from t_roomallocation where adv_recieptno='" + i1 + "'";

                                OdbcCommand strSql7 = new OdbcCommand();
                                strSql7.Parameters.AddWithValue("tblname", "t_roomallocation");
                                strSql7.Parameters.AddWithValue("attribute", "count(*)");
                                strSql7.Parameters.AddWithValue("conditionv", "adv_recieptno='" + i1 + "'");
                                OdbcDataReader dr99 = objcls.SpGetReader("call selectcond(?,?,?)", strSql7);
                                while (dr99.Read())
                                {
                                    if (int.Parse(dr99["count(*)"].ToString()) < 1)
                                    {
                                        PdfPCell cell21775 = new PdfPCell(new Phrase(new Chunk("", font8)));
                                        cell21775.Colspan = 1;
                                        table.AddCell(cell21775);
                                        PdfPCell cell21776 = new PdfPCell(new Phrase(new Chunk(i1.ToString(), font8)));
                                        cell21776.Colspan = 1;
                                        table.AddCell(cell21776);
                                        PdfPCell cell21771 = new PdfPCell(new Phrase(new Chunk("- - - -  Receipt  Damaged / Cancelled  - - - -", font80)));
                                        cell21771.HorizontalAlignment = 1;
                                        cell21771.Colspan = 7;
                                        table.AddCell(cell21771);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion



                #region extent remark&alter remark
                if (allocfrom != "")
                {
                    if (reason != "")
                    {

                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                        if (dtallocfr.Rows.Count > 0)
                        {
                            remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }


                    }
                    else
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

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
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor Paid Allocation")
                {
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor multiple pass")
                {

                    int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                    mpass = "";

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                    cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                    cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

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


                build = "";
                building = dtt350.Rows[ii]["buildingname"].ToString();
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

                room = dtt350.Rows[ii]["roomno"].ToString();

                Session["rec"] = rec.ToString();
                Session["tno"] = transno.ToString();

                indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                string inds = indat.ToString("dd-MMM");
                it = indat.ToString("hh:mm:tt");
                indate = it + "       " + inds;

                if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                {
                    outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                    string outds = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outds;
                }
                else
                {
                    string cc = Convert.ToString(dtt350.Rows[ii]["actualvecdate"]);
                    outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                    string outds = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outds;
                }

                rents = dtt350.Rows[ii]["roomrent"].ToString();
                deposits = dtt350.Rows[ii]["deposit"].ToString();


                rrent1 = decimal.Parse(rents.ToString());
                rrent = rrent + rrent1;

                rr = rrent.ToString();
                rdeposit1 = decimal.Parse(deposits.ToString());
                rdeposit = rdeposit + rdeposit1;

                dde = rdeposit.ToString();

                rrr = dtt350.Rows[ii]["adv_recieptno"].ToString();


                number = slno.ToString();

                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(number, font8)));
                table.AddCell(cell21);

                PdfPCell cell21j = new PdfPCell(new Phrase(new Chunk(rrr, font8)));
                table.AddCell(cell21j);

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
                slno = slno + 1;

                if ((i == 27) || (ii == cont - 1))
                {
                    PdfPTable table2 = new PdfPTable(9);
                    float[] colWidths2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table2.SetWidths(colWidths2);

                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                    cell41.Colspan = 6;
                    table2.AddCell(cell41);

                    PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                    table2.AddCell(cell49);

                    gtr = gtr + decimal.Parse(rr.ToString());
                    gtd = gtd + decimal.Parse(dde.ToString());

                    PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk("", font9)));
                    table2.AddCell(cell50);

                    PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table2.AddCell(cell51);

                    doc.Add(table2);

                    rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                }

                if (ii == cont - 1)
                {
                    PdfPTable table10 = new PdfPTable(9);
                    float[] colWidths10 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table10.SetWidths(colWidths10);



                    PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", fontLB)));
                    cell500p10.Colspan = 9;
                    cell500p10.Border = 0;
                    cell500p10.HorizontalAlignment = 1;
                    table10.AddCell(cell500p10);

                    PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
                    cell500p12.Colspan = 3;
                    cell500p12.Border = 0;
                    cell500p12.HorizontalAlignment = 0;
                    table10.AddCell(cell500p12);

                    PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
                    cell500p13.Colspan = 2;
                    cell500p13.Border = 0;
                    cell500p13.HorizontalAlignment = 0;
                    table10.AddCell(cell500p13);

                    PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500p15.Colspan = 2;
                    cell500p15.Border = 0;
                    cell500p15.HorizontalAlignment = 0;
                    table10.AddCell(cell500p15);

                    PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500p11.Colspan = 2;
                    cell500p11.Border = 1;
                    cell500p11.HorizontalAlignment = 1;
                    table10.AddCell(cell500p11);

                    PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", font10)));
                    cell500p14.Colspan = 2;
                    cell500p14.Border = 1;
                    cell500p14.HorizontalAlignment = 1;
                    table10.AddCell(cell500p14);

                    string re = objcls.NumberToTextWithLakhs(Int64.Parse(gtr.ToString()));
                    re = re + " Only";
                    PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, font10)));
                    cell500p16.Colspan = 7;
                    cell500p16.Border = 1;
                    cell500p16.HorizontalAlignment = 1;
                    table10.AddCell(cell500p16);

                    PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500p17.Colspan = 2;
                    cell500p17.Border = 1;
                    cell500p17.HorizontalAlignment = 1;
                    table10.AddCell(cell500p17);

                    string de = objcls.NumberToTextWithLakhs(Int64.Parse(gtd.ToString()));
                    de = de + " Only";
                    PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500p18.Colspan = 7;
                    cell500p18.Border = 1;
                    cell500p18.HorizontalAlignment = 1;
                    table10.AddCell(cell500p18);

                    gtr = 0;
                    gtd = 0;

                    PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb1.PaddingLeft = 20;
                    cellfb1.Colspan = 9;
                    cellfb1.MinimumHeight = 30;
                    cellfb1.Border = 0;
                    table10.AddCell(cellfb1);


                    PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                    cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb.PaddingLeft = 20;
                    cellfb.Colspan = 9;
                    cellfb.MinimumHeight = 30;
                    cellfb.Border = 0;
                    table10.AddCell(cellfb);

                    PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                    cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellf1b.PaddingLeft = 20;
                    cellf1b.Colspan = 9;
                    cellf1b.Border = 0;

                    table10.AddCell(cellf1b);

                    PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                    cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cellh2.PaddingLeft = 20;
                    cellh2.Border = 0;
                    cellh2.Colspan = 9;
                    table10.AddCell(cellh2);
                    /////////////////////////

                    doc.Add(table10);

                }
            }

            doc.Close();

            Random r = new Random();

            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report From-To";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in taking report");
        }
        #endregion

    }
              
    #region ledger report current date

    protected void lnkDonorPaidRoomAllocationReport_Click(object sender, EventArgs e)
    {
        #region new ledger
        DateTime rdate = DateTime.Now;
        string repdate = rdate.ToString("yyyy/MM/dd");
        string reptime = rdate.ToShortTimeString();
        string counter = cmbcounter.SelectedItem.ToString();
        string frm = " ", cond = " ";
        if (counter != "All")
        {
            frm = " INNER JOIN m_sub_counter ON alloc.counter_id = m_sub_counter.counter_id ";
            cond = " AND m_sub_counter.counter_ip = '" + cmbcounter.SelectedItem.ToString() + "' ";
        }
        try
        {
            if (txtdate.Text == "")
            {
                okmessage("Tsunami ARMS - Message", "Please Enter date");
                return;
            }
            string dt3 = objcls.yearmonthdate(txtdate.Text);
            Session["ledgerDate"] = dt3.ToString();
            OdbcCommand cmd550 = new OdbcCommand();
            cmd550.Parameters.AddWithValue("tblname", "t_printledger");
            cmd550.Parameters.AddWithValue("attribute", "*");
            cmd550.Parameters.AddWithValue("conditionv", "date='" + dt3 + "' and slno=" + 1 + "");
            DataTable dtt550 = new DataTable();
            dtt550 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd550);
            if (dtt550.Rows.Count > 0)
            {
                Session["tno"] = dtt550.Rows[0]["alloc_id"].ToString();
                Session["num"] = dtt550.Rows[0]["printed_no"].ToString();
                // lblMsg.Text = "Including half Print?";
                lblMsg.Text = "Want to take full report on the day?";
                ViewState["action"] = "Full Report";
                pnlOk.Visible = false;
                pnlYesNo.Visible = true;
                ModalPopupExtender1.Show();
                this.ScriptManager1.SetFocus(btnYes);
            }
            else
            {
                Session["rep"] = "full";
                Session["num"] = 1;
                Session["tno"] = null;
                string strsql1 = "m_room as room,"
                       + "m_sub_building as build,"
                       + "t_roomallocation as alloc"
                       + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                       + " Left join m_sub_district as dist on alloc.district_id=dist.district_id left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id " + frm + "";
                string strsql2 = "alloc.alloc_id,"
                               + "alloc.alloc_no,"
                               + "alloc.place,"
                               + "alloc.pass_id,"
                               + "alloc.phone,"
                               + "alloc.idproof,"
                               + "alloc.idproofno,"
                               + "alloc.noofinmates,"
                               + "alloc.numberofunit,"
                               + "alloc.advance,"
                               + "alloc.reason,"
                               + "alloc.othercharge,"
                               + "alloc.adv_recieptno,"
                               + "alloc.swaminame,"
                               + "build.buildingname,"
                               + "room.roomno,"
                               + "alloc.allocdate,"
                               + "alloc.exp_vecatedate,"
                               + "alloc.roomrent,"
                               + "alloc.state_id,"
                               + "alloc.district_id,"
                               + "alloc.deposit,"
                               + "alloc.alloc_type,"
                               + "alloc.totalcharge,"
                               + "alloc.realloc_from,"
                               + "alloc.reason_id,"
                               + "actualvecdate";
                strsql3 = "alloc.room_id=room.room_id"
                  + " and room.build_id=build.build_id"
                  + " and alloc.dayend='" + dt3 + "' " + cond + " order by alloc.alloc_id asc";
                OdbcCommand cmd350 = new OdbcCommand();
                cmd350.Parameters.AddWithValue("tblname", strsql1);
                cmd350.Parameters.AddWithValue("attribute", strsql2);
                cmd350.Parameters.AddWithValue("conditionv", strsql3);
                DataTable dtt350 = new DataTable();
                dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
                if (dtt350.Rows.Count == 0)
                {
                    okmessage("Tsunami ARMS - Warning", "No details found");
                    return;
                }
                int cont = dtt350.Rows.Count;
                int hp = cont % 20;
                Session["cont"] = cont.ToString();
                Session["hp"] = hp.ToString();
                if (hp < 18)
                {
                    lblMsg.Text = "Including half Print?";
                    ViewState["action"] = "Half Print include  on full report";
                    pnlOk.Visible = false;
                    pnlYesNo.Visible = true;
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnYes);
                }
                else
                {
                    DateTime reporttime = DateTime.Now;
                    report = "Ledger Report" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
                    string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;

                    Font font8 = FontFactory.GetFont("ARIAL", 9);
                    Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
                    Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
                    Font font10 = FontFactory.GetFont("Rupee Foradian", 10, 1);
                    pdfPage page = new pdfPage();
                    page.strRptMode = "Allocation";
                    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                    wr.PageEvent = page;
                    doc.Open();

                    PdfPTable table1 = new PdfPTable(10);
                    float[] colWidths1 = { 40, 65, 160, 75, 85, 85, 60, 60, 50, 70};
                    table1.SetWidths(colWidths1);

                    string repdates = rdate.ToString("dd/MM/yyyy");
                    string dt1 = dt.ToString("dd/MM/yyyy");

                    DateTime ss = DateTime.Parse(dt3.ToString());
                    string dateee = ss.ToString("dd-MMMM-yyyy");

                    PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "", fontLB)));
                    cell500.Colspan = 9;
                    cell500.Border = 1;
                    cell500.HorizontalAlignment = 1;
                    table1.AddCell(cell500);

                    PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                    cell501.Colspan = 5;
                    cell501.Border = 0;
                    cell501.HorizontalAlignment = 0;
                    table1.AddCell(cell501);

                    PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                    cell502.Colspan = 4;
                    cell502.Border = 0;
                    cell502.HorizontalAlignment = 2;
                    table1.AddCell(cell502);

                    PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table1.AddCell(cell2);

                    PdfPCell cell2fg = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
                    table1.AddCell(cell2fg);

                    PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
                    table1.AddCell(cell3);

                    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table1.AddCell(cell5);

                    PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
                    table1.AddCell(cell7);

                    PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
                    table1.AddCell(cell8);

                    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
                    table1.AddCell(cell9);

                    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
                    table1.AddCell(cell10);

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                    table1.AddCell(cell11);

                    doc.Add(table1);
                    int i = 0;
                    for (int ii = 0; ii < cont; ii++)
                    {
                        if (i > 26)
                        {
                            doc.NewPage();
                            PdfPTable table4 = new PdfPTable(10);
                            float[] colWidths4 = { 40, 65, 160, 75, 85, 85, 60, 60, 50, 70};
                            table4.SetWidths(colWidths4);

                            PdfPTable table3 = new PdfPTable(9);
                            float[] colWidths3 = { 40, 65, 130, 75, 85, 85, 60, 60, 50, 70};
                            table3.SetWidths(colWidths3);

                            PdfPCell cell500p = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "", fontLB)));
                            cell500p.Colspan = 9;
                            cell500p.Border = 1;
                            cell500p.HorizontalAlignment = 1;
                            table3.AddCell(cell500p);

                            PdfPCell cell501p = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                            cell501p.Colspan = 5;
                            cell501p.Border = 0;
                            cell501p.HorizontalAlignment = 0;
                            table3.AddCell(cell501p);

                            PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + font10, fontLB)));
                            cell502p.Colspan = 4;
                            cell502p.Border = 0;
                            cell502p.HorizontalAlignment = 2;
                            table3.AddCell(cell502p);

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
                            table3.AddCell(cell10);

                            PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                            table3.AddCell(cell11p);

                            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("ID Proof Type & NO:", font9)));
                            table3.AddCell(cell12);

                            i = 0;
                            doc.Add(table3);
                        }

                        PdfPTable table = new PdfPTable(10);
                        float[] colWidths = { 40, 65, 130, 75, 85, 85, 60, 60, 50, 70};
                        table.SetWidths(colWidths);

                        transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                        num = dtt350.Rows[ii]["alloc_no"].ToString();
                        Session["num"] = num.ToString();
                        name = dtt350.Rows[ii]["swaminame"].ToString();
                        place = dtt350.Rows[ii]["place"].ToString();
                        states = dtt350.Rows[ii]["state_id"].ToString();
                        dist = dtt350.Rows[ii]["district_id"].ToString();
                        rec = dtt350.Rows[ii]["adv_recieptno"].ToString();
                        allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                        reason = dtt350.Rows[ii]["reason_id"].ToString();
                        alloctype = dtt350.Rows[ii]["alloc_type"].ToString();

                        #region extent remark&alter remark
                        if (allocfrom != "")
                        {
                            if (reason != "")
                            {

                                OdbcCommand cmdallocfr = new OdbcCommand();
                                cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                                cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                                cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                                DataTable dtallocfr = new DataTable();
                                dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                                if (dtallocfr.Rows.Count > 0)
                                {
                                    remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                                }


                            }
                            else
                            {
                                OdbcCommand cmdallocfr = new OdbcCommand();
                                cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                                cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                                cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                                DataTable dtallocfr = new DataTable();
                                dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

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
                            int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                            OdbcCommand cmd115 = new OdbcCommand();
                            cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                            cmd115.Parameters.AddWithValue("attribute", "passno");
                            cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                            DataTable dtt115 = new DataTable();
                            dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                            if (dtt115.Rows.Count > 0)
                            {
                                passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                                remarks = remarks + passno;
                            }
                        }
                        else if (alloctype == "Donor Paid Allocation")
                        {
                            int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                            OdbcCommand cmd115 = new OdbcCommand();
                            cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                            cmd115.Parameters.AddWithValue("attribute", "passno");
                            cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                            DataTable dtt115 = new DataTable();
                            dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                            if (dtt115.Rows.Count > 0)
                            {
                                passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                                remarks = remarks + passno;
                            }
                        }
                        else if (alloctype == "Donor multiple pass")
                        {
                            //

                            int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                            mpass = "";

                            OdbcCommand cmd115 = new OdbcCommand();
                            cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                            cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                            cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");

                            DataTable dtt115 = new DataTable();
                            dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

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

                        build = "";
                        building = dtt350.Rows[ii]["buildingname"].ToString();
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
                        room = dtt350.Rows[ii]["roomno"].ToString();
                        Session["rec"] = rec.ToString();
                        Session["tno"] = transno.ToString();
                        indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                        ind = indat.ToString("dd-MMM");
                        it = indat.ToString("hh:mm:tt");
                        indate = it + "       " + ind;
                        if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                        {
                            outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                            outd = outdat.ToString("dd-MMM");
                            ot = outdat.ToString("hh:mm:tt");
                            outdate = ot + "       " + outd;
                        }
                        else
                        {
                            outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                            outd = outdat.ToString("dd-MMM");
                            ot = outdat.ToString("hh:mm:tt");
                            outdate = ot + "       " + outd;
                        }
                        rents = dtt350.Rows[ii]["roomrent"].ToString();
                        deposits = dtt350.Rows[ii]["deposit"].ToString();
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
                        if ((i == 27) || (ii == cont - 1))
                        {
                            PdfPTable table2 = new PdfPTable(10);
                            float[] colWidths2 = { 40, 65, 130, 75, 85, 85, 60, 60, 50, 70};
                            table2.SetWidths(colWidths2);

                            PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                            cell41.Colspan = 6;
                            table2.AddCell(cell41);

                            PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                            table2.AddCell(cell49);

                            gtr = gtr + decimal.Parse(rr.ToString());
                            gtd = gtd + decimal.Parse(dde.ToString());

                            PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                            table2.AddCell(cell50);

                            PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            table2.AddCell(cell51);

                            doc.Add(table2);
                            rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                        }
                        if (ii == cont - 1)
                        {
                            PdfPTable table10 = new PdfPTable(10);
                            float[] colWidths10 = { 40, 65, 130, 75, 85, 85, 60, 60, 50, 70};
                            table10.SetWidths(colWidths10);

                            PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", font10)));
                            cell500p10.Colspan = 9;
                            cell500p10.Border = 0;
                            cell500p10.HorizontalAlignment = 1;
                            table10.AddCell(cell500p10);
                            /////////////////////
                            PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
                            cell500p12.Colspan = 2;
                            cell500p12.Border = 0;
                            cell500p12.HorizontalAlignment = 0;
                            table10.AddCell(cell500p12);

                            PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
                            cell500p13.Colspan = 3;
                            cell500p13.Border = 0;
                            cell500p13.HorizontalAlignment = 0;
                            table10.AddCell(cell500p13);

                            PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("Deposit : ", font10)));
                            cell500p15.Colspan = 2;
                            cell500p15.Border = 0;
                            cell500p15.HorizontalAlignment = 0;
                            table10.AddCell(cell500p15);

                            PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), font10)));
                            cell500p11.Colspan = 2;
                            cell500p11.Border = 1;
                            cell500p11.HorizontalAlignment = 1;
                            table10.AddCell(cell500p11);
                            ///////////////////
                            PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", font10)));
                            cell500p14.Colspan = 2;
                            cell500p14.Border = 1;
                            cell500p14.HorizontalAlignment = 1;
                            table10.AddCell(cell500p14);

                            //NumberToEnglish n = new NumberToEnglish();
                            string re = objcls.NumberToTextWithLakhs(Int64.Parse(gtr.ToString()));
                            re = re + " Only";
                            PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, font10)));
                            cell500p16.Colspan = 7;
                            cell500p16.Border = 1;
                            cell500p16.HorizontalAlignment = 1;
                            table10.AddCell(cell500p16);

                            PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("Deposit: ", font10)));
                            cell500p17.Colspan = 2;
                            cell500p17.Border = 1;
                            cell500p17.HorizontalAlignment = 1;
                            table10.AddCell(cell500p17);

                            string de = objcls.NumberToTextWithLakhs(Int64.Parse(gtd.ToString()));
                            de = de + " Only";
                            PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk(de, font10)));
                            cell500p18.Colspan = 7;
                            cell500p18.Border = 1;
                            cell500p18.HorizontalAlignment = 1;
                            table10.AddCell(cell500p18);
                            gtr = 0;
                            gtd = 0;

                            PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellfb1.PaddingLeft = 20;
                            cellfb1.Colspan = 9;
                            cellfb1.MinimumHeight = 30;
                            cellfb1.Border = 0;
                            table10.AddCell(cellfb1);

                            PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                            cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellfb.PaddingLeft = 20;
                            cellfb.Colspan = 9;
                            cellfb.MinimumHeight = 30;
                            cellfb.Border = 0;
                            table10.AddCell(cellfb);

                            PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                            cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellf1b.PaddingLeft = 20;
                            cellf1b.Colspan = 9;
                            cellf1b.Border = 0;

                            table10.AddCell(cellf1b);

                            PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                            cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                            cellh2.PaddingLeft = 20;
                            cellh2.Border = 0;
                            cellh2.Colspan = 9;
                            table10.AddCell(cellh2);
                            doc.Add(table10);
                        }
                    }
                    doc.Close();
                    Random r = new Random();
                    string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report";
                    string Script = "";
                    Script += "<script id='PopupWindow'>";
                    Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                    Script += "confirmWin.Setfocus()</script>";
                    if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                        Page.RegisterClientScriptBlock("PopupWindow", Script);

                    try
                    {
                        OdbcCommand cmd901 = new OdbcCommand();
                        cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                        cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                        DataTable dtt901 = new DataTable();
                        dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                        id = int.Parse(dtt901.Rows[0][0].ToString());
                        int tno = int.Parse(Session["tno"].ToString());
                        string ct = Session["num"].ToString();
                        OdbcCommand cmd25 = new OdbcCommand();
                        cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                        cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + dt3 + "'");
                        cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                        int a1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
                    }
                    catch
                    {
                        id = 1;
                        int tno = int.Parse(Session["tno"].ToString());
                        string ct = Session["num"].ToString();

                        OdbcCommand cmd589 = new OdbcCommand();
                        cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                        cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + dt3 + "'");
                        int a11 = objcls.Procedures("CALL savedata(?,?)", cmd589);
                    }
                }
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in taking report");
        }
        #endregion

    }
    #endregion

    #region Leder Between dates excel
    protected void lnkAccLedBetExcel_Click(object sender, EventArgs e)
    {
        //dtt356a.Columns.RemoveAt(0);
        if ((txtfromd.Text == "") || (txttod.Text == ""))
        {
            okmessage("Tsunami ARMS - Warning", "Enter dates");
            return;
        }
        string str1 = objcls.yearmonthdate(txtfromd.Text.ToString());
        string str2 = objcls.yearmonthdate(txttod.Text.ToString());
        DateTime ind = DateTime.Parse(str1);
        DateTime outd = DateTime.Parse(str2);
        if (outd < ind)
        {
            okmessage("Tsunami ARMS - Warning", "Check the dates");
            return;
        }
        string datf = objcls.yearmonthdate(txtfromd.Text);
        string datt = objcls.yearmonthdate(txttod.Text);
        DateTime daf1 = DateTime.Parse(datf.ToString());
        DateTime dat1 = DateTime.Parse(datt.ToString());
        string g1 = daf1.ToString("yyyy-MM-dd");
        string g2 = dat1.ToString("yyyy-MM-dd");
        string g3 = daf1.ToString("dd/MM/yy");
        string g4 = dat1.ToString("dd/MM/yy");
        string strsql1 = "m_room as room,"
                + "m_sub_building as build,"
                + "t_roomallocation as alloc"
                + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                + " Left join m_sub_district as dist on alloc.district_id=dist.district_id  left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";
        string strsql2 = "alloc.alloc_id,"
                       + "alloc.alloc_no,"
                       + "alloc.adv_recieptno,"
                       + "alloc.pass_id,"
                        + "alloc.swaminame,"
                       + "alloc.place,"
                        + "build.buildingname,"
                       + "room.roomno,"
                       + "alloc.phone,"
                       + "alloc.idproof,"
                       + "alloc.alloc_type,"
                       + "alloc.idproofno,"

                       + "alloc.noofinmates,"
                       + "alloc.advance,"
                       + "alloc.reason,"
                       + "alloc.othercharge,"
                       + "alloc.allocdate,"
                       + "alloc.exp_vecatedate,"
                         + "alloc.numberofunit,"
                       + "alloc.roomrent,"
                       + "alloc.state_id,"
                       + "alloc.district_id,"
                       + "alloc.deposit,"
                       + "alloc.totalcharge,"
                       + "alloc.realloc_from,"
                       + "alloc.reason_id,"
                       + "actualvecdate,"
                       + "alloc.counter_id";
        strsql3 = "alloc.room_id=room.room_id"
          + " and room.build_id=build.build_id"
          + " and alloc.dayend >= '" + datf + "' and alloc.dayend <= '" + datt + "' order by alloc.alloc_id asc";
        OdbcCommand cmd350 = new OdbcCommand();
        cmd350.Parameters.AddWithValue("tblname", strsql1);
        cmd350.Parameters.AddWithValue("attribute", strsql2);
        cmd350.Parameters.AddWithValue("conditionv", strsql3);
        DataTable dtt350 = new DataTable();
        dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
        dtt350.Columns.Remove("alloc_id");
        dtt350.Columns.Remove("pass_id");
        dtt350.Columns.Remove("phone");
        dtt350.Columns.Remove("idproof");
        dtt350.Columns.Remove("idproofno");
        dtt350.Columns.Remove("advance");
        dtt350.Columns.Remove("reason");
        dtt350.Columns.Remove("othercharge");
        dtt350.Columns.Remove("state_id");
        dtt350.Columns.Remove("district_id");
        dtt350.Columns.Remove("totalcharge");
        dtt350.Columns.Remove("reason_id");
        dtt350.Columns.Remove("counter_id");
        dtt350.Columns.Remove("actualvecdate");
        dtt350.Columns.Remove("realloc_from");
        if (dtt350.Rows.Count > 0)
        {
            GetExcel(dtt350, "Accomodation Ledger Between Details ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion

    #region Accomodation ledger report Excel
    protected void lnkAccLedExcel_Click(object sender, EventArgs e)
    {

        if (txtdate.Text == "")
        {
            okmessage("Tsunami ARMS - Message", "Please Enter date");
            return;
        }

        string dt3 = objcls.yearmonthdate(txtdate.Text);


        string strsql1 = "m_room as room,"
               + "m_sub_building as build,"
               + "t_roomallocation as alloc"
               + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
               + " Left join m_sub_district as dist on alloc.district_id=dist.district_id left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";

        string strsql2 = "alloc.alloc_id,"
                       + "alloc.alloc_no,"
                        + "alloc.adv_recieptno,"
                        + "alloc.swaminame,"
                       + "alloc.place,"
                        + "build.buildingname,"
                       + "room.roomno,"
                        + "alloc.noofinmates,"
                        + "alloc.allocdate,"
                       + "alloc.exp_vecatedate,"
                       + "alloc.pass_id,"
                       + "alloc.phone,"
                       + "alloc.idproof,"
                       + "alloc.idproofno,"
                       + "alloc.numberofunit,"
                       + "alloc.advance,"
                       + "alloc.reason,"
                       + "alloc.othercharge,"
                       + "alloc.roomrent,"
                       + "alloc.state_id,"
                       + "alloc.district_id,"
                       + "alloc.deposit,"
                        + "alloc.alloc_type,"
                      + "alloc.totalcharge,"
                   + "alloc.realloc_from,"
                   + "alloc.reason_id,"
                   + "actualvecdate";


        strsql3 = "alloc.room_id=room.room_id"
          + " and room.build_id=build.build_id"
          + " and alloc.dayend='" + dt3 + "' order by alloc.alloc_id asc";


        OdbcCommand cmd350 = new OdbcCommand();
        cmd350.Parameters.AddWithValue("tblname", strsql1);
        cmd350.Parameters.AddWithValue("attribute", strsql2);
        cmd350.Parameters.AddWithValue("conditionv", strsql3);

        DataTable dtt350 = new DataTable();
        dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);

        dtt350.Columns.Remove("alloc_id");
        dtt350.Columns.Remove("pass_id");
        dtt350.Columns.Remove("phone");
        dtt350.Columns.Remove("idproof");
        dtt350.Columns.Remove("idproofno");
        dtt350.Columns.Remove("advance");
        dtt350.Columns.Remove("reason");
        dtt350.Columns.Remove("actualvecdate");
        dtt350.Columns.Remove("othercharge");
        dtt350.Columns.Remove("state_id");
        dtt350.Columns.Remove("district_id");
        dtt350.Columns.Remove("totalcharge");
        dtt350.Columns.Remove("reason_id");
        dtt350.Columns.Remove("realloc_from");
        if (dtt350.Rows.Count > 0)
        {
            GetExcel(dtt350, "Accomodation Ledger ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion
   
    protected void lnkCollectionCompare_Click(object sender, EventArgs e)
    {
        #region MyRegion
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string datte, timme, num;
        datte = ds2.ToString("dd MMMM yyyy");
        timme = ds2.ToShortTimeString();
        string dd1 = ds2.ToString("yyyy-MM-dd");        
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        OdbcCommand cmd205 = new OdbcCommand();
        cmd205.CommandType = CommandType.StoredProcedure;
        cmd205.Parameters.AddWithValue("tblname", "m_season ms,m_sub_season msb");
        cmd205.Parameters.AddWithValue("attribute", "seasonname");
        cmd205.Parameters.AddWithValue("conditionv", "curdate()>=startdate and enddate>=curdate()   and ms.rowstatus!='2'  and is_current='1' and msb.season_sub_id=ms.season_sub_id");
        DataTable dtt205 = new DataTable();
        dtt205 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd205);
        string seasonname = "";
        if (dtt205.Rows.Count > 0)
        {
            seasonname = dtt205.Rows[0]["seasonname"].ToString();
        }
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }        
        int malyear1 = 0, malyear2 = 0, malyearid = 0;
        OdbcCommand cmd2 = new OdbcCommand();
        cmd2.Parameters.AddWithValue("tblname", "t_settings");
        cmd2.Parameters.AddWithValue("attribute", "mal_year_id,mal_year");
        cmd2.Parameters.AddWithValue("conditionv", "curdate() between start_eng_date and end_eng_date and is_current=" + 1 + "");
        DataTable dtt2 = new DataTable();
        dtt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
        if (dtt2.Rows.Count > 0)
        {
            malyear1 = Convert.ToInt32(dtt2.Rows[0]["mal_year"]);
            malyearid = Convert.ToInt32(dtt2.Rows[0]["mal_year_id"]);
            Session["malyyearid"] = malyearid;
            Session["malyear"] = malyear1;                 
        }                   
        malyear2 = malyear1 - 1;
        DateTime d4 = DateTime.Now;
        string dd4 = d4.ToString("dd MMMM yyyy");
        string tt1 = d4.ToString("hh:mm tt");
        string bdate = dd4.ToString() + " " + tt1.ToString();

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "CollectionComparison" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font10 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        pdfPage page = new pdfPage();
        page.strRptMode = "Collectioncomparison";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table2 = new PdfPTable(6);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth1 = { 2, 3, 3, 3, 3, 4 };
        table2.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("COLLECTION COMPARISON STATEMENT OF ACCOMMODATION OFFICE", font9)));
        cell.Colspan = 7;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font10)));
        cell11.Rowspan = 2;
        table2.AddCell(cell11);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("" + malyear1.ToString() + " ME", font10)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("" + malyear2.ToString() + " ME", font10)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table2.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Variation", font10)));
        cell15.Rowspan = 2;
        table2.AddCell(cell15);

        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font10)));
        table2.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Collection", font10)));
        table2.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font10)));
        table2.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Collection", font10)));
        table2.AddCell(cell21);

        doc.Add(table2);
        int i = 0;
        //string season = "SELECT season_id FROM m_season WHERE season_sub_id='"+cmbseasoncomp.SelectedValue+"' AND YEAR(startdate) = YEAR((SELECT start_eng_date FROM t_settings WHERE mal_year = '"+malyear1.ToString()+"' LIMIT 1))";
        string season = "SELECT season_id FROM m_season WHERE season_sub_id='" + cmbseasoncomp.SelectedValue + "' AND YEAR(startdate) = '2013' LIMIT 1";
        DataTable dtseason=objcls.DtTbl(season);
        string seasonvalcurrent = dtseason.Rows[0]["season_id"].ToString();
        //string seasonprev = "SELECT season_id FROM m_season WHERE season_sub_id='" + cmbseasoncomp.SelectedValue + "' AND YEAR(startdate) = YEAR((SELECT start_eng_date FROM t_settings WHERE mal_year = '" + malyear2.ToString() + "' LIMIT 1))";
        string seasonprev = "SELECT season_id FROM m_season WHERE season_sub_id='" + cmbseasoncomp.SelectedValue + "' AND YEAR(startdate) = '2012' LIMIT 1";
        DataTable dtseasonprev = objcls.DtTbl(seasonprev);
        string seasonvalprev = dtseasonprev.Rows[0]["season_id"].ToString();
        string trans = "SELECT SUM(total),DATE_FORMAT(dayend,'%d-%b-%Y') AS dayend FROM t_liabilityregister WHERE dayend BETWEEN (SELECT startdate FROM m_season WHERE season_id ='" + seasonvalcurrent + "') AND (SELECT enddate FROM m_season WHERE season_id ='" + seasonvalcurrent + "') GROUP BY dayend";
        DataTable dtt351 = objcls.DtTbl(trans);
        string transprev = "SELECT SUM(total),DATE_FORMAT(dayend,'%d-%b-%Y') AS dayend FROM t_liabilityregister WHERE dayend BETWEEN (SELECT startdate FROM m_season WHERE season_id ='" + seasonvalprev + "') AND (SELECT enddate FROM m_season WHERE season_id ='" + seasonvalprev + "') GROUP BY dayend";
        DataTable dtt351prev = objcls.DtTbl(transprev);
        if (dtt351.Rows.Count >= dtt351prev.Rows.Count)
        {
            for (int ii = 0; ii < dtt351.Rows.Count; ii++)
            {
                no = no + 1;
                num = no.ToString();
                if (i > 45)// total rows on page
                {
                    i = 0;
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(6);
                    table1.TotalWidth = 550f;
                    table1.LockedWidth = true;
                    float[] colwidth2 = { 2, 3, 3, 3, 3, 4 };
                    table1.SetWidths(colwidth2);
                    PdfPCell cell11o = new PdfPCell(new Phrase(new Chunk("No", font10)));
                    cell11o.Rowspan = 2;
                    table1.AddCell(cell11o);
                    PdfPCell cell13o = new PdfPCell(new Phrase(new Chunk("" + malyear1.ToString() + " ME", font10)));
                    cell13o.Colspan = 2;
                    cell13o.HorizontalAlignment = 1;
                    table1.AddCell(cell13o);
                    PdfPCell cell14o = new PdfPCell(new Phrase(new Chunk("" + malyear2.ToString() + " ME", font10)));
                    cell14o.HorizontalAlignment = 1;
                    cell14o.Colspan = 2;
                    table1.AddCell(cell14o);
                    PdfPCell cell15o = new PdfPCell(new Phrase(new Chunk("Variation", font10)));
                    cell15o.Rowspan = 2;
                    table1.AddCell(cell15o);
                    PdfPCell cell18o = new PdfPCell(new Phrase(new Chunk("Date", font10)));
                    table1.AddCell(cell18o);
                    PdfPCell cell19o = new PdfPCell(new Phrase(new Chunk("Collection", font10)));
                    table1.AddCell(cell19o);
                    PdfPCell cell20o = new PdfPCell(new Phrase(new Chunk("Date", font10)));
                    table1.AddCell(cell20o);
                    PdfPCell cell21o = new PdfPCell(new Phrase(new Chunk("Collection", font10)));
                    table1.AddCell(cell21o);

                    doc.Add(table1);
                    i = 0;
                }
          
            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth3 = { 2, 3, 3, 3, 3, 4 };
            table.SetWidths(colwidth3);

            collectioncurrent = dtt351.Rows[ii]["SUM(total)"].ToString();
            datecurrent = dtt351.Rows[ii]["dayend"].ToString();
            if (ii >= dtt351prev.Rows.Count || dtt351prev.Rows.Count == 0)
            {
                collectionprev = "";
                dateprev = "";
            }
            else
            {
                collectionprev = dtt351prev.Rows[ii][0].ToString();
                dateprev = dtt351prev.Rows[ii][1].ToString();
            }
            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21b);
            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(datecurrent, font8)));
            table.AddCell(cell23);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(collectioncurrent, font8)));
            table.AddCell(cell23a);
            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(dateprev, font8)));
            table.AddCell(cell24);
            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(collectionprev, font8)));
            table.AddCell(cell25);
            if (collectionprev == "")
            {
                collectionprev = "0";
            }
            if (collectioncurrent == "")
            {
                collectioncurrent = "0";
            }
            variations = double.Parse(collectioncurrent) - double.Parse(collectionprev);
            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(variations.ToString(), font8)));
            table.AddCell(cell26);
            i++;
            doc.Add(table);
        }
        }
        else
        {
            for (int ii = 0; ii < dtt351prev.Rows.Count; ii++)
            {
                no = no + 1;
                num = no.ToString();
                if (i > 45)// total rows on page
                {
                    i = 0;
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(6);
                    table1.TotalWidth = 550f;
                    table1.LockedWidth = true;
                    float[] colwidth2 = { 2, 3, 3, 3, 3, 4 };
                    table1.SetWidths(colwidth2);
                    PdfPCell cell11o = new PdfPCell(new Phrase(new Chunk("No", font10)));
                    cell11o.Rowspan = 2;
                    table1.AddCell(cell11o);
                    PdfPCell cell13o = new PdfPCell(new Phrase(new Chunk("" + malyear1.ToString() + " ME", font10)));
                    cell13o.Colspan = 2;
                    cell13o.HorizontalAlignment = 1;
                    table1.AddCell(cell13o);
                    PdfPCell cell14o = new PdfPCell(new Phrase(new Chunk("" + malyear2.ToString() + " ME", font10)));
                    cell14o.HorizontalAlignment = 1;
                    cell14o.Colspan = 2;
                    table1.AddCell(cell14o);
                    PdfPCell cell15o = new PdfPCell(new Phrase(new Chunk("Variation", font10)));
                    cell15o.Rowspan = 2;
                    table1.AddCell(cell15o);
                    PdfPCell cell18o = new PdfPCell(new Phrase(new Chunk("Date", font10)));
                    table1.AddCell(cell18o);
                    PdfPCell cell19o = new PdfPCell(new Phrase(new Chunk("Collection", font10)));
                    table1.AddCell(cell19o);
                    PdfPCell cell20o = new PdfPCell(new Phrase(new Chunk("Date", font10)));
                    table1.AddCell(cell20o);
                    PdfPCell cell21o = new PdfPCell(new Phrase(new Chunk("Collection", font10)));
                    table1.AddCell(cell21o);

                    doc.Add(table1);
                    i = 0;
                }

                PdfPTable table = new PdfPTable(6);
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                float[] colwidth3 = { 2, 3, 3, 3, 3, 4 };
                table.SetWidths(colwidth3);

                //collectioncurrent = dtt351.Rows[ii]["SUM(amount)"].ToString();
                //datecurrent = dtt351.Rows[ii]["DATE"].ToString();
                if (ii > dtt351.Rows.Count || dtt351.Rows.Count == 0)
                {
                    collectioncurrent = "";
                    datecurrent = "";
                }
                else
                {

                   // ii = dtt351.Rows.Count-1;
                    collectioncurrent = dtt351.Rows[ii]["SUM(total)"].ToString();
                    datecurrent = dtt351.Rows[ii]["dayend"].ToString();                    
                }
                collectionprev = dtt351prev.Rows[ii]["SUM(total)"].ToString();
                dateprev = dtt351prev.Rows[ii]["dayend"].ToString();

                PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table.AddCell(cell21b);
                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(datecurrent, font8)));
                table.AddCell(cell23);
                PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(collectioncurrent, font8)));
                table.AddCell(cell23a);
                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(dateprev, font8)));
                table.AddCell(cell24);
                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(collectionprev, font8)));
                table.AddCell(cell25);
                if (collectionprev == "")
                {
                    collectionprev = "0";
                }
                if (collectioncurrent == "")
                {
                    collectioncurrent = "0";
                }
                variations = double.Parse(collectioncurrent) - double.Parse(collectionprev);
                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(variations.ToString(), font8)));
                table.AddCell(cell26);
                i++;
                doc.Add(table);
            }
        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font10)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);

        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font10)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font10)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);

        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Vacant Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        Session["head"] = ch.ToString();
        Response.Redirect("print.aspx");
        #endregion
    }

    protected void lnkreservelist_Click(object sender, EventArgs e)
    {
        try
        {
            lblmessage.Visible = false;
            string str1 = objcls.yearmonthdate(txtreportdatefrom.Text);
            string str2 = objcls.yearmonthdate(txtreportdateto.Text);
            string place;
            DataTable dt = new DataTable();
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_roomreservation t");
            cmd31.Parameters.AddWithValue("attribute", "DISTINCT t.reserve_no,t.place,t.reservedate 'Reserve from',t.expvacdate 'Reserve To',reserve_mode  AS 'Customer Type',t.swaminame,status_reserve,CASE  WHEN (SELECT DISTINCT reserve_id FROM t_roomallocation  WHERE t_roomallocation.reserve_id = t.reserve_id) != '' THEN 'allocated' ELSE 'not allocated' END AS 'status'  ");
            cmd31.Parameters.AddWithValue("conditionv", "DATE_FORMAT(reservedate,'%Y/%m/%d') >=  '" + str1.ToString() + "'  and DATE_FORMAT(reservedate,'%Y/%m/%d') <= '" + str2.ToString() + "' ORDER BY t.reserve_id  ASC");
            dt = objcls.SpDtTbl("call selectcond(?,?,?)", cmd31);
            Session["dataval"] = dt;
            if (dt.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = false;
                lblOk.Text = "No Details found";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                return;
            }
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string dat = gh.ToString("dd-MM-yyyy");
            // creating a  file to save the report .... setting its font
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);
            string ch = "Reservationchart" + transtim.ToString() + ".pdf";
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            //  string pdfFilePath = Server.MapPath(".") + "/pdf/Reservationchart.pdf";
            Font font6 = FontFactory.GetFont("Arial", 8);
            Font font8 = FontFactory.GetFont("Arial", 9, 1);
            Font font91 = FontFactory.GetFont("airial", 9, 1);
            Font font10 = FontFactory.GetFont("Arial", 10, 1);

            # region  report table coloumn and header settings
            pdfPage page = new pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

            PdfPTable table1q = new PdfPTable(1);
            float[] colwidthq = { 70 };
            table1q.SetWidths(colwidthq);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Room reservation chart of accommodation office", font10)));

            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1q.AddCell(cell);

            doc.Add(table1q);

            OdbcCommand ddh = new OdbcCommand();
            ddh.Parameters.AddWithValue("tblname", "m_sub_season s,m_season m");
            ddh.Parameters.AddWithValue("attribute", "distinct  s.season_sub_id, s.seasonname");
            ddh.Parameters.AddWithValue("conditionv", "s.rowstatus <> 2 and s.season_sub_id=m.season_sub_id AND startdate<='" + str1 + "' AND enddate>='" + str2 + "'");
            DataTable dttf = new DataTable();
            dttf = objcls.SpDtTbl("call selectcond(?,?,?)", ddh);
            string seas = "";
            if (dttf.Rows.Count > 0)
            {
                seas = dttf.Rows[0]["seasonname"].ToString();
            }
            else
            {
                lblHead.Visible = false;
                lblHead2.Visible = false;
                lblOk.Text = "No Season  found";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                // return;
            }

            PdfPTable tablep = new PdfPTable(2);
            float[] colWidths23 = { 70, 70 };


            PdfPCell cellv = new PdfPCell(new Phrase("Season: " + seas.ToString(), font91));
            // cellv.Colspan = 2;
            cellv.Border = 0;
            cellv.HorizontalAlignment = 0;
            tablep.AddCell(cellv);

            PdfPCell cellv2 = new PdfPCell(new Phrase("Date: " + txtreportdatefrom.Text + "\n \n", font91));
            //cellv2.Colspan = 2;
            cellv2.Border = 0;
            cellv2.HorizontalAlignment = 2;
            tablep.AddCell(cellv2);

            doc.Add(tablep);


            PdfPTable tablepqq = new PdfPTable(9);
            float[] colWidths1 = { 5, 10, 13, 13, 7, 13, 9, 16, 8 };
            tablepqq.WidthPercentage = 94;
            tablepqq.SetWidths(colWidths1);
            string s2 = "";
            if (cmbReportpass.SelectedValue == "All")
            {

                s2 = @"SELECT reserve_no,reserve_mode,swaminame,place,m_room.build,m_room.roomno,DATE_FORMAT(reservedate,'%d/%m/%Y %l:%i %p'),
CASE WHEN status_reserve =  0 THEN 'Reserved' WHEN status_reserve=2 THEN 'Occupied' WHEN status_reserve=3 THEN 'Cancelled' END AS 'Status'
FROM t_roomreservation INNER JOIN m_room ON m_room.room_id = t_roomreservation.room_id
WHERE reservedate BETWEEN '" + objcls.yearmonthdate(txtreportdatefrom.Text) + " ' AND '" + objcls.yearmonthdate(txtreportdateto.Text) + "' AND status_reserve != '2'";
            }
            else if (cmbReportpass.SelectedValue == "Tdb")
            {


                s2 = @"SELECT reserve_no,reserve_mode,swaminame,place,m_room.build,m_room.roomno,DATE_FORMAT(reservedate,'%d/%m/%Y %l:%i %p'),
CASE WHEN status_reserve =  0 THEN 'Reserved' WHEN status_reserve=2 THEN 'Occupied' WHEN status_reserve=3 THEN 'Cancelled' END AS 'Status'
FROM t_roomreservation INNER JOIN m_room ON m_room.room_id = t_roomreservation.room_id
WHERE reservedate BETWEEN '" + objcls.yearmonthdate(txtreportdatefrom.Text) + " ' AND '" + objcls.yearmonthdate(txtreportdateto.Text) + "' AND status_reserve != '2'AND t_roomreservation.reserve_mode='" + cmbReportpass.SelectedValue + "' ";


            }




            else if (cmbReportpass.SelectedValue == "0")
            {
                s2 = @"SELECT reserve_no,reserve_mode,swaminame,place,m_room.build,m_room.roomno,DATE_FORMAT(reservedate,'%d/%m/%Y %l:%i %p'),
CASE WHEN status_reserve =  0 THEN 'Reserved' WHEN status_reserve=2 THEN 'Occupied' WHEN status_reserve=3 THEN 'Cancelled' END AS 'Status'
FROM t_roomreservation INNER JOIN m_room ON m_room.room_id = t_roomreservation.room_id
WHERE reservedate BETWEEN '" + objcls.yearmonthdate(txtreportdatefrom.Text) + " ' AND '" + objcls.yearmonthdate(txtreportdateto.Text) + "' AND status_reserve != '2'AND t_roomreservation.passtype='" + cmbReportpass.SelectedValue + "' ";
            }
            else if (cmbReportpass.SelectedValue == "1")
            {
                s2 = @"SELECT reserve_no,reserve_mode,swaminame,place,m_room.build,m_room.roomno,DATE_FORMAT(reservedate,'%d/%m/%Y %l:%i %p'),
CASE WHEN status_reserve =  0 THEN 'Reserved' WHEN status_reserve=2 THEN 'Occupied' WHEN status_reserve=3 THEN 'Cancelled' END AS 'Status'
FROM t_roomreservation INNER JOIN m_room ON m_room.room_id = t_roomreservation.room_id
WHERE reservedate BETWEEN '" + objcls.yearmonthdate(txtreportdatefrom.Text) + " ' AND '" + objcls.yearmonthdate(txtreportdateto.Text) + "' AND status_reserve != '2'AND t_roomreservation.passtype='" + cmbReportpass.SelectedValue + "' ";
            }


            else if (cmbReportpass.SelectedValue == "General")
            {
                s2 = @"SELECT reserve_no,reserve_mode,swaminame,place,m_room.build,m_room.roomno,DATE_FORMAT(reservedate,'%d/%m/%Y %l:%i %p'),
CASE WHEN status_reserve =  0 THEN 'Reserved' WHEN status_reserve=2 THEN 'Occupied' WHEN status_reserve=3 THEN 'Cancelled' END AS 'Status'
FROM t_roomreservation INNER JOIN m_room ON m_room.room_id = t_roomreservation.room_id
WHERE reservedate BETWEEN '" + objcls.yearmonthdate(txtreportdatefrom.Text) + " ' AND '" + objcls.yearmonthdate(txtreportdateto.Text) + "' AND status_reserve != '2'AND t_roomreservation.reserve_mode='" + cmbReportpass.SelectedValue + "' ";
            }

            DataTable ds = objcls.DtTbl(s2);

            PdfPCell cellc1 = new PdfPCell(new Phrase(new Chunk("Sl No", font91)));
            tablepqq.AddCell(cellc1);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("  Reserve No", font91)));
            tablepqq.AddCell(cell1);

            PdfPCell cell07xx = new PdfPCell(new Phrase(new Chunk("  Reserve Mode", font91)));
            tablepqq.AddCell(cell07xx);


            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("  Swami Name", font91)));
            tablepqq.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("  Place", font91)));
            tablepqq.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("        Build", font91)));
            tablepqq.AddCell(cell5);

            PdfPCell cell5xx = new PdfPCell(new Phrase(new Chunk("  Room No", font91)));
            tablepqq.AddCell(cell5xx);

            PdfPCell cell5xx1 = new PdfPCell(new Phrase(new Chunk("  Reserve Date", font91)));
            tablepqq.AddCell(cell5xx1);

            PdfPCell cell5xx2 = new PdfPCell(new Phrase(new Chunk("  Status", font91)));
            tablepqq.AddCell(cell5xx2);



            for (int i = 0; i < ds.Rows.Count; i++)
            {

                PdfPCell celljaq1 = new PdfPCell(new Phrase((i + 1).ToString(), font91));
                celljaq1.HorizontalAlignment = 0;
                tablepqq.AddCell(celljaq1);


                PdfPCell cellja1 = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][0].ToString(), font91)));
                cellja1.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja1);

                PdfPCell cellja13 = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][1].ToString(), font91)));
                cellja13.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja13);


                PdfPCell cellja13a = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][2].ToString(), font91)));
                cellja13a.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja13a);


                PdfPCell cellja13c = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][3].ToString(), font91)));
                cellja13c.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja13c);


                PdfPCell cellja132 = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][4].ToString(), font91)));
                cellja132.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja132);

                PdfPCell cellja13v = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][5].ToString(), font91)));
                cellja13v.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja13v);

                PdfPCell cellja13w = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][6].ToString(), font91)));
                cellja13w.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja13w);

                PdfPCell cellja13d = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][7].ToString(), font91)));
                cellja13d.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja13d);




            }
            doc.Add(tablepqq);


            doc.Close();
            # endregion

            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        catch (Exception es)
        {
            string sss = es.Message;
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
            return;
        }

    }
    protected void txtreportdatefrom_TextChanged(object sender, EventArgs e)
    {
        String rtodate = objcls.yearmonthdate(txtreportdatefrom.Text);
        DateTime rtodate1 = DateTime.Parse(rtodate);
        rtodate1 = rtodate1.AddDays(1);
        txtreportdateto.Text = rtodate1.ToString("dd-MM-yyyy");
    }   

    # region Due Vacating Room Details
    protected void lnkDueVacatingReports_Click(object sender, EventArgs e)
    {
        if (txtTime.Text != "")
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }          
            string date5 = DateTime.Now.ToString("yyyy-MM-dd");
            string date6 = DateTime.Now.ToString("dd  MMM");
            DateTime datedd = DateTime.Parse(txtTime.Text);
            string date10 = datedd.ToString("hh:mm:ss");
            string checkdate = date5 + " " + date10;
            OdbcCommand cc = new OdbcCommand("DROP view if exists tempnonvacatexc", con);
            cc.ExecuteNonQuery();

            //string sqlview="create view  tempnonvacatexc as  (SELECT  * from t_roomallocation ta   WHERE ta.roomstatus='2' "
            //+ " and date(exp_vecatedate)=curdate() and ADDTIME(exp_vecatedate,MAKETIME((SELECT graceperiod from t_policy_allocation "
            //+ " WHERE reqtype='Common' and rowstatus<>'2' and ((curdate()>=fromdate and curdate()<=todate) "
            //+ " or (curdate()>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<'" + checkdate + "')";

            string date_new = txtDaycloseDate.Text;
            string check_date = "", date10x1 = "";
            try
            {
                if (date_new != "")
                {
                    date_new = objcls.yearmonthdate(txtDaycloseDate.Text);
                    DateTime dateddx = DateTime.Parse(txtTime.Text);
                    string date10x = dateddx.ToString("HH:mm:ss");
                    date10x1 = dateddx.ToString("hh:mm:ss tt");
                    check_date = date_new + " " + date10x;
                }
            }
            catch
            {
                okmessage("Please Check Date and Time", "aa");
            }
            string sqlview = @"create view  tempnonvacatexc as  (SELECT
                        ta.alloc_id,ta.alloc_no,ta.reserve_id,ta.swaminame,ta.district_id,
                        ta.state_id,ta.place,ta.std,ta.phone,ta.mobile,ta.idproof,ta.idproofno,
                        ta.room_id,ta.noofinmates,ta.allocdate,ta.exp_vecatedate,ta.barcode,
                        ta.is_plainprint,ta.adv_recieptno,ta.numberofunit,ta.alloc_type,
                        ta.pass_id,ta.donor_id,ta.dayend,ta.userid,
                        ta.roomrent,ta.roomstatus,ta.advance,ta.deposit,ta.rescharge,ta.reason,
                        ta.othercharge,ta.totalcharge,ta.balanceamount,
                        ta.season_id,ta.counter_id,ta.createdby,ta.createdon,ta.realloc_from,ta.reason_id
                        from t_roomallocation ta
                        WHERE ta.roomstatus='2' 
                        and date_format(exp_vecatedate,'%Y/%m/%d %H:%i:%s')<'" + check_date + "')";
            OdbcCommand cmdview = new OdbcCommand(sqlview, con);
            cmdview.ExecuteNonQuery();
            try
            {
                string data = Session["dayend"].ToString();
                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.CommandType = CommandType.StoredProcedure;
                cmd31.Parameters.AddWithValue("tblname", " tempnonvacatexc tt,m_room mr ,m_sub_building msb");
                cmd31.Parameters.AddWithValue("attribute", "adv_recieptno,place, buildingname ,roomno ,swaminame , exp_vecatedate as vacatedate,buildingname ");                               
                cmd31.Parameters.AddWithValue("conditionv", "tt.room_id=mr.room_id and msb.build_id=mr.build_id order by mr.build_id,roomno ");                               
                DataTable dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
               
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);
                DateTime gh = DateTime.Now;
                string transtim = gh.ToString("dd-MM-yyyy HH-mm");
                string ch = "DueVacatingRooms" + transtim.ToString() + ".pdf";
                string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

                Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
                Font font7 = FontFactory.GetFont("ARIAL", 9);
                Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
                Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
                pdfPage page = new pdfPage();
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;
                doc.Open();

                 PdfPTable table0 = new PdfPTable(1);
                float[] colWidths24 = { 140 };
                table0.SetWidths(colWidths24);
                page.strRptMode = "Duevacate";
                PdfPCell cell21 = new PdfPCell(new Phrase("Room Due for Vacating at " + txtTime.Text + " on " + txtreportdatefrom.Text, font12));
                cell21.Colspan = 5;
                cell21.Border = 1;
                cell21.HorizontalAlignment = 1;
                table0.AddCell(cell21);
                doc.Add(table0);

                OdbcCommand ddh = new OdbcCommand();
                ddh.Parameters.AddWithValue("tblname", "m_sub_season s,m_season m");
                ddh.Parameters.AddWithValue("attribute", "distinct  s.season_sub_id, s.seasonname");
                ddh.Parameters.AddWithValue("conditionv", "s.rowstatus <> 2 and s.season_sub_id=m.season_sub_id AND startdate<='" + objcls.yearmonthdate(txtreportdatefrom.Text) + "' AND enddate>='" + objcls.yearmonthdate(txtreportdateto.Text) + "'");
                DataTable dttf = new DataTable();
                dttf = objcls.SpDtTbl("call selectcond(?,?,?)", ddh);

                PdfPTable tablep = new PdfPTable(2);
                float[] colWidths23 = { 70,70};

                PdfPCell cellv = new PdfPCell(new Phrase("Season: " + dttf.Rows[0]["seasonname"].ToString(), font9));
               // cellv.Colspan = 2;
                cellv.Border = 0;
                cellv.HorizontalAlignment = 0;
                tablep.AddCell(cellv);

                PdfPCell cellv2 = new PdfPCell(new Phrase("Date: " + txtreportdatefrom.Text + "\n \n", font9));
                //cellv2.Colspan = 2;
                cellv2.Border = 0;
                cellv2.HorizontalAlignment = 2;
                tablep.AddCell(cellv2);

                doc.Add(tablep);

                PdfPTable table = new PdfPTable(6);
                float[] colWidths234 = { 10, 40, 30, 30, 30, 30 };

                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font8)));
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Building & Room No", font8)));
                table.AddCell(cell2);
                
                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Checkout Time", font8)));
                table.AddCell(cell3);

                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Inmates Name", font8)));
                table.AddCell(cell6);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Ph No", font8)));
                table.AddCell(cell5);

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                table.AddCell(cell4);
               
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
                        PdfPTable table1 = new PdfPTable(6);
                        float[] colWidths231 = { 10, 40, 30, 30, 30, 30 };
                        table1.SetWidths(colWidths231);

                        PdfPCell cell1n = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                        table1.AddCell(cell1n);

                        PdfPCell cell2n = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                        table1.AddCell(cell2n);

                        PdfPCell cell3n = new PdfPCell(new Phrase(new Chunk("Checkout Time", font8)));
                        table1.AddCell(cell3n);

                        PdfPCell cell6n = new PdfPCell(new Phrase(new Chunk("Inmates Name", font8)));
                        table.AddCell(cell6n);

                        PdfPCell cell5n = new PdfPCell(new Phrase(new Chunk("Ph No", font8)));
                        table1.AddCell(cell5n);

                        PdfPCell cell4n = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                        table1.AddCell(cell4n);                        
                        doc.Add(table1);
                    }
                    PdfPTable table3 = new PdfPTable(6);

                    #region  formate
                    float[] colWidths23u = { 10, 20, 20, 30, 20, 30 };
                    table3.SetWidths(colWidths23u);
                    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table3.AddCell(cell9);

                    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk(dttf.Rows[0]["seasonname"].ToString(), font8)));
                    table3.AddCell(cell10);
                    DateTime dated = DateTime.Parse(dr["vacatedate"].ToString());
                    string time1 = dated.ToString("dd-MM-yyyy hh:mm tt");

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(time1.ToString(), font8)));
                    table3.AddCell(cell11);

                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(date10x1.ToString(), font8)));
                    table3.AddCell(cell12);

                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Over Stay", font8)));
                    table3.AddCell(cell13);
                    i++;
                    #endregion

                    doc.Add(table3);
                }
                PdfPTable table4 = new PdfPTable(5);
                PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                cellf.HorizontalAlignment = Element.ALIGN_LEFT;
                cellf.PaddingLeft = 20;
                cellf.MinimumHeight = 20;
                cellf.Colspan = 5;
                cellf.Border = 0;
                table4.AddCell(cellf);

                PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
                cellf1.PaddingLeft = 20;
                cellf1.Border = 0;
                cellf1.Colspan = 5;
                table4.AddCell(cellf1);

                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom", font8)));
                cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
                cellh2.PaddingLeft = 20;
                cellh2.Border = 0;
                cellh2.Colspan = 5;
                table4.AddCell(cellh2);
                doc.Add(table4);
                doc.Close();

                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Nonvacating report";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);
            }
            catch (Exception ex)
            {
                okmessage("problem found", "aa");
            }
        }
        else
        {
            okmessage("Select Building and enter Time to take report", "ww");
        }

    }
    # endregion

    protected void lnkreservedunoccupied_Click(object sender, EventArgs e)
    {         
        try
        {
            lblmessage.Visible = false;
            string str1 = objcls.yearmonthdate(txtreportdatefrom.Text);
            string str2 = objcls.yearmonthdate(txtreportdateto.Text);
            string place;
            DataTable dt = new DataTable();
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", " m_room r,m_sub_building b,t_roomallocation s,t_roomreservation t left join t_donorpass p on t.pass_id=p.pass_id ");
            cmd31.Parameters.AddWithValue("attribute", "DISTINCT t.room_id,t.place,t.reservedate 'Reserve from',t.expvacdate 'Reserve To',b.buildingname 'Building',r.roomno 'Room No',case reserve_mode when 'tdb' then 'TDB Res' when 'Donor Free' then 'Donor free' when 'Donor Paid' then 'Donor paid' when 'General' then 'General' END as 'Customer Type',passno,t.swaminame,CASE s.roomstatus WHEN '1' THEN 'NOT CHECKED IN' WHEN '2' THEN 'CHECKED IN' ELSE 'UNKNOWN STATUS' END AS 'Status'");
            if (cmbReportpass.SelectedValue == "-1")
            {
                cmd31.Parameters.AddWithValue("conditionv", " t.room_id=r.room_id and r.build_id=b.build_id and status_reserve='0'   and date(reservedate) >= '" + str1.ToString() + "' and date(reservedate) <  '" + str2.ToString() + "'  order by r.room_id  asc");
            }
            else
            {
                cmd31.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and r.build_id=b.build_id and status_reserve='0' and reserve_mode='" + cmbReportpass.SelectedValue + "'  and date(reservedate) >= '" + str1.ToString() + "' and date(reservedate) <  '" + str2.ToString() + "'  order by r.room_id  asc");
            }
            dt = objcls.SpDtTbl("call selectcond(?,?,?)", cmd31);
            Session["dataval"] = dt;
            if (dt.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = false;
                lblOk.Text = "No Details found";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                return;
            }
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string dat = gh.ToString("dd-MM-yyyy");
            // creating a  file to save the report .... setting its font
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);
            string ch = "Reservedbutunoccupied" + transtim.ToString() + ".pdf";
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            //  string pdfFilePath = Server.MapPath(".") + "/pdf/Reservationchart.pdf";
            Font font6 = FontFactory.GetFont("Arial", 8);
            Font font8 = FontFactory.GetFont("Arial", 9);
            Font font10 = FontFactory.GetFont("Arial", 10, 1);

            # region  report table coloumn and header settings
            pdfPage page = new pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            PdfPTable table1 = new PdfPTable(6);
            float[] colwidth = { 3, 13, 11, 11, 6, 7 };    
            table1.SetWidths(colwidth);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Reserved But Unoccupied Room List on " + txtreportdatefrom.Text + "\n\n", font10)));
            cell.Colspan = 7;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);

            # endregion

            # region giving heading for each coloumn in report

            PdfPCell cell01 = new PdfPCell(new Phrase(new Chunk("No", font8)));
            table1.AddCell(cell01);

            PdfPCell cell07 = new PdfPCell(new Phrase(new Chunk("Devotee Name & Address", font8)));
            table1.AddCell(cell07);

            PdfPCell cell06 = new PdfPCell(new Phrase(new Chunk("Proposed Check in date & time", font8)));
            table1.AddCell(cell06);

            PdfPCell cell078 = new PdfPCell(new Phrase(new Chunk("Proposed Check out date & time", font8)));
            table1.AddCell(cell078);          

            PdfPCell cell03 = new PdfPCell(new Phrase(new Chunk("Res Type", font8)));
            table1.AddCell(cell03);

            PdfPCell cell05 = new PdfPCell(new Phrase(new Chunk("Building and Room No", font8)));
            table1.AddCell(cell05);

            doc.Add(table1);

            # endregion

            # region adding data to the report file
            int slno = 0;
            int i = 0, j = 0;
            Session["dataval"] = dt;
            foreach (DataRow dr in dt.Rows)
            {
                PdfPTable table = new PdfPTable(6);
                float[] colwidth1 = { 3, 13, 11, 11, 6, 7 };
                table.SetWidths(colwidth1);

                if (i > 43)// total rows on page
                {
                    doc.NewPage();

                    # region giving heading for each coloumn in report

                    PdfPCell cell1d = new PdfPCell(new Phrase(new Chunk("Reserved But Unoccupied Room List on " + transtim, font10)));
                    cell1d.Colspan = 7;
                    cell1d.Border = 1;
                    cell1d.HorizontalAlignment = 1;
                    table.AddCell(cell1d);

                    PdfPCell cella1 = new PdfPCell(new Phrase(new Chunk("Date: " + transtim, font10)));
                    cella1.Colspan = 7;
                    cella1.Border = 1;
                    cella1.HorizontalAlignment = 0;
                    table.AddCell(cella1);

                    PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font8)));
                    table.AddCell(cell1);

                    PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Devotee Name & Address", font8)));
                    table.AddCell(cell2);

                    PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Proposed Check in date & time", font8)));
                    table.AddCell(cell3);

                    PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Proposed Check in date & time", font8)));
                    table.AddCell(cell4);

                    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Res Type", font8)));
                    table.AddCell(cell5);

                    PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Building and Room No", font8)));
                    table.AddCell(cell7);                                  

                    doc.Add(table);

                    # endregion

                    i = 0; // reseting count for new page
                    j = 0;

                }
                slno = slno + 1;

                PdfPTable table2 = new PdfPTable(6);
                float[] colwidth2 = { 3, 13, 11, 11, 6, 7 };
                table2.SetWidths(colwidth2);

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font6)));
                table2.AddCell(cell11);

                place = dr["place"].ToString();
                PdfPCell cell17g = new PdfPCell(new Phrase(new Chunk(dr["swaminame"].ToString() + "," + "" + place, font6)));
                table2.AddCell(cell17g);

                DateTime dt5 = DateTime.Parse(dr["Reserve From"].ToString());
                string date1 = dt5.ToString("dd-MM-yyyy hh:mm tt");

                PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font6)));
                table2.AddCell(cell28);

                DateTime dt55 = DateTime.Parse(dr["Reserve To"].ToString());
                string date2 = dt55.ToString("dd-MM-yyyy hh:mm tt");


                PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(date2.ToString(), font6)));
                table2.AddCell(cell29); 

                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["Customer Type"].ToString(), font6)));
                table2.AddCell(cell16);

                build = "";
                building = dr["Building"].ToString();
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

                PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(building + "/" + "" + dr["Room No"].ToString(), font6)));
                table2.AddCell(cell17);                                                                                            
                i++;
                doc.Add(table2);
            }
            doc.Close();
            # endregion

            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        catch (Exception es)
        {
            string sss = es.Message;
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
            return;
        }
    
    }
    protected void lnkonline_Click(object sender, EventArgs e)
    {
        if ((txtfromd0.Text != "") && (txttod0.Text != ""))
        {
            DateTime reporttime = DateTime.Now;
            string report = "Online reserved room allotted list" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;

            Font font8 = FontFactory.GetFont("ARIAL", 6);
            Font font82 = FontFactory.GetFont("ARIAL", 13, 1);
            Font font80 = FontFactory.GetFont("ARIAL", 7);
            Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 8, 1);

            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();
            string dates = @"SELECT @COUNT:=@COUNT+1 AS id,DATE_FORMAT('" + objcls.yearmonthdate(txtfromd0.Text) + "' + INTERVAL a + b DAY,'%d-%m-%Y') dte"
                                  + " FROM"
                                   + " (SELECT 0 a UNION SELECT 1 a UNION SELECT 2 UNION SELECT 3"
                                   + "  UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7"
                                    + "   UNION SELECT 8 UNION SELECT 9 ) d,"
                                 + "  (SELECT 0 b UNION SELECT 10 UNION SELECT 20 "
                                  + "   UNION SELECT 30 UNION SELECT 40 UNION SELECT 50 UNION SELECT 60 UNION SELECT 70 UNION SELECT 80 UNION SELECT 90 UNION SELECT 100 UNION SELECT 110 UNION SELECT 120 UNION SELECT 130 UNION SELECT 140 UNION  SELECT 150 ) m,"
                                  + "   (SELECT @COUNT:=0) AS COUNT"
                                 + "  WHERE  DATE_FORMAT('" + objcls.yearmonthdate(txtfromd0.Text) + "','%Y-%m-%d')  + INTERVAL a + b DAY  <=  DATE_FORMAT('" + objcls.yearmonthdate(txttod0.Text) + "','%Y-%m-%d')"
                                + " ORDER BY a + b ";
            DataTable dtdates = objcls.DtTbl(dates);
            double rentot;
            double restot;
            if (dtdates.Rows.Count > 0)
            {
                for (int j = 0; j < dtdates.Rows.Count; j++)
                {
                    rentot = 0.0;
                    restot = 0.0;
                    string report1 = @"SELECT (@COUNT:=@COUNT+1) AS 'Sl no',abc.Resno AS 'Id no',abc.mode AS 'Mode',abc.name AS 'Devotename',abc.place AS 'Place',DATE_FORMAT(abc.indate,'%d/%m/%Y') AS 'Checkindate',DATE_FORMAT(abc.outdate,'%d/%m/%Y') AS 'Checkoutdate',abc.paymode AS 'Payment mode',abc.rent AS 'Rent',abc.reservecharge AS 'Reservecharge',abc.onlinepaid AS 'Onlinepaid',abc.counterpaid AS 'Counterpaid',abc.receipt AS 'Receipt No',abc.allocid AS 'allocid'   
                          FROM (SELECT @COUNT:=0) AS COUNT,(SELECT  t_roomreservation.reserve_no AS Resno,
		                    (CASE t_roomreservation.reserve_mode WHEN 'Donor' THEN 'Donor Paid' ELSE t_roomreservation.reserve_mode END) AS MODE,    
	                        t_roomreservation.swaminame AS NAME,
	                        t_roomreservation.place AS place,   
	                        m_reserve_userdetails.indate AS indate,
	                        t_roomallocation.exp_vecatedate  AS outdate, 
	                        payment_mode.payment_mode AS paymode,
	                        m_reserve_userdetails.rent AS rent,
	                        m_reserve_userdetails.reserve_charge reservecharge,                 
                            m_reserve_userdetails.total AS onlinepaid,  
                            m_reserve_userdetails.sec_deposit AS counterpaid,                                                        
                            t_roomallocation.adv_recieptno AS receipt,
                            t_roomallocation.alloc_id AS allocid                                          
                            FROM m_reserve_userdetails
                            INNER JOIN payment_mode ON m_reserve_userdetails.payment_mode=payment_mode.payment_id      
                            LEFT JOIN t_roomreservation ON m_reserve_userdetails.res_no=t_roomreservation.reserve_no
                            INNER JOIN t_roomallocation ON t_roomallocation.reserve_id=t_roomreservation.reserve_id                                                                     
                            WHERE reserve_no LIKE '9R%' AND m_reserve_userdetails.indate='" + objcls.yearmonthdate(dtdates.Rows[j][1].ToString()) + "' GROUP BY t_roomallocation.alloc_no"
                                        + " UNION   "
                                        + " SELECT  m_reserve_userdetails.res_no AS Resno,"
                                        + " 'Donor Free' AS MODE,"
                                        + " t_roomallocation.swaminame AS NAME, "
                                        + " t_roomallocation.place AS place,   "
                                        + " m_reserve_userdetails.indate AS indate,  "
                                        + " t_roomallocation.exp_vecatedate  AS outdate,  "
                                        + " payment_mode.payment_mode AS paymode,   "
                                        + " m_reserve_userdetails.rent AS rent,"
                                + " m_reserve_userdetails.reserve_charge reservecharge, "
                                + " m_reserve_userdetails.total AS onlinepaid, "
                                + " m_reserve_userdetails.sec_deposit AS counterpaid, "
                                + " t_roomallocation.adv_recieptno AS receipt,"
                                + " t_roomallocation.alloc_id AS allocid     "
                                + " FROM m_reserve_userdetails "
                                + " INNER JOIN t_donorpass ON m_reserve_userdetails.passno=t_donorpass.passno "
                                + " INNER JOIN t_roomallocation ON t_donorpass.pass_id=t_roomallocation.pass_id "
                                + " INNER JOIN payment_mode ON m_reserve_userdetails.payment_mode=payment_mode.payment_id    "
                                + " WHERE t_donorpass.passtype=0 AND m_reserve_userdetails.type_id=9 AND"
                                + " m_reserve_userdetails.indate ='" + objcls.yearmonthdate(dtdates.Rows[j][1].ToString()) + "' GROUP BY t_roomallocation.alloc_no UNION"
                                + " SELECT  t_roomreservation.reserve_no AS Resno,"
                                + " (CASE t_roomreservation.reserve_mode WHEN 'Donor' THEN 'Donor Paid' ELSE t_roomreservation.reserve_mode END) AS MODE, "
                                + " t_roomreservation.swaminame AS NAME, "
                                + " t_roomreservation.place AS place, "
                                + " m_reserve_userdetails.indate AS indate, "
                                + " t_roomallocation.exp_vecatedate  AS outdate,"
                                + " payment_mode.payment_mode AS paymode,"
                                + " m_reserve_userdetails.rent AS rent,"
                                + " m_reserve_userdetails.reserve_charge reservecharge,  "
                                + " m_reserve_userdetails.total AS onlinepaid, "
                                + " m_reserve_userdetails.sec_deposit AS counterpaid,  "
                                + " t_roomallocation.adv_recieptno AS receipt,"
                                + " t_roomallocation.alloc_id AS allocid   "
                                + " FROM m_reserve_userdetails "
                                + " LEFT JOIN t_roomreservation ON m_reserve_userdetails.res_no=t_roomreservation.reserve_no"
                                + " INNER JOIN t_clubdetails ON t_clubdetails.reserve_id=t_roomreservation.reserve_id"
                                + " INNER JOIN t_roomallocation ON t_roomallocation.alloc_id=t_clubdetails.alloc_id     "
                                + " INNER JOIN payment_mode ON m_reserve_userdetails.payment_mode=payment_mode.payment_id   "
                                + " WHERE reserve_no LIKE '9R%' AND  m_reserve_userdetails.indate ='" + objcls.yearmonthdate(dtdates.Rows[j][1].ToString()) + "' GROUP BY t_roomallocation.alloc_no"
                                + " ) AS abc "
                                + " ORDER BY abc.indate";
                    DataTable dtreprt = objcls.DtTbl(report1);
                    if (dtreprt.Rows.Count > 0)
                    {
                        DataTable dt_dts = objcls.DtTbl("SELECT DATE_FORMAT('" + objcls.yearmonthdate(dtdates.Rows[j][1].ToString()) + "','%D %M %Y')");
                        PdfPTable table0 = new PdfPTable(1);
                        float[] colWidths1x = { 100 };
                        table0.SetWidths(colWidths1x);

                        PdfPCell cellt01 = new PdfPCell(new Phrase(new Chunk("")));
                        cellt01.Border = 0;
                        cellt01.HorizontalAlignment = 1;
                        table0.AddCell(cellt01);

                        PdfPCell cellt0 = new PdfPCell(new Phrase(new Chunk("ONLINE RESERVED ROOM ALLOTTED LIST ON -" + dt_dts.Rows[0][0].ToString(), fontLB)));
                        cellt0.Border = 0;
                        cellt0.HorizontalAlignment = 1;
                        table0.AddCell(cellt0);

                        PdfPCell cellt0x = new PdfPCell(new Phrase(new Chunk("")));
                        cellt0x.Border = 0;
                        cellt0x.HorizontalAlignment = 1;
                        table0.AddCell(cellt0x);

                        doc.Add(table0);

                        PdfPTable table1 = new PdfPTable(13);
                        float[] colWidths1 = { 15, 30, 20, 35, 25, 25, 25, 20, 20, 20, 20, 20, 20 };
                        table1.SetWidths(colWidths1);

                        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("SlNo.", font10)));
                        cell1.HorizontalAlignment = 1;
                        table1.AddCell(cell1);

                        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Reservation No.", font10)));
                        cell2.HorizontalAlignment = 1;
                        table1.AddCell(cell2);

                        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Mode", font10)));
                        cell3.HorizontalAlignment = 1;
                        table1.AddCell(cell3);

                        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Devotee name", font10)));
                        cell4.HorizontalAlignment = 1;
                        table1.AddCell(cell4);

                        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Place", font10)));
                        cell5.HorizontalAlignment = 1;
                        table1.AddCell(cell5);

                        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Reservation date", font10)));
                        cell6.HorizontalAlignment = 1;
                        table1.AddCell(cell6);

                        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Vacate date", font10)));
                        cell7.HorizontalAlignment = 1;
                        table1.AddCell(cell7);

                        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Payment mode", font10)));
                        cell8.HorizontalAlignment = 1;
                        table1.AddCell(cell8);

                        PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font10)));
                        cell9.HorizontalAlignment = 1;
                        table1.AddCell(cell9);

                        PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Reserve charge", font10)));
                        cell10.HorizontalAlignment = 1;
                        table1.AddCell(cell10);

                        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Amount paid (online)", font10)));
                        cell11.HorizontalAlignment = 1;
                        table1.AddCell(cell11);

                        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Balance (counter)", font10)));
                        cell12.HorizontalAlignment = 1;
                        table1.AddCell(cell12);

                        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Receipt No.", font10)));
                        cell13.HorizontalAlignment = 1;
                        table1.AddCell(cell13);

                        doc.Add(table1);

                        for (int i = 0; i < dtreprt.Rows.Count; i++)
                        {
                            PdfPTable table2 = new PdfPTable(13);
                            float[] colWidths11 = { 15, 30, 20, 35, 25, 25, 25, 20, 20, 20, 20, 20, 20 };
                            table2.SetWidths(colWidths11);

                            PdfPCell cell1x = new PdfPCell(new Phrase(new Chunk(dtreprt.Rows[i][0].ToString(), font8)));
                            cell1x.HorizontalAlignment = 0;
                            table2.AddCell(cell1x);

                            PdfPCell cell2x = new PdfPCell(new Phrase(new Chunk(dtreprt.Rows[i][1].ToString(), font8)));
                            cell2x.HorizontalAlignment = 0;
                            table2.AddCell(cell2x);

                            PdfPCell cell3x = new PdfPCell(new Phrase(new Chunk(dtreprt.Rows[i][2].ToString(), font8)));
                            cell3x.HorizontalAlignment = 0;
                            table2.AddCell(cell3x);

                            PdfPCell cell4x = new PdfPCell(new Phrase(new Chunk(dtreprt.Rows[i][3].ToString(), font8)));
                            cell4x.HorizontalAlignment = 0;
                            table2.AddCell(cell4x);

                            PdfPCell cell5x = new PdfPCell(new Phrase(new Chunk(dtreprt.Rows[i][4].ToString(), font8)));
                            cell5x.HorizontalAlignment = 0;
                            table2.AddCell(cell5x);

                            PdfPCell cell6x = new PdfPCell(new Phrase(new Chunk(dtreprt.Rows[i][5].ToString(), font8)));
                            cell6x.HorizontalAlignment = 0;
                            table2.AddCell(cell6x);

                            PdfPCell cell7x = new PdfPCell(new Phrase(new Chunk(dtreprt.Rows[i][6].ToString(), font8)));
                            cell7x.HorizontalAlignment = 0;
                            table2.AddCell(cell7x);

                            PdfPCell cell8x = new PdfPCell(new Phrase(new Chunk(dtreprt.Rows[i][7].ToString(), font8)));
                            cell8x.HorizontalAlignment = 0;
                            table2.AddCell(cell8x);

                            PdfPCell cell9x = new PdfPCell(new Phrase(new Chunk(dtreprt.Rows[i][8].ToString(), font8)));
                            cell9x.HorizontalAlignment = 0;
                            table2.AddCell(cell9x);

                            PdfPCell cell10x = new PdfPCell(new Phrase(new Chunk(dtreprt.Rows[i][9].ToString(), font8)));
                            cell10x.HorizontalAlignment = 0;
                            table2.AddCell(cell10x);

                            PdfPCell cell11x = new PdfPCell(new Phrase(new Chunk(dtreprt.Rows[i][10].ToString(), font8)));
                            cell11x.HorizontalAlignment = 0;
                            table2.AddCell(cell11x);

                            PdfPCell cell12x = new PdfPCell(new Phrase(new Chunk(dtreprt.Rows[i][11].ToString(), font8)));
                            cell12x.HorizontalAlignment = 0;
                            table2.AddCell(cell12x);

                            PdfPCell cell13x = new PdfPCell(new Phrase(new Chunk(dtreprt.Rows[i][12].ToString(), font8)));
                            cell13x.HorizontalAlignment = 0;
                            table2.AddCell(cell13x);

                            doc.Add(table2);
                            rentot = rentot + Convert.ToDouble(dtreprt.Rows[i][8].ToString());
                            restot = restot + Convert.ToDouble(dtreprt.Rows[i][9].ToString());
                        }

                        PdfPTable table56 = new PdfPTable(3);
                        float[] colWidths1x1 = { 33, 33, 33 };
                        table56.SetWidths(colWidths1x1);

                        PdfPCell cellt011 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cellt011.Border = 0;
                        cellt011.HorizontalAlignment = 1;
                        table56.AddCell(cellt011);

                        PdfPCell cellt012 = new PdfPCell(new Phrase(new Chunk("Total rent : " + rentot.ToString(), font10)));
                        cellt012.Border = 0;
                        cellt012.HorizontalAlignment = 1;
                        table56.AddCell(cellt012);

                        PdfPCell cellt013 = new PdfPCell(new Phrase(new Chunk("Total reservation charge : " + restot.ToString(), font10)));
                        cellt013.Border = 0;
                        cellt013.HorizontalAlignment = 1;
                        table56.AddCell(cellt013);
                        doc.Add(table56);
                    }
                    else
                    {

                    }
                }

            }
            doc.Close();

            doc.Close();
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + report + ".pdf");
            Response.TransmitFile(pdfFilePath);
            Response.Flush();
            //Random r = new Random();
            //string PopUpWindowPage = "print.aspx?reportname=" + report.ToString() + "";
            //string Script = "";
            //Script += "<script id='PopupWindow'>";
            //Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            //Script += "confirmWin.Setfocus()</script>";
            //if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            //    Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        else
        {
            
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        if (ViewState["click"].ToString() == "no")
        {
            string select = @"SELECT  CAST(DATE_FORMAT( DATE,'%d-%m-%Y') AS CHAR(12)) AS 'Date',counter_no AS 'Counter',m_sub_counter.counter_ip AS 'Counter IP',amount AS 'Amount' FROM t_daily_transaction 
INNER JOIN m_sub_counter ON m_sub_counter.counter_id = t_daily_transaction.counter_id,m_season
WHERE DATE BETWEEN m_season.startdate AND m_season.enddate AND ledger_id='1' AND season_id='" + Session["season"].ToString() + "' GROUP BY DATE,t_daily_transaction.counter_id";
            DataTable dt_select = objcls.DtTbl(select);
            if (dt_select.Rows.Count > 0)
            {
                gvview.DataSource = "";
                gvview.DataBind();
                gvview.DataSource = dt_select;
                gvview.DataBind();
                gvview.Visible = true;
                ViewState["click"] = "yes";
            }
            else
            {
                ViewState["click"] ="no";
                gvview.DataSource = "";
                gvview.DataBind();
                gvview.Visible = false;
                obc.ShowAlertMessage(this,"no details found");
            }
        }
        else
        {
            ViewState["click"] = "no";
            gvview.DataSource = "";
            gvview.DataBind();
            gvview.Visible = false;
        }
    
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        if (ViewState["clkremit"].ToString() == "no")
        {
            string select = @"SELECT  CAST(DATE_FORMAT(dayend,'%d-%m-%Y') AS CHAR(12)) AS 'Date',amountRemitted AS 'Amount remit' FROM  t_ledgerremitted,m_season
WHERE dayend BETWEEN m_season.startdate AND m_season.enddate  AND season_id='" + Session["season"].ToString() + "' ";
            DataTable dt_select = objcls.DtTbl(select);
            if (dt_select.Rows.Count > 0)
            {
                gvview.DataSource = "";
                gvview.DataBind();
                gvview.DataSource = dt_select;
                gvview.DataBind();
                gvview.Visible = true;
              
                ViewState["clkremit"] = "yes";
            }
            else
            {
                ViewState["clkremit"] = "no";
                gvview.DataSource = "";
                gvview.DataBind();
                gvview.Visible = false;
                obc.ShowAlertMessage(this, "no details found");
            }
        }
        else
        {
            ViewState["clkremit"] = "no";
            gvview.DataSource = "";
            gvview.DataBind();
            gvview.Visible = false;
        }
    }
}
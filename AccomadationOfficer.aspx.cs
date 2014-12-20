using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Num2Wrd;
using PDF;
public partial class _Default : System.Web.UI.Page
{
    # region initialization
    commonClass objcls = new commonClass();
    OdbcConnection conn = new OdbcConnection();
    OdbcConnection con = new OdbcConnection();
    static string strConnection;
    string mm, dd, yy, gg;

    DateTime dt;
    Decimal rrent = 0, rrent1 = 0, rdeposit = 0, rdeposit1 = 0, gtr, gtd;
    string d, y, m, g, rr, dde;
    int id;
    string strsql3, granttotalrent, granttotaldeposit, seasonname;
    string curseason, malYear, season;
    string remarks;

    string name, place, building, room, indate, rents, deposits, num, stat, rec, outdate, states, dist, allocfrom, reason;
    int no = 0, transno;
    DateTime indat, outdat;
    string alloctype, passno, mpass;
    string rrr;
    string ind, outd, it, ot, build;
    string mal;
    string reporttime, report, Sname, f1;
    int Mal, NrId, Sea_Id, Seas, k, D;
    DateTime yee;

    string number;
    int slno = 0;

    int misrec, miss;

    string frmdate, fromtime, totime, reson, toodate, f;
    DateTime fromdate, todate;
    # endregion

    # region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        //clsCommon obj = new clsCommon();
        //strConnection = obj.ConnectionString();
        if (!Page.IsPostBack)
        {
            ViewState["action"] = "Nill";

            //check();   Name of form not in tsunamiarms
            OdbcCommand cmd46 = new OdbcCommand();
            cmd46.Parameters.AddWithValue("tblname", "t_dayclosing");
            cmd46.Parameters.AddWithValue("attribute", "closedate_start");
            cmd46.Parameters.AddWithValue("conditionv", "daystatus='open'");
            //OdbcDataAdapter dacnt46 = new OdbcDataAdapter(cmd46);
            DataTable dtt46 = new DataTable();
            dtt46 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd46);
            DateTime dt = DateTime.Parse(dtt46.Rows[0][0].ToString());
            string dtdd = dt.ToString("yyyy/MM/dd");
            Session["dayend"] = dtdd.ToString();
            txtdate.Text = dt.ToString("dd/MM/yyyy");


        }
    }

    # endregion

    #region Authentication Check function
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("NameNotinDB", level) == 0)
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


    protected void btnRM_Click(object sender, EventArgs e)
    {
        pnlCollection.Visible = false;
        Server.Transfer("Room Management.aspx");
    }
    protected void btnRalloc_Click(object sender, EventArgs e)
    {
        pnlCollection.Visible = false;
        Server.Transfer("roomallocation.aspx");
    }
    protected void btnVacBill_Click(object sender, EventArgs e)
    {
        pnlCollection.Visible = false;
        Server.Transfer("vacating and billing.aspx");
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        pnlCollection.Visible = false;
        Server.Transfer("Room Reservation.aspx");

    }
    # region Collection report
    protected void btnCollection_Click(object sender, EventArgs e)
    {
        btnCollection.BackColor = System.Drawing.Color.Bisque;
        pnlAccomodation.Visible = false;
        pnlReservation.Visible = false;
        pnlRoomStatus.Visible = false;
        pnlRoomstatusReport.Visible = false;
        pnlDonorpass.Visible = false;
        pnlNonvacating.Visible = false;
        pnlcollectioncomp.Visible = false;
        pnlCollection.Visible = true;

        //string sq1 = "select closedate_start from t_dayclosing  where daystatus='" + "open" + "' order by  closedate_start  desc limit 0,1";


        OdbcCommand sq1 = new OdbcCommand();
        sq1.Parameters.AddWithValue("tblname", "t_dayclosing");
        sq1.Parameters.AddWithValue("attribute", "closedate_start");
        sq1.Parameters.AddWithValue("conditionv", "daystatus='" + "open" + "' order by  closedate_start  desc limit 0,1");


        DataTable dtsq1 = new DataTable();
        dtsq1 = objcls.SpDtTbl("call selectcond(?,?,?)", sq1);
        DateTime dttt = new DateTime();
        string datetodayh = "";
        string date12 = "";
        int CollectedAmount = 0, CollectableAmount = 0;

        if (dtsq1.Rows.Count > 0)
        {
            dttt = DateTime.Parse(dtsq1.Rows[0]["closedate_start"].ToString());
            date12 = dttt.ToString("dd-MM-yyyy");
            datetodayh = objcls.yearmonthdate(date12);

        }

        // string sq2 = "select  sum(amount)  amount from t_daily_transaction where ledger_id='1' and liability_type='0' and  date='" + datetodayh + "'";

        OdbcCommand sq2 = new OdbcCommand();
        sq2.Parameters.AddWithValue("tblname", "t_daily_transaction");
        sq2.Parameters.AddWithValue("attribute", "sum(amount)  amount");
        sq2.Parameters.AddWithValue("conditionv", "ledger_id='1' and liability_type='0' and  date='" + datetodayh + "'");



        DataTable dtsq2 = new DataTable();
        dtsq2 = objcls.SpDtTbl("call selectcond(?,?,?)", sq2);

        if (dtsq2.Rows.Count > 0)
        {
            if (Convert.IsDBNull(dtsq2.Rows[0]["amount"]) == false)
            {

                CollectedAmount = Convert.ToInt32(dtsq2.Rows[0]["amount"]);

            }
        }

        OdbcCommand cmd2051 = new OdbcCommand(); //"CALL selectcond(?,?,?)", conn);
        cmd2051.Parameters.AddWithValue("tblname", "m_room rm,m_sub_room_category rc");
        cmd2051.Parameters.AddWithValue("attribute", "rc.rent");
        cmd2051.Parameters.AddWithValue("conditionv", "rc.room_cat_id=rm.room_cat_id and roomstatus=1  and rm.rowstatus!=2 order by room_id");
        DataTable dtt2051 = new DataTable();
        dtt2051 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2051);

        int sum = 0;
        for (int i = 0; i < dtt2051.Rows.Count; i++)
        {

            sum = sum + Convert.ToInt32(dtt2051.Rows[i]["rent"]);

        }

        CollectableAmount = CollectedAmount + sum;
        pnlCollection.Visible = true;
        Label1.Text = "Total possible collection for the day " + date12 + " is:";
        Label2.Text = CollectableAmount.ToString();
        btnReservationChart.BackColor = System.Drawing.Color.Plum;
        btnAccomodationLedger.BackColor = System.Drawing.Color.Plum;
        btnRoomStatus.BackColor = System.Drawing.Color.Plum;
        btnRoomManagement.BackColor = System.Drawing.Color.Plum;
        btnNonvacating.BackColor = System.Drawing.Color.Plum;
        btnDonorPass.BackColor = System.Drawing.Color.Plum;
        btncollectioncomparison.BackColor = System.Drawing.Color.Plum;
        btnOtherReports.BackColor = System.Drawing.Color.Plum;

    }
    # endregion

    # region ACOMODATION lEDGER click
    protected void btnAccomodationLedger_Click(object sender, EventArgs e)
    {
        btnAccomodationLedger.BackColor = System.Drawing.Color.Bisque;

        pnlAccomodation.Visible = true;
        pnlReservation.Visible = false;
        pnlRoomStatus.Visible = false;
        pnlRoomstatusReport.Visible = false;
        pnlDonorpass.Visible = false;
        pnlNonvacating.Visible = false;
        pnlCollection.Visible = false;
        pnlcollectioncomp.Visible = false;

        btnReservationChart.BackColor = System.Drawing.Color.Plum;
        btnRoomManagement.BackColor = System.Drawing.Color.Plum;
        btnRoomStatus.BackColor = System.Drawing.Color.Plum;
        btnNonvacating.BackColor = System.Drawing.Color.Plum;
        btnDonorPass.BackColor = System.Drawing.Color.Plum;
        btncollectioncomparison.BackColor = System.Drawing.Color.Plum;
        btnCollection.BackColor = System.Drawing.Color.Plum;
        btnOtherReports.BackColor = System.Drawing.Color.Plum;


    }
    # endregion


    protected void btnReservationChart_Click(object sender, EventArgs e)
    {
        Server.Transfer("AllocReport.aspx");

    }
    # region Accomodation Ledger Between Date 

    protected void lnktotalallocseasonreport_Click(object sender, EventArgs e)
    {

        #region ledger from to
        //try
        //{
        //    if ((txtfromd.Text == "") || (txttod.Text == ""))
        //    {
        //        okmessage("Tsunami ARMS - Warning", "Enter dates");
        //        return;
        //    }

        //    string str1 = objcls.yearmonthdate(txtfromd.Text.ToString());
        //   // str1 = mm + "-" + dd + "-" + yy;
        //    string str2 = objcls.yearmonthdate(txttod.Text.ToString());
        //   // str2 = mm + "-" + dd + "-" + yy;
        //    DateTime ind = DateTime.Parse(str1);
        //    DateTime outd = DateTime.Parse(str2);
        //    if (outd < ind)
        //    {
        //        okmessage("Tsunami ARMS - Warning", "Check the dates");
        //        return;
        //    }

        //    DateTime rdate = DateTime.Now;
        //    string repdate = rdate.ToString("yyyy/MM/dd");
        //    string reptime = rdate.ToShortTimeString();

        //    int no = 0, i = 0;
        //    int currentyear = rdate.Year;


        //    string datf = objcls.yearmonthdate(txtfromd.Text);
        //    string datt = objcls.yearmonthdate(txttod.Text);

        //    DateTime daf1 = DateTime.Parse(datf.ToString());
        //    DateTime dat1 = DateTime.Parse(datt.ToString());

        //    string g1 = daf1.ToString("yyyy-MM-dd");
        //    string g2 = dat1.ToString("yyyy-MM-dd");

        //    string g3 = daf1.ToString("dd/MM/yy");
        //    string g4 = dat1.ToString("dd/MM/yy");
        //    string strsql1 = "m_room as room,"
        //            + "m_sub_building as build,"
        //            + "t_roomallocation as alloc"
        //            + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
        //            + " Left join m_sub_district as dist on alloc.district_id=dist.district_id  left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";

        //    string strsql2 = "alloc.alloc_id,"
        //                   + "alloc.alloc_no,"
        //                   + "alloc.pass_id,"
        //                   + "alloc.place,"
        //                   + "alloc.phone,"
        //                   + "alloc.idproof,"
        //                   + "alloc.alloc_type,"
        //                   + "alloc.idproofno,"
        //                   + "alloc.noofinmates,"
        //                   + "alloc.numberofunit,"
        //                   + "alloc.advance,"
        //                   + "alloc.reason,"
        //                   + "alloc.othercharge,"
        //                   + "alloc.adv_recieptno,"
        //                   + "alloc.swaminame,"
        //                   + "build.buildingname,"
        //                   + "room.roomno,"
        //                   + "alloc.allocdate,"
        //                   + "alloc.exp_vecatedate,"
        //                   + "alloc.roomrent,"
        //                   + "alloc.state_id,"
        //                   + "alloc.district_id,"
        //                   + "alloc.deposit,"
        //                   + "alloc.totalcharge,"
        //                   + "alloc.realloc_from,"
        //                   + "alloc.reason_id,"
        //                   + "actualvecdate";


        //    strsql3 = "alloc.room_id=room.room_id"
        //      + " and room.build_id=build.build_id"
        //      + " and alloc.dayend >= '" + datf + "' and alloc.dayend <= '" + datt + "' order by alloc.alloc_id asc";


        //    OdbcCommand cmd350 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
        //    cmd350.Parameters.AddWithValue("tblname", strsql1);
        //    cmd350.Parameters.AddWithValue("attribute", strsql2);
        //    cmd350.Parameters.AddWithValue("conditionv", strsql3);
        //    DataTable dtt350 = new DataTable();
        //    dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
        //    int cont = dtt350.Rows.Count;
        //    if (dtt350.Rows.Count == 0)
        //    {
        //        okmessage("Tsunami ARMS - Warning", "No details found");
        //        return;
        //    }

        //    DateTime reporttime = DateTime.Now;
        //    report = "Ledger From-To " + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

        //    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
        //    string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;

        //    Font font8 = FontFactory.GetFont("ARIAL", 9);
        //    Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
        //    Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        //    Font font10 = FontFactory.GetFont("ARIAL", 10, 1);

        //    pdfPage page = new pdfPage();
        //    page.strRptMode = "Allocation";
        //    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //    wr.PageEvent = page;

        //    doc.Open();
        //    PdfPTable table1 = new PdfPTable(9);
        //    float[] colWidths1 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
        //    table1.SetWidths(colWidths1);

        //    string repdates = rdate.ToString("dd/MM/yyyy");

        //    PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
        //    cell500.Colspan = 9;
        //    cell500.Border = 1;
        //    cell500.HorizontalAlignment = 1;
        //    table1.AddCell(cell500);

        //    PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
        //    cell501.Colspan = 5;
        //    cell501.Border = 0;
        //    cell501.HorizontalAlignment = 0;
        //    table1.AddCell(cell501);

        //    if (txtfromd.Text.ToString() == txttod.Text.ToString())
        //    {
        //        PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + g3.ToString(), font10)));
        //        cell502.Colspan = 4;
        //        cell502.Border = 0;
        //        cell502.HorizontalAlignment = 2;
        //        table1.AddCell(cell502);


        //    }
        //    else
        //    {

        //        PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + g3 + "-" + g4, font10)));
        //        cell502.Colspan = 4;
        //        cell502.Border = 0;
        //        cell502.HorizontalAlignment = 2;
        //        table1.AddCell(cell502);
        //    }

        //    PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        //    table1.AddCell(cell2);

        //    PdfPCell cell2d = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
        //    table1.AddCell(cell2d);


        //    PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
        //    table1.AddCell(cell3);

        //    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        //    table1.AddCell(cell5);

        //    PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
        //    table1.AddCell(cell7);

        //    PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
        //    table1.AddCell(cell8);

        //    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
        //    table1.AddCell(cell9);

        //    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
        //    table1.AddCell(cell10);

        //    PdfPCell cell11e = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
        //    table1.AddCell(cell11e);


        //    doc.Add(table1);
        //    i = 0;

        //    slno = 1;
        //    for (int ii = 0; ii < cont; ii++)
        //    {
        //        if (i > 25)
        //        {
        //            doc.NewPage();

        //            PdfPTable table2 = new PdfPTable(9);
        //            float[] colWidths2 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
        //            table2.SetWidths(colWidths2);

        //            PdfPCell cell500d = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
        //            cell500d.Colspan = 9;
        //            cell500d.Border = 1;
        //            cell500d.HorizontalAlignment = 1;
        //            table2.AddCell(cell500d);

        //            PdfPCell cell501d = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
        //            cell501d.Colspan = 5;
        //            cell501d.Border = 0;
        //            cell501d.HorizontalAlignment = 0;
        //            table2.AddCell(cell501d);

        //            if (txtfromd.Text.ToString() == txttod.Text.ToString())
        //            {
        //                PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + g3.ToString(), font10)));
        //                cell502.Colspan = 4;
        //                cell502.Border = 0;

        //                cell502.HorizontalAlignment = 2;
        //                table2.AddCell(cell502);


        //            }
        //            else
        //            {

        //                PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + g3 + "-" + g4, font10)));
        //                cell502.Colspan = 4;
        //                cell502.Border = 0;
        //                cell502.HorizontalAlignment = 2;
        //                table2.AddCell(cell502);
        //            }

        //            PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        //            table2.AddCell(cell22);

        //            PdfPCell cell2df = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
        //            table1.AddCell(cell2df);

        //            PdfPCell cell32 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
        //            table2.AddCell(cell32);

        //            PdfPCell cell52 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        //            table2.AddCell(cell52);

        //            PdfPCell cell72 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
        //            table2.AddCell(cell72);

        //            PdfPCell cell82 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
        //            table2.AddCell(cell82);

        //            PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
        //            table2.AddCell(cell92);

        //            PdfPCell cell102 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
        //            table2.AddCell(cell102);

        //            PdfPCell cell112e = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
        //            table1.AddCell(cell112e);
        //            i = 0;
        //            doc.Add(table2);
        //        }

        //        PdfPTable table = new PdfPTable(9);
        //        float[] colWidths ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
        //        table.SetWidths(colWidths);



        //        transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
        //        num = dtt350.Rows[ii]["alloc_no"].ToString();
        //        Session["num"] = num.ToString();
        //        name = dtt350.Rows[ii]["swaminame"].ToString();
        //        place = dtt350.Rows[ii]["place"].ToString();
        //        states = dtt350.Rows[ii]["state_id"].ToString();
        //        dist = dtt350.Rows[ii]["district_id"].ToString();
        //        rec = dtt350.Rows[ii]["adv_recieptno"].ToString();
        //        allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
        //        reason = dtt350.Rows[ii]["reason_id"].ToString();
        //        alloctype = dtt350.Rows[ii]["alloc_type"].ToString();


        //        #region extent remark&alter remark
        //        if (allocfrom != "")
        //        {
        //            if (reason != "")
        //            {

        //                OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
        //                cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
        //                cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
        //                cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
        //                DataTable dtallocfr = new DataTable();
        //                dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
        //                if (dtallocfr.Rows.Count > 0)
        //                {
        //                    remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
        //                }
        //            }
        //            else
        //            {
        //                OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
        //                cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
        //                cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
        //                cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
        //                DataTable dtallocfr = new DataTable();
        //                dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
        //                if (dtallocfr.Rows.Count > 0)
        //                {
        //                    remarks = "Ext: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
        //                }

        //            }
        //        }
        //        else
        //        {
        //            remarks = "";
        //        }
        //        #endregion

        //        #region donor remark
        //        if (alloctype == "Donor Free Allocation")
        //        {
        //            int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());
        //            OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
        //            cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
        //            cmd115.Parameters.AddWithValue("attribute", "passno");
        //            cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
        //            DataTable dtt115 = new DataTable();
        //            dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
        //            if (dtt115.Rows.Count > 0)
        //            {
        //                passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
        //                remarks = remarks + passno;
        //            }
        //        }
        //        else if (alloctype == "Donor Paid Allocation")
        //        {
        //            int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

        //            OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
        //            cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
        //            cmd115.Parameters.AddWithValue("attribute", "passno");
        //            cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
        //            DataTable dtt115 = new DataTable();
        //            dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
        //            if (dtt115.Rows.Count > 0)
        //            {
        //                passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
        //                remarks = remarks + passno;
        //            }
        //        }
        //        else if (alloctype == "Donor multiple pass")
        //        {
        //            //

        //            int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
        //            mpass = "";

        //            OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
        //            cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
        //            cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
        //            cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");
        //            DataTable dtt115 = new DataTable();
        //            dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
        //            for (int b = 0; b < dtt115.Rows.Count; b++)
        //            {
        //                string ptype = dtt115.Rows[b]["passtype"].ToString();
        //                if (ptype == "0")
        //                {
        //                    passno = "F P: " + dtt115.Rows[b]["passno"].ToString();
        //                    mpass = passno + "   " + mpass;
        //                }
        //                else if (ptype == "1")
        //                {
        //                    passno = "P P: " + dtt115.Rows[b]["passno"].ToString();
        //                    mpass = passno + "   " + mpass;
        //                }
        //            }
        //            remarks = remarks + mpass;
        //        }
        //        else
        //        {
        //        }
        //        #endregion


        //        build = "";
        //        building = dtt350.Rows[ii]["buildingname"].ToString();
        //        if (building.Contains("(") == true)
        //        {
        //            string[] buildS1, buildS2; ;
        //            buildS1 = building.Split('(');
        //            build = buildS1[1];
        //            buildS2 = build.Split(')');
        //            build = buildS2[0];
        //            building = build;
        //        }
        //        else if (building.Contains("Cottage") == true)
        //        {
        //            building = building.Replace("Cottage", "Cot");
        //        }

        //        room = dtt350.Rows[ii]["roomno"].ToString();

        //        Session["rec"] = rec.ToString();
        //        Session["tno"] = transno.ToString();

        //        indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
        //        string inds = indat.ToString("dd-MMM");
        //        it = indat.ToString("hh:mm:tt");
        //        indate = it + "       " + inds;

        //        if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
        //        {
        //            outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
        //            string outds = outdat.ToString("dd-MMM");
        //            ot = outdat.ToString("hh:mm:tt");
        //            outdate = ot + "       " + outds;
        //        }
        //        else
        //        {
        //            string cc = Convert.ToString(dtt350.Rows[ii]["actualvecdate"]);
        //            outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
        //            string outds = outdat.ToString("dd-MMM");
        //            ot = outdat.ToString("hh:mm:tt");
        //            outdate = ot + "       " + outds;

        //        }
        //        rents = dtt350.Rows[ii]["roomrent"].ToString();
        //        deposits = dtt350.Rows[ii]["deposit"].ToString();


        //        rrent1 = decimal.Parse(rents.ToString());
        //        rrent = rrent + rrent1;

        //        rr = rrent.ToString();
        //        rdeposit1 = decimal.Parse(deposits.ToString());
        //        rdeposit = rdeposit + rdeposit1;

        //        dde = rdeposit.ToString();

        //        string rrr = dtt350.Rows[ii]["adv_recieptno"].ToString();

        //        number = slno.ToString();

        //        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(number, font8)));
        //        table.AddCell(cell21);

        //        PdfPCell cell21j = new PdfPCell(new Phrase(new Chunk(rrr, font8)));
        //        table.AddCell(cell21j);

        //        PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(name + "," + place, font8)));
        //        table.AddCell(cell23);

        //        PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
        //        table.AddCell(cell25);

        //        PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(indate.ToString(), font8)));
        //        table.AddCell(cell27);

        //        PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(outdate.ToString(), font8)));
        //        table.AddCell(cell28);

        //        PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(rents, font8)));
        //        table.AddCell(cell29);

        //        PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(deposits, font8)));
        //        table.AddCell(cell30);

        //        PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(remarks, font8)));
        //        table.AddCell(cell31);

        //        doc.Add(table);
        //        i++;
        //        slno = slno + 1;

        //        if ((i == 26) || (ii == cont - 1))
        //        {
        //            PdfPTable table2 = new PdfPTable(9);
        //            float[] colWidths2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
        //            table2.SetWidths(colWidths2);

        //            PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
        //            cell41.Colspan = 6;
        //            table2.AddCell(cell41);

        //            PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
        //            table2.AddCell(cell49);

        //            gtr = gtr + decimal.Parse(rr.ToString());
        //            gtd = gtd + decimal.Parse(dde.ToString());

        //            PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk("", font9)));
        //            table2.AddCell(cell50);

        //            //PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
        //            //table2.AddCell(cell50);


        //            PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
        //            table2.AddCell(cell51);

        //            doc.Add(table2);

        //            rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
        //        }

        //        if (ii == cont - 1)
        //        {
        //            PdfPTable table10 = new PdfPTable(9);
        //            float[] colWidths10 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
        //            table10.SetWidths(colWidths10);



        //            PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", fontLB)));
        //            cell500p10.Colspan = 9;
        //            cell500p10.Border = 0;
        //            cell500p10.HorizontalAlignment = 1;
        //            table10.AddCell(cell500p10);

        //            PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
        //            cell500p12.Colspan = 3;
        //            cell500p12.Border = 0;
        //            cell500p12.HorizontalAlignment = 0;
        //            table10.AddCell(cell500p12);

        //            PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
        //            cell500p13.Colspan = 2;
        //            cell500p13.Border = 0;
        //            cell500p13.HorizontalAlignment = 0;
        //            table10.AddCell(cell500p13);

        //            PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("", font10)));
        //            cell500p15.Colspan = 2;
        //            cell500p15.Border = 0;
        //            cell500p15.HorizontalAlignment = 0;
        //            table10.AddCell(cell500p15);

        //            PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk("", font10)));
        //            cell500p11.Colspan = 2;
        //            cell500p11.Border = 1;
        //            cell500p11.HorizontalAlignment = 1;
        //            table10.AddCell(cell500p11);


        //            PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", font10)));
        //            cell500p14.Colspan = 2;
        //            cell500p14.Border = 1;
        //            cell500p14.HorizontalAlignment = 1;
        //            table10.AddCell(cell500p14);

        //           // NumberToEnglish n = new NumberToEnglish();
        //            string re = objcls.NumberToTextWithLakhs(Int64.Parse( gtr.ToString()));
        //            re = re + " Only";
        //            PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, font10)));
        //            cell500p16.Colspan = 7;
        //            cell500p16.Border = 1;
        //            cell500p16.HorizontalAlignment = 1;
        //            table10.AddCell(cell500p16);

        //            PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("", font10)));
        //            cell500p17.Colspan = 2;
        //            cell500p17.Border = 1;
        //            cell500p17.HorizontalAlignment = 1;
        //            table10.AddCell(cell500p17);

        //            string de = objcls.NumberToTextWithLakhs(Int64.Parse( gtd.ToString()));
        //            de = de + " Only";
        //            PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk("", font10)));
        //            cell500p18.Colspan = 7;
        //            cell500p18.Border = 1;
        //            cell500p18.HorizontalAlignment = 1;
        //            table10.AddCell(cell500p18);

        //            gtr = 0;
        //            gtd = 0;


        //            /////////////////////////

        //            PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
        //            cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cellfb1.PaddingLeft = 20;
        //            cellfb1.Colspan = 9;
        //            cellfb1.MinimumHeight = 30;
        //            cellfb1.Border = 0;
        //            table10.AddCell(cellfb1);


        //            PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
        //            cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cellfb.PaddingLeft = 20;
        //            cellfb.Colspan = 9;
        //            cellfb.MinimumHeight = 30;
        //            cellfb.Border = 0;
        //            table10.AddCell(cellfb);

        //            PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
        //            cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cellf1b.PaddingLeft = 20;
        //            cellf1b.Colspan = 9;
        //            cellf1b.Border = 0;

        //            table10.AddCell(cellf1b);

        //            PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
        //            cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
        //            cellh2.PaddingLeft = 20;
        //            cellh2.Border = 0;
        //            cellh2.Colspan = 9;
        //            table10.AddCell(cellh2);
        //            /////////////////////////

        //            doc.Add(table10);

        //        }
        //    }

        //    doc.Close();

        //    Random r = new Random();

        //    string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report From-To";
        //    string Script = "";
        //    Script += "<script id='PopupWindow'>";
        //    Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        //    Script += "confirmWin.Setfocus()</script>";
        //    if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
        //        Page.RegisterClientScriptBlock("PopupWindow", Script);
        //}
        //catch
        //{
        //    okmessage("Tsunami ARMS - Warning", "Problem found in taking report");
        //}
       #endregion

        #region ledger from to
        try
        {
            if ((txtfromd.Text == "") || (txttod.Text == ""))
            {
                okmessage("Tsunami ARMS - Warning", "Enter dates");
                return;
            }

            string str11 = objcls.yearmonthdate(txtfromd.Text.ToString());
            // str1 = mm + "-" + dd + "-" + yy;
            string str21 = objcls.yearmonthdate(txttod.Text.ToString());
            // str2 = mm + "-" + dd + "-" + yy;
            DateTime ind = DateTime.Parse(str11);
            DateTime outd = DateTime.Parse(str21);
            if (outd < ind)
            {
                okmessage("Tsunami ARMS - Warning", "Check the dates");
                return;
            }
            string str1 = "2009-11-18 00:00:01";
            // str1 = mm + "-" + dd + "-" + yy;
            string str2 = "2010-09-21 23:59:59";
            // str2 = mm + "-" + dd + "-" + yy;
            DateTime ind11 = DateTime.Parse(str1);
            DateTime outd11 = DateTime.Parse(str2);
            DateTime chk = DateTime.Parse(str11);
            if (chk > ind11 && chk < outd11)
            {
                string std1 = str11 + " 00:00:01";
                string end1 = str21 + " 23:59:59";

                string sql1 = "bill.receiptNo,room.Custname,room.ADDRESS1, "
                      + " bill.buildingName, bill.roomNo, "
                      + " room.Roomalloctime, room.VacTime,"
                      + " bill.rentAmt, bill.advanceAmt,bill.status";

                string sql2 = " relatingreceiptandbill as bill INNER JOIN "
                                + " room_transaction as room ON bill.rowId = room.RowID";

                string sql3 = "room.Roomalloctime between '" + std1 + "'  and '" + end1 + "' order by bill.receiptNo";



                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", sql2);
                cmd5.Parameters.AddWithValue("attribute", sql1);
                cmd5.Parameters.AddWithValue("conditionv", sql3);
                DataTable dtt5 = new DataTable();
                dtt5 = objcls.SpDtTbl("Call selectcond(?,?,?)", cmd5);

                if (dtt5.Rows.Count > 0)
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
                    float[] colWidths1 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table1.SetWidths(colWidths1);



                    //string repdates = rdate.ToString("dd/MM/yyyy");
                    string dt1 = dt.ToString("dd/MM/yyyy");
                    string[] aa = str11.Split('/');
                    DateTime ss = DateTime.Parse(str11.ToString());
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

                    string datf = objcls.yearmonthdate(txtfromd.Text);
                    string datt = objcls.yearmonthdate(txttod.Text);

                    DateTime daf1 = DateTime.Parse(datf.ToString());
                    DateTime dat1 = DateTime.Parse(datt.ToString());

                    string g1 = daf1.ToString("yyyy-MM-dd");
                    string g2 = dat1.ToString("yyyy-MM-dd");

                    string g3 = daf1.ToString("dd/MM/yy");
                    string g4 = dat1.ToString("dd/MM/yy");

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

                    //PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                    //cell502.Colspan = 4;
                    //cell502.Border = 0;
                    //cell502.HorizontalAlignment = 2;
                    //table1.AddCell(cell502);

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

                    for (int ii = 0; ii < dtt5.Rows.Count; ii++)
                    {
                        if (i > 26)
                        {
                            doc.NewPage();
                            //PdfPTable table4 = new PdfPTable(9);
                            //float[] colWidths4 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                            //table4.SetWidths(colWidths4);


                            PdfPTable table3 = new PdfPTable(9);
                            float[] colWidths3 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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


                            //string datf = objcls.yearmonthdate(txtfromd.Text);
                            //string datt = objcls.yearmonthdate(txttod.Text);

                            //DateTime daf1 = DateTime.Parse(datf.ToString());
                            //DateTime dat1 = DateTime.Parse(datt.ToString());

                            //string g1 = daf1.ToString("yyyy-MM-dd");
                            //string g2 = dat1.ToString("yyyy-MM-dd");

                            //string g3 = daf1.ToString("dd/MM/yy");
                            //string g4 = dat1.ToString("dd/MM/yy");

                            if (txtfromd.Text.ToString() == txttod.Text.ToString())
                            {
                                PdfPCell cell502a = new PdfPCell(new Phrase(new Chunk("Date: " + g3.ToString(), font10)));
                                cell502a.Colspan = 4;
                                cell502a.Border = 0;

                                cell502a.HorizontalAlignment = 2;
                                table3.AddCell(cell502a);


                            }
                            else
                            {

                                PdfPCell cell502a = new PdfPCell(new Phrase(new Chunk("Date: " + g3 + "-" + g4, font10)));
                                cell502a.Colspan = 4;
                                cell502a.Border = 0;
                                cell502a.HorizontalAlignment = 2;
                                table3.AddCell(cell502a);
                            }

                            //PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, fontLB)));
                            //cell502p.Colspan = 4;
                            //cell502p.Border = 0;
                            //cell502p.HorizontalAlignment = 2;
                            //table3.AddCell(cell502p);

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
                        int j = ii + 1;
                        //aa[2] + " - " +
                        string no = j.ToString();
                        string Rec = dtt5.Rows[ii]["receiptNo"].ToString();
                        string name = dtt5.Rows[ii]["Custname"].ToString();
                        string addr = dtt5.Rows[ii]["ADDRESS1"].ToString();
                        string namadd = name + " ," + addr;
                        string build1 = "";
                        string building1 = dtt5.Rows[ii]["buildingname"].ToString();
                        if (building1.Contains("(") == true)
                        {
                            string[] buildS11, buildS21;
                            buildS11 = building1.Split('(');
                            build1 = buildS11[1];
                            buildS21 = build1.Split(')');
                            build1 = buildS21[0];
                            building1 = build1;
                        }
                        else if (building1.Contains("Cottage") == true)
                        {
                            building1 = building1.Replace("Cottage", "Cot");
                        }
                        string room = dtt5.Rows[ii]["roomNo"].ToString();
                        string buroom = building1 + " - " + room;
                        indat = DateTime.Parse(dtt5.Rows[ii]["Roomalloctime"].ToString());
                        string ind21 = indat.ToString("dd-MMM");
                        it = indat.ToString("hh:mm:tt");
                        indate = it + "       " + ind21;

                        outdat = DateTime.Parse(dtt5.Rows[ii]["VacTime"].ToString());
                        string outd21 = outdat.ToString("dd-MMM");
                        ot = outdat.ToString("hh:mm:tt");
                        outdate = ot + "       " + outd21;
                        string rent = dtt5.Rows[ii]["rentAmt"].ToString();
                        string advrent = dtt5.Rows[ii]["advanceAmt"].ToString();

                        string cancel = dtt5.Rows[ii]["status"].ToString();
                        if (cancel != "OK")
                        {
                            rent = "0";
                            remarks = dtt5.Rows[ii]["status"].ToString();

                            rrent1 = decimal.Parse(rent.ToString());
                            rrent = rrent + rrent1;

                            rr = rrent.ToString();
                            rdeposit1 = decimal.Parse(advrent.ToString());
                            rdeposit = rdeposit + rdeposit1;

                            dde = rdeposit.ToString();
                        }
                        else
                        {
                            remarks = "";
                            rrent1 = decimal.Parse(rent.ToString());
                            rrent = rrent + rrent1;

                            rr = rrent.ToString();
                            rdeposit1 = decimal.Parse(advrent.ToString());
                            rdeposit = rdeposit + rdeposit1;

                            dde = rdeposit.ToString();
                        }





                        PdfPTable table = new PdfPTable(9);
                        float[] colWidths6 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                        table.SetWidths(colWidths6);

                        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(no, font8)));
                        table.AddCell(cell21);

                        PdfPCell cell23g = new PdfPCell(new Phrase(new Chunk(Rec, font8)));
                        table.AddCell(cell23g);


                        PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(namadd, font8)));
                        table.AddCell(cell23);

                        PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(buroom, font8)));
                        table.AddCell(cell25);

                        PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(indate.ToString(), font8)));
                        table.AddCell(cell27);

                        PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(outdate.ToString(), font8)));
                        table.AddCell(cell28);

                        PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(rent, font8)));
                        table.AddCell(cell29);

                        PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(advrent, font8)));
                        table.AddCell(cell30);

                        PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(remarks, font8)));
                        table.AddCell(cell31);

                        doc.Add(table);
                        i++;

                        if ((i == 27) || (ii == dtt5.Rows.Count - 1))
                        {
                            PdfPTable table2 = new PdfPTable(9);
                            float[] colWidths2 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                            table2.SetWidths(colWidths2);

                            PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                            cell41.Colspan = 6;
                            table2.AddCell(cell41);

                            gtr = gtr + decimal.Parse(rr.ToString());
                            gtd = gtd + decimal.Parse(dde.ToString());


                            PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                            table2.AddCell(cell49);

                            PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                            table2.AddCell(cell50);

                            PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            table2.AddCell(cell51);

                            doc.Add(table2);

                            rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                        }

                        if (ii == dtt5.Rows.Count - 1)
                        {
                            PdfPTable table10 = new PdfPTable(9);
                            float[] colWidths10 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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

                            Int64 gt = Convert.ToInt64(gtr);
                            string re = objcls.NumberToTextWithLakhs(gt);
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

                            Int64 gtde = Convert.ToInt64(gtd);
                            string de = objcls.NumberToTextWithLakhs(gtde);
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
                }
            }
            else
            {


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
                               + "actualvecdate";


                strsql3 = "alloc.room_id=room.room_id"
                  + " and room.build_id=build.build_id"
                  + " and alloc.dayend >= '" + datf + "' and alloc.dayend <= '" + datt + "' order by alloc.alloc_id asc";


                OdbcCommand cmd350 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                DateTime reporttime = DateTime.Now;
                report = "Ledger From-To " + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

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
                float[] colWidths1 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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
                    if (i > 25)
                    {
                        doc.NewPage();

                        PdfPTable table2 = new PdfPTable(9);
                        float[] colWidths2 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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
                            PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + g3.ToString(), font10)));
                            cell502.Colspan = 4;
                            cell502.Border = 0;

                            cell502.HorizontalAlignment = 2;
                            table2.AddCell(cell502);


                        }
                        else
                        {

                            PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + g3 + "-" + g4, font10)));
                            cell502.Colspan = 4;
                            cell502.Border = 0;
                            cell502.HorizontalAlignment = 2;
                            table2.AddCell(cell502);
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
                    float[] colWidths ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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

                            OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                            OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                        OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                        OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                    string rrr = dtt350.Rows[ii]["adv_recieptno"].ToString();

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

                    if ((i == 26) || (ii == cont - 1))
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

                        //PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                        //table2.AddCell(cell50);


                        PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        table2.AddCell(cell51);

                        doc.Add(table2);

                        rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                    }

                    if (ii == cont - 1)
                    {
                        PdfPTable table10 = new PdfPTable(9);
                        float[] colWidths10 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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

                        // NumberToEnglish n = new NumberToEnglish();
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


                        /////////////////////////

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
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in taking report");
        }
        #endregion
    }

    # endregion

    protected void btnHidden_Click(object sender, EventArgs e)
    {

    }
    #region Button Yes
    protected void btnYes_Click(object sender, EventArgs e)
    {

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
           + " left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";

            string strsql2 = "alloc.alloc_id,"
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
                  + " and alloc.dayend='" + Session["ledgerDate"].ToString() + "' order by alloc.alloc_id asc";



            OdbcCommand cmd350 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

            PdfPTable table1 = new PdfPTable(9);
            float[] colWidths1 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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
                    PdfPTable table4 = new PdfPTable(9);
                    float[] colWidths4 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table4.SetWidths(colWidths4);


                    PdfPTable table3 = new PdfPTable(9);
                    float[] colWidths3 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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
                float[] colWidths ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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

                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
                        cmdallocfr.CommandType = CommandType.StoredProcedure;
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

                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                    float[] colWidths10 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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

                    NumberToEnglish n = new NumberToEnglish();
                    string re = n.changeNumericToWords(gtr.ToString());
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

                    string de = n.changeNumericToWords(gtd.ToString());
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
                OdbcCommand cmd901 = new OdbcCommand();//"CALL selectdata(?,?)", con);
                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                DataTable dtt901 = new DataTable();
                dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                id = int.Parse(dtt901.Rows[0][0].ToString());
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd25 = new OdbcCommand();//"call updatedata(?,?,?)", con);
                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                int iq;
                iq = objcls.Procedures("call updatedata(?,?,?)", cmd25);
                //  con.Close();
            }
            catch
            {
                id = 1;
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();

                OdbcCommand cmd589 = new OdbcCommand();//"CALL savedata(?,?)", con);
                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                int j = objcls.Procedures("CALL savedata(?,?)", cmd589);
                //cmd589.ExecuteNonQuery();
                //con.Close();
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
         + " left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";

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
             + " and alloc.alloc_id>" + s + " order by alloc.alloc_id asc";

            OdbcCommand cmd350 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

            PdfPTable table1 = new PdfPTable(9);
            float[] colWidths1 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
            table1.SetWidths(colWidths1);


            string repdates = rdate.ToString("dd/MM/yyyy");
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
            string dateee = ss.ToString("dd-MMMM-yyyy");

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
            cell500.Colspan = 8;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 4;
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

            doc.Add(table1);

            int i = 0;

            for (int ii = 0; ii < cont; ii++)
            {
                if (i > 26)
                {
                    doc.NewPage();
                    PdfPTable table4 = new PdfPTable(9);
                    float[] colWidths4 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
                    table4.SetWidths(colWidths4);


                    PdfPTable table3 = new PdfPTable(9);
                    float[] colWidths3 ={ 40, 60, 130, 80, 100, 100, 60, 40, 70 };
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
                float[] colWidths ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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

                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                doc.Add(table);
                i++;

                if ((i == 27) || (ii == cont - 1))
                {
                    PdfPTable table2 = new PdfPTable(9);
                    float[] colWidths2 = { 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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
                    float[] colWidths10 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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

                    NumberToEnglish n = new NumberToEnglish();
                    string re = n.changeNumericToWords(gtr.ToString());
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

                    string de = n.changeNumericToWords(gtd.ToString());
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
                OdbcCommand cmd901 = new OdbcCommand();//"CALL selectdata(?,?)", con);
                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                DataTable dtt901 = new DataTable();
                dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                id = int.Parse(dtt901.Rows[0][0].ToString());

                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd25 = new OdbcCommand();//"call updatedata(?,?,?)", con);
                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                int iss = objcls.Procedures("call updatedata(?,?,?)", cmd25);

            }
            catch
            {
                id = 1;
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();

                OdbcCommand cmd589 = new OdbcCommand();//"CALL savedata(?,?)", con);
                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                int j = objcls.Procedures("CALL savedata(?,?)", cmd589);
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
                  + " Left join m_sub_district as dist on alloc.district_id=dist.district_id left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";

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
             + " and alloc.alloc_id>" + s + " order by alloc.alloc_id asc";


            OdbcCommand cmd350 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

            PdfPTable table1 = new PdfPTable(9);
            float[] colWidths1 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
            table1.SetWidths(colWidths1);


            string repdates = rdate.ToString("dd/MM/yyyy");
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
            string dateee = ss.ToString("dd-MMMM-yyyy");

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
            cell500.Colspan = 8;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 4;
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

            doc.Add(table1);

            int i = 0;

            for (int ii = 0; ii < cont; ii++)
            {
                if (i > 26)
                {
                    doc.NewPage();
                    PdfPTable table4 = new PdfPTable(9);
                    float[] colWidths4 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
                    table4.SetWidths(colWidths4);


                    PdfPTable table3 = new PdfPTable(9);
                    float[] colWidths3 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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
                float[] colWidths ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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

                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                    float[] colWidths2 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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
                    float[] colWidths10 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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

                    NumberToEnglish n = new NumberToEnglish();
                    string re = n.changeNumericToWords(gtr.ToString());
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

                    string de = n.changeNumericToWords(gtd.ToString());
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
                OdbcCommand cmd901 = new OdbcCommand();//"CALL selectdata(?,?)", con);
                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                DataTable dtt901 = new DataTable();
                dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                id = int.Parse(dtt901.Rows[0][0].ToString());

                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd25 = new OdbcCommand();//"call updatedata(?,?,?)", con);
                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                int p1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
            }
            catch
            {
                id = 1;
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();

                OdbcCommand cmd589 = new OdbcCommand();//"CALL savedata(?,?)", con);
                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                int p2 = objcls.Procedures("CALL savedata(?,?)", cmd589);
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
            + " Left join m_sub_district as dist on alloc.district_id=dist.district_id left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";

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
                  + " and alloc.dayend='" + Session["ledgerDate"].ToString() + "' order by alloc.alloc_id asc";



            OdbcCommand cmd350 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                PdfPTable table1 = new PdfPTable(9);
                float[] colWidths1 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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

                doc.Add(table1);

                int i = 0;

                for (int ii = 0; ii < cont; ii++)
                {
                    if (i > 26)
                    {
                        doc.NewPage();
                        PdfPTable table4 = new PdfPTable(9);
                        float[] colWidths4 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
                        table4.SetWidths(colWidths4);


                        PdfPTable table3 = new PdfPTable(9);
                        float[] colWidths3 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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
                    float[] colWidths ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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

                            OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                            OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                        OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                        OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                        float[] colWidths2 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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
                        float[] colWidths10 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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

                        NumberToEnglish n = new NumberToEnglish();
                        string re = n.changeNumericToWords(gtr.ToString());
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

                        string de = n.changeNumericToWords(gtd.ToString());
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
                    OdbcCommand cmd901 = new OdbcCommand();//"CALL selectdata(?,?)", con);
                    cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                    cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                    DataTable dtt901 = new DataTable();
                    dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                    id = int.Parse(dtt901.Rows[0][0].ToString());

                    int tno = int.Parse(Session["tno"].ToString());
                    string ct = Session["num"].ToString();
                    OdbcCommand cmd25 = new OdbcCommand();//"call updatedata(?,?,?)", con);
                    cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                    cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                    cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                    int pr1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
                }
                catch
                {
                    id = 1;
                    int tno = int.Parse(Session["tno"].ToString());
                    string ct = Session["num"].ToString();

                    OdbcCommand cmd589 = new OdbcCommand();//"CALL savedata(?,?)", con);
                    cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                    cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                    int pr2 = objcls.Procedures("CALL savedata(?,?)", cmd589);
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


            int s = int.Parse(Session["tno"].ToString());
            strsql3 = "alloc.room_id=room.room_id"
             + " and room.build_id=build.build_id"
             + " and alloc.dayend='" + Session["ledgerDate"].ToString() + "' order by alloc.alloc_id asc";

            OdbcCommand cmd350 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
            float[] colWidths1 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
            table1.SetWidths(colWidths1);


            string repdates = rdate.ToString("dd/MM/yyyy");
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
            string dateee = ss.ToString("dd-MMMM-yyyy");

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
            cell500.Colspan = 8;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 4;
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
                    float[] colWidths4 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table4.SetWidths(colWidths4);


                    PdfPTable table3 = new PdfPTable(9);
                    float[] colWidths3 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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
                float[] colWidths ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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

                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                    float[] colWidths2 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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
                    float[] colWidths10 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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

                    NumberToEnglish n = new NumberToEnglish();
                    string re = n.changeNumericToWords(gtr.ToString());
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

                    string de = n.changeNumericToWords(gtd.ToString());
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
                OdbcCommand cmd901 = new OdbcCommand();//"CALL selectdata(?,?)", con);
                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                DataTable dtt901 = new DataTable();
                dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                id = int.Parse(dtt901.Rows[0][0].ToString());

                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd25 = new OdbcCommand();//"call updatedata(?,?,?)", con);
                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                int pr3 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
            }
            catch
            {
                id = 1;
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();

                OdbcCommand cmd589 = new OdbcCommand();//"CALL savedata(?,?)", con);
                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                int pr4 = objcls.Procedures("CALL savedata(?,?)", cmd589);
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

            OdbcCommand cmd350 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
            float[] colWidths1 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
            table1.SetWidths(colWidths1);


            string repdates = rdate.ToString("dd/MM/yyyy");
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
            string dateee = ss.ToString("dd-MMMM-yyyy");

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
            cell500.Colspan = 8;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 4;
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
                    float[] colWidths4 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
                    table4.SetWidths(colWidths4);


                    PdfPTable table3 = new PdfPTable(9);
                    float[] colWidths3 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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
                float[] colWidths ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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

                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
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

                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                    float[] colWidths2 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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
                    float[] colWidths10 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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

                    NumberToEnglish n = new NumberToEnglish();
                    string re = n.changeNumericToWords(gtr.ToString());
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

                    string de = n.changeNumericToWords(gtd.ToString());
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
                OdbcCommand cmd901 = new OdbcCommand();//"CALL selectdata(?,?)", con);
                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                DataTable dtt901 = new DataTable();
                dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                id = int.Parse(dtt901.Rows[0][0].ToString());

                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd25 = new OdbcCommand();//"call updatedata(?,?,?)", con);
                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                int pt1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
            }
            catch
            {
                id = 1;
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();

                OdbcCommand cmd589 = new OdbcCommand();//"CALL savedata(?,?)", con);
                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                int pt2 = objcls.Procedures("CALL savedata(?,?)", cmd589);
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




            OdbcCommand cmd350 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
            float[] colWidths1 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
            table1.SetWidths(colWidths1);

            string repdates = rdate.ToString("dd/MM/yyyy");
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
            string dateee = ss.ToString("dd-MMMM-yyyy");

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
            cell500.Colspan = 8;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 4;
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
                    float[] colWidths4 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
                    table4.SetWidths(colWidths4);


                    PdfPTable table3 = new PdfPTable(9);
                    float[] colWidths3 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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
                float[] colWidths ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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

                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                    float[] colWidths2 = { 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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
                    float[] colWidths10 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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

                    NumberToEnglish n = new NumberToEnglish();
                    string re = n.changeNumericToWords(gtr.ToString());
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

                    string de = n.changeNumericToWords(gtd.ToString());
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
                OdbcCommand cmd901 = new OdbcCommand();//"CALL selectdata(?,?)", con);
                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                DataTable dtt901 = new DataTable();
                dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                id = int.Parse(dtt901.Rows[0][0].ToString());

                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd25 = new OdbcCommand();//"call updatedata(?,?,?)", con);
                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                int py1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
            }
            catch
            {
                id = 1;
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();

                OdbcCommand cmd589 = new OdbcCommand();//"CALL savedata(?,?)", con);
                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                int py2 = objcls.Procedures("CALL savedata(?,?)", cmd589);
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



            OdbcCommand cmd350 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                float[] colWidths1 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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
                        float[] colWidths4 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
                        table4.SetWidths(colWidths4);


                        PdfPTable table3 = new PdfPTable(9);
                        float[] colWidths3 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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
                    float[] colWidths ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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

                            OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                            OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                        OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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

                        OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
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
                        float[] colWidths2 = { 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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
                        float[] colWidths10 ={ 70, 70, 130, 80, 100, 100, 60, 40, 50 };
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

                        NumberToEnglish n = new NumberToEnglish();
                        string re = n.changeNumericToWords(gtr.ToString());
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

                        string de = n.changeNumericToWords(gtd.ToString());
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
                    OdbcCommand cmd901 = new OdbcCommand();//"CALL selectdata(?,?)", con);
                    cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                    cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                    DataTable dtt901 = new DataTable();
                    dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                    id = int.Parse(dtt901.Rows[0][0].ToString());

                    int tno = int.Parse(Session["tno"].ToString());
                    string ct = Session["num"].ToString();
                    OdbcCommand cmd25 = new OdbcCommand();//"call updatedata(?,?,?)", con);
                    cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                    cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                    cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                    int pu = objcls.Procedures("call updatedata(?,?,?)", cmd25);
                }
                catch
                {
                    id = 1;
                    int tno = int.Parse(Session["tno"].ToString());
                    string ct = Session["num"].ToString();

                    OdbcCommand cmd589 = new OdbcCommand();//"CALL savedata(?,?)", con);
                    cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                    cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                    int pu = objcls.Procedures("CALL savedata(?,?)", cmd589);
                }
            }

        }
        #endregion

        #endregion

    }
    #endregion
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }

    }
    # region Ledger for the currrent day
    protected void lnkDonorPaidRoomAllocationReport_Click(object sender, EventArgs e)
    {
        #region new ledger
        //DateTime rdate = DateTime.Now;
        //string repdate = rdate.ToString("yyyy/MM/dd");
        //string reptime = rdate.ToShortTimeString();

        //try
        //{
        //    if (txtdate.Text == "")
        //    {
        //        okmessage("Tsunami ARMS - Message", "Please Enter date");
        //        return;
        //    }

        //    string dt3 = objcls.yearmonthdate(txtdate.Text);
        //    Session["ledgerDate"] = dt3.ToString();
                                   
        //    OdbcCommand cmd550 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
        //    cmd550.Parameters.AddWithValue("tblname", "t_printledger");
        //    cmd550.Parameters.AddWithValue("attribute", "*");
        //    cmd550.Parameters.AddWithValue("conditionv", "date='" + dt3 + "' and slno=" + 1 + "");
        //    DataTable dtt550 = new DataTable();
        //    dtt550 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd550);
        //    if (dtt550.Rows.Count > 0)
        //    {

        //        Session["tno"] = dtt550.Rows[0]["alloc_id"].ToString();
        //        Session["num"] = dtt550.Rows[0]["printed_no"].ToString();

        //        // lblMsg.Text = "Including half Print?";
        //        lblMsg.Text = "Want to take full report on the day?";
        //        ViewState["action"] = "Full Report";
        //        pnlOk.Visible = false;
        //        pnlYesNo.Visible = true;
        //        ModalPopupExtender1.Show();
        //        this.ScriptManager1.SetFocus(btnYes);
        //    }
        //    else
        //    {
        //        Session["rep"] = "full";
        //        Session["num"] = 1;
        //        Session["tno"] = null;

        //        string strsql1 = "m_room as room,"
        //             + "m_sub_building as build,"
        //             + "t_roomallocation as alloc"
        //             + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
        //             + " Left join m_sub_district as dist on alloc.district_id=dist.district_id";

        //        string strsql2 = "alloc.alloc_id,"
        //                       + "alloc.alloc_no,"
        //                       + "alloc.place,"
        //                       + "alloc.pass_id,"
        //                       + "alloc.phone,"
        //                       + "alloc.idproof,"
        //                       + "alloc.idproofno,"
        //                       + "alloc.noofinmates,"
        //                       + "alloc.numberofunit,"
        //                       + "alloc.advance,"
        //                       + "alloc.reason,"
        //                       + "alloc.othercharge,"
        //                       + "alloc.adv_recieptno,"
        //                       + "alloc.swaminame,"
        //                       + "build.buildingname,"
        //                       + "room.roomno,"
        //                       + "alloc.allocdate,"
        //                       + "alloc.exp_vecatedate,"
        //                       + "alloc.roomrent,"
        //                       + "alloc.state_id,"
        //                       + "alloc.district_id,"
        //                       + "alloc.deposit,"
        //                        + "alloc.alloc_type,"
        //                      + "alloc.totalcharge,"
        //                   + "alloc.realloc_from,"
        //                   + "alloc.reason_id";


        //        strsql3 = "alloc.room_id=room.room_id"
        //          + " and room.build_id=build.build_id"
        //          + " and dayend='" + dt3 + "' order by alloc_id asc";


        //        OdbcCommand cmd350 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
        //        cmd350.Parameters.AddWithValue("tblname", strsql1);
        //        cmd350.Parameters.AddWithValue("attribute", strsql2);
        //        cmd350.Parameters.AddWithValue("conditionv", strsql3);
        //        DataTable dtt350 = new DataTable();
        //        dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);

        //        if (dtt350.Rows.Count == 0)
        //        {
        //            okmessage("Tsunami ARMS - Warning", "No details found");
        //            return;
        //        }

        //        int cont = dtt350.Rows.Count;
        //        int hp = cont % 20;
        //        Session["cont"] = cont.ToString();
        //        Session["hp"] = hp.ToString();

        //        if (hp < 18)
        //        {
        //            lblMsg.Text = "Including half Print?";
        //            ViewState["action"] = "Half Print include  on full report";
        //            pnlOk.Visible = false;
        //            pnlYesNo.Visible = true;
        //            ModalPopupExtender1.Show();
        //            this.ScriptManager1.SetFocus(btnYes);
        //        }
        //        else
        //        {
        //            DateTime reporttime = DateTime.Now;
        //            report = "Ledger Report" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

        //            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
        //            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;




        //            Font font8 = FontFactory.GetFont("ARIAL", 9);
        //            Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
        //            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        //            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
        //            pdfPage page = new pdfPage();
        //            page.strRptMode = "Allocation";
        //            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //            wr.PageEvent = page;

        //            doc.Open();

        //            PdfPTable table1 = new PdfPTable(9);
        //            float[] colWidths1 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
        //            table1.SetWidths(colWidths1);



        //            string repdates = rdate.ToString("dd/MM/yyyy");
        //            string dt1 = dt.ToString("dd/MM/yyyy");

        //            DateTime ss = DateTime.Parse(dt3.ToString());
        //            string dateee = ss.ToString("dd-MMMM-yyyy");

        //            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
        //            cell500.Colspan = 8;
        //            cell500.Border = 1;
        //            cell500.HorizontalAlignment = 1;
        //            table1.AddCell(cell500);

        //            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
        //            cell501.Colspan = 4;
        //            cell501.Border = 0;
        //            cell501.HorizontalAlignment = 0;
        //            table1.AddCell(cell501);

        //            PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
        //            cell502.Colspan = 4;
        //            cell502.Border = 0;
        //            cell502.HorizontalAlignment = 2;
        //            table1.AddCell(cell502);

        //            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        //            table1.AddCell(cell2);

        //            PdfPCell cell2fg = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
        //            table1.AddCell(cell2fg);


        //            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
        //            table1.AddCell(cell3);

        //            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        //            table1.AddCell(cell5);

        //            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
        //            table1.AddCell(cell7);

        //            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
        //            table1.AddCell(cell8);

        //            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
        //            table1.AddCell(cell9);

        //            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
        //            table1.AddCell(cell10);

        //            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
        //            table1.AddCell(cell11);

        //            doc.Add(table1);

        //            int i = 0;

        //            for (int ii = 0; ii < cont; ii++)
        //            {
        //                if (i > 26)
        //                {
        //                    doc.NewPage();
        //                    PdfPTable table4 = new PdfPTable(9);
        //                    float[] colWidths4 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
        //                    table4.SetWidths(colWidths4);


        //                    PdfPTable table3 = new PdfPTable(9);
        //                    float[] colWidths3 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
        //                    table3.SetWidths(colWidths3);


        //                    PdfPCell cell500p = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
        //                    cell500p.Colspan = 9;
        //                    cell500p.Border = 1;
        //                    cell500p.HorizontalAlignment = 1;
        //                    table3.AddCell(cell500p);

        //                    PdfPCell cell501p = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
        //                    cell501p.Colspan = 5;
        //                    cell501p.Border = 0;
        //                    cell501p.HorizontalAlignment = 0;
        //                    table3.AddCell(cell501p);

        //                    PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + font10, fontLB)));
        //                    cell502p.Colspan = 4;
        //                    cell502p.Border = 0;
        //                    cell502p.HorizontalAlignment = 2;
        //                    table3.AddCell(cell502p);

        //                    PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("No", font9)));
        //                    table3.AddCell(cell2p);

        //                    PdfPCell cell3p1 = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
        //                    table3.AddCell(cell3p1);

        //                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
        //                    table3.AddCell(cell3p);

        //                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        //                    table3.AddCell(cell5p);

        //                    PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
        //                    table3.AddCell(cell7p);

        //                    PdfPCell cell8p = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
        //                    table3.AddCell(cell8p);

        //                    PdfPCell cell9p = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
        //                    table3.AddCell(cell9p);

        //                    PdfPCell cell10p = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
        //                    table3.AddCell(cell10);

        //                    PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
        //                    table3.AddCell(cell11p);

        //                    i = 0;

        //                    doc.Add(table3);
        //                }

        //                PdfPTable table = new PdfPTable(9);
        //                float[] colWidths ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
        //                table.SetWidths(colWidths);


        //                transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
        //                num = dtt350.Rows[ii]["alloc_no"].ToString();
        //                Session["num"] = num.ToString();
        //                name = dtt350.Rows[ii]["swaminame"].ToString();
        //                place = dtt350.Rows[ii]["place"].ToString();
        //                states = dtt350.Rows[ii]["state_id"].ToString();
        //                dist = dtt350.Rows[ii]["district_id"].ToString();
        //                rec = dtt350.Rows[ii]["adv_recieptno"].ToString();

        //                allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
        //                reason = dtt350.Rows[ii]["reason_id"].ToString();
        //                alloctype = dtt350.Rows[ii]["alloc_type"].ToString();


        //                #region extent remark&alter remark
        //                if (allocfrom != "")
        //                {
        //                    if (reason != "")
        //                    {

        //                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
        //                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
        //                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
        //                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
        //                        DataTable dtallocfr = new DataTable();
        //                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
        //                        if (dtallocfr.Rows.Count > 0)
        //                        {
        //                            remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
        //                        }


        //                    }
        //                    else
        //                    {
        //                        OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
        //                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
        //                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
        //                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
        //                        DataTable dtallocfr = new DataTable();
        //                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
        //                        if (dtallocfr.Rows.Count > 0)
        //                        {
        //                            remarks = "Ext: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
        //                        }

        //                    }
        //                }
        //                else
        //                {
        //                    remarks = "";
        //                }
        //                #endregion

        //                #region donor remark
        //                if (alloctype == "Donor Free Allocation")
        //                {
        //                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

        //                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
        //                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
        //                    cmd115.Parameters.AddWithValue("attribute", "passno");
        //                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
        //                    DataTable dtt115 = new DataTable();
        //                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
        //                    if (dtt115.Rows.Count > 0)
        //                    {
        //                        passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
        //                        remarks = remarks + passno;
        //                    }
        //                }
        //                else if (alloctype == "Donor Paid Allocation")
        //                {
        //                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

        //                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
        //                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
        //                    cmd115.Parameters.AddWithValue("attribute", "passno");
        //                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
        //                    DataTable dtt115 = new DataTable();
        //                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
        //                    if (dtt115.Rows.Count > 0)
        //                    {
        //                        passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
        //                        remarks = remarks + passno;
        //                    }
        //                }
        //                else if (alloctype == "Donor multiple pass")
        //                {
        //                    //

        //                    int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
        //                    mpass = "";

        //                    OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
        //                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
        //                    cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
        //                    cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");
        //                    DataTable dtt115 = new DataTable();
        //                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
        //                    for (int b = 0; b < dtt115.Rows.Count; b++)
        //                    {
        //                        string ptype = dtt115.Rows[b]["passtype"].ToString();
        //                        if (ptype == "0")
        //                        {
        //                            passno = "F P: " + dtt115.Rows[b]["passno"].ToString();
        //                            mpass = passno + "   " + mpass;
        //                        }
        //                        else if (ptype == "1")
        //                        {
        //                            passno = "P P: " + dtt115.Rows[b]["passno"].ToString();
        //                            mpass = passno + "   " + mpass;
        //                        }
        //                    }
        //                    remarks = remarks + mpass;
        //                }
        //                else
        //                {
        //                }
        //                #endregion


        //                build = "";
        //                building = dtt350.Rows[ii]["buildingname"].ToString();
        //                if (building.Contains("(") == true)
        //                {
        //                    string[] buildS1, buildS2; ;
        //                    buildS1 = building.Split('(');
        //                    build = buildS1[1];
        //                    buildS2 = build.Split(')');
        //                    build = buildS2[0];
        //                    building = build;
        //                }
        //                else if (building.Contains("Cottage") == true)
        //                {
        //                    building = building.Replace("Cottage", "Cot");
        //                }

        //                room = dtt350.Rows[ii]["roomno"].ToString();

        //                Session["rec"] = rec.ToString();
        //                Session["tno"] = transno.ToString();

        //                indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
        //                ind = indat.ToString("dd-MMM");
        //                it = indat.ToString("hh:mm:tt");
        //                indate = it + "       " + ind;

        //                outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
        //                outd = outdat.ToString("dd-MMM");
        //                ot = outdat.ToString("hh:mm:tt");
        //                outdate = ot + "       " + outd;

        //                rents = dtt350.Rows[ii]["roomrent"].ToString();
        //                deposits = dtt350.Rows[ii]["deposit"].ToString();


        //                rrent1 = decimal.Parse(rents.ToString());
        //                rrent = rrent + rrent1;

        //                rr = rrent.ToString();
        //                rdeposit1 = decimal.Parse(deposits.ToString());
        //                rdeposit = rdeposit + rdeposit1;

        //                dde = rdeposit.ToString();

        //                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
        //                table.AddCell(cell21);

        //                PdfPCell cell23g = new PdfPCell(new Phrase(new Chunk(rec, font8)));
        //                table.AddCell(cell23g);


        //                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(name + "," + place, font8)));
        //                table.AddCell(cell23);

        //                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
        //                table.AddCell(cell25);

        //                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(indate.ToString(), font8)));
        //                table.AddCell(cell27);

        //                PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(outdate.ToString(), font8)));
        //                table.AddCell(cell28);

        //                PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(rents, font8)));
        //                table.AddCell(cell29);

        //                PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(deposits, font8)));
        //                table.AddCell(cell30);

        //                PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(remarks, font8)));
        //                table.AddCell(cell31);

        //                doc.Add(table);
        //                i++;

        //                if ((i == 27) || (ii == cont - 1))
        //                {
        //                    PdfPTable table2 = new PdfPTable(9);
        //                    float[] colWidths2 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
        //                    table2.SetWidths(colWidths2);

        //                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
        //                    cell41.Colspan = 6;
        //                    table2.AddCell(cell41);

        //                    PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
        //                    table2.AddCell(cell49);

        //                    gtr = gtr + decimal.Parse(rr.ToString());
        //                    gtd = gtd + decimal.Parse(dde.ToString());

        //                    PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
        //                    table2.AddCell(cell50);

        //                    PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
        //                    table2.AddCell(cell51);

        //                    doc.Add(table2);

        //                    rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
        //                }

        //                if (ii == cont - 1)
        //                {
        //                    PdfPTable table10 = new PdfPTable(9);
        //                    float[] colWidths10 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
        //                    table10.SetWidths(colWidths10);



        //                    PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", font10)));
        //                    cell500p10.Colspan = 9;
        //                    cell500p10.Border = 0;
        //                    cell500p10.HorizontalAlignment = 1;
        //                    table10.AddCell(cell500p10);
        //                    /////////////////////
        //                    PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
        //                    cell500p12.Colspan = 2;
        //                    cell500p12.Border = 0;
        //                    cell500p12.HorizontalAlignment = 0;
        //                    table10.AddCell(cell500p12);

        //                    PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
        //                    cell500p13.Colspan = 3;
        //                    cell500p13.Border = 0;
        //                    cell500p13.HorizontalAlignment = 0;
        //                    table10.AddCell(cell500p13);

        //                    PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("Deposit : ", font10)));
        //                    cell500p15.Colspan = 2;
        //                    cell500p15.Border = 0;
        //                    cell500p15.HorizontalAlignment = 0;
        //                    table10.AddCell(cell500p15);


        //                    PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), font10)));
        //                    cell500p11.Colspan = 2;
        //                    cell500p11.Border = 1;
        //                    cell500p11.HorizontalAlignment = 1;
        //                    table10.AddCell(cell500p11);
        //                    ///////////////////
        //                    PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", font10)));
        //                    cell500p14.Colspan = 2;
        //                    cell500p14.Border = 1;
        //                    cell500p14.HorizontalAlignment = 1;
        //                    table10.AddCell(cell500p14);

        //                    NumberToEnglish n = new NumberToEnglish();
        //                    string re = n.changeNumericToWords(gtr.ToString());
        //                    re = re + " Only";
        //                    PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, font10)));
        //                    cell500p16.Colspan = 7;
        //                    cell500p16.Border = 1;
        //                    cell500p16.HorizontalAlignment = 1;
        //                    table10.AddCell(cell500p16);

        //                    PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("Deposit: ", font10)));
        //                    cell500p17.Colspan = 2;
        //                    cell500p17.Border = 1;
        //                    cell500p17.HorizontalAlignment = 1;
        //                    table10.AddCell(cell500p17);

        //                    string de = n.changeNumericToWords(gtd.ToString());
        //                    de = de + " Only";
        //                    PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk(de, font10)));
        //                    cell500p18.Colspan = 7;
        //                    cell500p18.Border = 1;
        //                    cell500p18.HorizontalAlignment = 1;
        //                    table10.AddCell(cell500p18);
        //                    gtr = 0;
        //                    gtd = 0;

        //                    PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
        //                    cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cellfb1.PaddingLeft = 20;
        //                    cellfb1.Colspan = 9;
        //                    cellfb1.MinimumHeight = 30;
        //                    cellfb1.Border = 0;
        //                    table10.AddCell(cellfb1);


        //                    PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
        //                    cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cellfb.PaddingLeft = 20;
        //                    cellfb.Colspan = 9;
        //                    cellfb.MinimumHeight = 30;
        //                    cellfb.Border = 0;
        //                    table10.AddCell(cellfb);

        //                    PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
        //                    cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
        //                    cellf1b.PaddingLeft = 20;
        //                    cellf1b.Colspan = 9;
        //                    cellf1b.Border = 0;

        //                    table10.AddCell(cellf1b);

        //                    PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
        //                    cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
        //                    cellh2.PaddingLeft = 20;
        //                    cellh2.Border = 0;
        //                    cellh2.Colspan = 9;
        //                    table10.AddCell(cellh2);

        //                    doc.Add(table10);
        //                }
        //            }

        //            doc.Close();


        //            Random r = new Random();
        //            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report";
        //            string Script = "";
        //            Script += "<script id='PopupWindow'>";
        //            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        //            Script += "confirmWin.Setfocus()</script>";
        //            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
        //                Page.RegisterClientScriptBlock("PopupWindow", Script);

        //            try
        //            {
        //                OdbcCommand cmd901 = new OdbcCommand();//"CALL selectdata(?,?)", conn);
        //                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
        //                cmd901.Parameters.AddWithValue("attribute", "max(slno)");
        //                DataTable dtt901 = new DataTable();
        //                dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
        //                id = int.Parse(dtt901.Rows[0][0].ToString());

        //                int tno = int.Parse(Session["tno"].ToString());
        //                string ct = Session["num"].ToString();
        //                OdbcCommand cmd25 = new OdbcCommand();//"call updatedata(?,?,?)", conn);
        //                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
        //                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + dt3 + "'");
        //                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
        //                int pi1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
        //            }
        //            catch
        //            {
        //                id = 1;
        //                int tno = int.Parse(Session["tno"].ToString());
        //                string ct = Session["num"].ToString();

        //                OdbcCommand cmd589 = new OdbcCommand();//"CALL savedata(?,?)", conn);
        //                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
        //                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + dt3 + "'");
        //                int pi2 = objcls.Procedures("CALL savedata(?,?)", cmd589);
        //            }
        //        }
        //    }
        //}
        //catch
        //{
        //    okmessage("Tsunami ARMS - Warning", "Problem found in taking report");
        //}

       #endregion

        #region new ledger
        DateTime rdate = DateTime.Now;
        string repdate = rdate.ToString("yyyy/MM/dd");
        string reptime = rdate.ToShortTimeString();

        try
        {
            if (txtdate.Text == "")
            {
                okmessage("Tsunami ARMS - Message", "Please Enter date");
                return;
            }



            string dt3 = objcls.yearmonthdate(txtdate.Text);
            Session["ledgerDate"] = dt3.ToString();

            string str1 = "2009-11-18 00:00:01";
            // str1 = mm + "-" + dd + "-" + yy;
            string str2 = "2010-09-21 23:59:59";
            // str2 = mm + "-" + dd + "-" + yy;
            DateTime ind11 = DateTime.Parse(str1);
            DateTime outd11 = DateTime.Parse(str2);
            DateTime chk = DateTime.Parse(dt3);
            if (chk > ind11 && chk < outd11)
            {
                string std1 = dt3 + " 00:00:01";
                string end1 = dt3 + " 23:59:59";

                string sql1 = "bill.receiptNo,room.Custname,room.ADDRESS1, "
                      + " bill.buildingName, bill.roomNo, "
                      + " room.Roomalloctime, room.VacTime,"
                      + " bill.rentAmt, bill.advanceAmt, bill.status";

                string sql2 = " relatingreceiptandbill as bill INNER JOIN "
                                + " room_transaction as room ON bill.rowId = room.RowID";

                string sql3 = "room.Roomalloctime between '" + std1 + "'  and '" + end1 + "' order by bill.receiptNo";



                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", sql2);
                cmd5.Parameters.AddWithValue("attribute", sql1);
                cmd5.Parameters.AddWithValue("conditionv", sql3);
                DataTable dtt5 = new DataTable();
                dtt5 = objcls.SpDtTbl("Call selectcond(?,?,?)", cmd5);

                if (dtt5.Rows.Count > 0)
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
                    float[] colWidths1 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table1.SetWidths(colWidths1);



                    string repdates = rdate.ToString("dd/MM/yyyy");
                    string dt1 = dt.ToString("dd/MM/yyyy");
                    string[] aa = dt3.Split('/');
                    DateTime ss = DateTime.Parse(dt3.ToString());
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

                    for (int ii = 0; ii < dtt5.Rows.Count; ii++)
                    {
                        if (i > 26)
                        {
                            doc.NewPage();
                            PdfPTable table4 = new PdfPTable(9);
                            float[] colWidths4 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                            table4.SetWidths(colWidths4);


                            PdfPTable table3 = new PdfPTable(9);
                            float[] colWidths3 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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

                            PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, fontLB)));
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
                        int j = ii + 1;
                        string no = aa[2] + " - " + j.ToString();
                        string Rec = dtt5.Rows[ii]["receiptNo"].ToString();
                        string name = dtt5.Rows[ii]["Custname"].ToString();
                        string addr = dtt5.Rows[ii]["ADDRESS1"].ToString();
                        string namadd = name + " ," + addr;
                        string build1 = "";
                        string building1 = dtt5.Rows[ii]["buildingname"].ToString();
                        if (building1.Contains("(") == true)
                        {
                            string[] buildS11, buildS21;
                            buildS11 = building1.Split('(');
                            build1 = buildS11[1];
                            buildS21 = build1.Split(')');
                            build1 = buildS21[0];
                            building1 = build1;
                        }
                        else if (building1.Contains("Cottage") == true)
                        {
                            building1 = building1.Replace("Cottage", "Cot");
                        }
                        string room = dtt5.Rows[ii]["roomNo"].ToString();
                        string buroom = building1 + " - " + room;
                        indat = DateTime.Parse(dtt5.Rows[ii]["Roomalloctime"].ToString());
                        ind = indat.ToString("dd-MMM");
                        it = indat.ToString("hh:mm:tt");
                        indate = it + "       " + ind;

                        outdat = DateTime.Parse(dtt5.Rows[ii]["VacTime"].ToString());
                        outd = outdat.ToString("dd-MMM");
                        ot = outdat.ToString("hh:mm:tt");
                        outdate = ot + "       " + outd;
                        string rent = dtt5.Rows[ii]["rentAmt"].ToString();
                        string advrent = dtt5.Rows[ii]["advanceAmt"].ToString();

                        string cancel = dtt5.Rows[ii]["status"].ToString();
                        if (cancel != "OK")
                        {
                            remarks = cancel;
                            rent = "0";
                            rrent1 = decimal.Parse(rent.ToString());
                            rrent = rrent + rrent1;

                            rr = rrent.ToString();
                            rdeposit1 = decimal.Parse(advrent.ToString());
                            rdeposit = rdeposit + rdeposit1;

                            dde = rdeposit.ToString();
                        }
                        else
                        {
                            remarks = "";
                            rrent1 = decimal.Parse(rent.ToString());
                            rrent = rrent + rrent1;

                            rr = rrent.ToString();
                            rdeposit1 = decimal.Parse(advrent.ToString());
                            rdeposit = rdeposit + rdeposit1;

                            dde = rdeposit.ToString();
                        }


                        PdfPTable table = new PdfPTable(9);
                        float[] colWidths6 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                        table.SetWidths(colWidths6);

                        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(no, font8)));
                        table.AddCell(cell21);

                        PdfPCell cell23g = new PdfPCell(new Phrase(new Chunk(Rec, font8)));
                        table.AddCell(cell23g);


                        PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(namadd, font8)));
                        table.AddCell(cell23);

                        PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(buroom, font8)));
                        table.AddCell(cell25);

                        PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(indate.ToString(), font8)));
                        table.AddCell(cell27);

                        PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(outdate.ToString(), font8)));
                        table.AddCell(cell28);

                        PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(rent, font8)));
                        table.AddCell(cell29);

                        PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(advrent, font8)));
                        table.AddCell(cell30);

                        PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(remarks, font8)));
                        table.AddCell(cell31);

                        doc.Add(table);
                        i++;

                        if ((i == 27) || (ii == dtt5.Rows.Count - 1))
                        {
                            PdfPTable table2 = new PdfPTable(9);
                            float[] colWidths2 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                            table2.SetWidths(colWidths2);

                            PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                            cell41.Colspan = 6;
                            table2.AddCell(cell41);

                            gtr = gtr + decimal.Parse(rr.ToString());
                            gtd = gtd + decimal.Parse(dde.ToString());


                            PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                            table2.AddCell(cell49);

                            PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                            table2.AddCell(cell50);

                            PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            table2.AddCell(cell51);

                            doc.Add(table2);

                            rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                        }

                        if (ii == dtt5.Rows.Count - 1)
                        {
                            PdfPTable table10 = new PdfPTable(9);
                            float[] colWidths10 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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

                            Int64 gt = Convert.ToInt64(gtr);
                            string re = objcls.NumberToTextWithLakhs(gt);
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

                            Int64 gtde = Convert.ToInt64(gtd);
                            string de = objcls.NumberToTextWithLakhs(gtde);
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
                }
            }
            else
            {

                OdbcCommand cmd550 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
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
                         + " Left join m_sub_district as dist on alloc.district_id=dist.district_id";

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
                               + "alloc.reason_id";


                    strsql3 = "alloc.room_id=room.room_id"
                      + " and room.build_id=build.build_id"
                      + " and dayend='" + dt3 + "' order by alloc_id asc";


                    OdbcCommand cmd350 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
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
                        Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
                        pdfPage page = new pdfPage();
                        page.strRptMode = "Allocation";
                        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                        wr.PageEvent = page;

                        doc.Open();

                        PdfPTable table1 = new PdfPTable(9);
                        float[] colWidths1 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                        table1.SetWidths(colWidths1);



                        string repdates = rdate.ToString("dd/MM/yyyy");
                        string dt1 = dt.ToString("dd/MM/yyyy");

                        DateTime ss = DateTime.Parse(dt3.ToString());
                        string dateee = ss.ToString("dd-MMMM-yyyy");

                        PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
                        cell500.Colspan = 8;
                        cell500.Border = 1;
                        cell500.HorizontalAlignment = 1;
                        table1.AddCell(cell500);

                        PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                        cell501.Colspan = 4;
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
                                PdfPTable table4 = new PdfPTable(9);
                                float[] colWidths4 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                                table4.SetWidths(colWidths4);


                                PdfPTable table3 = new PdfPTable(9);
                                float[] colWidths3 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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

                                i = 0;

                                doc.Add(table3);
                            }

                            PdfPTable table = new PdfPTable(9);
                            float[] colWidths ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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

                                    OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
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
                                    OdbcCommand cmdallocfr = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
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

                                OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
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

                                OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
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

                                OdbcCommand cmd115 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
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

                            outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                            outd = outdat.ToString("dd-MMM");
                            ot = outdat.ToString("hh:mm:tt");
                            outdate = ot + "       " + outd;

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
                                float[] colWidths2 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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
                                float[] colWidths10 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
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

                                NumberToEnglish n = new NumberToEnglish();
                                string re = n.changeNumericToWords(gtr.ToString());
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

                                string de = n.changeNumericToWords(gtd.ToString());
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
                            OdbcCommand cmd901 = new OdbcCommand();//"CALL selectdata(?,?)", conn);
                            cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                            cmd901.Parameters.AddWithValue("attribute", "max(slno)");
                            DataTable dtt901 = new DataTable();
                            dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);
                            id = int.Parse(dtt901.Rows[0][0].ToString());

                            int tno = int.Parse(Session["tno"].ToString());
                            string ct = Session["num"].ToString();
                            OdbcCommand cmd25 = new OdbcCommand();//"call updatedata(?,?,?)", conn);
                            cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                            cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + dt3 + "'");
                            cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                            int pi1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
                        }
                        catch
                        {
                            id = 1;
                            int tno = int.Parse(Session["tno"].ToString());
                            string ct = Session["num"].ToString();

                            OdbcCommand cmd589 = new OdbcCommand();//"CALL savedata(?,?)", conn);
                            cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                            cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + dt3 + "'");
                            int pi2 = objcls.Procedures("CALL savedata(?,?)", cmd589);
                        }
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
    # endregion

    #region OK Message

    public void okmessage(string head, string message)
    {
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender1.Show();
    }
    #endregion

    # region Reservation Chart 
    protected void btnChart_Click(object sender, EventArgs e)
    {

        DateTime ds2 = DateTime.Now;
        string datte, timme; string room = "", building = "", building1 = "", room1 = "";
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd/MM/yyyy");
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Reservation Chart" + transtim.ToString() + ".pdf";
        string Ddate, bdate;
        if (txtResDate.Text != "")
        {

            string dd = objcls.yearmonthdate(txtResDate.Text.ToString());
            bdate = dd.ToString();
            DateTime d4 = DateTime.Parse(dd);
            Ddate = d4.ToString("dd MMM yyyy");
        }
        else
        {
            bdate = gh.ToString("yyyy-MM-dd");
            Ddate = gh.ToString("dd MMM yyyy");

        }

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

        PdfPTable table2 = new PdfPTable(9);
        table2.TotalWidth = 560f;
        table2.LockedWidth = true;
        float[] colwidth1 ={ 1, 2, 2, 2, 3, 4, 4, 2, 3 };
        table2.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Reservation Chart", font10)));
        cell.Colspan = 9;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Date: " + Ddate, font10)));
        cella.Colspan = 9;
        cella.Border = 0;
        cella.HorizontalAlignment = 0;
        table2.AddCell(cella);


        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table2.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Customer Type", font9)));
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Pass No", font9)));
        table2.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Inmates Name", font9)));
        table2.AddCell(cell15);
        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Prop In Time", font9)));
        table2.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Prop Out Time", font9)));
        table2.AddCell(cell19);
        PdfPCell cell19b = new PdfPCell(new Phrase(new Chunk("Status", font9)));
        table2.AddCell(cell19b);
        PdfPCell cell19v = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
        table2.AddCell(cell19v);
        doc.Add(table2);

        OdbcCommand cmd31 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
        cmd31.Parameters.AddWithValue("tblname", " m_sub_building b,m_room r,t_roomreservation t left join t_donorpass p on t.pass_id=p.pass_id ");
        cmd31.Parameters.AddWithValue("attribute", "t.room_id,reservedate,expvacdate,altroom_id,buildingname,roomno,case reserve_mode when 'tdb' then 'TDB Res' when 'Donor Free' then 'Donor free' when 'Donor Paid' then 'Donor paid' END as Type,passno,swaminame,case status_reserve when '0' then 'Reserved' when '2' then 'Occupied' when '3' then 'Cancelled' end as status");

        cmd31.Parameters.AddWithValue("conditionv", " date(reservedate)='" + bdate.ToString() + "' and reserve_type!='direct' and r.room_id=t.room_id and r.build_id=b.build_id");



        DataTable dt = new DataTable();
        dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender1.Show();
            return;
        }

        int slno = 0, i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            slno = slno + 1;
            if (i > 40)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(9);
                table1.TotalWidth = 560f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 1, 2, 2, 2, 3, 4, 4, 2, 3 };
                table1.SetWidths(colwidth2);
                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("SlNo", font9)));
                table1.AddCell(cell11a);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("Customer Type", font9)));
                table1.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Pass No", font9)));
                table1.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Inmates Name", font9)));
                table1.AddCell(cell15a);
                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Prop In Time", font9)));
                table1.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Prop Out Time", font9)));
                table1.AddCell(cell19a);
                PdfPCell cell19c = new PdfPCell(new Phrase(new Chunk("Status", font9)));
                table1.AddCell(cell19c);
                PdfPCell cell19r = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
                table1.AddCell(cell19r);
                doc.Add(table1);
            }

            PdfPTable table = new PdfPTable(9);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            float[] colwidth4 ={ 1, 2, 2, 2, 3, 4, 4, 2, 3 };
            table.SetWidths(colwidth4);

            PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table.AddCell(cell11p);

            int RId;
            string AltRoom = dr["altroom_id"].ToString();
            if (AltRoom != "")
            {
                RId = Convert.ToInt32(dr["altroom_id"].ToString());

                //string sqq1 = "SELECT buildingname,roomno FROM m_sub_building b,m_room r WHERE room_id=" + RId + " and r.rowstatus<>'2' "
                //+"and b.rowstatus<>'2'";

                OdbcCommand sqq1 = new OdbcCommand();
                sqq1.Parameters.AddWithValue("tblname", "m_sub_building b,m_room r");
                sqq1.Parameters.AddWithValue("attribute", "buildingname,roomno");
                sqq1.Parameters.AddWithValue("conditionv", "room_id=" + RId + " and r.rowstatus<>'2' and b.rowstatus<>'2'");

                DataTable RooIdr = new DataTable(); ;
                RooIdr = objcls.SpDtTbl("call selectcond(?,?,?)", sqq1);
                if (RooIdr.Rows.Count > 0)
                {
                    //for (int i = 0; i < RooIdr.Rows.Count; i++)
                    //{
                    building1 = RooIdr.Rows[0]["buildingname"].ToString();
                    room1 = RooIdr.Rows[0]["roomno"].ToString();
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
                    //}

                }

            }

            building = dr["buildingname"].ToString();
            room = dr["roomno"].ToString();
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

            PdfPCell cell12p = new PdfPCell(new Phrase(new Chunk(building + " /  " + room, font8)));
            table.AddCell(cell12p);

            PdfPCell cell13p = new PdfPCell(new Phrase(new Chunk(dr["Type"].ToString(), font8)));
            table.AddCell(cell13p);
            try
            {
                PdfPCell cell13r = new PdfPCell(new Phrase(new Chunk(dr["passno"].ToString(), font8)));
                table.AddCell(cell13r);
            }
            catch
            {
                PdfPCell cell13r = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell13r);
            }
            try
            {
                PdfPCell cell14u = new PdfPCell(new Phrase(new Chunk(dr["swaminame"].ToString(), font8)));
                table.AddCell(cell14u);
            }
            catch
            {
                PdfPCell cell14u = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell14u);
            }

            try
            {
                DateTime ActVec1 = DateTime.Parse(dr["reservedate"].ToString());
                totime = ActVec1.ToString("dd-MM-yyyy hh:mm tt");

            }
            catch
            {
                totime = "";
            }

            PdfPCell cell14t = new PdfPCell(new Phrase(new Chunk(totime, font8)));
            table.AddCell(cell14t);
            string tttt;
            try
            {
                DateTime ActVec2 = DateTime.Parse(dr["expvacdate"].ToString());
                tttt = ActVec2.ToString("dd-MM-yyyy hh:mm tt");

            }
            catch
            {
                tttt = "";
            }
            PdfPCell cell14ta = new PdfPCell(new Phrase(new Chunk(tttt, font8)));
            table.AddCell(cell14ta);
            PdfPCell cell14ty = new PdfPCell(new Phrase(new Chunk(dr["status"].ToString(), font8)));
            table.AddCell(cell14ty);
            if (AltRoom != "")
            {
                PdfPCell cell14ti = new PdfPCell(new Phrase(new Chunk("A R for (" + building1 + " /" + room1 + ")", font8)));
                table.AddCell(cell14ti);
            }
            else
            {
                PdfPCell cell14ti = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table.AddCell(cell14ti);
            }
            i++;
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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Reservation Chart";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
       
    }

    # endregion

    # region Button reservation chart click
    protected void btnReservationChart_Click1(object sender, EventArgs e)
    {

        btnReservationChart.BackColor = System.Drawing.Color.Bisque;
        pnlcollectioncomp.Visible = false;
        pnlAccomodation.Visible = false;
        pnlReservation.Visible = true;
        pnlRoomStatus.Visible = false;

        pnlRoomstatusReport.Visible = false;

        pnlDonorpass.Visible = false;

        pnlNonvacating.Visible = false;

        pnlCollection.Visible = false;

        btnAccomodationLedger.BackColor = System.Drawing.Color.Plum;
        btnRoomManagement.BackColor = System.Drawing.Color.Plum;
        btnRoomStatus.BackColor = System.Drawing.Color.Plum;
        btnNonvacating.BackColor = System.Drawing.Color.Plum;
        btnDonorPass.BackColor = System.Drawing.Color.Plum;
        btncollectioncomparison.BackColor = System.Drawing.Color.Plum;
        btnCollection.BackColor = System.Drawing.Color.Plum;
        btnOtherReports.BackColor = System.Drawing.Color.Plum;



    }
    # endregion

    # region Occupied Room List
    protected void lnkOccupy_Click(object sender, EventArgs e)
    {
        
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd MMMM yyyy");
        timme = ds2.ToShortTimeString();
        string dd1 = ds2.ToString("yyyy-MM-dd");
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");

        string dd = objcls.yearmonthdate(txtDateReport.Text.ToString());
        DateTime d4 = DateTime.Parse(dd);
        string dd4 = d4.ToString("dd MMMM yyyy");
        string tt1 = ta.ToString("hh:mm tt");
        string bdate = dd.ToString() + " " + tt.ToString();

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "occupyingroom" + transtim.ToString() + ".pdf";

        string Sna1;
        int mal1 = 0;

       // string sqq2 = "select seasonname,season_id from m_season s,m_sub_season d where curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'";
       // OdbcCommand Malayalam = new OdbcCommand("select seasonname,season_id from m_season s,m_sub_season d where curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'", con);

        OdbcCommand sqq2 = new OdbcCommand();
        sqq2.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
        sqq2.Parameters.AddWithValue("attribute", "seasonname,season_id");
        sqq2.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");



        DataTable Malr = new DataTable();
        Malr = objcls.SpDtTbl("call selectcond(?,?,?)", sqq2);
        if (Malr.Rows.Count > 0)
        {
            mal1 = Convert.ToInt32(Malr.Rows[0][1].ToString());
            Sna1 = Malr.Rows[0][0].ToString();
        }


        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font10 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        pdfPage page = new pdfPage();
        page.strRptMode = "Occupying";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table2 = new PdfPTable(7);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth1 ={ 2, 3, 3, 3, 3, 3, 4 };
        table2.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("OCCUPANCY ROOM LIST on  " + dd4.ToString() + "   at  " + tt1, font9)));
        cell.Colspan = 7;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Sl No", font10)));
        cell11.Rowspan = 2;
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font10)));
        cell12.Rowspan = 2;
        table2.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Check In Time", font10)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font10)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table2.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Receipt No", font10)));
        cell15.Rowspan = 2;
        table2.AddCell(cell15);
        
        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font10)));
        table2.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font10)));
        table2.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font10)));
        table2.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font10)));
        table2.AddCell(cell21);
       
        doc.Add(table2);
        int i = 0;

        //OdbcCommand Vacate1 = new OdbcCommand("select a.adv_recieptno,a.room_id,roomno,b.buildingname,allocdate,exp_vecatedate,r.roomno from "
        //    + "t_roomallocation a,m_sub_building b,m_room r where '" + bdate.ToString() + "' between allocdate and exp_vecatedate and season_id="+mal1+" and "
        //    +"b.build_id=r.build_id and a.room_id=r.room_id and a.roomstatus='2' group by a.room_id", con);


        //OdbcCommand Vacate1 = new OdbcCommand("select a.adv_recieptno,a.room_id,roomno,b.buildingname,allocdate,exp_vecatedate,r.roomno from "
        //    + "t_roomallocation a,m_sub_building b,m_room r where ('" + bdate.ToString() + "' between allocdate and exp_vecatedate or exp_vecatedate<= '" + bdate.ToString() + "')"
        //    + "and b.build_id=r.build_id and a.room_id=r.room_id and a.roomstatus=2 group by a.room_id order by allocdate asc", con);

        //string sqq3 = "select a.adv_recieptno,a.room_id,roomno,b.buildingname,allocdate,exp_vecatedate,r.roomno from "
        //    + "t_roomallocation a,m_sub_building b,m_room r where ('" + bdate.ToString() + "' between allocdate and exp_vecatedate or exp_vecatedate<= '" + bdate.ToString() + "')"
        //    + "and b.build_id=r.build_id and a.room_id=r.room_id and a.roomstatus=2 group by a.room_id order by allocdate asc";

        OdbcCommand sqq3 = new OdbcCommand();
        sqq3.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r ");
        sqq3.Parameters.AddWithValue("attribute", " a.adv_recieptno,a.room_id,roomno,b.buildingname,allocdate,exp_vecatedate,r.roomno");
        sqq3.Parameters.AddWithValue("conditionv", "('" + bdate.ToString() + "' between allocdate and exp_vecatedate or exp_vecatedate<= '" + bdate.ToString() + "') and b.build_id=r.build_id and a.room_id=r.room_id and a.roomstatus=2 group by a.room_id order by allocdate asc");


        DataTable dtt351 = new DataTable();
        dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", sqq3);
        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();


            if (i > 45)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(7);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 2, 3, 3, 3, 3, 3, 4 };
                table1.SetWidths(colwidth2);
                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Sl No", font10)));
                cell11a.Rowspan = 2;
                table1.AddCell(cell11a);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font10)));
                cell12a.Rowspan = 2;
                table1.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("Check In Time", font10)));
                cell13a.HorizontalAlignment = 1;
                cell13.Colspan = 2;
                table1.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font10)));
                cell14a.HorizontalAlignment = 1;
                cell14.Colspan = 2;
                table1.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Receipt No", font10)));
                cell15a.Rowspan = 2;
                table1.AddCell(cell15a);
                //PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk("", font9)));
                //table1.AddCell(cell16a);
                //PdfPCell cell17a = new PdfPCell(new Phrase(new Chunk("", font9)));
                //table1.AddCell(cell17a);
                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font10)));
                table1.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font10)));
                table1.AddCell(cell19a);
                PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font10)));
                table1.AddCell(cell20a);
                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font10)));
                table1.AddCell(cell21a);
                //PdfPCell cell22a = new PdfPCell(new Phrase(new Chunk("", font9)));
                //table1.AddCell(cell22a);
                doc.Add(table1);
                //i = 0;
            }


            //no = no + 1;
            //num = no.ToString();
            PdfPTable table = new PdfPTable(7);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth3 ={ 2, 3, 3, 3, 3, 3, 4 };
            table.SetWidths(colwidth3);

            //string roomid = dtt351.Rows[ii]["room_id"].ToString();
            room = dtt351.Rows[ii]["roomno"].ToString();
            building = dtt351.Rows[ii]["buildingname"].ToString();

            //building = dtt350.Rows[ii]["buildingname"].ToString();
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

            fromdate = DateTime.Parse(dtt351.Rows[ii]["allocdate"].ToString());
            //frmdate = fromdate.ToString("dd-MM-yyyy");
            frmdate = fromdate.ToString("dd MMM yyyy");
            f = fromdate.ToString("dd");
            string ChTime = fromdate.ToString("hh:mm:ss tt");
            //fromtime = dtt351.Rows[ii]["alloctime"].ToString();
            todate = DateTime.Parse(dtt351.Rows[ii]["exp_vecatedate"].ToString());
            toodate = todate.ToString("dd MMM yyyy");
            string PrTime = todate.ToString("hh:mm:ss tt");
            //totime = dtt351.Rows[ii]["vectime"].ToString();
            int receipt = Convert.ToInt32(dtt351.Rows[ii]["adv_recieptno"].ToString());

            //PdfPTable table2 = new PdfPTable(7);
            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21b);

            PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building + " /  " + room, font8)));
            table.AddCell(cell22b);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            table.AddCell(cell23);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
            table.AddCell(cell23a);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
            table.AddCell(cell24);
            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
            table.AddCell(cell25);
            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(receipt.ToString() + "/ " + f, font8)));
            table.AddCell(cell26);
            i++;
            doc.Add(table);

        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font10)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);

        //PdfPCell cellaw1 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        //cellaw1.Border = 0;
        //table5.AddCell(cellaw1);
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
        //System.Diagnostics.Process.Start(pdfFilePath);
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Vacant Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);


       

    }
# endregion

    # region Over Stayed Room List
    protected void lnkOverStay_Click(object sender, EventArgs e)
    {
        
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        string ddh = ds2.ToString("yyyy-MM-dd");
        string dd = ds2.ToString("dd MMMM yyyy");

        string dd5 = objcls.yearmonthdate(txtDateReport.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string dd4 = d4.ToString("dd MMMM yyyy");

        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ttt = ta.ToString("hh:mm tt");

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "OverStayedRoom" + transtim.ToString() + ".pdf";

        string bdate = dd5.ToString() + " " + tt.ToString();


        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 40);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();


        PdfPTable table5 = new PdfPTable(7);
        table5.TotalWidth = 550f;
        table5.LockedWidth = true;
        float[] colwidth2 ={ 2, 6, 5, 5, 5, 5, 7 };
        table5.SetWidths(colwidth2);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("OVER STAYED ROOM LIST on  " + dd4.ToString() + "   at  " + ttt.ToString(), font10)));
        cell.Colspan = 7;
        cell.Border = 0;
        cell.HorizontalAlignment = 1;
        table5.AddCell(cell);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Sl No", font9)));
        cell11.Rowspan = 2;
        table5.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table5.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Check in Time", font9)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table5.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table5.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
        cell15.Rowspan = 2;
        table5.AddCell(cell15);

        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table5.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table5.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table5.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table5.AddCell(cell21);

        doc.Add(table5);
        int k = 0;

        //string sqq4 = "SELECT a.room_id,a.allocdate as allocdate,exp_vecatedate,a.adv_recieptno,b.buildingname,r.roomno FROM "
        //      + "t_roomallocation a,m_room r,m_sub_building b,t_roomvacate v WHERE a.room_id=r.room_id and r.build_id=b.build_id and a.alloc_id=v.alloc_id "
        //      + "and a.exp_vecatedate < v.actualvecdate  and '" + bdate.ToString() + "' between allocdate and exp_vecatedate  group by a.room_id union "
        //      + "SELECT a.room_id,a.allocdate as allocdate,a.exp_vecatedate as exp_vecatedate,a.adv_recieptno,b.buildingname,r.roomno FROM "
        //      + "t_roomallocation a,m_room r,m_sub_building b WHERE a.room_id=r.room_id and r.build_id=b.build_id and a.exp_vecatedate < '" + bdate.ToString() + "' "
        //      + "and a.roomstatus=2 group by a.room_id";

        string cc = "a.room_id=r.room_id and r.build_id=b.build_id and a.alloc_id=v.alloc_id "
                         + "and a.exp_vecatedate < v.actualvecdate  and '" + bdate.ToString() + "' between allocdate and exp_vecatedate  group by a.room_id union "
                         + "SELECT a.room_id,a.allocdate as allocdate,a.exp_vecatedate as exp_vecatedate,a.adv_recieptno,b.buildingname,r.roomno FROM "
                         + "t_roomallocation a,m_room r,m_sub_building b WHERE a.room_id=r.room_id and r.build_id=b.build_id and a.exp_vecatedate < '" + bdate.ToString() + "' "
                         + "and a.roomstatus=2 group by a.room_id";

        OdbcCommand sqq4 = new OdbcCommand();
        sqq4.Parameters.AddWithValue("tblname", "t_roomallocation a,m_room r,m_sub_building b,t_roomvacate v");
        sqq4.Parameters.AddWithValue("attribute", "a.room_id,a.allocdate as allocdate,exp_vecatedate,a.adv_recieptno,b.buildingname,r.roomno");
        sqq4.Parameters.AddWithValue("conditionv", cc);

        DataTable dtt351 = new DataTable();
        dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", sqq4);


        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();
            if (k > 45)// total rows on page
            {
                k = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(7);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth3 ={ 2, 6, 5, 5, 5, 5, 7 };
                table1.SetWidths(colwidth3);

                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Sl No", font9)));
                cell11a.Rowspan = 2;
                table1.AddCell(cell11a);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                cell12a.Rowspan = 2;
                table1.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("Check in Time", font9)));
                cell13a.HorizontalAlignment = 1;
                cell13a.Colspan = 2;
                table1.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
                cell14a.HorizontalAlignment = 1;
                cell14a.Colspan = 2;
                table1.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
                cell15a.Rowspan = 2;
                table1.AddCell(cell15a);

                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell19a);
                PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell20a);
                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell21a);

                doc.Add(table1);

            }
            PdfPTable table = new PdfPTable(7);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth1 ={ 2, 6, 5, 5, 5, 5, 7 };
            table.SetWidths(colwidth1);

            string ChTime, PrTime;

            room = dtt351.Rows[ii]["roomno"].ToString();
            building = dtt351.Rows[ii]["buildingname"].ToString();
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

            try
            {
                fromdate = DateTime.Parse(dtt351.Rows[ii]["allocdate"].ToString());
                frmdate = fromdate.ToString("dd MMM");
                ChTime = fromdate.ToString("hh:mm tt");
                f = fromdate.ToString("dd");

            }
            catch
            {
                frmdate = " ";
                ChTime = " ";
            }
            try
            {

                todate = DateTime.Parse(dtt351.Rows[ii]["exp_vecatedate"].ToString());
                toodate = todate.ToString("dd MMM");
                PrTime = todate.ToString("hh:mm tt");
            }
            catch
            {
                toodate = " ";
                PrTime = "";
            }

            int receipt = Convert.ToInt32(dtt351.Rows[ii]["adv_recieptno"].ToString());

            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21b);

            PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
            table.AddCell(cell22b);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            table.AddCell(cell23);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
            table.AddCell(cell23a);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
            table.AddCell(cell24);
            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
            table.AddCell(cell25);
            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(receipt.ToString() + "/ " + f, font8)));
            table.AddCell(cell26);
            k++;
            doc.Add(table);

        }
        //doc.Add(table);
        PdfPTable table6 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table6.AddCell(cellaw);

        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table6.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table6.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table6.AddCell(cellaw4);



        doc.Add(table6);
        doc.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Over Stayed Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        con.Close();
    }

    # endregion

    # region Non Occupied reserved Room List
    protected void lnknonoccupReserve_Click1(object sender, EventArgs e)
    {
        con = objcls.NewConnection();
        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");
        string dd5 = objcls.yearmonthdate(txtDateReport.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string d44 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();


        OdbcCommand cvz = new OdbcCommand("ALTER VIEW tempnonoccupy AS SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from "
            + "t_roomreservation WHERE status_reserve='0' and ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE "
            + "reqtype='Donor Free Allocation' and rowstatus<>'2' and (('" + bdate.ToString() + "' between fromdate and todate) or ('" + bdate.ToString() + "'>=fromdate and "
            + "todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<'" + bdate.ToString() + "' and reserve_mode='donor free'  UNION "
            + "(SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from t_roomreservation WHERE status_reserve='0' and "
            + "ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE reqtype='Donor Paid Allocation' and rowstatus<>'2' and "
            + "(('" + bdate.ToString() + "' between fromdate and todate) or ('" + bdate.ToString() + "'>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<'" + bdate.ToString() + "' "
            + "and reserve_mode='donor paid') UNION (SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from t_roomreservation "
            + "WHERE status_reserve='0' and ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE reqtype='TDB Allocation' "
            + "and rowstatus<>'2' and (('" + bdate.ToString() + "' between fromdate and todate) or ('" + bdate.ToString() + "'>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours')"
            + ",0,0))<'" + bdate.ToString() + "' and reserve_mode='tdb') order by reserve_id asc", con);
        cvz.ExecuteNonQuery();



        pnlMessage.Visible = true;
        if (txtTime.Text.ToString() == "")
        {
            lblOk.Text = "Please enter time"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender1.Show();
            return;
        }
        else
        { }

        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        string dd = ds2.ToString("yyyy-MM-dd");

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "NonoccupiedReservedRoom" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Nonoccupy";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;

        doc.Open();


        PdfPTable table2 = new PdfPTable(5);
        float[] colwidth2 ={ 2, 5, 7, 6, 6 };
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        table2.SetWidths(colwidth2);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("UNOCCUPIED RESERVED ROOM LIST", font10)));
        cell.Colspan = 5;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);
        PdfPCell cellP = new PdfPCell(new Phrase(new Chunk("Date:  " + datte, font9)));
        cellP.Colspan = 3;
        cellP.Border = 0;
        cellP.HorizontalAlignment = 0;
        table2.AddCell(cellP);

        PdfPCell celli = new PdfPCell(new Phrase(new Chunk("Time:  " + Atime.ToString(), font9)));
        celli.Colspan = 2;
        celli.Border = 0;
        celli.HorizontalAlignment = 0;
        table2.AddCell(celli);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table2.AddCell(cell11);

        PdfPCell cell123 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table2.AddCell(cell123);

        PdfPCell cell113 = new PdfPCell(new Phrase(new Chunk("Proposed in time", font9)));
        cell113.HorizontalAlignment = 0;
        table2.AddCell(cell113);

        PdfPCell cell133 = new PdfPCell(new Phrase(new Chunk("Res Type", font9)));
        table2.AddCell(cell133);
        PdfPCell cell1331 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
        table2.AddCell(cell1331);

        doc.Add(table2);

        int i = 0;
        //string aaa = "select distinct t.room_id,t.swaminame,t.reservedate,r.roomno,b.buildingname from tempnonoccupy t,m_sub_building b,m_room r where t.room_id=r.room_id and r.build_id=b.build_id and t.status_reserve='0'and reservedate>'" + bdate.ToString() + "'";
        //OdbcCommand Nonoccupy1 = new OdbcCommand("select distinct t.room_id,t.swaminame,t.reservedate,case t.reserve_mode when 'Donor Free' then 'Donor Free' "
        //                + "when 'Donor Paid' then 'Donor Paid' when 'Tdb' then 'TDB' END as reserve_mode,r.roomno,b.buildingname from tempnonoccupy t,"
        //                + "m_sub_building b,m_room r where t.room_id=r.room_id and r.build_id=b.build_id and t.status_reserve='0'and reservedate"
        //                + "<='" + bdate.ToString() + "' group by t.room_id order by t.reservedate asc", con);

        //string sqq5 = "select distinct t.room_id,t.swaminame,t.reservedate,case t.reserve_mode when 'Donor Free' then 'Donor Free' "
        //                + "when 'Donor Paid' then 'Donor Paid' when 'Tdb' then 'TDB' END as reserve_mode,r.roomno,b.buildingname from tempnonoccupy t,"
        //                + "m_sub_building b,m_room r where t.room_id=r.room_id and r.build_id=b.build_id and t.status_reserve='0'and reservedate"
        //                + "<='" + bdate.ToString() + "' group by t.room_id order by t.reservedate asc";

        OdbcCommand sqq5 = new OdbcCommand();
        sqq5.Parameters.AddWithValue("tblname", "tempnonoccupy t,m_sub_building b,m_room r");
        sqq5.Parameters.AddWithValue("attribute", "distinct t.room_id,t.swaminame,t.reservedate,case t.reserve_mode when 'Donor Free' then 'Donor Free' when 'Donor Paid' then 'Donor Paid' when 'Tdb' then 'TDB' END as reserve_mode,r.roomno,b.buildingname ");
        sqq5.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and r.build_id=b.build_id and t.status_reserve='0'and reservedate <='" + bdate.ToString() + "' group by t.room_id order by t.reservedate asc ");

       
        DataTable dtt22 = new DataTable();
        dtt22 = objcls.SpDtTbl("call selectcond(?,?,?)", sqq5);
        for (int ii = 0; ii < dtt22.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();

            if (i > 45)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(5);
                float[] colwidth3 ={ 2, 5, 7, 6, 6 };
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                table1.SetWidths(colwidth3);

                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table1.AddCell(cell11a);

                PdfPCell cell12a1 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell12a1);

                PdfPCell cell112a = new PdfPCell(new Phrase(new Chunk("Proposed in time", font9)));
                table1.AddCell(cell112a);

                PdfPCell cell113a = new PdfPCell(new Phrase(new Chunk("Res Type", font9)));
                table1.AddCell(cell113a);

                PdfPCell cell12a2 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
                table1.AddCell(cell12a2);
                doc.Add(table1);
            }

            PdfPTable table = new PdfPTable(5);
            table.TotalWidth = 550f;
            table.LockedWidth = true;

            float[] colwidth1 ={ 2, 5, 7, 6, 6 };
            table.SetWidths(colwidth1);



            building = dtt22.Rows[ii]["buildingname"].ToString();
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

            string roomid = dtt22.Rows[ii]["room_id"].ToString();
            room = dtt22.Rows[ii]["roomno"].ToString();
            fromdate = DateTime.Parse(dtt22.Rows[ii]["reservedate"].ToString());
            frmdate = fromdate.ToString("dd MMM");
            totime = fromdate.ToString("hh:mm tt");
            string Name = dtt22.Rows[ii]["reserve_mode"].ToString();
            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21);


            PdfPCell cell24a = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
            table.AddCell(cell24a);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate + "  " + totime, font8)));
            table.AddCell(cell23);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(Name, font8)));
            table.AddCell(cell24);


            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk("", font8)));
            table.AddCell(cell25);

            i++;
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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Non Occupying Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        con.Close();

    }
    # endregion

    # region Vacant Room Report
    protected void lnkVacant_Click(object sender, EventArgs e)
    {
        
        con = objcls.NewConnection();
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        //string dd = ds2.ToString("yyyy-MM-dd");
        string Atime = txtTime.Text.ToString();
        //DateTime d1 = DateTime.Parse();
        string dd = objcls.yearmonthdate(txtDateReport.Text.ToString());
        DateTime d4 = DateTime.Parse(dd);
        string dd4 = d4.ToString("dd MMMM yyyy");

        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string tim = ta.ToString("hh:mm tt");
        //string bdate = dd.ToString() + " " + tt.ToString();
        string transtim = ds2.ToString("dd-MM-yyyy HH-mm tt");
        string ch = "vacantroom" + transtim.ToString() + ".pdf";

        string bdate = dd.ToString() + " " + tt.ToString();

        OdbcCommand cvz = new OdbcCommand("ALTER VIEW tempnonoccupy AS SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from "
            + "t_roomreservation WHERE status_reserve='0' and ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE "
            + "reqtype='Donor Free Allocation' and rowstatus<>'2' and (('" + bdate.ToString() + "' between fromdate and todate) or ('" + bdate.ToString() + "'>=fromdate and "
            + "todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<'" + bdate.ToString() + "' and reserve_mode='donor free'  UNION "
            + "(SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from t_roomreservation WHERE status_reserve='0' and "
            + "ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE reqtype='Donor Paid Allocation' and rowstatus<>'2' and "
            + "(('" + bdate.ToString() + "' between fromdate and todate) or ('" + bdate.ToString() + "'>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<'" + bdate.ToString() + "' "
            + "and reserve_mode='donor paid') UNION (SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from t_roomreservation "
            + "WHERE status_reserve='0' and ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE reqtype='TDB Allocation' "
            + "and rowstatus<>'2' and (('" + bdate.ToString() + "' between fromdate and todate) or ('" + bdate.ToString() + "'>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours')"
            + ",0,0))<'" + bdate.ToString() + "' and reserve_mode='tdb') order by reserve_id asc", con);
        cvz.ExecuteNonQuery();



        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Vacant Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();


        PdfPTable table2 = new PdfPTable(4);
        table2.TotalWidth = 400f;
        table2.LockedWidth = true;
        float[] colwidth2 ={ 1, 2, 2, 4 };
        table2.SetWidths(colwidth2);
        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("VACANT ROOM LIST on  " + dd4.ToString() + "   at  " + tim, font10)));
        cell.Colspan = 4;


        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
        table2.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Status", font9)));
        table2.AddCell(cell14);
        doc.Add(table2);


        int j = 0;
        int roomid1 = -1;
        //OdbcCommand Vacate = new OdbcCommand("select distinct room_id,roomno,buildingname,case roomstatus when '1' then 'Vacant' end Status from m_room r,m_sub_building b where r.build_id=b.build_id and r.roomstatus='1' and r.rowstatus<>'2'", con);
        //OdbcCommand Vacate = new OdbcCommand("select distinct r.room_id,roomno,buildingname,case roomstatus when '1' then 'Vacant' end Status from m_room r,"
        //                + "m_sub_building b where r.build_id=b.build_id and r.roomstatus='1' and r.rowstatus<>'2' and r.room_id not in (select room_id  "
        //                + "from t_roomallocation a, t_roomvacate v where '"+bdate.ToString()+"'between allocdate and actualvecdate and "
        //                + "a.alloc_id=v.alloc_id) group by r.room_id", con);


        //OdbcCommand Vacate = new OdbcCommand("select distinct r.room_id,roomno,buildingname,case roomstatus when '1' then 'Vacant' end Status from m_room r, "
        //        + "m_sub_building b where r.build_id=b.build_id and r.roomstatus='1' and r.rowstatus<>'2' and r.room_id not in (select room_id  from "
        //        + "t_roomallocation a, t_roomvacate v where '" + bdate.ToString() + "' between allocdate and actualvecdate and a.alloc_id=v.alloc_id) "
        //   + " union "
        //        + "select t.room_id,roomno,buildingname,case status_reserve when '0' then 'Unoccupied Reserved Room' end as Status from tempnonoccupy t,"
        //        + "m_sub_building b,m_room r where status_reserve='0' and reservedate<='" + bdate.ToString() + "' and r.room_id=t.room_id and r.build_id=b.build_id "
        //        + "group by room_id", con);

        //string sqq6 = "select distinct r.room_id,roomno,buildingname,case roomstatus when '1' then 'Vacant' end Status from m_room r, "
        //        + "m_sub_building b where r.build_id=b.build_id and r.roomstatus='1' and r.rowstatus<>'2' and r.room_id not in (select room_id  from "
        //        + "t_roomallocation a, t_roomvacate v where '" + bdate.ToString() + "' between allocdate and actualvecdate and a.alloc_id=v.alloc_id) "
        //   + " union "
        //        + "select t.room_id,roomno,buildingname,case status_reserve when '0' then 'Unoccupied Reserved Room' end as Status from tempnonoccupy t,"
        //        + "m_sub_building b,m_room r where status_reserve='0' and reservedate<='" + bdate.ToString() + "' and r.room_id=t.room_id and r.build_id=b.build_id "
        //        + "group by room_id";


        OdbcCommand sqq6 = new OdbcCommand();
        sqq6.Parameters.AddWithValue("tblname", "m_room r, m_sub_building b ");
        sqq6.Parameters.AddWithValue("attribute", "distinct r.room_id,roomno,buildingname,case roomstatus when '1' then 'Vacant' end Status");
        sqq6.Parameters.AddWithValue("conditionv", "r.build_id=b.build_id and r.roomstatus='1' and r.rowstatus<>'2' and r.room_id not in (select room_id  from  t_roomallocation a, t_roomvacate v where '" + bdate.ToString() + "' between allocdate and actualvecdate and a.alloc_id=v.alloc_id) union select t.room_id,roomno,buildingname,case status_reserve when '0' then 'Unoccupied Reserved Room' end as Status from tempnonoccupy t, m_sub_building b,m_room r where status_reserve='0' and reservedate<='" + bdate.ToString() + "' and r.room_id=t.room_id and r.build_id=b.build_id group by room_id");

        DataTable dtt351 = new DataTable();
        dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", sqq6);
        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();

            if (j > 42)// total rows on page
            {
                j = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(4);
                table1.TotalWidth = 400f;
                table1.LockedWidth = true;
                float[] colwidth3 ={ 1, 2, 2, 4 };
                table1.SetWidths(colwidth3);

                PdfPCell cell11v = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table1.AddCell(cell11v);
                //PdfPCell cell12v = new PdfPCell(new Phrase(new Chunk("Room Id", font8)));
                //table.AddCell(cell12v);
                PdfPCell cell13v = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
                table1.AddCell(cell13v);
                PdfPCell cell14v = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell14v);
                PdfPCell cell15v = new PdfPCell(new Phrase(new Chunk("Status", font9)));
                table1.AddCell(cell15v);

                //sno = sno + 1;
                doc.Add(table1);
            }

            PdfPTable table = new PdfPTable(4);
            table.TotalWidth = 400f;
            table.LockedWidth = true;
            float[] colwidth1 ={ 1, 2, 2, 4 };
            table.SetWidths(colwidth1);

            int roomid2 = Convert.ToInt32(dtt351.Rows[ii]["room_id"].ToString());
            if (roomid2 != roomid1)
            {

                string roomid = dtt351.Rows[ii]["room_id"].ToString();
                room = dtt351.Rows[ii]["roomno"].ToString();
                building = dtt351.Rows[ii]["buildingname"].ToString();
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

                string status = dtt351.Rows[ii]["Status"].ToString();
                PdfPCell cell21y = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table.AddCell(cell21y);

                PdfPCell cell22y = new PdfPCell(new Phrase(new Chunk(building, font8)));
                table.AddCell(cell22y);

                PdfPCell cell23ay = new PdfPCell(new Phrase(new Chunk(room, font8)));
                table.AddCell(cell23ay);

                PdfPCell cell24y = new PdfPCell(new Phrase(new Chunk(status, font8)));
                table.AddCell(cell24y);
                j++;
                roomid1 = Convert.ToInt32(dtt351.Rows[ii]["room_id"].ToString());
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

        if (dtt351.Rows.Count == 0)
        {
            lblOk.Text = "No rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            //pnlOK1.Visible = false;
            pnlYesNo.Visible = false;
            ModalPopupExtender1.Show();

            doc.Add(table5);
            doc.Close();
            return;
        }


        doc.Close();
        //System.Diagnostics.Process.Start(pdfFilePath);
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Vacant Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        con.Close();

    }
    # endregion

    # region Extended Stay Report
    protected void lnkExtended_Click(object sender, EventArgs e)
    {
       
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.ConnectionString = strConnection;
        //    con.Open();
        //}
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd MMMM yyyy");
        timme = ds2.ToShortTimeString();


        string transtim = ds2.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Extendedroom" + transtim.ToString() + ".pdf";
        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");
        string dd5 = objcls.yearmonthdate(txtDateReport.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string d44 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Extended Stay";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();


        PdfPTable table2 = new PdfPTable(12);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;

        float[] colwidth2 ={ 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
        table2.SetWidths(colwidth2);



        int Sna1;
        //OdbcCommand Malayalam = new OdbcCommand("select seasonname,season_id from m_season s,m_sub_season d where curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'", con);
     // string sqq7=  "select seasonname,season_id from m_season s,m_sub_season d where curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'";

        OdbcCommand sqq7 = new OdbcCommand();
        sqq7.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d ");
        sqq7.Parameters.AddWithValue("attribute", "seasonname,season_id ");
        sqq7.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");


        DataTable Malr = new DataTable();
        Malr = objcls.SpDtTbl("call selectcond(?,?,?)", sqq7);
        if (Malr.Rows.Count > 0)
        {
            Sna1 = Convert.ToInt32(Malr.Rows[0][1].ToString());
            mal = Malr.Rows[0][0].ToString();
        }

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("EXTENDED ROOM LIST on  " + d44.ToString() + "   at  " + ta1, font10)));
        cell.Colspan = 12;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cell11o = new PdfPCell(new Phrase(new Chunk("Date :  " + d44.ToString(), font11)));
        cell11o.Colspan = 6;
        cell11o.Border = 0;
        cell11o.HorizontalAlignment = 0;
        table2.AddCell(cell11o);
        PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Season:  " + mal, font11)));
        cell11p.Colspan = 6;
        cell11p.Border = 0;
        cell11p.HorizontalAlignment = 1;
        table2.AddCell(cell11p);


        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        cell11.Rowspan = 2;
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table2.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Check In Time", font9)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table2.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Recpt No Old", font9)));
        cell15.Rowspan = 2;
        table2.AddCell(cell15);

        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Extd check in Time", font9)));
        cell16.Colspan = 2;
        table2.AddCell(cell16);
        PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("Extd vacating Time", font9)));
        cell17.Colspan = 2;
        table2.AddCell(cell17);
        PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk("Recpt No New", font9)));
        cell26.Rowspan = 2;
        table2.AddCell(cell26);

        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell21);
        PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell22);
        PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell23);
        PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell24);
        PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell25);
        doc.Add(table2);
        int i = 0; int Realloc = 0;

        //OdbcCommand Extend = new OdbcCommand("SELECT a.alloc_id,realloc_from,adv_recieptno,allocdate,exp_vecatedate from t_roomallocation a,t_roomvacate v "
        //       + "where realloc_from is not null and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate "
        //       + "and a.realloc_from=v.alloc_id group by alloc_id  order by realloc_from asc", con);
        //OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Extend);

        //string sqq8 = "SELECT a.alloc_id,realloc_from,adv_recieptno,allocdate,exp_vecatedate from t_roomallocation a,t_roomvacate v "
        //       + "where realloc_from is not null and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate "
        //       + "and a.realloc_from=v.alloc_id group by alloc_id  order by realloc_from asc";

        OdbcCommand sqq8 = new OdbcCommand();
        sqq8.Parameters.AddWithValue("tblname", "t_roomallocation a,t_roomvacate v");
        sqq8.Parameters.AddWithValue("attribute", "a.alloc_id,realloc_from,adv_recieptno,allocdate,exp_vecatedate");
        sqq8.Parameters.AddWithValue("conditionv", "realloc_from is not null and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate and a.realloc_from=v.alloc_id group by alloc_id  order by realloc_from asc");


        DataTable dtt351 = new DataTable();
        dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", sqq8);

        foreach (DataRow dr in dtt351.Rows)
        {
            no = no + 1;
            num = no.ToString();

            if (i > 45)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(12);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth4 ={ 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
                table1.SetWidths(colwidth4);


                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("No", font9)));
                cell11a.Rowspan = 2;
                table1.AddCell(cell11a);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                cell12a.Rowspan = 2;
                table1.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("Check In Time", font9)));
                cell13a.Colspan = 2;
                cell13a.HorizontalAlignment = 1;
                table1.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
                cell14a.HorizontalAlignment = 1;
                cell14a.Colspan = 2;
                table1.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
                cell15a.Rowspan = 2;
                table1.AddCell(cell15a);

                PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk("Extd check in Time", font9)));
                cell16a.Colspan = 2;
                table1.AddCell(cell16a);
                PdfPCell cell17a = new PdfPCell(new Phrase(new Chunk("Extd vacating Time", font9)));
                cell17a.Colspan = 2;
                table1.AddCell(cell17a);
                PdfPCell cell26a = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
                cell26a.Rowspan = 2;
                table1.AddCell(cell26a);

                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell19a);
                PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell20a);
                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell21a);
                PdfPCell cell22a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell22a);
                PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell23a);
                PdfPCell cell24a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell24a);
                PdfPCell cell25a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell25a);
                //i = 0;
                doc.Add(table1);
            }

            PdfPTable table = new PdfPTable(12);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth1 ={ 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
            table.SetWidths(colwidth1);

            Realloc = Convert.ToInt32(dr["realloc_from"].ToString());
            string dd = "SELECT a.room_id,Date_format(a.allocdate,'%d-%m-%y %l:%i %p') as allocdate,a.adv_recieptno,b.buildingname,r.roomno,Date_format(exp_vecatedate,'%d-%m-%y %l:%i %p') as exp_vecatedate from t_roomallocation a,m_room r,m_sub_building b where a.alloc_id=" + Realloc + " and a.room_id=r.room_id and b.build_id=r.build_id";
            //OdbcCommand Exten = new OdbcCommand("SELECT a.room_id,a.allocdate,a.adv_recieptno,b.buildingname,r.roomno,a.exp_vecatedate from t_roomallocation a,m_room r,m_sub_building b where a.alloc_id=" + Realloc + " and a.room_id=r.room_id and b.build_id=r.build_id order by a.alloc_id asc", con);
           
            //string sqq11 = "SELECT a.room_id,a.allocdate,a.adv_recieptno,b.buildingname,r.roomno,a.exp_vecatedate from t_roomallocation a,m_room r,m_sub_building b where a.alloc_id=" + Realloc + " and a.room_id=r.room_id and b.build_id=r.build_id order by a.alloc_id asc";

            OdbcCommand sqq11 = new OdbcCommand();
            sqq11.Parameters.AddWithValue("tblname", "t_roomallocation a,m_room r,m_sub_building b");
            sqq11.Parameters.AddWithValue("attribute", "a.room_id,a.allocdate,a.adv_recieptno,b.buildingname,r.roomno,a.exp_vecatedate ");
            sqq11.Parameters.AddWithValue("conditionv", "a.alloc_id=" + Realloc + " and a.room_id=r.room_id and b.build_id=r.build_id order by a.alloc_id asc");



            DataTable Extr = new DataTable();
            Extr = objcls.SpDtTbl("call selectcond(?,?,?)", sqq11);
            if (Extr.Rows.Count > 0)
            {

                room = Extr.Rows[0]["roomno"].ToString();
                building = Extr.Rows[0]["buildingname"].ToString();
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
                fromdate = DateTime.Parse(Extr.Rows[0]["allocdate"].ToString());
                frmdate = fromdate.ToString("dd MMM");
                f = fromdate.ToString("dd");
                string ChTime = fromdate.ToString("hh:mm tt");
                todate = DateTime.Parse(Extr.Rows[0]["exp_vecatedate"].ToString());
                toodate = todate.ToString("dd MMM");
                string PrTime = todate.ToString("hh:mm tt");
                int receipt = Convert.ToInt32(Extr.Rows[0]["adv_recieptno"].ToString());
                DateTime Efrom = DateTime.Parse(dr["allocdate"].ToString());
                string Efrom1 = Efrom.ToString("dd MMM");
                f1 = Efrom.ToString("dd");
                string ETime = Efrom.ToString("hh:mm tt");
                DateTime Eto = DateTime.Parse(dr["exp_vecatedate"].ToString());
                string Eto1 = Eto.ToString("dd MMM");
                string Etotime = Eto.ToString("hh:mm tt");
                int Extreceipt = Convert.ToInt32(dr["adv_recieptno"].ToString());

                PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table.AddCell(cell21b);

                PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
                table.AddCell(cell22b);

                PdfPCell cell23k = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
                table.AddCell(cell23k);
                PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
                table.AddCell(cell23a);

                PdfPCell cell24k = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
                table.AddCell(cell24k);
                PdfPCell cell25k = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
                table.AddCell(cell25k);
                PdfPCell cell26k = new PdfPCell(new Phrase(new Chunk(receipt.ToString() + "/ " + f, font8)));
                table.AddCell(cell26k);

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(Efrom1, font8)));
                table.AddCell(cell27);
                PdfPCell cell23n = new PdfPCell(new Phrase(new Chunk(ETime, font8)));
                table.AddCell(cell23n);

                PdfPCell cell24n = new PdfPCell(new Phrase(new Chunk(Eto1, font8)));
                table.AddCell(cell24n);
                PdfPCell cell25n = new PdfPCell(new Phrase(new Chunk(Etotime, font8)));
                table.AddCell(cell25n);
                PdfPCell cell26n = new PdfPCell(new Phrase(new Chunk(Extreceipt.ToString() + "/ " + f1, font8)));
                table.AddCell(cell26n);
                i++;
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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Vacant Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        con.Close();

    }
    # endregion

    # region Blocked Room Report
    protected void lnkBlocked_Click(object sender, EventArgs e)
    {
        
        int no = 0;
        string curdate;
        DateTime dat;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        string toodate;
        //string Atime = txtTime.Text.ToString();
        //DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        //string tt = ta.ToString("H:mm");
        string dd = objcls.yearmonthdate(txtDateReport.Text.ToString());
        DateTime d4 = DateTime.Parse(dd);
        string dd4 = d4.ToString("dd MMMM yyyy");
        // string bdate = dd.ToString() + " " + tt.ToString();

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "blockedroom" + transtim.ToString() + ".pdf";

        DataTable dtt351 = new DataTable();
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();


        PdfPTable table2 = new PdfPTable(7);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth2 ={ 2, 6, 5, 5, 5, 5, 7 };
        table2.SetWidths(colwidth2);


        PdfPCell cellq = new PdfPCell(new Phrase(new Chunk("BLOCKED ROOM LIST on  " + dd4.ToString(), font10)));
        cellq.Colspan = 7;
        cellq.Border = 1;
        cellq.HorizontalAlignment = 1;
        table2.AddCell(cellq);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
        cell11.Rowspan = 2;
        table2.AddCell(cell11);

        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table2.AddCell(cell12);


        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Blocked", font9)));
        cell14.Colspan = 2;
        cell14.HorizontalAlignment = 1;
        table2.AddCell(cell14);

        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Exp release", font9)));
        cell15.Colspan = 2;
        cell15.HorizontalAlignment = 1;
        table2.AddCell(cell15);

        PdfPCell cell171 = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
        cell171.Rowspan = 2;
        table2.AddCell(cell171);
        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell16);
        PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell17);
        PdfPCell cell16p = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell16p);
        PdfPCell cell17p = new PdfPCell(new Phrase(new Chunk("Time", font8)));
        table2.AddCell(cell17p);
        doc.Add(table2);
        int i = 0;


        //OdbcCommand Block = new OdbcCommand("select distinct t.room_id,todate,fromdate,totime,fromtime,reason,buildingname,roomno from t_manage_room t,"
        //    + "m_sub_building b,m_room r where t.roomstatus='3' and t.room_id in (select distinct room_id from m_room where roomstatus='3' and "
        //    + "rowstatus<>'2') and r.build_id=b.build_id and t.room_id=r.room_id and ('" + dd.ToString() + "' between fromdate and todate or "
        //    + "todate<='" + dd.ToString() + "') group by t.room_id order by fromdate asc,t.room_id asc", con);
        //OdbcDataAdapter dacnt351 = new OdbcDataAdapter(Block);

        //string sqq12 = "select distinct t.room_id,todate,fromdate,totime,fromtime,reason,buildingname,roomno from t_manage_room t,"
        //    + "m_sub_building b,m_room r where t.roomstatus='3' and t.room_id in (select distinct room_id from m_room where roomstatus='3' and "
        //    + "rowstatus<>'2') and r.build_id=b.build_id and t.room_id=r.room_id and ('" + dd.ToString() + "' between fromdate and todate or "
        //    + "todate<='" + dd.ToString() + "') group by t.room_id order by fromdate asc,t.room_id asc";


        OdbcCommand sqq12 = new OdbcCommand();
        sqq12.Parameters.AddWithValue("tblname", "t_manage_room t,m_sub_building b,m_room r ");
        sqq12.Parameters.AddWithValue("attribute", "distinct t.room_id,todate,fromdate,totime,fromtime,reason,buildingname,roomno ");
        sqq12.Parameters.AddWithValue("conditionv", "t.roomstatus='3' and t.room_id in (select distinct room_id from m_room where roomstatus='3' and rowstatus<>'2') and r.build_id=b.build_id and t.room_id=r.room_id and ('" + dd.ToString() + "' between fromdate and todate or todate<='" + dd.ToString() + "') group by t.room_id order by fromdate asc,t.room_id asc");


       // DataTable dtt351 = new DataTable();
        dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", sqq12);

        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();


            if (i > 45)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(7);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth3 ={ 2, 6, 5, 5, 5, 5, 7 };
                table1.SetWidths(colwidth3);

                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                cell11a.Rowspan = 2;
                table1.AddCell(cell11a);

                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                cell12a.Rowspan = 2;
                table1.AddCell(cell12a);


                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Blocked", font9)));
                cell14a.Colspan = 2;
                cell14a.HorizontalAlignment = 1;
                table1.AddCell(cell14a);

                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Exp release", font9)));
                cell15a.Rowspan = 2;
                cell15a.HorizontalAlignment = 1;
                table1.AddCell(cell15a);

                PdfPCell cell171a = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
                cell171a.Rowspan = 2;
                table1.AddCell(cell171a);
                PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell16a);
                PdfPCell cell17a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell17a);
                PdfPCell cell16pa = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell16pa);
                PdfPCell cell17pa = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell17pa);
                doc.Add(table1);

            }

            PdfPTable table = new PdfPTable(7);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth1 ={ 2, 6, 5, 5, 5, 5, 7 };
            table.SetWidths(colwidth1);

            building = dtt351.Rows[ii]["buildingname"].ToString();

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

            room = dtt351.Rows[ii]["roomno"].ToString();
            string roomid = dtt351.Rows[ii]["room_id"].ToString();

            try
            {
                fromdate = DateTime.Parse(dtt351.Rows[ii]["fromdate"].ToString());
                frmdate = fromdate.ToString("dd MMM ");
                fromtime = dtt351.Rows[ii]["fromtime"].ToString();
                DateTime todate = DateTime.Parse(dtt351.Rows[ii]["todate"].ToString());

                toodate = todate.ToString("dd MMM");

                totime = dtt351.Rows[ii]["totime"].ToString();
            }
            catch
            {
                toodate = " ";


            }
            reson = dtt351.Rows[ii]["reason"].ToString();

            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21);


            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(building + "/  " + room, font8)));
            table.AddCell(cell23);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            table.AddCell(cell24);

            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(fromtime, font8)));
            table.AddCell(cell25);

            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
            table.AddCell(cell26);

            PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(totime, font8)));
            table.AddCell(cell27);

            PdfPCell cell271 = new PdfPCell(new Phrase(new Chunk(reson, font8)));
            table.AddCell(cell271);
            i++;
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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Blocked Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        con.Close();
    }
    # endregion

    # region Multiple day alloted room list
    protected void lnkMultiple_Click(object sender, EventArgs e)
    {
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.ConnectionString = strConnection;
        //    con.Open();
        //}

        int no = 0;

        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num, buildN;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        string dd = ds2.ToString("yyyy-MM-dd");
        string transtim = ds2.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "MultipleDaysAllocatedRoom" + transtim.ToString() + ".pdf";
        DataTable dtt = new DataTable();
        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");


        string dd5 = objcls.yearmonthdate(txtDateReport.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string d44 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();


        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Multiple Days";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table2 = new PdfPTable(8);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth2 ={ 2, 3, 4, 4, 4, 4, 7, 5 };
        table2.SetWidths(colwidth2);
        int Sid;


        //OdbcCommand Malayalam = new OdbcCommand("select seasonname,season_id from m_season s,m_sub_season d where curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'", con);
       
        
       // string sqq14 = "select seasonname,season_id from m_season s,m_sub_season d where curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'";

        OdbcCommand sqq14 = new OdbcCommand();
        sqq14.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
        sqq14.Parameters.AddWithValue("attribute", "seasonname,season_id ");
        sqq14.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");


        DataTable Malr = new DataTable();
        if (Malr.Rows.Count > 0)
        {
            Mal = Convert.ToInt32(Malr.Rows[0][1].ToString());
            Sname = Malr.Rows[0][0].ToString();
        }


        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("MULTIPLE DAYS ALLOCATED ROOM LIST   on " + d44.ToString() + "", font10)));
        cell.Colspan = 8;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Date :  " + datte, font9)));
        cell11a.Colspan = 4;
        cell11a.Border = 0;
        table2.AddCell(cell11a);
        PdfPCell cell11b = new PdfPCell(new Phrase(new Chunk("Season:  " + Sname, font9)));
        cell11b.Colspan = 4;
        cell11b.Border = 0;
        table2.AddCell(cell11b);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        cell11.Rowspan = 2;
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table2.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("check in Time", font9)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table2.AddCell(cell14);

        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Alloc Type", font9)));
        cell16.Rowspan = 2;
        table2.AddCell(cell16);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Reciept No", font9)));
        cell15.Rowspan = 2;
        table2.AddCell(cell15);

        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell21);
        doc.Add(table2);
        int i = 0;



        //OdbcCommand Multiple = new OdbcCommand("select a.room_id,buildingname,roomno,allocdate,exp_vecatedate,alloc_id,adv_recieptno,alloc_type from "
        //    + "t_roomallocation a,m_sub_building b,m_room r where a.room_id=r.room_id and b.build_id=r.build_id and a.roomstatus='2' and  numberofunit >1 and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate "
        //    + "group by a.room_id  order by allocdate asc", con);
        //OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Multiple);

        //string sqq141 = "select a.room_id,buildingname,roomno,allocdate,exp_vecatedate,alloc_id,adv_recieptno,alloc_type from "
        //    + "t_roomallocation a,m_sub_building b,m_room r where a.room_id=r.room_id and b.build_id=r.build_id and a.roomstatus='2' and  numberofunit >1 and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate "
        //    + "group by a.room_id  order by allocdate asc";

        OdbcCommand sqq141 = new OdbcCommand();
        sqq141.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
        sqq141.Parameters.AddWithValue("attribute", "a.room_id,buildingname,roomno,allocdate,exp_vecatedate,alloc_id,adv_recieptno,alloc_type");
        sqq141.Parameters.AddWithValue("conditionv", "a.room_id=r.room_id and b.build_id=r.build_id and a.roomstatus='2' and  numberofunit >1 and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate group by a.room_id  order by allocdate asc");

        DataTable dtt351 = new DataTable();
        dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", sqq141);

        for (int ii = 0; ii < dtt.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();

            if (i > 45)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(8);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;

                float[] colwidth3 ={ 2, 3, 4, 4, 4, 4, 7, 5 };
                table1.SetWidths(colwidth3);

                PdfPCell cell11g = new PdfPCell(new Phrase(new Chunk("No", font9)));
                cell11g.Rowspan = 2;
                table1.AddCell(cell11g);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                cell12a.Rowspan = 2;
                table1.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("check in Time", font9)));
                cell13a.Colspan = 2;
                cell13a.HorizontalAlignment = 1;
                table1.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
                cell14a.HorizontalAlignment = 1;
                cell14a.Colspan = 2;
                table1.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Alloc Type", font9)));
                cell15a.Rowspan = 2;
                table1.AddCell(cell15a);

                PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
                cell16a.Colspan = 2;
                table1.AddCell(cell16a);


                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell19a);
                PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell20a);
                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell21a);
                i++;
                doc.Add(table1);

            }

            PdfPTable table = new PdfPTable(8);
            table.TotalWidth = 550f;
            table.LockedWidth = true;

            float[] colwidth1 ={ 2, 3, 4, 4, 4, 4, 7, 5 };
            table.SetWidths(colwidth1);


            room = dtt.Rows[ii]["roomno"].ToString();
            building = dtt.Rows[ii]["buildingname"].ToString();
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

            fromdate = DateTime.Parse(dtt.Rows[ii]["allocdate"].ToString());
            frmdate = fromdate.ToString("dd MMM");
            f = fromdate.ToString("dd");
            string ChTime = fromdate.ToString("hh:mm tt");

            todate = DateTime.Parse(dtt.Rows[ii]["exp_vecatedate"].ToString());
            toodate = todate.ToString("dd MMM");
            string PrTime = todate.ToString("hh:mm tt");

            int receipt = Convert.ToInt32(dtt.Rows[ii]["adv_recieptno"].ToString());
            string AllType = dtt.Rows[ii]["alloc_type"].ToString();


            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21b);


            PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building + "/ " + room, font8)));
            table.AddCell(cell22b);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            table.AddCell(cell23);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
            table.AddCell(cell23a);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
            table.AddCell(cell24);
            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
            table.AddCell(cell25);
            PdfPCell cell26a = new PdfPCell(new Phrase(new Chunk(AllType, font8)));
            table.AddCell(cell26a);
            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(receipt.ToString() + "/ " + f, font8)));
            table.AddCell(cell26);
            i++;
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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Multiple Days Allotted Room";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        con.Close();

    }
    # endregion

    # region Room Allotted for double rent
    protected void lnkDoubleRent_Click(object sender, EventArgs e)
    {
        

        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num, buildN;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        string dd = ds2.ToString("yyyy-MM-dd");
        string transtim = ds2.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Room allotted for Double rent" + transtim.ToString() + ".pdf";
        DataTable dtt = new DataTable();
        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");


        string dd5 = objcls.yearmonthdate(txtDateReport.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string d44 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();


        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Multiple Days";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table2 = new PdfPTable(8);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth2 ={ 2, 3, 4, 4, 4, 4, 7, 5 };
        table2.SetWidths(colwidth2);
        int Sid;


        //OdbcCommand Malayalam = new OdbcCommand("select seasonname,season_id from m_season s,m_sub_season d where curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'", con);
       
        
        //string sqq32 = "select seasonname,season_id from m_season s,m_sub_season d where curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'";

        OdbcCommand sqq32 = new OdbcCommand();
        sqq32.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
        sqq32.Parameters.AddWithValue("attribute", "seasonname,season_id");
        sqq32.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");

        DataTable Malr = new DataTable();
        Malr = objcls.SpDtTbl("call selectcond(?,?,?)", sqq32);
        if (Malr.Rows.Count > 0)
        {
            Mal = Convert.ToInt32(Malr.Rows[0][1].ToString());
            Sname = Malr.Rows[0][0].ToString();
        }


        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("ROOM ALLOTTED FOR DOUBLE RENT", font10)));
        cell.Colspan = 8;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Date :  " + datte, font9)));
        cell11a.Colspan = 4;
        cell11a.Border = 0;
        table2.AddCell(cell11a);
        PdfPCell cell11b = new PdfPCell(new Phrase(new Chunk("Season:  " + Sname, font9)));
        cell11b.Colspan = 4;
        cell11b.Border = 0;
        table2.AddCell(cell11b);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        cell11.Rowspan = 2;
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table2.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("check in Time", font9)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table2.AddCell(cell14);

        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Alloc Type", font9)));
        cell16.Rowspan = 2;
        table2.AddCell(cell16);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Reciept No", font9)));
        cell15.Rowspan = 2;
        table2.AddCell(cell15);

        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell21);
        doc.Add(table2);
        int i = 0;



        //OdbcCommand Multiple = new OdbcCommand("select a.room_id,buildingname,roomno,allocdate,exp_vecatedate,alloc_id,adv_recieptno,alloc_type from "
        //    + "t_roomallocation a,m_sub_building b,m_room r where a.room_id=r.room_id and b.build_id=r.build_id and a.roomstatus='2' and  numberofunit =2 "
        //    + " and timediff(allocdate,exp_vecatedate)<='34' and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate "
        //    + "group by a.room_id  order by allocdate asc", con);

        //string sq32 = "select a.room_id,buildingname,roomno,allocdate,exp_vecatedate,alloc_id,adv_recieptno,alloc_type from "
        //    + "t_roomallocation a,m_sub_building b,m_room r where a.room_id=r.room_id and b.build_id=r.build_id and a.roomstatus='2' and  numberofunit =2 "
        //    + " and timediff(allocdate,exp_vecatedate)<='34' and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate "
        //    + "group by a.room_id  order by allocdate asc";

        OdbcCommand sq32 = new OdbcCommand();
        sq32.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
        sq32.Parameters.AddWithValue("attribute", "a.room_id,buildingname,roomno,allocdate,exp_vecatedate,alloc_id,adv_recieptno,alloc_type");
        sq32.Parameters.AddWithValue("conditionv", "a.room_id=r.room_id and b.build_id=r.build_id and a.roomstatus='2' and  numberofunit >1 and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate group by a.room_id  order by allocdate asc");



      //  **********************************************
        //OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Multiple);
        DataTable dtt351 = new DataTable();
        dtt = objcls.SpDtTbl("call selectcond(?,?,?)", sq32);

        for (int ii = 0; ii < dtt.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();

            if (i > 45)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(8);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;

                float[] colwidth3 ={ 2, 3, 4, 4, 4, 4, 7, 5 };
                table1.SetWidths(colwidth3);

                PdfPCell cell11g = new PdfPCell(new Phrase(new Chunk("No", font9)));
                cell11g.Rowspan = 2;
                table1.AddCell(cell11g);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                cell12a.Rowspan = 2;
                table1.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("check in Time", font9)));
                cell13a.Colspan = 2;
                cell13a.HorizontalAlignment = 1;
                table1.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
                cell14a.HorizontalAlignment = 1;
                cell14a.Colspan = 2;
                table1.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Alloc Type", font9)));
                cell15a.Rowspan = 2;
                table1.AddCell(cell15a);

                PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
                cell16a.Colspan = 2;
                table1.AddCell(cell16a);


                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell19a);
                PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell20a);
                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell21a);
                i++;
                doc.Add(table1);

            }

            PdfPTable table = new PdfPTable(8);
            table.TotalWidth = 550f;
            table.LockedWidth = true;

            float[] colwidth1 ={ 2, 3, 4, 4, 4, 4, 7, 5 };
            table.SetWidths(colwidth1);


            room = dtt.Rows[ii]["roomno"].ToString();
            building = dtt.Rows[ii]["buildingname"].ToString();
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

            fromdate = DateTime.Parse(dtt.Rows[ii]["allocdate"].ToString());
            frmdate = fromdate.ToString("dd MMM");
            f = fromdate.ToString("dd");
            string ChTime = fromdate.ToString("hh:mm tt");

            todate = DateTime.Parse(dtt.Rows[ii]["exp_vecatedate"].ToString());
            toodate = todate.ToString("dd MMM");
            string PrTime = todate.ToString("hh:mm tt");

            int receipt = Convert.ToInt32(dtt.Rows[ii]["adv_recieptno"].ToString());
            string AllType = dtt.Rows[ii]["alloc_type"].ToString();


            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21b);


            PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building + "/ " + room, font8)));
            table.AddCell(cell22b);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            table.AddCell(cell23);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
            table.AddCell(cell23a);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
            table.AddCell(cell24);
            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
            table.AddCell(cell25);
            PdfPCell cell26a = new PdfPCell(new Phrase(new Chunk(AllType, font8)));
            table.AddCell(cell26a);
            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(receipt.ToString() + "/ " + f, font8)));
            table.AddCell(cell26);
            i++;
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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Room Allotted for Double Rent";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        con.Close();
    }
   # endregion

    # region Button Roommanagement click
    protected void btnRoomManagement_Click(object sender, EventArgs e)
    {
        btnRoomManagement.BackColor = System.Drawing.Color.Bisque;
        btnRoomManagement.BackColor = System.Drawing.Color.Bisque;
        pnlAccomodation.Visible = false;
        pnlReservation.Visible = false;
        pnlRoomStatus.Visible = true;
        pnlcollectioncomp.Visible = false;
        pnlRoomstatusReport.Visible = false;

        pnlDonorpass.Visible = false;

        pnlNonvacating.Visible = false;

        pnlCollection.Visible = false;
        

     btnReservationChart.BackColor = System.Drawing.Color.Plum;
     btnAccomodationLedger.BackColor = System.Drawing.Color.Plum;
     btnRoomStatus.BackColor=System.Drawing.Color.Plum;
     btnNonvacating.BackColor=System.Drawing.Color.Plum;
     btnDonorPass.BackColor=System.Drawing.Color.Plum;
     btncollectioncomparison.BackColor=System.Drawing.Color.Plum;
     btnCollection.BackColor=System.Drawing.Color.Plum;
     btnOtherReports.BackColor = System.Drawing.Color.Plum;


    }
    # endregion

    # region Button Room status
    protected void btnRoomStatus_Click(object sender, EventArgs e)
    {
        btnRoomStatus.BackColor = System.Drawing.Color.Bisque;
        pnlAccomodation.Visible = false;
        pnlReservation.Visible = false;
        pnlRoomStatus.Visible = false;
        pnlcollectioncomp.Visible = false;
        pnlRoomstatusReport.Visible =true;

        pnlDonorpass.Visible = false;

        pnlNonvacating.Visible = false;

        pnlCollection.Visible = false;
        
        //string strSql4b = " SELECT build_id, buildingname FROM  m_sub_building where rowstatus!='2'";

        OdbcCommand strSql4b = new OdbcCommand();
        strSql4b.Parameters.AddWithValue("tblname", "m_sub_building");
        strSql4b.Parameters.AddWithValue("attribute", "build_id, buildingname");
        strSql4b.Parameters.AddWithValue("conditionv", "rowstatus!='2'");

      //  OdbcDataAdapter dab = new OdbcDataAdapter(strSql4b, conn);


        DataTable dtt1b = new DataTable();
        //dab.Fill(dtt1b);
        dtt1b = objcls.SpDtTbl("call selectcond(?,?,?)", strSql4b);
        DataRow rowb = dtt1b.NewRow();
        rowb["build_id"] = "-1";
        rowb["buildingname"] = "All cottages";
        dtt1b.Rows.InsertAt(rowb, 0);
        //da.Fill(dtt1);

        cmbCompleteBuilding.DataSource = dtt1b;
        cmbCompleteBuilding.DataBind();

        btnReservationChart.BackColor = System.Drawing.Color.Plum;
        btnAccomodationLedger.BackColor = System.Drawing.Color.Plum;
        btnRoomManagement.BackColor = System.Drawing.Color.Plum;
        btnNonvacating.BackColor = System.Drawing.Color.Plum;
        btnDonorPass.BackColor = System.Drawing.Color.Plum;
        btncollectioncomparison.BackColor = System.Drawing.Color.Plum;
        btnCollection.BackColor = System.Drawing.Color.Plum;
        btnOtherReports.BackColor = System.Drawing.Color.Plum;



    }

    # endregion 

    # region Room Status Report
    protected void lnkCompleteRoomStatusReport_Click(object sender, EventArgs e)
    {
        try
        {
            Document doc = new Document(iTextSharp.text.PageSize.A4.Rotate(), 10, 10, 60, 60);
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string currentdate = gh.ToString("dd-MMM-yyyy");
            string datecur = gh.ToString("hh-mm tt");
            string datecur1 = gh.ToString("dd MMM");
            string ch = "RoomStatusReport" + transtim.ToString() + ".pdf";
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

            //Document doc = new Document(iTextSharp.text.PageSize.A4.Rotate(), 10, 10, 70, 60);
            //string pdfFilePath = Server.MapPath(".") + "/pdf/dueformaxtime.pdf";
            //Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9);
            Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 10, 1);

            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            //PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            doc.Open();
            Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
            if ((cmbCompleteBuilding.SelectedValue.ToString() != "-1"))
            {
               
                string dat;

                string ss;
                //conn.Open();
                string date5 = DateTime.Now.ToString("yyyy-MM-dd");
                string date6 = DateTime.Now.ToString("dd  MMM");
                string c = "5 PM";
                DateTime datedd = DateTime.Parse(c);
                string date10 = datedd.ToString("HH:mm");

                string checkdate = date5 + " " + date10;



                string data = Session["dayend"].ToString();
                OdbcCommand cmd31 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                //cmd31.CommandType = CommandType.StoredProcedure;
                cmd31.Parameters.AddWithValue("tblname", " m_room mr ,m_sub_building msb");
                cmd31.Parameters.AddWithValue("attribute", "buildingname ,roomno,roomstatus,room_id,mr.build_id ");
                cmd31.Parameters.AddWithValue("conditionv", " msb.build_id=mr.build_id and mr.build_id=" + Convert.ToInt32(cmbCompleteBuilding.SelectedValue) + " and mr.rowstatus!=2 order by roomno asc  ");

                //OdbcDataAdapter dadapt = new OdbcDataAdapter(cmd31);
                DataTable dtt = new DataTable();
                //dadapt.Fill(dtt);
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

                PdfPTable table = new PdfPTable(6);
                float[] colWidths23 = { 20, 20, 30, 40, 30, 60 };
                table.SetWidths(colWidths23);

                PdfPCell cell = new PdfPCell(new Phrase("Complete Room Status Report ", font12));
                cell.Colspan = 6;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                PdfPCell cellv = new PdfPCell(new Phrase("Building Name: ", font9));
                cellv.Colspan = 2;
                cellv.Border = 0;
                cellv.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv);


                PdfPCell cellv1 = new PdfPCell(new Phrase(cmbCompleteBuilding.SelectedItem.ToString(), font9));
                cellv1.Colspan = 2;
                cellv1.Border = 0;
                cellv1.HorizontalAlignment = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv1);

                PdfPCell cellv2 = new PdfPCell(new Phrase("Date :", font9));
                cellv2.Colspan = 0;
                cellv2.Border = 0;
                cellv2.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv2);

                PdfPCell cellv21 = new PdfPCell(new Phrase(currentdate, font9));
                cellv21.Colspan = 0;
                cellv21.HorizontalAlignment = 0;
                cellv21.Border = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv21);


                doc.Add(table);
                int i = 0;


                int slno = 0;

                PdfPTable table3 = new PdfPTable(10);



                foreach (DataRow dr in dtt.Rows)
                {
                    i++;
                    if (i == 10)
                    {
                        i = 1;
                    }


                    if (Convert.ToInt32(dr["roomstatus"]) == 1)
                    {




                       // OdbcCommand das2 = new OdbcCommand("select *  from t_roomreservation tr ,m_room  mr where status_reserve='0' and  ( now() between reservedate and expvacdate)  and mr.room_id=tr.room_id and mr.roomstatus='1'  and mr.build_id=" + Convert.ToInt32(dr["build_id"]) + " and mr.roomno=" + Convert.ToInt32(dr["roomno"]) + "", conn);
                       // string ssa1 = "select mr.room_id  from t_roomreservation tr ,m_room  mr where status_reserve='0' and  ( now() between reservedate and expvacdate)  and mr.room_id=tr.room_id and mr.roomstatus='1'  and mr.build_id=" + Convert.ToInt32(dr["build_id"]) + " and mr.roomno=" + Convert.ToInt32(dr["roomno"]) + "";

                        OdbcCommand ssa1 = new OdbcCommand();
                        ssa1.Parameters.AddWithValue("tblname", "t_roomreservation tr ,m_room  mr");
                        ssa1.Parameters.AddWithValue("attribute", "mr.room_id");
                        ssa1.Parameters.AddWithValue("conditionv", "status_reserve='0' and  ( now() between reservedate and expvacdate)  and mr.room_id=tr.room_id and mr.roomstatus='1'  and mr.build_id=" + Convert.ToInt32(dr["build_id"]) + " and mr.roomno=" + Convert.ToInt32(dr["roomno"]) + "");

                        OdbcDataReader or2 = objcls.SpGetReader("call selectcond(?,?,?)", ssa1);
                        if (or2.Read())
                        {

                            PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "RES", font7)));
                            cell92.MinimumHeight = 25;
                            table3.AddCell(cell92);
                        }
                        else
                        {

                            PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "VAC", font7)));
                            cell92.MinimumHeight = 25;
                            //cell92.BackgroundColor =iTextSharp.text.BaseColor.GREEN; 
                            table3.AddCell(cell92);
                        }
                    }
                    else if (Convert.ToInt32(dr["roomstatus"]) == 3)
                    {
                        PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "BLK", font7)));
                        cell92.MinimumHeight = 25;

                        //cell92.BackgroundColor =iTextSharp.text.BaseColor.GREEN; 
                        table3.AddCell(cell92);

                    }
                    else if (Convert.ToInt32(dr["roomstatus"]) == 4)
                    {
                       // string ssa2 = "select room_id  from t_roomallocation where " + Convert.ToInt32(dr["room_id"]) + " in (select room_id from t_roomallocation where exp_vecatedate<now() and roomstatus='2')";
                      //  OdbcCommand cmd = new OdbcCommand("select *  from t_roomallocation where " + Convert.ToInt32(dr["room_id"]) + " in (select room_id from t_roomallocation where exp_vecatedate<now() and roomstatus='2')", conn);

                        OdbcCommand ssa2 = new OdbcCommand();
                        ssa2.Parameters.AddWithValue("tblname", "t_roomallocation");
                        ssa2.Parameters.AddWithValue("attribute", "room_id");
                        ssa2.Parameters.AddWithValue("conditionv", "" + Convert.ToInt32(dr["room_id"]) + " in (select room_id from t_roomallocation where exp_vecatedate<now() and roomstatus='2')");

                        OdbcDataReader or = objcls.SpGetReader("call selectcond(?,?,?)", ssa2);
                        if (or.Read())
                        {

                            PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "OS", font7)));
                            cell92.MinimumHeight = 25;
                            table3.AddCell(cell92);

                        }
                        else
                        {

                            PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "OCC", font7)));
                            cell92.MinimumHeight = 25;
                            table3.AddCell(cell92);

                        }

                    }


                }

                if (i < 20)
                {
                    for (int j = 1; j <= 20 - i; j++)
                    {

                        PdfPCell cell921 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        cell921.Border = 0;
                        table3.AddCell(cell921);

                    }

                }
                doc.Add(table3);
                PdfPTable table31 = new PdfPTable(6);
                PdfPCell a = new PdfPCell(new Phrase(new Chunk("NB:-   VAC: Vacant        BLK: Blocked ,        RES: Reserved ,        OCC: Occupied,        OS: Overstayed", font8)));
                a.Border = 0;
                a.MinimumHeight = 10;
                a.Colspan = 6;
                table31.AddCell(a);
                doc.Add(table31);



            }

            else
            {

                int build3 = 0, yy = 0;

                //if (conn.State == ConnectionState.Closed)
                //{
                //    conn.ConnectionString = strConnection;
                //    conn.Open();

                //}

                string dat;

                string ss;
                //conn.Open();
                string date5 = DateTime.Now.ToString("yyyy-MM-dd");
                string date6 = DateTime.Now.ToString("dd  MMM");
                string c = "5 PM";
                DateTime datedd = DateTime.Parse(c);
                string date10 = datedd.ToString("HH:mm");

                string checkdate = date5 + " " + date10;


                //try
                //{ 


                string data = Session["dayend"].ToString();
                OdbcCommand cmd31 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
               // cmd31.CommandType = CommandType.StoredProcedure;
                cmd31.Parameters.AddWithValue("tblname", " m_room mr ,m_sub_building msb");
                cmd31.Parameters.AddWithValue("attribute", "buildingname ,roomno,roomstatus,room_id,mr.build_id ");
                cmd31.Parameters.AddWithValue("conditionv", " msb.build_id=mr.build_id  and mr.rowstatus!=2 and buildingname  NOT LIKE '%DH%'  and buildingname NOT LIKE '%PC%'  and buildingname NOT LIKE '%MSC%'  and buildingname NOT LIKE '%MOC%' order by build_id,roomno asc  ");

               // OdbcDataAdapter dadapt = new OdbcDataAdapter(cmd31);
                DataTable dtt = new DataTable();
              //  dadapt.Fill(dtt);
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

                PdfPTable table = new PdfPTable(6);
                float[] colWidths23 = { 20, 20, 30, 40, 30, 60 };
                table.SetWidths(colWidths23);

                PdfPCell cell = new PdfPCell(new Phrase("Complete Room Status Report of Cottages ", font12));
                cell.Colspan = 6;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                PdfPCell cellv2 = new PdfPCell(new Phrase("Date :" + currentdate, font9));
                cellv2.Colspan = 6;
                cellv2.Border = 0;
                cellv2.HorizontalAlignment = 2;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv2);



                doc.Add(table);
                int i = 0;

                int xx = 0;
                int slno = 0;

                PdfPTable table3 = new PdfPTable(10);

                int buildid2 = 0;

                foreach (DataRow dr in dtt.Rows)
                {

                    PdfPTable table4b = new PdfPTable(10);

                    if (buildid2 != (Convert.ToInt32(dr["build_id"])))
                    {
                        string cc = dr["buildingname"].ToString();

                        if (yy != 0)
                        {

                            if (i < 10)
                            {
                                for (int j = 1; j <= 10 - i; j++)
                                {

                                    PdfPCell cell921 = new PdfPCell(new Phrase(new Chunk("", font8)));
                                    cell921.Border = 0;
                                    table3.AddCell(cell921);

                                }

                            }

                            i = 0;
                            PdfPCell cell92v = new PdfPCell(new Phrase(new Chunk("Building :" + dr["buildingname"].ToString(), font9)));
                            cell92v.MinimumHeight = 25;
                            cell92v.Colspan = 10;
                            cell92v.Border = 0;
                            table3.AddCell(cell92v);
                            buildid2 = Convert.ToInt32(dr["build_id"]);

                        }


                        else
                        {

                            string ccc = dr["buildingname"].ToString();
                            PdfPCell cell92v = new PdfPCell(new Phrase(new Chunk("Building name:" + dr["buildingname"].ToString(), font9)));
                            cell92v.MinimumHeight = 25;
                            cell92v.Colspan = 10;
                            cell92v.Border = 1;
                            table3.AddCell(cell92v);
                            buildid2 = Convert.ToInt32(dr["build_id"]);

                        }
                        //doc.Add(table4b);

                    }
                    yy++;

                    if (i == 10)
                    {
                        i = 1;
                    }

                    if (Convert.ToInt32(dr["roomstatus"]) == 1)
                    {

                      //  string ssa3 = "select mr.room_id  from t_roomreservation tr ,m_room  mr where status_reserve='0' and  ( now() between reservedate and expvacdate)  and mr.room_id=tr.room_id and mr.roomstatus='1'  and mr.build_id=" + Convert.ToInt32(cmbCompleteBuilding.SelectedValue) + " and mr.roomno=" + Convert.ToInt32(dr["roomno"]) + "";
                       // OdbcCommand das2 = new OdbcCommand("select *  from t_roomreservation tr ,m_room  mr where status_reserve='0' and  ( now() between reservedate and expvacdate)  and mr.room_id=tr.room_id and mr.roomstatus='1'  and mr.build_id=" + Convert.ToInt32(cmbCompleteBuilding.SelectedValue) + " and mr.roomno=" + Convert.ToInt32(dr["roomno"]) + "", conn);

                        OdbcCommand ssa3 = new OdbcCommand();
                        ssa3.Parameters.AddWithValue("tblname", "t_roomreservation tr ,m_room  mr");
                        ssa3.Parameters.AddWithValue("attribute", "mr.room_id");
                        ssa3.Parameters.AddWithValue("conditionv", " status_reserve='0' and  ( now() between reservedate and expvacdate)  and mr.room_id=tr.room_id and mr.roomstatus='1'  and mr.build_id=" + Convert.ToInt32(cmbCompleteBuilding.SelectedValue) + " and mr.roomno=" + Convert.ToInt32(dr["roomno"]) + "");



                        OdbcDataReader or2 = objcls.SpGetReader("call selectcond(?,?,?)", ssa3);
                        if (or2.Read())
                        {

                            PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "RES", font7)));
                            cell92.MinimumHeight = 25;
                            table3.AddCell(cell92);
                        }
                        else
                        {

                            PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "VAC", font7)));
                            cell92.MinimumHeight = 25;
                            table3.AddCell(cell92);
                        }
                        i++;

                    }
                    else if (Convert.ToInt32(dr["roomstatus"]) == 3)
                    {
                        PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "BLK", font7)));
                        cell92.MinimumHeight = 25;

                        //cell92.BackgroundColor =iTextSharp.text.BaseColor.GREEN; 
                        table3.AddCell(cell92);
                        i++;
                    }
                    else if (Convert.ToInt32(dr["roomstatus"]) == 4)
                    {
                       // string ssa4 = "select room_id  from t_roomallocation where " + Convert.ToInt32(dr["room_id"]) + " in (select room_id from t_roomallocation where exp_vecatedate<now() and roomstatus='2')";
                        //OdbcCommand cmd = new OdbcCommand("select *  from t_roomallocation where " + Convert.ToInt32(dr["room_id"]) + " in (select room_id from t_roomallocation where exp_vecatedate<now() and roomstatus='2')", conn);

                        OdbcCommand ssa4 = new OdbcCommand();
                        ssa4.Parameters.AddWithValue("tblname", "t_roomallocation");
                        ssa4.Parameters.AddWithValue("attribute", "room_id");
                        ssa4.Parameters.AddWithValue("conditionv", " " + Convert.ToInt32(dr["room_id"]) + " in (select room_id from t_roomallocation where exp_vecatedate<now() and roomstatus='2')");


                        OdbcDataReader or = objcls.SpGetReader("call selectcond(?,?,?)", ssa4);

                        if (or.Read())
                        {

                            PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "OS", font7)));
                            cell92.MinimumHeight = 25;
                            table3.AddCell(cell92);

                        }
                        else
                        {

                            PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "OCC", font7)));
                            cell92.MinimumHeight = 25;
                            table3.AddCell(cell92);

                        }

                        i++;

                    }


                }

                if (i < 10)
                {
                    for (int j = 1; j <= 10 - i; j++)
                    {

                        PdfPCell cell921 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        cell921.Border = 0;
                        table3.AddCell(cell921);

                    }

                }
                doc.Add(table3);
                PdfPTable table31 = new PdfPTable(6);

                PdfPCell a = new PdfPCell(new Phrase(new Chunk("NB:-   VAC: Vacant        BLK: Blocked ,        RES: Reserved ,        OCC: Occupied,        OS: Overstayed", font8)));
                a.Border = 0;
                a.MinimumHeight = 10;
                a.Colspan = 6;
                table31.AddCell(a);


                doc.Add(table31);


            }

            PdfPTable table4 = new PdfPTable(7);
            PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
            cellf.HorizontalAlignment = Element.ALIGN_LEFT;
            cellf.PaddingLeft = 20;
            cellf.MinimumHeight = 30;
            cellf.Colspan = 7;
            cellf.Border = 0;
            table4.AddCell(cellf);

            PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
            cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
            cellf1.PaddingLeft = 20;
            cellf1.Border = 0;
            cellf1.Colspan = 7;
            table4.AddCell(cellf1);

            PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom", font8)));
            cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
            cellh2.PaddingLeft = 20;
            cellh2.Border = 0;
            cellh2.Colspan = 7;
            table4.AddCell(cellh2);


            doc.Add(table4);


            doc.Close();
            //System.Diagnostics.Process.Start(pdfFilePath);
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
            lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Problem found during report taking";
            ViewState["action"] = "warn27";
            ModalPopupExtender1.Show();




        }
       // conn.Close();

    }

    # endregion

    # region Button Nonvacating click
    protected void btnNonvacating_Click(object sender, EventArgs e)
    {
        btnNonvacating.BackColor = System.Drawing.Color.Bisque;
        pnlAccomodation.Visible = false;
        pnlReservation.Visible = false;
        pnlRoomStatus.Visible = false;
        pnlcollectioncomp.Visible = false;
        pnlRoomstatusReport.Visible = false;

        pnlDonorpass.Visible = false;

        pnlNonvacating.Visible = true;

        pnlCollection.Visible = false;



        //string strSql4 = " select  distinct msb.build_id,buildingname from t_roomallocation tr,m_sub_building msb ,m_room mr where tr.room_id=mr.room_id and mr.build_id=msb.build_id and tr.roomstatus=2";


        OdbcCommand strSql4 = new OdbcCommand();
        strSql4.Parameters.AddWithValue("tblname", "t_roomallocation tr,m_sub_building msb ,m_room mr");
        strSql4.Parameters.AddWithValue("attribute", "distinct msb.build_id,buildingname");
        strSql4.Parameters.AddWithValue("conditionv", " tr.room_id=mr.room_id and mr.build_id=msb.build_id and tr.roomstatus=2");

        DataTable dtt1 = new DataTable();
        dtt1 = objcls.SpDtTbl("call selectcond(?,?,?)", strSql4);
        //DataColumn colID = dtt1.Columns.Add("build_id", System.Type.GetType("System.Int32"));
        //DataColumn colNo = dtt1.Columns.Add("buildingname", System.Type.GetType("System.String"));
        DataRow row = dtt1.NewRow();
        row["build_id"] = "-1";
        row["buildingname"] = "--Select--";
        dtt1.Rows.InsertAt(row, 0);
       // da.Fill(dtt1);
       
        cmbSelectBuilding.DataSource = dtt1;
        cmbSelectBuilding.DataBind();
        btnReservationChart.BackColor = System.Drawing.Color.Plum;
        btnAccomodationLedger.BackColor = System.Drawing.Color.Plum;
        btnRoomStatus.BackColor = System.Drawing.Color.Plum;
        btnRoomManagement.BackColor = System.Drawing.Color.Plum;
        btnDonorPass.BackColor = System.Drawing.Color.Plum;
        btncollectioncomparison.BackColor = System.Drawing.Color.Plum;
        btnCollection.BackColor = System.Drawing.Color.Plum;
        btnOtherReports.BackColor = System.Drawing.Color.Plum;

    }

    # endregion 

    # region Due Vacating Room Details
    protected void lnkDueVacatingReports_Click(object sender, EventArgs e)
    {
        if ((txtTime1.Text != "") && (Convert.ToInt32(cmbSelectBuilding.SelectedValue) != -1))
        {
            
            string dat;

            string ss;
            //conn.Open();
            string date5 = DateTime.Now.ToString("yyyy-MM-dd");
            string date6 = DateTime.Now.ToString("dd  MMM");

            DateTime datedd = DateTime.Parse(txtTime1.Text);
            string date10 = datedd.ToString("HH:mm");

            string checkdate = date5 + " " + date10;

            string cv1 = "DROP view if exists tempnonvacatexc";
            int cv11 = objcls.exeNonQuery(cv1);

            //OdbcCommand cc = new OdbcCommand("DROP view if exists tempnonvacatexc", conn);
           // cc.ExecuteNonQuery();


            //OdbcCommand cmdview = new OdbcCommand("create view  tempnonvacatexc as  (SELECT  * from t_roomallocation ta   WHERE ta.roomstatus='2'  and date(exp_vecatedate)=curdate() and ADDTIME(exp_vecatedate,MAKETIME((SELECT graceperiod from t_policy_allocation WHERE reqtype='Common' and rowstatus<>'2' and ((curdate()>=fromdate and curdate()<=todate) or (curdate()>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<'" + checkdate + "')", conn);
            string cv21 = "create view  tempnonvacatexc as  (SELECT  * from t_roomallocation ta   WHERE ta.roomstatus='2'  and date(exp_vecatedate)=curdate() and ADDTIME(exp_vecatedate,MAKETIME((SELECT graceperiod from t_policy_allocation WHERE reqtype='Common' and rowstatus<>'2' and ((curdate()>=fromdate and curdate()<=todate) or (curdate()>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<'" + checkdate + "')";
            int cv22 = objcls.exeNonQuery(cv21);
            //cmdview.ExecuteNonQuery();

            try
            {

                //string xx = "select adv_recieptno,place, buildingname ,roomno ,swaminame , DATE_FORMAT(exp_vecatedate, '%d-%m-%y  %l:%i %p') as vacatedate from tempnonvacate tt,m_room mr ,m_sub_building msb where tt.room_id=mr.room_id and msb.build_id=mr.build_id";


                string data = Session["dayend"].ToString();
                OdbcCommand cmd31 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
               // cmd31.CommandType = CommandType.StoredProcedure;
                cmd31.Parameters.AddWithValue("tblname", " tempnonvacatexc tt,m_room mr ,m_sub_building msb");
                cmd31.Parameters.AddWithValue("attribute", "adv_recieptno,place, buildingname ,roomno ,swaminame , exp_vecatedate as vacatedate");
                cmd31.Parameters.AddWithValue("conditionv", "tt.room_id=mr.room_id and msb.build_id=mr.build_id and mr.build_id=" + Convert.ToInt32(cmbSelectBuilding.SelectedValue) + "");

                //OdbcDataAdapter dadapt = new OdbcDataAdapter(cmd31);
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
               // dadapt.Fill(dtt);

                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);

                DateTime gh = DateTime.Now;
                string transtim = gh.ToString("dd-MM-yyyy HH-mm");
                string ch = "DueVacatingRooms" + transtim.ToString() + ".pdf";
                string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
                //string ch = "transactionprinteron" + transtim.ToString() + ".pdf";

                //string pdfFilePath = Server.MapPath(".") + "/pdf/nonvacroom.pdf";
                Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
                Font font7 = FontFactory.GetFont("ARIAL", 9);
                Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
                Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
                pdfPage page = new pdfPage();
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;
                //PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                doc.Open();
                PdfPTable table = new PdfPTable(5);
                float[] colWidths23 = { 20, 20, 40, 30, 60 };
                table.SetWidths(colWidths23);
                page.strRptMode = "Duevacate";
                PdfPCell cell = new PdfPCell(new Phrase("Room Due for Vacating ", font12));
                cell.Colspan = 5;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                PdfPCell cellv = new PdfPCell(new Phrase("Building Name: ", font9));
                cellv.Colspan = 2;
                cellv.Border = 0;
                cellv.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv);


                PdfPCell cellv1 = new PdfPCell(new Phrase(cmbSelectBuilding.SelectedItem.ToString(), font9));
                cellv1.Colspan = 1;
                cellv1.Border = 0;
                cellv1.HorizontalAlignment = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv1);

                PdfPCell cellv2 = new PdfPCell(new Phrase("Due Time:", font9));
                cellv2.Colspan = 0;
                cellv2.Border = 0;
                cellv2.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv2);

                PdfPCell cellv21 = new PdfPCell(new Phrase(txtTime.Text + " On " + date6, font9));
                cellv21.Colspan = 0;
                cellv21.HorizontalAlignment = 0;
                cellv21.Border = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv21);




                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                table.AddCell(cell2);
                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Prop Checkout Time", font8)));
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                table.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                table.AddCell(cell5);

                //PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
                //table.AddCell(cell6);

                //PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Expect Vacate Date", font9)));
                //table.AddCell(cell7);
                //PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("vacatingtime", font9)));
                //table.AddCell(cell8);

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
                        PdfPTable table1 = new PdfPTable(5);
                        float[] colWidths231 = { 20, 20, 40, 30, 60 };
                        table1.SetWidths(colWidths231);


                        PdfPCell cell1n = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                        table1.AddCell(cell1n);

                        PdfPCell cell2n = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                        table1.AddCell(cell2n);

                        PdfPCell cell3n = new PdfPCell(new Phrase(new Chunk("Prop Checkout Time", font8)));
                        table1.AddCell(cell3n);

                        PdfPCell cell4n = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                        table1.AddCell(cell4n);

                        PdfPCell cell5n = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                        table1.AddCell(cell5n);

                        //PdfPCell cell6n = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
                        //table1.AddCell(cell6n);

                        //PdfPCell cell7n = new PdfPCell(new Phrase(new Chunk("Expected Vacate Date", font9)));
                        //table1.AddCell(cell7n);
                        //PdfPCell cell8n = new PdfPCell(new Phrase(new Chunk("vacatingtime", font9)));
                        //table1.AddCell(cell8n);

                        doc.Add(table1);




                    }

                    PdfPTable table3 = new PdfPTable(5);
                    #region  formate

                    float[] colWidths23u = { 20, 20, 40, 30, 60 };
                    table3.SetWidths(colWidths23u);
                    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table3.AddCell(cell9);

                    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font8)));
                    table3.AddCell(cell10);
                    DateTime dated = DateTime.Parse(dr["vacatedate"].ToString());
                    string time1 = dated.ToString("hh:mm tt");




                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(time1.ToString(), font8)));
                    table3.AddCell(cell11);

                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("4 PM", font8)));
                    table3.AddCell(cell12);

                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Over Stay", font8)));
                    table3.AddCell(cell13);

                    //PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font8)));
                    //table3.AddCell(cell14);

                    //DateTime dt5 = DateTime.Parse(dr["exvedate"].ToString());
                    //string date1 = dt5.ToString("dd-MM-yyyy");

                    //PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["vacatedate"].ToString(), font8)));
                    //table3.AddCell(cell15);

                    //PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["exvectime"].ToString(), font8)));
                    //table3.AddCell(cell16);
                    i++;
                    #endregion

                    //  float[] colWidths23u = { 20, 30, 40, 50, 60, 30, 40 };
                    //  table3.SetWidths(colWidths23u);

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
                //System.Diagnostics.Process.Start(pdfFilePath);
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
                //MessageBox.Show("Problem found in taking report", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);

                lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "Problem found during report taking";
                ViewState["action"] = "warn27";
                ModalPopupExtender1.Show();

            }

        }
        else
        {
            messagedisplay("Select Building and enter Time to take report", "ww");

        }

    }

    # endregion

    # region Due vacating for Max Stay of current day
    protected void lnkDueVacatingMaxtime_Click(object sender, EventArgs e)
    {
        if ((cmbSelectBuilding.SelectedValue != "-1"))
        {
            
            string dat;

            string ss;
            //conn.Open();
            string date5 = DateTime.Now.ToString("yyyy-MM-dd");
            string date6 = DateTime.Now.ToString("dd  MMM");
            string c = "5 PM";
            DateTime datedd = DateTime.Parse(c);
            string date10 = datedd.ToString("HH:mm");

            string checkdate = date5 + " " + date10;

           // OdbcCommand cc = new OdbcCommand("DROP view if exists tempnonvacate11", conn);
            string sa1 = "DROP view if exists tempnonvacate11";
            int as1 = objcls.exeNonQuery(sa1);
           // cc.ExecuteNonQuery();


            //OdbcCommand cmdview = new OdbcCommand("create view  tempnonvacate11 as  (SELECT  * from t_roomallocation ta   WHERE ta.roomstatus='2'  and date(exp_vecatedate)=curdate() and ADDTIME(exp_vecatedate,MAKETIME((SELECT graceperiod from t_policy_allocation WHERE reqtype='Common' and rowstatus<>'2' and ((curdate() >= fromdate and  curdate()<=todate) or (curdate()>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<'" + checkdate + "')", conn);
            string sa2 = "create view  tempnonvacate11 as  (SELECT  * from t_roomallocation ta   WHERE ta.roomstatus='2'  and date(exp_vecatedate)=curdate() and ADDTIME(exp_vecatedate,MAKETIME((SELECT graceperiod from t_policy_allocation WHERE reqtype='Common' and rowstatus<>'2' and ((curdate() >= fromdate and  curdate()<=todate) or (curdate()>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<'" + checkdate + "')";
         //   cmdview.ExecuteNonQuery();
            int as2 = objcls.exeNonQuery(sa2);

            try
            {

                //string xx = "select adv_recieptno,place, buildingname ,roomno ,swaminame , DATE_FORMAT(exp_vecatedate, '%d-%m-%y  %l:%i %p') as vacatedate from tempnonvacate tt,m_room mr ,m_sub_building msb where tt.room_id=mr.room_id and msb.build_id=mr.build_id";


                string data = Session["dayend"].ToString();
                OdbcCommand cmd31 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                //cmd31.CommandType = CommandType.StoredProcedure;
                cmd31.Parameters.AddWithValue("tblname", " tempnonvacate11 tt,m_room mr ,m_sub_building msb");
                cmd31.Parameters.AddWithValue("attribute", "adv_recieptno,place, buildingname ,roomno ,swaminame , exp_vecatedate as vacatedate");
                cmd31.Parameters.AddWithValue("conditionv", "tt.room_id=mr.room_id and msb.build_id=mr.build_id and mr.build_id=" + Convert.ToInt32(cmbSelectBuilding.SelectedValue) + "");

                //OdbcDataAdapter dadapt = new OdbcDataAdapter(cmd31);
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
                //dadapt.Fill(dtt);


                DateTime gh = DateTime.Now;
                string transtim = gh.ToString("dd-MM-yyyy HH-mm");




                string ch = "DueVacatingmaxtime" + transtim.ToString() + ".pdf";

                string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;







                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);
                //string pdfFilePath = Server.MapPath(".") + "/pdf/dueformaxtime.pdf";
                Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
                Font font7 = FontFactory.GetFont("ARIAL", 9);
                Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
                Font font9 = FontFactory.GetFont("ARIAL", 10, 1);

                pdfPage page = new pdfPage();
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;
                //PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                doc.Open();
                PdfPTable table = new PdfPTable(5);
                float[] colWidths23 = { 20, 20, 40, 30, 60 };
                table.SetWidths(colWidths23);
                page.strRptMode = "Duevacate";
                PdfPCell cell = new PdfPCell(new Phrase("Room Due for Vacating ", font12));
                cell.Colspan = 5;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                PdfPCell cellv = new PdfPCell(new Phrase("Building Name: ", font9));
                cellv.Colspan = 2;
                cellv.Border = 0;
                cellv.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv);


                PdfPCell cellv1 = new PdfPCell(new Phrase(cmbSelectBuilding.SelectedItem.ToString(), font9));
                cellv1.Colspan = 1;
                cellv1.Border = 0;
                cellv1.HorizontalAlignment = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv1);

                PdfPCell cellv2 = new PdfPCell(new Phrase("Due Time:", font9));
                cellv2.Colspan = 0;
                cellv2.Border = 0;
                cellv2.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv2);

                PdfPCell cellv21 = new PdfPCell(new Phrase("5 PM on  " + date6, font9));
                cellv21.Colspan = 0;
                cellv21.HorizontalAlignment = 0;
                cellv21.Border = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv21);




                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                table.AddCell(cell2);
                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Prop Checkout Time", font8)));
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                table.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                table.AddCell(cell5);

            

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
                        PdfPTable table1 = new PdfPTable(5);
                        float[] colWidths231 = { 20, 20, 40, 30, 60 };
                        table1.SetWidths(colWidths231);


                        PdfPCell cell1n = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                        table1.AddCell(cell1n);

                        PdfPCell cell2n = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                        table1.AddCell(cell2n);

                        PdfPCell cell3n = new PdfPCell(new Phrase(new Chunk("Prop Checkout Time", font8)));
                        table1.AddCell(cell3n);

                        PdfPCell cell4n = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                        table1.AddCell(cell4n);

                        PdfPCell cell5n = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                        table1.AddCell(cell5n);

                       

                        doc.Add(table1);




                    }

                    PdfPTable table3 = new PdfPTable(5);
                    #region commented

                    float[] colWidths23u = { 20, 20, 40, 30, 60 };
                    table3.SetWidths(colWidths23u);
                    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                    table3.AddCell(cell9);

                    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font7)));
                    table3.AddCell(cell10);
                    DateTime dated = DateTime.Parse(dr["vacatedate"].ToString());
                    string time1 = dated.ToString("hh:mm tt");




                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(time1.ToString(), font7)));
                    table3.AddCell(cell11);

                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("4 PM", font7)));
                    table3.AddCell(cell12);

                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Over stay", font7)));
                    table3.AddCell(cell13);

                   
                    i++;
                    #endregion

                    //  float[] colWidths23u = { 20, 30, 40, 50, 60, 30, 40 };
                    //  table3.SetWidths(colWidths23u);

                    doc.Add(table3);

                }
                PdfPTable table4 = new PdfPTable(5);
                PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                cellf.HorizontalAlignment = Element.ALIGN_LEFT;
                cellf.PaddingLeft = 100;
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
                //System.Diagnostics.Process.Start(pdfFilePath);
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
                //MessageBox.Show("Problem found in taking report", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);

                lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "Problem found during report taking";
                ViewState["action"] = "warn27";
                ModalPopupExtender1.Show();




            }





        }
        else
        {
            messagedisplay("Select Building to take report", "ww");

        }

    }
    # endregion

    # region All non vacating
    protected void lnkNonvacateWhole_Click(object sender, EventArgs e)
    {
        

        string dat;

        string ss;
        //conn.Open();
        string date5 = DateTime.Now.ToString("yyyy-MM-dd");
        string date6 = DateTime.Now.ToString("dd  MMM");
        string c = "5 PM";
        DateTime datedd = DateTime.Parse(c);
        string date10 = datedd.ToString("HH:mm");

        string checkdate = date5 + " " + date10;

       // OdbcCommand cc = new OdbcCommand("DROP view if exists tempnonvacatewhole", conn);
       // cc.ExecuteNonQuery();
        string sqa1 = "DROP view if exists tempnonvacatewhole";
        int sqa11 = objcls.exeNonQuery(sqa1);



       // OdbcCommand cmdview = new OdbcCommand("create view  tempnonvacatewhole as  (SELECT  * from t_roomallocation ta   WHERE ta.roomstatus='2'   and ADDTIME(exp_vecatedate,MAKETIME((SELECT graceperiod from t_policy_allocation WHERE reqtype='Common' and rowstatus<>'2' and ((curdate() >= fromdate and  curdate()<=todate) or (curdate()>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<now())", conn);
        //cmdview.ExecuteNonQuery();
        string sqa2 = "create view  tempnonvacatewhole as  (SELECT  * from t_roomallocation ta   WHERE ta.roomstatus='2'   and ADDTIME(exp_vecatedate,MAKETIME((SELECT graceperiod from t_policy_allocation WHERE reqtype='Common' and rowstatus<>'2' and ((curdate() >= fromdate and  curdate()<=todate) or (curdate()>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<now())";
        int saq22 = objcls.exeNonQuery(sqa2);
        try
        {
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");

            string datecur = gh.ToString("hh-mm tt");
            string datecur1 = gh.ToString("dd MMM");
            string ch = "DueVacatingmaxtime" + transtim.ToString() + ".pdf";
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);
            //string pdfFilePath = Server.MapPath(".") + "/pdf/dueformaxtime.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9);
            Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 10, 1);

            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            //PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            doc.Open();

            //string xx = "select adv_recieptno,place, buildingname ,roomno ,swaminame , DATE_FORMAT(exp_vecatedate, '%d-%m-%y  %l:%i %p') as vacatedate from tempnonvacate tt,m_room mr ,m_sub_building msb where tt.room_id=mr.room_id and msb.build_id=mr.build_id";
            page.strRptMode = "nonvacate";
            if (cmbSelectBuilding.SelectedValue.ToString() != "-1")
            {


                string data = Session["dayend"].ToString();
                OdbcCommand cmd31 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
               // cmd31.CommandType = CommandType.StoredProcedure;
                cmd31.Parameters.AddWithValue("tblname", " tempnonvacatewhole tt,m_room mr ,m_sub_building msb");
                cmd31.Parameters.AddWithValue("attribute", "adv_recieptno,place, buildingname ,roomno ,swaminame , exp_vecatedate as vacatedate");
                cmd31.Parameters.AddWithValue("conditionv", "tt.room_id=mr.room_id and msb.build_id=mr.build_id and mr.build_id=" + Convert.ToInt32(cmbSelectBuilding.SelectedValue) + " order by exp_vecatedate asc ");

                //OdbcDataAdapter dadapt = new OdbcDataAdapter(cmd31);
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
                //dadapt.Fill(dtt);


                PdfPTable table = new PdfPTable(6);
                float[] colWidths23 = { 20, 20, 30, 40, 30, 60 };
                table.SetWidths(colWidths23);

                PdfPCell cell = new PdfPCell(new Phrase("Nonvacating Rooms ", font12));
                cell.Colspan = 6;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                PdfPCell cellv = new PdfPCell(new Phrase("Building Name: ", font9));
                cellv.Colspan = 2;
                cellv.Border = 0;
                cellv.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv);


                PdfPCell cellv1 = new PdfPCell(new Phrase(cmbSelectBuilding.SelectedItem.ToString(), font9));
                cellv1.Colspan = 2;
                cellv1.Border = 0;
                cellv1.HorizontalAlignment = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv1);

                PdfPCell cellv2 = new PdfPCell(new Phrase("Due Time:", font9));
                cellv2.Colspan = 0;
                cellv2.Border = 0;
                cellv2.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv2);

                PdfPCell cellv21 = new PdfPCell(new Phrase(datecur + " on " + datecur1, font9));
                cellv21.Colspan = 0;
                cellv21.HorizontalAlignment = 0;
                cellv21.Border = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv21);

                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                table.AddCell(cell1);

                PdfPCell cell1b = new PdfPCell(new Phrase(new Chunk("Adv Rec", font8)));
                table.AddCell(cell1b);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                table.AddCell(cell2);
                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Prop Checkout Time", font8)));
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                table.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                table.AddCell(cell5);

                //PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
                //table.AddCell(cell6);

                //PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Expect Vacate Date", font9)));
                //table.AddCell(cell7);
                //PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("vacatingtime", font9)));
                //table.AddCell(cell8);

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
                        float[] colWidths231 = { 20, 20, 30, 40, 30, 60 };
                        table1.SetWidths(colWidths231);


                        PdfPCell cell1n = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                        table1.AddCell(cell1n);

                        PdfPCell cell1n2 = new PdfPCell(new Phrase(new Chunk("Adv Rec", font8)));
                        table1.AddCell(cell1n2);
                        PdfPCell cell2n = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                        table1.AddCell(cell2n);

                        PdfPCell cell3n = new PdfPCell(new Phrase(new Chunk("Prop Checkout Time", font8)));
                        table1.AddCell(cell3n);

                        PdfPCell cell4n = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                        table1.AddCell(cell4n);

                        PdfPCell cell5n = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                        table1.AddCell(cell5n);



                        doc.Add(table1);




                    }

                    PdfPTable table3 = new PdfPTable(6);


                    float[] colWidths23u = { 20, 20, 30, 40, 30, 60 };
                    table3.SetWidths(colWidths23u);
                    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                    table3.AddCell(cell9);


                    PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["adv_recieptno"].ToString(), font7)));
                    table3.AddCell(cell92);

                    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font7)));
                    table3.AddCell(cell10);
                    DateTime dated = DateTime.Parse(dr["vacatedate"].ToString());
                    string time1 = dated.ToString("dd-MM-yyyy  hh:mm tt");

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(time1.ToString(), font7)));
                    table3.AddCell(cell11);

                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("4 PM", font7)));
                    table3.AddCell(cell12);

                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Over stay", font7)));
                    table3.AddCell(cell13);

                    i++;
                    //  float[] colWidths23u = { 20, 30, 40, 50, 60, 30, 40 };
                    //  table3.SetWidths(colWidths23u);

                    doc.Add(table3);

                }
            }

            else
            {
                string data = Session["dayend"].ToString();
                OdbcCommand cmd31 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
               // cmd31.CommandType = CommandType.StoredProcedure;
                cmd31.Parameters.AddWithValue("tblname", " tempnonvacatewhole tt,m_room mr ,m_sub_building msb");
                cmd31.Parameters.AddWithValue("attribute", "adv_recieptno,place, buildingname ,roomno ,swaminame , exp_vecatedate as vacatedate");
                cmd31.Parameters.AddWithValue("conditionv", "tt.room_id=mr.room_id and msb.build_id=mr.build_id order by mr.build_id ,exp_vecatedate asc");

                //OdbcDataAdapter dadapt = new OdbcDataAdapter(cmd31);
                DataTable dtt = new DataTable();
               // dadapt.Fill(dtt);
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

                PdfPTable table = new PdfPTable(7);
                float[] colWidths23 = { 20, 20, 40, 20, 45, 20, 40 };
                table.SetWidths(colWidths23);

                PdfPCell cell = new PdfPCell(new Phrase("Non vacating Rooms ", font12));
                cell.Colspan = 7;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                PdfPCell cellv1 = new PdfPCell(new Phrase("All Building", font9));
                cellv1.Colspan = 3;
                cellv1.Border = 0;
                cellv1.HorizontalAlignment = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv1);

                PdfPCell cellv2 = new PdfPCell(new Phrase("Due Time:", font9));
                cellv2.Colspan = 2;
                cellv2.Border = 0;
                cellv2.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv2);

                PdfPCell cellv21 = new PdfPCell(new Phrase(datecur + " on " + datecur1, font9));
                cellv21.Colspan = 2;
                cellv21.HorizontalAlignment = 0;
                cellv21.Border = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv21);




                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                table.AddCell(cell1);

                PdfPCell celle1 = new PdfPCell(new Phrase(new Chunk("Adv Rec", font8)));
                table.AddCell(celle1);

                PdfPCell cell1c = new PdfPCell(new Phrase(new Chunk("Building", font8)));
                table.AddCell(cell1c);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));

                table.AddCell(cell2);
                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Prop Checkout Time", font8)));
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                table.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                table.AddCell(cell5);


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
                        PdfPTable table1 = new PdfPTable(7);
                        float[] colWidths231 = { 20, 20, 40, 20, 45, 20, 40 };
                        table1.SetWidths(colWidths231);


                        PdfPCell cell1n = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                        table1.AddCell(cell1n);
                        PdfPCell cell1n2 = new PdfPCell(new Phrase(new Chunk("Adv Rec", font8)));
                        table1.AddCell(cell1n2);

                        PdfPCell cell1ns = new PdfPCell(new Phrase(new Chunk("Building", font8)));
                        table1.AddCell(cell1ns);

                        PdfPCell cell2n = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                        table1.AddCell(cell2n);

                        PdfPCell cell3n = new PdfPCell(new Phrase(new Chunk("Prop Checkout Time", font8)));
                        table1.AddCell(cell3n);

                        PdfPCell cell4n = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                        table1.AddCell(cell4n);

                        PdfPCell cell5n = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                        table1.AddCell(cell5n);


                        doc.Add(table1);




                    }

                    PdfPTable table3 = new PdfPTable(7);


                    float[] colWidths23u = { 20, 20, 40, 20, 45, 20, 40 };
                    table3.SetWidths(colWidths23u);
                    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                    table3.AddCell(cell9);


                    PdfPCell cell9e = new PdfPCell(new Phrase(new Chunk(dr["adv_recieptno"].ToString(), font7)));
                    table3.AddCell(cell9e);

                    PdfPCell cell9d = new PdfPCell(new Phrase(new Chunk(dr["buildingname"].ToString(), font7)));
                    table3.AddCell(cell9d);

                    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font7)));
                    table3.AddCell(cell10);
                    DateTime dated = DateTime.Parse(dr["vacatedate"].ToString());
                    string time1 = dated.ToString("dd-MM-yyyy hh:mm tt");

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(time1.ToString(), font7)));
                    table3.AddCell(cell11);

                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("4 PM", font7)));
                    table3.AddCell(cell12);

                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Over stay", font7)));
                    table3.AddCell(cell13);


                    i++;


                    doc.Add(table3);

                }

            }

            PdfPTable table4 = new PdfPTable(7);
            PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
            cellf.HorizontalAlignment = Element.ALIGN_LEFT;
            cellf.PaddingLeft = 20;
            cellf.MinimumHeight = 30;
            cellf.Colspan = 7;
            cellf.Border = 0;
            table4.AddCell(cellf);

            PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
            cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
            cellf1.PaddingLeft = 20;
            cellf1.Border = 0;
            cellf1.Colspan = 7;
            table4.AddCell(cellf1);

            PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom", font8)));
            cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
            cellh2.PaddingLeft = 20;
            cellh2.Border = 0;
            cellh2.Colspan = 7;
            table4.AddCell(cellh2);


            doc.Add(table4);


            doc.Close();
            //System.Diagnostics.Process.Start(pdfFilePath);
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
            //MessageBox.Show("Problem found in taking report", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);

            lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Problem found during report taking";
            ViewState["action"] = "warn27";
            ModalPopupExtender1.Show();


        }

    }

    # endregion

    # region Messge display
    public void messagedisplay(string message, string view)
    {
        lblHead.Text = "Tsunami ARMS - Warning";
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        lblOk.Text = message;
        ViewState["action"] = view;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnOk);

    }
    # endregion 

    # region Button Donor pass click
    protected void btnDonorPass_Click(object sender, EventArgs e)
    {
        btnDonorPass.BackColor = System.Drawing.Color.Bisque;
        pnlAccomodation.Visible = false;
        pnlReservation.Visible = false;
        pnlRoomStatus.Visible = false;
        pnlcollectioncomp.Visible = false;
        pnlRoomstatusReport.Visible = false;

        pnlDonorpass.Visible = true;

        pnlNonvacating.Visible = false;

        pnlCollection.Visible = false;
       
       // string strSql4b = " SELECT build_id, buildingname FROM  m_sub_building where rowstatus!='2'";

        OdbcCommand strSql4b = new OdbcCommand();
        strSql4b.Parameters.AddWithValue("tblname", "m_sub_building");
        strSql4b.Parameters.AddWithValue("attribute", "build_id, buildingname");
        strSql4b.Parameters.AddWithValue("conditionv", " rowstatus!='2'");

       // OdbcDataAdapter dab = new OdbcDataAdapter(strSql4b, conn);


        DataTable dtt1b = new DataTable();
        //dab.Fill(dtt1b);
        dtt1b = objcls.SpDtTbl("call selectcond(?,?,?)", strSql4b);

        DataRow rowb = dtt1b.NewRow();
        rowb["build_id"] = "-1";
        rowb["buildingname"] = "Select";
        dtt1b.Rows.InsertAt(rowb, 0);
        //da.Fill(dtt1);

        cmbDReport.DataSource = dtt1b;
        cmbDReport.DataBind();

        

        btnReservationChart.BackColor = System.Drawing.Color.Plum;
        btnAccomodationLedger.BackColor = System.Drawing.Color.Plum;
        btnRoomStatus.BackColor = System.Drawing.Color.Plum;
        btnRoomManagement.BackColor = System.Drawing.Color.Plum;
        btnNonvacating.BackColor = System.Drawing.Color.Plum;
        btncollectioncomparison.BackColor = System.Drawing.Color.Plum;
        btnCollection.BackColor = System.Drawing.Color.Plum;
        btnOtherReports.BackColor = System.Drawing.Color.Plum;







    }
    # endregion

    # region Donor Pass Utilization Report
    protected void lnkpass_Click(object sender, EventArgs e)
    {
        int doid, count5, count55;
        string dname, ptype, status2;
        int ye;
       
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
            ModalPopupExtender1.Show();
            return;
        }

       // string sqq1 = "SELECT seasonname,season_id,m.season_sub_id FROM m_sub_season ms,m_season m WHERE ms.season_sub_id=m.season_sub_id and curdate() between startdate and enddate";
      //  OdbcCommand cseas = new OdbcCommand("SELECT seasonname,season_id,m.season_sub_id FROM m_sub_season ms,m_season m WHERE ms.season_sub_id=m.season_sub_id and curdate() between startdate and enddate", conn);

        OdbcCommand sqq1 = new OdbcCommand();
        sqq1.Parameters.AddWithValue("tblname", "m_sub_season ms,m_season m");
        sqq1.Parameters.AddWithValue("attribute", "seasonname,season_id,m.season_sub_id");
        sqq1.Parameters.AddWithValue("conditionv", "ms.season_sub_id=m.season_sub_id and curdate() between startdate and enddate");


        OdbcDataReader csers = objcls.SpGetReader("call selectcond(?,?,?)", sqq1);
        if (csers.Read())
        {
            season = csers["seasonname"].ToString();
            Session["season"] = season.ToString();
            Seas = Convert.ToInt32(csers["season_sub_id"].ToString());
        }
       // string sqq2 = "SELECT mal_year_id from t_settings where curdate() between start_eng_date and end_eng_date and rowstatus<>'2'";
        //OdbcCommand Malayalam1 = new OdbcCommand("SELECT mal_year_id from t_settings where curdate() between start_eng_date and end_eng_date and rowstatus<>'2'", conn);

        OdbcCommand sqq2 = new OdbcCommand();
        sqq2.Parameters.AddWithValue("tblname", "t_settings");
        sqq2.Parameters.AddWithValue("attribute", "mal_year_id");
        sqq2.Parameters.AddWithValue("conditionv", "curdate() between start_eng_date and end_eng_date and rowstatus<>'2'");


        DataTable Malr1 = new DataTable();
        Malr1 = objcls.SpDtTbl("call selectcond(?,?,?)", sqq2);
        if (Malr1.Rows.Count > 0)
        {
            Mal = Convert.ToInt32(Malr1.Rows[0][0].ToString());
        }



        if (cmbDReport.SelectedItem.Text == "Select All")
        {

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Blocked Room";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

            PdfPTable table1 = new PdfPTable(5);
            float[] colwidth1 ={ 2, 4, 8, 3, 3 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase("DONOR PASS UTILIZATION REPORT", font9));
            cell.Colspan = 5;
            cell.Border = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);

            PdfPCell cell1e1y = new PdfPCell(new Phrase(new Chunk("Building Name :  All Building", font9)));
            cell1e1y.Colspan = 2;
            cell1e1y.Border = 0;
            cell1e1y.HorizontalAlignment = 0;
            table1.AddCell(cell1e1y);

            PdfPCell cell1e = new PdfPCell(new Phrase(new Chunk("Season Name :  " + season, font9)));
            cell1e.Colspan = 2;
            cell1e.Border = 0;
            cell1e.HorizontalAlignment = 1;
            table1.AddCell(cell1e);

            PdfPCell cell1g = new PdfPCell(new Phrase(new Chunk("Date :  " + Cur.ToString(), font9)));
            cell1g.Border = 0;
            cell1g.HorizontalAlignment = 0;
            table1.AddCell(cell1g);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell1);
            //PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Donor Id", font9)));
            //table1.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Donor Name", font9)));
            table1.AddCell(cell4);
            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("No: of Free Pass", font9)));
            table1.AddCell(cell5);
            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("No: of Paid Pass", font9)));
            table1.AddCell(cell6);
            doc.Add(table1);
            //OdbcCommand Puse = new OdbcCommand("select p.donor_id,p.build_id,donor_name,buildingname,room_id,passtype,status_pass_use from t_donorpass p,m_donor d,m_sub_building b where status_pass_use=" + "2" + "  and mal_year_id=" + Mal + " group by donor_id", conn);

            //OdbcCommand Puse = new OdbcCommand("select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno from "
            //    + "t_donorpass p,m_donor d,m_sub_building b,m_room r "
            //    + "where status_pass_use='" + "2" + "' and mal_year_id=" + Mal + " and season_id=" + Seas + " and b.build_id=p.build_id and p.donor_id=d.donor_id and r.room_id=p.room_id "
            //    + "and b.build_id=r.build_id group by donor_id order by p.room_id asc", conn);


            //string sqq4 = "select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno from "
            //     + "t_donorpass p,m_donor d,m_sub_building b,m_room r "
            //     + "where status_pass_use='" + "2" + "' and mal_year_id=" + Mal + " and season_id=" + Seas + " and b.build_id=p.build_id and p.donor_id=d.donor_id and r.room_id=p.room_id "
            //     + "and b.build_id=r.build_id group by donor_id order by p.room_id asc";

           // OdbcDataAdapter da9 = new OdbcDataAdapter(Puse);

            OdbcCommand sqq4 = new OdbcCommand();
            sqq4.Parameters.AddWithValue("tblname", "t_donorpass p,m_donor d,m_sub_building b,m_room r");
            sqq4.Parameters.AddWithValue("attribute", "p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno");
            sqq4.Parameters.AddWithValue("conditionv", "status_pass_use='" + "2" + "' and mal_year_id=" + Mal + " and season_id=" + Seas + " and b.build_id=p.build_id and p.donor_id=d.donor_id and r.room_id=p.room_id and b.build_id=r.build_id group by donor_id order by p.room_id asc");


            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("call selectcond(?,?,?)", sqq4);
           // da9.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                int slno = 0;
                for (int ii = 0; ii < dt.Rows.Count; ii++)
                {
                    doid = Convert.ToInt32(dt.Rows[ii][0].ToString());
                    slno = slno + 1;

                    if (k > 45)// total rows on page
                    {
                        k = 0;
                        doc.NewPage();
                        PdfPTable table2 = new PdfPTable(5);
                        float[] colwidth2 ={ 2, 4, 8, 3, 3 };
                        table2.SetWidths(colwidth2);

                        PdfPCell cell1q = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                        table2.AddCell(cell1q);
                        //PdfPCell cell6q = new PdfPCell(new Phrase(new Chunk("Donor Id", font9)));
                        //table2.AddCell(cell6q);
                        PdfPCell cell2q = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                        table2.AddCell(cell2q);
                        PdfPCell cell3q = new PdfPCell(new Phrase(new Chunk("Donor Name", font9)));
                        table2.AddCell(cell3q);
                        PdfPCell cell4q = new PdfPCell(new Phrase(new Chunk("No: of Free Pass", font9)));
                        table2.AddCell(cell4q);
                        PdfPCell cell5q = new PdfPCell(new Phrase(new Chunk("No: of Paid Pass", font9)));
                        table2.AddCell(cell5q);
                        doc.Add(table2);
                    }

                    PdfPTable table = new PdfPTable(5);
                    float[] colwidth3 ={ 2, 4, 8, 3, 3 };
                    table.SetWidths(colwidth3);

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table.AddCell(cell11);
                    //PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(doid.ToString(), font8)));
                    //table.AddCell(cell12);



                    string building = dt.Rows[ii]["buildingname"].ToString();

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
                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(building + " / " + dt.Rows[ii]["roomno"].ToString(), font8)));
                    table.AddCell(cell13);
                    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dt.Rows[ii]["donor_name"].ToString(), font8)));
                    table.AddCell(cell14);

                    int Fp, Pp;
                    //OdbcCommand StPass = new OdbcCommand("SELECT count(*) from t_donorpass t where t.mal_year_id="+Mal+" and donor_id=" + doid + " and status_pass='"+"0"+"' and season_id="+Seas+" and status_pass_use='" + "2" + "' group by passtype", conn);
                    //OdbcCommand StPass = new OdbcCommand("SELECT count(*) from t_donorpass t where t.mal_year_id=" + Mal + " and donor_id=" + doid + " and season_id=" + Seas + " and status_pass_use='" + "2" + "' and passtype='" + 0 + "'", conn);

                    string sqq5 = "SELECT count(*) from t_donorpass t where t.mal_year_id=" + Mal + " and donor_id=" + doid + " and season_id=" + Seas + " and status_pass_use='" + "2" + "' and passtype='" + 0 + "'";
                    OdbcDataReader stpr = objcls.GetReader(sqq5);
                    if (stpr.Read())
                    {
                        Fp = Convert.ToInt32(stpr[0].ToString());
                    }
                    else
                    {
                        Fp = 0;
                    }
                  //  OdbcCommand StPass1 = new OdbcCommand("SELECT count(*) from t_donorpass t where t.mal_year_id=" + Mal + " and donor_id=" + doid + " and season_id=" + Seas + " and status_pass_use='" + "2" + "' and passtype='" + 1 + "'", conn);
                    //string sqq6 = "SELECT count(*) from t_donorpass t where t.mal_year_id=" + Mal + " and donor_id=" + doid + " and season_id=" + Seas + " and status_pass_use='" + "2" + "' and passtype='" + 1 + "'";

                    OdbcCommand sqq6 = new OdbcCommand();
                    sqq6.Parameters.AddWithValue("tblname", "t_donorpass t ");
                    sqq6.Parameters.AddWithValue("attribute", "count(*)");
                    sqq6.Parameters.AddWithValue("conditionv", "t.mal_year_id=" + Mal + " and donor_id=" + doid + " and season_id=" + Seas + " and status_pass_use='" + "2" + "' and passtype='" + 1 + "'");


                    OdbcDataReader stpr1 = objcls.SpGetReader("call selectcond(?,?,?)", sqq6);
                    if (stpr1.Read())
                    {
                        Pp = Convert.ToInt32(stpr1[0].ToString());
                    }
                    else
                    {
                        Pp = 0;
                    }

                    if (Fp == 0)
                    {
                        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                        table.AddCell(cell15);
                    }
                    else
                    {
                        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(Fp.ToString(), font8)));
                        table.AddCell(cell15);
                    }
                    if (Pp == 0)
                    {
                        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                        table.AddCell(cell16);
                    }
                    else
                    {
                        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(Pp.ToString(), font8)));
                        table.AddCell(cell16);
                    }

                    k++;
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
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Donor Pass Utilization Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }

        else if (cmbDReport.SelectedItem.Text != "Select All" && cmbDReport.SelectedItem.Text != "--Select--")
        {


            building = cmbDReport.SelectedValue.ToString();
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

            PdfPTable table1 = new PdfPTable(5);
            float[] colwidth1 ={ 2, 4, 8, 3, 3 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase("DONOR PASS UTILIZATION REPORT", font9));
            cell.Colspan = 5;
            cell.Border = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);

            PdfPCell cell1e1y = new PdfPCell(new Phrase(new Chunk("Building Name:  " + cmbDReport.SelectedItem.Text.ToString(), font9)));
            cell1e1y.Colspan = 2;
            cell1e1y.Border = 0;
            cell1e1y.HorizontalAlignment = 0;
            table1.AddCell(cell1e1y);

            PdfPCell cell1e1 = new PdfPCell(new Phrase(new Chunk("Season Name :  " + season, font9)));
            cell1e1.Colspan = 2;
            cell1e1.Border = 0;
            cell1e1.HorizontalAlignment = 1;
            table1.AddCell(cell1e1);

            PdfPCell cell1g1 = new PdfPCell(new Phrase(new Chunk("Date :  " + Cur.ToString(), font9)));

            cell1g1.Border = 0;
            cell1g1.HorizontalAlignment = 0;
            table1.AddCell(cell1g1);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table1.AddCell(cell1);
            //PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Donor Id", font9)));
            //table1.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Donor Name", font9)));
            table1.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("No: of Free Pass", font9)));
            table1.AddCell(cell5);
            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("No: of Paid Pass", font9)));
            table1.AddCell(cell6);
            doc.Add(table1);

            //OdbcCommand Puse1 = new OdbcCommand("select p.donor_id,p.build_id,donor_name,buildingname,room_id,passtype,status_pass_use from t_donorpass p,m_donor d,m_sub_building b where status_pass_use=" + "2" + " and mal_year_id=" + Mal + " and p.build_id=" + building + " group by donor_id", conn);
            //OdbcCommand Puse1 = new OdbcCommand("select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno from t_donorpass p,"
            //    + "m_donor d,m_sub_building b,m_room r "
            //    + "where status_pass_use='" + 2 + "' and status_pass='" + 0 + "' and mal_year_id=" + Mal + " and season_id=" + Seas + " and b.build_id=p.build_id and p.donor_id=d.donor_id and r.room_id=p.room_id "
            //    + "and b.build_id=r.build_id and p.build_id=" + building + " group by donor_id order by p.room_id asc", conn);

          //  string sqqq1 = "select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno from t_donorpass p,"
          //      + "m_donor d,m_sub_building b,m_room r "
          //      + "where status_pass_use='" + 2 + "' and status_pass='" + 0 + "' and mal_year_id=" + Mal + " and season_id=" + Seas + " and b.build_id=p.build_id and p.donor_id=d.donor_id and r.room_id=p.room_id "
          //      + "and b.build_id=r.build_id and p.build_id=" + building + " group by donor_id order by p.room_id asc";
          ////  OdbcDataAdapter da91 = new OdbcDataAdapter(Puse1);


            OdbcCommand sqqq1 = new OdbcCommand();
            sqqq1.Parameters.AddWithValue("tblname", "t_donorpass p, m_donor d,m_sub_building b,m_room r ");
            sqqq1.Parameters.AddWithValue("attribute", "p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno");
            sqqq1.Parameters.AddWithValue("conditionv", "status_pass_use='" + 2 + "' and status_pass='" + 0 + "' and mal_year_id=" + Mal + " and season_id=" + Seas + " and b.build_id=p.build_id and p.donor_id=d.donor_id and r.room_id=p.room_id and b.build_id=r.build_id and p.build_id=" + building + " group by donor_id order by p.room_id asc");

            
            
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("call selectcond(?,?,?)", sqqq1);
           // da91.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                int slno = 0;
                for (int ii = 0; ii < dt.Rows.Count; ii++)
                {
                    doid = Convert.ToInt32(dt.Rows[ii][0].ToString());
                    slno = slno + 1;

                    if (D > 45)// total rows on page
                    {
                        D = 0;
                        doc.NewPage();
                        PdfPTable table2 = new PdfPTable(5);
                        float[] colwidth3 ={ 2, 4, 8, 3, 3 };
                        table2.SetWidths(colwidth3);
                        PdfPCell cell1q = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                        table2.AddCell(cell1q);
                        //PdfPCell cell6q = new PdfPCell(new Phrase(new Chunk("Donor Id", font9)));
                        //table2.AddCell(cell6q);
                        PdfPCell cell2q = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                        table2.AddCell(cell2q);
                        PdfPCell cell3q = new PdfPCell(new Phrase(new Chunk("Donor Name", font9)));
                        table2.AddCell(cell3q);
                        PdfPCell cell4q = new PdfPCell(new Phrase(new Chunk("No: of Free Pass", font9)));
                        table2.AddCell(cell4q);
                        PdfPCell cell5q = new PdfPCell(new Phrase(new Chunk("No: of Paid Pass", font9)));
                        table2.AddCell(cell5q);
                        doc.Add(table2);
                    }

                    PdfPTable table = new PdfPTable(5);
                    float[] colwidth4 ={ 2, 4, 8, 3, 3 };
                    table.SetWidths(colwidth4);

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table.AddCell(cell11);

                    //PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(doid.ToString(), font8)));
                    //table.AddCell(cell12);

                    string building1 = dt.Rows[ii]["buildingname"].ToString();

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

                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(building1 + "/ " + dt.Rows[ii]["roomno"].ToString(), font8)));
                    table.AddCell(cell13);
                    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dt.Rows[ii]["donor_name"].ToString(), font8)));
                    table.AddCell(cell14);


                    int Fp, Pp;
                    //OdbcCommand StPass = new OdbcCommand("SELECT count(*) from t_donorpass t where t.mal_year_id="+Mal+" and donor_id=" + doid + " and status_pass='"+"0"+"' and season_id="+Seas+" and status_pass_use='" + "2" + "' group by passtype", conn);
                   // OdbcCommand StPass = new OdbcCommand("SELECT count(*) from t_donorpass t where t.mal_year_id=" + Mal + " and donor_id=" + doid + " and status_pass='" + "0" + "' and season_id=" + Seas + " and status_pass_use='" + "2" + "' and passtype='" + 0 + "'", conn);
                   // string sqqq2 = "SELECT count(*) from t_donorpass t where t.mal_year_id=" + Mal + " and donor_id=" + doid + " and status_pass='" + "0" + "' and season_id=" + Seas + " and status_pass_use='" + "2" + "' and passtype='" + 0 + "'";

                    OdbcCommand sqqq2 = new OdbcCommand();
                    sqqq2.Parameters.AddWithValue("tblname", "t_donorpass t ");
                    sqqq2.Parameters.AddWithValue("attribute", "count(*)");
                    sqqq2.Parameters.AddWithValue("conditionv", "t.mal_year_id=" + Mal + " and donor_id=" + doid + " and status_pass='" + "0" + "' and season_id=" + Seas + " and status_pass_use='" + "2" + "' and passtype='" + 0 + "'");


                    OdbcDataReader stpr = objcls.SpGetReader("ca;; selectcond(?,?,?)", sqqq2);
                    if (stpr.Read())
                    {
                        Fp = Convert.ToInt32(stpr[0].ToString());
                    }
                    else
                    {
                        Fp = 0;
                    }
                    //OdbcCommand StPass1 = new OdbcCommand("SELECT count(*) from t_donorpass t where t.mal_year_id=" + Mal + " and donor_id=" + doid + " and status_pass='" + "0" + "' and season_id=" + Seas + " and status_pass_use='" + "2" + "' and passtype='" + 1 + "'", conn);
                   // string sqqq3 = "SELECT count(*) from t_donorpass t where t.mal_year_id=" + Mal + " and donor_id=" + doid + " and status_pass='" + "0" + "' and season_id=" + Seas + " and status_pass_use='" + "2" + "' and passtype='" + 1 + "'";


                    OdbcCommand sqqq3 = new OdbcCommand();
                    sqqq3.Parameters.AddWithValue("tblname", "t_donorpass t ");
                    sqqq3.Parameters.AddWithValue("attribute", "count(*)");
                    sqqq3.Parameters.AddWithValue("conditionv", "t.mal_year_id=" + Mal + " and donor_id=" + doid + " and status_pass='" + "0" + "' and season_id=" + Seas + " and status_pass_use='" + "2" + "' and passtype='" + 1 + "'");


                    OdbcDataReader stpr1 = objcls.SpGetReader("call selectcond(?,?,?)", sqqq3);
                    if (stpr1.Read())
                    {
                        Pp = Convert.ToInt32(stpr1[0].ToString());
                    }
                    else
                    {
                        Pp = 0;
                    }

                    if (Fp == 0)
                    {
                        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                        table.AddCell(cell15);
                    }
                    else
                    {
                        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(Fp.ToString(), font8)));
                        table.AddCell(cell15);
                    }
                    if (Pp == 0)
                    {
                        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                        table.AddCell(cell16);
                    }
                    else
                    {
                        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(Pp.ToString(), font8)));
                        table.AddCell(cell16);
                    }

                    D++;
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
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch1.ToString() + "&Title=Donor Passs Utilization Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            //conn.Close();
        }
    }
    # endregion

    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtToDate);
    }
    # region Collection Comparison
    protected void lnkConsolidatedIncomeReport_Click(object sender, EventArgs e)
    {
        //int malyear1=0, malyearid = 0;

        int malyearid = 0;
        if ((txtFromDate.Text != "") && (txtToDate.Text != ""))
        {
          
            string fromdate = objcls.yearmonthdate(txtFromDate.Text);
            string todate = objcls.yearmonthdate(txtToDate.Text);

            // SELECT mal_year from t_settings where '2010-11-10' between start_eng_date and end_eng_date or '2010-11-15' between start_eng_date and 
            //end_eng_date or start_eng_date between '2010-11-10' and '2010-11-15' or end_eng_date between '2010-11-10' and '2010-11-15'


            //OdbcCommand cmdmalyear = new OdbcCommand("SELECT mal_year from t_settings where '"+fromdate+"' between start_eng_date and end_eng_date or '2010-11-15' between start_eng_date and end_eng_date or start_eng_date between '2010-11-10' and '2010-11-15' or end_eng_date between '2010-11-10' and '2010-11-15'", conn);

            //OdbcCommand cmdmalyear = new OdbcCommand("select mal_year,mal_year_id from  t_settings where   end_eng_date>='" + fromdate + "'  and start_eng_date<'" + todate + "'", conn);

            //string sw1 = "select mal_year,mal_year_id from  t_settings where   end_eng_date>='" + fromdate + "'  and start_eng_date<'" + todate + "'";

            OdbcCommand sw1 = new OdbcCommand();
            sw1.Parameters.AddWithValue("tblname", "t_settings ");
            sw1.Parameters.AddWithValue("attribute", "mal_year,mal_year_id");
            sw1.Parameters.AddWithValue("conditionv", "end_eng_date>='" + fromdate + "'  and start_eng_date<'" + todate + "'");


            OdbcDataReader ormalyear = objcls.SpGetReader("call selectcond(?,?,?)", sw1);
            int malyear1 = 0, malyear2 = 0, malyear3 = 0;
            if (ormalyear.Read())
            {
                malyear1 = Convert.ToInt32(ormalyear["mal_year"]);
                malyearid = Convert.ToInt32(ormalyear["mal_year_id"]);
                Session["malyear"] = malyear1;
                Session["malyyearid"] = malyearid;

            }
            malyear2 = malyear1 - 1;
            malyear3 = malyear1 - 2;
            string[] totdate = new string[1000];
            string[] totdate1 = new string[1000];
            string[] totdate2 = new string[1000];
            OdbcCommand cmdselectdate = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
            //cmdselectdate.CommandType = CommandType.StoredProcedure;
            cmdselectdate.Parameters.AddWithValue("tblname", " t_liabilityregister");
            cmdselectdate.Parameters.AddWithValue("attribute", "  distinct dayend");
            cmdselectdate.Parameters.AddWithValue("conditionv", "dayend>='" + fromdate + "' and dayend<='" + todate + "' and ledger_id=1 order by dayend asc");

           // OdbcDataAdapter da = new OdbcDataAdapter(cmdselectdate);
            DataTable dttdate = new DataTable();
            dttdate = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdate);

           // da.Fill(dttdate);
            int count = 0;
            if (dttdate.Rows.Count > 0)
            {

                for (int i = 0; i < dttdate.Rows.Count; i++)
                {
                    // DateTime dt5 = DateTime.Parse(dr["date"].ToString());
                    //string date1 = dt5.ToString("dd-MM-yyyy");


                    DateTime date5 = DateTime.Parse(dttdate.Rows[i]["dayend"].ToString());

                    string date1 = date5.ToString("dd/MM/yyyy");
                    string dater = date5.ToString("MM/dd/yyyy");
                    DateTime date3 = DateTime.Parse(dater);
                    date1 = objcls.yearmonthdate(date1);

                    int year11 = date3.Year;
                    int year22 = year11 - 1;
                    int year33 = year11 - 2;
                    string prevyear = date3.Day + "/" + date3.Month + "/" + year22;
                    string prevyear1 = date3.Day + "/" + date3.Month + "/" + year33;

                    prevyear = objcls.yearmonthdate(prevyear);
                    prevyear1 = objcls.yearmonthdate(prevyear1);
                    totdate[i] = date1;
                    totdate1[i] = prevyear;
                    totdate2[i] = prevyear1;
                    count++;

                }


            }


            string fromdate1 = totdate1[0];
            string fromdate2 = totdate2[0];
            //OdbcCommand cmdcreate = new OdbcCommand("create table consolidatedcollection (date date, total int(30), cumilative int(40),year1 int(30),year2 int(30))", conn);
            //cmdcreate.ExecuteNonQuery();

            //for (int i = 0; i < count; i++)
            //{
            //    OdbcCommand cmdinsert = new OdbcCommand("insert into consolidatedcollection(date)values('" + totdate[i] + "')", conn);
            //    cmdinsert.ExecuteNonQuery();
            //}
            DataTable dttotalamount = new DataTable();
            dttotalamount.Columns.Clear();
            dttotalamount.Columns.Add("date", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("total", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("cumilative", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("year1", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("cum1", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("year2", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("cum2", System.Type.GetType("System.String"));



            // int iRow = dtDisplay.Rows.Count;
            //dtDisplay2.Rows.Add();
            //dtDisplay2.Rows[0]["display_id"] = displayid;
            //dtDisplay2.Rows[0]["Slno"] = 1;
            //dtDisplay2.Rows[0]["displayname"] = reports;

            for (int i = 0; i < count; i++)
            {


                string datea = totdate[i];

                int count1 = 0;
                int count2 = 0;
                OdbcCommand cmdselectdata = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                //cmdselectdata.CommandType = CommandType.StoredProcedure;
                cmdselectdata.Parameters.AddWithValue("tblname", "t_liabilityregister");
                cmdselectdata.Parameters.AddWithValue("attribute", "sum(total) as total ");
                cmdselectdata.Parameters.AddWithValue("conditionv", " dayend='" + datea + "' and ledger_id=1 ");
             //   OdbcDataAdapter das = new OdbcDataAdapter(cmdselectdata);
                DataTable dttdate1 = new DataTable();
                //das.Fill(dttdate1);
                dttdate1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata);
                int amount1 = 0, amountcum = 0, prevamount = 0, prevamount1 = 0, prevcum = 0, prevcum1 = 0;

                if (dttdate1.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate1.Rows[0]["total"]) == false)
                    {

                        amount1 = Convert.ToInt32(dttdate1.Rows[0]["total"]);

                    }

                }

                OdbcCommand cmdselectdata1 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
               // cmdselectdata1.CommandType = CommandType.StoredProcedure;
                cmdselectdata1.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata1.Parameters.AddWithValue("attribute", "sum(total)as total1 ");
                cmdselectdata1.Parameters.AddWithValue("conditionv", " dayend>='" + fromdate + "' and dayend<='" + datea + "' and ledger_id=1 ");
               // OdbcDataAdapter das1 = new OdbcDataAdapter(cmdselectdata1);
                DataTable dttdate11 = new DataTable();
                dttdate11 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata1);
               // das1.Fill(dttdate11);

                if (dttdate11.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate11.Rows[0]["total1"]) == false)
                    {

                        amountcum = Convert.ToInt32(dttdate11.Rows[0]["total1"]);

                    }



                }

                OdbcCommand cmdselectdata12 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
               // cmdselectdata12.CommandType = CommandType.StoredProcedure;
                cmdselectdata12.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata12.Parameters.AddWithValue("attribute", "sum(total) as total ");
                cmdselectdata12.Parameters.AddWithValue("conditionv", " dayend='" + totdate1[i] + "' and ledger_id=1");
               // OdbcDataAdapter das12 = new OdbcDataAdapter(cmdselectdata12);
                DataTable dttdate112 = new DataTable();
                dttdate112 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata12);
               // das12.Fill(dttdate112);

                if (dttdate112.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate112.Rows[0]["total"]) == false)
                    {
                        string bb = totdate1[i];

                        prevamount = Convert.ToInt32(dttdate112.Rows[0]["total"]);

                    }



                }


                OdbcCommand cmdselectdata11 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
               // cmdselectdata11.CommandType = CommandType.StoredProcedure;
                cmdselectdata11.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata11.Parameters.AddWithValue("attribute", "sum(total) as total11 ");
                cmdselectdata11.Parameters.AddWithValue("conditionv", " dayend>='" + fromdate1 + "' and dayend<='" + totdate1[i] + "' and ledger_id=1 ");
               // OdbcDataAdapter das11 = new OdbcDataAdapter(cmdselectdata11);
                DataTable dttdate111 = new DataTable();
                dttdate111 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata11);
               // das11.Fill(dttdate111);

                if (dttdate111.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate111.Rows[0]["total11"]) == false)
                    {

                        prevcum = Convert.ToInt32(dttdate111.Rows[0]["total11"]);

                    }



                }

                OdbcCommand cmdselectdata0 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                //cmdselectdata0.CommandType = CommandType.StoredProcedure;
                cmdselectdata0.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata0.Parameters.AddWithValue("attribute", "sum(total)  as total22");
                cmdselectdata0.Parameters.AddWithValue("conditionv", " dayend='" + totdate2[i] + "' and ledger_id=1");
               // OdbcDataAdapter das0 = new OdbcDataAdapter(cmdselectdata0);
                DataTable dttdate10 = new DataTable();
                dttdate10 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata0);
                //das0.Fill(dttdate10);

                if (dttdate10.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate10.Rows[0]["total22"]) == false)
                    {

                        prevamount1 = Convert.ToInt32(dttdate10.Rows[0]["total22"]);

                    }

                }

                string ff = totdate2[i];
                OdbcCommand cmdselectdata121 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
              //  cmdselectdata121.CommandType = CommandType.StoredProcedure;
                cmdselectdata121.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata121.Parameters.AddWithValue("attribute", "sum(total) as total112 ");
                cmdselectdata121.Parameters.AddWithValue("conditionv", " dayend>='" + fromdate2 + "' and dayend<='" + totdate2[i] + "' and ledger_id=1 ");
                //OdbcDataAdapter das121 = new OdbcDataAdapter(cmdselectdata121);
                DataTable dttdate1121 = new DataTable();
                dttdate1121 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata121);
                //das121.Fill(dttdate1121);

                if (dttdate1121.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate1121.Rows[0]["total112"]) == false)
                    {

                        prevcum1 = Convert.ToInt32(dttdate1121.Rows[0]["total112"]);

                    }



                }

                dttotalamount.Rows.Add();
                dttotalamount.Rows[i]["date"] = datea;
                if (amount1 == 0)
                {
                    dttotalamount.Rows[i]["total"] = "";
                }
                else
                {

                    dttotalamount.Rows[i]["total"] = amount1;
                }
                if (amountcum == 0)
                {
                    dttotalamount.Rows[i]["cumilative"] = "";
                }
                else
                {
                    dttotalamount.Rows[i]["cumilative"] = amountcum;
                }
                if (prevamount == 0)
                {
                    dttotalamount.Rows[i]["year1"] = "";
                }
                else
                {
                    dttotalamount.Rows[i]["year1"] = prevamount;
                }
                if (prevcum == 0)
                {
                    dttotalamount.Rows[i]["cum1"] = "";
                }
                else
                {
                    dttotalamount.Rows[i]["cum1"] = prevcum;
                }
                if (prevamount1 == 0)
                {
                    dttotalamount.Rows[i]["year2"] = "";

                }
                else
                {

                    dttotalamount.Rows[i]["year2"] = prevamount1;
                }
                if (prevcum1 == 0)
                {
                    dttotalamount.Rows[i]["cum2"] = "";
                }
                else
                {
                    dttotalamount.Rows[i]["cum2"] = prevcum1;
                }


                
            }
            DateTime datedt = DateTime.Now;

            string dt1 = datedt.ToString("dd  MMMM  yyyy");

            string time1 = datedt.ToString(" hh :mm tt");

            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");


            string ch = "collectionComparison" + transtim.ToString() + ".pdf";

            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 70);
            //string pdfFilePath = Server.MapPath(".") + "/pdf/consolidatedcollection.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9);
            Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            //PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            doc.Open();
            page.strRptMode = "Collection Comparison";
            PdfPTable table = new PdfPTable(8);
            float[] colW11 = { 10, 30, 20, 20, 20, 20, 20, 20 };
            table.SetWidths(colW11);


            PdfPCell cell = new PdfPCell(new Phrase("Consolidated   Collection comparison report Taken on   " + dt1 + " at " + time1, font12));
            cell.Colspan = 8;
            cell.HorizontalAlignment = 1;
            cell.Border = 1;
            table.AddCell(cell);
            PdfPCell cellc1 = new PdfPCell(new Phrase("No", font9));
            //cellc.Colspan = 3;
            cellc1.Rowspan = 2;
            cellc1.HorizontalAlignment = 1;
            table.AddCell(cellc1);


            PdfPCell cellc = new PdfPCell(new Phrase("Date", font9));
            //cellc.Colspan = 3;
            cellc.HorizontalAlignment = 1;
            cellc.Rowspan = 2;
            table.AddCell(cellc);

            PdfPCell cella = new PdfPCell(new Phrase(malyear1.ToString(), font9));
            cella.Colspan = 2;
            cella.HorizontalAlignment = 1;
            table.AddCell(cella);

            PdfPCell cellb = new PdfPCell(new Phrase(malyear2.ToString(), font9));
            cellb.Colspan = 2;
            cellb.HorizontalAlignment = 1;
            table.AddCell(cellb);
            PdfPCell cell11q = new PdfPCell(new Phrase(malyear3.ToString(), font9));
            cell11q.Colspan = 2;
            cell11q.HorizontalAlignment = 1;
            table.AddCell(cell11q);


            PdfPCell cellxvvv = new PdfPCell(new Phrase("Day's Coln", font8));
            //cellxvvv.Colspan = 1;
            cellxvvv.HorizontalAlignment = 1;
            table.AddCell(cellxvvv);

            PdfPCell cellx = new PdfPCell(new Phrase("Cum Coln", font8));
            cellx.Colspan = 1;
            cellx.HorizontalAlignment = 1;
            table.AddCell(cellx);

            PdfPCell cell1h = new PdfPCell(new Phrase("Day's Coln", font8));
            cell1h.Colspan = 1;
            cell1h.HorizontalAlignment = 1;
            table.AddCell(cell1h);


            PdfPCell cell11n = new PdfPCell(new Phrase("Cum Coln", font8));
            //cell11n.Colspan = 3;
            cell11n.HorizontalAlignment = 1;
            table.AddCell(cell11n);
            PdfPCell cell1h1 = new PdfPCell(new Phrase("Day's Coln", font8));
            cell1h1.Colspan = 1;
            cell1h1.HorizontalAlignment = 1;
            table.AddCell(cell1h1);


            PdfPCell cell11n1 = new PdfPCell(new Phrase("Cum Coln", font8));
            //cell11n.Colspan = 3;
            cell11n1.HorizontalAlignment = 1;
            table.AddCell(cell11n1);



            doc.Add(table);

            int slno = 0, ii = 0;
            foreach (DataRow dr in dttotalamount.Rows)
            {

                slno = slno + 1;
                if (ii > 40)
                {
                    ii = 0;
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(8);
                    float[] colW111 = { 10, 30, 20, 20, 20, 20, 20, 20 };
                    table1.SetWidths(colW111);

                    PdfPCell cell11v12 = new PdfPCell(new Phrase("No", font8));
                    //cell11v1.Colspan= 3;
                    cell11v12.Rowspan = 2;
                    cell11v12.HorizontalAlignment = 1;
                    table1.AddCell(cell11v12);


                    PdfPCell cell11v1 = new PdfPCell(new Phrase("Date", font8));
                    //cell11v1.Colspan= 3;
                    cell11v1.Rowspan = 2;
                    cell11v1.HorizontalAlignment = 1;
                    table1.AddCell(cell11v1);

                    PdfPCell cell11v = new PdfPCell(new Phrase(malyear1.ToString(), font8));
                    cell11v.Colspan = 2;
                    cell11v.HorizontalAlignment = 1;
                    table1.AddCell(cell11v);


                    PdfPCell cell112v = new PdfPCell(new Phrase(malyear2.ToString(), font8));
                    cell112v.Colspan = 2;
                    cell112v.HorizontalAlignment = 1;
                    table1.AddCell(cell112v);

                    PdfPCell cell11qv = new PdfPCell(new Phrase(malyear3.ToString(), font8));
                    cell11qv.Colspan = 2;
                    cell11qv.HorizontalAlignment = 1;
                    table1.AddCell(cell11qv);



                    PdfPCell cellxv = new PdfPCell(new Phrase("Total Coln", font8));
                    cellxv.Colspan = 1;
                    cellxv.HorizontalAlignment = 1;
                    table1.AddCell(cellxv);

                    PdfPCell cellk = new PdfPCell(new Phrase(" Total Cum Coln", font8));
                    cellk.Colspan = 1;
                    cellk.HorizontalAlignment = 1;
                    table1.AddCell(cellk);

                    PdfPCell cell1hv = new PdfPCell(new Phrase("Total Coln", font8));
                    cell1hv.Colspan = 1;
                    cell1hv.HorizontalAlignment = 1;
                    table1.AddCell(cell1hv);

                    PdfPCell cell11ny = new PdfPCell(new Phrase("Total Cum Coln", font8));
                    //cel1l1n.Colspan = 3;
                    cell11ny.HorizontalAlignment = 1;
                    table1.AddCell(cell11ny);

                    PdfPCell cell1hvb = new PdfPCell(new Phrase("Total Coln", font8));
                    cell1hvb.Colspan = 1;
                    cell1hvb.HorizontalAlignment = 1;
                    table1.AddCell(cell1hvb);

                    PdfPCell cell11nyb = new PdfPCell(new Phrase("Total Cum Coln", font8));
                    //cel1l1n.Colspan = 3;
                    cell11nyb.HorizontalAlignment = 1;
                    table1.AddCell(cell11nyb);



                    doc.Add(table1);


                }


                ii++;

                PdfPTable table2 = new PdfPTable(8);
                float[] colW1111 = { 10, 30, 20, 20, 20, 20, 20, 20 };
                table2.SetWidths(colW1111);
                PdfPCell cell11v2d = new PdfPCell(new Phrase(slno.ToString(), font7));
                //cell11v2.Colspan = 3;
                cell11v2d.HorizontalAlignment = 1;
                table2.AddCell(cell11v2d);



                DateTime dtd = DateTime.Parse(dr["date"].ToString());
                string datert = dtd.ToString("dd MMMM");

                PdfPCell cell11v2 = new PdfPCell(new Phrase(datert.ToString(), font7));
                //cell11v2.Colspan = 3;
                cell11v2.HorizontalAlignment = 0;
                table2.AddCell(cell11v2);

                PdfPCell cell112v22 = new PdfPCell(new Phrase(dr["total"].ToString(), font7));
                cell112v22.Colspan = 1;
                cell112v22.HorizontalAlignment = 1;
                table2.AddCell(cell112v22);

                PdfPCell cellxv2 = new PdfPCell(new Phrase(dr["cumilative"].ToString(), font7));
                cellxv2.Colspan = 1;
                cellxv2.HorizontalAlignment = 1;
                table2.AddCell(cellxv2);



                PdfPCell cell11qv2 = new PdfPCell(new Phrase(dr["year1"].ToString(), font7));
                //cel11q.Colspan = 1;
                cell11qv2.HorizontalAlignment = 1;
                table2.AddCell(cell11qv2);

                PdfPCell cell11qv22 = new PdfPCell(new Phrase(dr["cum1"].ToString(), font7));
                //cel11q.Colspan = 1;
                cell11qv22.HorizontalAlignment = 1;
                table2.AddCell(cell11qv22);




                PdfPCell cell11v21 = new PdfPCell(new Phrase(dr["year2"].ToString(), font7));
                //cel112.Colspanv = 3;
                cell11v21.HorizontalAlignment = 1;
                table2.AddCell(cell11v21);

                PdfPCell cell11v211 = new PdfPCell(new Phrase(dr["cum2"].ToString(), font7));
                //cel112.Colspanv = 3;
                cell11v211.HorizontalAlignment = 1;
                table2.AddCell(cell11v211);

                doc.Add(table2);


            }

            PdfPTable table4 = new PdfPTable(8);

            PdfPCell cellff = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
            cellff.HorizontalAlignment = Element.ALIGN_LEFT;
            cellff.PaddingLeft = 30;
            cellff.Colspan = 8;

            cellff.MinimumHeight = 30;
            cellff.Border = 0;
            table4.AddCell(cellff);

            PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
            cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
            cellf1.PaddingLeft = 30;
            cellf1.Colspan = 8;
            cellf1.Border = 0;
            table4.AddCell(cellf1);

            PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom  ", font8)));
            cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
            cellh2.PaddingLeft = 30;
            cellh2.Border = 0;
            cellh2.Colspan = 8;
            table4.AddCell(cellh2);

            doc.Add(table4);

            doc.Close();
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=OllectionComparison";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);



        }
        else
        {

            lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Enter the From date or To date";
            ViewState["action"] = "warn12";
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnOk);


        }

    }
    # endregion

    # region To date text change
    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        string str1 = objcls.yearmonthdate(txtFromDate.Text);
      //  str1 = mm + "-" + dd + "-" + yy;
        DateTime dt1 = DateTime.Parse(str1);
        string str2 = objcls.yearmonthdate(txtToDate.Text);
     //   str2 = mm + "-" + dd + "-" + yy;
        DateTime dt2 = DateTime.Parse(str2);
        if (dt1 > dt2)
        {

            lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "From date is greater than To date";
            ViewState["action"] = "warn28";
            ModalPopupExtender1.Show();

            //MessageBox.Show("From date is greater than To date", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3, MessageBoxOptions.DefaultDesktopOnly);
            //this.ScriptManager1.SetFocus(txttodate);


        }
    }

    # endregion 

    # region Button Collection Comparison check
    protected void btncollectioncomparison_Click(object sender, EventArgs e)
    {
        btncollectioncomparison.BackColor = System.Drawing.Color.Bisque;
        pnlAccomodation.Visible = false;
        pnlReservation.Visible = false;
        pnlRoomStatus.Visible = false;

        pnlRoomstatusReport.Visible = false;

        pnlDonorpass.Visible = false;

        pnlNonvacating.Visible = false;

        pnlCollection.Visible = false;
        pnlcollectioncomp.Visible = true;


        btnReservationChart.BackColor = System.Drawing.Color.Plum;
        btnAccomodationLedger.BackColor = System.Drawing.Color.Plum;
        btnRoomStatus.BackColor = System.Drawing.Color.Plum;
        btnRoomManagement.BackColor = System.Drawing.Color.Plum;
        btnNonvacating.BackColor = System.Drawing.Color.Plum;
        btnDonorPass.BackColor = System.Drawing.Color.Plum;
        btnCollection.BackColor = System.Drawing.Color.Plum;
        btnOtherReports.BackColor = System.Drawing.Color.Plum;

       

    }
    # endregion
    protected void btnOtherReports_Click(object sender, EventArgs e)
    {
        btnReservationChart.BackColor = System.Drawing.Color.Plum;
        btnAccomodationLedger.BackColor = System.Drawing.Color.Plum;
        btnRoomStatus.BackColor = System.Drawing.Color.Plum;
        btnRoomManagement.BackColor = System.Drawing.Color.Plum;
        btnNonvacating.BackColor = System.Drawing.Color.Plum;
        btnDonorPass.BackColor = System.Drawing.Color.Plum;
        btncollectioncomparison.BackColor = System.Drawing.Color.Plum;
        btnOtherReports.BackColor = System.Drawing.Color.Plum;
        btnCollection.BackColor = System.Drawing.Color.Plum;
        btnOtherReports.BackColor = System.Drawing.Color.Bisque;

        pnlAccomodation.Visible = false;
        pnlReservation.Visible = false;
        pnlRoomStatus.Visible = false;

        pnlRoomstatusReport.Visible = false;

        pnlDonorpass.Visible = false;

        pnlNonvacating.Visible = false;

        pnlCollection.Visible = false;
        pnlcollectioncomp.Visible = false;
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string qry = @" SELECT
                        m_sub_building.buildingname as 'Building Name',
                        m_room.roomno as 'Room No',
                        m_sub_room_category.rent as 'Rent(12 hr)',
                        m_sub_room_category.rent as 'Deposite(12 hr)',
                        m_sub_room_category.rent_1 as 'Rent(16 hr)',
                        m_sub_room_category.rent_1 as 'Deposite(16 hr)'
                        FROM
                        m_sub_building
                        INNER JOIN m_room ON m_sub_building.build_id = m_room.build_id
                        INNER JOIN m_sub_room_category ON m_room.room_cat_id = m_sub_room_category.room_cat_id
                        WHERE
                        m_sub_building.rowstatus <> 2 AND
                        m_room.rowstatus <> 2 AND
                        m_sub_room_category.rowstatus <> 2";

        DataTable dt = new DataTable();
        dt = objcls.DtTbl(qry);
       
        if (dt.Rows.Count > 0)
        {
            GetExcel(dt, "Room Rent and Security Deposite Dertails");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }

    }

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

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        string qry = @" SELECT
            m_sub_building.buildingname as 'Building Name',
            m_room.roomno as 'Room No',
            m_donor.donor_name as 'Donor',
            m_donor.housename as 'House Name',
            m_donor.housenumber as 'House No',
            m_donor.address1 as 'Address1',
            m_donor.address2 as 'Address2',
            m_donor.pincode as 'Pincode',
            m_sub_state.statename as 'Sate',
            m_sub_district.districtname as 'District',
            m_sub_room_category.rent as 'Rent(12 hr)',
            m_sub_room_category.rent as 'Deposite(12 hr)',
            m_sub_room_category.rent_1 as 'Rent(16 hr)',
            m_sub_room_category.rent_1 as 'Deposite(16 hr)'

        FROM
            m_donor
            INNER JOIN m_room ON m_donor.donor_id = m_room.donor_id
            INNER JOIN m_sub_building ON m_room.build_id = m_sub_building.build_id
            INNER JOIN m_sub_district ON m_donor.district_id = m_sub_district.district_id
            INNER JOIN m_sub_state ON m_sub_district.state_id = m_sub_state.state_id
            INNER JOIN m_sub_room_category ON m_room.room_cat_id = m_sub_room_category.room_cat_id
        WHERE
            m_donor.rowstatus <> 2 AND
            m_room.rowstatus <> 2 AND
            m_sub_building.rowstatus <> 2 AND
            m_sub_room_category.rowstatus <> 2
        ORDER BY
            m_sub_building.build_id ASC,
            m_room.roomno ASC ";

        DataTable dt = new DataTable();
        dt = objcls.DtTbl(qry);

        if (dt.Rows.Count > 0)
        {
            GetExcel(dt, "Donor Dertails");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    protected void lnk_total_Click(object sender, EventArgs e)
    {

        int malyearid = 0;
        if ((txtFromDate.Text != "") && (txtToDate.Text != ""))
        {

            string fromdate = objcls.yearmonthdate(txtFromDate.Text);
            string todate = objcls.yearmonthdate(txtToDate.Text);

            // SELECT mal_year from t_settings where '2010-11-10' between start_eng_date and end_eng_date or '2010-11-15' between start_eng_date and 
            //end_eng_date or start_eng_date between '2010-11-10' and '2010-11-15' or end_eng_date between '2010-11-10' and '2010-11-15'


            //OdbcCommand cmdmalyear = new OdbcCommand("SELECT mal_year from t_settings where '"+fromdate+"' between start_eng_date and end_eng_date or '2010-11-15' between start_eng_date and end_eng_date or start_eng_date between '2010-11-10' and '2010-11-15' or end_eng_date between '2010-11-10' and '2010-11-15'", conn);

            //OdbcCommand cmdmalyear = new OdbcCommand("select mal_year,mal_year_id from  t_settings where   end_eng_date>='" + fromdate + "'  and start_eng_date<'" + todate + "'", conn);

            //string sw1 = "select mal_year,mal_year_id from  t_settings where   end_eng_date>='" + fromdate + "'  and start_eng_date<'" + todate + "'";

            OdbcCommand sw1 = new OdbcCommand();
            sw1.Parameters.AddWithValue("tblname", "t_settings ");
            sw1.Parameters.AddWithValue("attribute", "mal_year,mal_year_id");
            sw1.Parameters.AddWithValue("conditionv", "end_eng_date>='" + fromdate + "'  and start_eng_date<'" + todate + "'");


            OdbcDataReader ormalyear = objcls.SpGetReader("call selectcond(?,?,?)", sw1);
            int malyear1 = 0, malyear2 = 0, malyear3 = 0;
            if (ormalyear.Read())
            {
                malyear1 = Convert.ToInt32(ormalyear["mal_year"]);
                malyearid = Convert.ToInt32(ormalyear["mal_year_id"]);
                Session["malyear"] = malyear1;
                Session["malyyearid"] = malyearid;

            }
            malyear2 = malyear1 - 1;
            malyear3 = malyear1 - 2;
            string[] totdate = new string[1000];
            string[] totdate1 = new string[1000];
            string[] totdate2 = new string[1000];
            OdbcCommand cmdselectdate = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
            //cmdselectdate.CommandType = CommandType.StoredProcedure;
            cmdselectdate.Parameters.AddWithValue("tblname", " t_liabilityregister");
            cmdselectdate.Parameters.AddWithValue("attribute", "  distinct dayend");
            cmdselectdate.Parameters.AddWithValue("conditionv", "dayend>='" + fromdate + "' and dayend<='" + todate + "'  order by dayend asc");

            // OdbcDataAdapter da = new OdbcDataAdapter(cmdselectdate);
            DataTable dttdate = new DataTable();
            dttdate = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdate);

            // da.Fill(dttdate);
            int count = 0;
            if (dttdate.Rows.Count > 0)
            {

                for (int i = 0; i < dttdate.Rows.Count; i++)
                {
                    // DateTime dt5 = DateTime.Parse(dr["date"].ToString());
                    //string date1 = dt5.ToString("dd-MM-yyyy");


                    DateTime date5 = DateTime.Parse(dttdate.Rows[i]["dayend"].ToString());

                    string date1 = date5.ToString("dd/MM/yyyy");
                    string dater = date5.ToString("MM/dd/yyyy");
                    DateTime date3 = DateTime.Parse(dater);
                    date1 = objcls.yearmonthdate(date1);

                    int year11 = date3.Year;
                    int year22 = year11 - 1;
                    int year33 = year11 - 2;
                    string prevyear = date3.Day + "/" + date3.Month + "/" + year22;
                    string prevyear1 = date3.Day + "/" + date3.Month + "/" + year33;

                    prevyear = objcls.yearmonthdate(prevyear);
                    prevyear1 = objcls.yearmonthdate(prevyear1);
                    totdate[i] = date1;
                    totdate1[i] = prevyear;
                    totdate2[i] = prevyear1;
                    count++;

                }


            }


            string fromdate1 = totdate1[0];
            string fromdate2 = totdate2[0];
            //OdbcCommand cmdcreate = new OdbcCommand("create table consolidatedcollection (date date, total int(30), cumilative int(40),year1 int(30),year2 int(30))", conn);
            //cmdcreate.ExecuteNonQuery();

            //for (int i = 0; i < count; i++)
            //{
            //    OdbcCommand cmdinsert = new OdbcCommand("insert into consolidatedcollection(date)values('" + totdate[i] + "')", conn);
            //    cmdinsert.ExecuteNonQuery();
            //}
            DataTable dttotalamount = new DataTable();
            dttotalamount.Columns.Clear();
            dttotalamount.Columns.Add("date", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("total", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("cumilative", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("year1", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("cum1", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("year2", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("cum2", System.Type.GetType("System.String"));



            // int iRow = dtDisplay.Rows.Count;
            //dtDisplay2.Rows.Add();
            //dtDisplay2.Rows[0]["display_id"] = displayid;
            //dtDisplay2.Rows[0]["Slno"] = 1;
            //dtDisplay2.Rows[0]["displayname"] = reports;

            for (int i = 0; i < count; i++)
            {


                string datea = totdate[i];

                int count1 = 0;
                int count2 = 0;
                OdbcCommand cmdselectdata = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                //cmdselectdata.CommandType = CommandType.StoredProcedure;
                cmdselectdata.Parameters.AddWithValue("tblname", "t_liabilityregister");
                cmdselectdata.Parameters.AddWithValue("attribute", "sum(total) as total ");
                cmdselectdata.Parameters.AddWithValue("conditionv", " dayend='" + datea + "'");
                //   OdbcDataAdapter das = new OdbcDataAdapter(cmdselectdata);
                DataTable dttdate1 = new DataTable();
                //das.Fill(dttdate1);
                dttdate1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata);
                //online rent
                OdbcCommand cmdrent = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                //cmdselectdata.CommandType = CommandType.StoredProcedure;
                cmdrent.Parameters.AddWithValue("tblname", "t_roomallocation INNER JOIN t_roomreservation  ON t_roomallocation.reserve_id = t_roomreservation.reserve_id  INNER JOIN t_roomreservation_generaltdbtemp   ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no");
                cmdrent.Parameters.AddWithValue("attribute", "SUM(t_roomreservation_generaltdbtemp.room_rent) AS 'online rent' ");
                cmdrent.Parameters.AddWithValue("conditionv", " dayend = '" + datea + "'  AND t_roomreservation.reserve_no LIKE '9R%'");
                //   OdbcDataAdapter das = new OdbcDataAdapter(cmdselectdata);
                DataTable dtrent = new DataTable();
                //das.Fill(dttdate1);
                dtrent = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdrent);
                int onrent = 0;
                int all1 = 0;
                if (dtrent.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dtrent.Rows[0]["online rent"]) == false)
                    {

                        onrent = Convert.ToInt32(dtrent.Rows[0]["online rent"]);

                    }

                }

                int amount1 = 0, amountcum = 0, prevamount = 0, prevamount1 = 0, prevcum = 0, prevcum1 = 0;

                if (dttdate1.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate1.Rows[0]["total"]) == false)
                    {

                        amount1 = Convert.ToInt32(dttdate1.Rows[0]["total"]);

                    }

                }
                all1 = onrent + amount1;

                OdbcCommand cmdselectdata1 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                // cmdselectdata1.CommandType = CommandType.StoredProcedure;
                cmdselectdata1.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata1.Parameters.AddWithValue("attribute", "sum(total)as total1 ");
                cmdselectdata1.Parameters.AddWithValue("conditionv", " dayend>='" + fromdate + "' and dayend<='" + datea + "'");
                // OdbcDataAdapter das1 = new OdbcDataAdapter(cmdselectdata1);
                DataTable dttdate11 = new DataTable();
                dttdate11 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata1);
                // das1.Fill(dttdate11);

                if (dttdate11.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate11.Rows[0]["total1"]) == false)
                    {

                        amountcum = Convert.ToInt32(dttdate11.Rows[0]["total1"]);

                    }

                }

                OdbcCommand cmdrentcum = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                //cmdselectdata.CommandType = CommandType.StoredProcedure;
                cmdrentcum.Parameters.AddWithValue("tblname", "t_roomallocation INNER JOIN t_roomreservation  ON t_roomallocation.reserve_id = t_roomreservation.reserve_id  INNER JOIN t_roomreservation_generaltdbtemp   ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no");
                cmdrentcum.Parameters.AddWithValue("attribute", "SUM(t_roomreservation_generaltdbtemp.room_rent) AS 'online rent1' ");
                cmdrentcum.Parameters.AddWithValue("conditionv", " dayend>='" + fromdate + "' and dayend<='" + datea + "' AND t_roomreservation.reserve_no LIKE '9R%'");
                //   OdbcDataAdapter das = new OdbcDataAdapter(cmdselectdata);
                DataTable dtrentcum = new DataTable();
                //das.Fill(dttdate1);
                dtrentcum = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdrentcum);
                int onrentcum = 0;
                int cumall1 = 0;
                if (dtrentcum.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dtrentcum.Rows[0]["online rent1"]) == false)
                    {

                        onrentcum = Convert.ToInt32(dtrentcum.Rows[0]["online rent1"]);

                    }

                }
                cumall1 = amountcum + onrentcum;

                OdbcCommand cmdselectdata12 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                // cmdselectdata12.CommandType = CommandType.StoredProcedure;
                cmdselectdata12.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata12.Parameters.AddWithValue("attribute", "sum(total) as total ");
                cmdselectdata12.Parameters.AddWithValue("conditionv", " dayend='" + totdate1[i] + "'");
                // OdbcDataAdapter das12 = new OdbcDataAdapter(cmdselectdata12);
                DataTable dttdate112 = new DataTable();
                dttdate112 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata12);
                // das12.Fill(dttdate112);

                if (dttdate112.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate112.Rows[0]["total"]) == false)
                    {
                        string bb = totdate1[i];

                        prevamount = Convert.ToInt32(dttdate112.Rows[0]["total"]);

                    }
                }

                OdbcCommand cmdprevrentcum = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                //cmdselectdata.CommandType = CommandType.StoredProcedure;
                cmdprevrentcum.Parameters.AddWithValue("tblname", "t_roomallocation INNER JOIN t_roomreservation  ON t_roomallocation.reserve_id = t_roomreservation.reserve_id  INNER JOIN t_roomreservation_generaltdbtemp   ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no");
                cmdprevrentcum.Parameters.AddWithValue("attribute", "SUM(t_roomreservation_generaltdbtemp.room_rent) AS 'online rent1' ");
                cmdprevrentcum.Parameters.AddWithValue("conditionv", " dayend='" + totdate1[i] + "' AND t_roomreservation.reserve_no LIKE '9R%'");
                //   OdbcDataAdapter das = new OdbcDataAdapter(cmdselectdata);
                DataTable dtprevrentcum = new DataTable();
                //das.Fill(dttdate1);
                dtprevrentcum = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdprevrentcum);
                int onprevrentcum = 0;
                int prevrentcumall1 = 0;
                if (dtrentcum.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dtprevrentcum.Rows[0]["online rent1"]) == false)
                    {

                        onprevrentcum = Convert.ToInt32(dtprevrentcum.Rows[0]["online rent1"]);

                    }

                }
                prevrentcumall1 = prevamount + onprevrentcum;



                OdbcCommand cmdselectdata11 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                // cmdselectdata11.CommandType = CommandType.StoredProcedure;
                cmdselectdata11.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata11.Parameters.AddWithValue("attribute", "sum(total) as total11 ");
                cmdselectdata11.Parameters.AddWithValue("conditionv", " dayend>='" + fromdate1 + "' and dayend<='" + totdate1[i] + "'");
                // OdbcDataAdapter das11 = new OdbcDataAdapter(cmdselectdata11);
                DataTable dttdate111 = new DataTable();
                dttdate111 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata11);
                // das11.Fill(dttdate111);

                if (dttdate111.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate111.Rows[0]["total11"]) == false)
                    {

                        prevcum = Convert.ToInt32(dttdate111.Rows[0]["total11"]);

                    }
                }
                OdbcCommand cmdprevcum = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                //cmdselectdata.CommandType = CommandType.StoredProcedure;
                cmdprevcum.Parameters.AddWithValue("tblname", "t_roomallocation INNER JOIN t_roomreservation  ON t_roomallocation.reserve_id = t_roomreservation.reserve_id  INNER JOIN t_roomreservation_generaltdbtemp   ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no");
                cmdprevcum.Parameters.AddWithValue("attribute", "SUM(t_roomreservation_generaltdbtemp.room_rent) AS 'online rent11' ");
                cmdprevcum.Parameters.AddWithValue("conditionv", " dayend>='" + fromdate1 + "' and dayend<='" + totdate1[i] + "' AND t_roomreservation.reserve_no LIKE '9R%'");
                //   OdbcDataAdapter das = new OdbcDataAdapter(cmdselectdata);
                DataTable dtprevcum = new DataTable();
                //das.Fill(dttdate1);
                dtprevcum = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdprevcum);
                int onprevcum = 0;
                int prevcumall1 = 0;
                if (dtprevcum.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dtprevcum.Rows[0]["online rent11"]) == false)
                    {

                        onprevcum = Convert.ToInt32(dtprevcum.Rows[0]["online rent11"]);

                    }

                }
                prevcumall1 = prevcum + onprevcum;



                OdbcCommand cmdselectdata0 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                //cmdselectdata0.CommandType = CommandType.StoredProcedure;
                cmdselectdata0.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata0.Parameters.AddWithValue("attribute", "sum(total)  as total22");
                cmdselectdata0.Parameters.AddWithValue("conditionv", " dayend='" + totdate2[i] + "'");
                // OdbcDataAdapter das0 = new OdbcDataAdapter(cmdselectdata0);
                DataTable dttdate10 = new DataTable();
                dttdate10 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata0);
                //das0.Fill(dttdate10);

                if (dttdate10.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate10.Rows[0]["total22"]) == false)
                    {

                        prevamount1 = Convert.ToInt32(dttdate10.Rows[0]["total22"]);

                    }

                }
                string chk2 = totdate2[i].ToString();
                OdbcCommand cmdprevamount1 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                //cmdselectdata.CommandType = CommandType.StoredProcedure;
                cmdprevamount1.Parameters.AddWithValue("tblname", "t_roomallocation INNER JOIN t_roomreservation  ON t_roomallocation.reserve_id = t_roomreservation.reserve_id  INNER JOIN t_roomreservation_generaltdbtemp   ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no");
                cmdprevamount1.Parameters.AddWithValue("attribute", "SUM(t_roomreservation_generaltdbtemp.room_rent) AS 'online rent22' ");
                cmdprevamount1.Parameters.AddWithValue("conditionv", "dayend='" + totdate2[i] + "' AND t_roomreservation.reserve_no LIKE '9R%'");
                //   OdbcDataAdapter das = new OdbcDataAdapter(cmdselectdata);
                DataTable dtprevamount1 = new DataTable();
                //das.Fill(dttdate1);
                dtprevamount1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdprevamount1);
                int onprevamount1 = 0;
                int prevamtall1 = 0;
                if (dtprevamount1.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dtprevamount1.Rows[0]["online rent22"]) == false)
                    {

                        onprevamount1 = Convert.ToInt32(dtprevamount1.Rows[0]["online rent22"]);

                    }

                }
                prevamtall1 = prevamount1 + onprevamount1;



                string ff = totdate2[i];
                OdbcCommand cmdselectdata121 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                //  cmdselectdata121.CommandType = CommandType.StoredProcedure;
                cmdselectdata121.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata121.Parameters.AddWithValue("attribute", "sum(total) as total112 ");
                cmdselectdata121.Parameters.AddWithValue("conditionv", " dayend>='" + fromdate2 + "' and dayend<='" + totdate2[i] + "'");
                //OdbcDataAdapter das121 = new OdbcDataAdapter(cmdselectdata121);
                DataTable dttdate1121 = new DataTable();
                dttdate1121 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata121);
                //das121.Fill(dttdate1121);

                if (dttdate1121.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate1121.Rows[0]["total112"]) == false)
                    {

                        prevcum1 = Convert.ToInt32(dttdate1121.Rows[0]["total112"]);

                    }
                }

                string chkdt = totdate2[i].ToString();
                OdbcCommand cmdprevcum1 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
                //cmdselectdata.CommandType = CommandType.StoredProcedure;
                cmdprevcum1.Parameters.AddWithValue("tblname", "t_roomallocation INNER JOIN t_roomreservation  ON t_roomallocation.reserve_id = t_roomreservation.reserve_id  INNER JOIN t_roomreservation_generaltdbtemp   ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no");
                cmdprevcum1.Parameters.AddWithValue("attribute", "SUM(t_roomreservation_generaltdbtemp.room_rent) AS 'online rent122' ");
                cmdprevcum1.Parameters.AddWithValue("conditionv", "dayend>='" + fromdate2 + "' and dayend<='" + totdate2[i] + "' AND t_roomreservation.reserve_no LIKE '9R%'");
                //   OdbcDataAdapter das = new OdbcDataAdapter(cmdselectdata);
                DataTable dtprevcum1 = new DataTable();
                //das.Fill(dttdate1);
                dtprevcum1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdprevcum1);
                int onprevcum1 = 0;
                int prevcumall2 = 0;
                if (dtprevcum1.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dtprevcum1.Rows[0]["online rent122"]) == false)
                    {

                        onprevcum1 = Convert.ToInt32(dtprevcum1.Rows[0]["online rent122"]);

                    }

                }
                prevcumall2 = prevcum1 + onprevcum1;


                dttotalamount.Rows.Add();
                dttotalamount.Rows[i]["date"] = datea;
                if (all1 == 0)
                {
                    dttotalamount.Rows[i]["total"] = "";
                }
                else
                {

                    dttotalamount.Rows[i]["total"] = all1;
                }
                if (cumall1 == 0)
                {
                    dttotalamount.Rows[i]["cumilative"] = "";
                }
                else
                {
                    dttotalamount.Rows[i]["cumilative"] = cumall1;
                }
                if (prevrentcumall1 == 0)
                {
                    dttotalamount.Rows[i]["year1"] = "";
                }
                else
                {
                    dttotalamount.Rows[i]["year1"] = prevrentcumall1;
                }
                if (prevcumall1 == 0)
                {
                    dttotalamount.Rows[i]["cum1"] = "";
                }
                else
                {
                    dttotalamount.Rows[i]["cum1"] = prevcumall1;
                }
                if (prevamtall1 == 0)
                {
                    dttotalamount.Rows[i]["year2"] = "";

                }
                else
                {

                    dttotalamount.Rows[i]["year2"] = prevamtall1;
                }
                if (prevcumall2 == 0)
                {
                    dttotalamount.Rows[i]["cum2"] = "";
                }
                else
                {
                    dttotalamount.Rows[i]["cum2"] = prevcumall2;
                }



            }
            DateTime datedt = DateTime.Now;

            string dt1 = datedt.ToString("dd  MMMM  yyyy");

            string time1 = datedt.ToString(" hh :mm tt");

            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");


            string ch = "CollectionComparison" + transtim.ToString() + ".pdf";

            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 70);
            //string pdfFilePath = Server.MapPath(".") + "/pdf/consolidatedcollection.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9);
            Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            //PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            doc.Open();
            page.strRptMode = "Collection Comparison";
            PdfPTable table = new PdfPTable(8);
            float[] colW11 = { 10, 30, 20, 20, 20, 20, 20, 20 };
            table.SetWidths(colW11);


            PdfPCell cell = new PdfPCell(new Phrase("Consolidated   Collection comparison report Taken on   " + dt1 + " at " + time1, font12));
            cell.Colspan = 8;
            cell.HorizontalAlignment = 1;
            cell.Border = 1;
            table.AddCell(cell);
            PdfPCell cellc1 = new PdfPCell(new Phrase("No", font9));
            //cellc.Colspan = 3;
            cellc1.Rowspan = 2;
            cellc1.HorizontalAlignment = 1;
            table.AddCell(cellc1);


            PdfPCell cellc = new PdfPCell(new Phrase("Date", font9));
            //cellc.Colspan = 3;
            cellc.HorizontalAlignment = 1;
            cellc.Rowspan = 2;
            table.AddCell(cellc);

            PdfPCell cella = new PdfPCell(new Phrase(malyear1.ToString(), font9));
            cella.Colspan = 2;
            cella.HorizontalAlignment = 1;
            table.AddCell(cella);

            PdfPCell cellb = new PdfPCell(new Phrase(malyear2.ToString(), font9));
            cellb.Colspan = 2;
            cellb.HorizontalAlignment = 1;
            table.AddCell(cellb);
            PdfPCell cell11q = new PdfPCell(new Phrase(malyear3.ToString(), font9));
            cell11q.Colspan = 2;
            cell11q.HorizontalAlignment = 1;
            table.AddCell(cell11q);


            PdfPCell cellxvvv = new PdfPCell(new Phrase("Day's Coln", font8));
            //cellxvvv.Colspan = 1;
            cellxvvv.HorizontalAlignment = 1;
            table.AddCell(cellxvvv);

            PdfPCell cellx = new PdfPCell(new Phrase("Cum Coln", font8));
            cellx.Colspan = 1;
            cellx.HorizontalAlignment = 1;
            table.AddCell(cellx);

            PdfPCell cell1h = new PdfPCell(new Phrase("Day's Coln", font8));
            cell1h.Colspan = 1;
            cell1h.HorizontalAlignment = 1;
            table.AddCell(cell1h);


            PdfPCell cell11n = new PdfPCell(new Phrase("Cum Coln", font8));
            //cell11n.Colspan = 3;
            cell11n.HorizontalAlignment = 1;
            table.AddCell(cell11n);
            PdfPCell cell1h1 = new PdfPCell(new Phrase("Day's Coln", font8));
            cell1h1.Colspan = 1;
            cell1h1.HorizontalAlignment = 1;
            table.AddCell(cell1h1);


            PdfPCell cell11n1 = new PdfPCell(new Phrase("Cum Coln", font8));
            //cell11n.Colspan = 3;
            cell11n1.HorizontalAlignment = 1;
            table.AddCell(cell11n1);



            doc.Add(table);

            int slno = 0, ii = 0;
            foreach (DataRow dr in dttotalamount.Rows)
            {

                slno = slno + 1;
                if (ii > 40)
                {
                    ii = 0;
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(8);
                    float[] colW111 = { 10, 30, 20, 20, 20, 20, 20, 20 };
                    table1.SetWidths(colW111);

                    PdfPCell cell11v12 = new PdfPCell(new Phrase("No", font8));
                    //cell11v1.Colspan= 3;
                    cell11v12.Rowspan = 2;
                    cell11v12.HorizontalAlignment = 1;
                    table1.AddCell(cell11v12);


                    PdfPCell cell11v1 = new PdfPCell(new Phrase("Date", font8));
                    //cell11v1.Colspan= 3;
                    cell11v1.Rowspan = 2;
                    cell11v1.HorizontalAlignment = 1;
                    table1.AddCell(cell11v1);

                    PdfPCell cell11v = new PdfPCell(new Phrase(malyear1.ToString(), font8));
                    cell11v.Colspan = 2;
                    cell11v.HorizontalAlignment = 1;
                    table1.AddCell(cell11v);


                    PdfPCell cell112v = new PdfPCell(new Phrase(malyear2.ToString(), font8));
                    cell112v.Colspan = 2;
                    cell112v.HorizontalAlignment = 1;
                    table1.AddCell(cell112v);

                    PdfPCell cell11qv = new PdfPCell(new Phrase(malyear3.ToString(), font8));
                    cell11qv.Colspan = 2;
                    cell11qv.HorizontalAlignment = 1;
                    table1.AddCell(cell11qv);



                    PdfPCell cellxv = new PdfPCell(new Phrase("Total Coln", font8));
                    cellxv.Colspan = 1;
                    cellxv.HorizontalAlignment = 1;
                    table1.AddCell(cellxv);

                    PdfPCell cellk = new PdfPCell(new Phrase(" Total Cum Coln", font8));
                    cellk.Colspan = 1;
                    cellk.HorizontalAlignment = 1;
                    table1.AddCell(cellk);

                    PdfPCell cell1hv = new PdfPCell(new Phrase("Total Coln", font8));
                    cell1hv.Colspan = 1;
                    cell1hv.HorizontalAlignment = 1;
                    table1.AddCell(cell1hv);

                    PdfPCell cell11ny = new PdfPCell(new Phrase("Total Cum Coln", font8));
                    //cel1l1n.Colspan = 3;
                    cell11ny.HorizontalAlignment = 1;
                    table1.AddCell(cell11ny);

                    PdfPCell cell1hvb = new PdfPCell(new Phrase("Total Coln", font8));
                    cell1hvb.Colspan = 1;
                    cell1hvb.HorizontalAlignment = 1;
                    table1.AddCell(cell1hvb);

                    PdfPCell cell11nyb = new PdfPCell(new Phrase("Total Cum Coln", font8));
                    //cel1l1n.Colspan = 3;
                    cell11nyb.HorizontalAlignment = 1;
                    table1.AddCell(cell11nyb);



                    doc.Add(table1);


                }


                ii++;

                PdfPTable table2 = new PdfPTable(8);
                float[] colW1111 = { 10, 30, 20, 20, 20, 20, 20, 20 };
                table2.SetWidths(colW1111);
                PdfPCell cell11v2d = new PdfPCell(new Phrase(slno.ToString(), font7));
                //cell11v2.Colspan = 3;
                cell11v2d.HorizontalAlignment = 1;
                table2.AddCell(cell11v2d);



                DateTime dtd = DateTime.Parse(dr["date"].ToString());
                string datert = dtd.ToString("dd MMMM");

                PdfPCell cell11v2 = new PdfPCell(new Phrase(datert.ToString(), font7));
                //cell11v2.Colspan = 3;
                cell11v2.HorizontalAlignment = 0;
                table2.AddCell(cell11v2);

                PdfPCell cell112v22 = new PdfPCell(new Phrase(dr["total"].ToString(), font7));
                cell112v22.Colspan = 1;
                cell112v22.HorizontalAlignment = 1;
                table2.AddCell(cell112v22);

                PdfPCell cellxv2 = new PdfPCell(new Phrase(dr["cumilative"].ToString(), font7));
                cellxv2.Colspan = 1;
                cellxv2.HorizontalAlignment = 1;
                table2.AddCell(cellxv2);



                PdfPCell cell11qv2 = new PdfPCell(new Phrase(dr["year1"].ToString(), font7));
                //cel11q.Colspan = 1;
                cell11qv2.HorizontalAlignment = 1;
                table2.AddCell(cell11qv2);

                PdfPCell cell11qv22 = new PdfPCell(new Phrase(dr["cum1"].ToString(), font7));
                //cel11q.Colspan = 1;
                cell11qv22.HorizontalAlignment = 1;
                table2.AddCell(cell11qv22);




                PdfPCell cell11v21 = new PdfPCell(new Phrase(dr["year2"].ToString(), font7));
                //cel112.Colspanv = 3;
                cell11v21.HorizontalAlignment = 1;
                table2.AddCell(cell11v21);

                PdfPCell cell11v211 = new PdfPCell(new Phrase(dr["cum2"].ToString(), font7));
                //cel112.Colspanv = 3;
                cell11v211.HorizontalAlignment = 1;
                table2.AddCell(cell11v211);

                doc.Add(table2);


            }

            PdfPTable table4 = new PdfPTable(8);

            PdfPCell cellff = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
            cellff.HorizontalAlignment = Element.ALIGN_LEFT;
            cellff.PaddingLeft = 30;
            cellff.Colspan = 8;

            cellff.MinimumHeight = 30;
            cellff.Border = 0;
            table4.AddCell(cellff);

            PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
            cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
            cellf1.PaddingLeft = 30;
            cellf1.Colspan = 8;
            cellf1.Border = 0;
            table4.AddCell(cellf1);

            PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom  ", font8)));
            cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
            cellh2.PaddingLeft = 30;
            cellh2.Border = 0;
            cellh2.Colspan = 8;
            table4.AddCell(cellh2);

            doc.Add(table4);

            doc.Close();
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            //Response.ContentType = "Application/pdf";
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + report + ".pdf");
            //Response.TransmitFile(pdfFilePath);
            //Response.Flush();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=COllectionComparison";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);



        }
        else
        {

            lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Enter the From date or To date";
            ViewState["action"] = "warn12";
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnOk);


        }
    }
}



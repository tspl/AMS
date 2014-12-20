using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using clsDAL;
using GenCode128;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Num2Wrd;
public partial class uncliamedremittance : System.Web.UI.Page
{

    commonClass objcls = new commonClass();
    clsCommon obc = new clsCommon();
    OdbcConnection con = new OdbcConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            clr();
       
        }

    }
    private void Load()
    {
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
            txtfromdate.Text = dtS.Rows[0]["startdate"].ToString();
            txttodate.Text = dtS.Rows[0]["enddate"].ToString();


            DataTable dt_seas = objcls.DtTbl("SELECT season_sub_id,seasonname FROM  m_sub_season WHERE season_sub_id = '" + dtS.Rows[0]["season_sub_id"].ToString() + "'");
            if (dt_seas.Rows.Count > 0)
            {
                ddlseason.DataSource = dt_seas;
                ddlseason.DataBind();
            }

            string unclaimed = @"SELECT IFNULL(SUM(amount),0)  -IFNULL((SELECT  IFNULL(SUM(amount),0) AS 'Deposit'  FROM  t_unclaimedremittance,m_season  WHERE 
  t_unclaimedremittance.DATE BETWEEN
m_season.startdate AND m_season.enddate AND m_season.season_id='" + Session["seasonid"] + "'  GROUP BY m_season.season_id),0) AS 'Unclaimed'  FROM  t_daily_transaction,m_season  WHERE t_daily_transaction.ledger_id = '2' AND t_daily_transaction.DATE BETWEEN m_season.startdate AND m_season.enddate AND m_season.season_id='" + Session["seasonid"] + "'  GROUP BY m_season.season_id";
            DataTable dt_unclaimed = objcls.DtTbl(unclaimed);
            if (dt_unclaimed.Rows.Count > 0)
            {
                txtunclaimed.Text = dt_unclaimed.Rows[0][0].ToString();
            }
            else
            {
                txtunclaimed.Text = "0";

            }


        }
        else
        {
            obc.ShowAlertMessage(this, "Season not found");
        }
        DataTable dt_nw = objcls.DtTbl("select date_format(now(),'%d/%m/%Y') as 'dt',date_format(now(),'%l:%i:%s %p') as 'time'");
        txtdate.Text = dt_nw.Rows[0][0].ToString();
    }



    protected void btnremit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtremitamount.Text != "" && txtdate.Text != "")
            {
                string insert = @"INSERT INTO `armsapr9`.`t_unclaimedremittance`
            (
             `Date`,
             `season_id`,
             `amount`)
VALUES (
        '" + objcls.yearmonthdate(txtdate.Text) + "','" + Session["seasonid"].ToString() + "',  '" + txtremitamount.Text + "');";
                objcls.exeNonQuery(insert);
                txtremitamount.Text = "";
                clr();
                obc.ShowAlertMessage(this,"Saved successfully");

            }
        }
        catch
        {

        }
    }
    protected void txtremitamount_TextChanged(object sender, EventArgs e)
    {
        if (txtunclaimed.Text != "" && txtremitamount.Text != "")
        {
            if (Convert.ToDouble(txtremitamount.Text) > Convert.ToDouble(txtunclaimed.Text))
            {
                txtremitamount.Text = "";
                SetFocus(txtremitamount);
                obc.ShowAlertMessage(this,"Entered amount grater then available unclaimed deposit");
            }
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        
        string fromdt = objcls.yearmonthdate(txtfromdate.Text.ToString());
        string todt = objcls.yearmonthdate(txttodate.Text.ToString());

        double balance = 0;

        string report = "Ledger ";//-"+txtfromdate.Text+" to "+txttodate.Text+"";
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 10, 10);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + report + ".pdf";
        Font font6 = FontFactory.GetFont("Arial", 6, 0);
        Font font8 = FontFactory.GetFont("Arial", 8, 0);
        Font font80 = FontFactory.GetFont("Arial", 8, 1);
        Font font9 = FontFactory.GetFont("Times New Roman", 8, 0);
        Font font90 = FontFactory.GetFont("Times New Roman", 8, 1);
        Font font10 = FontFactory.GetFont("Times New Roman", 10, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 7);
        Font font12 = FontFactory.GetFont("Times New Roman", 11, 1);
        Font font13 = FontFactory.GetFont("Times New Roman", 11);
        Font font14 = FontFactory.GetFont("Times New Roman", 14, 1);
        Font font15 = FontFactory.GetFont("Times New Roman", 16, 1);

        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        doc.Open();
        //PdfPTable headerTbl = new PdfPTable(1);
        //iTextSharp.text.Image head = iTextSharp.text.Image.GetInstance(Server.MapPath(".") + "/Images/Buttons/header.JPG");
        //head.ScaleToFit(500, 400);

     

        //PdfPCell cell02 = new PdfPCell(head);
        //cell02.Border = 0;
        //cell02.HorizontalAlignment = 0;
        //headerTbl.AddCell(cell02);
        //doc.Add(headerTbl);


        PdfPTable tabletitlep = new PdfPTable(1);
        float[] colWidthsuiop = { 100 };
        tabletitlep.SetWidths(colWidthsuiop);
        tabletitlep.TotalWidth = 400f;

        PdfPCell cell1ta = new PdfPCell(new Phrase("SWAMI SARANAM", font10));
        cell1ta.Border = 0;
        cell1ta.HorizontalAlignment = 1;
        tabletitlep.AddCell(cell1ta);
        PdfPCell cell1tb = new PdfPCell(new Phrase("TRAVANCORE DEVASWOM BOARD", font15));
        cell1tb.Border = 0;
        cell1tb.HorizontalAlignment = 1;
        tabletitlep.AddCell(cell1tb);

        doc.Add(tabletitlep);

        PdfPTable tabletitle = new PdfPTable(1);
        float[] colWidthsuio = { 100 };
        tabletitle.SetWidths(colWidthsuio);
        tabletitle.TotalWidth = 400f;

        PdfPCell cell1t = new PdfPCell(new Phrase("UNCLAIMED LEDGER ", font14));
        cell1t.Border = 0;
        cell1t.HorizontalAlignment = 1;
        tabletitle.AddCell(cell1t);

        PdfPCell cell1tp1 = new PdfPCell(new Phrase(" ", font14));
        cell1tp1.Border = 0;
        cell1tp1.HorizontalAlignment = 0;
        tabletitle.AddCell(cell1tp1);

        doc.Add(tabletitle);

        //PdfPTable tabletitlep1 = new PdfPTable(2);
        //float[] colWidthsuio1 = { 50, 50 };
        //tabletitlep1.SetWidths(colWidthsuio1);
        //tabletitlep1.TotalWidth = 400f;

        //PdfPCell cell1tp11 = new PdfPCell(new Phrase("Employee name: " + txtname.Text, font14));
        //cell1tp11.Border = 0;
        //cell1tp11.HorizontalAlignment = 0;
        //tabletitlep1.AddCell(cell1tp11);

        //PdfPCell cell1tp112 = new PdfPCell(new Phrase("From " + txtfrom.Text + " to " + txtto.Text, font14));
        //cell1tp112.Border = 0;
        //cell1tp112.HorizontalAlignment = 2;
        //tabletitlep1.AddCell(cell1tp112);

        //doc.Add(tabletitlep1);

        PdfPTable table32 = new PdfPTable(5);
        //float[] colWidths72 = { 10, 20, 45, 25, 25, 10, 20, 45, 25, 25 };
        float[] colWidths72 = { 10, 25, 25, 25, 25 };

        table32.SetWidths(colWidths72);
        table32.TotalWidth = 400f;

       

        PdfPCell cell1x2 = new PdfPCell(new Phrase("No.", font12));
        //cell1x.Border = 1;
        cell1x2.HorizontalAlignment = 1;
        table32.AddCell(cell1x2);

        PdfPCell cell1xm2 = new PdfPCell(new Phrase("Date", font12));
        // cell1xm.Border = 1;
        cell1xm2.HorizontalAlignment = 1;
        table32.AddCell(cell1xm2);

        PdfPCell cellfrfrf = new PdfPCell(new Phrase("Unclaimed deposit", font12));
        // cell1xm.Border = 1;
        cellfrfrf.HorizontalAlignment = 1;
        table32.AddCell(cellfrfrf);

        PdfPCell celllmlm1 = new PdfPCell(new Phrase("Remittance", font12));
        // cell1xm.Border = 1;
        celllmlm1.HorizontalAlignment = 1;
        table32.AddCell(celllmlm1);

        PdfPCell cell102 = new PdfPCell(new Phrase("Balance", font12));
        // cell10.Border = 1;
        cell102.HorizontalAlignment = 1;
        table32.AddCell(cell102);


        //PdfPCell cell1x21 = new PdfPCell(new Phrase("", font12));
        ////cell1x.Border = 1;
        //cell1x21.HorizontalAlignment = 1;
        //table32.AddCell(cell1x21);

        //PdfPCell cell1xmvv2 = new PdfPCell(new Phrase("", font12));
        //// cell1xm.Border = 1;
        //cell1xmvv2.HorizontalAlignment = 1;
        //table32.AddCell(cell1xmvv2);

        //PdfPCell cellfrfmmrf = new PdfPCell(new Phrase("", font12));
        //// cell1xm.Border = 1;
        //cellfrfmmrf.HorizontalAlignment = 1;
        //table32.AddCell(cellfrfmmrf);

        //PdfPCell cellnmlmlm1 = new PdfPCell(new Phrase("", font12));
        //// cell1xm.Border = 1;
        //cellnmlmlm1.HorizontalAlignment = 1;
        //table32.AddCell(cellnmlmlm1);

        //PdfPCell cell102125 = new PdfPCell(new Phrase(balance.ToString(), font12));
        //// cell10.Border = 1;
        //cell102125.HorizontalAlignment = 1;
        //table32.AddCell(cell102125);



        doc.Add(table32);


        PdfPTable tablesub = new PdfPTable(5);
        float[] colWidthsub = { 10, 25, 25, 25, 25 };
        tablesub.SetWidths(colWidthsub);
        tablesub.TotalWidth = 400f;
        int i = 0;

      
        int count = 0;
        string[] fromdate = (txtfromdate.Text).Split('/');
        string[] todate = (txttodate.Text).Split('/');
        string fdate = fromdate[2] + "-" + fromdate[1] + "-" + fromdate[0];
        string tdate = todate[2] + "-" + todate[1] + "-" + todate[0];


        string stcvb = @"SELECT DATE_FORMAT(CAST(selected_date AS CHAR(12)),'%d/%m/%Y' ) AS 'date' FROM 
        (SELECT ADDDATE('1970-01-01',t4.i*10000 + t3.i*1000 + t2.i*100 + t1.i*10 + t0.i) selected_date FROM
         (SELECT 0 i UNION SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9) t0,
         (SELECT 0 i UNION SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9) t1,
         (SELECT 0 i UNION SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9) t2,
         (SELECT 0 i UNION SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9) t3,
         (SELECT 0 i UNION SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9) t4) v
        WHERE selected_date BETWEEN '" + fdate + "'AND '" + tdate + "' ";
        DataTable dt_date = objcls.DtTbl(stcvb);

        double remitamt = 0, unclaimedamt = 0; 
        for (int x = 0; x < dt_date.Rows.Count; x++)
        {
            remitamt = 0;
            unclaimedamt = 0; 

           count++;


           string remit = @"SELECT IFNULL(SUM(amount),0) AS 'Deposit'  FROM  t_unclaimedremittance  WHERE 
  t_unclaimedremittance.DATE = '"+objcls.yearmonthdate( dt_date.Rows[x][0].ToString())+"'  GROUP BY t_unclaimedremittance.Date";

           DataTable dt_remit = objcls.DtTbl(remit);
           if (dt_remit.Rows.Count > 0)
           {
               remitamt = Convert.ToDouble( dt_remit.Rows[0][0].ToString());

           }
           else
           {
               remitamt = 0;
           }

           string unclaimed = @"SELECT IFNULL(SUM(amount),0) 'Unclaimed'  FROM  t_daily_transaction  WHERE t_daily_transaction.ledger_id = '2'
 AND t_daily_transaction.DATE ='" + objcls.yearmonthdate( dt_date.Rows[x][0].ToString() )+ "'   GROUP BY t_daily_transaction.DATE";


           DataTable dt_unclaimed = objcls.DtTbl(unclaimed);
           if (dt_unclaimed.Rows.Count > 0)
           {
               unclaimedamt = Convert.ToDouble(dt_unclaimed.Rows[0][0].ToString());

           }
           else
           {
               unclaimedamt = 0;
           }
           balance = (balance + unclaimedamt) - remitamt;

       

            PdfPCell cell1a11 = new PdfPCell(new Phrase(count.ToString(), font13));
            cell1a11.HorizontalAlignment = 1;
            // cell1a.Border = 2;
            tablesub.AddCell(cell1a11);

            PdfPCell cell1a1cc1 = new PdfPCell(new Phrase( dt_date.Rows[x][0].ToString(), font13));
            cell1a1cc1.HorizontalAlignment = 1;
            // cell1a.Border = 2;
            tablesub.AddCell(cell1a1cc1);

            PdfPCell cell1a1bb1 = new PdfPCell(new Phrase(unclaimedamt.ToString(), font13));
            cell1a1bb1.HorizontalAlignment = 1;
            // cell1a.Border = 2;
            tablesub.AddCell(cell1a1bb1);


            PdfPCell cell1arrb1 = new PdfPCell(new Phrase(remitamt.ToString(), font13));
            cell1arrb1.HorizontalAlignment = 1;
            // cell1a.Border = 2;
            tablesub.AddCell(cell1arrb1);

            PdfPCell cell1ahhb1 = new PdfPCell(new Phrase(balance.ToString(), font13));
            cell1ahhb1.HorizontalAlignment = 1;
            // cell1a.Border = 2;
            tablesub.AddCell(cell1ahhb1);
        
          
            
        }
        //doc.Add(tablesub);

        doc.Add(tablesub);
        doc.Close();
        Response.ContentType = "Application/pdf";
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + report + ".pdf");
        Response.TransmitFile(pdfFilePath);
        Response.Flush();
    
    }
    protected void btnunclaimed_Click(object sender, EventArgs e)
    {
        pnlledger.Visible = true;
      
    }
    protected void btnremit0_Click(object sender, EventArgs e)
    {
        clr();
    }
    private void clr()
    {
        txtdate.Text = "";
       
        pnlledger.Visible = false;
        Load();
    }
}
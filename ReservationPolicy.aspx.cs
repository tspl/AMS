/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Accomodation
// Screen Name      :      Tsunami ARMS - Reservation Policy
// Form Name        :      ReservationPolicy.aspx
// ClassFile Name   :      Season_Master
// Purpose          :      Policy setting for reservation

// Created by       :      Sajith
// Created On       :      31-July-2010
// Last Modified    :      2-August-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       2/09/2010  sajith        coding changes as per database
		
//-------------------------------------------------------------------


using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF;

public partial class ReservationPolicy : System.Web.UI.Page
{
    //giving conection string

    static string strConnection;
    OdbcConnection con = new OdbcConnection();

    int temp, temp1, temp2, temp3, k, policyid, boolextra, sesid, userid, temp4;
    String g, d, m, y, from, to, policytype, date;
    bool isrent,isdeposit,isother;
    DateTime curDate;
    DataSet ds = new DataSet();

    # region ON PAGE LOAD 
    protected void Page_Load(object sender, EventArgs e)
    {

        Session.Timeout = 60;
        if (!Page.IsPostBack)
        {
            Session["userid"] = 1;

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";


            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();

            check();
           
            txtpolicyid.Text = primarykey("sno","reservationpolicy");             
            //postpone
            txtpostnoofdys.Visible = false;
            txtpostno.Visible = false;
            txtpostamt.Visible = false;
            lblpostno.Visible = false;
            lblpostnoofdays.Visible = false;
            lblRCpostAmt.Visible=false;
            revpostamt.Visible = false;
            revpostno.Visible = false;
            revpostnoofdays.Visible = false;

            //prepone
            txtprenoofdys.Visible = false;
            txtpreno.Visible = false;
            txtpreamt.Visible = false;
            lblpreamt.Visible = false;
            lblpreno.Visible = false;
            lblprenoofdays.Visible = false;
            revpreamt.Visible = false;
            revpreno.Visible = false;
            revprenoofdays.Visible = false;

            //cancel
            txtcanclno.Visible = false;
            txtcanclamt.Visible = false;
            lblcancelcharge.Visible = false;
            lblcancelno.Visible = false;
            revcancelamt.Visible = false;
            revcancelno.Visible = false;
          
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
                
                string strSql="SELECT seasonname "
                       +" FROM "                                                                   
                                    +"m_sub_season "
                       +" WHERE "                                   
                                    +"rowstatus<>" + 2 + "";

                OdbcCommand cmdseason = new OdbcCommand(strSql, con);
                OdbcDataReader or = cmdseason.ExecuteReader();
                while (or.Read())
                {
                    lstseason.Items.Add(or[0].ToString());
                }
                or.Close();
            }
            catch
            { }
            finally
            {
                con.Close();
            }

            this.ScriptManager1.SetFocus(cmbtype);
            grid_load();
            pnlreport.Visible = false;
        }

    }
        #   endregion

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
            if (obj.CheckUserRight("ReservationPolicy", level) == 0)
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

    # region Primary key
    public string primarykey(string s1, string s2)
    {
        try
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
            }
            catch
            {
            }
            //fetching primary key of Reservation policy table
            OdbcCommand cmdpk = new OdbcCommand("select max(" + s1 + ") from " + s2 + "", con);

            try
            {
                policyid = Convert.ToInt32(cmdpk.ExecuteScalar());
                policyid = policyid + 1;
            }
            catch
            {
                policyid = 1;
            }
            
        }
        catch
        { }
        finally
        {
            con.Close();
        }

        return policyid.ToString();
    }
    # endregion

    # region CLEAR ALL FIELDS Reset all fields
    protected void clear()
    {
        //clearing all combo box        
        cmbtype.SelectedIndex = -1;
        cmbprepon.SelectedIndex = -1;
        cmbpostpon.SelectedIndex = -1;
        cmbcanc.SelectedIndex = -1;
        

        //clearing all textbox fields
        txtcanclamt.Text = "0";
        txtcanclno.Text = "0";
        txtfrmdate.Text = "";
        txtmaxdays.Text = "";
        txtmaxstay.Text = "";
        txtmindays.Text = "";
        txtpostamt.Text = "0";
        txtpostno.Text = "0";
        txtpostnoofdys.Text = "0";
        txtpreamt.Text = "0";
        txtpreno.Text = "0";
        txtprenoofdys.Text = "0";
        txtRCamount.Text = "0";
        txttodate.Text = "";
        //clearing lst box selection
        lstseason.SelectedIndex = -1;

        //disabling fields
        txtRCamount.Enabled = true;
        //postpone
        txtpostnoofdys.Visible = false;
        txtpostno.Visible = false;
        txtpostamt.Visible = false;
        lblpostno.Visible = false;
        lblpostnoofdays.Visible = false;
        lblRCpostAmt.Visible = false;
        revpostamt.Visible = false;
        revpostno.Visible = false;
        revpostnoofdays.Visible = false;

        //prepone
        txtprenoofdys.Visible = false;
        txtpreno.Visible = false;
        txtpreamt.Visible = false;
        lblpreamt.Visible = false;
        lblpreno.Visible = false;
        lblprenoofdays.Visible = false;
        revpreamt.Visible = false;
        revpreno.Visible = false;
        revprenoofdays.Visible = false;

        //cancel
        txtcanclno.Visible = false;
        txtcanclamt.Visible = false;
        lblcancelcharge.Visible = false;
        lblcancelno.Visible = false;
        revcancelamt.Visible = false;
        revcancelno.Visible = false;
       
        btndelete.Enabled = false;

        this.ScriptManager1.SetFocus(cmbtype);
    }
        # endregion

    # region CONVERTING DD/MM/YYYY TO YYYY-MM-DD yearmonthdate(string s)  
   public string yearmonthdate(string s)
   {
       try
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
       catch
       {
           MessageBox.Show("Error on converting date format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
           g = "";
           return g;
       }
  }

#endregion

    # region Convert string to Boolean

  public bool converttoboolean(String s)
    {
        
        if (s=="YES")
        {
            return true;
        }

        else if (s == "1")
        {
            return true;
        }
        else
        {
            return false;
        }

       
    }
  # endregion

    # region Boolean to string
    public string yesorno(int a)
    {
        string s;
        if (a > 0)
            s = "YES";
        else
            s = "NO";
        return s;
    }

    public string combo(int a)
    {
        string s;
        if (a > 0)
            s = "1";
        else
            s = "0";

        return s;
    }
  # endregion
   
    # region GRID LOADING with policy details
    public void grid_load()
    {
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }

            // Loading Grid with Policy  Details
            string strSql1 = "SELECT res_policy_id as Serialno,"
                                   + "res_type as TYPE,"
                                   + "amount_res as AMOUNT,"
                                   + "day_res_max as 'Max days',"
                                   + "day_res_min as 'Min days',"
                                   + "day_res_maxstay as 'Max stay',"
                                   + "DATE_FORMAT(res_from,'%d-%m-%Y') as 'Res From',"
                                   + "DATE_FORMAT(res_to,'%d-%m-%Y') as 'Res To'"
                       + " FROM "
                                   + "t_policy_reservation"
                       + " WHERE "
                                   + "rowstatus <>" + 2 + "";

            OdbcDataAdapter da = new OdbcDataAdapter(strSql1, con);
            DataTable dtt = new DataTable();
            da.Fill(dtt);
            gdrespolicy.DataSource = dtt;
            gdrespolicy.DataBind();
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
       
    # region CLEAR BUTTON CLICK
    protected void btnclr_Click(object sender, EventArgs e)
    {
        clear();
        txtpolicyid.Text = primarykey("res_policy_id", "t_policy_reservation");
       
    }
    # endregion

    # region SAVE BUTTON CLICK
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "Do you want Save?";
        ViewState["action"] = "save";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);         
    }
    # endregion

    # region EDIT button click
    protected void btnedit_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "Do you want Edit?";
        ViewState["action"] = "edit";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);       
    }
    # endregion  

    # region DELETE Button Click
    protected void btndelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "Do you want Delete?";
        ViewState["action"] = "delete";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);     
    }
    # endregion
   
    # region  Textchange functions change mindays, max days, max stay

    protected void txtmaxdays_TextChanged(object sender, EventArgs e)
    {
        if (txtmaxdays.Text == "")
        {
            this.ScriptManager1.SetFocus(txtmaxdays);
            
        }
        else
        {
            this.ScriptManager1.SetFocus(txtmindays);
            
        }
    }

    protected void txtmindays_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtmaxstay);
    }
    protected void txtmaxstay_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbpostpon);
    }

    # endregion
    
    # region button report 
    protected void btnreport_Click(object sender, EventArgs e)
    {
        gdrespolicy.Visible = false;
        pnlreport.Visible = true;     
    }
    # endregion

    # region button hide
    protected void btnhide_Click(object sender, EventArgs e)
    {
        gdrespolicy.Visible = true;
        pnlreport.Visible = false;
       
    }
    # endregion

    # region button clear report 
    protected void btnreportclear_Click(object sender, EventArgs e)
    {
        lblmessage.Visible = false;
        cmbRep.SelectedIndex = -1;
        txtreportfrom.Text = "";
        txtreportto.Text = "";
    }
    # endregion

    # region policy history report button 
    protected void btnpolicyhis_Click(object sender, EventArgs e)
    {
        string str1, str2;
        int flag = 0;
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            lblmessage.Visible = false;

            if (cmbRep.SelectedIndex == -1)
            {
                lblmessage.Visible = true;
                return;
            }

            if ((txtreportfrom.Text != "" && txtreportto.Text == "") || (txtreportfrom.Text == "" && txtreportto.Text != ""))
            {
                lblmessage.Visible = true;
                return;

            }


            # region fetching the data needed to show as report from database and assigning to a datatable

            OdbcCommand cmd31 = new OdbcCommand("CALL selectcond(?,?,?)", con);
            cmd31.CommandType = CommandType.StoredProcedure;
            cmd31.Parameters.AddWithValue("tblname", "t_policy_reservation_log");
            cmd31.Parameters.AddWithValue("attribute", "res_policy_id,res_type,amount_res,res_from,res_to,day_res_max,day_res_min,day_res_maxstay,createdon,createdby");

            if (cmbRep.SelectedValue == "All")
                flag = 1;
            if (txtreportfrom.Text != "" && txtreportto.Text != "")
            {
                str1 = yearmonthdate(txtreportfrom.Text);
                str2 = yearmonthdate(txtreportto.Text);
                if(flag==0)
                    cmd31.Parameters.AddWithValue("conditionv", "res_type='" + cmbRep.SelectedValue.ToString() + "' and (createdon between '" + str1.ToString() + "' and '" + str2.ToString() + "') order by res_policy_id");
                else
                    cmd31.Parameters.AddWithValue("conditionv", "updateddate between '" + str1.ToString() + "' and '" + str2.ToString() + "' order by res_policy_id");
                

            }
            else if (txtreportfrom.Text == "" && txtreportto.Text == "")
            {
                if (flag == 0)
                    cmd31.Parameters.AddWithValue("conditionv", "res_type='" + cmbRep.SelectedValue.ToString() + "' order by res_policy_id");
                else
                    cmd31.Parameters.AddWithValue("conditionv", "res_type='DONOR FREE' or res_type= 'DONOR PAID' or res_type='TDB' order by res_policy_id");
              
            }

            OdbcDataAdapter da = new OdbcDataAdapter(cmd31);
            DataTable dt = new DataTable();
            da.Fill(dt);
           
            # endregion

                  
            // creating a  file to save the report .... setting its font
            Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 80, 80);
            string pdfFilePath = Server.MapPath(".") + "/pdf/currentpolicy.pdf";
            Font font8 = FontFactory.GetFont("Arial", 8);
            Font font9 = FontFactory.GetFont("Arial", 9);
            Font font10 = FontFactory.GetFont("Arial", 10);
            pdfPage page = new pdfPage();           
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            PdfPTable table1 = new PdfPTable(9);
            float[] colwidth1 ={ 5, 8, 10, 8, 8, 8, 10,15,10 };
            table1.SetWidths(colwidth1);
            PdfPCell cell1001 = new PdfPCell(new Phrase(new Chunk(cmbRep.SelectedValue.ToString() + " POLICY HISTORY REPORT", font10)));
            cell1001.Colspan = 9;
            cell1001.Border = 0;
            cell1001.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell1001);
            # region giving heading for each coloumn in report
            PdfPCell cell100 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table1.AddCell(cell100);

            PdfPCell cell300 = new PdfPCell(new Phrase(new Chunk("AMOUNT", font9)));
            table1.AddCell(cell300);

            PdfPCell cell400 = new PdfPCell(new Phrase(new Chunk("POLICY FROM", font9)));
            table1.AddCell(cell400);

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("POLICY TO", font9)));
            table1.AddCell(cell500);

            PdfPCell cell600 = new PdfPCell(new Phrase(new Chunk("MAX DAYS TO RESERVE", font9)));
            table1.AddCell(cell600);

            PdfPCell cell700 = new PdfPCell(new Phrase(new Chunk("MIN DAYS TO RESERVE", font9)));
            table1.AddCell(cell700);

            PdfPCell cell800 = new PdfPCell(new Phrase(new Chunk("DAYS OF STAY", font9)));
            table1.AddCell(cell800);

            PdfPCell cell900 = new PdfPCell(new Phrase(new Chunk("UPDATED ON", font9)));
            table1.AddCell(cell900);

            PdfPCell cell010 = new PdfPCell(new Phrase(new Chunk("STATUS", font9)));
            table1.AddCell(cell010);

            # endregion
            doc.Add(table1);

            # region adding data to the report file
            int slno = 0;            
            int i = 0, j = 0;            
            foreach (DataRow dr in dt.Rows)
            {
                PdfPTable table = new PdfPTable(9);
                float[] colwidth ={ 5, 8, 10, 8, 8, 8, 10, 15, 10 };
                table.SetWidths(colwidth);
                if (i + j > 45)// total rows on page
                {                   
                    doc.NewPage();

                    # region giving heading for each coloumn in report
                    PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                    table.AddCell(cell1);

                    PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("AMOUNT", font9)));
                    table.AddCell(cell3);

                    PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("POLICY FROM", font9)));
                    table.AddCell(cell4);

                    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("POLICY TO", font9)));
                    table.AddCell(cell5);

                    PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("MAX DAYS TO RESERVE", font9)));
                    table.AddCell(cell6);

                    PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("MIN DAYS TO RESERVE", font9)));
                    table.AddCell(cell7);

                    PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("DAYS OF STAY", font9)));
                    table.AddCell(cell8);

                    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("UPDATED ON", font9)));
                    table.AddCell(cell9);

                    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("STATUS", font9)));
                    table.AddCell(cell10);

                    # endregion

                    i = 0; // reseting count for new page
                    j = 0;

                }
                # region data on page 
                slno = slno + 1;

                if (slno == 1)
                {
                    policyid = int.Parse(dr["res_policy_id"].ToString());
                    policytype = dr["res_type"].ToString();
                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Policy ID: " + dr["res_policy_id"].ToString() + "     Policy type: " + dr["res_type"].ToString(), font8)));
                    cell12.Colspan = 9;
                    cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);
                    j++;//  sub heading count
                }
                else if (policyid != int.Parse(dr["sno"].ToString()))
                {


                    policyid = int.Parse(dr["res_policy_id"].ToString());
                    policytype = dr["res_type"].ToString();
                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Policy ID: " + dr["res_policy_id"].ToString() + "     Policy type: " + dr["res_type"].ToString(), font8)));
                    cell12.Colspan = 9;
                    cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);
                    slno = 1;
                   j++;//  sub heading count
                }

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table.AddCell(cell11);

                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["amount_res"].ToString(), font8)));
                table.AddCell(cell13);

                string sss = dr["res_from"].ToString();
                DateTime dt5 = DateTime.Parse(dr["res_from"].ToString());
                string date1 = dt5.ToString("dd-MM-yyyy");


                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                table.AddCell(cell14);

                dt5 = DateTime.Parse(dr["res_to"].ToString());
                date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                table.AddCell(cell15);

                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["day_res_max"].ToString(), font8)));
                table.AddCell(cell16);

                PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["day_res_min"].ToString(), font8)));
                table.AddCell(cell17);

                PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["day_res_maxstay"].ToString(), font8)));
                table.AddCell(cell18);

                dt5 = DateTime.Parse(dr["createdon"].ToString());
                date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                table.AddCell(cell19);

                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(dr["rowstatus"].ToString(), font8)));
                table.AddCell(cell20);

                i++;//no of data row count
                # endregion


                doc.Add(table);
                
            }
            # endregion

           
            doc.Close();
         
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=currentpolicy.pdf";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }

        catch
        {
            lblHead.Text = "Tsunami ARMS -Error Message";
            lblOk.Text = "Problem found in opening pdf.";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnOk);
                   
        }
        finally
        {
            con.Close();
        }

    }

    # endregion 

    # region current policy button click 
    protected void btncurrentpolicy_Click(object sender, EventArgs e)
    {
        lblmessage.Visible = false;
        try
        {
            string str1 = yearmonthdate(txtreportfrom.Text);
            string str2 = yearmonthdate(txtreportto.Text);
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }

            # region fetching the data needed to show as report from database and assigning to a datatable
            OdbcCommand cmd31 = new OdbcCommand("CALL selectcond(?,?,?)", con);
            cmd31.CommandType = CommandType.StoredProcedure;
            cmd31.Parameters.AddWithValue("tblname", "t_policy_reservation");
            cmd31.Parameters.AddWithValue("attribute", "res_policy_id,res_type,amount_res,res_from,res_to,day_res_max,day_res_min,day_res_maxstay");

            if (cmbRep.SelectedValue == "All")
            {
                cmd31.Parameters.AddWithValue("conditionv", "curdate() between res_from and res_to and rowstatus <> " + 2 + " order by res_type");
            }
            else
            {
                cmd31.Parameters.AddWithValue("conditionv", "curdate() between res_from and res_to and rowstatus <> " + 2 + " and res_type='" + cmbRep.SelectedValue.ToString() + "' order by res_type");
            }

            OdbcDataAdapter da = new OdbcDataAdapter(cmd31);
            DataTable dt = new DataTable();
            da.Fill(dt);
            # endregion

            // creating a  file to save the report .... setting its font
            Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 80, 80);
            string pdfFilePath = Server.MapPath(".") + "/pdf/currentpolicy.pdf";
            Font font8 = FontFactory.GetFont("Arial", 8);
            Font font9 = FontFactory.GetFont("Arial", 9);
            Font font10 = FontFactory.GetFont("Arial", 10);
            // Font newfont = new Font(Font.FontFamily);

            # region  report table coloumn and header settings
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();
            PdfPTable table1 = new PdfPTable(7);
            float[] colwidth1 ={ 5, 8, 10, 10, 10, 10, 10};
            table1.SetWidths(colwidth1);
            PdfPCell cell = new PdfPCell(new Phrase(cmbRep.SelectedValue.ToString() + "  CURRENT RESERVATION POLICIES", font10));
            cell.Colspan = 7;
            cell.Border = 0;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);
            # endregion

            # region giving heading for each coloumn in report
            PdfPCell cell1001 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table1.AddCell(cell1001);

            PdfPCell cell1003 = new PdfPCell(new Phrase(new Chunk("AMOUNT", font9)));
            table1.AddCell(cell1003);

            PdfPCell cell1004 = new PdfPCell(new Phrase(new Chunk("POLICY FROM", font9)));
            table1.AddCell(cell1004);

            PdfPCell cell1005 = new PdfPCell(new Phrase(new Chunk("POLICY TO", font9)));
            table1.AddCell(cell1005);

            PdfPCell cell1006 = new PdfPCell(new Phrase(new Chunk("MAX DAYS TO RESERVE", font9)));
            table1.AddCell(cell1006);

            PdfPCell cell1007 = new PdfPCell(new Phrase(new Chunk("MIN DAYS TO RESERVE", font9)));
            table1.AddCell(cell1007);

            PdfPCell cell1008 = new PdfPCell(new Phrase(new Chunk("DAYS OF STAY", font9)));
            table1.AddCell(cell1008);            
            doc.Add(table1);
            # endregion

            # region adding data to the report file
            int slno = 0;
            int i = 0, j = 0;
            foreach (DataRow dr in dt.Rows)
            {
                PdfPTable table = new PdfPTable(7);
                float[] colwidth ={ 5, 8, 10, 10, 10, 10, 10 };
                table.SetWidths(colwidth);
                if (i + j > 45)// total rows on page
                {
                    doc.NewPage();

                    # region giving heading for each coloumn in report
                    PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                    table.AddCell(cell1);

                    PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("AMOUNT", font9)));
                    table.AddCell(cell3);

                    PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("POLICY FROM", font9)));
                    table.AddCell(cell4);

                    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("POLICY TO", font9)));
                    table.AddCell(cell5);

                    PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("MAX DAYS TO RESERVE", font9)));
                    table.AddCell(cell6);

                    PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("MIN DAYS TO RESERVE", font9)));
                    table.AddCell(cell7);

                    PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("DAYS OF STAY", font9)));
                    table.AddCell(cell8);
                    # endregion

                    i = 0; // reseting count for new page
                    j = 0;

                }
                # region entering datas
                slno = slno + 1;

                if (slno == 1)
                {
                    policyid = int.Parse(dr["res_policy_id"].ToString());
                    policytype = dr["res_type"].ToString();
                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Policy ID: " + dr["res_policy_id"].ToString() + "     Policy type: " + dr["res_type"].ToString(), font8)));
                    cell12.Colspan = 7;
                    cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);
                    j++;
                }
                else if (policyid != int.Parse(dr["res_policy_id"].ToString()))
                {
                    policyid = int.Parse(dr["res_policy_id"].ToString());
                    policytype = dr["res_type"].ToString();
                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Policy ID: " + dr["res_policy_id"].ToString() + "     Policy type: " + dr["res_type"].ToString(), font8)));
                    cell12.Colspan = 7;
                    cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);
                    slno = 1;
                    j++;
                }

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table.AddCell(cell11);

                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["amount_res"].ToString(), font8)));
                table.AddCell(cell13);

                DateTime dt5 = DateTime.Parse(dr["res_from"].ToString());
                string date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                table.AddCell(cell14);

                dt5 = DateTime.Parse(dr["res_to"].ToString());
                date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                table.AddCell(cell15);

                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["day_res_max"].ToString(), font8)));
                table.AddCell(cell16);

                PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["day_res_min"].ToString(), font8)));
                table.AddCell(cell17);

                PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["day_res_maxstay"].ToString(), font8)));
                table.AddCell(cell18);
                i++;//no of data row count                

                # endregion

                doc.Add(table);
            }
            # endregion

            doc.Close();
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=currentpolicy.pdf";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);


        }

        catch
        {           
            lblHead.Text = "Tsunami ARMS -Error Message";
            lblOk.Text = "Problem found in opening pdf.";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnOk);
        }
    finally
    {
        con.Close();
    }
    }
    # endregion
 
    # region policy fetch corresponding to the date selected
    protected void btnfetchpolicy_Click(object sender, EventArgs e)
    {
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            if (txtreportfrom.Text == "" || txtreportto.Text == "")
            {
                lblmessage.Visible = true;
                return;
            }
            else
                lblmessage.Visible = false;


            string str1 = yearmonthdate(txtreportfrom.Text);
            string str2 = yearmonthdate(txtreportto.Text);
           
            # region fetching the data needed to show as report from database and assigning to a datatable
            OdbcCommand cmd31 = new OdbcCommand("CALL selectcond(?,?,?)", con);
            cmd31.CommandType = CommandType.StoredProcedure;
            cmd31.Parameters.AddWithValue("tblname", "t_policy_reservation");
            cmd31.Parameters.AddWithValue("attribute", "*");
            if (cmbRep.SelectedValue == "All")
            {
                cmd31.Parameters.AddWithValue("conditionv", " rowstatus <> " + 2 + " and (('" + str1.ToString() + "' between res_from and res_to) or ('" + str2.ToString() + "' between res_from and res_to) or(res_from between '" + str1.ToString() + "'  and '" + str2.ToString() + "')or (res_to between '" + str1.ToString() + "' and '" + str2.ToString() + "'))");
            }
            else
            {
                cmd31.Parameters.AddWithValue("conditionv", " rowstatus <> " + 2 + " and (('" + str1.ToString() + "' between res_from and res_to) or ('" + str2.ToString() + "' between res_from and res_to) or(res_from between '" + str1.ToString() + "'  and '" + str2.ToString() + "')or (res_to between '" + str1.ToString() + "'  and '" + str2.ToString() + "')) and  res_from='" + cmbRep.SelectedValue.ToString() + "'");
            }

            OdbcDataAdapter da = new OdbcDataAdapter(cmd31);
            DataTable dt = new DataTable();
            da.Fill(dt);
            # endregion
             
            // creating a  file to save the report .... setting its font
            Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 60, 60);
            string pdfFilePath = Server.MapPath(".") + "/pdf/currentpolicy.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 8);
            Font font9 = FontFactory.GetFont("ARIAL", 9);
            Font font10 = FontFactory.GetFont("ARIAL", 10);
            # region  report table coloumn and header settings

            pdfPage page = new pdfPage();            
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));           
            wr.PageEvent = page;
            doc.Open();
           
            PdfPTable table1 = new PdfPTable(8);
            float[] colwidth1 ={ 5, 8, 10, 10, 8, 8, 8,10 };
            table1.SetWidths(colwidth1);
            PdfPCell cell = new PdfPCell(new Phrase("RESERVATION POLICIES", font10));
            cell.Colspan = 8;
            cell.Border = 0;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);
            # endregion
           

            # region giving heading for each coloumn in report
            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table1.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Type", font9)));
            table1.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Amount", font9)));
            table1.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Policy from", font9)));
            table1.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Policy to", font9)));
            table1.AddCell(cell5);

            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Max days", font9)));
            table1.AddCell(cell6);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Min days", font9)));
            table1.AddCell(cell7);

            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Stay", font9)));
            table1.AddCell(cell8);
            doc.Add(table1);
            # endregion

           
            # region adding data to the report file
            int slno = 0;            
            int i = 0, j = 0;            
            foreach (DataRow dr in dt.Rows)
            {
                
                PdfPTable table = new PdfPTable(8);
                float[] colwidth ={ 5, 8, 10, 10, 8, 8, 8, 10 };
                table.SetWidths(colwidth);
                if (i + j > 45)// total rows on page
                {
                    
                    doc.NewPage();
                    # region giving heading for each coloumn in report
                    PdfPCell cell1001 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                    table.AddCell(cell1001);

                    PdfPCell cell1002 = new PdfPCell(new Phrase(new Chunk("Type", font9)));
                    table.AddCell(cell1002);

                    PdfPCell cell1003 = new PdfPCell(new Phrase(new Chunk("Amount", font9)));
                    table.AddCell(cell1003);

                    PdfPCell cell1004 = new PdfPCell(new Phrase(new Chunk("Policy from", font9)));
                    table.AddCell(cell1004);

                    PdfPCell cell1005 = new PdfPCell(new Phrase(new Chunk("Policy to", font9)));
                    table.AddCell(cell1005);

                    PdfPCell cell1006 = new PdfPCell(new Phrase(new Chunk("Max days", font9)));
                    table.AddCell(cell1006);

                    PdfPCell cell1007 = new PdfPCell(new Phrase(new Chunk("Min days", font9)));
                    table.AddCell(cell1007);

                    PdfPCell cell1008 = new PdfPCell(new Phrase(new Chunk("Stay", font9)));
                    table.AddCell(cell1008);
                    
                    # endregion
                    i = 0; // reseting count for new page
                    j = 0;                
                }

                slno = slno + 1;
                # region ADDING DATA 
                if (slno == 1)
                {

                    policyid = int.Parse(dr["res_policy_id"].ToString());
                    policytype = dr["res_type"].ToString();
                    PdfPCell cell100 = new PdfPCell(new Phrase(new Chunk("Policy ID: " + dr["res_policy_id"].ToString() + "     Policy type: " + dr["res_type"].ToString(), font8)));
                    cell100.Colspan = 8;
                    cell100.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell100);
                    j++;//  sub heading count
                }
                else if (policyid != int.Parse(dr["res_policy_id"].ToString()))
                {


                    policyid = int.Parse(dr["res_policy_id"].ToString());
                    policytype = dr["res_type"].ToString();
                    PdfPCell cell100 = new PdfPCell(new Phrase(new Chunk("Policy ID: " + dr["res_policy_id"].ToString() + "     Policy type: " + dr["res_type"].ToString(), font8)));
                    cell100.Colspan = 8;
                    cell100.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell100);
                    slno = 1;
                    j++;//  sub heading count
                }

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table.AddCell(cell11);

                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["res_type"].ToString(), font8)));
                table.AddCell(cell12);

                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["amount_res"].ToString(), font8)));
                table.AddCell(cell13);

                DateTime dt5 = DateTime.Parse(dr["res_from"].ToString());
                string date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                table.AddCell(cell14);

                dt5 = DateTime.Parse(dr["res_to"].ToString());
                date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                table.AddCell(cell15);

                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["day_res_max"].ToString(), font8)));
                table.AddCell(cell16);

                PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["day_res_min"].ToString(), font8)));
                table.AddCell(cell17);

                PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["day_res_maxstay"].ToString(), font8)));
                table.AddCell(cell18);
                i++;
                doc.Add(table);
                # endregion
            }
            # endregion
            
            doc.Close();
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=currentpolicy.pdf";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

        }
        catch
        {
            lblHead.Text = "Tsunami ARMS -Error Message";
            lblOk.Text = "Problem found in opening pdf.";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnOk);

        }
        finally
        {
            con.Close();
        }
    }
   # endregion

    #region todate index cahang
    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        if (txttodate.Text != "")
        {
            string tempfrom = yearmonthdate(txtfrmdate.Text);
            string tempto = yearmonthdate(txttodate.Text);

            DateTime from = DateTime.Parse(tempfrom);
            DateTime to = DateTime.Parse(tempto);

            if (from >= to)
            {
                MessageBox.Show("To date cannot be less than from date", "Error in to date", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                txttodate.Text = "";
                return;
            }
        }
    }
    #endregion

    #region grid

    #region grid row created

    protected void gdrespolicy_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdrespolicy, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    protected void gdrespolicy_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

    }

    #region grid paging

    protected void gdrespolicy_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdrespolicy.PageIndex = e.NewPageIndex;
        gdrespolicy.DataBind();
        grid_load();
    }
    #endregion

    #region grid selected index changing

    protected void gdrespolicy_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gdrespolicy.SelectedRow;
      

        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            policyid = Convert.ToInt32(gdrespolicy.DataKeys[gdrespolicy.SelectedRow.RowIndex].Value.ToString());

            //start---creating view for log table insertion
            OdbcCommand gridselection = new OdbcCommand("CALL selectcond(?,?,?)", con);
            gridselection.CommandType = CommandType.StoredProcedure;
            gridselection.Parameters.AddWithValue("tblname", "t_policy_reservation");
            gridselection.Parameters.AddWithValue("attribute", "*");
            gridselection.Parameters.AddWithValue("conditionv", "res_policy_id=" + policyid + "");
            OdbcDataAdapter dacnt = new OdbcDataAdapter(gridselection);
            DataTable dttgrdselect = new DataTable();
            dacnt.Fill(dttgrdselect);
            ViewState["gridselection"] = dttgrdselect;

            OdbcCommand gridselection1 = new OdbcCommand("CALL selectcond(?,?,?)", con);
            gridselection1.CommandType = CommandType.StoredProcedure;
            gridselection1.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons");
            gridselection1.Parameters.AddWithValue("attribute", "*");
            gridselection1.Parameters.AddWithValue("conditionv", "res_policy_id=" + policyid + "");
            OdbcDataAdapter dacnt1 = new OdbcDataAdapter(gridselection1);
            DataTable dttgrdselect1 = new DataTable();
            dacnt1.Fill(dttgrdselect1);
            ViewState["gridselection1"] = dttgrdselect1;
            //end --- creating view for log table insertion


            OdbcCommand cmd = new OdbcCommand("select * from t_policy_reservation where res_policy_id = " + policyid + " and rowstatus <> " + 2 + " ", con);
            OdbcDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                clear();
                //Loading all combo box from grid
                cmbtype.SelectedValue = rd["res_type"].ToString();

                cmbprepon.SelectedValue = combo(int.Parse(rd["is_prepone"].ToString()));
                if (cmbprepon.SelectedValue == "1")
                {
                    txtprenoofdys.Visible = true;
                    txtpreno.Visible = true;
                    txtpreamt.Visible = true;
                    lblpreamt.Visible = true;
                    lblpreno.Visible = true;
                    lblprenoofdays.Visible = true;
                    revpreamt.Visible = true;
                    revpreno.Visible = true;
                    revprenoofdays.Visible = true;
                }
                else
                {
                    //prepone
                    txtprenoofdys.Visible = false;
                    txtpreno.Visible = false;
                    txtpreamt.Visible = false;
                    lblpreamt.Visible = false;
                    lblpreno.Visible = false;
                    lblprenoofdays.Visible = false;
                    revpreamt.Visible = false;
                    revpreno.Visible = false;
                    revprenoofdays.Visible = false;

                    txtprenoofdys.Text = "0";
                    txtpreno.Text = "0";
                    txtpreamt.Text = "0";

                }


                cmbpostpon.SelectedValue = combo(int.Parse(rd["is_postpone"].ToString()));
                if (cmbpostpon.SelectedValue == "1")
                {
                    txtpostnoofdys.Visible = true;
                    txtpostno.Visible = true;
                    txtpostamt.Visible = true;
                    lblpostno.Visible = true;
                    lblpostnoofdays.Visible = true;
                    lblRCpostAmt.Visible = true;
                    revpostamt.Visible = true;
                    revpostno.Visible = true;
                    revpostnoofdays.Visible = true;

                }
                else
                {
                    //postpone
                    txtpostnoofdys.Visible = false;
                    txtpostno.Visible = false;
                    txtpostamt.Visible = false;
                    lblpostno.Visible = false;
                    lblpostnoofdays.Visible = false;
                    lblRCpostAmt.Visible = false;
                    revpostamt.Visible = false;
                    revpostno.Visible = false;
                    revpostnoofdays.Visible = false;
                    txtpostnoofdys.Text = "0";
                    txtpostno.Text = "0";
                    txtpostamt.Text = "0";
                }

                cmbcanc.SelectedValue = combo(int.Parse(rd["is_cancel"].ToString()));
                if (cmbcanc.SelectedValue == "1")
                {
                    txtcanclno.Visible = true;
                    txtcanclamt.Visible = true;
                    lblcancelcharge.Visible = true;
                    lblcancelno.Visible = true;
                    revcancelamt.Visible = true;
                    revcancelno.Visible = true;

                }
                else
                {

                    //cancel
                    txtcanclno.Visible = false;
                    txtcanclamt.Visible = false;
                    lblcancelcharge.Visible = false;
                    lblcancelno.Visible = false;
                    revcancelamt.Visible = false;
                    revcancelno.Visible = false;
                    txtcanclno.Text = "0";
                    txtcanclamt.Text = "0";
                }

                //loading all textbox fields
                txtpolicyid.Text = policyid.ToString();
                txtcanclamt.Text = rd["amount_cancel"].ToString();
                txtcanclno.Text = rd["count_cancel"].ToString();
                txtmaxdays.Text = rd["day_res_max"].ToString();
                txtmindays.Text = rd["day_res_min"].ToString();
                txtmaxstay.Text = rd["day_res_maxstay"].ToString();
                txtpostamt.Text = rd["amount_prepone"].ToString();
                txtpostno.Text = rd["count_prepone"].ToString();
                txtpostnoofdys.Text = rd["day_postpone"].ToString();
                txtpreamt.Text = rd["amount_postpone"].ToString();
                txtpreno.Text = rd["count_postpone"].ToString();
                txtprenoofdys.Text = rd["day_prepone"].ToString();
                txtRCamount.Text = rd["amount_res"].ToString();


                DateTime dt1 = DateTime.Parse(rd["res_from"].ToString());
                txtfrmdate.Text = dt1.ToString("dd/MM/yyyy");

                if (rd["res_to"].ToString() != "")
                {
                    DateTime dt2 = DateTime.Parse(rd["res_to"].ToString());
                    txttodate.Text = dt2.ToString("dd/MM/yyyy");
                }

                //loading list box selection
                OdbcCommand cmd1 = new OdbcCommand("select * from t_policy_reserv_seasons where  res_policy_id= " + policyid + " and rowstatus <> " + 2 + " ", con);
                OdbcDataReader rdseason = cmd1.ExecuteReader();

                while (rdseason.Read())
                {
                    for (int i = 0; i < lstseason.Items.Count; i++)
                    {
                        int ss = int.Parse(rdseason["season_id"].ToString());

                        OdbcCommand cmd11 = new OdbcCommand("select * from m_sub_season as mas,m_season as ses where ses.season_id= " + ss + " and ses.season_sub_id=mas.season_id and ses.rowstatus <> " + 2 + " ", con);
                        OdbcDataReader rdseason1 = cmd11.ExecuteReader();
                        while (rdseason1.Read())
                        {
                            if (rdseason1["seasonname"].Equals(lstseason.Items[i].Text))
                                lstseason.Items[i].Selected = true;
                        }
                    }
                }
                rdseason.Close();
                btndelete.Enabled = true;

            }
            rd.Close();

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

    #endregion
  
    protected void btnOk_Click1(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
    }

    #region button yes
    protected void btnYes_Click(object sender, EventArgs e)
    {
        bool q, s, u;
        isrent = commbo(cmbRentApplicable.SelectedItem.Text);        
        isdeposit = commbo(cmbSecurityDeposit.SelectedItem.Text);                      
        if (ViewState["action"].ToString() == "save")
        {
            #region checking for open ended policy
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
                string strSql6 = "SELECT res_policy_id"
                       + " FROM "
                               + "t_policy_reservation"
                       + " WHERE "
                               + " res_to='0000-00-00'"
                               + " and res_type='" + cmbtype.SelectedValue.ToString() + "'"
                               + " and rowstatus <>" + 2 + "";
                OdbcCommand cmdseason6 = new OdbcCommand(strSql6, con);
                OdbcDataReader rdseason6 = cmdseason6.ExecuteReader();
                if (rdseason6.Read())
                {
                    okmessage("Tsunami ARMS - Information", "An open ended policy found"); //message box
                    int openPolicyID = int.Parse(rdseason6[0].ToString());
                    DateTime endDatedt = DateTime.Now.AddDays(-1);
                    string endDateEnding = endDatedt.ToString("yyyy/MM/dd");
                    userid = int.Parse(Session["userid"].ToString());
                    curDate = DateTime.Now;
                    date = curDate.ToString("yyyy-MM-dd") + ' ' + curDate.ToString("HH:mm:ss");
                    string strSql7 = "rowstatus=" + 1 + ","
                                   + "updatedby=" + userid + ","
                                   + "updateddate= '" + date.ToString() + "',"
                                   + "res_to='" + endDateEnding + "'";
                    OdbcCommand cmdupdteOpen = new OdbcCommand("CALL updatedata(?,?,?)", con);
                    cmdupdteOpen.CommandType = CommandType.StoredProcedure;
                    cmdupdteOpen.Parameters.AddWithValue("tablename", "t_policy_reservation");
                    cmdupdteOpen.Parameters.AddWithValue("valu", strSql7);
                    cmdupdteOpen.Parameters.AddWithValue("convariable", "res_policy_id= " + openPolicyID + "");
                    cmdupdteOpen.ExecuteNonQuery();
                }

                con.Close();
                grid_load();
            }
            catch
            {
                okmessage("Tsunami ARMS - Error Message", "Error found in checking open ended policy");
            }
            #endregion

            ////////////////////////////////////////////
            try
            {
                #region save
                DateTime tempfrom, tempto;
                from = yearmonthdate(txtfrmdate.Text);
                tempfrom = DateTime.Parse(from);

                if (txttodate.Text != "")
                {
                    to = yearmonthdate(txttodate.Text);
                    tempto = DateTime.Parse(to);
                    if (tempfrom > tempto)
                    {                        
                        okmessage("Tsunami ARMS - Information", "To Date less than from date is not allowed."); //message box
                        return;
                    }
                }
                else
                {
                    txttodate.Text = null;
                    to = null;                    
                }
                                                                          
                    txtpolicyid.Text = primarykey("res_policy_id", "t_policy_reservation");
                    temp = int.Parse(txtpolicyid.Text.ToString());

                    # region Checking previously existing polcies
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.ConnectionString = strConnection;
                            con.Open();
                        }
                    
                        # region checking from date with previous policy

                        string strSql2 = "SELECT *"
                               + " FROM "
                                       + "t_policy_reservation"
                               + " WHERE "
                                       + "res_from <= '" + from.ToString() + "'"
                                       + " and res_to >= '" + from.ToString() + "' "
                                       + " and res_type='" + cmbtype.SelectedValue.ToString() + "'"
                                       + " and rowstatus <>" + 2 + "";


                        OdbcCommand cmdseason = new OdbcCommand(strSql2, con);
                        OdbcDataReader rdseason = cmdseason.ExecuteReader();
                        int flag1 = 1;
                        while (flag1 == 1)
                        {
                            if (rdseason.Read())
                            {
                                flag1 = 1;
                                lblHead.Text = "Tsunami ARMS - Message";
                                lblOk.Text = "Polciy already exists in the period for the selected type.";
                                pnlOk.Visible = true;
                                pnlYesNo.Visible = false;
                                ModalPopupExtender2.Show();
                                this.ScriptManager1.SetFocus(btnOk);
                                con.Close();
                                return;                    
                            }
                            else
                                flag1 = 0;

                        }

                        rdseason.Close();


                        # endregion

                        # region checking to date with previous policy
                        OdbcCommand cmdseason1 = new OdbcCommand("select * from t_policy_reservation where res_from <= '" + to.ToString() + "' and res_to >= '" + to.ToString() + "' and res_type='" + cmbtype.SelectedValue.ToString() + "' and rowstatus <> " + 2 + "", con);
                        OdbcDataReader rdseason1 = cmdseason1.ExecuteReader();
                        flag1 = 1;
                        while (flag1 == 1)
                        {
                            if (rdseason1.Read())
                            {
                                flag1 = 1;

                                lblHead.Text = "Tsunami ARMS - Message";
                                lblOk.Text = "Polciy already exists in the period for the selected type.";
                                pnlOk.Visible = true;
                                pnlYesNo.Visible = false;
                                ModalPopupExtender2.Show();
                                this.ScriptManager1.SetFocus(btnOk);
                                con.Close();
                                return;                            
                            }
                            else
                                flag1 = 0;
                        }
                        rdseason1.Close();
                        # endregion

                        # region checking from date with previous policy
                        OdbcCommand cmdseason2 = new OdbcCommand("select * from t_policy_reservation where ('" + from.ToString() + "' between res_from  and res_to) and res_type='" + cmbtype.SelectedValue.ToString() + "' and rowstatus <> " + 2 + "", con);
                        OdbcDataReader rdseason2 = cmdseason2.ExecuteReader();
                        flag1 = 1;
                        while (flag1 == 1)
                        {
                            if (rdseason2.Read())
                            {
                                lblHead.Text = "Tsunami ARMS - Message";
                                lblOk.Text = "Polciy already exists for the season selected.";
                                pnlOk.Visible = true;
                                pnlYesNo.Visible = false;
                                ModalPopupExtender2.Show();
                                this.ScriptManager1.SetFocus(btnOk);
                                con.Close();
                                return;                          
                            }
                            else
                                flag1 = 0;

                        }

                        rdseason.Close();


                        # endregion

                        # region checking from date with previous policy
                        OdbcCommand cmdseason3 = new OdbcCommand("select * from t_policy_reservation where  ('" + from.ToString() + "' between res_from and res_to) and res_type='" + cmbtype.SelectedValue.ToString() + "' and rowstatus <> " + 2 + "", con);
                        OdbcDataReader rdseason3 = cmdseason3.ExecuteReader();
                        flag1 = 1;
                        while (flag1 == 1)
                        {
                            if (rdseason3.Read())
                            {
                                lblHead.Text = "Tsunami ARMS - Message";
                                lblOk.Text = "Polciy already exists for the season selected.";
                                pnlOk.Visible = true;
                                pnlYesNo.Visible = false;
                                ModalPopupExtender2.Show();
                                this.ScriptManager1.SetFocus(btnOk);
                                con.Close();
                                return;                        
                            }
                            else
                                flag1 = 0;

                        }

                        rdseason.Close();


                        # endregion
                    }
                    catch
                    { }
                    finally
                    {
                        con.Close();
                    }

                    # endregion

                    # region VALIDATING NULL VALUES

                    //prepone
                    if (cmbprepon.SelectedValue == "0")
                    {
                        txtpreamt.Text = "0";
                        txtpreno.Text = "0";
                        txtprenoofdys.Text = "0";
                    }


                    // postpone
                    if (cmbpostpon.SelectedValue == "0")
                    {
                        txtpostamt.Text = "0";
                        txtpostno.Text = "0";
                        txtpostnoofdys.Text = "0";
                    }

                    //cancellation
                    if (cmbcanc.SelectedValue == "0")
                    {
                        txtcanclamt.Text = "0";
                        txtcanclno.Text = "0";
                    }

                    # endregion

                    # region coversion to boolean
                    q = converttoboolean(cmbprepon.SelectedValue.ToString());
                    s = converttoboolean(cmbcanc.SelectedValue.ToString());
                    u = converttoboolean(cmbpostpon.SelectedValue.ToString());
                    # endregion

                    # region DATE CONVERTIONS AND SERVER DATE
                    //changing to 
                    txtfrmdate.Text = yearmonthdate(txtfrmdate.Text);
                    txttodate.Text = yearmonthdate(txttodate.Text);
                    curDate = DateTime.Now;
                    date = curDate.ToString("yyyy-MM-dd") + ' ' + curDate.ToString("HH:mm:ss");

                    //taking server time
                   
                    # endregion

                    if (string.Compare(btnsave.Text, "Save") == 0)
                    {
                        # region  SAVE BUTTON FUNCTION
                        try
                        {
                            userid = int.Parse(Session["userid"].ToString());
                        }
                        catch
                        {
                            userid = 0;
                        }

                        # region policy table insertion
                        if (con.State == ConnectionState.Closed)
                        {
                            con.ConnectionString = strConnection;
                            con.Open();
                        }
                        // inserting into reservation policy table

                        string st = @"" + temp + ",'" + cmbtype.SelectedValue.ToString() + "', " + int.Parse(txtRCamount.Text.ToString()) + "," + int.Parse(txtmaxdays.Text.ToString()) + ", " + int.Parse(txtmindays.Text.ToString()) + "," + int.Parse(txtmaxstay.Text.ToString()) + ", " + q + ", " + int.Parse(txtpreamt.Text.ToString()) + "," + int.Parse(txtprenoofdys.Text.ToString()) + "," + int.Parse(txtpreno.Text.ToString()) + ", " + s + "," + int.Parse(txtcanclamt.Text.ToString()) + "," + int.Parse(txtcanclno.Text.ToString()) + "," + u + "," + int.Parse(txtpostamt.Text.ToString()) + "," + int.Parse(txtpostnoofdys.Text.ToString()) + "," + int.Parse(txtpostno.Text.ToString()) + ",'" + txtfrmdate.Text.ToString() + "','" + txttodate.Text.ToString() + "'," + 0 + "," + userid + ",'" + date.ToString() + "'," + userid + ",'" + date.ToString() + "'," + txtPre.Text + "," + isrent + "," + isdeposit + "," + isother + "";
                        OdbcCommand cmdsaveRpolicy = new OdbcCommand("CALL savedata(?,?)", con);
                        cmdsaveRpolicy.CommandType = CommandType.StoredProcedure;
                        cmdsaveRpolicy.Parameters.AddWithValue("tblname", "t_policy_reservation");
                        cmdsaveRpolicy.Parameters.AddWithValue("val", "" + temp + ",'" + cmbtype.SelectedValue.ToString() + "', " + int.Parse(txtRCamount.Text.ToString()) + "," + int.Parse(txtmaxdays.Text.ToString()) + ", " + int.Parse(txtmindays.Text.ToString()) + "," + int.Parse(txtmaxstay.Text.ToString()) + ", " + q + ", " + int.Parse(txtpreamt.Text.ToString()) + "," + int.Parse(txtprenoofdys.Text.ToString()) + "," + int.Parse(txtpreno.Text.ToString()) + ", " + s + "," + int.Parse(txtcanclamt.Text.ToString()) + "," + int.Parse(txtcanclno.Text.ToString()) + "," + u + "," + int.Parse(txtpostamt.Text.ToString()) + "," + int.Parse(txtpostnoofdys.Text.ToString()) + "," + int.Parse(txtpostno.Text.ToString()) + ",'" + txtfrmdate.Text.ToString() + "','" + txttodate.Text.ToString() + "'," + 0 + "," + userid + ",'" + date.ToString() + "'," + userid + ",'" + date.ToString() + "'," + txtPre.Text + "," + isrent + "," + isdeposit + "," + isother + "");
                        cmdsaveRpolicy.ExecuteNonQuery();
                        con.Close();
                        # endregion

                        # region Policy season table 

                        // primary key of season table             
                        temp2 = int.Parse(primarykey("res_season_id", "t_policy_reserv_seasons"));
                        // primary key of season table log                                  
                        if (con.State == ConnectionState.Closed)
                        {
                            con.ConnectionString = strConnection;
                            con.Open();
                        }
                        for (int i = 0; i < lstseason.Items.Count; i++)
                        {
                            if (lstseason.Items[i].Selected == true)
                            {
                             

                                string seson = lstseason.Items[i].Text.ToString();

                                OdbcCommand cmdseason11 = new OdbcCommand("select season_sub_id from m_sub_season where seasonname='" + seson + "' and rowstatus<>" + 2 + "", con);
                                OdbcDataReader sessid = cmdseason11.ExecuteReader();
                                if (sessid.Read())
                                {
                                    sesid = int.Parse(sessid["season_sub_id"].ToString());
                                }                               

                                // inserting into reservation policy season table
                                OdbcCommand cmdsaveRPseason = new OdbcCommand("CALL savedata(?,?)", con);
                                cmdsaveRPseason.CommandType = CommandType.StoredProcedure;
                                cmdsaveRPseason.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons");
                                cmdsaveRPseason.Parameters.AddWithValue("val", "" + temp2 + "," + temp + "," + sesid + "," + userid + ",'" + date.ToString() + "'," + 0 + "," + userid + ",'" + date.ToString() + "'");
                                cmdsaveRPseason.ExecuteNonQuery();

                                temp2++;
                            }

                        }
                        con.Close();

                        # endregion
                       
                        clear();
                        txtpolicyid.Text = primarykey("sno", "reservationpolicy");
                        grid_load();

                        # region saved successfully message box                       
                        okmessage("Tsunami ARMS - Information", "Policy was saved succesfully."); //message box
                        # endregion

                        # endregion
                    }                             
                #endregion
            }
            catch
            {
                #region error message
                okmessage("Tsunami ARMS - Information", "Error found in saving Polcy."); //message box
                #endregion
            }
        }
        else if (ViewState["action"].ToString() == "edit")
        {
            try
            {
                #region edit

                policyid = Convert.ToInt32(gdrespolicy.DataKeys[gdrespolicy.SelectedRow.RowIndex].Value.ToString());
                from = yearmonthdate(txtfrmdate.Text);
                to = yearmonthdate(txttodate.Text);
                DateTime tempfrom = DateTime.Parse(from);
                DateTime tempto = DateTime.Parse(to);
                if (tempfrom > tempto)
                {
                    lblHead.Text = "Tsunami ARMS - Message";
                    lblOk.Text = "To Date less than from date is not allowed.";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                else
                {

                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }

                    string strSql3 = "SELECT *"
                              + " FROM "
                                      + "t_policy_reservation"
                              + " WHERE "
                                      + "res_from <= '" + from.ToString() + "'"
                                      + " and res_to >= '" + from.ToString() + "' "
                                      + " and res_type='" + cmbtype.SelectedValue.ToString() + "'"
                                      + " and rowstatus <>" + 2 + ""
                                      + " and res_policy_id!=" + policyid + "";


                    OdbcCommand cmdseason = new OdbcCommand(strSql3, con);
                    OdbcDataReader rdseason = cmdseason.ExecuteReader();

                    if (rdseason.Read())
                    {
                        lblMsg.Text = "Polciy already exists for the from date period selected: Press Yes to replace existing policy";
                        ViewState["action"] = "edit-previous policy1";
                        pnlOk.Visible = false;
                        pnlYesNo.Visible = true;
                        ModalPopupExtender2.Show();
                        this.ScriptManager1.SetFocus(btnYes);
                        con.Close();

                    }
                    else
                    {
                        string strSql4 = "SELECT *"
                             + " FROM "
                                     + "t_policy_reservation"
                             + " WHERE "
                                     + "res_from <= '" + to.ToString() + "'"
                                     + " and res_to >= '" + to.ToString() + "' "
                                     + " and res_type='" + cmbtype.SelectedValue.ToString() + "'"
                                     + " and rowstatus <>" + 2 + ""
                                     + " and res_policy_id!=" + policyid + "";

                        OdbcCommand cmdseason1 = new OdbcCommand(strSql4, con);
                        OdbcDataReader rdseason1 = cmdseason1.ExecuteReader();

                        if (rdseason1.Read())
                        {
                            lblMsg.Text = "Polciy already exists for the period selected: Press Yes to replace existing policy";
                            ViewState["action"] = "edit-previous policy2";
                            pnlOk.Visible = false;
                            pnlYesNo.Visible = true;
                            ModalPopupExtender2.Show();
                            this.ScriptManager1.SetFocus(btnYes);
                            con.Close();
                        }
                        else
                        {
                                                
                            # region VALIDATING NULL VALUES

                            //prepone
                            if (cmbprepon.SelectedValue == "0")
                            {
                                txtpreamt.Text = "0";
                                txtpreno.Text = "0";
                                txtprenoofdys.Text = "0";
                            }


                            // postpone
                            if (cmbpostpon.SelectedValue == "0")
                            {
                                txtpostamt.Text = "0";
                                txtpostno.Text = "0";
                                txtpostnoofdys.Text = "0";
                            }

                            //cancellation
                            if (cmbcanc.SelectedValue == "0")
                            {
                                txtcanclamt.Text = "0";
                                txtcanclno.Text = "0";
                            }

                            # endregion

                            # region coversion to boolean
                            q = converttoboolean(cmbprepon.SelectedValue.ToString());
                            s = converttoboolean(cmbcanc.SelectedValue.ToString());
                            u = converttoboolean(cmbpostpon.SelectedValue.ToString());
                            # endregion

                            # region DATE CONVERTIONS AND SERVER DATE
                            //changing to 
                            txtfrmdate.Text = yearmonthdate(txtfrmdate.Text);
                            txttodate.Text = yearmonthdate(txttodate.Text);

                            //taking server time
                            DateTime dt = DateTime.Now;
                            String date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
                            # endregion

                            # region EDIT BUTTON FUNCTION

                            //fetching primary key of Reservation policy log table
                            temp1 = int.Parse(primarykey("rowno", "t_policy_reservation_log"));
                            temp = int.Parse(txtpolicyid.Text.ToString());
                            temp2 = int.Parse(primarykey("rowno", "t_policy_reserv_seasons_log"));


                            # region policy log table insertion

                            DataTable dttgrdselect = new DataTable();
                            dttgrdselect = (DataTable)ViewState["gridselection"];

                            try
                            {
                                userid = int.Parse(Session["userid"].ToString());
                            }
                            catch
                            {
                                userid = 0;
                            }

                            if (con.State == ConnectionState.Closed)
                            {
                                con.ConnectionString = strConnection;
                                con.Open();
                            }

                            DateTime ReservationFrom1 = DateTime.Parse(dttgrdselect.Rows[0]["res_from"].ToString());
                            string ReservationFrom = ReservationFrom1.ToString("yyyy/MM/dd");
                            DateTime ReservationFrom2 = DateTime.Parse(dttgrdselect.Rows[0]["res_to"].ToString());
                            string ReservationTo = ReservationFrom2.ToString("yyyy/MM/dd");

                            // inserting into reservation policy log table
                            OdbcCommand cmdsaveRpolicylog = new OdbcCommand("CALL savedata(?,?)", con);
                            cmdsaveRpolicylog.CommandType = CommandType.StoredProcedure;
                            cmdsaveRpolicylog.Parameters.AddWithValue("tblname", "t_policy_reservation_log");
                            cmdsaveRpolicylog.Parameters.AddWithValue("val", "" + temp1 + "," + temp + ",'" + dttgrdselect.Rows[0]["res_type"].ToString() + "'," + int.Parse(dttgrdselect.Rows[0]["amount_res"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_res_max"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_res_min"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_res_maxstay"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["is_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["amount_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["count_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["is_cancel"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["amount_cancel"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["count_cancel"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["is_postpone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["amount_postpone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_postpone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["count_postpone"].ToString()) + ",'" + ReservationFrom + "','" + ReservationTo + "'," + 0 + "," + userid + ",'" + date.ToString() + "'");
                            cmdsaveRpolicylog.ExecuteNonQuery();
                            con.Close();

                            # endregion

                            # region Policy table update
                            if (con.State == ConnectionState.Closed)
                            {
                                con.ConnectionString = strConnection;
                                con.Open();
                            }
                            OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);
                            cmdupdte.CommandType = CommandType.StoredProcedure;
                            cmdupdte.Parameters.AddWithValue("tablename", "t_policy_reservation");
                            cmdupdte.Parameters.AddWithValue("valu", "res_type='" + cmbtype.SelectedValue.ToString() + "',amount_res= " + int.Parse(txtRCamount.Text.ToString()) + ",day_res_max=" + int.Parse(txtmaxdays.Text.ToString()) + ",day_res_min= " + int.Parse(txtmindays.Text.ToString()) + ",day_res_maxstay=" + int.Parse(txtmaxstay.Text.ToString()) + ",is_prepone= " + q + ",amount_prepone= " + int.Parse(txtpreamt.Text.ToString()) + ",day_prepone=" + int.Parse(txtprenoofdys.Text.ToString()) + ",count_prepone= " + int.Parse(txtpreno.Text.ToString()) + ",is_cancel= " + s + ",amount_cancel=" + int.Parse(txtcanclamt.Text.ToString()) + ",count_cancel= " + int.Parse(txtcanclno.Text.ToString()) + ",is_postpone=" + u + ",amount_postpone=" + int.Parse(txtpostamt.Text.ToString()) + ",day_postpone= " + int.Parse(txtpostnoofdys.Text.ToString()) + ",count_postpone= " + int.Parse(txtpostno.Text.ToString()) + ",res_from='" + txtfrmdate.Text.ToString() + "',res_to= '" + txttodate.Text.ToString() + "', rowstatus=" + 1 + ",updatedby=" + userid + ",updateddate= '" + date.ToString() + "',pre_reserve_day='"+txtPre.Text+"',");
                            cmdupdte.Parameters.AddWithValue("convariable", "res_policy_id= " + policyid + "");
                            cmdupdte.ExecuteNonQuery();
                            con.Close();
                            # endregion

                            # region SEASON TABLE  old entry status deleting
                            if (con.State == ConnectionState.Closed)
                            {
                                con.ConnectionString = strConnection;
                                con.Open();
                            }
                            OdbcCommand cmdpk4 = new OdbcCommand("select * from t_policy_reserv_seasons where  res_policy_id= " + temp + " and rowstatus <> " + 2 + " ", con);
                            OdbcDataReader rdseason4 = cmdpk4.ExecuteReader();

                            while (rdseason4.Read())
                            {
                                // season table status delete
                                OdbcCommand cmdupdteseason = new OdbcCommand("CALL updatedata(?,?,?)", con);
                                cmdupdteseason.CommandType = CommandType.StoredProcedure;
                                cmdupdteseason.Parameters.AddWithValue("tablename", "t_policy_reserv_seasons");
                                cmdupdteseason.Parameters.AddWithValue("valu", "rowstatus=" + 2 + "");
                                cmdupdteseason.Parameters.AddWithValue("convariable", "res_season_id= '" + int.Parse(rdseason4["res_season_id"].ToString()) + "'");
                                cmdupdteseason.ExecuteNonQuery();
                            }
                            rdseason4.Close();
                            con.Close();

                            # endregion

                            # region Policy season table and log table insertion new selection

                            // primary key of season table             
                            temp2 = int.Parse(primarykey("res_season_id", "t_policy_reserv_seasons"));
                            // primary key of season table log            
                            temp3 = int.Parse(primarykey("rowno", "t_policy_reserv_seasons_log"));

                            if (con.State == ConnectionState.Closed)
                            {
                                con.ConnectionString = strConnection;
                                con.Open();
                            }


                            DataTable dttgrdselect1 = new DataTable();
                            dttgrdselect1 = (DataTable)ViewState["gridselection1"];
                            int count1 = int.Parse((dttgrdselect1.Rows.Count).ToString());
                            int ii = 0;
                            while (ii < count1)
                            {
                                OdbcCommand cmdsaveRSpolicylog = new OdbcCommand("CALL savedata(?,?)", con);
                                cmdsaveRSpolicylog.CommandType = CommandType.StoredProcedure;
                                cmdsaveRSpolicylog.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons_log");
                                cmdsaveRSpolicylog.Parameters.AddWithValue("val", "" + temp3 + "," + int.Parse(dttgrdselect1.Rows[ii]["res_season_id"].ToString()) + "," + int.Parse(dttgrdselect1.Rows[ii]["res_policy_id"].ToString()) + "," + int.Parse(dttgrdselect1.Rows[ii]["season_sub_id"].ToString()) + "," + userid + ",'" + date.ToString() + "'," + 0 + "");
                                cmdsaveRSpolicylog.ExecuteNonQuery();

                                temp3++;
                                ii++;
                            }


                            for (int i = 0; i < lstseason.Items.Count; i++)
                            {
                                if (lstseason.Items[i].Selected == true)
                                {
                                    string seson = lstseason.Items[i].Text.ToString();
                                    OdbcCommand cmdseason11 = new OdbcCommand("select season_sub_id from m_sub_season where seasonname='" + seson + "' and rowstatus<>" + 2 + "", con);
                                    OdbcDataReader sessid = cmdseason11.ExecuteReader();
                                    if (sessid.Read())
                                    {
                                        sesid = int.Parse(sessid["season_sub_id"].ToString());
                                    }

                                
                                    // inserting into reservation policy season table

                                    OdbcCommand cmdsaveRPseason = new OdbcCommand("CALL savedata(?,?)", con);
                                    cmdsaveRPseason.CommandType = CommandType.StoredProcedure;
                                    cmdsaveRPseason.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons");
                                    cmdsaveRPseason.Parameters.AddWithValue("val", "" + temp2 + "," + policyid + "," + sesid + "," + userid + ",'" + date.ToString() + "'," + 0 + "," + userid + ",'" + date.ToString() + "'");
                                    cmdsaveRPseason.ExecuteNonQuery();

                                    temp2++;

                                    // inserting into reservation policy season log table
                                    // season log table insertion


                                }

                            }
                            con.Close();

                            # endregion

                            # endregion

                            clear();
                            txtpolicyid.Text = primarykey("res_policy_id", "t_policy_reservation");
                            grid_load();

                            okmessage("Tsunami ARMS - Information", "Policy edited succesfully."); //message box
                        }
                    }
                }
                #endregion
            }
            catch
            {
                #region error message
                okmessage("Tsunami ARMS - Information", "Error found in editing Polcy."); //message box
                #endregion
            }
        }
        else if (ViewState["action"].ToString() == "delete")
        {
            try
            {
                #region delete

                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
                DateTime dateCurrent = DateTime.Now;
                String date = dateCurrent.ToString("yyyy-MM-dd") + ' ' + dateCurrent.ToString("HH:mm:ss");
                txtfrmdate.Text = yearmonthdate(txtfrmdate.Text);
                txttodate.Text = yearmonthdate(txttodate.Text);
                policyid = Convert.ToInt32(gdrespolicy.DataKeys[gdrespolicy.SelectedRow.RowIndex].Value.ToString());

                string strSql1 = "SELECT *"
                   + " FROM "
                                  + "t_policy_reservation"
                   + " WHERE "
                                  + " res_type='" + cmbtype.SelectedValue.ToString() + "'"
                                  + " and rowstatus<>" + 2 + ""
                                  + " and '" + date + "' between res_from and res_to"
                                  + " and res_policy_id=" + int.Parse(policyid.ToString()) + "";


                OdbcCommand cmdSeasonCheck1 = new OdbcCommand(strSql1, con);
                OdbcDataReader odrSeasonCheck1 = cmdSeasonCheck1.ExecuteReader();
                if (!odrSeasonCheck1.Read())
                {


                    //taking server time


                    # region VALIDATING NULL VALUES


                    //prepone
                    if (cmbprepon.SelectedValue == "0")
                    {
                        txtpreamt.Text = "0";
                        txtpreno.Text = "0";
                        txtprenoofdys.Text = "0";
                    }


                    // postpone
                    if (cmbpostpon.SelectedValue == "0")
                    {

                        txtpostamt.Text = "0";
                        txtpostno.Text = "0";
                        txtpostnoofdys.Text = "0";
                    }

                    //cancellation
                    if (cmbcanc.SelectedValue == "0")
                    {

                        txtcanclamt.Text = "0";
                        txtcanclno.Text = "0";
                    }


                    # endregion

                    # region coversion to boolean

                    q = converttoboolean(cmbcanc.SelectedValue.ToString());
                    s = converttoboolean(cmbprepon.SelectedValue.ToString());
                    u = converttoboolean(cmbpostpon.SelectedValue.ToString());

                    # endregion

                    #region comented
                    //# region date comparison whether current policy or not
                    //try                    
                    //{
                    //    con.ConnectionString = strConnection;
                    //    con.Open();
                    //    OdbcCommand cmdcurseason = new OdbcCommand("select seasonname from m_season where (curdate() between startengdate and endengdate) and rowstatus <> " + 2 + "", con);
                    //    OdbcDataReader rdcurseason = cmdcurseason.ExecuteReader();
                    //    if (rdcurseason.Read())
                    //    {
                    //        string season = rdcurseason[0].ToString();
                    //        rdcurseason.Close();
                    //        OdbcCommand cmdpolicyseason = new OdbcCommand("select season_id from t_policy_reserv_seasons where rowstatus <> " + 2 + " and res_policy_id =" + int.Parse(txtpolicyid.Text.ToString()) + "", con);
                    //        OdbcDataReader rdpolicyseason = cmdpolicyseason.ExecuteReader();
                    //        while (rdpolicyseason.Read())
                    //        {
                    //            if (season == rdpolicyseason[0].ToString())
                    //            {
                    //                OdbcCommand cmdpolicy0 = new OdbcCommand("select * from t_policy_reservation where rowstatus <> " + 2 + " and res_policy_id =" + int.Parse(txtpolicyid.Text.ToString()) + " and curdat() between res_from and res_to", con);
                    //                OdbcDataReader rdpolicy0 = cmdpolicy0.ExecuteReader();
                    //                if(rdpolicy0.Read())
                    //                {
                    //                    MessageBox.Show("Policy you selected to delete is currrent policy. Ite cannot be deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    //                    rdpolicyseason.Close();
                    //                    return;
                    //                }

                    //            }
                    //        }
                    //        rdpolicyseason.Close();
                    //    }
                    //    else
                    //    {
                    //        if (MessageBox.Show("No season for current date. Do you want to delete anyway?", "warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly).Equals(DialogResult.No))
                    //        {
                    //            return;
                    //        }
                    //    }



                    //}
                    //catch (Exception ex)
                    //{
                    //    if (MessageBox.Show("Cannot check whether this is current policy. Do you want to delete anyway?", "warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly).Equals(DialogResult.No))
                    //    {
                    //        return;
                    //    }


                    //}
                    //finally
                    //{
                    //    con.Close();
                    //}


                    //# endregion

                    ///////////////////////////////////////

                    #endregion

                    temp1 = int.Parse(primarykey("rowno", "t_policy_reservation_log"));
                    temp = int.Parse(txtpolicyid.Text.ToString());
                    policyid = Convert.ToInt32(gdrespolicy.DataKeys[gdrespolicy.SelectedRow.RowIndex].Value.ToString());
                    temp2 = int.Parse(primarykey("rowno", "t_policy_reserv_seasons_log"));

                    # region policy log table insertion

                    DataTable dttgrdselect = new DataTable();
                    dttgrdselect = (DataTable)ViewState["gridselection"];

                    try
                    {
                        userid = int.Parse(Session["userid"].ToString());
                    }
                    catch
                    {
                        userid = 0;
                    }

                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }

                    string ReservationTo;
                    DateTime ReservationFrom1 = DateTime.Parse(dttgrdselect.Rows[0]["res_from"].ToString());
                    string ReservationFrom = ReservationFrom1.ToString("yyyy/MM/dd");
                    if (dttgrdselect.Rows[0]["res_to"].ToString() != "")
                    {
                        DateTime ReservationFrom2 = DateTime.Parse(dttgrdselect.Rows[0]["res_to"].ToString());
                        ReservationTo = ReservationFrom2.ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        ReservationTo = "";
                    }


                    // inserting into reservation policy log table
                    OdbcCommand cmdsaveRpolicylog = new OdbcCommand("CALL savedata(?,?)", con);
                    cmdsaveRpolicylog.CommandType = CommandType.StoredProcedure;
                    cmdsaveRpolicylog.Parameters.AddWithValue("tblname", "t_policy_reservation_log");
                    cmdsaveRpolicylog.Parameters.AddWithValue("val", "" + temp1 + "," + temp + ",'" + dttgrdselect.Rows[0]["res_type"].ToString() + "'," + int.Parse(dttgrdselect.Rows[0]["amount_res"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_res_max"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_res_min"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_res_maxstay"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["is_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["amount_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["count_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["is_cancel"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["amount_cancel"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["count_cancel"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["is_postpone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["amount_postpone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_postpone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["count_postpone"].ToString()) + ",'" + ReservationFrom + "','" + ReservationTo + "'," + 0 + "," + userid + ",'" + date.ToString() + "'");
                    cmdsaveRpolicylog.ExecuteNonQuery();
                    con.Close();

                    # endregion

                    # region Policy table delete
                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }
                    policyid = Convert.ToInt32(gdrespolicy.DataKeys[gdrespolicy.SelectedRow.RowIndex].Value.ToString());
                    OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);
                    cmdupdte.CommandType = CommandType.StoredProcedure;
                    cmdupdte.Parameters.AddWithValue("tablename", "t_policy_reservation");
                    cmdupdte.Parameters.AddWithValue("valu", "rowstatus=" + 2 + ",updatedby=" + userid + ",updateddate= '" + date.ToString() + "'");
                    cmdupdte.Parameters.AddWithValue("convariable", "res_policy_id= " + policyid + "");
                    cmdupdte.ExecuteNonQuery();
                    con.Close();
                    # endregion

                    # region SEASON TABLE   deleting
                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }
                    OdbcCommand cmdpk4 = new OdbcCommand("select * from t_policy_reserv_seasons where  res_policy_id= " + temp + " and rowstatus <> " + 2 + " ", con);
                    OdbcDataReader rdseason4 = cmdpk4.ExecuteReader();

                    while (rdseason4.Read())
                    {
                        // season table status delete
                        OdbcCommand cmdupdteseason = new OdbcCommand("CALL updatedata(?,?,?)", con);
                        cmdupdteseason.CommandType = CommandType.StoredProcedure;
                        cmdupdteseason.Parameters.AddWithValue("tablename", "t_policy_reserv_seasons");
                        cmdupdteseason.Parameters.AddWithValue("valu", "rowstatus=" + 2 + "");
                        cmdupdteseason.Parameters.AddWithValue("convariable", "res_season_id= '" + int.Parse(rdseason4["res_season_id"].ToString()) + "'");
                        cmdupdteseason.ExecuteNonQuery();
                    }
                    rdseason4.Close();
                    con.Close();

                    # endregion

                    # region Policy season table and log table insertion new selection

                    // primary key of season table             
                    temp2 = int.Parse(primarykey("res_season_id", "t_policy_reserv_seasons"));
                    // primary key of season table log            
                    temp3 = int.Parse(primarykey("rowno", "t_policy_reserv_seasons_log"));

                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }


                    DataTable dttgrdselect1 = new DataTable();
                    dttgrdselect1 = (DataTable)ViewState["gridselection1"];
                    int count = int.Parse((dttgrdselect1.Rows.Count).ToString());
                    int ii = 0;
                    while (ii < count)
                    {
                        OdbcCommand cmdsaveRSpolicylog = new OdbcCommand("CALL savedata(?,?)", con);
                        cmdsaveRSpolicylog.CommandType = CommandType.StoredProcedure;
                        cmdsaveRSpolicylog.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons_log");
                        cmdsaveRSpolicylog.Parameters.AddWithValue("val", "" + temp3 + "," + int.Parse(dttgrdselect1.Rows[ii]["res_season_id"].ToString()) + "," + int.Parse(dttgrdselect1.Rows[ii]["res_policy_id"].ToString()) + "," + int.Parse(dttgrdselect1.Rows[ii]["season_id"].ToString()) + "," + userid + ",'" + date.ToString() + "'," + 0 + "");
                        cmdsaveRSpolicylog.ExecuteNonQuery();

                        temp3++;
                        ii++;
                    }





                    con.Close();



                    # endregion





                    clear();
                    txtpolicyid.Text = primarykey("res_policy_id", "t_policy_reservation");
                    grid_load();
                   
                    okmessage("Tsunami ARMS - Information", "Policy deleted succesfully."); //message box
                }
                else
                {                   
                    okmessage("Tsunami ARMS - Information", "Current Policy not allowed to delete."); //message box
                }
                #endregion
            }
            catch
            {
                #region error message
                okmessage("Tsunami ARMS - Information", "Error found in deleting Polcy."); //message box
                #endregion
            }
        }
        else if (ViewState["action"].ToString() == "edit-previous policy1")
        {
            try
            {
                #region edit-previous policy1
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }

                policyid = Convert.ToInt32(gdrespolicy.DataKeys[gdrespolicy.SelectedRow.RowIndex].Value.ToString());
                from = yearmonthdate(txtfrmdate.Text);
                to = yearmonthdate(txttodate.Text);
                DateTime tempfrom = DateTime.Parse(from);
                DateTime tempto = DateTime.Parse(to);

               
                OdbcCommand cmdseason1 = new OdbcCommand("select * from t_policy_reservation where res_from <= '" + to.ToString() + "' and res_to >= '" + to.ToString() + "' and res_type='" + cmbtype.SelectedValue.ToString() + "' and rowstatus <> " + 2 + "", con);
                OdbcDataReader rdseason1 = cmdseason1.ExecuteReader();

                if (rdseason1.Read())
                {
                    lblMsg.Text = "Polciy already exists for the To date period selected: Press Yes to replace existing policy";
                    ViewState["action"] = "edit-previous policy2";
                    pnlOk.Visible = false;
                    pnlYesNo.Visible = true;
                    ModalPopupExtender2.Show();
                    this.ScriptManager1.SetFocus(btnYes);
                    con.Close();
                }
                else
                {
                    # region VALIDATING NULL VALUES

                    //prepone
                    if (cmbprepon.SelectedValue == "0")
                    {
                        txtpreamt.Text = "0";
                        txtpreno.Text = "0";
                        txtprenoofdys.Text = "0";
                    }


                    // postpone
                    if (cmbpostpon.SelectedValue == "0")
                    {
                        txtpostamt.Text = "0";
                        txtpostno.Text = "0";
                        txtpostnoofdys.Text = "0";
                    }

                    //cancellation
                    if (cmbcanc.SelectedValue == "0")
                    {
                        txtcanclamt.Text = "0";
                        txtcanclno.Text = "0";
                    }

                    # endregion

                    # region coversion to boolean
                    q = converttoboolean(cmbprepon.SelectedValue.ToString());
                    s = converttoboolean(cmbcanc.SelectedValue.ToString());
                    u = converttoboolean(cmbpostpon.SelectedValue.ToString());
                    # endregion

                    # region DATE CONVERTIONS AND SERVER DATE
                    //changing to 
                    txtfrmdate.Text = yearmonthdate(txtfrmdate.Text);
                    txttodate.Text = yearmonthdate(txttodate.Text);

                    //taking server time
                    DateTime dt = DateTime.Now;
                    String date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
                    # endregion

                    # region EDIT BUTTON FUNCTION

                    //fetching primary key of Reservation policy log table
                    temp1 = int.Parse(primarykey("rowno", "t_policy_reservation_log"));
                    temp = int.Parse(txtpolicyid.Text.ToString());
                    temp2 = int.Parse(primarykey("rowno", "t_policy_reserv_seasons_log"));


                    # region policy log table insertion

                    DataTable dttgrdselect = new DataTable();
                    dttgrdselect = (DataTable)ViewState["gridselection"];

                    try
                    {
                        userid = int.Parse(Session["userid"].ToString());
                    }
                    catch
                    {
                        userid = 0;
                    }

                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }

                    DateTime ReservationFrom1 = DateTime.Parse(dttgrdselect.Rows[0]["res_from"].ToString());
                    string ReservationFrom = ReservationFrom1.ToString("yyyy/MM/dd");
                    DateTime ReservationFrom2 = DateTime.Parse(dttgrdselect.Rows[0]["res_to"].ToString());
                    string ReservationTo = ReservationFrom2.ToString("yyyy/MM/dd");

                    // inserting into reservation policy log table
                    OdbcCommand cmdsaveRpolicylog = new OdbcCommand("CALL savedata(?,?)", con);
                    cmdsaveRpolicylog.CommandType = CommandType.StoredProcedure;
                    cmdsaveRpolicylog.Parameters.AddWithValue("tblname", "t_policy_reservation_log");
                    cmdsaveRpolicylog.Parameters.AddWithValue("val", "" + temp1 + "," + temp + ",'" + dttgrdselect.Rows[0]["res_type"].ToString() + "'," + int.Parse(dttgrdselect.Rows[0]["amount_res"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_res_max"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_res_min"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_res_maxstay"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["is_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["amount_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["count_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["is_cancel"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["amount_cancel"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["count_cancel"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["is_postpone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["amount_postpone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_postpone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["count_postpone"].ToString()) + ",'" + ReservationFrom + "','" + ReservationTo + "'," + 0 + "," + userid + ",'" + date.ToString() + "'");
                    cmdsaveRpolicylog.ExecuteNonQuery();
                    con.Close();

                    # endregion

                    # region Policy table update
                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }
                    OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);
                    cmdupdte.CommandType = CommandType.StoredProcedure;
                    cmdupdte.Parameters.AddWithValue("tablename", "t_policy_reservation");
                    cmdupdte.Parameters.AddWithValue("valu", "res_type='" + cmbtype.SelectedValue.ToString() + "',amount_res= " + int.Parse(txtRCamount.Text.ToString()) + ",day_res_max=" + int.Parse(txtmaxdays.Text.ToString()) + ",day_res_min= " + int.Parse(txtmindays.Text.ToString()) + ",day_res_maxstay=" + int.Parse(txtmaxstay.Text.ToString()) + ",is_prepone= " + q + ",amount_prepone= " + int.Parse(txtpreamt.Text.ToString()) + ",day_prepone=" + int.Parse(txtprenoofdys.Text.ToString()) + ",count_prepone= " + int.Parse(txtpreno.Text.ToString()) + ",is_cancel= " + s + ",amount_cancel=" + int.Parse(txtcanclamt.Text.ToString()) + ",count_cancel= " + int.Parse(txtcanclno.Text.ToString()) + ",is_postpone=" + u + ",amount_postpone=" + int.Parse(txtpostamt.Text.ToString()) + ",day_postpone= " + int.Parse(txtpostnoofdys.Text.ToString()) + ",count_postpone= " + int.Parse(txtpostno.Text.ToString()) + ",res_from='" + txtfrmdate.Text.ToString() + "',res_to= '" + txttodate.Text.ToString() + "', rowstatus=" + 1 + ",updatedby=" + userid + ",updateddate= '" + date.ToString() + "'");
                    cmdupdte.Parameters.AddWithValue("convariable", "res_policy_id= " + policyid + "");
                    cmdupdte.ExecuteNonQuery();
                    con.Close();
                    # endregion

                    # region SEASON TABLE  old entry status deleting
                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }
                    OdbcCommand cmdpk4 = new OdbcCommand("select * from t_policy_reserv_seasons where  res_policy_id= " + temp + " and rowstatus <> " + 2 + " ", con);
                    OdbcDataReader rdseason4 = cmdpk4.ExecuteReader();

                    while (rdseason4.Read())
                    {
                        // season table status delete
                        OdbcCommand cmdupdteseason = new OdbcCommand("CALL updatedata(?,?,?)", con);
                        cmdupdteseason.CommandType = CommandType.StoredProcedure;
                        cmdupdteseason.Parameters.AddWithValue("tablename", "t_policy_reserv_seasons");
                        cmdupdteseason.Parameters.AddWithValue("valu", "rowstatus=" + 2 + "");
                        cmdupdteseason.Parameters.AddWithValue("convariable", "res_season_id= '" + int.Parse(rdseason4["res_season_id"].ToString()) + "'");
                        cmdupdteseason.ExecuteNonQuery();
                    }
                    rdseason4.Close();
                    con.Close();

                    # endregion

                    # region Policy season table and log table insertion new selection

                    // primary key of season table             
                    temp2 = int.Parse(primarykey("res_season_id", "t_policy_reserv_seasons"));
                    // primary key of season table log            
                    temp3 = int.Parse(primarykey("rowno", "t_policy_reserv_seasons_log"));

                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }


                    DataTable dttgrdselect1 = new DataTable();
                    dttgrdselect1 = (DataTable)ViewState["gridselection1"];
                    int count1 = int.Parse((dttgrdselect1.Rows.Count).ToString());
                    int ii = 0;
                    while (ii < count1)
                    {
                        OdbcCommand cmdsaveRSpolicylog = new OdbcCommand("CALL savedata(?,?)", con);
                        cmdsaveRSpolicylog.CommandType = CommandType.StoredProcedure;
                        cmdsaveRSpolicylog.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons_log");
                        cmdsaveRSpolicylog.Parameters.AddWithValue("val", "" + temp3 + "," + int.Parse(dttgrdselect1.Rows[ii]["res_season_id"].ToString()) + "," + int.Parse(dttgrdselect1.Rows[ii]["res_policy_id"].ToString()) + "," + int.Parse(dttgrdselect1.Rows[ii]["season_id"].ToString()) + "," + userid + ",'" + date.ToString() + "'," + 0 + "");
                        cmdsaveRSpolicylog.ExecuteNonQuery();

                        temp3++;
                        ii++;
                    }


                    for (int i = 0; i < lstseason.Items.Count; i++)
                    {
                        if (lstseason.Items[i].Selected == true)
                        {

                            string seson = lstseason.Items[i].Text.ToString();
                            OdbcCommand cmdseason11 = new OdbcCommand("select season_sub_id from m_sub_season where seasonname='" + seson + "' and rowstatus<>" + 2 + "", con);
                            OdbcDataReader sessid = cmdseason11.ExecuteReader();
                            if (sessid.Read())
                            {
                                sesid = int.Parse(sessid["season_sub_id"].ToString());
                            }
                        
                            // inserting into reservation policy season table

                            OdbcCommand cmdsaveRPseason = new OdbcCommand("CALL savedata(?,?)", con);
                            cmdsaveRPseason.CommandType = CommandType.StoredProcedure;
                            cmdsaveRPseason.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons");
                            cmdsaveRPseason.Parameters.AddWithValue("val", "" + temp2 + "," + policyid + "," + sesid + "," + userid + ",'" + date.ToString() + "'," + 0 + "," + userid + ",'" + date.ToString() + "'");
                            cmdsaveRPseason.ExecuteNonQuery();

                            temp2++;

                            // inserting into reservation policy season log table
                            // season log table insertion


                        }

                    }
                    con.Close();

                    # endregion

                    # endregion

                    clear();
                    txtpolicyid.Text = primarykey("res_policy_id", "t_policy_reservation");
                    grid_load();
                    okmessage("Tsunami ARMS - Information", "Policy was edited succesfully."); //message box
                }
                #endregion
            }
            catch
            {
                #region error message
                okmessage("Tsunami ARMS - Information", "Error found in editing Polcy."); //message box
                #endregion
            }
        }
        else if (ViewState["action"].ToString() == "edit-previous policy2")
        {
            try
            {
                #region edit-previous policy1

                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }

                policyid = Convert.ToInt32(gdrespolicy.DataKeys[gdrespolicy.SelectedRow.RowIndex].Value.ToString());
                from = yearmonthdate(txtfrmdate.Text);
                to = yearmonthdate(txttodate.Text);
                DateTime tempfrom = DateTime.Parse(from);
                DateTime tempto = DateTime.Parse(to);

             

                # region VALIDATING NULL VALUES

                //prepone
                if (cmbprepon.SelectedValue == "0")
                {
                    txtpreamt.Text = "0";
                    txtpreno.Text = "0";
                    txtprenoofdys.Text = "0";
                }


                // postpone
                if (cmbpostpon.SelectedValue == "0")
                {
                    txtpostamt.Text = "0";
                    txtpostno.Text = "0";
                    txtpostnoofdys.Text = "0";
                }

                //cancellation
                if (cmbcanc.SelectedValue == "0")
                {
                    txtcanclamt.Text = "0";
                    txtcanclno.Text = "0";
                }

                # endregion

                # region coversion to boolean
                q = converttoboolean(cmbprepon.SelectedValue.ToString());
                s = converttoboolean(cmbcanc.SelectedValue.ToString());
                u = converttoboolean(cmbpostpon.SelectedValue.ToString());
                # endregion

                # region DATE CONVERTIONS AND SERVER DATE
                //changing to 
                txtfrmdate.Text = yearmonthdate(txtfrmdate.Text);
                txttodate.Text = yearmonthdate(txttodate.Text);

                //taking server time
                DateTime dt = DateTime.Now;
                String date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
                # endregion

                # region EDIT BUTTON FUNCTION

                //fetching primary key of Reservation policy log table
                temp1 = int.Parse(primarykey("rowno", "t_policy_reservation_log"));
                temp = int.Parse(txtpolicyid.Text.ToString());
                temp2 = int.Parse(primarykey("rowno", "t_policy_reserv_seasons_log"));


                # region policy log table insertion

                DataTable dttgrdselect = new DataTable();
                dttgrdselect = (DataTable)ViewState["gridselection"];

                try
                {
                    userid = int.Parse(Session["userid"].ToString());
                }
                catch
                {
                    userid = 0;
                }

                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }

                DateTime ReservationFrom1 = DateTime.Parse(dttgrdselect.Rows[0]["res_from"].ToString());
                string ReservationFrom = ReservationFrom1.ToString("yyyy/MM/dd");
                DateTime ReservationFrom2 = DateTime.Parse(dttgrdselect.Rows[0]["res_to"].ToString());
                string ReservationTo = ReservationFrom2.ToString("yyyy/MM/dd");

                // inserting into reservation policy log table
                OdbcCommand cmdsaveRpolicylog = new OdbcCommand("CALL savedata(?,?)", con);
                cmdsaveRpolicylog.CommandType = CommandType.StoredProcedure;
                cmdsaveRpolicylog.Parameters.AddWithValue("tblname", "t_policy_reservation_log");
                cmdsaveRpolicylog.Parameters.AddWithValue("val", "" + temp1 + "," + temp + ",'" + dttgrdselect.Rows[0]["res_type"].ToString() + "'," + int.Parse(dttgrdselect.Rows[0]["amount_res"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_res_max"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_res_min"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_res_maxstay"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["is_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["amount_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["count_prepone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["is_cancel"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["amount_cancel"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["count_cancel"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["is_postpone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["amount_postpone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["day_postpone"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["count_postpone"].ToString()) + ",'" + ReservationFrom + "','" + ReservationTo + "'," + 0 + "," + userid + ",'" + date.ToString() + "'");
                cmdsaveRpolicylog.ExecuteNonQuery();
                con.Close();

                # endregion

                # region Policy table update
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
                OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);
                cmdupdte.CommandType = CommandType.StoredProcedure;
                cmdupdte.Parameters.AddWithValue("tablename", "t_policy_reservation");
                cmdupdte.Parameters.AddWithValue("valu", "res_type='" + cmbtype.SelectedValue.ToString() + "',amount_res= " + int.Parse(txtRCamount.Text.ToString()) + ",day_res_max=" + int.Parse(txtmaxdays.Text.ToString()) + ",day_res_min= " + int.Parse(txtmindays.Text.ToString()) + ",day_res_maxstay=" + int.Parse(txtmaxstay.Text.ToString()) + ",is_prepone= " + q + ",amount_prepone= " + int.Parse(txtpreamt.Text.ToString()) + ",day_prepone=" + int.Parse(txtprenoofdys.Text.ToString()) + ",count_prepone= " + int.Parse(txtpreno.Text.ToString()) + ",is_cancel= " + s + ",amount_cancel=" + int.Parse(txtcanclamt.Text.ToString()) + ",count_cancel= " + int.Parse(txtcanclno.Text.ToString()) + ",is_postpone=" + u + ",amount_postpone=" + int.Parse(txtpostamt.Text.ToString()) + ",day_postpone= " + int.Parse(txtpostnoofdys.Text.ToString()) + ",count_postpone= " + int.Parse(txtpostno.Text.ToString()) + ",res_from='" + txtfrmdate.Text.ToString() + "',res_to= '" + txttodate.Text.ToString() + "', rowstatus=" + 1 + ",updatedby=" + userid + ",updateddate= '" + date.ToString() + "'");
                cmdupdte.Parameters.AddWithValue("convariable", "res_policy_id= " + policyid + "");
                cmdupdte.ExecuteNonQuery();
                con.Close();
                # endregion

                # region SEASON TABLE  old entry status deleting
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
                OdbcCommand cmdpk4 = new OdbcCommand("select * from t_policy_reserv_seasons where  res_policy_id= " + temp + " and rowstatus <> " + 2 + " ", con);
                OdbcDataReader rdseason4 = cmdpk4.ExecuteReader();

                while (rdseason4.Read())
                {
                    // season table status delete
                    OdbcCommand cmdupdteseason = new OdbcCommand("CALL updatedata(?,?,?)", con);
                    cmdupdteseason.CommandType = CommandType.StoredProcedure;
                    cmdupdteseason.Parameters.AddWithValue("tablename", "t_policy_reserv_seasons");
                    cmdupdteseason.Parameters.AddWithValue("valu", "rowstatus=" + 2 + "");
                    cmdupdteseason.Parameters.AddWithValue("convariable", "res_season_id= '" + int.Parse(rdseason4["res_season_id"].ToString()) + "'");
                    cmdupdteseason.ExecuteNonQuery();
                }
                rdseason4.Close();
                con.Close();

                # endregion

                # region Policy season table and log table insertion new selection

                // primary key of season table             
                temp2 = int.Parse(primarykey("res_season_id", "t_policy_reserv_seasons"));
                // primary key of season table log            
                temp3 = int.Parse(primarykey("rowno", "t_policy_reserv_seasons_log"));

                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }


                DataTable dttgrdselect1 = new DataTable();
                dttgrdselect1 = (DataTable)ViewState["gridselection1"];
                int count1 = int.Parse((dttgrdselect1.Rows.Count).ToString());
                int ii = 0;
                while (ii < count1)
                {
                    OdbcCommand cmdsaveRSpolicylog = new OdbcCommand("CALL savedata(?,?)", con);
                    cmdsaveRSpolicylog.CommandType = CommandType.StoredProcedure;
                    cmdsaveRSpolicylog.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons_log");
                    cmdsaveRSpolicylog.Parameters.AddWithValue("val", "" + temp3 + "," + int.Parse(dttgrdselect1.Rows[ii]["res_season_id"].ToString()) + "," + int.Parse(dttgrdselect1.Rows[ii]["res_policy_id"].ToString()) + "," + int.Parse(dttgrdselect1.Rows[ii]["season_id"].ToString()) + "," + userid + ",'" + date.ToString() + "'," + 0 + "");
                    cmdsaveRSpolicylog.ExecuteNonQuery();

                    temp3++;
                    ii++;
                }


                for (int i = 0; i < lstseason.Items.Count; i++)
                {
                    if (lstseason.Items[i].Selected == true)
                    {

                        string seson = lstseason.Items[i].Text.ToString();
                        OdbcCommand cmdseason11 = new OdbcCommand("select season_sub_id from m_sub_season where seasonname='" + seson + "' and rowstatus<>" + 2 + "", con);
                        OdbcDataReader sessid = cmdseason11.ExecuteReader();
                        if (sessid.Read())
                        {
                            sesid = int.Parse(sessid["season_sub_id"].ToString());
                        }
                      
                        // inserting into reservation policy season table

                        OdbcCommand cmdsaveRPseason = new OdbcCommand("CALL savedata(?,?)", con);
                        cmdsaveRPseason.CommandType = CommandType.StoredProcedure;
                        cmdsaveRPseason.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons");
                        cmdsaveRPseason.Parameters.AddWithValue("val", "" + temp2 + "," + policyid + "," + sesid + "," + userid + ",'" + date.ToString() + "'," + 0 + "," + userid + ",'" + date.ToString() + "'");
                        cmdsaveRPseason.ExecuteNonQuery();

                        temp2++;

                        // inserting into reservation policy season log table
                        // season log table insertion


                    }

                }
                con.Close();

                # endregion

                # endregion



                clear();
                txtpolicyid.Text = primarykey("res_policy_id", "t_policy_reservation");
                grid_load();

                
                okmessage("Tsunami ARMS - Information", "Policy was edited succesfully."); //message box

                #endregion
            }
            catch
            {
                #region error message
                okmessage("Tsunami ARMS - Information", "Error found in editing Polcy."); //message box
                #endregion
            }
        }
    }
    #endregion

    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    protected void txtfrmdate_TextChanged(object sender, EventArgs e)
    {

    }
    protected void cmbpostpon_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbpostpon.SelectedValue == "1")
        {
            txtpostnoofdys.Visible = true;
            txtpostno.Visible = true;
            txtpostamt.Visible = true;
            lblpostno.Visible = true;
            lblpostnoofdays.Visible = true;
            lblRCpostAmt.Visible = true;
            revpostamt.Visible = true;
            revpostno.Visible = true;
            revpostnoofdays.Visible = true;
            txtpostnoofdys.Text = "0";
            txtpostno.Text = "0";
            txtpostamt.Text = "0";
        }
        else
        {
            txtpostnoofdys.Visible = false;
            txtpostno.Visible = false;
            txtpostamt.Visible = false;
            txtpostnoofdys.Text = "0";
            txtpostno.Text = "0";
            txtpostamt.Text = "0";
            lblpostno.Visible = false;
            lblpostnoofdays.Visible = false;
            lblRCpostAmt.Visible = false;
            revpostamt.Visible = false;
            revpostno.Visible = false;
            revpostnoofdays.Visible = false;
        }
        this.ScriptManager1.SetFocus(cmbpostpon);
    }
    protected void cmbprepon_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbprepon.SelectedValue == "1")
        {
            txtprenoofdys.Visible = true;
            txtpreno.Visible = true;
            txtpreamt.Visible = true;
            lblpreamt.Visible = true;
            lblpreno.Visible = true;
            lblprenoofdays.Visible = true;
            revpreamt.Visible = true;
            revpreno.Visible = true;
            revprenoofdays.Visible = true;
            txtprenoofdys.Text = "0";
            txtpreno.Text = "0";
            txtpreamt.Text = "0";
        }
        else
        {
            txtprenoofdys.Visible = false;
            txtpreno.Visible = false;
            txtpreamt.Visible = false;
            txtprenoofdys.Text = "0";
            txtpreno.Text = "0";
            txtpreamt.Text = "0";
            lblpreamt.Visible = false;
            lblpreno.Visible = false;
            lblprenoofdays.Visible = false;
            revpreamt.Visible = false;
            revpreno.Visible = false;
            revprenoofdays.Visible = false; ;
        }
        this.ScriptManager1.SetFocus(cmbprepon);
    }
    protected void cmbcanc_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbcanc.SelectedValue == "1")
        {

            txtcanclno.Visible = true;
            txtcanclamt.Visible = true;
            txtcanclno.Text = "0";
            txtcanclamt.Text = "0";
            lblcancelcharge.Visible = true;
            lblcancelno.Visible = true;
            revcancelamt.Visible = true;
            revcancelno.Visible = true;
        }
        else
        {
            txtcanclno.Text = "0";
            txtcanclamt.Text = "0";
            txtcanclno.Visible = false;
            txtcanclamt.Visible = false;
            lblcancelcharge.Visible = false;
            lblcancelno.Visible = false;
            revcancelamt.Visible = false;
            revcancelno.Visible = false;
        }
        this.ScriptManager1.SetFocus(cmbcanc);
    }
    public bool commbo(string s)
    {
        #region  empty Boolean
        bool p = false;
        if (s == "Yes")
        {
            p = true;
        }
        else if (s == "No")
        {
            p = false;
        }
        else if (s == "")
        {
            p = false;
        }
        return p;
        #endregion
    }
}// end of code
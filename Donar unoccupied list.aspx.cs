using System;
using System.Data;
using System.Data.Odbc;
using System.Net;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;

public partial class Donar_unoccupied_list : System.Web.UI.Page
{
    commonClass objcls = new commonClass();
    int userid;
    DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Title = "Tsunami ARMS - Donar unoccupied list";
            try
            {
                userid = int.Parse(Session["userid"].ToString());
                SetFocus(cmbReportpass);
            }
            catch
            {
                userid = 1;
                Session["userid"] = userid.ToString();
            }
        }       

    }

    protected void btnview_Click(object sender, EventArgs e)
    {
        view();
            //Session["dataval"] = dt;
            
           
            
    }

    private void view()
    {        
        string str1 = objcls.yearmonthdate(txtreportdatefrom.Text);
        string str2 = objcls.yearmonthdate(txtreportdateto.Text);
        OdbcCommand cmd31 = new OdbcCommand();
        cmd31.Parameters.AddWithValue("tblname", " t_roomreservation,t_roomallocation,m_room,m_sub_building ");
        cmd31.Parameters.AddWithValue("attribute", "t_roomreservation.room_id,t_roomreservation.reserve_no,t_roomreservation.place,t_roomreservation.reservedate 'Reserve from',t_roomreservation.expvacdate 'Reserve To',m_sub_building.buildingname 'Building',m_room.roomno 'Room No',case reserve_mode when 'tdb' then 'TDB Res' when 'Donor Free' then 'Donor free' when 'Donor Paid' then 'Donor paid' when 'General' then 'General' END as 'Customer Type',t_roomreservation.swaminame,t_roomallocation.mobile");
        if (cmbReportpass.SelectedValue == "-1")
        {
            //cmd31.Parameters.AddWithValue("conditionv", " status_reserve='0' and t_roomreservation.reserve_no=t_roomallocation.reserve_id and t_roomallocation.roomstatus!=2 and t_roomreservation.room_id=m_room.room_id and m_room.build_id=m_sub_building.build_id and date(reservedate) >= '" + str1.ToString() + "' and date(reservedate) <  '" + str2.ToString() + "' and t_roomreservation.room_id=t_roomreservation.room_id order by t_roomallocation.room_id  asc");
            cmd31.Parameters.AddWithValue("conditionv", " status_reserve='0' and t_roomreservation.reserve_no=t_roomallocation.reserve_id and t_roomreservation.room_id=m_room.room_id and m_room.build_id=m_sub_building.build_id and date(reservedate) >= '" + str1.ToString() + "' and date(reservedate) <  '" + str2.ToString() + "' and t_roomreservation.room_id=t_roomreservation.room_id AND m_room.room_id NOT IN (SELECT t_roomallocation.room_id FROM t_roomallocation WHERE t_roomallocation.allocdate BETWEEN '" + str1.ToString() + "' AND  '" + str2.ToString() + "' AND t_roomallocation.exp_vecatedate BETWEEN '" + str1.ToString() + "' AND '" + str2.ToString() + "' )ORDER BY t_roomreservation.room_id ASC");
        }
        else
        {
            cmd31.Parameters.AddWithValue("conditionv", " status_reserve='0' and t_roomreservation.reserve_no=t_roomallocation.reserve_id and t_roomreservation.room_id=m_room.room_id and m_room.build_id=m_sub_building.build_id and reserve_mode='" + cmbReportpass.SelectedValue + "' and date(reservedate) >= '" + str1.ToString() + "' and date(reservedate) <  '" + str2.ToString() + "' and t_roomreservation.room_id=t_roomreservation.room_id AND m_room.room_id NOT IN (SELECT t_roomallocation.room_id FROM t_roomallocation WHERE t_roomallocation.allocdate BETWEEN '" + str1.ToString() + "' AND  '" + str2.ToString() + "' AND t_roomallocation.exp_vecatedate BETWEEN '" + str1.ToString() + "' AND '" + str2.ToString() + "' )ORDER BY t_roomreservation.room_id ASC");
        }       
        DataTable dtrr = new DataTable();
        dtrr = objcls.SpDtTbl("call selectcond(?,?,?)", cmd31);
        dt = dtrr;
        
        if (dt.Rows.Count > 0)
        {
            gv_details.DataSource = dt;
            gv_details.DataBind();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoData();", true);
        }
    }
    protected void txtreportdatefrom_TextChanged(object sender, EventArgs e)
    {
        String rtodate = objcls.yearmonthdate(txtreportdatefrom.Text);
        DateTime rtodate1 = DateTime.Parse(rtodate);
        rtodate1 = rtodate1.AddDays(1);
        txtreportdateto.Text = rtodate1.ToString("dd-MM-yyyy");
    }
    protected void gv_details_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = gv_details.SelectedIndex;
        //cmbReportpass.SelectedValue=gv_details.Rows[i].Cells[7].Text;         
        Session["reserve_no"]  = gv_details.Rows[i].Cells[1].Text;
    }
    protected void gv_details_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gv_details, "Select$" + e.Row.RowIndex);
            //e.Row.Cells[0].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView header = (GridView)sender;
            GridViewRow gvr = new GridViewRow(0, 0,
                DataControlRowType.Header,
                DataControlRowState.Insert);
            TableCell tCell = new TableCell();
            tCell.ColumnSpan = 15;
            tCell.HorizontalAlign = HorizontalAlign.Center;
            gvr.Cells.Add(tCell);
            Table tbl = gv_details.Controls[0] as Table;
            if (tbl != null)
            {
                tbl.Rows.AddAt(0, gvr);
            }
        }

    }
    protected void gv_details_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate) && (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header))
        {
           // e.Row.Cells[0].Visible = false;
        }


    }
    protected void btnrelease_Click(object sender, EventArgs e)
    {
        int i = gv_details.SelectedIndex;
        if (i == -1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
        }
        else
        {
            string id = Session["reserve_no"].ToString();            
            string tr = @"update t_roomreservation set status_reserve=3 where reserve_no='" + id + "'";
            objcls.exeNonQuery(tr);
            string gs = @"update m_room set roomstatus=1 where room_id='"+ gv_details.Rows[i].Cells[0].Text +"'";
            objcls.exeNonQuery(gs);
            view();
            //ds = Session["dataval"].ToString();           
            string ee = @"SELECT swaminame,inmates_email FROM t_roomreservation_generaltdbtemp WHERE reserve_no='" + id + "'";
            DataTable dt_name = objcls.DtTbl(ee);
            string nn = dt_name.Rows[0][0].ToString();
            string ff = dt_name.Rows[0][1].ToString();
            //string nn = txtname.Text;


            string to = "info@tsunamisoftware.co.in";
            string pdfFilePath = Server.MapPath(".");
            //objcls.Email(to, "tsunami123", ee, "Your Registration is confirmed! ", "Sir " + nn + ", \n Please take the print of this confirmation letter for your future purpose");

            //var smtp = new System.Net.Mail.SmtpClient();
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("test@tsunamisoftware.co.in");
            msg.To.Add(ff);
            msg.Subject = "Response for your registration!";
            msg.Body = "Sir " + nn + ", \n \n \t\t" + "Due to reservation time gets end,your room is cancelled!.\n";
            SmtpClient Sc = new SmtpClient("smtp.gmail.com");
            SmtpClient Sp = new SmtpClient("smtp.yahoo.com");
            SmtpClient Sh = new SmtpClient("smtp.hotmail.com");
            SmtpClient Sr = new SmtpClient("smtp.rediff.com");
            Sc.Port = 587;
            Sc.EnableSsl = true;
            Sc.UseDefaultCredentials = true;
            Sc.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            Sc.Credentials = new NetworkCredential("test@tsunamisoftware.co.in", "test2345");
            Sp.Credentials = new NetworkCredential("test@tsunamisoftware.co.in", "test2345");
            Sh.Credentials = new NetworkCredential("test@tsunamisoftware.co.in", "test2345");
            Sr.Credentials = new NetworkCredential("test@tsunamisoftware.co.in", "test2345");
            Sc.Send(msg);

            view();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRelease();", true); ;
        }
    } 
}
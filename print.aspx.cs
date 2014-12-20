using System;
using System.Net;
public partial class print : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string pdfFilePath = "";
            if (Session["head"] != null && Session["head"] != "")
            {

                string head = Session["head"].ToString();                
                 pdfFilePath = Server.MapPath(".") + "/pdf/" + head;
                 Session["head"] = "";
            }
            else
            {
               pdfFilePath = Server.MapPath(".") + "/pdf/" + Request.QueryString["reportname"].ToString();
            }
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(pdfFilePath);
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(buffer);
            Response.Flush();
            Response.Close();
            
           // document.window.print;

        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Room Reservation.aspx");
    }
}

using System;
using System.Net;

public partial class print2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string name = Session["head"].ToString();
            //string pdfFilePath = Server.MapPath(".") + "/pdf/"+ Request.QueryString["reportname"].ToString();
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + name;
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(pdfFilePath);

            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-length", buffer.Length.ToString());
            //Response.BinaryWrite(buffer);
            //Response.Flush();
            //Response.Close();

            Random r = new Random();
            string PopUpWindowPage = "print2.aspx?reportname=" + name + "&Title=AdvancedReceipt";

            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
    }
}
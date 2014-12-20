using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using clsDAL;

public partial class Payment_reconcilation : System.Web.UI.Page
{
    private commonClass objcls = new commonClass();
    private OdbcConnection con = new OdbcConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            string payment = @"SELECT payment_id,payment_mode FROM payment_mode";
            DataTable dt_payment = objcls.DtTbl(payment);
            if(dt_payment.Rows.Count > 0)
            {
                DataRow dr = dt_payment.NewRow();
                dr["payment_id"] = "-1";
                dr["payment_mode"] = "--Select--";
                dt_payment.Rows.InsertAt(dr,0);
                ddlMode.DataSource = dt_payment;
                ddlMode.DataBind();
            }
            else
            {
                ddlMode.DataSource = null;
                ddlMode.DataBind();
                okmessage("Tsunami ARMS - Warning", "Payment mode not defined");
                
            }
        }
    }
    protected void gvDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
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
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvDetails, "Select$" + e.Row.RowIndex); 
            }

        }
        catch
        {

        }
    }
    protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        string resno = gvDetails.SelectedRow.Cells[1].Text;
        string details = @"SELECT reserve_no,swaminame,payment_mode,dd_no,dd_date,bank,total_charge FROM t_roomreservation_generaltdbtemp WHERE reserve_no='" + resno + "'";
        DataTable dt_details = objcls.DtTbl(details);
        if (dt_details.Rows.Count > 0)
        {
            txtResNo.Text = dt_details.Rows[0]["reserve_no"].ToString();
            txtName.Text = dt_details.Rows[0]["swaminame"].ToString();
            if (dt_details.Rows[0]["payment_mode"].ToString() != "" && dt_details.Rows[0]["payment_mode"].ToString() != null)
                ddlMode.SelectedValue = dt_details.Rows[0]["payment_mode"].ToString();
            txtDDno.Text = dt_details.Rows[0]["dd_no"].ToString();
            txtDDdate.Text = dt_details.Rows[0]["dd_date"].ToString();
            txtBank.Text = dt_details.Rows[0]["bank"].ToString();
            txtAmount.Text = dt_details.Rows[0]["total_charge"].ToString();

        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Problem in loading values");
        }

    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
    }
    #region OK Message

    public void okmessage(string head, string message)
    {
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender1.Show();
    }
    #endregion
    private void load()
    {
            string type = "";
            if(ddlReservation.SelectedValue=="0")
            {
                type = "General";
              
            }
            else if(ddlReservation.SelectedValue=="1")
            {
                type = "TDB";
            }
            else if(ddlReservation.SelectedValue=="2")
            {
                type = "Donor Paid";
            }
            else
            {
                type = "Donor Free";
            }


            string details = @"SELECT reserve_no as 'Reserve no',reservedate as 'Reserve date',payment_mode.payment_mode as 'Payment type',dd_no as 'DD No',dd_date as 'DD date',bank as 'Bank',total_charge as 'Amount',CASE  WHEN payment_status=0 THEN 'Committed' WHEN payment_status=1 THEN 'Cancelled' WHEN payment_status=3 THEN 'DD Received' END AS 'Status'
FROM t_roomreservation_generaltdbtemp 
LEFT JOIN payment_mode ON payment_mode.payment_id=t_roomreservation_generaltdbtemp.payment_mode
WHERE payment_status !=2  and reserve_mode='"+type+"'";
            DataTable dt_details = objcls.DtTbl(details);
            if (dt_details.Rows.Count > 0)
            {
                gvDetails.DataSource = dt_details;
                gvDetails.DataBind();
            }
            else
            {
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                okmessage("Tsunami ARMS - Warning", "No pending reservation");
            }
        
    }
    private void clear()
    {
        txtAmount.Text = "";
        txtAmount2.Text = "";
        txtBank.Text = "";
        txtDDdate.Text = "";
        txtDDno.Text = "";
        txtName.Text = "";
        txtResNo.Text = "";
        txtTrans.Text = "";
        ddlMode.SelectedIndex = -1;
        ddlReservation.SelectedIndex = -1;
        pnlDetails.Visible = false;
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        if(ddlReservation.SelectedIndex != -1 && ddlReservation.SelectedValue != "-1")
        {
            pnlDetails.Visible = true;
            load();
        }
        else
        {
            pnlDetails.Visible = false;
            okmessage("Tsunami ARMS - Warning", "Select the Reservation type");
        }
    }
    protected void btnDD_Click(object sender, EventArgs e)
    {
        if (gvDetails.SelectedIndex != -1 && txtAmount2.Text != "")
        {
            string resno = gvDetails.SelectedRow.Cells[1].Text;
            string update = @"UPDATE t_roomreservation_generaltdbtemp SET payment_status=3 WHERE reserve_no='"+resno+"'";
            int i = objcls.exeNonQuery(update);
            if (i == 1)
            {
                okmessage("Tsunami ARMS - Warning", "Updated successfully");
                load();
                pdf_chellan();
                clear();

            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Error in updation");
            }
        }
        else
        {
            if (gvDetails.SelectedIndex == -1)
            {
                okmessage("Tsunami ARMS - Warning", "Select the value from grid");
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Enter the amount received");
            }
                 
        }
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        if (gvDetails.SelectedIndex != -1 && txtAmount2.Text != "" && txtTrans.Text !="")
        {
            string resno = gvDetails.SelectedRow.Cells[1].Text;
            string update = @"UPDATE t_roomreservation_generaltdbtemp SET payment_status=2,bank_transno='" + txtTrans.Text + "',dd_amt_received='" + txtAmount2.Text +"' WHERE reserve_no='" + resno + "'";
            int i = objcls.exeNonQuery(update);
            if (i == 1)
            {
                okmessage("Tsunami ARMS - Warning", "Confirmed successfully");
                load();
                clear();
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Error in confirmation");
                
            }
        }
        else
        {
            if (gvDetails.SelectedIndex == -1)
            {
                okmessage("Tsunami ARMS - Warning", "Select the value from grid");
            }
            else if(txtAmount2.Text == "")
            {
                okmessage("Tsunami ARMS - Warning", "Enter the amount received");
            }
            else if(txtTrans.Text =="")
            {
                okmessage("Tsunami ARMS - Warning", "Enter the Bank trans no");
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (gvDetails.SelectedIndex != -1)
        {
            string resno = gvDetails.SelectedRow.Cells[1].Text;
            string update = @"UPDATE t_roomreservation_generaltdbtemp SET payment_status=1 WHERE reserve_no='"+resno+"'";
            int i = objcls.exeNonQuery(update);
            if (i == 1)
            {
                okmessage("Tsunami ARMS - Warning", "Cancelled successfully");
                load();
                clear();
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Error in cancellation");
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Select the value from grid");
        }
    }
    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate) && (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header))
        {
            //e.Row.Cells[1].Visible = false;
        }
    }
    private void pdf_chellan()
    {
        
    }
    protected void ddlReservation_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvDetails.DataSource = null;
        gvDetails.DataBind();
        pnlDetails.Visible = false;
    }
}
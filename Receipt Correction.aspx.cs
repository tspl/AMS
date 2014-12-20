using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using System.Data.Odbc;
using System.Data;

public partial class Receipt_Correction : System.Web.UI.Page
{
    commonClass objcls = new commonClass();
    OdbcConnection con = new OdbcConnection();
    static string strConnection;
    clsCommon obj = new clsCommon();
            
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string q_counter = "SELECT counter_id,counter_no FROM m_sub_counter ORDER BY counter_id ASC";
            DataTable dt_counter = objcls.DtTbl(q_counter);
            DataRow dr = dt_counter.NewRow();
            dr["counter_no"] = "--Select--";
            dr["counter_id"] = "-1";
            dt_counter.Rows.InsertAt(dr, 0);
            cmbCounter.DataSource = dt_counter;
            cmbCounter.DataBind();
            cmbCounter.Enabled = false;

            string q_status = "SELECT id,receipt_status FROM m_receipt_status WHERE rowstatus=0";
            DataTable dt_status = objcls.DtTbl(q_status);
            DataRow dr_status = dt_status.NewRow();
            dr_status["receipt_status"] = "--Select--";
            dr_status["id"] = "-1";
            dt_status.Rows.InsertAt(dr_status, 0);
            cmbStatus.DataSource = dt_status;
            cmbStatus.DataBind();

            strConnection = obj.ConnectionString();
        }
    }
    protected void btnCorrect_Click(object sender, EventArgs e)
    {
        if ((cmbCounter.SelectedValue != "-1") && (cmbStatus.SelectedValue != "-1") && (txtReceiptno.Text != ""))
        {
            string q_dayend = "SELECT DISTINCT DATE_FORMAT(dayend,'%d-%m-%Y') FROM t_roomallocation WHERE adv_recieptno=" + txtReceiptno.Text + " AND counter_id=" + cmbCounter.SelectedValue + "";
            string dayend = objcls.exeScalar(q_dayend);           
            if (dayend != "")
            {
                string dayendnew = objcls.yearmonthdate(dayend);
                string q_dayendcheck = "SELECT DATE_FORMAT(closedate_start,'%d-%m-%Y') AS closedate_start FROM  t_dayclosing WHERE daystatus='open' AND DATE_FORMAT(closedate_start,'%d-%m-%Y')='" + dayend + "'";
                string dayendcheck = objcls.exeScalar(q_dayendcheck);
                if (dayendcheck != "")
                {
                    int correctid = objcls.PK_exeSaclarInt("crct_id", "t_receiptcorrection");
                    correctid = correctid + 1;
                    OdbcTransaction odbTrans = null;
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.ConnectionString = strConnection;
                            con.Open();
                        }
                        odbTrans = con.BeginTransaction();
                        int receiptno = Convert.ToInt32(txtReceiptno.Text) + 1;
                        string q_receiptstatus = @"INSERT INTO t_receiptcorrection(crct_id,counter_no,recipt_no,crct_status,crct_date,dayend)
VALUES (" + correctid + "," + cmbCounter.SelectedValue + "," + txtReceiptno.Text + "," + cmbStatus.SelectedValue + ",now(),'" + dayendnew + "')";
                        OdbcCommand cmd1 = new OdbcCommand(q_receiptstatus, con);
                        cmd1.Transaction = odbTrans;
                        cmd1.ExecuteNonQuery();
                        string q_correct = @"UPDATE t_roomallocation SET adv_recieptno=adv_recieptno+1 WHERE adv_recieptno>=" + txtReceiptno.Text + " AND counter_id=" + cmbCounter.SelectedValue + "";
                        OdbcCommand cmd2 = new OdbcCommand(q_correct, con);
                        cmd2.Transaction = odbTrans;
                        cmd2.ExecuteNonQuery();
                        odbTrans.Commit();
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowUpdated();", true);
                        clear();
                    }
                    catch
                    {
                        odbTrans.Rollback();
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowError();", true);
                        clear();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showdate();", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
            }
        }
    }

    private void clear()
    {
        txtReceiptno.Text = "";
        cmbCounter.SelectedValue = "-1";
        cmbStatus.SelectedValue = "-1";
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        string q_view = "SELECT @COUNT:=@COUNT+1 AS 'Sl.No', DATE_FORMAT(t_receiptcorrection.crct_date,'%d-%m-%Y') AS 'Date',m_receipt_status.receipt_status,m_sub_counter.counter_no,t_receiptcorrection.recipt_no FROM t_receiptcorrection,m_receipt_status,m_sub_counter,(SELECT @COUNT:=0) AS COUNT WHERE  t_receiptcorrection.crct_status=m_receipt_status.id AND m_sub_counter.counter_id=t_receiptcorrection.counter_no";
        DataTable dt_view = objcls.DtTbl(q_view);
        if (dt_view.Rows.Count > 0)
        {
            gvView.DataSource = dt_view;
            gvView.DataBind();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoData();", true);
        }
    }
    protected void txtReceiptno_TextChanged(object sender, EventArgs e)
    {
        string q_counterid = "select counter_id from t_roomallocation where adv_recieptno=" + txtReceiptno.Text + "";
        string counterid = objcls.exeScalar(q_counterid);
        if (counterid != "")
        {
            cmbCounter.SelectedValue = counterid;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoData();", true);
        }
    }


}
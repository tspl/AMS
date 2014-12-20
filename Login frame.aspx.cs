/////=======================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Accomodation
// Screen Name      :      Login frame-Tsunami ARMS
// Form Name        :      Login frame.aspx
// Purpose          :      Login page

// Created by       :
// Created On       :      20-Nov-2010
// Last Modified    :      26-Nov-2010
//---------------------------------------------------------------------------
// SL.NO    Date             Modified By                 Reason
//---------------------------------------------------------------------------
//  1       31-Jan-2011    	    Sadhik                   Optimization
//  2       28-Aug-2013         Magesh M                 Counter checking    
//---------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.Odbc;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using clsDAL;

public partial class login2 : System.Web.UI.Page
{
    #region DECLARATION
    private commonClass objDAL = new commonClass();
    private string desgination, office, defaultformname;
    private int h, defaultform;
    #endregion DECLARATION

    #region DECRYPT
    public string base64Decode(string sData)
    {
        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();

        System.Text.Decoder utf8Decode = encoder.GetDecoder();

        byte[] todecode_byte = Convert.FromBase64String(sData);

        int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);

        char[] decoded_char = new char[charCount];

        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);

        string result = new String(decoded_char);

        return result;
    }
    #endregion DECRYPT

    #region PAGE LOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime date = DateTime.Now;
        Title = "Tsunami ARMS Login Page";

        if (!IsPostBack)
        {
            Loginframe.Focus();
        }
    }
    #endregion PAGE LOAD

    #region LOGIN FRAME CLICK
    protected void Loginframe_Authenticate(object sender, AuthenticateEventArgs e)
    {
        Loginframe.Focus();
        authenticate();
    }
    #endregion LOGIN FRAME CLICK
    private void authenticate()
    {
        
        ViewState["check"] = (1).ToString();
        Session["logintime"] = DateTime.Now;
        string s = Loginframe.UserName;
        string psswd = Loginframe.Password;
        string userid;
        try
        {
            OdbcCommand login = new OdbcCommand();
            login.Parameters.AddWithValue("tblname", "m_user");
            login.Parameters.AddWithValue("attribute", "username,password,level,user_id,staff_id,defaultform");
            login.Parameters.AddWithValue("conditionv", "username='" + s + "' and rowstatus<>" + 2 + "");
            OdbcDataReader rd1 = objDAL.SpGetReader("CALL selectcond(?,?,?)", login);
            string strHostName = System.Net.Dns.GetHostName();
            // string clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            string clientIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            string ipaddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            string counter = @"SELECT counter_no,counter_id FROM m_sub_counter WHERE counter_ip='" + ipaddress + "'";
            DataTable dt_counter = objDAL.DtTbl(counter);
           
            if (dt_counter.Rows.Count > 0)
            {
                Session["counter"] = dt_counter.Rows[0][0].ToString();
                Session["counter_id"] = dt_counter.Rows[0][1].ToString();
               
            }
            if (rd1.Read())
            {
                userid = rd1[3].ToString();
                string user = rd1[0].ToString();
                string pwd1 = rd1[1].ToString();
                string pwd = base64Decode(pwd1);
                int level = int.Parse(rd1[2].ToString());
                int staffid = int.Parse(rd1[4].ToString());
                Session["staffid"] = staffid.ToString();
                defaultform = int.Parse(rd1[5].ToString());
                rd1.Close();
                try
                {
                    OdbcCommand cmdstaff = new OdbcCommand();
                    cmdstaff.Parameters.AddWithValue("tblname", "m_staff as st,m_sub_designation as desig,m_sub_office as office");
                    cmdstaff.Parameters.AddWithValue("attribute", "desig.designation,office.office");
                    cmdstaff.Parameters.AddWithValue("conditionv", "staff_id=" + staffid + " and desig.desig_id=st.desig_id and office.office_id=st.office_id");
                    OdbcDataReader rdstaff = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmdstaff);
                    if (rdstaff.Read())
                    {
                        desgination = rdstaff[0].ToString();
                        office = rdstaff[1].ToString();
                    }
                }
                catch
                {
                }
                if (s.Equals(user))
                {
                    if (psswd.Equals(pwd))
                    {
                        OdbcCommand cmddefaultform = new OdbcCommand();
                        cmddefaultform.Parameters.AddWithValue("tblname", "m_user as user,m_sub_form as form");
                        cmddefaultform.Parameters.AddWithValue("attribute", "form.formname");
                        cmddefaultform.Parameters.AddWithValue("conditionv", "form.form_id=" + defaultform + "");
                        OdbcDataReader rddefault = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmddefaultform);
                        if (rddefault.Read())
                        {
                            defaultformname = rddefault[0].ToString();
                        }
                        DateTime dt = DateTime.Now;
                        string date = dt.ToString("yyyy/MM/dd") + ' ' + dt.ToString("HH:mm:ss");
                        OdbcCommand cmd11 = new OdbcCommand();
                        cmd11.Parameters.AddWithValue("tblname", "t_login");
                        cmd11.Parameters.AddWithValue("attribute", "max(sno)");
                        DataTable dtt11 = new DataTable();
                        dtt11 = objDAL.SpDtTbl("call selectdata(?,?)", cmd11);
                        try
                        {
                            int ab = int.Parse(dtt11.Rows[0][0].ToString());
                            h = int.Parse(dtt11.Rows[0][0].ToString());
                            h = h + 1;
                        }
                        catch 
                        {
                            h = 1;
                        }
                        Session["username"] = Loginframe.UserName;
                        Session["password"] = Loginframe.Password;
                        Session["level"] = level;
                        Session["designation"] = desgination;
                        Session["office"] = office;
                        Session["userid"] = userid;
                        Application["CheckFormLoad"] = 0;
                        Session["CheckFormLoad"] = 0;
                        Session["sno"] = h;
                        OdbcCommand cmd3 = new OdbcCommand();
                        cmd3.Parameters.AddWithValue("tblname", "t_login");
                        cmd3.Parameters.AddWithValue("val", "" + h + ",'" + userid + "','" + date + "',null," + 0 + ",'" + strHostName + "','" + clientIPAddress + "'");
                        try
                        {
                            objDAL.Procedures_void("CALL savedata(?,?)", cmd3);
                        }
                        catch { }
                        string hj = "~/" + defaultformname + ".aspx";

                        #region CHECK CURRENT DATE
                        try
                        {
                            OdbcCommand cmd246 = new OdbcCommand();
                            cmd246.Parameters.AddWithValue("tblname", "t_settings");
                            cmd246.Parameters.AddWithValue("attribute", "count(*)");
                            cmd246.Parameters.AddWithValue("conditionv", "is_current=1 and curdate() between start_eng_date and end_eng_date ");
                            OdbcDataReader dr = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmd246);
                            while (dr.Read())
                            {
                                if (int.Parse(dr["count(*)"].ToString()) == 0)
                                {
                                    MessageBox.Show("Current date not set", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
                                    hj = "settingmaster.aspx";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        #endregion CHECK CURRENT DATE

                        ViewState["page"] = hj;
                        Loginframe.Visible = false;
                        if (Session["counter"] != null)
                        {
                            lblCounter.Text = "Do you wish to continue in counter- " + Session["counter"].ToString();
                        }
                        else
                        {
                            lblCounter.Text = "Counter not set, Please contact administrator..,";
                        }
                        SetFocus(btnYes);
                        this.ModalPopupExtender1.Show();

                        //Response.Redirect(hj, false);
                    }
                    else
                    {
                        Loginframe.FailureText = "Your login attempt was not successful. Please try again.";
                        lblOk.Text = "Your login attempt was not successful. Please try again.";
                        SetFocus(btnOk);
                        this.ModalPopupExtender2.Show();
                    }
                }
                else
                {
                    Loginframe.FailureText = "Your login attempt was not successful. Please try again.";
                    lblOk.Text = "Your login attempt was not successful. Please try again.";
                    SetFocus(btnOk);
                    this.ModalPopupExtender2.Show();
                }
            }
            else
            {
                Loginframe.FailureText = "Your login attempt was not successful. Please try again.".ToString();
                lblOk.Text = "Your login attempt was not successful. Please try again.";
                SetFocus(btnOk);
                this.ModalPopupExtender2.Show();
            }
        }
        catch { Response.Redirect("~Login frame.aspx"); }
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (ViewState["check"] != null)
        {
            if (Session["counter"] != null)
            {
                if (ViewState["page"] != null)
                {
                    string page = ViewState["page"].ToString();
                    Response.Redirect(page, false);
                }
                else
                {
                    authenticate();
                }
            }
            else
            {
                Response.Redirect("Login frame.aspx");
            }
        }
        else
        {
            authenticate();
        }
    }
    protected void btnNO_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login frame.aspx");
    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        authenticate();
    }
}
using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class User_Account_Information : System.Web.UI.Page
{
    OdbcConnection con = new OdbcConnection();
    commonClass objcls = new commonClass();
    static string strConnection;
   int c, s, p, q, v,l,r;
    string temp1;
    int policyid, fo,old;
    string policytype;
    int user_id,k,k1;

    #region PAGELOAD

    protected void Page_Load(object sender, EventArgs e)
    {
         clsCommon obj = new clsCommon();
         strConnection = obj.ConnectionString();

         try
         {

             if (!Page.IsPostBack)
             {
                 ViewState["action"] = "NIL";
                check();
               
                 Panel4.Visible = false;
                 Button2.Visible = false;
                 Title = " Tsunami ARMS User Account Information ";

                 #region combo level


                 try
                 {
                    // string dds1 = "  SELECT  prev_level  from m_userprevsetting WHERE  rowstatus <>2";

                     OdbcCommand dds1 = new OdbcCommand();
                     dds1.Parameters.AddWithValue("tblname", "m_userprevsetting");
                     dds1.Parameters.AddWithValue("attribute", "prev_level");
                     dds1.Parameters.AddWithValue("conditionv", "rowstatus <>2");

                     OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", dds1);

                     DataTable dtt1f = new DataTable();
                     dtt1f = objcls.GetTable(rd);
                   
                     DataRow Lrow = dtt1f.NewRow();
                     Lrow["prev_level"] = "--Select--";
                     dtt1f.Rows.InsertAt(Lrow, 0);
                     dtt1f.AcceptChanges();           
                     DropDownList2.DataSource = dtt1f;
                     DropDownList2.DataBind();
                 }
                 catch (Exception ex)
                 {
                     lblHead.Visible = false;
                     lblHead2.Visible = true;
                     lblOk.Text = "Error in loading Prev Level ";
                     pnlOk.Visible = true;
                     pnlYesNo.Visible = false;
                     ModalPopupExtender2.Show();
                 }



                
                 #endregion

                 #region cmbo staffname


                 try
                 {
                     //string dds2 = "SELECT staffname,staff_id FROM m_staff WHERE  rowstatus<>" + 2 + " order by staffname asc";


                     OdbcCommand dds2 = new OdbcCommand();
                     dds2.Parameters.AddWithValue("tblname", "m_staff");
                     dds2.Parameters.AddWithValue("attribute", "staffname,staff_id");
                     dds2.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by staffname asc");

                     DataTable dtt1r = new DataTable();
                     dtt1r = objcls.SpDtTbl("call selectcond(?,?,?)", dds2);
                     DataRow row11br = dtt1r.NewRow();
                     row11br["staff_id"] = "-1";
                     row11br["staffname"] = "--Select--";
                     dtt1r.Rows.InsertAt(row11br, 0);
                     dtt1r.AcceptChanges();
                     DropDownList1.DataSource = dtt1r;
                     DropDownList1.DataBind();
                 }
                 catch (Exception ex)
                 {
                     lblHead.Visible = false;
                     lblHead2.Visible = true;
                     lblOk.Text = "Staff  does not exists";
                     pnlOk.Visible = true;
                     pnlYesNo.Visible = false;
                     ModalPopupExtender2.Show();
                 }
                 #endregion

                 #region GRID
                 //string df1 = "select  u.user_id 'User Id',u.username 'User Name',u.level 'User Rigts level',d.formname 'Default form' from m_user u,m_sub_form d where u.defaultform=d.form_id and rowstatus<>'2'";

                 OdbcCommand df1 = new OdbcCommand();
                 df1.Parameters.AddWithValue("tblname", "m_user u,m_sub_form d");
                 df1.Parameters.AddWithValue("attribute", "u.user_id 'User Id',u.username 'User Name',u.level 'User Rigts level',d.formname 'Default form'");
                 df1.Parameters.AddWithValue("conditionv", "u.defaultform=d.form_id and rowstatus<>'2'");

                 DataTable ds = new  DataTable();
                 ds = objcls.SpDtTbl("call selectcond(?,?,?)", df1);
                 dguseraccount.DataSource = ds;
                 dguseraccount.DataBind();
                 #endregion

             }
         }
         catch (Exception ex)

         { }
        
       this.ScriptManager1.SetFocus(DropDownList1);

   }//page load
#endregion PAGE LOAD

    #region USERNAME CHECKING
    protected void TextUserName_TextChanged(object sender, EventArgs e)
    {
        try
        {

           // string u1 = "Select  distinct username from m_user where  rowstatus<>" + 2 + "";

            OdbcCommand u1 = new OdbcCommand();
            u1.Parameters.AddWithValue("tblname", "m_user");
            u1.Parameters.AddWithValue("attribute", "distinct username");
            u1.Parameters.AddWithValue("conditionv", " rowstatus<>" + 2 + "");

            OdbcDataReader user = objcls.SpGetReader("call selectcond(?,?,?)", u1);
            while (user.Read())
            {
                if (TextUserName.Text == user["username"].ToString())
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = " User name Already registered in database";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();

                    TextUserName.Text = "";
                    TextUserName.Focus();
                }

                else
                {
                    lblHead.Visible = true;
                    lblHead2.Visible = false;
                    lblOk.Text = "User name accepted";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                }
            }
            this.ScriptManager1.SetFocus(TextPsword);
        }
        catch (Exception ex)
        { }
       
    }
        #endregion 
      
    

    #region  SAVE AND EDIT

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (TextPsword.Text == "" || TextUserName.Text == "" || TextRetypePswd.Text=="")
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Please Check User Name & Password";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
            return;
        }

        if (BtnSave.Text == "Save")
        {
            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblMsg.Text = "Do you want to Add the user?";
            ViewState["action"] = "Save";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        else
        {
            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblMsg.Text = " Do you want to update ?";
            ViewState["action"] = "Edit";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();

        }
    } //buttonclick
                #endregion  SAVE AND EDIT

  
    #region    DISPLAY FROM GRID

    protected void dguseraccount_SelectedIndexChanged1(object sender, EventArgs e)
    {

        GridViewRow row = dguseraccount.SelectedRow;// dguseraccount.SelectedRow;
        try
        {


            #region fetching db values

            BtnSave.Text = "Edit";


            k = int.Parse(dguseraccount.SelectedRow.Cells[1].Text.ToString());
            //  string u2 = "select * from m_user where user_id=" + k + " and rowstatus<>'2'";

            OdbcCommand u2 = new OdbcCommand();
            u2.Parameters.AddWithValue("tblname", "m_user");
            u2.Parameters.AddWithValue("attribute", "staff_id,username,password,level");
            u2.Parameters.AddWithValue("conditionv", " user_id=" + k + " and rowstatus<>'2'");

            DataTable rd = new DataTable();
            rd = objcls.SpDtTbl("call selectcond(?,?,?)", u2);

            if (rd.Rows.Count > 0)
            {
                txtstaffid.Text = rd.Rows[0]["staff_id"].ToString();

                //  string u3 = "select staffname from m_staff where staff_id='" + rd["staff_id"].ToString() + "' and rowstatus <> '2'";

                OdbcCommand u3 = new OdbcCommand();
                u3.Parameters.AddWithValue("tblname", "m_staff");
                u3.Parameters.AddWithValue("attribute", "staffname");
                u3.Parameters.AddWithValue("conditionv", " staff_id='" + rd.Rows[0]["staff_id"].ToString() + "' and rowstatus <> '2'");

                DataTable rde = new DataTable();
                rde = objcls.SpDtTbl("call selectcond(?,?,?)", u3);

                if (rde.Rows.Count > 0)
                {

                    DropDownList1.SelectedValue = rd.Rows[0]["staff_id"].ToString();

                }


                TextUserName.Text = rd.Rows[0]["username"].ToString();

                TextPsword.Text = rd.Rows[0]["password"].ToString();
                TextRetypePswd.Text = rd.Rows[0]["password"].ToString();
                DropDownList2.SelectedValue = rd.Rows[0]["level"].ToString();

              //  string u4 = "select  f.formname  from m_userprevsetting up,m_sub_form f where prev_level = " + DropDownList2.SelectedValue.ToString() + " and f.form_id=up.defaultform_id ";


                OdbcCommand u4 = new OdbcCommand();
                u4.Parameters.AddWithValue("tblname", "m_userprevsetting up,m_sub_form f");
                u4.Parameters.AddWithValue("attribute", "f.formname");
                u4.Parameters.AddWithValue("conditionv", "prev_level = " + DropDownList2.SelectedValue.ToString() + " and f.form_id=up.defaultform_id ");

                DataTable or3 = new DataTable();
                or3 = objcls.SpDtTbl("call selectcond(?,?,?)", u4);

                if (or3.Rows.Count>0)
                {
                    txtdefaultform.Text = or3.Rows[0]["formname"].ToString();

                }

             
                //string uq1 = "select formname from m_sub_form f, m_userprev_formset p where p.prev_forms_id=f.form_id and  p.rowstatus <>2";

                OdbcCommand uq1 = new OdbcCommand();
                uq1.Parameters.AddWithValue("tblname", "m_sub_form f, m_userprev_formset p");
                uq1.Parameters.AddWithValue("attribute", "f.formname");
                uq1.Parameters.AddWithValue("conditionv", "p.prev_forms_id=f.form_id and  p.rowstatus <>2");

                DataTable or6 = new DataTable();
                or6 = objcls.SpDtTbl("call selectcond(?,?,?)", uq1);

                if (or6.Rows.Count > 0)
                {
                    for (int aa = 0; aa < or6.Rows.Count; aa++)
                    {
                        ListUserPrivlegs.Items.Add(or6.Rows[aa][0].ToString());
                    }

                }

               // string uq2 = "Select execoverride from m_userprevsetting where prev_level =" + DropDownList2.SelectedValue.ToString() + " and  rowstatus <>2";


                OdbcCommand uq2 = new OdbcCommand();
                uq2.Parameters.AddWithValue("tblname", "m_userprevsetting");
                uq2.Parameters.AddWithValue("attribute", "execoverride");
                uq2.Parameters.AddWithValue("conditionv", "prev_level =" + DropDownList2.SelectedValue.ToString() + " and  rowstatus <>2");

                DataTable or9 = new DataTable();
                or9 = objcls.SpDtTbl("call selectcond(?,?,?)", uq2);

                if (or9.Rows.Count > 0)
                {
                    txtexecuteop.Text = or9.Rows[0]["execoverride"].ToString();
                }
        
            }


            #endregion


        }//try

        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Selected staff does not exists in m_staff ";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
        }
       
    }//function
    #endregion    DISPLAY FROM GRID

    #region  GRIDVIEW SELECTION
    protected void dguseraccount_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dguseraccount, "Select$" + e.Row.RowIndex);
            }

    }



        #endregion  GRIDVIEW SELECTION

    #region DELETE

    protected void btndelete_Click(object sender, EventArgs e)
    {
        lblHead.Visible = true;
        lblHead2.Visible = false;
        lblMsg.Text = "Do you want to delete the user?";
        ViewState["action"] = "Delete";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }

    #endregion DELETE

    #region clear function
    public void clear1()
    {
        txtstaffid.Text = "";
        DropDownList1.SelectedIndex = -1;
       
        TextUserName.Text = "";

        #region cmbo staffname


        try
        {
            //string uq3 = "SELECT staffname,staff_id FROM m_staff WHERE  rowstatus<>" + 2 + " order by staffname asc";

            OdbcCommand uq3 = new OdbcCommand();
            uq3.Parameters.AddWithValue("tblname", "m_staff");
            uq3.Parameters.AddWithValue("attribute", "staffname,staff_id");
            uq3.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by staffname asc");

            DataTable dtt1r = new DataTable();
            dtt1r = objcls.SpDtTbl("call selectcond(?,?,?)", uq3);
            DataRow row11br = dtt1r.NewRow();
            row11br["staff_id"] = "-1";
            row11br["staffname"] = "--Select--";
            dtt1r.Rows.InsertAt(row11br, 0);
            dtt1r.AcceptChanges();
            DropDownList1.DataSource = dtt1r;
            DropDownList1.DataBind();
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Staff  does not exists";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        #endregion
       TextPsword.Attributes.Add("value","");
       TextRetypePswd.Attributes.Add("value","");

       BtnSave.Text = "Save";
       DropDownList2.SelectedIndex = -1;
       ListUserPrivlegs.Items.Clear();
        txtdefaultform.Text = "";
        txtexecuteop.Text = "";
    }

    #endregion

    #region CLEAR BUTTON
    protected void clear_Click(object sender, EventArgs e)
    {

        clear1();

    }
    #endregion

    #region encryption/decryption
    public string base64Encode(string sData)
    {
        try
        {
            byte[] encData_byte = new byte[sData.Length];

            encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);

            string encodedData = Convert.ToBase64String(encData_byte);

            return encodedData;

        }
        catch (Exception ex)
        {
            throw new Exception("Error in base64Encode" + ex.Message);
        }
    }

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
    #endregion

   

    #region grid6 paging 

    
    protected void dguseraccount_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        #region GRID
       // string uq4 = "select  u.user_id 'User Id',u.username 'User Name',u.level 'User Rigts level',d.formname 'Default form' from m_user u,m_sub_form d where u.defaultform=d.form_id and rowstatus<>'2'";

        OdbcCommand uq4 = new OdbcCommand();
        uq4.Parameters.AddWithValue("tblname", "m_user u,m_sub_form d");
        uq4.Parameters.AddWithValue("attribute", "u.user_id 'User Id',u.username 'User Name',u.level 'User Rigts level',d.formname 'Default form'");
        uq4.Parameters.AddWithValue("conditionv", "u.defaultform=d.form_id and rowstatus<>'2'");

        
        DataTable ds = new  DataTable();
        ds = objcls.SpDtTbl("call selectcond(?,?,?)", uq4);
        dguseraccount.DataSource = ds;
        dguseraccount.DataBind();
        #endregion


        dguseraccount.PageIndex = e.NewPageIndex;
        dguseraccount.DataBind();

    }
    #endregion



    #region OK Message
    public void okmessage(string head, string message)
    {
        lblHead.Visible = false;
        lblHead2.Visible = true;
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
            if (obj.CheckUserRight("User Account Information", level) == 0)
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
       
    }
    #endregion



   
    protected void TextPsword_TextChanged(object sender, EventArgs e)
    {
        TextPsword.Attributes.Add("value", TextPsword.Text.ToString());

        this.ScriptManager1.SetFocus(TextRetypePswd);
    }
   
    
        
    

    protected void Button1_Click(object sender, EventArgs e)
    {
        panel.Visible = false;
        dguseraccount.Visible = false;
        Label2.Visible = false;
        Panel4.Visible = true;
        Button2.Visible = true;
        this.ScriptManager1.SetFocus(DropDownList1);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Panel4.Visible = false;
        Button2.Visible = false;
        panel.Visible = true;
        dguseraccount.Visible = true;
        Label2.Visible = true;
    }
    protected void TextRetypePswd_TextChanged(object sender, EventArgs e)
    {
        TextRetypePswd.Attributes.Add("value", TextRetypePswd.Text.ToString());
        this.ScriptManager1.SetFocus(DropDownList2);

    }
    #region new msg Yes
    protected void btnYes_Click(object sender, EventArgs e)
    {
       
        int staf_id = 0, form_id = 0;
        try
        {
            DateTime dt = DateTime.Now;
            String date = dt.ToString("yyyy-MM-dd HH:mm:ss");
            string pwd = base64Encode(TextPsword.Text.ToString());

            string ssa1 = "Select form_id from m_sub_form  where formname='" + txtdefaultform.Text.ToString() + "' and  status <>2";
            form_id = objcls.exeScalarint(ssa1);

           

            Session["username"] = "admin";
            Session["password"] = "admin!123";

            #region ----SAVE-----
            if (ViewState["action"].ToString() == "Save")
            {
                int user_id = 1;

                if ((Session["username"].Equals("admin")) && (Session["password"].Equals("admin!123")))
                {
                    try
                    {

                       string  vv1 = "select staff_id from m_staff where staffname='" + DropDownList1.SelectedItem.Text.ToString() + "'";
                       staf_id = objcls.exeScalarint(vv1);

                       

                    }
                    catch
                    {

                    }


                    #region SAVE

                    #region checking in database

                    OdbcCommand vv2 = new OdbcCommand();
                    vv2.Parameters.AddWithValue("tblname", "m_user");
                    vv2.Parameters.AddWithValue("attribute", "staff_id,username,level");
                    vv2.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");


                    DataTable rd = new DataTable();
                    rd = objcls.SpDtTbl("call selectcond(?,?,?)", vv2);
                    if (rd.Rows.Count > 0)
                    {
                        if (txtstaffid.Text == rd.Rows[0]["staff_id"].ToString() && TextUserName.Text == rd.Rows[0]["username"].ToString() && DropDownList2.SelectedValue == rd.Rows[0]["level"].ToString())
                        {
                            lblOk.Text = "Already registered in database";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                            clear1();
                            return;
                        }//if
                    }
                    #endregion

                    #region Primary key incrementation
                    try
                    {
                        string vv3 = "Select max(user_id) from m_user";
                        c = objcls.exeScalarint(vv3);
                        c = c + 1;
                    }
                    catch
                    {
                        c = 1;
                    }
                    #endregion

                    //Session["userid"] = user_id;


                    OdbcCommand cmd3 = new OdbcCommand();// con);
                    cmd3.Parameters.AddWithValue("tblname", "m_user");
                    cmd3.Parameters.AddWithValue("val", " " + c + "," + staf_id + ",0,'" + TextUserName.Text.ToString() + "', '" + pwd + "' ," + int.Parse(DropDownList2.SelectedValue) + " ," + form_id + "," + user_id + ",'" + date.ToString() + "'," + user_id + ",'" + date.ToString() + "'," + 0 + " ");
                    objcls.TransExeNonQuerySP_void("CALL savedata(?,?)", cmd3);
                    #endregion
                    #region GRID

                    //    string aas1 = "select  u.user_id 'User Id',u.username 'User Name',u.level 'User Rigts level',d.formname 'Default form' from m_user u,m_sub_form d where u.defaultform=d.form_id and rowstatus<>'2'";

                    OdbcCommand aas1 = new OdbcCommand();
                    aas1.Parameters.AddWithValue("tblname", "m_user u,m_sub_form d");
                    aas1.Parameters.AddWithValue("attribute", "u.user_id 'User Id',u.username 'User Name',u.level 'User Rigts level',d.formname 'Default form'");
                    aas1.Parameters.AddWithValue("conditionv", "u.defaultform=d.form_id and rowstatus<>'2'");


                    DataTable ds = new DataTable();
                    ds = objcls.SpDtTbl("call selectcond(?,?,?)", aas1);
                    dguseraccount.DataSource = ds;
                    dguseraccount.DataBind();
                    #endregion
                    lblHead.Visible = true;
                    lblHead2.Visible = false;
                    lblOk.Text = "User Added";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();


                    clear1();
                    dguseraccount.SelectedIndex = -1;


                }

                else
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Only admin can add data";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                }
            }
            #endregion

            #region --EDIT----------

            else if (ViewState["action"].ToString() == "Edit")
            {

                try
                {

                    if ((Session["username"].Equals("admin")) && (Session["password"].Equals("admin!123")))
                    {

                        #region EDIT
                        k = int.Parse(dguseraccount.SelectedRow.Cells[1].Text.ToString());


                        #region checking in database

                        // string zz1 = "select staff_id from m_user where rowstatus <>2";

                        OdbcCommand zz1 = new OdbcCommand();
                        zz1.Parameters.AddWithValue("tblname", "m_user");
                        zz1.Parameters.AddWithValue("attribute", "staff_id,username,level");
                        zz1.Parameters.AddWithValue("conditionv", "rowstatus <>2");

                        DataTable rd = new DataTable();
                        rd = objcls.SpDtTbl("call selectcond(?,?,?)", zz1);
                        if (rd.Rows.Count > 0)
                        {
                            if (txtstaffid.Text == rd.Rows[0]["staff_id"].ToString() && TextUserName.Text == rd.Rows[0]["username"].ToString() && DropDownList2.SelectedValue == rd.Rows[0]["level"].ToString())
                            {
                                lblOk.Text = "Already registered in database";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                clear1();
                                return;
                            }//if
                        }
                        #endregion


                        OdbcCommand cmd9 = new OdbcCommand();
                        cmd9.Parameters.AddWithValue("tblname", "m_user");
                        cmd9.Parameters.AddWithValue("val", "staff_id= " + int.Parse(DropDownList1.SelectedValue) + ",username='" + TextUserName.Text.ToString() + "',password ='" + pwd + "' ,level=" + int.Parse(DropDownList2.SelectedValue) + " ,defaultform=" + form_id + ",updatedby=" + user_id + ",updateddate='" + date.ToString() + "',rowstatus=" + 1 + "");
                        cmd9.Parameters.AddWithValue("convariable", "user_id=" + k + "");
                        objcls.Procedures_void("CALL updatedata(?,?,?)", cmd9);
                        #endregion EDIT

                        #region EDIT LOG TABLE
                     

                        //int q = objcls.PK_exeSaclarInt("rowno", "m_user_log");

                        //try
                        //{

                        //    q = q + 1;

                        //}
                        //catch
                        //{
                        //    q = 1;
                        //}

                 

                        //OdbcCommand zza1 = new OdbcCommand();
                        //zza1.Parameters.AddWithValue("tblname", "m_user");
                        //zza1.Parameters.AddWithValue("attribute", "staff_id,username,password,level,defaultform");
                        //zza1.Parameters.AddWithValue("conditionv", "user_id=" + k + " and rowstatus<>2");


                        //DataTable editr = new DataTable();
                        //editr = objcls.SpDtTbl("call selectcond(?,?,?)", zza1);
                        //if (editr.Rows.Count > 0)
                        //{
                        //    OdbcCommand cmd13 = new OdbcCommand();
                        //    cmd13.Parameters.AddWithValue("tblname", "m_user_log");
                        //    cmd13.Parameters.AddWithValue("val", "" + k + "," + int.Parse(editr.Rows[0]["staff_id"].ToString()) + "," + 0 + ",'" + editr.Rows[0]["username"].ToString() + "','" + editr.Rows[0]["password"].ToString() + "'," + int.Parse(editr.Rows[0]["level"].ToString()) + "," + int.Parse(editr.Rows[0]["defaultform"].ToString()) + "," + user_id + ",'" + date.ToString() + "'," + r + "," + 1 + "");
                        //    objcls.Procedures_void("CALL savedata(?,?)", cmd13);
                      //  }

                        #endregion EDIT LOG TABLE



                        lblHead.Visible = true;
                        lblHead2.Visible = false;
                        lblOk.Text = "Data Updated";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();

                        #region GRID
                        //   string dda1 = "select  u.user_id 'User Id',u.username 'User Name',u.level 'User Rigts level',d.formname 'Default form' from m_user u,m_sub_form d where u.defaultform=d.form_id and rowstatus<>'2'";

                        OdbcCommand dda1 = new OdbcCommand();
                        dda1.Parameters.AddWithValue("tblname", " m_user u,m_sub_form d");
                        dda1.Parameters.AddWithValue("attribute", "u.user_id 'User Id',u.username 'User Name',u.level 'User Rigts level',d.formname 'Default form'");
                        dda1.Parameters.AddWithValue("conditionv", "u.defaultform=d.form_id and rowstatus<>'2'");

                        DataTable ds = new DataTable();
                        ds = objcls.SpDtTbl("call selectcond(?,?,?)", dda1);
                        dguseraccount.DataSource = ds;
                        dguseraccount.DataBind();
                        #endregion
                        clear1();
                        BtnSave.Text = "Save";
                        dguseraccount.SelectedIndex = -1;

                    }
                    else
                    {
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "Only admin can update data";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                    }

                }
                catch (Exception ex)
                {
                }



            }//else 

            #endregion

            #region ---------DELETE----

            else if (ViewState["action"].ToString() == "Delete")
            {
                try
                {
                    k1 = int.Parse(dguseraccount.SelectedRow.Cells[1].Text.ToString());
                    Session["username"] = "admin";
                    Session["password"] = "admin!123";
                    if ((Session["username"].Equals("admin")) && (Session["password"].Equals("admin!123")))
                    {

                        #region DELETE



                        OdbcCommand cma = new OdbcCommand();
                        cma.Parameters.AddWithValue("tblname", "m_user");
                        cma.Parameters.AddWithValue("val", "rowstatus=2");
                        cma.Parameters.AddWithValue("convariable", "user_id=" + k1 + "");
                        objcls.Procedures_void(" call updatedata(?,?,?)", cma);
                        #endregion

                        #region LOGTABLE

                        //string fda1 = "select max(rowno) from m_user_log";
                        //s = objcls.PK_exeSaclarInt("rowno", "m_user_log");


                        //try
                        //{
                        //    s = s + 1;
                        //}
                        //catch
                        //{
                        //    s = 1;
                        //}
                       

                        //OdbcCommand gf1 = new OdbcCommand();
                        //gf1.Parameters.AddWithValue("tblname", "m_user");
                        //gf1.Parameters.AddWithValue("attribute", "staff_id,username,password,level,defaultform");
                        //gf1.Parameters.AddWithValue("conditionv", "user_id=" + k1 + " and rowstatus<>" + 2 + "");


                        //DataTable editr = new DataTable();
                        //editr = objcls.SpDtTbl("call selectcond(?,?,?)", gf1);
                        //if (editr.Rows.Count > 0)
                        //{
                        //    OdbcCommand cmd13 = new OdbcCommand();
                        //    cmd13.CommandType = CommandType.StoredProcedure;
                        //    cmd13.Parameters.AddWithValue("tblname", "m_user_log");
                        //    cmd13.Parameters.AddWithValue("val", "" + k1 + "," + int.Parse(editr.Rows[0]["staff_id"].ToString()) + "," + 0 + ",'" + editr.Rows[0]["username"].ToString() + "','" + editr.Rows[0]["password"].ToString() + "'," + int.Parse(editr.Rows[0]["level"].ToString()) + "," + int.Parse(editr.Rows[0]["defaultform"].ToString()) + "," + user_id + ",'" + date.ToString() + "'," + s + "," + 2 + "");
                        //    objcls.TransExeNonQuerySP_void("CALL savedata(?,?)", cmd13);
                        //}


                        #endregion LOGTABLE

                        lblHead.Visible = true;
                        lblHead2.Visible = false;
                        lblOk.Text = "Data deleted";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        #region GRID

                        //string gh1 = "select  u.user_id 'User Id',u.username 'User Name',u.level 'User Rigts level',d.formname 'Default form' from m_user u,m_sub_form d where u.defaultform=d.form_id and rowstatus<>'2'";

                        OdbcCommand gh1 = new OdbcCommand();
                        gh1.Parameters.AddWithValue("tblname", "m_user u,m_sub_form d");
                        gh1.Parameters.AddWithValue("attribute", " u.user_id 'User Id',u.username 'User Name',u.level 'User Rigts level',d.formname 'Default form'");
                        gh1.Parameters.AddWithValue("conditionv", "u.defaultform=d.form_id and rowstatus<>'2'");

                        DataTable ds = new DataTable();
                        ds = objcls.SpDtTbl("call selectcond(?,?,?)", gh1);
                        dguseraccount.DataSource = ds;
                        dguseraccount.DataBind();
                        #endregion
                        dguseraccount.SelectedIndex = -1;

                    }
                    else
                    {
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "Only admin can delete data";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();

                    }



                }
                catch (Exception ex)
                {

                }

                clear1();

            }
            #endregion
        }
        catch (Exception ex)
        {
        }

    }
    #endregion

    #region new msg no
    protected void btnNo_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Save")
        {

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";

        }
        else if (ViewState["action"].ToString() == "Edit")
        {
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
        else
        {

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
    }
    #endregion

    #region USER PRFILE REPORT
    protected void LinkButton1_Click1(object sender, EventArgs e)
    {
        try
        {

            #region report
            string str1, str2;
            int flag = 0;
            try
            {

                # region fetching the data needed to show as report from database and assigning to a datatable

                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.Parameters.AddWithValue("tblname", "m_user u,m_staff s ,m_sub_form f");
                cmd31.Parameters.AddWithValue("attribute", "u.user_id,u.staff_id,s.staffname,u.username,u.Level,f.formname");
                cmd31.Parameters.AddWithValue("conditionv", "u.staff_id=s.staff_id and u.defaultform=f.form_id and u.rowstatus<>2 order by s.staffname");

                DataTable dt = new DataTable();
                dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

                # endregion


                // creating a  file to save the report .... setting its font
                Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 80, 80);
                string pdfFilePath = Server.MapPath(".") + "/pdf/user.pdf";
                Font font8 = FontFactory.GetFont("ARIAL", 7);
                Font font9 = FontFactory.GetFont("ARIAL", 7, 1);
                PDF.pdfPage page = new PDF.pdfPage();

                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;

                doc.Open();
                PdfPTable table1 = new PdfPTable(7);

                PdfPCell cell1001 = new PdfPCell(new Phrase(new Chunk("USER PROFILE", font9)));
                cell1001.Colspan = 7;
                cell1001.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1001);

                # region giving heading for each coloumn in report
                PdfPCell cell100 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                table1.AddCell(cell100);


                PdfPCell cell400 = new PdfPCell(new Phrase(new Chunk("User Id", font9)));
                table1.AddCell(cell400);

                PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Staff Id", font9)));
                table1.AddCell(cell500);

                PdfPCell cell600 = new PdfPCell(new Phrase(new Chunk("Staff name", font9)));
                table1.AddCell(cell600);

                PdfPCell cell700 = new PdfPCell(new Phrase(new Chunk("User Name", font9)));
                table1.AddCell(cell700);

                PdfPCell cell900 = new PdfPCell(new Phrase(new Chunk("Level", font9)));
                table1.AddCell(cell900);

                PdfPCell cell010 = new PdfPCell(new Phrase(new Chunk("Defauult Form", font9)));
                table1.AddCell(cell010);

                # endregion
                doc.Add(table1);

                # region adding data to the report file
                int slno = 0;
                int i = 0, j = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    PdfPTable table = new PdfPTable(7);
                    if (i + j > 45)// total rows on page
                    {
                        doc.NewPage();

                        # region giving heading for each coloumn in report
                        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                        table.AddCell(cell1);


                        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("User Id", font9)));
                        table.AddCell(cell4);

                        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Staff Id", font9)));
                        table.AddCell(cell5);

                        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Staff name", font9)));
                        table.AddCell(cell6);

                        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("User Name", font9)));
                        table1.AddCell(cell7);

                        PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Level", font9)));
                        table.AddCell(cell9);

                        PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Defauult Form", font9)));
                        table.AddCell(cell10);

                        # endregion

                        i = 0; // reseting count for new page
                        j = 0;

                    }
                    # region data on page

                    slno = slno + 1;


                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table.AddCell(cell11);


                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["user_id"].ToString(), font8)));
                    table.AddCell(cell12);

                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["staff_id"].ToString(), font8)));
                    table.AddCell(cell13);


                    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["staffname"].ToString(), font8)));
                    table.AddCell(cell14);

                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["username"].ToString(), font8)));
                    table.AddCell(cell15);

                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["level"].ToString(), font8)));
                    table.AddCell(cell16);

                    PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["formname"].ToString(), font8)));
                    table.AddCell(cell17);

                    i++;//no of data row count
                    # endregion


                    doc.Add(table);

                }
                # endregion


                doc.Close();
                //System.Diagnostics.Process.Start(pdfFilePath);
                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=user.pdf&Title=User profile";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);
            }

            catch
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "caused exception ,cannot open pdf file";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
            }

            #endregion
        }
        catch (Exception ex)
        {
        }

    }
    #   endregion

    #region user audit
    protected void LnkBtnRepot_Click(object sender, EventArgs e)
    {
        try
        {
           
            #region report
            string str1, str2;
            int flag = 0;
            try
            {





                # region fetching the data needed to show as report from database and assigning to a datatable

                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.Parameters.AddWithValue("tblname", "t_login order by IPcode");
                cmd31.Parameters.AddWithValue("attribute", "sno,userid,logindate,logoutdate,IPcode");

                DataTable dt = new DataTable();
                dt = objcls.SpDtTbl("CALL selectdata(?,?)", cmd31);

                # endregion


                // creating a  file to save the report .... setting its font
                Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 80, 80);
                string pdfFilePath = Server.MapPath(".") + "/pdf/trial.pdf";
                Font font8 = FontFactory.GetFont("ARIAL", 7);
                Font font9 = FontFactory.GetFont("ARIAL", 7, 1);
                PDF.pdfPage page = new PDF.pdfPage();

                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;

                doc.Open();
                PdfPTable table1 = new PdfPTable(5);

                PdfPCell cell1001 = new PdfPCell(new Phrase(new Chunk("USER AUDIT TRIAL REPORT", font9)));
                cell1001.Colspan = 5;
                cell1001.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1001);

                # region giving heading for each coloumn in report
                PdfPCell cell100 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                table1.AddCell(cell100);


                PdfPCell cell400 = new PdfPCell(new Phrase(new Chunk("User Id", font9)));
                table1.AddCell(cell400);

                PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Login Date", font9)));
                table1.AddCell(cell500);

                PdfPCell cell600 = new PdfPCell(new Phrase(new Chunk("LogOut Date", font9)));
                table1.AddCell(cell600);

                PdfPCell cell700 = new PdfPCell(new Phrase(new Chunk("IP CODE", font9)));
                table1.AddCell(cell700);

               

                # endregion
                doc.Add(table1);

                # region adding data to the report file
                int slno = 0;
                int i = 0, j = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    PdfPTable table = new PdfPTable(5);
                    if (i + j > 55)// total rows on page
                    {
                        doc.NewPage();

                        # region giving heading for each coloumn in report
                        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                        table.AddCell(cell1);


                        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("User Id", font9)));
                        table.AddCell(cell4);

                        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Login Date", font9)));
                        table.AddCell(cell5);

                        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("LogOut Date", font9)));
                        table.AddCell(cell6);

                        PdfPCell cell75 = new PdfPCell(new Phrase(new Chunk("IP CODE", font9)));
                        table.AddCell(cell75);

                       
                        # endregion

                        i = 0; // reseting count for new page
                        j = 0;

                    }
                    # region data on page
                 
                    slno = slno + 1;

                    if (slno == 1)
                    {
                        policytype = dr["IPcode"].ToString();

                        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("IP of Machine: " + dr["IPcode"].ToString() + "   ", font9)));
                        cell12.Colspan = 5;
                        cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12);
                        j++;//  sub heading count
                    }
                    else if (policytype != dr["IPcode"].ToString())
                    {


                        policytype = dr["IPcode"].ToString();
                      
                        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("IP of Machine: " + dr["IPcode"].ToString() + " "    , font9)));
                        cell12.Colspan =5;
                        cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12);
                        slno = 1;
                        j++;//  sub heading count
                    }



                    PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table.AddCell(cell22);


                    PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["userid"].ToString(), font8)));
                    table.AddCell(cell18);


                    DateTime dt5 = DateTime.Parse(dr["logindate"].ToString());
                    string date1 = dt5.ToString("dd/MM/yyyy HH:mm:ss");

                    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                    table.AddCell(cell14);

                    string dateou=dr["logoutdate"].ToString();

                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dateou.ToString(), font8)));
                    table.AddCell(cell16);

                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["IPcode"].ToString(), font8)));
                    table.AddCell(cell15);
                    i++;//no of data row count
                    # endregion


                    doc.Add(table);

                }
                # endregion


                doc.Close();
                //System.Diagnostics.Process.Start(pdfFilePath);
                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=trial.pdf&Title=User profile";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);
            }

            catch
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "caused exception ,cannot open pdf file";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
            }
           
            #endregion
        }
        catch (Exception ex)
        {
        }
       
    }
    #endregion

  
   

   
    protected void ListUserPrivlegs_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtstaffid.Text = DropDownList1.SelectedValue;
    }
    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListUserPrivlegs.Items.Clear();
        try
        {

            if (DropDownList2.Text != "--Select--")
            {

                //string we1 = " select distinct f.formname from m_sub_form f,m_userprevsetting u where prev_level = " + DropDownList2.SelectedValue.ToString() + " and  f.form_id=u.defaultform_id  and rowstatus <>2";


                OdbcCommand we1 = new OdbcCommand();
                we1.Parameters.AddWithValue("tblname", "m_sub_form f,m_userprevsetting u");
                we1.Parameters.AddWithValue("attribute", " distinct f.formname");
                we1.Parameters.AddWithValue("conditionv", "prev_level = " + DropDownList2.SelectedValue.ToString() + " and  f.form_id=u.defaultform_id  and rowstatus <>2");

                DataTable rde = new DataTable();
                rde = objcls.SpDtTbl("call selectcond(?,?,?)", we1);
                if (rde.Rows.Count > 0)
                {
                    txtdefaultform.Text = rde.Rows[0]["formname"].ToString();

                }

                //string we2 = "select distinct  formname from m_sub_form f,m_userprev_formset s where f.form_id=s.form_id and prev_level = " + DropDownList2.SelectedValue.ToString() + " and rowstatus <>2 ";

                OdbcCommand we2 = new OdbcCommand();
                we2.Parameters.AddWithValue("tblname", "m_sub_form f,m_userprev_formset s");
                we2.Parameters.AddWithValue("attribute", "distinct  formname");
                we2.Parameters.AddWithValue("conditionv", "f.form_id=s.form_id and prev_level = " + DropDownList2.SelectedValue.ToString() + " and rowstatus <>2");

                DataTable rde2 = new DataTable();
                rde2 = objcls.SpDtTbl("call selectcond(?,?,?)", we2);
                if (rde2.Rows.Count > 0)
                {
                    for (int i = 0; i < rde2.Rows.Count; i++)
                    {
                        ListUserPrivlegs.Items.Add(rde2.Rows[i][0].ToString());
                    }

                }

                //string we3 = "Select distinct execoverride from m_userprevsetting where prev_level =" + DropDownList2.SelectedValue.ToString() + " and  rowstatus <>2";

                OdbcCommand we3 = new OdbcCommand();
                we3.Parameters.AddWithValue("tblname", "m_userprevsetting");
                we3.Parameters.AddWithValue("attribute", "distinct execoverride");
                we3.Parameters.AddWithValue("conditionv", "prev_level =" + DropDownList2.SelectedValue.ToString() + " and  rowstatus <>2");

                DataTable or9 = new DataTable();
                or9 = objcls.SpDtTbl("call selectcond(?,?,?)", we3);

                if (or9.Rows.Count > 0)
                {
                    txtexecuteop.Text = or9.Rows[0]["execoverride"].ToString();
                }
            }
            else
            {
                ListUserPrivlegs.Items.Clear();
                txtexecuteop.Text = "";
                txtdefaultform.Text = "";
            }

        }
        catch (Exception ex)
        { }
       
        this.ScriptManager1.SetFocus(DropDownList2);
    
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }

    }
} //first

       


    




  




    


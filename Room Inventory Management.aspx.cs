
/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMMODATION
// Screen Name      :      Inventory Management
// Form Name        :      Room Inventory Management.aspx
// ClassFile Name   :      Room Inventory Managemen.aspx.cs
// Purpose          :      Items requested, approved and issued etc are done through this form
// Created by       :      Asha
// Created On       :      14-September-2010
// Last Modified    :      8-September-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       8-September-2010  Asha        Code change as per the review


//-------------------------------------------------------------------

#region INVENTORY MANAGEMENT
using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF;


public partial class Room_Inventory_Management : System.Web.UI.Page
{
    #region declaration
    OdbcConnection conn = new OdbcConnection();//("Driver={MySQL ODBC 3.51 Driver};database=tdbnew;option=0;port=3306;server=192.168.2.66;uid=root;password=root");
    string user, ss,d, y, m, g,year;
    string SeasName; string season;
    static string strConnection;
    int k5, Rm;
    string FD, TD; 
    string strReqNo;string Key_code;
    int a4, id,  a6, a47, id1, OffR, ApReq, AdApp,Key_id;
    clsCommon obj = new clsCommon();
    commonClass obje = new commonClass();
    string Req;
    int Counter, slno;
    string Ddate1, GName;
    decimal Rqt2;
    int Reqqty, Apqqty,itemId1;
    string StorName;   
    DataTable dtitem = new DataTable();    
    int  it2; int YN;
    int NewItemId; decimal amount1; string AppId;
    string TextReq; string code, stock1;
    string a1, b, c, d1; string StorName1; string OffName,FromOff;
    int sl, endl,Rqt5;
    string GName1;
    #endregion
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        #region combo,current date,username,password

        if (!IsPostBack)
        {
           
            strConnection = obj.ConnectionString();
            conn.ConnectionString = strConnection;
          
            Title = "Tsunami ARMS - Room Inventory Management";
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            check();

            conn = obje.NewConnection();
            dtitem = additem();

            Session["dtItem"] = dtitem;

            #region Alert--- Below Reorder level 
            //OdbcCommand Rol = new OdbcCommand("select itemname from m_inventory mi,m_sub_item i where reorderlevel <stock_qty and mi.item_id=i.item_id and mi.rowstatus<>'2'", conn);
            //OdbcDataReader Rolr = Rol.ExecuteReader();
            //while (Rolr.Read())
            //{
            //    string Ritem = Rolr["itemname"].ToString();

            //    lblOk.Text = Ritem.ToString() + "'s Reorderlevel is less than Stock quantity  "; lblHead.Text = "Tsunami ARMS - Warning";
            //    pnlOk.Visible = true;
            //    pnlYesNo.Visible = false;
            //    ModalPopupExtender2.Show();
            //}
            #endregion

            try
            {
                user = Session["username"].ToString();
                txtRequestOfficer.Text = user.ToString();
                txtIssueOfficer.Text = user.ToString();

                OdbcCommand User = new OdbcCommand();
                User.CommandType = CommandType.StoredProcedure;
                User.Parameters.AddWithValue("tblname", "m_user");
                User.Parameters.AddWithValue("attribute", "user_id");
                User.Parameters.AddWithValue("conditionv", "username='" + user + "' and rowstatus<>'2'");
                OdbcDataAdapter d346 = new OdbcDataAdapter(User);
                DataTable dt46 = new DataTable();
                dt46 = obje.SpDtTbl("CALL selectcond(?,?,?)", User);
                if (dt46.Rows.Count > 0)
                {
                    for (int k = 0; k < dt46.Rows.Count; k++)
                    {
                        id = Convert.ToInt32(dt46.Rows[k]["user_id"].ToString());
                        Session["userid"] = id;
                    }
                }
                else
                {
                    id = 0;
                }
                                
            }
            catch
            {
                id = 0;
            }


            conn = obje.NewConnection();            
            Panel8.Visible = false;
            pnlapprove1.Visible = false;
            DateTime date = DateTime.Now;
            string dt = date.ToString("dd-MM-yyyy");
            txtDate.Text = dt.ToString();
            dtgItem.Visible = true;
            DateTime yee = DateTime.Now;
            year = yee.ToString("yyyy");
            Session["year"] = year;
            dtgItem.Visible = true;
         
          
            OdbcDataAdapter Store2 = new OdbcDataAdapter("(SELECT store_id as Id, storename as Name FROM  m_sub_store where rowstatus<>2) union (SELECT counter_id as Id, counter_no as Name FROM m_sub_counter) union (SELECT team_id as Id,teamname as Name FROM m_team where rowstatus<>2)", conn);
            DataTable ds2 = new DataTable();
            DataColumn colID2 = ds2.Columns.Add("Id", System.Type.GetType("System.Int32"));
            DataColumn colNo2 = ds2.Columns.Add("Name", System.Type.GetType("System.String"));
            DataRow row2 = ds2.NewRow();
            row2["Id"] = "-1";
            row2["Name"] = "--Select--";
            ds2.Rows.InsertAt(row2, 0);
            Store2.Fill(ds2);
            cmbReqStore.DataSource = ds2;
            cmbReqStore.DataBind();

            OdbcCommand Store3 = new OdbcCommand();
            Store3.CommandType = CommandType.StoredProcedure;
            Store3.Parameters.AddWithValue("tblname", "m_sub_store");
            Store3.Parameters.AddWithValue("attribute", "store_id as Id, storename as Sname");
            Store3.Parameters.AddWithValue("conditionv", "rowstatus<>2");
            OdbcDataAdapter Store36 = new OdbcDataAdapter(Store3);
            DataTable ds3 = new DataTable();
            ds3 = obje.SpDtTbl("CALL selectcond(?,?,?)", Store3);
            DataRow row3 = ds3.NewRow();
            ds3.Rows.InsertAt(row3, 0);
            row3["Id"] = "-1";
            row3["Sname"] = "--Select--";
            cmbIssueStore.DataSource = ds3;
            cmbIssueStore.DataBind();

            conn = obje.NewConnection();
            OdbcDataAdapter Store = new OdbcDataAdapter("SELECT CAST(CONCAT('S',`store_id`) as CHAR)as Id, storename as Name FROM  m_sub_store where rowstatus<>2 union SELECT CAST(CONCAT('C',`counter_id`) as CHAR) as Id, counter_no as Name FROM  m_sub_counter where rowstatus<>2 union SELECT CAST(CONCAT('T',`team_id`) as CHAR) as Id, teamname as Name FROM  m_team where rowstatus<>2", conn);
            DataTable ds = new DataTable();
            Store.Fill(ds);
            DataRow row9 = ds.NewRow();
            ds.Rows.InsertAt(row9, 0);
            row9["Id"] = "-1";
            row9["Name"] = "--Select--";
            cmbReqStore.DataSource = ds;
            cmbReqStore.DataBind();
           
            OdbcCommand cd1 = new OdbcCommand("select Max(reqno)from t_inventoryrequest", conn);           
            if (Convert.IsDBNull(cd1.ExecuteScalar()) == true)
            {
                strReqNo = "SrNo/" + year + "/" + "0001";
                txtRequestNo.Text = strReqNo.ToString();
            }
            else
            {
                string o1 = cd1.ExecuteScalar().ToString();
                string ab1 = o1.Substring(10, 4);
               a4 = Convert.ToInt32(ab1);
               a4 = a4 + 1;
                if (a4 >= 1000)
                {
                    strReqNo = "SrNo/" + year + "/" + a4;
                    txtRequestNo.Text = strReqNo.ToString();

                }
                else if (a4 >= 100)
                {
                    strReqNo = "SrNo/" + year + "/0" + a4;
                    txtRequestNo.Text = strReqNo.ToString();
                }
                else if (a4 >= 10)
                {

                    strReqNo = "SrNo/" + year + "/00" + a4;
                    txtRequestNo.Text = strReqNo.ToString();
                }
                else if (a4 < 10)
                {
                    strReqNo = "SrNo/" + year + "/000" + a4;
                    txtRequestNo.Text = strReqNo.ToString();
                }
            }

            string strIssNo;
            OdbcCommand Issue1 = new OdbcCommand("SELECT max(issueno) from t_inventoryrequest_issue", conn);
            if (Convert.IsDBNull(Issue1.ExecuteScalar()) == true)
            {
                strIssNo = "ImNo/" + year + "/" + "0001";
                txtIssueNo.Text = strIssNo.ToString();
            }
            else
            {
                string o1 = Issue1.ExecuteScalar().ToString();
                string ab1 = o1.Substring(10, 4);
                a4 = Convert.ToInt32(ab1);
                a4 = a4 + 1;
                if (a4 >= 1000)
                {
                    strIssNo = "ImNo/" + year + "/" + a4;
                    txtIssueNo.Text = strIssNo.ToString();

                }
                else if (a4 >= 100)
                {
                    strIssNo = "ImNo/" + year + "/0" + a4;
                    txtIssueNo.Text = strIssNo.ToString();
                }
                else if (a4 >= 10)
                {

                    strIssNo = "ImNo/" + year + "/00" + a4;
                    txtIssueNo.Text = strIssNo.ToString();
                }
                else if (a4 < 10)
                {
                    strIssNo = "ImNo/" + year + "/000" + a4;
                    txtIssueNo.Text = strIssNo.ToString();
                }                          

            }            
        
            }


            conn = obje.NewConnection();          
            btnRequest.Visible = true;
            btnIssue.Visible = true;
            btnApprove.Enabled = true;
        
            btnAddRequest.Enabled = true;
       
        #endregion

    }
    protected void cmbItem_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {

    }
    protected void cmbItemName_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {             
    }

    #region data table add item
    public DataTable additem()
    {
        dtitem.Columns.Clear();
        dtitem.Columns.Add("Itemcode", System.Type.GetType("System.String"));
        dtitem.Columns.Add("Itemcategory", System.Type.GetType("System.String"));
        dtitem.Columns.Add("Itemname", System.Type.GetType("System.String"));
        dtitem.Columns.Add("Quantity", System.Type.GetType("System.Int32"));
        dtitem.Columns.Add("Measurement", System.Type.GetType("System.String"));
        return (dtitem);
    }
    #endregion


    protected void btnAddRequest_Click(object sender, EventArgs e)
    {
        dtgItem.Visible = true;
       
        #region add item
        
        int iRowCount = 0; 

        dtitem = (DataTable)Session["dtItem"];
        dtgItem.Visible = true;
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        int Qty = int.Parse(txtQuantity.Text);
        if (cmbItem.SelectedItem.Text == "Kit")
        {

            try
            {
                if (dtitem.Rows.Count > 0)
                {

                    if (txtCode.Text != "")
                    {

                        DataRow[] drItem = dtitem.Select("Itemcode='" + txtCode.Text.ToString() + "'");
                        if (drItem.Length > 0)
                        {
                            foreach (DataRow row in drItem)
                            {
                                iRowCount = row.Table.Rows.IndexOf(row);
                                dtitem.Rows[iRowCount]["Itemcategory"] = Convert.ToString(row.ItemArray[1]);
                                dtitem.Rows[iRowCount]["Itemname"] = Convert.ToString(row.ItemArray[2]);
                                dtitem.Rows[iRowCount]["Quantity"] = Convert.ToInt32(row.ItemArray[3]);
                                dtitem.Rows[iRowCount]["Measurement"] = (row.ItemArray[4]);
                            }

                        }

                        else
                        {
                            iRowCount = dtitem.Rows.Count;
                            
                        }

                    }
                    string KitName = cmbItemName.SelectedItem.Text;
                    int ItemId = int.Parse(cmbItemName.SelectedValue.ToString());
                    if (txtQuantity.Text != "")
                    {
                        OdbcCommand Kit1 = new OdbcCommand();
                        Kit1.CommandType = CommandType.StoredProcedure;
                        Kit1.Parameters.AddWithValue("tblname", "m_inventory inv,m_inventory_kit mk,m_sub_item si,m_sub_itemcategory ic,m_sub_unit u");
                        Kit1.Parameters.AddWithValue("attribute", "mk.item_id,itemname,itemcode,itemcatname,unitname,mk.qty");
                        Kit1.Parameters.AddWithValue("conditionv", "inv.invent_id=mk.invent_id and mk.item_id=si.item_id and inv.itemcat_id=ic.itemcat_id and inv.unit_id=u.unit_id and inv.item_id=" + ItemId + " and inv.rowstatus<>'2'");
                        OdbcDataAdapter dacnt346 = new OdbcDataAdapter(Kit1);
                        DataTable dtt346 = new DataTable();
                        //dacnt346.Fill(dtt346); dt46 = obje.SpDtTbl("CALL selectcond(?,?,?)", User);
                        dtt346 = obje.SpDtTbl("CALL selectcond(?,?,?)", Kit1);
                        #region COMMENTED************
                        //if (dtt346.Rows.Count > 0)
                        //{
                        //    Session["counter"] = dtt346.Rows[0]["counter_id"].ToString();
                        //    counter = "";
                        //}

                        //OdbcCommand Kit1 = new OdbcCommand("SELECT mk.item_id,itemname,itemcode,itemcatname,unitname,mk.qty "

                        //                                    + "FROM m_inventory inv,m_inventory_kit mk,m_sub_item si,m_sub_itemcategory ic,m_sub_unit u "

                        //                                    + "WHERE inv.invent_id=mk.invent_id and mk.item_id=si.item_id and inv.itemcat_id=ic.itemcat_id and inv.unit_id=u.unit_id and inv.item_id=" + ItemId + " and inv.rowstatus<>'2'", conn);

                        //OdbcDataReader KitR = Kit1.ExecuteReader();
                        //while (KitR.Read())
                        #endregion

                        for (int i = 0; i < dtt346.Rows.Count; i++)
                        {
                            iRowCount = dtitem.Rows.Count;
                            dtitem.Rows.Add();
                            Session["dtItem"] = dtitem;
                            Session["dtItem"] = dtgItem.DataSource;
                            int NewQty = 0;
                            int ItQnt = Convert.ToInt32(dtt346.Rows[i]["qty"].ToString());
                            NewQty = ItQnt * Qty;
                            int ItId = Convert.ToInt32(dtt346.Rows[i]["item_id"].ToString());

                            OdbcCommand Kit5 = new OdbcCommand();
                            Kit5.CommandType = CommandType.StoredProcedure;
                            Kit5.Parameters.AddWithValue("tblname", "m_inventory inv,m_sub_item si,m_sub_itemcategory ic,m_sub_unit u");
                            Kit5.Parameters.AddWithValue("attribute", "inv.item_id,itemname,itemcode,itemcatname,unitname");
                            Kit5.Parameters.AddWithValue("conditionv", "inv.itemcat_id=ic.itemcat_id and inv.unit_id=u.unit_id and inv.item_id=" + ItId + " and inv.item_id=si.item_id and inv.rowstatus<>'2'");
                            OdbcDataAdapter da3 = new OdbcDataAdapter(Kit5);
                            DataTable da3a = new DataTable();
                            da3a = obje.SpDtTbl("CALL selectcond(?,?,?)", Kit5);

                            #region COMMENTED****************
                            //OdbcCommand Kit5 = new OdbcCommand("SELECT inv.item_id,itemname,itemcode,itemcatname,unitname "

                            //                                + "FROM m_inventory inv,m_sub_item si,m_sub_itemcategory ic,m_sub_unit u "

                            //                                + "WHERE inv.itemcat_id=ic.itemcat_id and inv.unit_id=u.unit_id and inv.item_id=" + ItId + " and inv.item_id=si.item_id and inv.rowstatus<>'2'", conn);
                            //OdbcDataReader Kity = Kit5.ExecuteReader();
                            //while (Kity.Read())
                            #endregion

                            for(int l=0;l<da3a.Rows.Count;l++)
                            {


                                dtitem.Rows[iRowCount]["Itemcode"] = da3a.Rows[l]["itemcode"].ToString();// SelectedItem.Text;// ddlCat1.SelectedValue;
                                dtitem.Rows[iRowCount]["Itemcategory"] = da3a.Rows[l]["itemcatname"].ToString();//.SelectedItem.Text;
                                dtitem.Rows[iRowCount]["Itemname"] = da3a.Rows[l]["itemname"].ToString();
                                dtitem.Rows[iRowCount]["Measurement"] = da3a.Rows[l]["unitname"].ToString();
                                dtitem.Rows[iRowCount]["Quantity"] = NewQty.ToString();
                            }
                            dtgItem.DataSource = dtitem;
                            dtgItem.DataBind();
                            Session["dtItem"] = dtgItem.DataSource;
                        }                                               

                    }

                   }

                else
                {
                    if (txtCode.Text != "")
                    {

                        iRowCount = dtitem.Rows.Count;
                        string KitName = cmbItemName.SelectedItem.Text;
                        int ItemId = int.Parse(cmbItemName.SelectedValue.ToString());
                        if (txtQuantity.Text != "")
                        {
                            OdbcCommand Kit1 = new OdbcCommand();
                            Kit1.CommandType = CommandType.StoredProcedure;
                            Kit1.Parameters.AddWithValue("tblname", "m_inventory inv,m_inventory_kit mk,m_sub_item si,m_sub_itemcategory ic,m_sub_unit u");
                            Kit1.Parameters.AddWithValue("attribute", "mk.item_id,itemname,itemcode,itemcatname,unitname,mk.qty");
                            Kit1.Parameters.AddWithValue("conditionv", "inv.invent_id=mk.invent_id and mk.item_id=si.item_id and inv.itemcat_id=ic.itemcat_id and inv.unit_id=u.unit_id and inv.item_id=" + ItemId + " and inv.rowstatus<>'2'");
                            OdbcDataAdapter dacnt346 = new OdbcDataAdapter(Kit1);
                            DataTable dtt346 = new DataTable();
                            dtt346 = obje.SpDtTbl("CALL selectcond(?,?,?)", Kit1);

                            #region COMMENTED****************
                            //OdbcCommand Kit2 = new OdbcCommand("SELECT mk.item_id,itemname,itemcode,itemcatname,unitname,mk.qty "

                            //                                    + "FROM m_inventory inv,m_inventory_kit mk,m_sub_item si,m_sub_itemcategory ic,m_sub_unit u "

                            //                                    + "WHERE inv.invent_id=mk.invent_id and mk.item_id=si.item_id and inv.itemcat_id=ic.itemcat_id and inv.unit_id=u.unit_id and inv.item_id=" + ItemId + " and inv.rowstatus<>'2'", conn);

                            //OdbcDataReader KitR2 = Kit2.ExecuteReader();
                            //dtgItem.Visible = true;

                            //while (KitR2.Read())
                            #endregion

                            for(int k=0;k<dtt346.Rows.Count;k++)
                            {
                                iRowCount = dtitem.Rows.Count;
                                dtitem.Rows.Add();
                                Session["dtItem"] = dtitem;
                                Session["dtItem"] = dtgItem.DataSource;
                                int NewQty = 0;
                                int ItQnt = Convert.ToInt32(dtt346.Rows[k]["qty"].ToString());
                                NewQty = ItQnt * Qty;
                                int ItId = Convert.ToInt32(dtt346.Rows[k]["item_id"].ToString());

                                OdbcCommand Kit5 = new OdbcCommand();
                                Kit5.CommandType = CommandType.StoredProcedure;
                                Kit5.Parameters.AddWithValue("tblname", "m_inventory inv,m_sub_item si,m_sub_itemcategory ic,m_sub_unit u");
                                Kit5.Parameters.AddWithValue("attribute", "inv.item_id,itemname,itemcode,itemcatname,unitname");
                                Kit5.Parameters.AddWithValue("conditionv", "inv.itemcat_id=ic.itemcat_id and inv.unit_id=u.unit_id and inv.item_id=" + ItId + " and inv.item_id=si.item_id and inv.rowstatus<>'2'");
                                OdbcDataAdapter da3 = new OdbcDataAdapter(Kit5);
                                DataTable da3a = new DataTable();
                                da3a = obje.SpDtTbl("CALL selectcond(?,?,?)", Kit5);

                                #region COMMENTED*****************

                                //OdbcCommand Kit5 = new OdbcCommand("SELECT inv.item_id,itemname,itemcode,itemcatname,unitname "

                                //                                + "FROM m_inventory inv,m_sub_item si,m_sub_itemcategory ic,m_sub_unit u "

                                //                                + "WHERE inv.itemcat_id=ic.itemcat_id and inv.unit_id=u.unit_id and inv.item_id=" + ItId + " and inv.item_id=si.item_id and inv.rowstatus<>'2'", conn);
                                //OdbcDataReader Kity = Kit5.ExecuteReader();
                                //while (Kity.Read())
                                #endregion

                                for(int p=0;p<da3a.Rows.Count;p++)
                                {


                                    dtitem.Rows[iRowCount]["Itemcode"] = da3a.Rows[p]["itemcode"].ToString();// SelectedItem.Text;// ddlCat1.SelectedValue;
                                    dtitem.Rows[iRowCount]["Itemcategory"] = da3a.Rows[p]["itemcatname"].ToString();//.SelectedItem.Text;
                                    dtitem.Rows[iRowCount]["Itemname"] = da3a.Rows[p]["itemname"].ToString();
                                    dtitem.Rows[iRowCount]["Measurement"] = da3a.Rows[p]["unitname"].ToString();
                                    dtitem.Rows[iRowCount]["Quantity"] = NewQty.ToString();
                                }


                                dtgItem.DataSource = dtitem;
                                dtgItem.DataBind();
                                Session["dtItem"] = dtgItem.DataSource;

                            }

                     }

                    }
                }
            }
            catch
            {

            }

        }

        #region COMMEMTED***********
        //OdbcCommand quantity = new OdbcCommand("SELECT openingstock FROM m_inventory WHERE itemcode='" + txtCode.Text + "' and store_id='" + cmbIssueStore.SelectedValue + "' and rowstatus<>'2'", conn);
        //OdbcDataReader quantityr = quantity.ExecuteReader();
        //if (quantityr.Read())
        //{
        //     openquan = Convert.ToInt32(quantityr["openingstock"].ToString());
        //}
        //int oq=int.Parse(txtQuantity.Text);
        //if (openquan < oq)
        //{
        //    lblOk.Text = "Requested quantity is greater than Opening Stock"; lblHead.Text = "Tsunami ARMS - Confirmation";
        //    pnlOk.Visible = true;
        //    pnlYesNo.Visible = false;
        //    ModalPopupExtender2.Show();
        //    cmbItem.SelectedIndex = -1;
        //    cmbItemName.SelectedIndex = -1;
        //    txtUnit.Text = "";
        //    txtQuantity.Text = "";
        //    txtCode.Text = "";
        //    return;

        //}
        //else if (openquan == 0)
        //{

        //        lblOk.Text = "There is no stock for requested item"; lblHead.Text = "Tsunami ARMS - Warning";
        //        pnlOk.Visible = true;
        //        pnlYesNo.Visible = false;
        //        ModalPopupExtender2.Show();
        //        return;

        //}

        //else
        //{

        //dtitem = additem();
        #endregion
        else
        {
            try
            {
                if (dtitem.Rows.Count > 0)
                {

                    if (txtCode.Text != "")
                    {

                        DataRow[] drItem = dtitem.Select("Itemcode='" + txtCode.Text.ToString() + "'");
                        if (drItem.Length > 0)
                        {
                            foreach (DataRow row in drItem)
                            {
                                iRowCount = row.Table.Rows.IndexOf(row);
                                dtitem.Rows[iRowCount]["Itemcategory"] = Convert.ToString(row.ItemArray[1]);
                                dtitem.Rows[iRowCount]["Itemname"] = Convert.ToString(row.ItemArray[2]);
                                dtitem.Rows[iRowCount]["Quantity"] = Convert.ToInt32(row.ItemArray[3]);
                                dtitem.Rows[iRowCount]["Measurement"] = (row.ItemArray[4]);
                            }

                        }

                        else
                        {
                            iRowCount = dtitem.Rows.Count;
                            dtitem.Rows.Add();

                        }

                    }

                    dtitem.Rows[iRowCount]["Itemcode"] = txtCode.Text.ToString();// SelectedItem.Text;// ddlCat1.SelectedValue;
                    dtitem.Rows[iRowCount]["Itemcategory"] = cmbItem.SelectedItem.ToString();//.SelectedItem.Text;
                    dtitem.Rows[iRowCount]["Itemname"] = cmbItemName.SelectedItem.ToString();
                    dtitem.Rows[iRowCount]["Measurement"] = txtUnit.Text.ToString();
                    if (txtQuantity.Text != "")
                    {
                        dtitem.Rows[iRowCount]["Quantity"] = Convert.ToInt32(txtQuantity.Text);
                    }
                    else
                    {
                        dtitem.Rows[iRowCount]["Quantity"] = Convert.DBNull;
                    }


                }

                else
                {
                    if (txtCode.Text != "")
                    {

                        iRowCount = dtitem.Rows.Count;
                        dtitem.Rows.Add();
                        dtitem.Rows[iRowCount]["Itemcode"] = txtCode.Text.ToString();
                        dtitem.Rows[iRowCount]["Itemcategory"] = cmbItem.SelectedItem.ToString();
                        dtitem.Rows[iRowCount]["Itemname"] = cmbItemName.SelectedItem.ToString();
                        dtitem.Rows[iRowCount]["Measurement"] = txtUnit.Text.ToString();
                        if (txtQuantity.Text != "")
                        {
                            dtitem.Rows[iRowCount]["Quantity"] = Convert.ToInt32(txtQuantity.Text);
                        }
                        else
                        {
                            dtitem.Rows[iRowCount]["Quantity"] = Convert.DBNull;
                        }
                    }
                }
            }
            catch
            {

            }
            dtgItem.DataSource = dtitem;
            dtgItem.DataBind();
        }
       
        Session["dtItem"] = dtgItem.DataSource;
        cmbItem.SelectedIndex = -1;
        cmbItemName.SelectedIndex = -1;
        txtCode.Text = "";
        txtQuantity.Text = "";
        txtUnit.Text = "";
       
        this.ScriptManager1.SetFocus(cmbItemName);
        #endregion


    }

    #region emptystring
    public string emptystring(string s)
    {
        if (s == "")
        {
            s = null;
        }
        return s;
    }
    #endregion


    #region yearmonth date conversion
    //public string yearmonth(string s)
    //{
    //    #region YYYY/MM/DD


    //    if (s != "")
    //    {
    //        // date

    //        if (s[2] == '-' || s[2] == '/')
    //        {
    //            d = s.Substring(0, 2).ToString();
    //        }
    //        else if (s[1] == '-' || s[1] == '/')
    //        {
    //            d = s.Substring(0, 1).ToString();
    //        }
    //        else
    //        {

    //        }


    //        // month  && year


    //        if (s[5] == '-' || s[5] == '/')
    //        {
    //            m = s.Substring(3, 2).ToString();


    //            //year

    //            if (s.Length >= 9)
    //            {
    //                y = s.Substring(6, 4).ToString();
    //            }
    //            else if (s.Length < 9)
    //            {
    //                y = "20" + s.Substring(6, 2).ToString();
    //            }
    //            else
    //            {

    //            }

    //            ///year

    //        }
    //        else if (s[4] == '-' || s[4] == '/')
    //        {
    //            //year

    //            if (s.Length >= 8)
    //            {
    //                y = s.Substring(5, 4).ToString();
    //            }
    //            else if (s.Length < 8)
    //            {
    //                y = "20" + s.Substring(5, 2).ToString();
    //            }
    //            else
    //            {

    //            }

    //            //year


    //            if (s[1] == '-' || s[1] == '/')
    //            {
    //                m = s.Substring(2, 2).ToString();
    //            }
    //            else if (s[2] == '-' || s[2] == '/')
    //            {
    //                m = s.Substring(3, 1).ToString();
    //            }
    //            else
    //            {

    //            }
    //        }
    //        else if (s[3] == '-' || s[3] == '/')
    //        {
    //            if (s[1] == '-' || s[1] == '/')
    //            {
    //                m = s.Substring(2, 1).ToString();
    //            }

    //            //year



    //            if (s.Length >= 7)
    //            {
    //                y = s.Substring(4, 4).ToString();
    //            }
    //            else if (s.Length < 7)
    //            {
    //                y = "20" + s.Substring(4, 2).ToString();
    //            }
    //            else
    //            {

    //            }



    //        }

    //        g = y.ToString() + '/' + m.ToString() + '/' + d.ToString();

    //    }
    //    else
    //    {
    //        g = "";
    //    }
    //    return (g);


    //    #endregion
    //}
    #endregion

    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Request")
        {

            #region Request
            int CountId = -1; ;
            string abc7 = txtRequestNo.Text.ToString();
            string ab7;
            conn = obje.NewConnection();
            OdbcTransaction odbTrans = null;
            try
            {
                if (abc7 != "")
                {
                    ab7 = abc7.Substring(10, 4);
                    a47 = Convert.ToInt32(ab7);

                }
                odbTrans = conn.BeginTransaction();
                OdbcCommand check = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                check.CommandType = CommandType.StoredProcedure;
                check.Parameters.AddWithValue("tblname", "t_inventoryrequest_items i,m_inventory inv");
                check.Parameters.AddWithValue("attribute", "i.reqno,inv.itemcode");
                check.Parameters.AddWithValue("conditionv", "item_status='0' and inv.item_id=i.item_id");
                OdbcDataAdapter da3 = new OdbcDataAdapter(check);
                check.Transaction = odbTrans;
                DataTable dtt = new DataTable();
                da3.Fill(dtt);            

                for (int k = 0; k < dtt.Rows.Count; k++)
                {
                    if ((txtCode.Text.ToString() == dtt.Rows[k]["itemcode"].ToString()) && (abc7 == dtt.Rows[k]["reqno"].ToString()))
                    {

                        lblOk.Text = "This Item is Already Request for the Same Request"; lblHead.Text = "Tsunami ARMS - Warning";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                        return;
                    }

                }
                
                Panel6.Visible = false;
                OdbcCommand cmd = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("tblname", "m_staff");
                cmd.Parameters.AddWithValue("attribute", "dept_id");
                cmd.Parameters.AddWithValue("conditionv", "staffname='" + User + "' and rowstatus<>'2'");
                OdbcDataAdapter da5 = new OdbcDataAdapter(check);
                cmd.Transaction = odbTrans;
                DataTable dtt5 = new DataTable();
                da5.Fill(dtt5);               

                txtRequestOfficer.Text = emptystring(txtRequestOfficer.Text);
                cmbIssueStore.SelectedItem.Text = emptystring(cmbIssueStore.SelectedItem.Text);
                txtDate.Text = emptystring(txtDate.Text);
                cmbItem.SelectedItem.Text = emptystring(cmbItem.SelectedItem.Text.ToString());
                txtIssueOfficer.Text = emptystring(txtIssueOfficer.Text);
                string abc = txtRequestNo.Text.ToString();
                DateTime date = DateTime.Now;
                string dt1 = date.ToString("yyyy-MM-dd") + " " + date.ToString("HH:mm:ss");
                string StId = cmbReqStore.SelectedValue.ToString();
                string StId1 = StId.Substring(0, 1);
                string StCode = StId.Substring(1, 1);

                if (StId1 == "S")
                {
                    CountId = int.Parse(StCode.ToString());
                    Counter = 0;
                }
                else if (StId1 == "T")
                {
                    CountId = int.Parse(StCode.ToString());
                    Counter = 2;
                }
                else if (StId1 == "C")
                {
                    CountId = int.Parse(StCode.ToString());
                    Counter = 1;
                }
                ss = obje.yearmonthdate(txtDate.Text.ToString());
                OdbcCommand cm44 = new OdbcCommand("CALL savedata(?,?)", conn);
                cm44.CommandType = CommandType.StoredProcedure;
                cm44.Parameters.AddWithValue("tblname", "t_inventoryrequest");
                string aaaaa = "'" + abc + "','" + txtRequestOfficer.Text.ToString() + "','" + Counter.ToString() + "'," + CountId + "," + cmbIssueStore.SelectedValue + ",'" + ss + "',null,'" + "0" + "'," + id + ",'" + dt1.ToString() + "'," + id + ",'" + dt1.ToString() + "'";
                cm44.Parameters.AddWithValue("val", "'" + abc + "','" + txtRequestOfficer.Text.ToString() + "','" + txtIssueOfficer.Text.ToString() + "','" + Counter.ToString() + "'," + CountId + "," + cmbIssueStore.SelectedValue + ",'" + ss + "','" + "0" + "'," + id + ",'" + dt1.ToString() + "'," + id + ",'" + dt1.ToString() + "'");
                cm44.Transaction = odbTrans;
                cm44.ExecuteNonQuery();

                dtitem = (DataTable)Session["dtItem"];
                for (int i = 0; i < dtitem.Rows.Count; i++)
                {
                    DataRow[] drSpare = dtitem.Select("Itemcode='" + (dtitem.Rows[i]["Itemcode"]) + "'");
                    if (drSpare.Length > 0)
                    {
                        foreach (DataRow row in drSpare)
                        {
                            try
                            {
                                OdbcCommand cmd90 = new OdbcCommand("CALL selectdata(?,?)", conn);
                                cmd90.CommandType = CommandType.StoredProcedure;
                                cmd90.Parameters.AddWithValue("tblname", "t_inventoryrequest_items");
                                cmd90.Parameters.AddWithValue("attribute", "max(req_itemid)");
                                cmd90.Transaction = odbTrans;
                                OdbcDataAdapter dacnt90 = new OdbcDataAdapter(cmd90);
                                DataTable dtt90 = new DataTable();
                                dacnt90.Fill(dtt90);
                                id1 = int.Parse(dtt90.Rows[0][0].ToString());
                                id1 = id1 + 1;
                            }
                            catch
                            {
                                id1 = 1;
                            }
                            
                            String itname = dtitem.Rows[i]["Itemname"].ToString();
                            OdbcCommand ItName = new OdbcCommand("SELECT item_id from m_sub_item where itemname='" + itname.ToString() + "' and rowstatus<>'2'", conn);
                            ItName.Transaction = odbTrans;
                            OdbcDataReader ItNamer = ItName.ExecuteReader();
                            if (ItNamer.Read())
                            {
                                NewItemId = Convert.ToInt32(ItNamer[0].ToString());
                            }
                            
                            OdbcCommand cm41 = new OdbcCommand("CALL savedata(?,?)", conn);
                            cm41.CommandType = CommandType.StoredProcedure;
                            cm41.Parameters.AddWithValue("tblname", "t_inventoryrequest_items");
                            string a10 = "" + id1 + ",'" + abc + "'," + NewItemId + "," + Convert.ToInt32(dtitem.Rows[i]["Quantity"]) + ",null,null,'" + "0" + "'," + id + ",'" + dt1.ToString() + "'," + id + ",'" + dt1.ToString() + "'";
                            cm41.Parameters.AddWithValue("val", "" + id1 + ",'" + abc + "'," + NewItemId + "," + Convert.ToInt32(dtitem.Rows[i]["Quantity"]) + "," + 0 + "," + 0 + "," + 0 + ",'" + "0" + "'," + id + ",'" + dt1.ToString() + "'," + id + ",'" + dt1.ToString() + "'");
                            cm41.Transaction = odbTrans;
                            cm41.ExecuteNonQuery();
                        }
                    }

                }
                ViewState["action"] = "itemrequest";
                string ssq = ViewState["action"].ToString();
                lblOk.Text = "Item Requested Successfully"; lblHead.Text = "Tsunami ARMS - Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();

                cmbItemName.SelectedIndex = -1;
                cmbItem.SelectedIndex = -1;
                txtCode.Text = "";
                txtQuantity.Text = "";
                txtIssueOfficer.Text = "";
                clear();

                year = Session["year"].ToString();
                OdbcCommand cd = new OdbcCommand("select Max(reqno)from t_inventoryrequest", conn);
                cd.Transaction = odbTrans;
                if (Convert.IsDBNull(cd.ExecuteScalar()) == true)
                {
                    strReqNo = "SrNo/" + year + "/" + "0001";
                    txtRequestNo.Text = strReqNo.ToString();
                }
                else
                {
                   string o1 = cd.ExecuteScalar().ToString();
                   string ab1 = o1.Substring(10, 4);
                    a4 = Convert.ToInt32(ab1);
                    a4 = a4 + 1;
                    if (a4 >= 1000)
                    {
                        strReqNo = "SrNo/" + year + "/" + a4;
                        txtRequestNo.Text = strReqNo.ToString();

                    }
                    else if (a4 >= 100)
                    {
                        strReqNo = "SrNo/" + year + "/0" + a4;
                        txtRequestNo.Text = strReqNo.ToString();
                    }
                    else if (a4 >= 10)
                    {

                        strReqNo = "SrNo/" + year + "/00" + a4;
                        txtRequestNo.Text = strReqNo.ToString();
                    }
                    else if (a4 < 10)
                    {
                        strReqNo = "SrNo/" + year + "/000" + a4;
                        txtRequestNo.Text = strReqNo.ToString();
                    }
                }
                odbTrans.Commit();  
                dtgItem.Visible = true;
                conn.Close();
            }
            catch
            {
                odbTrans.Rollback();
                ViewState["action"] = "NILL";
                okmessage("Tsunami ARMS - Warning", "Error in Requesting ");
            }
            #endregion
           
        }
       

        else if (ViewState["action"].ToString() == "Approve")
        {
            #region approve
            conn = obje.NewConnection();
            string a55;
            int ab;
            DateTime date = DateTime.Now;
            string dt1 = date.ToString("yyyy-MM-dd") + " " + date.ToString("HH:mm:ss");
            string a5 = txtRequestNo.Text.ToString();
            OdbcTransaction odbTrans = null;
            try
            {
                if (a5 != "")
                {
                    a55 = a5.Substring(10, 4);
                    a6 = Convert.ToInt32(a55);
                }


                int flag = 0;
                int Bal, tQty;
                odbTrans = conn.BeginTransaction();
                for (int i = 0; i < dtgItemDetails.Rows.Count; i++)
                {
                    Req = (Session["row"].ToString());
                    CheckBox ch = (CheckBox)dtgItemDetails.Rows[i].FindControl("CheckBox1");
                    if (ch.Checked == true)
                    {
                        TextBox txtQty = (TextBox)dtgItemDetails.Rows[i].FindControl("TextBox3");
                        string str = txtQty.Text;
                        tQty = int.Parse(str);

                        itemId1 = Convert.ToInt32(dtgItemDetails.DataKeys[i].Values[1].ToString());

                        OdbcCommand RequestQty = new OdbcCommand("SELECT req_qty FROM t_inventoryrequest_items WHERE reqno='" + Req + "' and item_id=" + itemId1 + " and item_status='0'", conn);
                        RequestQty.Transaction = odbTrans;
                        OdbcDataReader Requestre = RequestQty.ExecuteReader();
                        if (Requestre.Read())
                        {
                            Reqqty = Convert.ToInt32(Requestre[0].ToString());
                        }
                        OdbcCommand RequestQty1 = new OdbcCommand("SELECT (req_qty-approved_qty) as approved_qty FROM t_inventoryrequest_items WHERE reqno='" + Req + "' and item_id=" + itemId1 + " and item_status='3'", conn);
                        RequestQty1.Transaction = odbTrans;
                        OdbcDataReader Requestre1 = RequestQty1.ExecuteReader();
                        if (Requestre1.Read())
                        {
                            Reqqty = Convert.ToInt32(Requestre1[0].ToString());
                        }
                        OdbcCommand RequestQty5 = new OdbcCommand("SELECT (req_qty-approved_qty) as approved_qty FROM t_inventoryrequest_items WHERE reqno='" + Req + "' and item_id=" + itemId1 + " and item_status='2'", conn);
                        RequestQty5.Transaction = odbTrans;
                        OdbcDataReader Requestre5 = RequestQty5.ExecuteReader();
                        if (Requestre5.Read())
                        {
                            Reqqty = Convert.ToInt32(Requestre5[0].ToString());
                        }

                        Bal = Reqqty - tQty;
                        if (Bal == 0)
                        {


                            OdbcCommand UpQty = new OdbcCommand("call updatedata(?,?,?)", conn);
                            UpQty.CommandType = CommandType.StoredProcedure;
                            UpQty.Parameters.AddWithValue("tablename", "t_inventoryrequest_items");
                            UpQty.Parameters.AddWithValue("valu", "approved_qty=(approved_qty+" + tQty + "),item_status=" + "1" + "");
                            UpQty.Parameters.AddWithValue("convariable", "reqno='" + Req + "' and item_id=" + itemId1 + "");
                            UpQty.Transaction = odbTrans;
                            UpQty.ExecuteNonQuery();                       
                                                       
                        }

                        else if (tQty < Reqqty)
                        {
                            OdbcCommand UpQty1 = new OdbcCommand("call updatedata(?,?,?)", conn);
                            UpQty1.CommandType = CommandType.StoredProcedure;
                            UpQty1.Parameters.AddWithValue("tablename", "t_inventoryrequest_items");
                            UpQty1.Parameters.AddWithValue("valu", "approved_qty=(approved_qty+" + tQty + "),item_status=" + "3" + "");
                            UpQty1.Parameters.AddWithValue("convariable", "reqno='" + Req + "' and item_id=" + itemId1 + "");
                            UpQty1.Transaction = odbTrans;
                            UpQty1.ExecuteNonQuery();  
                        }

                        OdbcCommand cmd4a = new OdbcCommand("select max(req_itemid) from t_inventoryrequest_items_approv", conn);
                        cmd4a.Transaction = odbTrans;
                        if (Convert.IsDBNull(cmd4a.ExecuteScalar()) == true)
                        {
                            ab = 1;
                        }
                        else
                        {
                            ab = Convert.ToInt32(cmd4a.ExecuteScalar());
                            ab = ab + 1;
                        }

                        OdbcCommand InvApp = new OdbcCommand("CALL savedata(?,?)", conn);
                        InvApp.CommandType = CommandType.StoredProcedure;
                        InvApp.Parameters.AddWithValue("tblname", "t_inventoryrequest_items_approv");
                        InvApp.Parameters.AddWithValue("val", "" + ab + ",'" + Req + "'," + itemId1 + "," + tQty + "," + id + ",'" + dt1.ToString() + "'");
                        InvApp.Transaction = odbTrans;
                        InvApp.ExecuteNonQuery();  
                    }

                }

                OdbcCommand Appr5 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                Appr5.CommandType = CommandType.StoredProcedure;
                Appr5.Parameters.AddWithValue("tblname", "t_inventoryrequest_items");
                Appr5.Parameters.AddWithValue("attribute", "req_qty,approved_qty");
                Appr5.Parameters.AddWithValue("conditionv", "reqno='" + Req + "'");
                OdbcDataAdapter da3 = new OdbcDataAdapter(Appr5);
                Appr5.Transaction = odbTrans;
                DataTable dtt = new DataTable();
                da3.Fill(dtt);

                for (int k = 0; k < dtt.Rows.Count; k++)
                {
                    int R10 = Convert.ToInt32(dtt.Rows[k][0].ToString());
                    int A10 = Convert.ToInt32(dtt.Rows[k][1].ToString());
                    if (R10 == A10)
                    {
                        OdbcCommand UpQty3 = new OdbcCommand("call updatedata(?,?,?)", conn);
                        UpQty3.CommandType = CommandType.StoredProcedure;
                        UpQty3.Parameters.AddWithValue("tablename", "t_inventoryrequest");
                        UpQty3.Parameters.AddWithValue("valu", "reqstatus='" + "1" + "'");
                        UpQty3.Parameters.AddWithValue("convariable", "reqno='" + Req + "'");
                        UpQty3.Transaction = odbTrans;
                        UpQty3.ExecuteNonQuery();                        
                        flag = 0;
                    }
                    else if (R10 > A10)
                    {
                        flag = 1;

                    }

                }
                if (flag == 1)
                {
                    OdbcCommand UpQty2 = new OdbcCommand("call updatedata(?,?,?)", conn);
                    UpQty2.CommandType = CommandType.StoredProcedure;
                    UpQty2.Parameters.AddWithValue("tablename", "t_inventoryrequest");
                    UpQty2.Parameters.AddWithValue("valu", "reqstatus='" + "3" + "'");
                    UpQty2.Parameters.AddWithValue("convariable", "reqno='" + Req + "'");
                    UpQty2.Transaction = odbTrans;
                    UpQty2.ExecuteNonQuery();  
                }

                odbTrans.Commit();
                RdoRequest.Checked = false;
                lblOk.Text = "Item Approved for Issuing"; lblHead.Text = "Tsunami ARMS - Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                clear();

                conn.Close();
            #endregion

                ViewState["option"] = "NIL";
                ViewState["action"] = "NIL";
            }
            catch
            {
                odbTrans.Rollback();
                ViewState["action"] = "NILL";
                okmessage("Tsunami ARMS - Warning", "Error in Approving ");
            }

        }

        else if (ViewState["action"].ToString() == "Issue")
        {

            #region issue
            conn = obje.NewConnection();

            DateTime date = DateTime.Now;
            string dt1 = date.ToString("yyyy-MM-dd") + " " + date.ToString("HH:mm:ss");
            OdbcTransaction odbTrans = null;
            int Aqty, itemId1, Bal, Store1, Store2,PartAppr,PartIss;
            decimal OpenStock, OpenStock1, TotalAmount, TotalAmount1;
            int Item1, Item2;
            
            int Gstart, Gend;

            try
            {

                #region AMOUNT
                id = Convert.ToInt32(Session["userid"].ToString());
                odbTrans = conn.BeginTransaction();
                for (int i = 0; i < dtgApproved.Rows.Count; i++)
                {

                    CheckBox ch = (CheckBox)dtgApproved.Rows[i].FindControl("CheckBox2");
                    if (ch.Checked == true)
                    {
                        TextBox txtAty = (TextBox)dtgApproved.Rows[i].FindControl("TextBox5");
                        string str = txtAty.Text;
                        Aqty = int.Parse(str);

                        //itemId1 = dtgApproved.Rows[i][2].ToString();
                        itemId1 = int.Parse(dtgApproved.DataKeys[i].Values[1].ToString());
                        // AppId = int.Parse(dtgApproved.DataKeys[dtgApproved.SelectedRow.RowIndex].Value.ToString());
                        AppId = dtgApproved.DataKeys[i].Values[0].ToString();


                        OdbcCommand ApproveQty = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                        ApproveQty.CommandType = CommandType.StoredProcedure;
                        ApproveQty.Parameters.AddWithValue("tblname", "t_inventoryrequest_items");
                        ApproveQty.Parameters.AddWithValue("attribute", "(approved_qty-issued_qty) as approved_qty,req_qty,approved_qty app");
                        ApproveQty.Parameters.AddWithValue("conditionv", "reqno='" + AppId + "' and item_id=" + itemId1 + "");
                        OdbcDataAdapter dapprove = new OdbcDataAdapter(ApproveQty);
                        ApproveQty.Transaction = odbTrans;
                        DataTable dtta = new DataTable();
                        dapprove.Fill(dtta);                       

                        for (int h = 0; h < dtta.Rows.Count; h++)
                        {
                            Apqqty = Convert.ToInt32(dtta.Rows[h][0].ToString());
                            ApReq = Convert.ToInt32(dtta.Rows[h][1].ToString());
                            AdApp = Convert.ToInt32(dtta.Rows[h][2].ToString());
                        }


                        Bal = Apqqty - Aqty;
                        PartAppr = Aqty + AdApp;
                        PartIss = ApReq - Apqqty;
                        if (Bal == 0 && ApReq == PartAppr && Apqqty == Aqty)
                        {
                            OdbcCommand UpAqty1 = new OdbcCommand("call updatedata(?,?,?)", conn);
                            UpAqty1.CommandType = CommandType.StoredProcedure;
                            UpAqty1.Parameters.AddWithValue("tablename", "t_inventoryrequest_items");
                            UpAqty1.Parameters.AddWithValue("valu", "issued_qty=(issued_qty+" + Aqty + "),item_status=" + "2" + "");
                            UpAqty1.Parameters.AddWithValue("convariable", "reqno='" + AppId + "' and item_id=" + itemId1 + "");
                            UpAqty1.Transaction = odbTrans;
                            UpAqty1.ExecuteNonQuery();   
                        }
                        else if (Bal == 0 && Apqqty > Aqty)
                        {
                            OdbcCommand UpAqty1a = new OdbcCommand("call updatedata(?,?,?)", conn);
                            UpAqty1a.CommandType = CommandType.StoredProcedure;
                            UpAqty1a.Parameters.AddWithValue("tablename", "t_inventoryrequest_items");
                            UpAqty1a.Parameters.AddWithValue("valu", "issued_qty=(issued_qty+" + Aqty + "),item_status=" + "2" + "");
                            UpAqty1a.Parameters.AddWithValue("convariable", "reqno='" + AppId + "' and item_id=" + itemId1 + "");
                            UpAqty1a.Transaction = odbTrans;
                            UpAqty1a.ExecuteNonQuery();  
                        }

                        else if (Bal > 0 && Aqty < Apqqty)
                        {
                            #region COMMENTED******
                            //OdbcCommand UpdQt1 = new OdbcCommand("select item_status FROM t_inventoryrequest_items where WHERE reqno='" + AppId + "' and item_id=" + itemId1 + "", conn);
                            //OdbcDataReader UpQtr1 = UpdQt1.ExecuteReader();
                            //while (UpQtr1.Read())
                            //{
                            //    status = UpQtr1[0].ToString();
                            //}

                            ////string aa = "update t_inventoryrequest_items set issued_qty=(issued_qty+" + Aqty + ",item_status=" + "4" + " WHERE reqno='" + AppId + "' and item_id=" + itemId1 + "";
                            //OdbcCommand UpAqty1 = new OdbcCommand("update t_inventoryrequest_items set issued_qty=(issued_qty+" + Aqty + "),item_status=" + "4" + " WHERE reqno='" + AppId + "' and item_id=" + itemId1 + "", conn);
                            //UpAqty1.ExecuteNonQuery();
                            ////OdbcCommand UpAqty3 = new OdbcCommand("update t_inventoryrequest set reqstatus='" + "4" + "' WHERE reqno='" + AppId + "'", conn);
                            ////UpAqty3.ExecuteNonQuery();
                            #endregion

                            OdbcCommand UpAqty1a = new OdbcCommand("call updatedata(?,?,?)", conn);
                            UpAqty1a.CommandType = CommandType.StoredProcedure;
                            UpAqty1a.Parameters.AddWithValue("tablename", "t_inventoryrequest_items");
                            UpAqty1a.Parameters.AddWithValue("valu", "issued_qty=(issued_qty+" + Aqty + "),item_status=" + "4" + " ");
                            UpAqty1a.Parameters.AddWithValue("convariable", "reqno='" + AppId + "' and item_id=" + itemId1 + "");
                            UpAqty1a.Transaction = odbTrans;
                            UpAqty1a.ExecuteNonQuery();
                          
                        }
                        else if (Bal == 0 && Aqty == Apqqty)
                        {

                            OdbcCommand UpAqty1a = new OdbcCommand("call updatedata(?,?,?)", conn);
                            UpAqty1a.CommandType = CommandType.StoredProcedure;
                            UpAqty1a.Parameters.AddWithValue("tablename", "t_inventoryrequest_items");
                            UpAqty1a.Parameters.AddWithValue("valu", "issued_qty=(issued_qty+" + Aqty + "),item_status=" + "2" + " ");
                            UpAqty1a.Parameters.AddWithValue("convariable", "reqno='" + AppId + "' and item_id=" + itemId1 + "");
                            UpAqty1a.Transaction = odbTrans;
                            UpAqty1a.ExecuteNonQuery();
                        }

                        OdbcCommand IssueStoreq = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                        IssueStoreq.CommandType = CommandType.StoredProcedure;
                        IssueStoreq.Parameters.AddWithValue("tblname", "m_inventory i,t_inventoryrequest_items t,t_inventoryrequest m ,m_sub_store s");
                        IssueStoreq.Parameters.AddWithValue("attribute", "i.stock_qty,t.item_id,m.office_request,m.office_issue");
                        IssueStoreq.Parameters.AddWithValue("conditionv", "i.rowstatus<>'2' and s.store_id=i.store_id and t.reqno=m.reqno and t.item_id=i.item_id and i.item_id=" + itemId1 + " and m.reqno='" + AppId + "'");
                        OdbcDataAdapter dissue = new OdbcDataAdapter(IssueStoreq);
                        IssueStoreq.Transaction = odbTrans;
                        DataTable dttb = new DataTable();
                        dissue.Fill(dttb);
                       
                        if (dttb.Rows.Count > 0)
                        {
                            for (int k = 0; k < dttb.Rows.Count; k++)
                            {
                                OpenStock1 = Convert.ToDecimal(dttb.Rows[k][0].ToString());
                                TotalAmount1 = OpenStock1 - Aqty;
                                Item2 = Convert.ToInt32(dttb.Rows[k]["item_id"].ToString());
                                try
                                {
                                    Store2 = Convert.ToInt32(dttb.Rows[k]["office_issue"].ToString());
                                }
                                catch
                                {
                                    Store2 = 0;
                                }

                                OdbcCommand cm5 = new OdbcCommand("call updatedata(?,?,?)", conn);
                                cm5.CommandType = CommandType.StoredProcedure;
                                cm5.Parameters.AddWithValue("tablename", "m_inventory");
                                cm5.Parameters.AddWithValue("valu", "stock_qty=" + TotalAmount1 + " ");
                                cm5.Parameters.AddWithValue("convariable", "store_id=" + Store2 + " and item_id=" + Item2 + "");
                                cm5.Transaction = odbTrans;
                                cm5.ExecuteNonQuery();   
                            }
                        }

                       
                #endregion

                        int ab, abc;

                        try
                        {
                            TextBox txtStart = (TextBox)dtgApproved.Rows[i].FindControl("TextBox6");
                            string start = txtStart.Text;
                            Gstart = int.Parse(start);
                        }
                        catch
                        {
                            Gstart = 0;
                        }

                        try
                        {
                            TextBox txtend = (TextBox)dtgApproved.Rows[i].FindControl("TextBox7");
                            string end = txtend.Text;
                            Gend = int.Parse(end);
                        }
                        catch
                        {
                            Gend = 0;
                        }

                        OdbcCommand IssStore1 = new OdbcCommand("CALL selectcond(?,?,?)",conn);
                        IssStore1.CommandType = CommandType.StoredProcedure;
                        IssStore1.Parameters.AddWithValue("tblname", "t_inventoryrequest,m_sub_store,m_sub_counter,m_team");
                        IssStore1.Parameters.AddWithValue("attribute", "distinct office_request,case req_from when '0' then 'store' when '1' then 'counter' when '2' then 'team' END as Reqfrom,storename");
                        IssStore1.Parameters.AddWithValue("conditionv", "reqno='" + AppId + "' and (m_sub_store.store_id=t_inventoryrequest.office_request  or m_sub_counter.counter_id=t_inventoryrequest.office_request or m_team.team_id=t_inventoryrequest.office_request)");
                        OdbcDataAdapter dissuea = new OdbcDataAdapter(IssStore1);
                        IssStore1.Transaction = odbTrans;
                        DataTable dttc = new DataTable();
                        dissuea.Fill(dttc);                       

                        if (dttc.Rows.Count > 0)
                        {
                            for (int y = 0; y < dttc.Rows.Count; y++)
                            {
                                string Cou = dttc.Rows[y]["storename"].ToString();
                                OffR = Convert.ToInt32(dttc.Rows[y]["office_request"].ToString());
                                FromOff = dttc.Rows[y]["Reqfrom"].ToString();
                            }
                        }

                        #region COUNTER
                        if (FromOff == "counter")
                        {
                            OdbcCommand cmd4ab = new OdbcCommand("select max(req_itemid) from t_inventoryrequest_items_issue", conn);
                            cmd4ab.Transaction = odbTrans;
                            if (Convert.IsDBNull(cmd4ab.ExecuteScalar()) == true)
                            {
                                ab = 1;
                            }
                            else
                            {
                                ab = Convert.ToInt32(cmd4ab.ExecuteScalar());
                                ab = ab + 1;
                            }
                            OdbcCommand InvIss = new OdbcCommand("CALL savedata(?,?)", conn);
                            InvIss.CommandType = CommandType.StoredProcedure;
                            InvIss.Parameters.AddWithValue("tblname", "t_inventoryrequest_items_issue");
                            InvIss.Transaction = odbTrans;
                            try
                            {
                                InvIss.Parameters.AddWithValue("val", "" + ab + ",'" + txtIssueNo.Text.ToString() + "'," + itemId1 + "," + Aqty + "," + Gstart + "," + Gend + "," + id + ",'" + dt1.ToString() + "'");
                                InvIss.ExecuteNonQuery();
                            }
                            catch
                            {
                                InvIss.Parameters.AddWithValue("val", "" + ab + ",'" + txtIssueNo.Text.ToString() + "'," + itemId1 + "," + Aqty + "," + 0 + "," + 0 + "," + id + ",'" + dt1.ToString() + "'");
                                InvIss.ExecuteNonQuery();

                            }
                            
                        }

                        else
                        {
                            OdbcCommand cmd4ab1 = new OdbcCommand("select max(req_itemid) from t_inventoryrequest_items_issue", conn);
                            cmd4ab1.Transaction = odbTrans;
                            if (Convert.IsDBNull(cmd4ab1.ExecuteScalar()) == true)
                            {
                                ab = 1;
                            }
                            else
                            {
                                ab = Convert.ToInt32(cmd4ab1.ExecuteScalar());
                                ab = ab + 1;
                            }


                            OdbcCommand InvIss1 = new OdbcCommand("CALL savedata(?,?)", conn);
                            InvIss1.CommandType = CommandType.StoredProcedure;
                            InvIss1.Parameters.AddWithValue("tblname", "t_inventoryrequest_items_issue");
                            InvIss1.Transaction = odbTrans;
                            try
                            {
                                InvIss1.Parameters.AddWithValue("val", "" + ab + ",'" + txtIssueNo.Text.ToString() + "'," + itemId1 + "," + Aqty + "," + Gstart + "," + Gend + "," + id + ",'" + dt1.ToString() + "'");
                                InvIss1.ExecuteNonQuery();

                            }
                            catch
                            {
                                InvIss1.Parameters.AddWithValue("val", "" + ab + ",'" + txtIssueNo.Text.ToString() + "'," + itemId1 + "," + Aqty + "," + 0 + "," + 0 + "," + id + ",'" + dt1.ToString() + "'");
                                InvIss1.ExecuteNonQuery();

                            }
                        }
                        #endregion

                    }
                    else { }
                }
                int flag = 0, flg = 0;
                OdbcCommand Appr5 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                Appr5.CommandType = CommandType.StoredProcedure;
                Appr5.Parameters.AddWithValue("tblname", "t_inventoryrequest_items");
                Appr5.Parameters.AddWithValue("attribute", "req_qty,approved_qty,issued_qty");
                Appr5.Parameters.AddWithValue("conditionv", "reqno='" + AppId + "'");
                OdbcDataAdapter dissue1 = new OdbcDataAdapter(Appr5);
                Appr5.Transaction = odbTrans;
                DataTable dttd = new DataTable();
                dissue1.Fill(dttd);
                
                for (int k = 0; k < dttd.Rows.Count; k++)
                {
                    int R10 = Convert.ToInt32(dttd.Rows[k][0].ToString());
                    int A10 = Convert.ToInt32(dttd.Rows[k][1].ToString());
                    int B10 = Convert.ToInt32(dttd.Rows[k][2].ToString());
                    if (R10 == A10 && A10 == B10)
                    {
                        OdbcCommand UpQty3 = new OdbcCommand("update t_inventoryrequest set reqstatus='" + "2" + "' WHERE reqno='" + AppId + "'", conn);
                        UpQty3.Transaction = odbTrans;
                        UpQty3.ExecuteNonQuery();
                        flag = 0;
                        flg = 0;
                    }
                    else if (A10 > B10)
                    {
                        flag = 1;
                        flg = 0;

                    }
                    else if (R10 > A10)
                    {
                        flg = 1;
                        flag = 0;
                    }

                }
                if (flag == 1)
                {
                    OdbcCommand UpQty2 = new OdbcCommand("update t_inventoryrequest set reqstatus='" + "4" + "' WHERE reqno='" + AppId + "'", conn);
                    UpQty2.Transaction = odbTrans;
                    UpQty2.ExecuteNonQuery();
                }
                if (flg == 1)
                {
                    OdbcCommand UpQty4 = new OdbcCommand("update t_inventoryrequest set reqstatus='" + "3" + "' WHERE reqno='" + AppId + "'", conn);
                    UpQty4.Transaction = odbTrans;
                    UpQty4.ExecuteNonQuery();
                }


                if (txtIssueNo.Text.ToString() != "")
                {
                    OdbcCommand InvIss1 = new OdbcCommand("CALL savedata(?,?)", conn);
                    InvIss1.CommandType = CommandType.StoredProcedure;
                    InvIss1.Parameters.AddWithValue("tblname", "t_inventoryrequest_issue");
                    InvIss1.Parameters.AddWithValue("val", "'" + txtIssueNo.Text.ToString() + "','" + AppId + "'," + id + ",'" + dt1.ToString() + "'");
                    InvIss1.Transaction = odbTrans;
                    InvIss1.ExecuteNonQuery();
                  
                }

                odbTrans.Commit();
                RdoApprove.Checked = false;
                ViewState["action"] = "itemissue";
                lblOk.Text = "Approved Item Issued Successfully"; lblHead.Text = "Tsunami ARMS - Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                cmbItem.SelectedIndex = -1;
                cmbItemName.SelectedIndex = -1;
                txtCode.Text = "";
                txtQuantity.Text = "";
                cmbReqStore.SelectedIndex = -1;
                cmbIssueStore.SelectedIndex = -1;
                txtUnit.Text = "";
                clear();
            }
            catch
            {
                odbTrans.Rollback();
                ViewState["action"] = "NILL";
                okmessage("Tsunami ARMS - Warning", "Error in Issuing ");
            }
            #endregion
        }

        }

    protected void  TextBox5_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)(sender as TextBox);
        string str = txt.Text;
        int Eend = int.Parse(str);
        GridViewRow row = (GridViewRow)((sender as TextBox).Parent.Parent as GridViewRow);
        CheckQuantity1(row);
        int Item = int.Parse(dtgApproved.DataKeys[0].Values[1].ToString());
             
        OdbcCommand Control = new OdbcCommand("select control_slno from m_inventory where item_id=" + Item + " and rowstatus<>'2'", conn);
        OdbcDataReader Controlr = Control.ExecuteReader();
        if (Controlr.Read())
        {
            YN = Convert.ToInt32(Controlr[0].ToString());
            Session["controlslno"] = YN.ToString();
        }

        if (YN == 1)
        {
           

        }
        else
        {
            
        }
    }
      

     #region clear
   public void clear()
    {
        #region clear
        cmbItem.SelectedIndex = -1;
        cmbItemName.SelectedIndex = -1;
       
        cmbReqStore.SelectedIndex = -1;
        cmbIssueStore.SelectedIndex = -1;
        cmbTeamCounter.SelectedIndex = -1;
       
        dtgItem.Visible = false;
        txtQuantity.Text = "";
     
        txtCode.Text = "";
        Panel8.Visible = false;
        pnlapprove1.Visible = false;
        lnkis.Visible = false;
        lnkreq.Visible = false;
        lnkr.Visible = false;
      
        dtitem.Rows.Clear();
        txtUnit.Text = "";
        dtgRequestedItems.Visible = false;
        lblIssueNo.Visible = false;
        txtIssueNo.Visible = false;
        txtApprovingOfficer.Visible = false;
        lblApprOfficer.Visible = false;
        lblIssueOfficer.Visible = false;
        txtApprovingOfficer.Visible = false;
        cmbReqStore.SelectedIndex = -1;
        dtgItemDetails.Visible = false;
        dtgApproved.Visible = false;
        dtgAItem.Visible = false;
        pnlapprove1.Visible = false;
        Panel10.Visible = false;
        RdoApprove.Checked = false;
        lblIssueOfficer.Visible = false;
        txtIssueOfficer.Visible = false;
        lblApprOfficer.Visible = false;
        txtApprovingOfficer.Visible = false;
        lblIssueNo.Visible = false;
        txtIssueNo.Visible = false;
        lnkStoreManager.Visible = false;
        LnkDPStockLed.Visible = false;
        lnkPPSL.Visible = false;
        lnkStock.Visible = false;
        RdoApprove.Visible = false;
        RdoRequest.Visible = false;
        pnlItems.Visible = false;
        lblRStore.Visible = false;
        cmbRStore.Visible = false;
        btnRol.Visible = false;
        lnkAuthor.Visible = false;
        lblKStore.Visible = false;
        lblBuilding.Visible = false;
        lblRoomNO.Visible = false;
        cmbKBuilding.Visible = false;
        cmbKStore.Visible = false;
        cmbKRoom.Visible = false;
        btnKeyStockLed.Visible = false;
        lnkKeyStockLedger.Visible = false;
        lnkRol.Visible = false;
        lblStaff.Visible = false;
        cmbStaff.Visible = false;
        btnLed.Visible = false;
        lblStore1.Visible = false;
        cmbStore1.Visible = false;
        LnkDPStockLed.Visible = false;
        lnkPPSL.Visible = false;
        #endregion

    }
       #endregion


    #region REQUESTED ITEM DETAILS
    public void RequestedItems()
    {

        conn = obje.NewConnection();
        dtgRequestedItems.Visible = true;
               
       // OdbcDataAdapter da = new OdbcDataAdapter("SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,o.storename as Store from t_inventoryrequest t,m_sub_store o where (t.reqstatus='0' or t.reqstatus='3') and t.office_request=o.store_id", conn);
        OdbcDataAdapter da = new OdbcDataAdapter("SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,"
                            +"o.storename as Store from t_inventoryrequest t,m_sub_store o where t.reqstatus='0' and t.office_request=o.store_id and "
                            +"req_from='0' union SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,"
                            +"o.counter_no as Store from t_inventoryrequest t,m_sub_counter o where t.reqstatus='0' and t.office_request=o.counter_id and "
                            +"req_from='1' union SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,"
                            +"o.teamname as Store from t_inventoryrequest t,m_team o where t.reqstatus='0' and t.office_request=o.team_id and req_from='2' "
                            +"union SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,o.storename as "
                            +"Store from t_inventoryrequest t,m_sub_store o where t.reqstatus='3' and t.office_request=o.store_id and req_from='0' union "
                            +"SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,o.counter_no as Store "
                            +"from t_inventoryrequest t,m_sub_counter o where t.reqstatus='3' and t.office_request=o.counter_id and req_from='1' union SELECT "
                            +"t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,o.teamname as Store from "
                            +"t_inventoryrequest t,m_team o where t.reqstatus='3' and t.office_request=o.team_id and req_from='2' "
                            + "union SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,o.storename as "
                            + "Store from t_inventoryrequest t,m_sub_store o where t.reqstatus='4' and t.office_request=o.store_id and req_from='0' union "
                            + "SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,o.counter_no as Store "
                            + "from t_inventoryrequest t,m_sub_counter o where t.reqstatus='4' and t.office_request=o.counter_id and req_from='1' union SELECT "
                            + "t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,o.teamname as Store from "
                            + "t_inventoryrequest t,m_team o where t.reqstatus='4' and t.office_request=o.team_id and req_from='2' order by reqno asc ", conn);
        DataSet ds = new DataSet();
        da.Fill(ds, "t_inventoryrequest");
        dtgRequestedItems.DataSource = ds;
        dtgRequestedItems.DataBind();

    }
    #endregion

    #region APPROVED ITEM DETAILS
    public void ApproveItems()
    {
        conn = obje.NewConnection();
        Panel10.Visible = true;
        dtgAItem.Visible = true;
        OdbcDataAdapter da1 = new OdbcDataAdapter("SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,o.storename as Store "
                                                + "from t_inventoryrequest t,m_sub_store o where t.reqstatus='1' and t.office_request=o.store_id and req_from='0' union "
                                                + "SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,o.counter_no as "
                                                 + "Store from t_inventoryrequest t,m_sub_counter o where t.reqstatus='1' and t.office_request=o.counter_id and req_from='1' "
                                                 + "union SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,o.teamname as "
                                                 + "Store from t_inventoryrequest t,m_team o where t.reqstatus='1' and t.office_request=o.team_id and "
                                                 +"req_from='2' "

                                                 + "union SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,"
                                                 + "o.storename as Store from t_inventoryrequest t,m_sub_store o where t.reqstatus='3' and t.office_request="
                                                 + "o.store_id and req_from='0' union SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT"
                                                 + "(t.date_request,'%d-%m-%Y') as Date,o.counter_no as Store from t_inventoryrequest t,m_sub_counter o where "
                                                 + "t.reqstatus='3' and t.office_request=o.counter_id and req_from='1' union SELECT t.reqno as reqno,"
                                                 + "t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,o.teamname as Store from "
                                                 + "t_inventoryrequest t,m_team o where t.reqstatus='3' and t.office_request=o.team_id and req_from='2' "

                                                 +"union SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as "
                                                 +"Date,o.storename as Store from t_inventoryrequest t,m_sub_store o where t.reqstatus='4' and t.office_request="
                                                 +"o.store_id and req_from='0' union SELECT t.reqno as reqno,t.req_officer as Request_Officer,DATE_FORMAT"
                                                 +"(t.date_request,'%d-%m-%Y') as Date,o.counter_no as Store from t_inventoryrequest t,m_sub_counter o where "
                                                 +"t.reqstatus='4' and t.office_request=o.counter_id and req_from='1' union SELECT t.reqno as reqno,"
                                                 +"t.req_officer as Request_Officer,DATE_FORMAT(t.date_request,'%d-%m-%Y') as Date,o.teamname as Store from "
                                                 +"t_inventoryrequest t,m_team o where t.reqstatus='4' and t.office_request=o.team_id and req_from='2'"
                                                 +"order by reqno asc", conn);
         DataSet ds = new DataSet();
         da1.Fill(ds, "t_inventoryrequest");
         dtgAItem.DataSource = ds;
         dtgAItem.DataBind();

     }
    #endregion

     protected void btnre_Click(object sender, EventArgs e)
    {

    }
    protected void btnRequest_Click(object sender, EventArgs e)
    {
        #region request

        lblMsg.Text = "Do you want to Request?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Request";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
        #endregion
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        #region approve request
        int flag=0,flag1=0;
        for (int i = 0; i < dtgItemDetails.Rows.Count; i++)
        { 
          CheckBox ch = (CheckBox)dtgItemDetails.Rows[i].FindControl("CheckBox1");
          if (ch.Checked == true)
          {
              flag = 1;
              
          }
          else
          {
              flag1 = 1;
              
          }
        }
        if (flag==0)
        {
            
            lblOk.Text = "Please Click on Check Box"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        
        }

        lblMsg.Text = "Do you want to Approve?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Approve";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
        #endregion
    }
    protected void btnIssue_Click(object sender, EventArgs e)
    {
        #region issue
        int flag = 0, flag1 = 0;
        for (int i = 0; i < dtgApproved.Rows.Count; i++)
        {
            CheckBox ch = (CheckBox)dtgApproved.Rows[i].FindControl("CheckBox2");
            if (ch.Checked == true)
            {
                flag = 1;

            }
            else
            {
                flag1 = 1;

            }
        }
        if (flag == 0)
        {
            
            lblOk.Text = "Please Click on Check Box"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;

        }

        lblMsg.Text = "Do you want to Issue?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Issue";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);

        #endregion

    }
    protected void rdoItem_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }

    public void Request1()
    {
        #region Request
        string l;
        conn = obje.NewConnection();
        l = dtgRequestedItems.SelectedRow.Cells[1].Text;
        Session["reqnumber"] = l;
        DataTable ds = new DataTable();       
        OdbcCommand Req5 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        Req5.CommandType = CommandType.StoredProcedure;
        Req5.Parameters.AddWithValue("tblname", "m_sub_item n,t_inventoryrequest_items t,m_sub_itemcategory i,m_inventory m");
        Req5.Parameters.AddWithValue("attribute", "distinct t.reqno as reqno,t.item_id as item_id,m.itemcode as Itemcode,i.itemcatname as itemcatname,n.itemname as itemname,t.req_qty as req_qty ");
        Req5.Parameters.AddWithValue("conditionv", "t.reqno='" + l + "' and t.item_id=n.item_id and t.item_status='0' and n.itemcat_id=i.itemcat_id and m.rowstatus<>'2' and m.itemcat_id=i.itemcat_id and m.item_id =n.item_id and req_qty> 0");
        OdbcDataAdapter daA = new OdbcDataAdapter(Req5);
        daA.Fill(ds);
         
        dtgItemDetails.DataSource = ds;
        dtgItemDetails.DataBind();
        OdbcCommand Req6 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        Req6.CommandType = CommandType.StoredProcedure;
        Req6.Parameters.AddWithValue("tblname", "m_sub_item n,t_inventoryrequest_items t,m_sub_itemcategory i,m_inventory m");
        Req6.Parameters.AddWithValue("attribute", "distinct t.reqno as reqno,t.item_id as item_id,m.itemcode as Itemcode,i.itemcatname as itemcatname,n.itemname as itemname,(t.req_qty-t.approved_qty) as req_qty ");
        Req6.Parameters.AddWithValue("conditionv", "t.reqno='" + l + "' and t.item_id=n.item_id and t.item_status='3' and n.itemcat_id=i.itemcat_id and m.rowstatus<>'2' and req_qty > 0 and m.itemcat_id=i.itemcat_id group by item_id");
        OdbcDataAdapter da11 = new OdbcDataAdapter(Req6);

        da11.Fill(ds);           
        dtgItemDetails.DataSource = ds;
        dtgItemDetails.DataBind();

        OdbcCommand Req7 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        Req7.CommandType = CommandType.StoredProcedure;
        Req7.Parameters.AddWithValue("tblname", "m_sub_item n,t_inventoryrequest_items t,m_sub_itemcategory i,m_inventory m");
        Req7.Parameters.AddWithValue("attribute", "distinct t.reqno as reqno,t.item_id as item_id,m.itemcode as Itemcode,i.itemcatname as itemcatname,n.itemname as itemname,(t.req_qty-t.approved_qty) as req_qty");
        Req7.Parameters.AddWithValue("conditionv", "t.reqno='" + l + "' and t.item_id=n.item_id and t.item_status='4' and n.itemcat_id=i.itemcat_id and m.rowstatus<>'2' and req_qty > 0 and m.itemcat_id=i.itemcat_id group by item_id");
        OdbcDataAdapter da14 = new OdbcDataAdapter(Req7);

        da14.Fill(ds);
        dtgItemDetails.DataSource = ds;
        dtgItemDetails.DataBind();


        OdbcCommand Req8 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        Req8.CommandType = CommandType.StoredProcedure;
        Req8.Parameters.AddWithValue("tblname", "m_sub_item n,t_inventoryrequest_items t,m_sub_itemcategory i,m_inventory m");
        Req8.Parameters.AddWithValue("attribute", "distinct t.reqno as reqno,t.item_id as item_id,m.itemcode as Itemcode,i.itemcatname as itemcatname,n.itemname as itemname,(t.req_qty-t.approved_qty) as req_qty");
        Req8.Parameters.AddWithValue("conditionv", "t.reqno='" + l + "' and t.item_id=n.item_id and t.item_status='2' and n.itemcat_id=i.itemcat_id and m.rowstatus<>'2' and req_qty > 0 and m.itemcat_id=i.itemcat_id group by item_id");
        OdbcDataAdapter da15 = new OdbcDataAdapter(Req8);
       

        da15.Fill(ds);             
        dtgItemDetails.DataSource = ds;
        dtgItemDetails.DataBind();
        #endregion
    
    }


    public void Approve()
    {
      
    }


    protected void dtgItemDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void dtgItemDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        
    }
    protected void dtgItemDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
      
    }
    protected void dtgItemDetails_Sorting(object sender, GridViewSortEventArgs e)
    {

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
    }
    protected void btnHidden_Click(object sender, EventArgs e)
    {

    }
    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
       
        if (ViewState["action"].ToString() == "itemrequest")
        {
            #region request

            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
            dtgItem.Visible = false;
            string dd;
            DateTime ds2 = DateTime.Now;
            string transtim = ds2.ToString("dd-MM-yyyy HH-mm");

            string building, room, stat, datte, timme, num,datte1;
            datte1 = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");

            timme = ds2.ToShortTimeString();
            datte = ds2.ToString("dd-MMMM-yyyy");
            dd = ds2.ToString("dd MMM");
            OdbcCommand MaxReqno = new OdbcCommand("SELECT max(reqno) from t_inventoryrequest where reqstatus='0'", conn);
            OdbcDataReader MaxRer = MaxReqno.ExecuteReader();
            if (MaxRer.Read())
            {
                TextReq = MaxRer[0].ToString();
            }

            string ch = "materialrequestnote " + transtim.ToString() + ".pdf";
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
            string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
            Font font8 = FontFactory.GetFont("ARIAL", 9,1);
            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 9);
            //PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            pdfPage page = new pdfPage();
            page.strRptMode = "Material Request";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            
            PdfPTable table = new PdfPTable(10);
            table.TotalWidth = 750f;

            float[] colwidth1 ={ 2, 8, 5, 5, 4, 4,3, 3,3,6 };
            table.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase("Stores Requisition Note ", font12));
            cell.Colspan = 10;
            cell.Border =1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);

            OdbcCommand Req9 = new OdbcCommand();
            Req9.CommandType = CommandType.StoredProcedure;
            Req9.Parameters.AddWithValue("tblname", "t_inventoryrequest_items t,t_inventoryrequest q,m_sub_item i,m_inventory mi,m_sub_itemcategory mc,m_sub_unit mu");
            Req9.Parameters.AddWithValue("attribute", "t.reqno as reqno,q.req_officer,q.req_from,q.iss_officer,q.office_request,q.office_issue,DATE_FORMAT(q.date_request,'%d-%m-%Y') as Date,t.req_qty as req_qty,itemname,itemcode,unitname,mi.itemcode");
            Req9.Parameters.AddWithValue("conditionv", "t.reqno='" + TextReq.ToString() + "' and reqstatus=0 and q.reqno=t.reqno and mi.unit_id=mu.unit_id and t.item_id=i.item_id and t.item_id=mi.item_id and mi.itemcat_id=mc.itemcat_id group by itemcode");
              
            //OdbcCommand receipt = new OdbcCommand("SELECT t.reqno as reqno,q.req_officer,q.req_from,q.iss_officer,q.office_request,q.office_issue,DATE_FORMAT(q.date_request,'%d-%m-%Y') as Date,t.req_qty as req_qty,itemname,itemcode,unitname,mi.itemcode FROM t_inventoryrequest_items t,t_inventoryrequest q,m_sub_item i,m_inventory mi,m_sub_itemcategory mc,m_sub_unit mu WHERE t.reqno='" + TextReq.ToString() + "' and reqstatus=0 and q.reqno=t.reqno and mi.unit_id=mu.unit_id and t.item_id=i.item_id and t.item_id=mi.item_id and mi.itemcat_id=mc.itemcat_id group by itemcode", conn);
            OdbcDataAdapter dar = new OdbcDataAdapter(Req9);
            DataTable dt = new DataTable();
            dt = obje.SpDtTbl("CALL selectcond(?,?,?)", Req9);
            
            int kk = 0; 
            if (dt.Rows.Count > 0)
            {
                for (int jj = 0; jj < dt.Rows.Count; jj++)
                {
                    kk = kk + 1;
                    if (kk == 1)
                    {
                         string ReNo = dt.Rows[jj][0].ToString();
                        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("SR. No: "+ReNo.ToString(), font10)));
                        cell5.Border = 0;
                        cell5.Colspan = 4;
                        table.AddCell(cell5);
                        conn = obje.NewConnection();
                        int ReqFrom = Convert.ToInt32(dt.Rows[jj]["req_from"].ToString());
                        int StoreN = Convert.ToInt32(dt.Rows[jj]["office_request"].ToString());
                        if (ReqFrom == 0)
                        {
                            OdbcCommand stora = new OdbcCommand("SELECT distinct s.storename as Name,store_id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + StoreN + "", conn);
                            OdbcDataReader storra = stora.ExecuteReader();
                            if (storra.Read())
                            {
                               StorName1 = storra[0].ToString();
                                
                            }
                        }
                        else if (ReqFrom == 1)
                        {
                            OdbcCommand stora = new OdbcCommand("SELECT distinct s.counter_no as Name,counter_id from m_sub_counter s where s.rowstatus<>'2' and s.counter_id=" + StoreN + "", conn);
                            OdbcDataReader storra = stora.ExecuteReader();
                            if (storra.Read())
                            {
                                StorName1 = storra[0].ToString();
                               
                            }

                        }
                        else if (ReqFrom == 2)
                        {
                            OdbcCommand stora = new OdbcCommand("SELECT distinct s.teamname as Name,team_id from m_team s where s.rowstatus<>'2' and s.team_id=" + StoreN + "", conn);
                            OdbcDataReader storra = stora.ExecuteReader();
                            if (storra.Read())
                            {
                                StorName1 = storra[0].ToString();                                
                            }
                        }
                        else
                        {                            
                            StorName1 = "";
                        }

                        PdfPCell cell8 = new PdfPCell(new Phrase("Req. Office: " + StorName1, font10));
                        cell8.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell8.Colspan = 3;
                        cell8.Border = 0;
                        table.AddCell(cell8);

                        int OffIssue;
                        try
                        {
                            OffIssue = Convert.ToInt32(dt.Rows[jj]["office_issue"].ToString());
                        }
                        catch
                        {
                           OffIssue = 0;
                        }
                        OdbcCommand IssuOffice = new OdbcCommand("SELECT distinct s.storename as Name,store_id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + OffIssue + "", conn);
                        OdbcDataReader IssOffi = IssuOffice.ExecuteReader();
                        if (IssOffi.Read())
                        {
                            OffName = IssOffi[0].ToString();
                            
                        }
                        else
                        {
                            OffName = "";
                            
                        }
                        PdfPCell cell10 = new PdfPCell(new Phrase("Issuing office: "+OffName, font10));
                        cell10.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell10.Colspan = 3;
                        cell10.Border = 0;
                        table.AddCell(cell10);

                        string Date1 = dt.Rows[jj]["Date"].ToString();
                        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Date : "+dd.ToString(), font10)));
                        cell6.HorizontalAlignment = 0;
                        cell6.Border = 0;
                        cell6.Colspan =4;
                        table.AddCell(cell6);
                       
                        

                        string ReqOffr = dt.Rows[jj]["req_officer"].ToString();
                        PdfPCell cell12 = new PdfPCell(new Phrase("Re. Officer: " + ReqOffr, font10));
                        cell12.Colspan = 4;
                        cell12.Border = 0;
                        cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12);
                        

                        PdfPCell cell131 = new PdfPCell(new Phrase(" ", font10));
                        cell131.Border = 0;
                        cell131.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell131);
                        PdfPCell cell132 = new PdfPCell(new Phrase(" ", font10));
                        cell132.Border = 0;
                        cell132.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell132);

                    }
                }
            }

            PdfPCell cell5a = new PdfPCell(new Phrase(new Chunk("No", font8)));
            cell5a.Rowspan = 2;
            table.AddCell(cell5a);

            PdfPCell cell6a = new PdfPCell(new Phrase(new Chunk("Item Name", font8)));
            cell6a.Rowspan = 2;
            cell6a.HorizontalAlignment = 0;
            table.AddCell(cell6a);

            PdfPCell cell8a = new PdfPCell(new Phrase(new Chunk("Code", font8)));
            cell8a.Rowspan = 2;
            table.AddCell(cell8a);

            PdfPCell cell8b = new PdfPCell(new Phrase(new Chunk("UOM", font8)));
            cell8b.Rowspan = 2;
            table.AddCell(cell8b);

            PdfPCell cell9a = new PdfPCell(new Phrase(new Chunk("Quantity", font8)));
            cell9a.Colspan = 3;
            cell9a.HorizontalAlignment = 1;
            table.AddCell(cell9a);

            PdfPCell cell10a = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
            cell10a.Colspan = 3;
            cell10a.Rowspan = 2;
            table.AddCell(cell10a);

            PdfPCell cell9y = new PdfPCell(new Phrase(new Chunk("Request", font8)));
            table.AddCell(cell9y);
            PdfPCell cell9t = new PdfPCell(new Phrase(new Chunk("Issued", font8)));
            table.AddCell(cell9t);
            PdfPCell cell9r = new PdfPCell(new Phrase(new Chunk("Bal", font8)));
            table.AddCell(cell9r);

            int slno = 0; int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                PdfPTable table1 = new PdfPTable(10);
                if (i > 39)
                {
                    doc.NewPage();
                    PdfPCell cell5ab = new PdfPCell(new Phrase(new Chunk("No", font8)));
                    cell5ab.Rowspan = 2;
                    table.AddCell(cell5ab);

                    PdfPCell cell6ab = new PdfPCell(new Phrase(new Chunk("Item Name", font8)));
                    cell6ab.Rowspan = 2;
                    cell6ab.HorizontalAlignment = 0;
                    table.AddCell(cell6ab);

                    PdfPCell cell8ab = new PdfPCell(new Phrase(new Chunk("Code", font8)));
                    cell8ab.Rowspan = 2;
                    table.AddCell(cell8ab);

                    PdfPCell cell8bb = new PdfPCell(new Phrase(new Chunk("UOM", font8)));
                    cell8bb.Rowspan = 2;
                    table.AddCell(cell8bb);

                    PdfPCell cell9ab = new PdfPCell(new Phrase(new Chunk("Quantity", font8)));
                    cell9ab.Colspan = 3;
                    cell9ab.HorizontalAlignment = 1;
                    table.AddCell(cell9ab);

                    PdfPCell cell10ab = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                    cell10ab.Colspan = 3;
                    cell10ab.Rowspan = 2;
                    table.AddCell(cell10ab);
                    PdfPCell cell9yi = new PdfPCell(new Phrase(new Chunk("Request", font8)));
                    table.AddCell(cell9yi);
                    PdfPCell cell9ti = new PdfPCell(new Phrase(new Chunk("Issued", font8)));
                    table.AddCell(cell9ti);
                    PdfPCell cell9ri = new PdfPCell(new Phrase(new Chunk("Bal", font8)));
                    table.AddCell(cell9ri);
                    i = 0;
                    doc.Add(table1);
                }

                slno = slno + 1;
                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font9)));
                table.AddCell(cell11);
                string itn = dr["itemname"].ToString();
                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(itn.ToString(), font9)));
                table.AddCell(cell12);
                string ic = dr["itemcode"].ToString();
                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(ic.ToString(), font9)));
                table.AddCell(cell14);
                string un = dr["unitname"].ToString();
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(un.ToString(), font9)));
                table.AddCell(cell15);
                int rq = Convert.ToInt32(dr["req_qty"].ToString());
                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(rq.ToString(), font9)));
                table.AddCell(cell16);
                PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk(" ", font9)));
                table.AddCell(cell16a);
                PdfPCell cell16b = new PdfPCell(new Phrase(new Chunk(" ", font9)));
                table.AddCell(cell16b);
                PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
                cell17.Colspan = 3;
                table.AddCell(cell17);                
                i++;
            }


            PdfPTable table5 = new PdfPTable(3);
            PdfPCell cellab = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellab.Border = 1;
            table5.AddCell(cellab);
            PdfPCell cellac = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellac.Border = 1;
            table5.AddCell(cellac);
            PdfPCell cellav = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellav.Border = 1;

            table5.AddCell(cellav);

            PdfPCell cellaq = new PdfPCell(new Phrase(new Chunk("Prepared by", font8)));
            //cellaq.Border = 1;
            cellaq.Border = 0;
            table5.AddCell(cellaq);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Approved by", font8)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);
            PdfPCell cellae = new PdfPCell(new Phrase(new Chunk("Issued by", font8)));
            cellae.Border = 0;
            table5.AddCell(cellae);
            doc.Add(table);
            doc.Add(table5);
            doc.Close();
              Random r = new Random();
              string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Requested Item Detials"; 
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

            #endregion

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";

        
        }
        else if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }

        else if (ViewState["action"].ToString() == "itemissue")
        {
            #region Issue Items Report
            conn = obje.NewConnection();

            DateTime ds2 = DateTime.Now;
            string datte,datte1,timme;
            datte1 = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
            timme = ds2.ToShortTimeString();
            datte = ds2.ToString("dd-MMMM-yyyy");
            string dd = ds2.ToString("dd MMM");
            OdbcCommand MaxReqno = new OdbcCommand("SELECT max(issueno) from t_inventoryrequest_issue", conn);
            OdbcDataReader MaxRer = MaxReqno.ExecuteReader();
            if (MaxRer.Read())
            {
                TextReq = MaxRer[0].ToString();
            }

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string ch = "materialissuenote" + transtim.ToString() + ".pdf";

            string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
            Font font9 = FontFactory.GetFont("ARIAL", 9);
            Font font12 = FontFactory.GetFont("ARIAL", 12,1);
            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font8 = FontFactory.GetFont("ARIAL", 9,1);
 
            pdfPage page = new pdfPage();
            page.strRptMode = "Material Issue";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

            PdfPTable table = new PdfPTable(9);
            table.TotalWidth = 750f;

            float[] colwidth1 ={ 2, 8, 3, 5, 5, 4, 4, 3, 5 };
            table.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase("Stores Issue Note  ", font12));
            cell.Colspan = 9;
            cell.Border = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);

            OdbcCommand IssueStore = new OdbcCommand();
            IssueStore.CommandType = CommandType.StoredProcedure;
            IssueStore.Parameters.AddWithValue("tblname", "t_inventoryrequest q,t_inventoryrequest_items t,m_sub_item i,m_inventory mi,m_sub_itemcategory mc,m_sub_unit mu,t_inventoryrequest_issue iss,t_inventoryrequest_items_issue ist");
            IssueStore.Parameters.AddWithValue("attribute", "t.reqno,q.req_officer,q.req_from,q.iss_officer,q.office_request,q.office_issue,t.req_qty,t.issued_qty,(t.req_qty-t.issued_qty) as balance,itemname,itemcode,unitname,mi.itemcode,ist.start_slno,ist.end_slno");
            IssueStore.Parameters.AddWithValue("conditionv", "iss.issueno='" + TextReq.ToString() + "' and q.reqno=t.reqno and mi.unit_id=mu.unit_id and t.item_id=i.item_id and t.item_id=mi.item_id and mi.itemcat_id=mc.itemcat_id and iss.reqno=q.reqno and iss.issueno=ist.issueno and ist.item_id=mi.item_id group by itemcode");
                        
            OdbcDataAdapter IssSt = new OdbcDataAdapter(IssueStore);
            DataTable dt = new DataTable();
            dt = obje.SpDtTbl("CALL selectcond(?,?,?)", IssueStore);
            conn = obje.NewConnection();
            if (dt.Rows.Count > 0)
            {
                int kk = 0;
                for (int jj = 0; jj < dt.Rows.Count; jj++)
                {
                    kk = kk + 1;
                    if (kk == 1)
                    {
                        string ReNo = dt.Rows[jj][0].ToString();
                        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("SIN No: " + ReNo.ToString(), font10)));
                        cell5.Border = 0;
                        cell5.Colspan = 2;
                        table.AddCell(cell5);
                        
                        int ReqFrom = Convert.ToInt32(dt.Rows[jj]["req_from"].ToString());
                        int StoreN = Convert.ToInt32(dt.Rows[jj]["office_request"].ToString());

                        if (ReqFrom == 0)
                        {
                            OdbcCommand stora = new OdbcCommand("SELECT distinct s.storename as Name from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + StoreN + "", conn);
                            OdbcDataReader storra = stora.ExecuteReader();
                            if (storra.Read())
                            {
                                StorName = storra[0].ToString();                                
                            }
                        }
                        else if (ReqFrom == 1)
                        {
                            OdbcCommand stora = new OdbcCommand("SELECT distinct s.counter_no as Name from m_sub_counter s where s.rowstatus<>'2' and s.counter_id=" + StoreN + "", conn);
                            OdbcDataReader storra = stora.ExecuteReader();
                            if (storra.Read())
                            {
                                StorName = storra[0].ToString();                                
                            }

                        }
                        else if (ReqFrom == 2)
                        {
                            OdbcCommand stora = new OdbcCommand("SELECT distinct s.teamname as Name from m_team s where s.rowstatus<>'2' and s.team_id=" + StoreN + "", conn);
                            OdbcDataReader storra = stora.ExecuteReader();
                            if (storra.Read())
                            {
                                StorName = storra[0].ToString();                                
                            }
                        }
                        else
                        {
                            StorName = "";
                        }

                        PdfPCell cell8 = new PdfPCell(new Phrase("Req. Office: " + StorName.ToString(), font10));
                        cell8.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell8.Colspan = 3;
                        cell8.Border = 0;
                        table.AddCell(cell8);

                        int OffIssue;
                        try
                        {
                            OffIssue = Convert.ToInt32(dt.Rows[jj]["office_issue"].ToString());
                        }
                        catch
                        {
                             OffIssue = 0;
                        }
                        string OffName;
                        OdbcCommand IssuOffice = new OdbcCommand("SELECT distinct s.storename as Name from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + OffIssue + "", conn);
                        OdbcDataReader IssOffi = IssuOffice.ExecuteReader();
                        if (IssOffi.Read())
                        {
                            OffName = IssOffi[0].ToString();
                            
                        }
                        else
                        {
                            OffName = "";                            
                        }

                        PdfPCell cell10 = new PdfPCell(new Phrase("Issuing office: " + OffName.ToString(), font10));
                        cell10.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell10.Colspan = 4;
                        cell10.Border = 0;
                        table.AddCell(cell10);

                                              
                        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Date : " + dd.ToString(), font10)));
                        cell6.HorizontalAlignment = 0;
                        cell6.Border = 0;
                        cell6.Colspan = 3;
                        table.AddCell(cell6);                        


                        string ReqOffr = dt.Rows[jj]["req_officer"].ToString();
                        PdfPCell cell13 = new PdfPCell(new Phrase("Re. Officer :" + ReqOffr.ToString(), font10));
                        cell13.Colspan = 4;
                        cell13.Border = 0;
                        cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell13);

                        PdfPCell cell131 = new PdfPCell(new Phrase("Date ", font10));
                        cell131.Border = 0;
                        cell131.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell131);
                        PdfPCell cell132 = new PdfPCell(new Phrase(" ", font10));
                        cell132.Border = 0;
                        cell132.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell132);

                    }
                }
            }
            PdfPCell cell5a = new PdfPCell(new Phrase(new Chunk("No", font8)));
            cell5a.Rowspan = 2;
            table.AddCell(cell5a);

            PdfPCell cell6a = new PdfPCell(new Phrase(new Chunk("Item Name", font8)));
            cell6a.Colspan = 1;
            cell6a.Rowspan = 2;
            cell6a.HorizontalAlignment = 0;
            table.AddCell(cell6a);
            PdfPCell cell6k = new PdfPCell(new Phrase(new Chunk("Item Code", font8)));
            cell6k.Colspan = 1;
            cell6k.Rowspan = 2;
            cell6k.HorizontalAlignment = 0;
            table.AddCell(cell6k);

            PdfPCell cell8a = new PdfPCell(new Phrase(new Chunk("SR No", font8)));            
            cell8a.Rowspan = 2;
            table.AddCell(cell8a);

            PdfPCell cell8b = new PdfPCell(new Phrase(new Chunk("UOM", font8)));           
            cell8b.Rowspan = 2;
            table.AddCell(cell8b);

            PdfPCell cell9a = new PdfPCell(new Phrase(new Chunk("Quantity", font8)));
            cell9a.Colspan = 3;
            cell9a.HorizontalAlignment = 1;
            table.AddCell(cell9a);

            PdfPCell cell10a = new PdfPCell(new Phrase(new Chunk("Remark", font8)));           
            cell10a.Rowspan = 2;
            table.AddCell(cell10a);

            PdfPCell cell9y = new PdfPCell(new Phrase(new Chunk("Request", font8)));
            table.AddCell(cell9y);
            PdfPCell cell9t = new PdfPCell(new Phrase(new Chunk("Issued", font8)));
            table.AddCell(cell9t);
            PdfPCell cell9r = new PdfPCell(new Phrase(new Chunk("Bal", font8)));
            table.AddCell(cell9r);

            int slno = 0; int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                PdfPTable table1 = new PdfPTable(9);
                if (i > 39)
                {
                    doc.NewPage();
                    PdfPCell cell5ab = new PdfPCell(new Phrase(new Chunk("No", font8)));
                    cell5ab.Rowspan = 2;
                    table.AddCell(cell5ab);

                    PdfPCell cell6ab = new PdfPCell(new Phrase(new Chunk("Item Name", font8)));
                    cell6ab.Rowspan = 2;
                    cell6ab.HorizontalAlignment = 0;
                    table.AddCell(cell6ab);

                    PdfPCell cell6abk = new PdfPCell(new Phrase(new Chunk("Item Code", font8)));
                    cell6abk.Rowspan = 2;
                    cell6abk.HorizontalAlignment = 0;
                    table.AddCell(cell6abk);

                    PdfPCell cell8ab = new PdfPCell(new Phrase(new Chunk(" SR No", font8)));
                    cell8ab.Rowspan = 2;
                    table.AddCell(cell8ab);

                    PdfPCell cell8bb = new PdfPCell(new Phrase(new Chunk("UOM", font8)));
                    cell8bb.Rowspan = 2;
                    table.AddCell(cell8bb);

                    PdfPCell cell9ab = new PdfPCell(new Phrase(new Chunk("Quantity", font8)));
                    cell9ab.Colspan = 3;
                    cell6ab.HorizontalAlignment = 1;
                    table.AddCell(cell9ab);

                    PdfPCell cell10ab = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                    cell10ab.Rowspan = 2;
                    table.AddCell(cell10ab);
                    PdfPCell cell9yi = new PdfPCell(new Phrase(new Chunk("Request", font8)));
                    table.AddCell(cell9yi);
                    PdfPCell cell9ti = new PdfPCell(new Phrase(new Chunk("Issued", font8)));
                    table.AddCell(cell9ti);
                    PdfPCell cell9ri = new PdfPCell(new Phrase(new Chunk("Bal", font8)));
                    table.AddCell(cell9ri);
                    i = 0;
                    doc.Add(table1);
                }

                slno = slno + 1;
                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font9)));
                table.AddCell(cell11);
                string itn = dr["itemname"].ToString();
                string ICode = dr["itemcode"].ToString();
                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(itn.ToString(), font9)));
                table.AddCell(cell12);
                PdfPCell cell12k = new PdfPCell(new Phrase(new Chunk(ICode.ToString(), font9)));
                table.AddCell(cell12k);
                string ic = dr["reqno"].ToString();
                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(ic.ToString(), font9)));
                //cell14.Colspan = 3;
                table.AddCell(cell14);
                string un = dr["unitname"].ToString();
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(un.ToString(), font9)));
                //cell15.Colspan = 2;
                table.AddCell(cell15);
                int rq = Convert.ToInt32(dr["req_qty"].ToString());
                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(rq.ToString(), font9)));
                table.AddCell(cell16);
                int iq = Convert.ToInt32(dr["issued_qty"].ToString());
                PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk(iq.ToString(), font9)));
                table.AddCell(cell16a);
                int bal = Convert.ToInt32(dr["balance"].ToString());
                PdfPCell cell16b = new PdfPCell(new Phrase(new Chunk(bal.ToString(), font9)));
                table.AddCell(cell16b);
                 int st = Convert.ToInt32(dr["start_slno"].ToString());
                 int en = Convert.ToInt32(dr["end_slno"].ToString());
                 if (st == 0 && en == 0)
                 {
                     PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
                     cell17.Colspan = 3;
                     table.AddCell(cell17);
                 }

                 else
                 {
                     try
                     {

                         PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("Sl no " + st.ToString() + " - " + en.ToString(), font9)));
                         cell17.Colspan = 3;
                         table.AddCell(cell17);

                     }
                     catch
                     {
                         PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
                         cell17.Colspan = 3;
                         table.AddCell(cell17);
                     }
                 }
                i++;
            }
            PdfPTable table5 = new PdfPTable(3);
            PdfPCell cellab = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellab.Border = 1;
            table5.AddCell(cellab);
            PdfPCell cellac = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellac.Border = 1;
            table5.AddCell(cellac);
            PdfPCell cellav = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellav.Border = 1;

            table5.AddCell(cellav);

            PdfPCell cellaq = new PdfPCell(new Phrase(new Chunk("Issued by", font8)));
            //cellaq.Border = 1;
            cellaq.Border = 0;
            table5.AddCell(cellaq);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Received by", font8)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);
            PdfPCell cellae = new PdfPCell(new Phrase(new Chunk("Posted by", font8)));
            cellae.Border = 0;
            table5.AddCell(cellae);
            doc.Add(table);
            doc.Add(table5);
            doc.Close();
        
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Stock Requestition Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

            
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            #endregion
        }
        else if (ViewState["action"].ToString() == "flag")
        {
            lblMsg.Text = "Do you want to Approve?"; lblHead.Text = "Tsunami ARMS- Confirmation";
            ViewState["action"] = "Approve";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        
        }
            
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        #region report button
        Panel8.Visible = false;
        clear();
        pnlItems.Visible = true;
        lnkStoreManager.Visible = true;
        lnkStock.Visible = true;
        lnkRol.Visible = true;
        LnkDPStockLed.Visible = true;
        lnkPPSL.Visible = true;
        lnkAuthor.Visible = true;
        lnkKeyStockLedger.Visible = true;
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        // OdbcDataAdapter Stock = new OdbcDataAdapter("SELECT t.item_id,itemname FROM m_sub_item i,t_inventoryrequest_items t,m_inventory v WHERE t.item_id=i.item_id and i.rowstatus<>'2'and v.openingstock> '0' group by t.item_id", con);
        OdbcDataAdapter Stock = new OdbcDataAdapter("select distinct storename as Sname, CAST(CONCAT('S',`store_id`)  as CHAR) as id from  "
            + "t_inventoryrequest inv,m_sub_store ms where inv.office_request=ms.store_id and ms.rowstatus<>'2' union SELECT distinct counter_no as "
            + "Sname,CAST(CONCAT('C',`counter_id`) as CHAR) as id from t_inventoryrequest t,t_inventoryrequest_items i,m_sub_counter c where i.reqno=t.reqno "
            + "and c.counter_id=t.office_request", conn);
        DataTable ds1 = new DataTable();
        Stock.Fill(ds1);
        DataRow row = ds1.NewRow();
        row["id"] = "-1";
        row["Sname"] = "--Select--";
        ds1.Rows.InsertAt(row, 0);
        cmbStockRegistry.DataSource = ds1;
        cmbStockRegistry.DataBind();

        OdbcCommand ROL = new OdbcCommand();
        ROL.CommandType = CommandType.StoredProcedure;
        ROL.Parameters.AddWithValue("tblname", "m_inventory mi,m_sub_item i,m_sub_store s,m_sub_unit u");
        ROL.Parameters.AddWithValue("attribute", "storename,mi.store_id");
        ROL.Parameters.AddWithValue("conditionv", "reorderlevel > stock_qty and mi.item_id=i.item_id and mi.rowstatus<>'2' and s.store_id=mi.store_id and mi.rowstatus<>'2' and i.rowstatus<>'2' and u.unit_id=mi.unit_id and u.rowstatus<>2");
        OdbcDataAdapter ROL1 = new OdbcDataAdapter(ROL);

        #region COMMENTED***********************
        //OdbcDataAdapter ROL = new OdbcDataAdapter("select storename,mi.store_id from m_inventory mi,m_sub_item i,m_sub_store s,m_sub_unit u "
        //     + "where reorderlevel > stock_qty and mi.item_id=i.item_id and mi.rowstatus<>'2' and s.store_id=mi.store_id and mi.rowstatus<>'2' and "
        //     + "i.rowstatus<>'2' and u.unit_id=mi.unit_id and u.rowstatus<>2", conn);
        #endregion

        DataTable ds2 = new DataTable();
        //ROL1.Fill(ds2);
        ds2 = obje.SpDtTbl("CALL selectcond(?,?,?)", ROL);
        DataRow row1 = ds2.NewRow();
        row1["store_id"] = "-1";
        row1["storename"] = "--Select--";
        ds2.Rows.InsertAt(row1, 0);
        cmbRStore.DataSource = ds2;
        cmbRStore.DataBind();

        OdbcCommand RoomKey = new OdbcCommand();
        RoomKey.CommandType = CommandType.StoredProcedure;
        RoomKey.Parameters.AddWithValue("tblname", "m_sub_building b,m_room r");
        RoomKey.Parameters.AddWithValue("attribute", "distinct buildingname,r.build_id");
        RoomKey.Parameters.AddWithValue("conditionv", "r.build_id=b.build_id and r.room_id in (SELECT room_id from t_roomvacate v,t_roomallocation a where "
            +"return_key='0' and a.alloc_id=v.alloc_id and season_id=(select season_id from m_season where curdate()>= startdate and enddate>=curdate()))");
        OdbcDataAdapter RoomKey2 = new OdbcDataAdapter(RoomKey);

        #region COMMENTED*****************
        //OdbcDataAdapter RoomKey = new OdbcDataAdapter("SELECT distinct buildingname,r.build_id from m_sub_building b,m_room r where r.build_id=b.build_id and r.room_id "
        //      + "in (SELECT room_id from t_roomvacate v,t_roomallocation a where return_key='0' and a.alloc_id=v.alloc_id and season_id=(select season_id "
        //      +"from m_season where curdate()>= startdate and enddate>=curdate()))",conn);
        #endregion

        DataTable ds3 = new DataTable();
        //RoomKey2.Fill(ds3);
        ds3 = obje.SpDtTbl("CALL selectcond(?,?,?)", RoomKey);
        DataRow row3 = ds3.NewRow();
        row3["build_id"] = "-1";
        row3["buildingname"] = "--Select--";
        ds3.Rows.InsertAt(row3, 0);
        cmbKBuilding.DataSource = ds3;
        cmbKBuilding.DataBind();

        OdbcDataAdapter StoreKey = new OdbcDataAdapter("SELECT counter_no as store,counter_id as id from t_inventoryrequest t,t_inventoryrequest_items i,m_sub_counter c "
             +"where  i.reqno=t.reqno and c.counter_id=t.office_request and req_from=1 and item_id=(SELECT item_id from m_sub_item where itemname='Key' and "
             +"rowstatus<>2) "
       +"UNION "
             +"SELECT storename as store,store_id as id from t_inventoryrequest t,t_inventoryrequest_items i,m_sub_store o where req_from=2 and o.store_id="
             +"office_request  and t.reqno=i.reqno and item_id=(SELECT item_id from m_sub_item where itemname='Key' and rowstatus<>2) group by req_from,"
             +"office_request", conn);
        DataTable ds4 = new DataTable();
        StoreKey.Fill(ds4);
        DataRow row4 = ds4.NewRow();
        row4["id"] = "-1";
        row4["store"] = "--Select--";
        ds4.Rows.InsertAt(row4, 0);
        cmbKStore.DataSource = ds4;
        cmbKStore.DataBind();

        OdbcDataAdapter StorePass = new OdbcDataAdapter("select distinct storename as Sname, CAST(CONCAT('S',`store_id`)  as CHAR) as id from  "
            +"t_inventoryrequest inv,m_sub_store ms where inv.office_request=ms.store_id and ms.rowstatus<>'2' UNION SELECT distinct counter_no as "
            +"Sname,CAST(CONCAT('C',`counter_id`) as CHAR) as id from t_inventoryrequest t,t_inventoryrequest_items i,m_sub_counter c where i.reqno=t.reqno "
            +"and c.counter_id=t.office_request", conn);
        DataTable ds5 = new DataTable();
        StorePass.Fill(ds5);
        DataRow row5 = ds5.NewRow();
        row5["id"] = "-1";
        row5["Sname"] = "--Select--";
        ds5.Rows.InsertAt(row5, 0);
        cmbPStore.DataSource = ds5;
        cmbPStore.DataBind();
        conn.Close();

        #endregion
    }
    protected void lnkmaterial_Click(object sender, EventArgs e)
    {

    }
    protected void lnkstockregister_Click(object sender, EventArgs e)
    {

    }
    protected void lnkreceipt_Click(object sender, EventArgs e)
    {

    }
    protected void lnkissue_Click(object sender, EventArgs e)
    {

    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtQuantity_TextChanged(object sender, EventArgs e)
    {
        btnApprove.Enabled = true;
        btnIssue.Enabled = true;
    }
    protected void lnkMaterial_Click(object sender, EventArgs e)
    {
        #region Material Issue
        //int rno;
        //string ab9;
        //DateTime tt;

        //if (conn.State == ConnectionState.Closed)
        //{
        //    conn.ConnectionString = strConnection;
        //    conn.Open();
        //}

        //string hh = txtRequestNo.Text.ToString();

        //if (hh != "")
        //{
        //    ab9 = hh.Substring(9, 4);
        //    a9 = Convert.ToInt32(ab9);

        //}

        //Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        //string pdfFilePath = Server.MapPath(".") + "/pdf/material request.pdf";
        //Font font8 = FontFactory.GetFont("ARIAL", 9);
        ////PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //pdfPage page = new pdfPage();
        //PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //wr.PageEvent = page;
        //doc.Open();
        //PdfPTable table = new PdfPTable(8);

        //float[] headers = { 100, 230, 350, 400, 430, 480, 500, 550 };
        //table.SetWidths(headers);
        //table.WidthPercentage = 100;
        //table.TotalWidth = 550f;
        //table.LockedWidth = true;


        //int y9 = Convert.ToInt32(Session["year"].ToString());


        //OdbcCommand creq = new OdbcCommand("SELECT t.reqno,t.req_officer,o.storename,t.date_request,u.deptname,i.itemname,inv.req_qty,inv.approved_qty,inv.issued_qty,(inv.req_qty-inv.issued_qty) as Balance FROM t_inventoryrequest t,t_inventoryrequest_items inv,m_sub_item i,m_sub_department u,m_sub_store o where t.reqno=inv.reqno and t.userdept_id=u.dept_id and inv.item_id=i.item_id and t.reqstatus=2 and t.office_request=o.store_id", conn);
        //OdbcDataReader crrr = creq.ExecuteReader();
        //while (crrr.Read())
        //{
        //    u9 = Convert.ToInt32(crrr["reqno"].ToString());
        //    string us1 = crrr["req_officer"].ToString();
        //    if (u9 >= 1000)
        //    {
        //        uu = "REQ/" + y9 + "/" + u9;
        //    }
        //    else if (u9 >= 100)
        //    {
        //        uu = "REQ/" + y9 + "/0" + u9;
        //    }
        //    else if (u9 >= 10)
        //    {
        //        uu = "REQ/" + y9 + "/00" + u9;
        //    }
        //    else if (u9 >= 0)
        //    {
        //        uu = "REQ/" + y9 + "/000" + u9;
        //    }
        
        //PdfPCell cell = new PdfPCell(new Phrase("MATERIAL REQUEST FORM", font8));
        //cell.Colspan = 8;
        //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //table.AddCell(cell);
        //string us1 = Session["username"].ToString();

        //PdfPCell cells0 = new PdfPCell(new Phrase("Req.No:" + uu.ToString(), font8));
        //cells0.Colspan = 2;
        //cells0.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //table.AddCell(cells0);

      

        //PdfPCell cells02 = new PdfPCell(new Phrase("Request Officer:", font8));
        //cells02.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
        //table.AddCell(cells02);

        //PdfPCell cells2a = new PdfPCell(new Phrase(us1, font8));
        //cells2a.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //table.AddCell(cells2a);


        //PdfPCell cells2ad = new PdfPCell(new Phrase("Request Office:", font8));
        //cells2ad.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
        //table.AddCell(cells2ad);

        //string dep = crrr["deptname"].ToString();

        //PdfPCell cells2d = new PdfPCell(new Phrase(dep, font8));
        //cells2d.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
        //table.AddCell(cells2d);

        //PdfPCell cells2ae = new PdfPCell(new Phrase("Date:", font8));
        //cells2ae.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
        //table.AddCell(cells2ae);
        //DateTime dd = DateTime.Parse(crrr["date_request"].ToString());
        //string redate = dd.ToString("dd-MM-yyyy");
        //PdfPCell cells2af = new PdfPCell(new Phrase(redate.ToString(), font8));
        //cells2af.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //table.AddCell(cells2af);
        //PdfPCell cells2ag = new PdfPCell(new Phrase("", font8));
        
        //cells2ag.Colspan = 8;
        //table.AddCell(cells2ag);

        //PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Sl.No", font8)));
        //cell1.Colspan = 2;
        //cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //table.AddCell(cell1);
        //PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Item Code", font8)));
        ////cell2.Border = 0;
        //// cell2.Colspan = 2;
        //cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //table.AddCell(cell2);
        //PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Item Description", font8)));
        ////cell3.Border = 0;
        //cell3.Colspan = 2;
        //cell3.HorizontalAlignment = 0;
        //table.AddCell(cell3);

        //PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Quantity", font8)));
        ////cell4.Border = 0;
        //cell4.Colspan = 3;
        //cell4.HorizontalAlignment = 1;
        //table.AddCell(cell4);


        //PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("", font8)));
        ////cell5.Border = 0;
        //cell5.Colspan = 2;
        //cell5.HorizontalAlignment = 0;
        //table.AddCell(cell5);
        //PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("", font8)));
        ////cell6.Border = 0;
        ////cell6.Colspan = 2;
        //cell6.HorizontalAlignment = 0;
        //table.AddCell(cell6);
        //PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("", font8)));
        //// cell7.Border = 0;
        //cell7.Colspan = 2;
        //cell7.HorizontalAlignment = 0;
        //table.AddCell(cell7);
        //PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Requested", font8)));
        //table.AddCell(cell8);
        //PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Issued", font8)));
        //table.AddCell(cell9);
        //PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Balance", font8)));
        //table.AddCell(cell10);

        //PdfPCell cell111 = new PdfPCell(new Phrase(new Chunk("", font8)));
        ////cell111.Border = 1;
        //cell111.Colspan = 8;
        //table.AddCell(cell111);


        //OdbcCommand cmd35 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        //cmd35.CommandType = CommandType.StoredProcedure;
        //cmd35.Parameters.AddWithValue("tblname", "material");
        //cmd35.Parameters.AddWithValue("attribute", "*");
        //cmd35.Parameters.AddWithValue("conditionv", "slno='" + a9 + "'");

        //OdbcDataAdapter da5 = new OdbcDataAdapter(cmd35);
        //DataTable dt = new DataTable();
        //da5.Fill(dt);

        //int slno = 0; int i = 0;
        //foreach (DataRow dr in dt.Rows)
        //{

        //    slno = slno + 1;
        //    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
        //    //cell11.Border = 0;
        //    cell11.Colspan = 2;
        //    table.AddCell(cell11);
        //    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["code"].ToString(), font8)));
        //    //cell12.Border = 0;
        //    //cell12.Colspan = 2;
        //    table.AddCell(cell12);
        //    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["name"].ToString(), font8)));
        //    cell13.Colspan = 2;
        //    table.AddCell(cell13);
        //    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["rquantity"].ToString(), font8)));
        //    table.AddCell(cell14);

        //    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["iquantity"].ToString(), font8)));
        //    table.AddCell(cell15);
        //    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["balance"].ToString(), font8)));
        //    table.AddCell(cell16);

        //}
        //doc.Add(table);
        //doc.Close();
        ////System.Diagnostics.Process.Start(pdfFilePath);
        //Random r = new Random();
        //string PopUpWindowPage = "print.aspx?reportname=material request.pdf&Title=Material Request ";
        //string Script = "";
        //Script += "<script id='PopupWindow'>";
        //Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        //Script += "confirmWin.Setfocus()</script>";
        //if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
        //    Page.RegisterClientScriptBlock("PopupWindow", Script);
        #endregion
    }
    protected void dtgRequestedItems_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region Requested Items
        Panel8.Visible = true;
       
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        dtgItemDetails.Visible = true;
        btnApprove.Visible = true;
        Req = dtgRequestedItems.SelectedRow.Cells[1].Text;
        Session["row"] = Req;
        txtApprovingOfficer.Visible = true;
        lblApprOfficer.Visible = true;
        btnApprove.Visible = true;

        OdbcCommand Grid = new OdbcCommand();
        Grid.CommandType = CommandType.StoredProcedure;
        Grid.Parameters.AddWithValue("tblname", "t_inventoryrequest");
        Grid.Parameters.AddWithValue("attribute", "reqno,req_officer,req_from,iss_officer,office_request,office_issue,DATE_FORMAT(date_request,'%d-%m-%Y') as Date");
        Grid.Parameters.AddWithValue("conditionv", "reqno='" + Req.ToString() + "' and reqstatus=0");
        OdbcDataAdapter Grid1 = new OdbcDataAdapter(Grid);
        DataTable dtg = new DataTable();
        dtg = obje.SpDtTbl("CALL selectcond(?,?,?)", Grid);

        #region COMMENTED****************
        //OdbcCommand Grid = new OdbcCommand("select reqno,req_officer,req_from,iss_officer,office_request,office_issue,DATE_FORMAT(date_request,'%d-%m-%Y') as Date FROM t_inventoryrequest where reqno='"+Req.ToString()+"' and reqstatus=0", conn);
        //OdbcDataReader Gridr = Grid.ExecuteReader();
        //while (Gridr.Read())
        #endregion
        conn = obje.NewConnection();
        for (int k=0;k<dtg.Rows.Count;k++)
        {
            txtRequestNo.Text = dtg.Rows[k]["reqno"].ToString();
            txtRequestOfficer.Text = dtg.Rows[k]["req_officer"].ToString();
            txtDate.Text = dtg.Rows[k]["Date"].ToString();
            int rs = Convert.ToInt32(dtg.Rows[k]["office_request"].ToString());
            int iss;
            try
            {
                iss = Convert.ToInt32(dtg.Rows[k]["office_issue"].ToString());
            }
            catch
            {
                 iss = 0;
            }
            int ReqFr = Convert.ToInt32(dtg.Rows[k]["req_from"].ToString());
            if (ReqFr == 0)
            {               
                OdbcCommand stor = new OdbcCommand("SELECT distinct s.storename as Name,CAST(concat('S',`store_id`) as CHAR) as Id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + rs + "", conn);
                OdbcDataReader storr = stor.ExecuteReader();
                if (storr.Read())
                {
                    cmbReqStore.SelectedItem.Text = storr["Name"].ToString();
                    cmbReqStore.SelectedValue = storr["Id"].ToString();
                }
            }
            else if (ReqFr == 1)
            {

                OdbcCommand stor = new OdbcCommand("SELECT distinct s.counter_no as Name,CAST(concat('C',`counter_id`) as CHAR) as Id from m_sub_counter s where s.rowstatus<>'2' and s.counter_id=" + rs + "", conn);
                OdbcDataReader storr = stor.ExecuteReader();
                if (storr.Read())
                {
                    cmbReqStore.SelectedItem.Text = storr["Name"].ToString();
                    cmbReqStore.SelectedValue = storr["Id"].ToString();
                }
            }
            else if (ReqFr == 2)
            {

                OdbcCommand stor = new OdbcCommand("SELECT distinct s.teamname as Name,CAST(concat('T',`team_id`) as CHAR) as Id from m_team s where s.rowstatus<>'2' and s.team_id=" + rs + "", conn);
                OdbcDataReader storr = stor.ExecuteReader();
                if (storr.Read())
                {
                    cmbReqStore.SelectedItem.Text = storr["Name"].ToString();
                    cmbReqStore.SelectedValue = storr["Id"].ToString();
                }
            }
                        
            OdbcCommand storis = new OdbcCommand("SELECT distinct s.storename as Sname,s.store_id as Id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + iss + "", conn);
            OdbcDataReader storris = storis.ExecuteReader();
            if (storris.Read())
            {
                cmbIssueStore.SelectedItem.Text = storris["Sname"].ToString();
                cmbIssueStore.SelectedValue = storris["Id"].ToString();
            }
        }
        Request1();
        #endregion

    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        #region View button click

                RdoRequest.Checked = true;
                RdoApprove.Visible = true;
                RdoRequest.Visible = true;
                RequestedItems();
                dtgItem.Visible = false;
                Panel3.Visible = true;
                dtgItemDetails.Visible = false;
                dtgRequestedItems.Visible = true;
                dtgApproved.Visible = false;
                dtgAItem.Visible = false;
                btnIssue.Visible = false;
                btnApprove.Visible = false;
                pnlapprove1.Visible = false;
                Panel10.Visible = false;
                RdoApprove.Checked = false;
                lblIssueOfficer.Visible = false;
                txtIssueOfficer.Visible = false;
                lblApprOfficer.Visible = false;
                txtApprovingOfficer.Visible = false;
                lblIssueNo.Visible = false;
                txtIssueNo.Visible = false;
                lnkStoreManager.Visible = false;
                LnkDPStockLed.Visible = false;
                lnkPPSL.Visible = false;
                lnkStock.Visible = false;
                LnkDPStockLed.Visible = false;
                lnkPPSL.Visible = false;
       
        #endregion

    }
    protected void TextBox2_TextChanged(object sender, EventArgs e)
    {        
    }
    protected void cmbIssueStore_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {
        #region COMMENTED**********
        ////OdbcCommand IssueItem = new OdbcCommand("SELECT t.item_id,n.itemname,c.itemcatname,t.req_qty FROM t_inventoryrequest_items t,m_sub_item n,m_sub_itemcategory c,m_inventory iwhere t.item_id=i.item_id and i.store_id=1 and i.store_id=2", conn);
        //if (conn.State == ConnectionState.Closed)
        //{
        //    conn.ConnectionString = strConnection;
        //    conn.Open();
        //}
        //string StId = cmbReqStore.SelectedValue.ToString();
        ////int STid = Convert.ToInt32(cmbReqStore.SelectedValue.ToString());
        ////string StId = STid.ToString();
        //string StId1 = StId.Substring(0, 1);
        //string StCode = StId.Substring(1, 1);

        //cmbItemName.Items.Add("--select--");
        
        //if (StId1 == "S")
        //{
        //    storeId1 = int.Parse(StCode.ToString());
        //}
        //else if (StId1 == "T")
        //{
        //    TeamId1 = int.Parse(StCode.ToString());
        //}
        ////int storeId=int.Parse(cmbReqStore.SelectedValue);
        
        ////OdbcCommand store=new OdbcCommand("SELECT store_id  FROM m_sub_store WHERE store_id="+cmbReqStore.SelectedValue+" and rowstatus<>'2'",conn);
        //OdbcCommand store = new OdbcCommand("SELECT distinct store_id  FROM m_sub_store WHERE store_id=" +storeId1  + " and rowstatus<>'2'", conn);
        //OdbcDataReader storer=store.ExecuteReader();
        //if (storer.Read())
        //{

        //    sId=Convert.ToInt32(storer[0].ToString());
            
        //}
        //if (storeId1 == sId)
        //{
        //    //SqlDataSource5.SelectCommand = "SELECT distinct i.item_id,n.itemname FROM m_sub_item n,m_sub_itemcategory c,m_inventory i,m_sub_store s where n.itemcat_id=c.itemcat_id and n.item_id=i.item_id and i.itemcat_id=c.itemcat_id and i.store_id=s.store_id and i.itemcat_id=n.itemcat_id and i.rowstatus<>'2' and c.rowstatus<>'2'";

        //    OdbcDataAdapter StoreName = new OdbcDataAdapter("SELECT distinct i.item_id as item_id,n.itemname as itemname FROM m_sub_item n,m_sub_itemcategory c,m_inventory i,m_sub_store s where n.itemcat_id=c.itemcat_id and n.item_id=i.item_id and i.itemcat_id=c.itemcat_id and i.store_id=s.store_id and i.itemcat_id=n.itemcat_id and i.rowstatus<>'2' and c.rowstatus<>'2'", conn);
        //    DataTable ds1 = new DataTable();
        //    //StoreName.Fill(ds, "m_inventory");
        //    DataColumn colID = ds1.Columns.Add("item_id", System.Type.GetType("System.Int32"));
        //    DataColumn colNo = ds1.Columns.Add("itemname", System.Type.GetType("System.String"));
        //    DataRow row = ds1.NewRow();
        //    row["item_id"] = "-1";
        //    row["itemname"] = "--Select--";
        //    ds1.Rows.InsertAt(row, 0);
        //    StoreName.Fill(ds1);

        //    cmbItemName.DataSource = ds1;
        //    cmbItemName.DataBind();
            
        //}
        //else if (storeId1 != sId)
        //{

        //    OdbcCommand Team = new OdbcCommand("SELECT team_id FROM m_team_inventory WHERE team_id=" + TeamId1 + "", conn);
        //    OdbcDataReader Teamr = Team.ExecuteReader();
        //    if (Teamr.Read())
        //    {
        //        tId = Convert.ToInt32(Teamr[0].ToString());
        //    }
        //    if (TeamId1 == tId)
        //    {
        //        //SqlDataSource5.SelectCommand = "SELECT distinct i.item_id,n.itemname FROM m_sub_item n,m_sub_itemcategory c,m_inventory i,m_team_inventory mi WHERE n.itemcat_id=c.itemcat_id and n.item_id=i.item_id and i.itemcat_id=c.itemcat_id and i.itemcat_id=n.itemcat_id and i.rowstatus<>'2' and c.rowstatus<>'2' and mi.item_id=i.item_id and n.rowstatus<>'2' and mi.item_id=n.item_id";

        //       // SqlDataSource4.SelectCommand = "SELECT distinct c.itemcat_id,c.itemcatname FROM m_sub_item n,m_sub_itemcategory c,m_inventory i,m_team_inventory mi WHERE n.itemcat_id=c.itemcat_id and n.item_id=i.item_id and i.itemcat_id=c.itemcat_id and i.itemcat_id=n.itemcat_id and i.rowstatus<>'2' and c.rowstatus<>'2' and mi.item_id=i.item_id and n.rowstatus<>'2' and mi.item_id=n.item_id";
        //        OdbcDataAdapter StoreName1 = new OdbcDataAdapter("SELECT distinct i.item_id as item_id,n.itemname as itemname FROM m_sub_item n,m_sub_itemcategory c,m_inventory i,m_sub_store s where n.itemcat_id=c.itemcat_id and n.item_id=i.item_id and i.itemcat_id=c.itemcat_id and i.store_id=s.store_id and i.itemcat_id=n.itemcat_id and i.rowstatus<>'2' and c.rowstatus<>'2'", conn);
        //        DataTable ds1 = new DataTable();
               
        //        DataColumn colID = ds1.Columns.Add("item_id", System.Type.GetType("System.Int32"));
        //        DataColumn colNo = ds1.Columns.Add("itemname", System.Type.GetType("System.String"));
        //        DataRow row = ds1.NewRow();
        //        row["item_id"] = "-1";
        //        row["itemname"] = "--Select--";
        //        ds1.Rows.InsertAt(row, 0);
        //        StoreName1.Fill(ds1);

        //        cmbItemName.DataSource = ds1;
        //        cmbItemName.DataBind();
              
        //    }
        //}
        //else
        //{

        //    OdbcDataAdapter StoreName2 = new OdbcDataAdapter("SELECT distinct ii.item_id as item_id,n.itemname as itemname FROM m_sub_item n,m_sub_itemcategory c,m_inventory i,m_sub_store s where n.itemcat_id=c.itemcat_id and n.item_id=i.item_id and i.itemcat_id=c.itemcat_id and i.store_id=s.store_id and i.itemcat_id=n.itemcat_id and i.rowstatus<>'2' and c.rowstatus<>'2'", conn);
        //    DataTable ds1a = new DataTable();
        //    //StoreName.Fill(ds, "m_inventory");
        //    DataColumn colID = ds1a.Columns.Add("item_id", System.Type.GetType("System.Int32"));
        //    DataColumn colNo = ds1a.Columns.Add("itemname", System.Type.GetType("System.String"));
        //    DataRow row = ds1a.NewRow();
        //    row["item_id"] = "-1";
        //    row["itemname"] = "--Select--";
        //    ds1a.Rows.InsertAt(row, 0);
        //    StoreName2.Fill(ds1a);

        //    cmbItemName.DataSource = ds1a;
        //    cmbItemName.DataBind();

        //   // SqlDataSource5.SelectCommand = "SELECT distinct i.item_id,n.itemname FROM m_sub_item n,m_sub_itemcategory c,m_inventory i WHERE n.itemcat_id=c.itemcat_id and n.item_id=i.item_id and i.itemcat_id=c.itemcat_id and i.itemcat_id=n.itemcat_id and i.rowstatus<>'2' and c.rowstatus<>'2' and n.rowstatus<>'2'";

        //    //SqlDataSource5.SelectParameters["item_id"].DefaultValue = cmbIssueStore.SelectedValue;

        //   //SqlDataSource4.SelectCommand = "SELECT distinct c.itemcat_id,c.itemcatname FROM m_sub_item n,m_sub_itemcategory c,m_inventory i WHERE n.itemcat_id=c.itemcat_id and n.item_id=i.item_id and i.itemcat_id=c.itemcat_id and i.itemcat_id=n.itemcat_id and i.rowstatus<>'2' and c.rowstatus<>'2' and n.rowstatus<>'2'";

        //    //SqlDataSource4.SelectParameters["item_id"].DefaultValue = cmbIssueStore.SelectedValue;
        //}
        #endregion
    }
    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {        
    }
    protected void rdoViewRequest_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region Radio button click
        if (rdoViewRequest.Text == "Requested")
        {
            dtgRequestedItems.Visible = true;
            RequestedItems();
            pnlapprove.Visible = false;
            
        }
        else if (rdoViewRequest.Text == "Approved")
        {
            pnlapprove1.Visible = true;
            Panel8.Visible = false;
            dtgItem.Visible = false;
            dtgRequestedItems.Visible = false;
            lblIssueOfficer.Visible = true;
            txtIssueOfficer.Visible = true;
            Approve();

        }
        #endregion
    }
    protected void dtgApproved_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region  COMMENTED ******
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        //string Areqno;
        //txtIssueOfficer.Visible = true;
        //lblIssueOfficer.Visible = true;
        //for (int i = 0; i < dtgApproved.Rows.Count; i++)
        //{
        //    GridViewRow row = dtgApproved.Rows[i];

        //    bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("CheckBox2")).Checked;

        //    if (isChecked)
        //    {
        //        Areqno = dtgApproved.DataKeys[i].Values[0].ToString();
        //        OdbcCommand AppLoad = new OdbcCommand("SELECT * from t_inventoryrequest WHERE reqno='" + Areqno + "' and reqstatus='" + "0" + "'", conn);
        //        OdbcDataReader AppLoadr = AppLoad.ExecuteReader();
        //        while (AppLoadr.Read())
        //        {

        //            txtRequestNo.Text = AppLoadr["reqno"].ToString();
        //            txtRequestOfficer.SelectedText = AppLoadr["req_officer"].ToString();
        //            txtRequestOfficer.SelectedValue = AppLoadr["req_officer"].ToString();


        //            txtDate.Text = AppLoadr["Date"].ToString();
        //            int rs = Convert.ToInt32(AppLoadr["office_request"].ToString());
        //            int iss = Convert.ToInt32(AppLoadr["office_issue"].ToString());
        //            int ReqFr = Convert.ToInt32(AppLoadr["req_from"].ToString());
        //            if (ReqFr == 0)
        //            {
        //                //CAST(CONCAT('S',`store_id`) as CHAR)as Id
        //                OdbcCommand stor1 = new OdbcCommand("SELECT distinct s.storename as Name,CAST(concat('S',`store_id`) as CHAR) as Id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + rs + "", conn);
        //                OdbcDataReader storr1 = stor1.ExecuteReader();
        //                if (storr1.Read())
        //                {
        //                    cmbReqStore.SelectedText = storr1["Name"].ToString();
        //                    cmbReqStore.SelectedValue = storr1["Id"].ToString();
        //                }
        //            }
        //            else if (ReqFr == 1)
        //            {

        //                OdbcCommand stor1 = new OdbcCommand("SELECT distinct s.counter_no as Name,CAST(concat('C',`counter_id`) as CHAR) as Id from m_sub_counter s where s.rowstatus<>'2' and s.counter_id=" + rs + "", conn);
        //                OdbcDataReader storr1 = stor1.ExecuteReader();
        //                if (storr1.Read())
        //                {
        //                    cmbReqStore.SelectedText = storr1["Name"].ToString();
        //                    cmbReqStore.SelectedValue = storr1["Id"].ToString();
        //                }
        //            }
        //            else if (ReqFr == 2)
        //            {

        //                OdbcCommand stor1 = new OdbcCommand("SELECT distinct s.team as Name,CAST(concat('T',`team_id`) as CHAR) as Id from m_team s where s.rowstatus<>'2' and s.team_id=" + rs + "", conn);
        //                OdbcDataReader storr1 = stor1.ExecuteReader();
        //                if (storr1.Read())
        //                {
        //                    cmbReqStore.SelectedText = storr1["Name"].ToString();
        //                    cmbReqStore.SelectedValue = storr1["Id"].ToString();
        //                }
        //            }


        //            string aaa = "SELECT distinct s.storename,s.store_id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + iss + "";
        //            OdbcCommand storis1 = new OdbcCommand("SELECT distinct s.storename as Sname,s.store_id as Id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + iss + "", conn);
        //            OdbcDataReader storris1 = storis1.ExecuteReader();
        //            if (storris1.Read())
        //            {
        //                cmbIssueStore.SelectedText = storris1["Sname"].ToString();
        //                cmbIssueStore.SelectedValue = storris1["Id"].ToString();
        //            }

        //        }
        //    }
        //}
        #endregion

    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((sender as CheckBox).Parent.Parent as GridViewRow);
        CheckQuantity(row);
    }

    protected void TextBox3_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)(sender as TextBox);
        string str = txt.Text;
        GridViewRow row = (GridViewRow)((sender as TextBox).Parent.Parent as GridViewRow);
        CheckQuantity(row);
    }

    protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
    {

        #region  Serial number controlled or not
        GridViewRow row = (GridViewRow)((sender as CheckBox).Parent.Parent as GridViewRow);
        CheckQuantity1(row);
       
        string itemQn1 = dtgApproved.DataKeys[0].Values[1].ToString();
        string itemRe = dtgApproved.DataKeys[0].Values[0].ToString();
        int itemQn = int.Parse(itemQn1);
        int ITit;
        string Areqno;
        txtIssueOfficer.Visible = true;
        lblIssueOfficer.Visible = true;
        for (int i = 0; i < dtgApproved.Rows.Count; i++)
        {
            GridViewRow rowa = dtgApproved.Rows[i];

            bool isChecked = ((System.Web.UI.WebControls.CheckBox)rowa.FindControl("CheckBox2")).Checked;

            if (isChecked)
            {
                Areqno = dtgApproved.DataKeys[i].Values[0].ToString();
                ITit = int.Parse(dtgApproved.DataKeys[i].Values[1].ToString());
                //Areqno=dtgApproved.DataKeys[dtgApproved.SelectedRow.RowIndex].Values[0].ToString();
                //ITit = Convert.ToInt32(dtgApproved.DataKeys[dtgApproved.SelectedRow.RowIndex].Values[1].ToString());
               // Convert.ToInt32(dtgNonOccupiedReserved.DataKeys[dtgNonOccupiedReserved.SelectedRow.RowIndex].Value.ToString());

                OdbcCommand AppLoad = new OdbcCommand();
                AppLoad.CommandType = CommandType.StoredProcedure;
                AppLoad.Parameters.AddWithValue("tblname", "t_inventoryrequest_items t,t_inventoryrequest q");
                AppLoad.Parameters.AddWithValue("attribute", "t.reqno as reqno,q.req_officer,q.req_from,q.iss_officer,q.office_request,q.office_issue,DATE_FORMAT(q.date_request,'%d-%m-%Y') as Date,t.req_qty as req_qty");
                AppLoad.Parameters.AddWithValue("conditionv", "t.reqno='" + Areqno + "' and q.reqno=t.reqno");
                OdbcDataAdapter AppLoadr = new OdbcDataAdapter(AppLoad);
                DataTable dtg = new DataTable();
                //AppLoadr.Fill(dtg);
                dtg = obje.SpDtTbl("CALL selectcond(?,?,?)", AppLoad);

                #region COMMENTED*****************
                //OdbcCommand AppLoad = new OdbcCommand("SELECT t.reqno as reqno,q.req_officer,q.req_from,q.iss_officer,q.office_request,q.office_issue,DATE_FORMAT(q.date_request,'%d-%m-%Y') as Date,t.req_qty as req_qty FROM t_inventoryrequest_items t,t_inventoryrequest q WHERE t.reqno='"+Areqno+"' and q.reqno=t.reqno", conn);
                //OdbcDataReader AppLoadr = AppLoad.ExecuteReader();
                //while (AppLoadr.Read())
                #endregion
                conn = obje.NewConnection();
                for (int k=0;k<dtg.Rows.Count;k++)
                {

                    txtRequestNo.Text = dtg.Rows[k]["reqno"].ToString();
                    txtRequestOfficer.Text = dtg.Rows[k]["req_officer"].ToString();
                    txtDate.Text = dtg.Rows[k]["Date"].ToString();
                    int iss;
                    int rs = Convert.ToInt32(dtg.Rows[k]["office_request"].ToString());
                    try
                    {
                        iss = Convert.ToInt32(dtg.Rows[k]["office_issue"].ToString());
                    }
                    catch
                    {
                       iss = 0;
                    }
                    int ReqFr = Convert.ToInt32(dtg.Rows[k]["req_from"].ToString());
                    if (ReqFr == 0)
                    {
              
                        OdbcCommand stor1 = new OdbcCommand("SELECT distinct s.storename as Name,CAST(concat('S',`store_id`) as CHAR) as Id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + rs + "", conn);
                        OdbcDataReader storr1 = stor1.ExecuteReader();
                        if (storr1.Read())
                        {
                            cmbReqStore.SelectedItem.Text = storr1["Name"].ToString();
                            cmbReqStore.SelectedValue = storr1["Id"].ToString();
                        }
                    }
                    else if (ReqFr == 1)
                    {

                        OdbcCommand stor1 = new OdbcCommand("SELECT distinct s.counter_no as Name,CAST(concat('C',`counter_id`) as CHAR) as Id from m_sub_counter s where s.rowstatus<>'2' and s.counter_id=" + rs + "", conn);
                        OdbcDataReader storr1 = stor1.ExecuteReader();
                        if (storr1.Read())
                        {
                            cmbReqStore.SelectedItem.Text = storr1["Name"].ToString();
                            cmbReqStore.SelectedValue = storr1["Id"].ToString();
                        }
                    }
                    else if (ReqFr == 2)
                    {

                        OdbcCommand stor1 = new OdbcCommand("SELECT distinct s.teamname as Name,CAST(concat('T',`team_id`) as CHAR) as Id from m_team s where s.rowstatus<>'2' and s.team_id=" + rs + "", conn);
                        OdbcDataReader storr1 = stor1.ExecuteReader();
                        if (storr1.Read())
                        {
                            cmbReqStore.SelectedItem.Text = storr1["Name"].ToString();
                            cmbReqStore.SelectedValue = storr1["Id"].ToString();
                        }
                    }

                    OdbcCommand storis1 = new OdbcCommand("SELECT distinct s.storename as Sname,s.store_id as Id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + iss + "", conn);
                    OdbcDataReader storris1 = storis1.ExecuteReader();
                    if (storris1.Read())
                    {
                        cmbIssueStore.SelectedItem.Text = storris1["Sname"].ToString();
                        cmbIssueStore.SelectedValue = storris1["Id"].ToString();
                    }
                }
                
        OdbcCommand Control = new OdbcCommand("select control_slno from m_inventory where item_id=" + ITit + " and rowstatus<>'2'", conn);
        OdbcDataReader Controlr = Control.ExecuteReader();
        if (Controlr.Read())
        {
            YN = Convert.ToInt32(Controlr[0].ToString());
            Session["controlslno"] = YN.ToString();
        }

        if (YN == 1)
        {           
            dtgApproved.Columns[7].Visible = true;
            dtgApproved.Columns[6].Visible = true;           
        }
        else if (YN==0)
        {
            
            dtgApproved.Rows[rowa.RowIndex].Cells[7].Enabled = false;
            dtgApproved.Rows[rowa.RowIndex].Cells[6].Enabled = false;
        }

        #region COMMENTED***********
        //int YN;

        //OdbcCommand Control = new OdbcCommand("SELECT control_slno from m_inventory where item_id=" + itemQn + " and rowstatus<>'2'", conn);
        //OdbcDataReader Controlr = Control.ExecuteReader();
        //if (Controlr.Read())
        //{
        //    YN = Convert.ToInt32(Controlr["control_slno"].ToString());
        //    Session["YesNo"] = YN;
        //    if (YN == 1)
        //    {
        //        OdbcCommand Pass = new OdbcCommand("SELECT item_id,itemname FROM m_sub_item WHERE rowstatus<>'2' and is_editable=0 and item_id=" + itemQn + "", conn);
        //        OdbcDataReader Passr = Pass.ExecuteReader();
        //        while (Passr.Read())
        //        {

        //            lblStart.Visible = true;
        //            lblEnd.Visible = true;
        //            txtStart.Visible = true;
        //            txtEnd.Visible = true;
                    

        //        }
        //    }
        //    else
        //    { }
        //}
        #endregion
    }

}
        #endregion
   }
   
    private void CheckQuantity(GridViewRow row)
    {
        #region Check box validation

        RequiredFieldValidator Rfv2 = (RequiredFieldValidator)row.FindControl("RequiredFieldValidator5");
        CheckBox chk = (CheckBox)row.FindControl("CheckBox1");
        if (chk.Checked == true)
        {
            Rfv2.Enabled = true;
            TextBox txt = (TextBox)row.FindControl("TextBox3");
        }
        else
        {
            Rfv2.Enabled = false;
        }
        #endregion
    }
    private void CheckQuantity1(GridViewRow row)
    {
        #region Check box validation
        RequiredFieldValidator Rfv2 = (RequiredFieldValidator)row.FindControl("RequiredFieldValidator2");
        CheckBox chk = (CheckBox)row.FindControl("CheckBox2");
        if (chk.Checked == true)
        {
            Rfv2.Enabled = true;
            TextBox txt = (TextBox)row.FindControl("TextBox5");
        }
        else
        {
            Rfv2.Enabled = false;
        }
        #endregion
    }
    protected void dtgItem_RowCreated(object sender, GridViewRowEventArgs e)
    {
        #region Item gridview
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgItem, "Select$" + e.Row.RowIndex);
        }
#endregion
    }
    protected void dtgRequestedItems_RowCreated(object sender, GridViewRowEventArgs e)
    {
        #region Requested Items Grid View
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgRequestedItems, "Select$" + e.Row.RowIndex);
        }
        #endregion
    }
    protected void dtgApproved_RowCreated(object sender, GridViewRowEventArgs e)
    {
        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Panel8.Visible = true;
        dtgRequestedItems.Visible = true;
        RequestedItems();
        pnlapprove.Visible = false;
    }

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
            if (obj.CheckUserRight("Room Inventory Management", level) == 0)
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
            conn.Close();
        }
    }
    #endregion

    protected void lnkReceipt_Click(object sender, EventArgs e)
    {
    }
    
    protected void RdoRequest_CheckedChanged(object sender, EventArgs e)
    {
        
        Panel8.Visible = false;
        Panel10.Visible = false;
        dtgRequestedItems.Visible = true;
        dtgAItem.Visible = false;
        pnlapprove1.Visible = false;
        dtgRequestedItems.Visible = true;
        RequestedItems();
        dtgItemDetails.Visible = true;
       
        pnlapprove.Visible = true;
        dtgApproved.Visible = false;
        btnIssue.Visible = false;
        lblIssueOfficer.Visible = false;
        txtIssueOfficer.Visible = false;
        lblApprOfficer.Visible = false;
        txtApprovingOfficer.Visible = false;
    }

    protected void RdoApprove_CheckedChanged(object sender, EventArgs e)
    {

        string strIssNo;
        Panel8.Visible = false;
        pnlapprove1.Visible = false;
        Panel10.Visible = true;
        lblIssueNo.Visible = true;
        txtIssueNo.Visible = true;
        dtgRequestedItems.Visible = false;
        dtgItemDetails.Visible = false;
        btnApprove.Visible = false;
        lblIssueOfficer.Visible = true;
        dtgApproved.Visible = true;
        btnIssue.Visible = true;
        txtIssueOfficer.Visible = true;
        year = Session["year"].ToString();
        dtgAItem.Visible = true;
        lblApprOfficer.Visible = true;
        txtApprovingOfficer.Visible = true;

        OdbcCommand Issue = new OdbcCommand("SELECT max(issueno) from t_inventoryrequest_issue", conn);
        if (Convert.IsDBNull(Issue.ExecuteScalar()) == true)
        {
            strIssNo = "ImNo/" + year + "/" + "0001";
            txtIssueNo.Text = strIssNo.ToString();
        }
        else
        {

            string o1 = Issue.ExecuteScalar().ToString();

            string ab1 = o1.Substring(10, 4);
            a4 = Convert.ToInt32(ab1);
            a4 = a4 + 1;
            if (a4 >= 1000)
            {
                strIssNo = "ImNo/" + year + "/" + a4;
                txtIssueNo.Text = strIssNo.ToString();

            }
            else if (a4 >= 100)
            {
                strIssNo = "ImNo/" + year + "/0" + a4;
                txtIssueNo.Text = strIssNo.ToString();
            }
            else if (a4 >= 10)
            {

                strIssNo = "ImNo/" + year + "/00" + a4;
                txtIssueNo.Text = strIssNo.ToString();
            }
            else if (a4 < 10)
            {
                strIssNo = "ImNo/" + year + "/000" + a4;
                txtIssueNo.Text = strIssNo.ToString();
            }
        }

        ApproveItems();
    }
    protected void lnkStoreManager_Click(object sender, EventArgs e)
    {
        lblStaff.Visible = true;
        cmbStaff.Visible = true;
        btnLed.Visible = true;
        lblStore1.Visible = true;
        cmbStore1.Visible = true;
        lblStoreName.Visible = false;
        cmbStockRegistry.Visible = false;
        lblItName.Visible = false;
        cmbStockItem.Visible = false;
        btnStock.Visible = false;
        lblRStore.Visible = false;
        cmbRStore.Visible = false;
        btnRol.Visible = false;
        lnkAuthor.Visible = false;

        
        OdbcDataAdapter Liab = new OdbcDataAdapter("select staffname,ms.manager_id as staff_id from m_inventory inv,m_sub_store ms,m_staff s,m_sub_item im where "
                  + "ms.manager_id=s.staff_id and inv.store_id=ms.store_id and im.item_id=inv.item_id group by staff_id", conn);

        DataTable ds = new DataTable();
        DataRow row = ds.NewRow();
        Liab.Fill(ds);
        row["staff_id"] = "-1";
        row["staffname"] = "--Select--";
        ds.Rows.InsertAt(row, 0);
        cmbStaff.DataSource = ds;
        cmbStaff.DataBind();       

    }
    protected void dtgApproved_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgApproved.PageIndex = e.NewPageIndex;
        dtgApproved.DataBind();
        Approve();
    }

    protected void cmbItem_SelectedIndexChanged1(object sender, EventArgs e)
    {
     
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        //OdbcCommand Store1 = new OdbcCommand();
        //Store1.CommandType = CommandType.StoredProcedure;
        //Store1.Parameters.AddWithValue("tblname", "m_inventory inv,m_sub_item it");
        //Store1.Parameters.AddWithValue("attribute", "distinct itemname,inv.item_id as item_id");
        //Store1.Parameters.AddWithValue("conditionv", "it.itemcat_id=" + cmbItem.SelectedValue.ToString() + "  and inv.rowstatus<>'2' and inv.item_id=it.item_id and it.rowstatus<>'2'");
        //OdbcDataAdapter Store15 = new OdbcDataAdapter(Store1);

       OdbcDataAdapter Store15 = new OdbcDataAdapter("select distinct itemname,inv.item_id as item_id from m_inventory inv,m_sub_item it where it.itemcat_id=" + cmbItem.SelectedValue.ToString() + "  and inv.rowstatus<>'2' and inv.item_id=it.item_id and it.rowstatus<>'2'", conn);
    

        DataTable ds1 = new DataTable();
        DataRow row = ds1.NewRow();
        Store15.Fill(ds1);
        //ds1 = obje.SpDtTbl("CALL selectcond(?,?,?)", Store1);
        row["item_id"] = "-1";
        row["itemname"] = "--Select--";
        ds1.Rows.InsertAt(row, 0);    
        cmbItemName.DataSource = ds1;
        cmbItemName.DataBind();
        conn.Close();
        
    }
    protected void cmbItemName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        conn = obje.NewConnection();
        Session["itemid"] = cmbItemName.SelectedValue.ToString();

        OdbcCommand Item = new OdbcCommand();
        Item.CommandType = CommandType.StoredProcedure;
        Item.Parameters.AddWithValue("tblname", "m_inventory,m_sub_itemcategory,m_sub_unit");
        Item.Parameters.AddWithValue("attribute", "itemcode,unitname as Unit");
        Item.Parameters.AddWithValue("conditionv", "item_id='" + cmbItemName.SelectedValue + "' and m_inventory.rowstatus<>'2' and m_inventory.itemcat_id=m_sub_itemcategory.itemcat_id and m_inventory.unit_id=m_sub_unit.unit_id");
        OdbcDataAdapter Itemr = new OdbcDataAdapter(Item);
        DataTable ds = new DataTable();        
        ds = obje.SpDtTbl("CALL selectcond(?,?,?)", Item);

        #region COMMENTED****************
        //Itemr.Fill(ds);
        //OdbcCommand Item = new OdbcCommand("select itemcode,unitname as Unit from m_inventory,m_sub_itemcategory,m_sub_unit where item_id='" + cmbItemName.SelectedValue + "' and m_inventory.rowstatus<>'2' and m_inventory.itemcat_id=m_sub_itemcategory.itemcat_id and m_inventory.unit_id=m_sub_unit.unit_id", conn);
        //OdbcDataReader Itemr = Item.ExecuteReader();
        //while (Itemr.Read())
        #endregion

        for (int i=0;i<ds.Rows.Count;i++)
        {
            txtCode.Text = ds.Rows[i]["itemcode"].ToString();
            txtUnit.Text = ds.Rows[i]["Unit"].ToString();
        }
    }
    protected void btnStock_Click(object sender, EventArgs e)
    {
        #region stock report
        if (cmbStockRegistry.SelectedValue == "-1")
        {
            lblOk.Text = "Please Select a store"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        if (cmbStockItem.SelectedValue == "-1")
        {
            lblOk.Text = "Please Select Item"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        DateTime tt0;
        DateTime ds2 = DateTime.Now;
        DateTime gh = DateTime.Now;
        string transtim = ds2.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "StockLedger" + transtim.ToString() + ".pdf";
        //string ch = " " + transtim.ToString() + ".pdf";

        string datte = ds2.ToString("dd-MM-yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        string timme = ds2.ToShortTimeString();
        string datte1 = ds2.ToString("dd MMMM yyyy");
        string dat4 = ds2.ToString("dd-MM-yyyy");

        string num, tt2;
         decimal OpenSt;
       
            OdbcCommand Rstatus1 = new OdbcCommand("DROP VIEW if exists tempstockledger", conn);
            Rstatus1.ExecuteNonQuery();
            OdbcCommand StockLed = new OdbcCommand("CREATE VIEW tempstockledger as select req_from,iss.issueno,t.reqno,itemcode,openingstock,iss.issued_qty,"
             +"received_qty,start_slno,end_slno,iss.item_id,office_request,office_issue,inv.createdon as opend,ri.createdon as isdate from m_inventory inv, "
             +"t_inventoryrequest_items item,t_inventoryrequest_items_issue iss,t_inventoryrequest_issue ri,t_inventoryrequest t left join m_sub_store s on "
             + "(s.store_id=office_request or s.store_id=office_issue) where t.reqno=item.reqno and (iss.item_id=" + cmbStockItem.SelectedValue + " or item.item_id=" + cmbStockItem.SelectedValue + ") "
             +"and inv.item_id=item.item_id and ri.issueno=iss.issueno and iss.item_id=item.item_id and iss.item_id=inv.item_id and ri.reqno=t.reqno and "
             + "(office_request=(select store_id as id from m_sub_store where storename='" + cmbStockRegistry.SelectedItem.Text.ToString() + "' and rowstatus<>'2' union select "
             + "counter_id as id from m_sub_counter where counter_no='" + cmbStockRegistry.SelectedItem.Text.ToString() + "' and rowstatus<>'2' union select team_id from m_team where "
             + "teamname='" + cmbStockRegistry.SelectedItem.Text.ToString() + "' and rowstatus<>'2') or office_issue=(select store_id as id from m_sub_store where storename='" + cmbStockRegistry.SelectedItem.Text.ToString() + "' "
             + "and rowstatus<>'2' union select counter_id as id from m_sub_counter where counter_no='" + cmbStockRegistry.SelectedItem.Text.ToString() + "' and rowstatus<>'2' union "
             + "select team_id from m_team where teamname='" + cmbStockRegistry.SelectedItem.Text.ToString() + "' and rowstatus<>'2'))  group by t.reqno order by isdate asc", conn);
            StockLed.ExecuteNonQuery();
           
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 60, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
       
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Stock Ledger";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        OdbcCommand cmd456 = new OdbcCommand();
        cmd456.CommandType = CommandType.StoredProcedure;
        cmd456.Parameters.AddWithValue("tblname", "tempstockledger");
        cmd456.Parameters.AddWithValue("attribute", "*");

        //OdbcCommand cmd456 = new OdbcCommand("select * from tempstockledger", conn);
        OdbcDataAdapter dacnt456 = new OdbcDataAdapter(cmd456);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectdata(?,?)", cmd456);

        if (dt.Rows.Count == 0)
        {
            lblOk.Text = "No Details found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        for (int ii = 0; ii < dt.Rows.Count; ii++)
        {
            code = dt.Rows[ii]["itemcode"].ToString();
            break;
        }

        PdfPTable table1 = new PdfPTable(8);
        float[] colwidth1 ={ 2, 3, 5, 3, 3, 3, 3,1 };
        table1.SetWidths(colwidth1);
        table1.TotalWidth = 650f;
        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Stock Ledger", font10)));
        cell.Colspan = 8;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table1.AddCell(cell);
        //doc.Add(table1);
        try
        {
            PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Store name: " + cmbStockRegistry.SelectedItem.Text.ToString(), font11)));
            cella.Colspan = 4;
            cella.Border = 0;
            cella.HorizontalAlignment = 0;
            table1.AddCell(cella);
            PdfPCell cellb = new PdfPCell(new Phrase(new Chunk("Item Name: " + cmbStockItem.SelectedItem.Text.ToString(), font11)));
            cellb.Colspan = 4;
            cellb.Border = 0;
            cellb.HorizontalAlignment = 0;
            table1.AddCell(cellb);

            PdfPCell cellc = new PdfPCell(new Phrase(new Chunk("Item Code: " + code.ToString(), font11)));
            cellc.Colspan = 4;
            cellc.Border = 0;
            cellc.HorizontalAlignment = 0;
            table1.AddCell(cellc);
            PdfPCell celld = new PdfPCell(new Phrase(new Chunk("Balance Stock:  ", font11)));
            celld.Colspan = 4;
            celld.Border = 0;
            celld.HorizontalAlignment = 0;
            table1.AddCell(celld);
        }
        catch
        { }
        doc.Add(table1);
        PdfPTable table = new PdfPTable(8);
        float[] colwidth2 ={ 2, 3, 5, 3, 3, 3, 3, 1 };
        table.SetWidths(colwidth2);
        table.TotalWidth = 650f;
        

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        cell1.Rowspan = 2;
        table.AddCell(cell1);

        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        cell2.Rowspan = 2;
        table.AddCell(cell2);

        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Description", font9)));
        cell3.Rowspan = 2;
        table.AddCell(cell3);

        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Stock item status", font9)));
        cell4.Colspan = 3;
        cell4.HorizontalAlignment = 1;
        table.AddCell(cell4);

        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
        cell5.Rowspan = 2;
        cell5.Colspan = 2;
        table.AddCell(cell5);

        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Recv", font9)));
        table.AddCell(cell6);
        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Isue", font9)));
        table.AddCell(cell7);
        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Bal", font9)));
        table.AddCell(cell8);

        int slno = 0;
       
      
        foreach (DataRow dr in dt.Rows)
        {

     
            int ItemId = int.Parse(dr["item_id"].ToString());
        

            if (slno == 0)
            {
                slno = slno + 1;
                num = slno.ToString();
                PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table.AddCell(cell21b);
                DateTime Date = DateTime.Parse(dr["opend"].ToString());
                string Ddate = Date.ToString("dd MMM yyyy");
                PdfPCell cell21c = new PdfPCell(new Phrase(new Chunk(Ddate.ToString(), font8)));
                table.AddCell(cell21c);
                PdfPCell cell21e = new PdfPCell(new Phrase(new Chunk("Opening Stock", font8)));
                table.AddCell(cell21e);
                decimal OpenStock1 = Convert.ToDecimal(dr["openingstock"].ToString());
   
                amount1 = OpenStock1;
                Session["open"] = OpenStock1;
                Session["RecAm"] = OpenStock1;
                PdfPCell cell21d = new PdfPCell(new Phrase(new Chunk(OpenStock1.ToString(), font8)));
                table.AddCell(cell21d);
                PdfPCell cell21f = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell21f);
                PdfPCell cell21g = new PdfPCell(new Phrase(new Chunk(OpenStock1.ToString(), font8)));
                table.AddCell(cell21g);
                PdfPCell cell21o = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                cell21o.Colspan = 2;
                table.AddCell(cell21o);

            }
        }
        int item1, item2 = 0;
        foreach (DataRow dr in dt.Rows)
        {

            item1 = Convert.ToInt32(dr["item_id"].ToString());
            decimal RecQty;

            decimal amount;
            try
            {
                RecQty = Convert.ToDecimal(dr["received_qty"].ToString());

                if (RecQty != 0)
                {
                    slno = slno + 1;
                    num = slno.ToString();
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.ConnectionString = strConnection;
                        conn.Open();
                    }
                   
                    string StReq = dr["issueno"].ToString();
                    int ItNa = Convert.ToInt32(dr["item_id"].ToString());

                    OdbcCommand Recp = new OdbcCommand();
                    Recp.CommandType = CommandType.StoredProcedure;
                    Recp.Parameters.AddWithValue("tblname", "t_grn g,t_grn_items gi,t_inventoryrequest_items_issue iss");
                    Recp.Parameters.AddWithValue("attribute", "distinct receive_qty,g.receivedon as rdate,g.grnno,start_slno,end_slno");
                    Recp.Parameters.AddWithValue("conditionv", "gi.grnno=g.grnno and g.refno='" + StReq.ToString() + "' and gi.item_id=" + ItNa + " and iss.issueno=g.refno");
                    OdbcDataAdapter dacnt152 = new OdbcDataAdapter(Recp);
                    DataTable dt1 = new DataTable();
                    dt1 = obje.SpDtTbl("CALL selectcond(?,?,?)", Recp);

                    #region COMMENTED******************
                    //OdbcCommand Recp = new OdbcCommand("SELECT distinct receive_qty,g.receivedon as rdate,g.grnno,start_slno,end_slno from t_grn g,t_grn_items gi,t_inventoryrequest_items_issue iss where "
                    //               + "gi.grnno=g.grnno and g.refno='" + StReq.ToString() + "' and gi.item_id=" + ItNa + " and iss.issueno=g.refno", conn);

                    //OdbcDataReader Recr = Recp.ExecuteReader();
                    //if (Recr.Read())
                    #endregion

                    if (dt1.Rows.Count > 0)
                    {
                        for (int k = 0; k < dt1.Rows.Count; k++)
                        {
                            DateTime Date1 = DateTime.Parse(dt1.Rows[k]["rdate"].ToString());
                            Ddate1 = Date1.ToString("dd MMM yyyy");
                            Rqt2 = Convert.ToDecimal(dt1.Rows[k]["receive_qty"].ToString());
                            GName = dt1.Rows[k]["grnno"].ToString();
                            sl = Convert.ToInt32(dt1.Rows[k]["start_slno"].ToString());
                            endl = Convert.ToInt32(dt1.Rows[k]["end_slno"].ToString());

                        }
                    }
                    PdfPCell cell33a = new PdfPCell(new Phrase(new Chunk(num, font8)));
                    table.AddCell(cell33a);

                    PdfPCell cell33b = new PdfPCell(new Phrase(new Chunk(Ddate1.ToString(), font8)));
                    table.AddCell(cell33b);
                    try
                    {
                        int iss1 = Convert.ToInt32(dr["office_issue"].ToString());

                    }
                    catch
                    {
                        int iss1 = 0;
                    }
                    PdfPCell cell33c = new PdfPCell(new Phrase(new Chunk("Received from  " + GName.ToString(), font8)));
                    table.AddCell(cell33c);


                    PdfPCell cell33d = new PdfPCell(new Phrase(new Chunk(Rqt2.ToString(), font8)));
                    table.AddCell(cell33d);
                    PdfPCell cell33e = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell33e);
                    amount = Rqt2;
                    OpenSt = decimal.Parse(Session["RecAm"].ToString());
                    amount1 = amount + OpenSt;
                    Session["RecAm"] = amount1;
                    PdfPCell cell33f = new PdfPCell(new Phrase(new Chunk(amount1.ToString(), font8)));
                    table.AddCell(cell33f);

                    try
                    {
                        if (sl == 0 && endl == 0)
                        {
                            PdfPCell cell33n = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                            cell33n.Colspan = 2;
                            table.AddCell(cell33n);
                        }

                        else
                        {
                            try
                            {

                                PdfPCell cell33n = new PdfPCell(new Phrase(new Chunk("Sl no " + sl.ToString() + " - " + endl.ToString(), font8)));
                                cell33n.Colspan = 2;
                                table.AddCell(cell33n);

                            }
                            catch
                            {
                                PdfPCell cell33n = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                                cell33n.Colspan = 2;
                                table.AddCell(cell33n);
                            }

                        }

                    }
                    catch
                    {

                        PdfPCell cell33n = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                        cell33n.Colspan = 2;
                        table.AddCell(cell33n);
                    }

                    
                }
                    item2 = Convert.ToInt32(dr["item_id"].ToString());
                
            }
            
            catch
            {

            }
            decimal IssQty;
            int it1;
            try
            {
                IssQty = Convert.ToDecimal(dr["issued_qty"].ToString());
                if (IssQty != 0)
                {
                    slno = slno + 1;
                    num = slno.ToString();
                    PdfPCell cell33h = new PdfPCell(new Phrase(new Chunk(num.ToString(), font8)));
                    table.AddCell(cell33h);
                    DateTime Date2 = DateTime.Parse(dr["isdate"].ToString());
                    string Ddate2 = Date2.ToString("dd MMM yyyy");
                    PdfPCell cell33i = new PdfPCell(new Phrase(new Chunk(Ddate2.ToString(), font8)));
                    table.AddCell(cell33i);
                    string Name = dr["reqno"].ToString();
                    PdfPCell cell33j = new PdfPCell(new Phrase(new Chunk("Issued to " + Name.ToString(), font8)));
                    table.AddCell(cell33j);
                    PdfPCell cell33k = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell33k);
                    decimal Issqt = Convert.ToDecimal(dr["issued_qty"].ToString());
                    PdfPCell cell33l = new PdfPCell(new Phrase(new Chunk(Issqt.ToString(), font8)));
                    table.AddCell(cell33l);
                    amount1 = decimal.Parse(Session["RecAm"].ToString());
                    decimal Bal = amount1 - Issqt;
                    Session["RecAm"] = Bal;
                    PdfPCell cell33m = new PdfPCell(new Phrase(new Chunk(Bal.ToString(), font8)));
                    table.AddCell(cell33m);

                    try
                    {
                        int st = Convert.ToInt32(dr["start_slno"].ToString());
                        int en = Convert.ToInt32(dr["end_slno"].ToString());
                        if (st == 0 && en == 0)
                        {
                            PdfPCell cell33n = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                            cell33n.Colspan = 2;
                            table.AddCell(cell33n);
                        }

                        else
                        {
                            try
                            {

                                PdfPCell cell33n = new PdfPCell(new Phrase(new Chunk("Sl no " + st.ToString() + " - " + en.ToString(), font8)));
                                cell33n.Colspan = 2;
                                table.AddCell(cell33n);

                            }
                            catch
                            {
                                PdfPCell cell33n = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                                cell33n.Colspan = 2;
                                table.AddCell(cell33n);
                            }

                        }

                    }
                    catch
                    {

                        PdfPCell cell33n = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                        cell33n.Colspan = 2;
                        table.AddCell(cell33n);
                    }
                }

                it2 = Convert.ToInt32(dr["item_id"].ToString());
            }
            catch (Exception ex)
            {

            }
        }

        #region COMMENTED******************
        /////////////////
        //try
        //{
        //    OdbcCommand Return = new OdbcCommand("SELECT sum(return_qty) as qty,counter_no,returnedon from t_material_return_items i,t_material_retrun r,"
        //        +"m_sub_counter c where item_id="+cmbStockItem.SelectedValue+" and r.retno=i.retno and c.counter_id=r.returnedto group by item_id,returnedto",conn);
        //    OdbcDataReader Ret = Return.ExecuteReader();
        //    if (Ret.Read())
        //    {
        //        slno = slno + 1;
        //        PdfPCell cell33k = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
        //        table.AddCell(cell33k);
        //        DateTime Date3 = DateTime.Parse(Ret["returnedon"].ToString());
        //        string Ddate3 = Date3.ToString("dd MMM yyyy");
        //        PdfPCell cell33ia = new PdfPCell(new Phrase(new Chunk(Ddate3.ToString(), font8)));
        //        table.AddCell(cell33ia);
        //        PdfPCell cell33iab = new PdfPCell(new Phrase(new Chunk("Returned to  "+Ret["counter_no"].ToString(), font8)));
        //        table.AddCell(cell33iab);
        //        PdfPCell cell33iac = new PdfPCell(new Phrase(new Chunk("", font8)));
        //        table.AddCell(cell33iac);
        //        decimal cc = Convert.ToDecimal(Ret["qty"].ToString());
        //        PdfPCell cell33iad = new PdfPCell(new Phrase(new Chunk(cc.ToString(), font8)));
        //        table.AddCell(cell33iad);
        //        decimal dd =decimal.Parse(Session["RecAm"].ToString());
        //        decimal am = dd - cc;
        //        PdfPCell cell33iae = new PdfPCell(new Phrase(new Chunk(am.ToString(), font8)));
        //        table.AddCell(cell33iae);
        //        PdfPCell cell33iaf = new PdfPCell(new Phrase(new Chunk("", font8)));
        //        cell33iaf.Colspan = 2;
        //        table.AddCell(cell33iaf);
            
        //    }
        //}
        //catch
        //{ 
        
        //}
        //////////////////////
        #endregion

            PdfPTable table5 = new PdfPTable(3);
            PdfPCell cellab = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellab.Border = 1;
            table5.AddCell(cellab);
            PdfPCell cellac = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellac.Border = 1;
            table5.AddCell(cellac);
            PdfPCell cellav = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellav.Border = 1;

            table5.AddCell(cellav);

            PdfPCell cellaq = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
            //cellaq.Border = 1;
            cellaq.Border = 0;
            table5.AddCell(cellaq);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("", font8)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);
            PdfPCell cellae = new PdfPCell(new Phrase(new Chunk("", font8)));
            cellae.Border = 0;
            table5.AddCell(cellae);
            PdfPCell cellaj = new PdfPCell(new Phrase(new Chunk("Stores superintendent", font9)));
            cellaj.Border = 0;
            table5.AddCell(cellaj);

            PdfPCell cellawi = new PdfPCell(new Phrase(new Chunk("", font8)));
            cellawi.Border = 0;
            table5.AddCell(cellawi);
            PdfPCell cellaei = new PdfPCell(new Phrase(new Chunk("", font8)));
            cellaei.Border = 0;
            table5.AddCell(cellaei);

            PdfPCell cellaj1 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom", font9)));
            cellaj1.Border = 0;
            table5.AddCell(cellaj1);

            PdfPCell cellawi2 = new PdfPCell(new Phrase(new Chunk("", font8)));
            cellawi2.Border = 0;
            table5.AddCell(cellawi2);
            PdfPCell cellaei3 = new PdfPCell(new Phrase(new Chunk("", font8)));
            cellaei3.Border = 0;
            table5.AddCell(cellaei3);

            doc.Add(table);
            doc.Add(table5);
            doc.Close();

            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Stock Ledger report";
            string Script = "";
             Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            conn.Close();
        #endregion
        
    }
     #region COMMENTED**********
    
    //}







       // for (int jj = 0; jj < dtt456.Rows.Count; jj++)
       // {
       //     #region COMMENTED*****************
       //     //         

       //     //         OdbcCommand ItemDet = new OdbcCommand("select openingstock,stock_qty,itemcode,itemname,DATE_FORMAT(i.createdon,'%d-%m-%Y') as Date from m_inventory i,m_sub_item ms where ms.item_id=i.item_id and i.rowstatus<>'2' and i.item_id=" + ItemId + " and openingstock> '0'", con);
       //     //         OdbcDataReader Itemr = ItemDet.ExecuteReader();
       //     //         if (Itemr.Read())
       //     //         {

       //     //             string icode = Itemr["itemcode"].ToString();

       //     //             slno1 = slno1 + 1;
       //     //             PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Store Name", font9)));
       //     //             cella.Border = 1;
       //     //             table5.AddCell(cella);
       //     //             string Store = cmbStockRegistry.SelectedText.ToString();
       //     //             PdfPCell cellb = new PdfPCell(new Phrase(new Chunk(Store.ToString(), font8)));
       //     //             cellb.Border = 1;
       //     //             table5.AddCell(cellb);
       //     //             PdfPCell cellc = new PdfPCell(new Phrase(new Chunk("Item Name", font9)));
       //     //             cellc.Border = 1;
       //     //             table5.AddCell(cellc);
       //     //             string Item = Itemr["itemname"].ToString();
       //     //             PdfPCell celld = new PdfPCell(new Phrase(new Chunk(Item.ToString(), font8)));
       //     //             celld.Border = 1;
       //     //             table5.AddCell(celld);
       //     //             PdfPCell celle = new PdfPCell(new Phrase(new Chunk("Item Code", font8)));
       //     //             celle.Border = 1;
       //     //             table5.AddCell(celle);
       //     //             string Code = Itemr["itemcode"].ToString();
       //     //             PdfPCell cellf = new PdfPCell(new Phrase(new Chunk(Code.ToString(), font8)));
       //     //             cellf.Border = 1;
       //     //             table5.AddCell(cellf);
       //     //             PdfPCell cellg = new PdfPCell(new Phrase(new Chunk("Opening Date", font8)));
       //     //             cellg.Border = 1;
       //     //             table5.AddCell(cellg);
       //     //             string date = Itemr["Date"].ToString();
       //     //             PdfPCell cellh = new PdfPCell(new Phrase(new Chunk(date.ToString(), font8)));
       //     //             cellh.Border = 1;
       //     //             table5.AddCell(cellh);
       //     //             doc.Add(table5);


       //     //             PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Sl No", font9)));
       //     //             table1.AddCell(cell1);

       //     //             PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
       //     //             table1.AddCell(cell2);

       //     //             PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Description", font9)));
       //     //             table1.AddCell(cell3);

       //     //             PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Received Qty", font9)));
       //     //             table1.AddCell(cell4);

       //     //             PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Issued Qty", font9)));
       //     //             table1.AddCell(cell5);

       //     //             PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Balance", font9)));
       //     //             table1.AddCell(cell6);
       //     //             PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
       //     //             table1.AddCell(cell7);


       //     //             PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(slno1.ToString(), font8)));
       //     //             table1.AddCell(cell19);
       //     //             PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(date.ToString(), font8)));
       //     //             table1.AddCell(cell20);
       //     //             PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Opening Stock", font8)));
       //     //             table1.AddCell(cell21);

       //     //             PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("0", font8)));
       //     //             table1.AddCell(cell22);
       //     //             PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk("0", font8)));
       //     //             table1.AddCell(cell23);
       //     //             decimal open1 = Convert.ToDecimal(Itemr["openingstock"].ToString());
       //     //             PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(open1.ToString(), font8)));
       //     //             table1.AddCell(cell25);
       //     //             PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //     //             table1.AddCell(cell24);
       //     //             doc.Add(table1);

       //     //             string Reqno = dtt456.Rows[jj]["reqno"].ToString();
       //     //             OdbcDataAdapter Req = new OdbcDataAdapter("select t.issueno,DATE_FORMAT(iss.createdon,'%d-%m-%Y') as Date,t.issued_qty,(stock_qty-issued_qty) as Balance from m_inventory i,t_inventoryrequest_items_issue t,t_inventoryrequest_issue iss where i.item_id=t.item_id and reqno='" + Reqno.ToString() + "' and t.item_id=1 and t.issueno=iss.issueno", con);
       //     //             DataTable db = new DataTable();
       //     //             Req.Fill(db);

       //     //             foreach (DataRow dr in db.Rows)
       //     //             {
       //     //                 //PdfPTable table7 = new PdfPTable(8);
       //     //                 slno1 = slno1 + 1;
       //     //                 PdfPCell cell25q = new PdfPCell(new Phrase(new Chunk(slno1.ToString(), font8)));
       //     //                 table1.AddCell(cell25q);

       //     //                 PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(dr["date"].ToString(), font8)));
       //     //                 table1.AddCell(cell26);

       //     //                 PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(dr["issueno"].ToString(), font8)));
       //     //                 table1.AddCell(cell27);
       //     //                 PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk("", font8)));
       //     //                 table1.AddCell(cell28);
       //     //                 decimal iss4 = Convert.ToDecimal(dr["issued_qty"].ToString());
       //     //                 PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(iss4.ToString(), font8)));
       //     //                 table1.AddCell(cell29);
       //     //                 decimal bal4 = Convert.ToDecimal(dr["Balance"].ToString());
       //     //                 PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(bal4.ToString(), font8)));
       //     //                 table1.AddCell(cell30);
       //     //                 PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //     //                 table1.AddCell(cell31);
       //     //                 doc.Add(table1);

       //     //             }
       //     //         }
       //     //         //slno = slno + 1;
       //     //         //PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(slno, font8)));
       //     //         //table1.AddCell(cell25);

       //     //         //PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(dr["date"].ToString(), font8)));
       //     //         //table1.AddCell(cell26);

       //     //         //PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(dr["issueno"].ToString(), font8)));
       //     //         //table1.AddCell(cell27);
       //     //         //PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk("", font8)));
       //     //         //table1.AddCell(cell28);
       //     //         //decimal iss = Convert.ToDecimal(dr["issued_qty"].ToString());
       //     //         //PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(iss, font8)));
       //     //         //table1.AddCell(cell29);
       //     //         //decimal bal = Convert.ToDecimal(dr["Balance"].ToString());
       //     //         //PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(bal, font8)));
       //     //         //table1.AddCell(cell30);
       //     //         //PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //     //         //table1.AddCell(cell31);



       //     //        // doc.Add(table1);

       //     //     }

       //     //// }

       //     #region COMMENTED***********
       //     //for (int ii = 0; ii < dtt456.Rows.Count; ii++)
       //     //{
       //     //    PdfPTable tablea = new PdfPTable(7);
       //     //    if (i+j > 39)
       //     //    {
       //     //        doc.NewPage();
       //     //        PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("No", font9)));
       //     //        tablea.AddCell(cell1a);

       //     //        PdfPCell cell2a = new PdfPCell(new Phrase(new Chunk("Opening Stock", font9)));
       //     //        tablea.AddCell(cell2a);

       //     //        PdfPCell cell3a = new PdfPCell(new Phrase(new Chunk("Issued Store/ Issued From", font9)));
       //     //        tablea.AddCell(cell3a);

       //     //        PdfPCell cell4a = new PdfPCell(new Phrase(new Chunk("Received Store/ Issued To", font9)));
       //     //        tablea.AddCell(cell4a);

       //     //        PdfPCell cell5a = new PdfPCell(new Phrase(new Chunk("Issued Quantity", font9)));
       //     //        tablea.AddCell(cell5a);

       //     //        PdfPCell cell6a = new PdfPCell(new Phrase(new Chunk("Requested Quantity", font9)));
       //     //        tablea.AddCell(cell6a);
       //     //        PdfPCell cell7a = new PdfPCell(new Phrase(new Chunk("Balance", font9)));
       //     //        tablea.AddCell(cell7a);
       //     //        i = 0; j = 0;
       //     //        doc.Add(tablea);

       //     //    }




       //     //    //catg = dtt456.Rows[ii]["itemcatgoryreq"].ToString();
       //     //    //item = dtt456.Rows[ii]["itemnamereq"].ToString();
       //     //    bal = int.Parse(dtt456.Rows[ii]["Balance"].ToString());
       //     //    iss = int.Parse(dtt456.Rows[ii]["issued_qty"].ToString());
       //     //    //stor = dtt456.Rows[ii]["storename"].ToString();
       //     //   // appuse = dtt456.Rows[ii]["username"].ToString();
       //     //    //adate1 = DateTime.Parse(dtt456.Rows[ii]["date"].ToString());
       //     //   // adate = adate1.ToString("dd-MM-yyyy");
       //     //    //rno = int.Parse(dtt456.Rows[ii]["reqno"].ToString());
       //     //    //itcode = dtt456.Rows[ii]["itemcode"].ToString();
       //     //    //max = int.Parse(dtt456.Rows[ii]["maxstock"].ToString());
       //     //    //max = max - iss;
       //     //    ReqNumber = dtt456.Rows[ii]["reqno"].ToString();
       //     //    Itid = int.Parse(dtt456.Rows[ii]["item_id"].ToString());
       //     //    Rqt = int.Parse(dtt456.Rows[ii]["req_qty"].ToString());
       //     //    Roff = int.Parse(dtt456.Rows[ii]["office_request"].ToString());
       //     //    Ioff = int.Parse(dtt456.Rows[ii]["office_issue"].ToString());

       //     //    OdbcCommand cmd459 = new OdbcCommand("CALL selectcond(?,?,?)", con);
       //     //    cmd459.CommandType = CommandType.StoredProcedure;
       //     //    cmd459.Parameters.AddWithValue("tblname", "m_inventory");
       //     //    cmd459.Parameters.AddWithValue("attribute", "openingstock");
       //     //    cmd459.Parameters.AddWithValue("conditionv", "item_id="+Itid+" and store_id="+Roff+" and rowstatus<>'2'");
       //     //    OdbcDataAdapter dacnt459 = new OdbcDataAdapter(cmd459);
       //     //    DataTable dtt459 = new DataTable();
       //     //    dacnt459.Fill(dtt459);
       //     //    if (dtt459.Rows.Count > 0)
       //     //    {
       //     //        min = int.Parse(dtt459.Rows[0]["openingstock"].ToString());
       //     //        open = min - iss;
       //     //        Session["min"] = open;
       //     //        //obdate1 = DateTime.Parse(dtt459.Rows[0]["updateddate"].ToString());
       //     //        //obdate = obdate1.ToString("dd-MM-yyyy");
       //     //        //Session["obdat"] = obdate.ToString();

       //     //    }
       //     //    OdbcCommand ReqOffice=new OdbcCommand("SELECT storename from m_sub_store where store_id="+Roff+" and rowstatus<>'2'",con);
       //     //    OdbcDataReader Reqoff=ReqOffice.ExecuteReader();
       //     //    if(Reqoff.Read())
       //     //    {
       //     //     RofName=Reqoff[0].ToString();
       //     //    }

       //     //    OdbcCommand IssOffice=new OdbcCommand("SELECT storename from m_sub_store where store_id="+Ioff+" and rowstatus<>'2'",con);
       //     //    OdbcDataReader Issoff=IssOffice.ExecuteReader();
       //     //    if(Issoff.Read())
       //     //    {
       //     //     IofName =Reqoff[0].ToString();
       //     //    }


       //     //    //OdbcCommand cmd457 = new OdbcCommand("CALL selectcond(?,?,?)", con);
       //     //    //cmd457.CommandType = CommandType.StoredProcedure;
       //     //    //cmd457.Parameters.AddWithValue("tblname", "inventory_trequest");
       //     //    //cmd457.Parameters.AddWithValue("attribute", "*");
       //     //    //cmd457.Parameters.AddWithValue("conditionv", "reqno=" + rno + "");
       //     //    //OdbcDataAdapter dacnt457 = new OdbcDataAdapter(cmd457);
       //     //    //DataTable dtt457 = new DataTable();
       //     //    //dacnt457.Fill(dtt457);
       //     //    //if (dtt457.Rows.Count > 0)
       //     //    //{
       //     //    //    requse = dtt457.Rows[0]["username"].ToString();
       //     //    //    Session["use"] = requse.ToString();
       //     //    //    rdate1 = DateTime.Parse(dtt457.Rows[0]["reqdate"].ToString());
       //     //    //    rdate = rdate1.ToString("dd-MM-yyyy");
       //     //    //    Session["d"] = rdate.ToString();
       //     //    //}


       //     //    slno = slno + 1;
       //     //    //reqtype = dr["reqtype"].ToString();
       //     //    if (slno == 1)
       //     //    {
       //     //        PdfPTable tablep = new PdfPTable(7);
       //     //        PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("Store Name:       " +RofName.ToString() , font8)));
       //     //        cell1a.Colspan = 3;
       //     //        cell1a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
       //     //        tablep.AddCell(cell1a);
       //     //        //PdfPCell cell1b = new PdfPCell(new Phrase(new Chunk("", font8)));
       //     //        //table.AddCell(cell1b);
       //     //        PdfPCell cell1c = new PdfPCell(new Phrase(new Chunk("Request Number:       " +ReqNumber , font8)));
       //     //        cell1c.Colspan = 4;
       //     //        cell1c.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
       //     //        tablep.AddCell(cell1c);
       //     //        j++;
       //     //        doc.Add(tablep);

       //     //    }
       //     //    else
       //     //    {

       //     //        if (ReqNumber == dtt456.Rows[ii]["reqno"].ToString())
       //     //        {
       //     //        }
       //     //        else
       //     //        {
       //     //            PdfPTable tablep = new PdfPTable(7);
       //     //            ReqNumber = dtt2.Rows[ii]["reqno"].ToString();
       //     //            PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("Store Name:       " + RofName.ToString(), font8)));
       //     //            cell1a.Colspan = 3;
       //     //            cell1a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
       //     //            tablep.AddCell(cell1a);
       //     //            //PdfPCell cell1b = new PdfPCell(new Phrase(new Chunk("", font8)));
       //     //            //table.AddCell(cell1b);
       //     //            PdfPCell cell1c = new PdfPCell(new Phrase(new Chunk("Request Number:       " + ReqNumber, font8)));
       //     //            cell1c.Colspan = 4;
       //     //            cell1c.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
       //     //            tablep.AddCell(cell1c);
       //     //            j++;
       //     //            slno = 1;
       //     //            doc.Add(tablep);

       //     //        }

       //     //    }


       //     //    PdfPTable table2 = new PdfPTable(7);
       //     //    //float[] colWidths2 = { 30, 60, 60, 60, 60, 60 };
       //     //    //table2.SetWidths(colWidths2);
       //     //    if (ii == 0)
       //     //    {
       //     //        no = no + 1;
       //     //        num = no.ToString();

       //     //        PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk(num, font8)));
       //     //        table2.AddCell(cell1p);

       //     //        PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk(Session["min"].ToString(), font8)));
       //     //        table2.AddCell(cell2p);

       //     //        PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk(IofName.ToString(), font8)));
       //     //        table2.AddCell(cell5p);


       //     //        PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk(RofName.ToString(), font8)));
       //     //        table2.AddCell(cell3p);

       //     //        PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk(iss.ToString(), font8)));
       //     //        table2.AddCell(cell4p);


       //     //        PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk(Rqt.ToString(), font8)));
       //     //        table2.AddCell(cell6p);

       //     //        PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk(bal.ToString(), font8)));
       //     //        table2.AddCell(cell7p);
       //     //        i++;
       //     //        doc.Add(table2);

       //     //    }

       //     //    no = no + 1;
       //     //    num = no.ToString();

       //     //    PdfPTable table3 = new PdfPTable(7);
       //     //    //float[] colWidths3 = { 30, 60, 60, 60, 60, 60 };
       //     //    //table3.SetWidths(colWidths3);

       //     //    PdfPCell cell1p3 = new PdfPCell(new Phrase(new Chunk(num, font8)));
       //     //    table3.AddCell(cell1p3);

       //     //    PdfPCell cell2p3 = new PdfPCell(new Phrase(new Chunk(Session["min"].ToString(), font8)));
       //     //    table3.AddCell(cell2p3);

       //     //    PdfPCell cell5p3 = new PdfPCell(new Phrase(new Chunk(Ioff.ToString(), font8)));
       //     //    table3.AddCell(cell5p3);


       //     //    PdfPCell cell3p3 = new PdfPCell(new Phrase(new Chunk(Roff.ToString(), font8)));
       //     //    table3.AddCell(cell3p3);

       //     //    PdfPCell cell4p4 = new PdfPCell(new Phrase(new Chunk(iss.ToString(), font8)));
       //     //    table3.AddCell(cell4p4);


       //     //    PdfPCell cell65 = new PdfPCell(new Phrase(new Chunk(Rqt.ToString(), font8)));
       //     //    table3.AddCell(cell65);
       //     //    PdfPCell cell65a = new PdfPCell(new Phrase(new Chunk(bal.ToString(), font8)));
       //     //    table3.AddCell(cell65a);
       //     //    i++;
       //     //    doc.Add(table3);

       //     //}
       //     #endregion
       //     #endregion

       //     int ItemId = Convert.ToInt32(dtt456.Rows[jj]["item_id"].ToString());

       //     OdbcCommand Itemselect = new OdbcCommand("select * from tempstockledger where item_id=" + int.Parse(ItemId.ToString()) + "", conn);
       //     OdbcDataAdapter da0 = new OdbcDataAdapter(Itemselect);
       //     DataTable dt = new DataTable();
       //     da0.Fill(dt);
       //     foreach (DataRow dr in dt.Rows)
       //     {
       //         slno = slno + 1;
       //         ItemId = int.Parse(dr["item_id"].ToString());
       //         string Rnumber = dr["reqno"].ToString();

       //         if (slno == 1)
       //         {

       //             //PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("ALLOCATION TYPE:       " + reqtype, font8)));
       //             //cell1a.Colspan = 9;
       //             //cell1a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
       //             //table.AddCell(cell1a);
       //             PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Store Name", font9)));
       //             cella.Border = 1;
       //             table5.AddCell(cella);
       //             string Store = cmbStockRegistry.SelectedItem.ToString();
       //             PdfPCell cellb = new PdfPCell(new Phrase(new Chunk(Store.ToString(), font8)));
       //             cellb.Border = 1;
       //             table5.AddCell(cellb);
       //             PdfPCell cellc = new PdfPCell(new Phrase(new Chunk("Item Name", font9)));
       //             cellc.Border = 1;
       //             table5.AddCell(cellc);
       //             string Item = dr["itemname"].ToString();
       //             PdfPCell celld = new PdfPCell(new Phrase(new Chunk(Item.ToString(), font8)));
       //             celld.Border = 1;
       //             table5.AddCell(celld);
       //             PdfPCell celle = new PdfPCell(new Phrase(new Chunk("Item Code", font9)));
       //             celle.Border = 1;
       //             table5.AddCell(celle);
       //             string Code = dr["itemcode"].ToString();
       //             PdfPCell cellf = new PdfPCell(new Phrase(new Chunk(Code.ToString(), font8)));
       //             cellf.Border = 1;
       //             table5.AddCell(cellf);
       //             PdfPCell cellg = new PdfPCell(new Phrase(new Chunk("Opening Date", font9)));
       //             cellg.Border = 1;
       //             table5.AddCell(cellg);
       //             string date = dr["Date"].ToString();
       //             PdfPCell cellh = new PdfPCell(new Phrase(new Chunk(date.ToString(), font8)));
       //             cellh.Border = 1;
       //             table5.AddCell(cellh);
       //             PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
       //             table.AddCell(cell19);
       //             PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(dr["Date"].ToString(), font8)));
       //             table.AddCell(cell20);
       //             PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Opening Stock", font8)));
       //             table.AddCell(cell21);

       //             PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("0", font8)));
       //             table.AddCell(cell22);
       //             PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk("0", font8)));
       //             table.AddCell(cell23);
       //             decimal open1 = Convert.ToDecimal(dr["openingstock"].ToString());
       //             PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(open1.ToString(), font8)));
       //             table.AddCell(cell25);
       //             PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //             table.AddCell(cell24);

       //             string Rreq1 = "";
       //             string Aab = "SELECT distinct reqno,req_qty,Date,openingstock from tempstockledger where item_id=" + ItemId + "";
       //             OdbcCommand Req = new OdbcCommand("SELECT distinct reqno,req_qty,Date,openingstock from tempstockledger where item_id=" + ItemId + "", conn);
       //             OdbcDataReader Reqr = Req.ExecuteReader();
       //             while (Reqr.Read())
       //             {

       //                 string Rreq = Reqr["reqno"].ToString();
       //                 if ((Rreq != "") && (Rreq != Rreq1))
       //                 {
       //                     slno = slno + 1;
       //                     PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
       //                     table.AddCell(cell19a);
       //                     PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk(dr["Date"].ToString(), font8)));
       //                     table.AddCell(cell20a);
       //                     PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk(dr["reqno"].ToString(), font8)));
       //                     table.AddCell(cell21a);
       //                     int Rqty = Convert.ToInt32(dr["req_qty"].ToString());
       //                     PdfPCell cell22a = new PdfPCell(new Phrase(new Chunk(Rqty.ToString(), font8)));
       //                     table.AddCell(cell22a);
       //                     PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //                     table.AddCell(cell23a);
       //                     decimal open1a = Convert.ToDecimal(dr["openingstock"].ToString());
       //                     PdfPCell cell25a = new PdfPCell(new Phrase(new Chunk(open1a.ToString(), font8)));
       //                     table.AddCell(cell25a);
       //                     PdfPCell cell24a = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //                     table.AddCell(cell24a);

       //                     Rreq1 = Reqr["reqno"].ToString();
       //                 }

       //             }
       //             string IsRe1 = "";
       //             OdbcCommand Issamount = new OdbcCommand("select distinct iss.issueno,DATE_FORMAT(iss.createdon,'%d-%m-%Y') as Date,iss.issued_qty,rs.req_qty,(inv.stock_qty-iss.issued_qty) as Balance,inv.stock_qty from t_inventoryrequest_items_issue iss,t_inventoryrequest_issue iu,t_inventoryrequest_items rs,m_inventory inv where iss.issueno=iu.issueno and iu.reqno=rs.reqno and rs.reqno='" + Rnumber.ToString() + "' and iss.item_id=" + ItemId + " and rs.item_id=inv.item_id", conn);
       //             OdbcDataReader Issamr = Issamount.ExecuteReader();
       //             while (Issamr.Read())
       //             {
       //                 string IssNumber = Issamr["issueno"].ToString();
       //                 decimal Stamount = decimal.Parse(Issamr["stock_qty"].ToString());
       //                 if ((IssNumber != "") && (IssNumber != IsRe1))
       //                 {
       //                     slno = slno + 1;
       //                     PdfPCell cell19b = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
       //                     table.AddCell(cell19b);
       //                     PdfPCell cell20b = new PdfPCell(new Phrase(new Chunk(Issamr["Date"].ToString(), font8)));
       //                     table.AddCell(cell20b);
       //                     PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(Issamr["issueno"].ToString(), font8)));
       //                     table.AddCell(cell21b);
       //                     int Rqty = Convert.ToInt32(Issamr["issued_qty"].ToString());
       //                     PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //                     table.AddCell(cell22b);
       //                     PdfPCell cell23b = new PdfPCell(new Phrase(new Chunk(Rqty.ToString(), font8)));
       //                     table.AddCell(cell23b);

       //                     decimal open1b = Convert.ToDecimal(Issamr["Balance"].ToString());
       //                     PdfPCell cell25b = new PdfPCell(new Phrase(new Chunk(open1b.ToString(), font8)));
       //                     table.AddCell(cell25b);
       //                     PdfPCell cell24b = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //                     table.AddCell(cell24b);
       //                     IsRe1 = Issamr["issueno"].ToString();

       //                 }


       //             }


       //             j++;

       //         }
       //         else
       //         {

       //             if (ItemId == int.Parse(dr["item_id"].ToString()))
       //             {
       //             }
       //             else
       //             {
       //                 //PdfPTable table1 = new PdfPTable(8);
       //                 //table1.TotalWidth = 750f;
       //                 ItemId = int.Parse(dr["item_id"].ToString());
       //                 //PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("ALLOCATION TYPE:       " + reqtype, font8)));
       //                 //cell1a.Colspan = 9;
       //                 //cell1a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
       //                 //table.AddCell(cell1a);
       //                 PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Store Name", font9)));
       //                 cella.Border = 1;
       //                 table5.AddCell(cella);
       //                 string Store = cmbStockRegistry.SelectedItem.ToString();
       //                 PdfPCell cellb = new PdfPCell(new Phrase(new Chunk(Store.ToString(), font8)));
       //                 cellb.Border = 1;
       //                 table1.AddCell(cellb);
       //                 PdfPCell cellc = new PdfPCell(new Phrase(new Chunk("Item Name", font9)));
       //                 cellc.Border = 1;
       //                 table5.AddCell(cellc);
       //                 string Item = dr["itemname"].ToString();
       //                 PdfPCell celld = new PdfPCell(new Phrase(new Chunk(Item.ToString(), font8)));
       //                 celld.Border = 1;
       //                 table5.AddCell(celld);
       //                 PdfPCell celle = new PdfPCell(new Phrase(new Chunk("Item Code", font9)));
       //                 celle.Border = 1;
       //                 table5.AddCell(celle);
       //                 string Code = dr["itemcode"].ToString();
       //                 PdfPCell cellf = new PdfPCell(new Phrase(new Chunk(Code.ToString(), font8)));
       //                 cellf.Border = 1;
       //                 table5.AddCell(cellf);
       //                 PdfPCell cellg = new PdfPCell(new Phrase(new Chunk("Opening Date", font9)));
       //                 cellg.Border = 1;
       //                 table5.AddCell(cellg);
       //                 string date = dr["Date"].ToString();
       //                 PdfPCell cellh = new PdfPCell(new Phrase(new Chunk(date.ToString(), font8)));
       //                 cellh.Border = 1;
       //                 table5.AddCell(cellh);
       //                 PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
       //                 table.AddCell(cell19);
       //                 PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(dr["Date"].ToString(), font8)));
       //                 table.AddCell(cell20);
       //                 PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Opening Stock", font9)));
       //                 table.AddCell(cell21);

       //                 PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("0", font8)));
       //                 table.AddCell(cell22);
       //                 PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk("0", font8)));
       //                 table.AddCell(cell23);
       //                 decimal open1 = Convert.ToDecimal(dr["openingstock"].ToString());
       //                 PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(open1.ToString(), font8)));
       //                 table.AddCell(cell25);
       //                 PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //                 table.AddCell(cell24);
       //                 string Rreq1 = "";
       //                 OdbcCommand Req = new OdbcCommand("SELECT distinct reqno,req_qty,Date,openingstock from tempstockledger where item_id=" + ItemId + " group by issueno", conn);
       //                 OdbcDataReader Reqr = Req.ExecuteReader();
       //                 while (Reqr.Read())
       //                 {

       //                     string Rreq = Reqr["reqno"].ToString();
       //                     if ((Rreq != "") && (Rreq != Rreq1))
       //                     {
       //                         slno = slno + 1;
       //                         PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
       //                         table.AddCell(cell19a);
       //                         PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk(dr["Date"].ToString(), font8)));
       //                         table.AddCell(cell20a);
       //                         PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk(dr["reqno"].ToString(), font8)));
       //                         table.AddCell(cell21a);
       //                         int Rqty = Convert.ToInt32(dr["req_qty"].ToString());
       //                         PdfPCell cell22a = new PdfPCell(new Phrase(new Chunk(Rqty.ToString(), font8)));
       //                         table.AddCell(cell22a);
       //                         PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //                         table.AddCell(cell23a);
       //                         decimal open1a = Convert.ToDecimal(dr["openingstock"].ToString());
       //                         PdfPCell cell25a = new PdfPCell(new Phrase(new Chunk(open1a.ToString(), font8)));
       //                         table.AddCell(cell25a);
       //                         PdfPCell cell24a = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //                         table.AddCell(cell24a);
       //                         Rreq1 = Reqr["reqno"].ToString();
       //                     }

       //                 }
       //                 string IsRe1 = "";
       //                 OdbcCommand Issamount = new OdbcCommand("select distinct iss.issueno,DATE_FORMAT(iss.createdon,'%d-%m-%Y') as Date,iss.issued_qty,rs.req_qty,(inv.stock_qty-iss.issued_qty) as Balance,inv.stock_qty from t_inventoryrequest_items_issue iss,t_inventoryrequest_issue iu,t_inventoryrequest_items rs,m_inventory inv where iss.issueno=iu.issueno and iu.reqno=rs.reqno and rs.reqno='" + Rnumber.ToString() + "' and iss.item_id=" + ItemId + " and rs.item_id=inv.item_id group by issueno", conn);
       //                 OdbcDataReader Issamr = Issamount.ExecuteReader();
       //                 while (Issamr.Read())
       //                 {
       //                     string IssNumber = Issamr["issueno"].ToString();
       //                     if ((IssNumber != "") && (IssNumber != IsRe1))
       //                     {
       //                         slno = slno + 1;
       //                         PdfPCell cell19b = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
       //                         table.AddCell(cell19b);
       //                         PdfPCell cell20b = new PdfPCell(new Phrase(new Chunk(Issamr["Date"].ToString(), font8)));
       //                         table.AddCell(cell20b);
       //                         PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(Issamr["issueno"].ToString(), font8)));
       //                         table.AddCell(cell21b);
       //                         int Rqty = Convert.ToInt32(Issamr["issued_qty"].ToString());
       //                         PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //                         table.AddCell(cell22b);
       //                         PdfPCell cell23b = new PdfPCell(new Phrase(new Chunk(Rqty.ToString(), font8)));
       //                         table.AddCell(cell23b);
       //                         decimal open1b = Convert.ToDecimal(Issamr["Balance"].ToString());
       //                         PdfPCell cell25b = new PdfPCell(new Phrase(new Chunk(open1b.ToString(), font8)));
       //                         table.AddCell(cell25b);
       //                         PdfPCell cell24b = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //                         table.AddCell(cell24b);
       //                         IsRe1 = Issamr["issueno"].ToString();
       //                     }


       //                 }

       //                 j++;
       //                 slno = 1;

       //             }

       //         }
       //         #region COMMENTED**********
       //         //PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
       //         //table.AddCell(cell19);
       //         //PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(dr["Date"].ToString(), font8)));
       //         //table.AddCell(cell20);
       //         //PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Opening Stock", font8)));
       //         //table.AddCell(cell21);

       //         //PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("0", font8)));
       //         //table.AddCell(cell22);
       //         //PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk("0", font8)));
       //         //table.AddCell(cell23);
       //         //decimal open1 = Convert.ToDecimal(dr["openingstock"].ToString());
       //         //PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(open1.ToString(), font8)));
       //         //table.AddCell(cell25);
       //         //PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //         //table.AddCell(cell24);
       //         ////doc.Add(table);

       //         //OdbcCommand Req = new OdbcCommand("SELECT distinct reqno,req_qty,Date,openingstock from tempstockledger where item_id=" + ItemId + "", con);
       //         //OdbcDataReader Reqr = Req.ExecuteReader();
       //         //while (Reqr.Read())
       //         //{

       //         //    string Rreq = Reqr["reqno"].ToString();
       //         //    if (Rreq != "")
       //         //    {
       //         //        slno = slno + 1;
       //         //        PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
       //         //        table.AddCell(cell19a);
       //         //        PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk(dr["Date"].ToString(), font8)));
       //         //        table.AddCell(cell20a);
       //         //        PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk(dr["reqno"].ToString(), font8)));
       //         //        table.AddCell(cell21a);
       //         //        int Rqty = Convert.ToInt32(dr["req_qty"].ToString());
       //         //        PdfPCell cell22a = new PdfPCell(new Phrase(new Chunk(Rqty.ToString(), font8)));
       //         //        table.AddCell(cell22a);
       //         //        PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //         //        table.AddCell(cell23a);
       //         //        decimal open1a = Convert.ToDecimal(dr["openingstock"].ToString());
       //         //        PdfPCell cell25a = new PdfPCell(new Phrase(new Chunk(open1a.ToString(), font8)));
       //         //        table.AddCell(cell25a);
       //         //        PdfPCell cell24a = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //         //        table.AddCell(cell24a);

       //         //    }

       //         //}
       //         //OdbcCommand Issamount = new OdbcCommand("select distinct iss.issueno,DATE_FORMAT(iss.createdon,'%d-%m-%Y') as Date,iss.issued_qty,rs.req_qty,(inv.stock_qty-iss.issued_qty) as Balance from t_inventoryrequest_items_issue iss,t_inventoryrequest_issue iu,t_inventoryrequest_items rs,m_inventory inv where iss.issueno=iu.issueno and iu.reqno=rs.reqno and rs.reqno='" + Rnumber.ToString() + "' and iss.item_id=" + ItemId + " and rs.item_id=inv.item_id group by issueno", con);
       //         //OdbcDataReader Issamr = Issamount.ExecuteReader();
       //         //while (Issamr.Read())
       //         //{
       //         //    string IssNumber = Issamr["issueno"].ToString();
       //         //    if (IssNumber != "")
       //         //    {
       //         //        slno = slno + 1;
       //         //        PdfPCell cell19b = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
       //         //        table.AddCell(cell19b);
       //         //        PdfPCell cell20b = new PdfPCell(new Phrase(new Chunk(Issamr["Date"].ToString(), font8)));
       //         //        table.AddCell(cell20b);
       //         //        PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(Issamr["issueno"].ToString(), font8)));
       //         //        table.AddCell(cell21b);
       //         //        int Rqty = Convert.ToInt32(Issamr["issued_qty"].ToString());
       //         //        PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //         //        table.AddCell(cell22b);
       //         //        PdfPCell cell23b = new PdfPCell(new Phrase(new Chunk(Rqty.ToString(), font8)));
       //         //        table.AddCell(cell23b);
       //         //        decimal open1b = Convert.ToDecimal(Issamr["Balance"].ToString());
       //         //        PdfPCell cell25b = new PdfPCell(new Phrase(new Chunk(open1b.ToString(), font8)));
       //         //        table.AddCell(cell25b);
       //         //        PdfPCell cell24b = new PdfPCell(new Phrase(new Chunk(" ", font8)));
       //         //        table.AddCell(cell24b);

       //         //    }


       //         //}
       //         #endregion

       //     }

       // }
       // doc.Add(table1);
       // doc.Add(table5);
       // doc.Add(table);
       // doc.Close();
       // //System.Diagnostics.Process.Start(pdfFilePath);
       // Random r = new Random();
       // string PopUpWindowPage = "print.aspx?reportname=stockregistry5.pdf";
       // string Script = "";
       // Script += "<script id='PopupWindow'>";
       // Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
       // Script += "confirmWin.Setfocus()</script>";
       // if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
       //     Page.RegisterClientScriptBlock("PopupWindow", Script);
#endregion
            
    protected void lnkStock_Click(object sender, EventArgs e)
    {
        btnStock.Visible = true;
        cmbStockRegistry.Visible = true;
        cmbStockItem.Visible = true;
        lblStoreName.Visible = true;
        lblItName.Visible = true;
        lblStaff.Visible = false;
        cmbStaff.Visible = false;
        btnLed.Visible = false;
        lblStore1.Visible = false;
        cmbStore1.Visible = false;
        lblRStore.Visible = false;
        cmbRStore.Visible = false;
        btnRol.Visible = false;
        lnkAuthor.Visible = false;
    }
    protected void cmbStockRegistry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        OdbcDataAdapter StockIt = new OdbcDataAdapter("select itemname,inv.item_id from m_sub_item it,m_inventory inv where inv.store_id=(select store_id as "
             + "id from m_sub_store where storename='" + cmbStockRegistry.SelectedItem.Text.ToString() + "' and rowstatus<>'2' UNION select counter_id as id from "
             + "m_sub_counter where counter_no='" + cmbStockRegistry.SelectedItem.Text.ToString() + "' and rowstatus<>'2') and inv.item_id=it.item_id group by inv.item_id,inv.store_id", conn);
        DataTable ds1 = new DataTable();
        DataRow row = ds1.NewRow();
        StockIt.Fill(ds1);
        row["item_id"] = "-1";
        row["itemname"] = "--Select--";
        ds1.Rows.InsertAt(row, 0);       
        cmbStockItem.DataSource = ds1;
        cmbStockItem.DataBind();
        conn.Close();

    }

    protected void dtgAItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        dtgAItem.Visible = true;
        pnlapprove1.Visible = true;

        string Ap;
        Ap = dtgAItem.SelectedRow.Cells[1].Text;
        Session["row1"] = Ap;
        txtIssueOfficer.Visible = true;
        lblIssueOfficer.Visible = true;
        btnIssue.Visible = true;

        OdbcCommand Grid5 = new OdbcCommand();
        Grid5.CommandType = CommandType.StoredProcedure;
        Grid5.Parameters.AddWithValue("tblname", "t_inventoryrequest");
        Grid5.Parameters.AddWithValue("attribute", "reqno,req_officer,req_from,iss_officer,office_request,office_issue,DATE_FORMAT(date_request,'%d-%m-%Y') as Date");
        Grid5.Parameters.AddWithValue("conditionv", "reqno='" + Ap.ToString() + "' and reqstatus=1");
        OdbcDataAdapter Gridr = new OdbcDataAdapter(Grid5);
        DataTable ds = new DataTable();
        ds = obje.SpDtTbl("CALL selectcond(?,?,?)", Grid5);

          #region COMMENTED*************************
        //OdbcCommand Grid5 = new OdbcCommand("select reqno,req_officer,req_from,iss_officer,office_request,office_issue,DATE_FORMAT(date_request,'%d-%m-%Y') as Date FROM t_inventoryrequest where reqno='"+Ap.ToString()+"' and reqstatus=1", conn);
        //OdbcDataReader Gridr = Grid5.ExecuteReader();
        //while (Gridr.Read())
            #endregion 
        conn = obje.NewConnection();
        for(int k=0;k<ds.Rows.Count;k++)
        {
            txtRequestNo.Text = ds.Rows[k]["reqno"].ToString();
            txtRequestOfficer.Text = ds.Rows[k]["req_officer"].ToString();
            txtDate.Text = ds.Rows[k]["Date"].ToString();
            int rs = Convert.ToInt32(ds.Rows[k]["office_request"].ToString());
            int iss;
            try
            {
                iss = Convert.ToInt32(ds.Rows[k]["office_issue"].ToString());
            }
            catch
            {
                iss = 0;
            }
            int ReqFr = Convert.ToInt32(ds.Rows[k]["req_from"].ToString());
            if (ReqFr == 0)
            {
       
                OdbcCommand stor = new OdbcCommand("SELECT distinct s.storename as Name,CAST(concat('S',`store_id`) as CHAR) as Id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + rs + "", conn);
                OdbcDataReader storr = stor.ExecuteReader();
                if (storr.Read())
                {
                    cmbReqStore.SelectedItem.Text = storr["Name"].ToString();
                    cmbReqStore.SelectedValue = storr["Id"].ToString();
                }
            }
            else if (ReqFr == 1)
            {

                OdbcCommand stor = new OdbcCommand("SELECT distinct s.counter_no as Name,CAST(concat('C',`counter_id`) as CHAR) as Id from m_sub_counter s where s.rowstatus<>'2' and s.counter_id=" + rs + "", conn);
                OdbcDataReader storr = stor.ExecuteReader();
                if (storr.Read())
                {
                    cmbReqStore.SelectedItem.Text = storr["Name"].ToString();
                    cmbReqStore.SelectedValue = storr["Id"].ToString();
                }
            }
            else if (ReqFr == 2)
            {

                OdbcCommand stor = new OdbcCommand("SELECT distinct s.teamname as Name,CAST(concat('T',`team_id`) as CHAR) as Id from m_team s where s.rowstatus<>'2' and s.team_id=" + rs + "", conn);
                OdbcDataReader storr = stor.ExecuteReader();
                if (storr.Read())
                {
                    cmbReqStore.SelectedItem.Text = storr["Name"].ToString();
                    cmbReqStore.SelectedValue = storr["Id"].ToString();
                }
            }
            
            
            OdbcCommand storis = new OdbcCommand("SELECT distinct s.storename as Sname,s.store_id as Id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + iss + "", conn);
            OdbcDataReader storris = storis.ExecuteReader();
            if (storris.Read())
            {
                cmbIssueStore.SelectedItem.Text = storris["Sname"].ToString();
                cmbIssueStore.SelectedValue = storris["Id"].ToString();
            }
            
        }
        Approve5();


    }

    public void Approve5()
    {

        #region Request
        string l;
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        l = dtgAItem.SelectedRow.Cells[1].Text;
        Session["reqnumber1"] = l;

        OdbcCommand Appr1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        Appr1.CommandType = CommandType.StoredProcedure;
        Appr1.Parameters.AddWithValue("tblname", "m_sub_item n,t_inventoryrequest_items t,m_sub_itemcategory i,m_inventory m");
        Appr1.Parameters.AddWithValue("attribute", "distinct t.reqno as reqno,t.item_id as item_id,m.itemcode as Itemcode,i.itemcatname as itemcatname,n.itemname as itemname,t.approved_qty as approved_qty");
        Appr1.Parameters.AddWithValue("conditionv", "t.reqno='" + l + "' and t.item_id=n.item_id and t.item_status='1' and t.approved_qty >0 and n.itemcat_id=i.itemcat_id and m.rowstatus<>'2' and m.itemcat_id=i.itemcat_id and m.item_id =n.item_id");

        OdbcDataAdapter da = new OdbcDataAdapter(Appr1);//("SELECT distinct t.reqno as reqno,t.item_id as item_id,m.itemcode as Itemcode,i.itemcatname as itemcatname,n.itemname as itemname,t.approved_qty as approved_qty FROM m_sub_item n,t_inventoryrequest_items t,m_sub_itemcategory i,m_inventory m WHERE t.reqno='" + l + "' and t.item_id=n.item_id and t.item_status='1' and t.approved_qty >0 and n.itemcat_id=i.itemcat_id and m.rowstatus<>'2' and m.itemcat_id=i.itemcat_id and m.item_id =n.item_id", conn);
        DataTable ds = new DataTable();
        da.Fill(ds);
        dtgApproved.DataSource = ds;
        dtgApproved.DataBind();

        OdbcCommand Appr2 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        Appr2.CommandType = CommandType.StoredProcedure;
        Appr2.Parameters.AddWithValue("tblname", "m_sub_item n,t_inventoryrequest_items t,m_sub_itemcategory i,m_inventory m");
        Appr2.Parameters.AddWithValue("attribute", "distinct t.reqno as reqno,t.item_id as item_id,m.itemcode as Itemcode,i.itemcatname as itemcatname,n.itemname as itemname,(t.approved_qty-t.issued_qty) as approved_qty");
        Appr2.Parameters.AddWithValue("conditionv", "t.reqno='" + l + "' and t.item_id=n.item_id and t.item_status='4' and n.itemcat_id=i.itemcat_id and m.rowstatus<>'2' and (t.approved_qty-t.issued_qty) >'0' and m.itemcat_id=i.itemcat_id group by item_id");

        OdbcDataAdapter da11 = new OdbcDataAdapter(Appr2);//("SELECT distinct t.reqno as reqno,t.item_id as item_id,m.itemcode as Itemcode,i.itemcatname as itemcatname,n.itemname as itemname,(t.approved_qty-t.issued_qty) as approved_qty FROM m_sub_item n,t_inventoryrequest_items t,m_sub_itemcategory i,m_inventory m WHERE t.reqno='" + l + "' and t.item_id=n.item_id and t.item_status='4' and n.itemcat_id=i.itemcat_id and m.rowstatus<>'2' and (t.approved_qty-t.issued_qty) >'0' and m.itemcat_id=i.itemcat_id group by item_id", conn);
        da11.Fill(ds);
        dtgApproved.DataSource = ds;
        dtgApproved.DataBind();

        OdbcCommand Appr3 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        Appr3.CommandType = CommandType.StoredProcedure;
        Appr3.Parameters.AddWithValue("tblname", "m_sub_item n,t_inventoryrequest_items t,m_sub_itemcategory i,m_inventory m");
        Appr3.Parameters.AddWithValue("attribute", "distinct t.reqno as reqno,t.item_id as item_id,m.itemcode as Itemcode,i.itemcatname as itemcatname,n.itemname as itemname,t.approved_qty as approved_qty");
        Appr3.Parameters.AddWithValue("conditionv", "t.reqno='" + l + "' and t.item_id=n.item_id and t.item_status='3' and n.itemcat_id=i.itemcat_id and m.rowstatus<>'2' and t.approved_qty >'0' and m.itemcat_id=i.itemcat_id group by item_id");

        OdbcDataAdapter da12 = new OdbcDataAdapter(Appr3);//("SELECT distinct t.reqno as reqno,t.item_id as item_id,m.itemcode as Itemcode,i.itemcatname as itemcatname,n.itemname as itemname,t.approved_qty as approved_qty FROM m_sub_item n,t_inventoryrequest_items t,m_sub_itemcategory i,m_inventory m WHERE t.reqno='" + l + "' and t.item_id=n.item_id and t.item_status='3' and n.itemcat_id=i.itemcat_id and m.rowstatus<>'2' and t.approved_qty >'0' and m.itemcat_id=i.itemcat_id group by item_id", conn);
        da12.Fill(ds);
        dtgApproved.DataSource = ds;
        dtgApproved.DataBind();


        #endregion
    }
    protected void dtgAItem_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgAItem, "Select$" + e.Row.RowIndex);
        }
    }
    protected void TextBox6_TextChanged(object sender, EventArgs e)
    {
        #region STARTING SERIAL NO FOR RECEIPT OR PASS
        int NewItem = int.Parse(dtgApproved.DataKeys[0].Values[1].ToString());
        TextBox txt = (TextBox)(sender as TextBox);
        string str = txt.Text;
        int Eend = int.Parse(str);
        ViewState["startslno"] = Eend;
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        OdbcCommand Slno1 = new OdbcCommand();
        Slno1.CommandType = CommandType.StoredProcedure;
        Slno1.Parameters.AddWithValue("tblname", "t_inventoryrequest_items_issue");
        Slno1.Parameters.AddWithValue("attribute", "*");
        Slno1.Parameters.AddWithValue("conditionv", "" + Eend + ">=start_slno and " + Eend + "<=end_slno and item_id=" + NewItem + "");
        OdbcDataAdapter Slm1 = new OdbcDataAdapter(Slno1);
        DataTable ds = new DataTable();
        ds = obje.SpDtTbl("CALL selectcond(?,?,?)", Slno1);

        #region COMMENTED*****************
        //OdbcCommand Slno1 = new OdbcCommand("select * from t_inventoryrequest_items_issue where " + Eend + ">=start_slno and " + Eend + "<=end_slno and item_id="+NewItem+"", conn);
        //OdbcDataReader Slm1 = Slno1.ExecuteReader();
        #endregion

        if (ds.Rows.Count>0)
        {
            txt.Text = "";
            lblOk.Text = " This Item is Already Issued with the Same Serial Number"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        #endregion
    }
    protected void TextBox7_TextChanged(object sender, EventArgs e)
    {
        #region ENDING SERIAL NO FOR RECEIPT OR PASS
        int NewItem = int.Parse(dtgApproved.DataKeys[0].Values[1].ToString());
        TextBox txt = (TextBox)(sender as TextBox);
        string str = txt.Text;
        int Eend = int.Parse(str);
        ViewState["endslno"] = Eend;
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        OdbcCommand Slno1 = new OdbcCommand();
        Slno1.CommandType = CommandType.StoredProcedure;
        Slno1.Parameters.AddWithValue("tblname", "t_inventoryrequest_items_issue");
        Slno1.Parameters.AddWithValue("attribute", "*");
        Slno1.Parameters.AddWithValue("conditionv", "" + Eend + ">=start_slno and " + Eend + "<=end_slno and item_id=" + NewItem + "");
        OdbcDataAdapter Slm1 = new OdbcDataAdapter(Slno1);
        DataTable ds = new DataTable();
        ds = obje.SpDtTbl("CALL selectcond(?,?,?)", Slno1);

        #region COMMENTED*****************
        //OdbcCommand Slno1 = new OdbcCommand("select * from t_inventoryrequest_items_issue where " + Eend + ">=start_slno and " + Eend + "<=end_slno and item_id=" + NewItem + "", conn);
        //OdbcDataReader Slm1 = Slno1.ExecuteReader();
        #endregion

        if (ds.Rows.Count>0)
        {
            txt.Text = "";
            lblOk.Text = " This Item is Already Issued with the Same Serial Number"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        #endregion
    }
    protected void lnkRol_Click(object sender, EventArgs e)
    {
        btnStock.Visible = false;
        cmbStockRegistry.Visible = false;
        cmbStockItem.Visible = false;
        lblStoreName.Visible = false;
        lblItName.Visible = false;
        lblStaff.Visible = false;
        cmbStaff.Visible = false;
        btnLed.Visible = false;
        lblStore1.Visible = false;
        cmbStore1.Visible = false;
        lblRStore.Visible = true;
        cmbRStore.Visible = true;
        btnRol.Visible = true;
        lnkAuthor.Visible = false;
        lblBuilding.Visible = false;
        cmbKBuilding.Visible = false;
        lblRoomNO.Visible = false;
        cmbKRoom.Visible = false;
        lblKStore.Visible = false;
        cmbKStore.Visible = false;
        btnKeyStockLed.Visible = false;

    }
    protected void cmbIssueStore_SelectedIndexChanged1(object sender, EventArgs e)
    {

        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

       
        //string aa = "select distinct c.itemcat_id,itemcatname from m_sub_itemcategory c,m_inventory inv where inv.rowstatus<>'2' "
        //                     + "and c.rowstatus<>'2'and inv.store_id=" + cmbIssueStore.SelectedValue + " and inv.rowstatus<>'2' and c.itemcat_id=inv.itemcat_id";
        OdbcDataAdapter Store1A = new OdbcDataAdapter("select distinct c.itemcat_id,itemcatname from m_sub_itemcategory c,m_inventory inv where inv.rowstatus<>'2' "
                             + "and c.rowstatus<>'2'and inv.store_id=" + cmbIssueStore.SelectedValue + " and inv.rowstatus<>'2' and c.itemcat_id=inv.itemcat_id", conn);
  

        //OdbcCommand Store1A = new OdbcCommand();
        //Store1A.CommandType = CommandType.StoredProcedure;
        //Store1A.Parameters.AddWithValue("tblname", "m_sub_itemcategory c,m_inventory inv");
        //Store1A.Parameters.AddWithValue("attribute", "distinct c.itemcat_id,itemcatname");
        //Store1A.Parameters.AddWithValue("conditionv", "inv.rowstatus<>'2' and c.rowstatus<>'2'and inv.store_id=" + cmbIssueStore.SelectedValue + " and inv.rowstatus<>'2' and c.itemcat_id=inv.itemcat_id");
        //OdbcDataAdapter Store1B = new OdbcDataAdapter(Store1A);
        DataTable ds1 = new DataTable();
        DataRow row = ds1.NewRow();
        Store1A.Fill(ds1);
        //ds1 = obje.SpDtTbl("CALL selectcond(?,?,?)", Store1A);
        row["itemcat_id"] = "-1";
        row["itemcatname"] = "--Select--";
        ds1.Rows.InsertAt(row, 0);       
        cmbItem.DataSource = ds1;
        cmbItem.DataBind();
        conn.Close();
    }

    protected void dtgRequestedItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgRequestedItems.PageIndex = e.NewPageIndex;
        dtgRequestedItems.DataBind();
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        RequestedItems();
    }
    protected void dtgAItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgAItem.PageIndex = e.NewPageIndex;
        dtgAItem.DataBind();
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        ApproveItems();
    }
    protected void btnLed_Click(object sender, EventArgs e)
    {
        #region STORE MANAGER'S LIABILITY LEDGER
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        int no = 0;
        int SeasId;
      
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string Dat = gh.ToString("dd MMMM yyyy");
        string ch = "StoreManagerLiability" + transtim.ToString() +".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        OdbcCommand Malayalam = new OdbcCommand();
        Malayalam.CommandType = CommandType.StoredProcedure;
        Malayalam.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
        Malayalam.Parameters.AddWithValue("attribute", "seasonname,season_id");
        Malayalam.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");
        OdbcDataAdapter Malayalam1 = new OdbcDataAdapter(Malayalam);
        DataTable dm = new DataTable();
        dm = obje.SpDtTbl("CALL selectcond(?,?,?)", Malayalam);

        #region COMMENTED*****************
        //Malayalam1.Fill(dm);
        //OdbcCommand Malayalam = new OdbcCommand("select seasonname,season_id from m_season s,m_sub_season d where curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'", conn);
        //OdbcDataReader Malr = Malayalam.ExecuteReader();
        //if (Malr.Read())
        #endregion

        for (int k=0;k<dm.Rows.Count;k++)
        {
             SeasId= Convert.ToInt32(dm.Rows[k][1].ToString());
             SeasName= dm.Rows[k][0].ToString();
        }

        OdbcCommand Liability = new OdbcCommand();
        Liability.CommandType = CommandType.StoredProcedure;
        Liability.Parameters.AddWithValue("tblname", "m_inventory inv,m_sub_store ms,m_staff s,m_sub_item im,m_sub_unit u");
        Liability.Parameters.AddWithValue("attribute", "storename,staffname,ms.manager_id,stock_qty,itemname,itemcode,unitname");
        Liability.Parameters.AddWithValue("conditionv", "ms.manager_id=s.staff_id and inv.store_id=ms.store_id and im.item_id=inv.item_id and ms.manager_id=" + cmbStaff.SelectedValue + " and inv.store_id=" + cmbStore1.SelectedValue + " and ms.rowstatus<>2 and inv.rowstatus<>2 and u.unit_id=inv.unit_id group by itemname");

        #region COMMENTED***************
        //OdbcCommand Liability = new OdbcCommand("select storename,staffname,ms.manager_id,stock_qty,itemname,itemcode,unitname from m_inventory inv,m_sub_store ms,"
        //       + "m_staff s,m_sub_item im,m_sub_unit u where ms.manager_id=s.staff_id and inv.store_id=ms.store_id and im.item_id=inv.item_id and "
        //       + "ms.manager_id=" + cmbStaff.SelectedValue + " and inv.store_id=" + cmbStore1.SelectedValue + " and ms.rowstatus<>2 and inv.rowstatus<>2 and u.unit_id=inv.unit_id "
        //       + "group by itemname", conn);
        #endregion

        OdbcDataAdapter Liabr = new OdbcDataAdapter(Liability);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", Liability);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = "No Details found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        PdfPTable table1 = new PdfPTable(7);
        table1.TotalWidth = 550f;
        table1.LockedWidth = true;
        float[] colwidth1 ={ 2, 4, 5, 5, 5, 5,5 };
        table1.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("STORE MANAGER LIABILITY LEDGER", font10)));
        cell.Colspan = 7;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table1.AddCell(cell);
        
        PdfPCell ce11p = new PdfPCell(new Phrase(new Chunk("Store Name :  " +cmbStore1.SelectedItem.Text.ToString() , font11)));
        ce11p.Colspan = 4;
        ce11p.Border = 0;
        table1.AddCell(ce11p);
        PdfPCell celb = new PdfPCell(new Phrase(new Chunk("Manager Name:  " + cmbStaff.SelectedItem.Text.ToString(), font11)));
        celb.Colspan = 3;
        celb.Border = 0;
        table1.AddCell(celb);
        PdfPCell cellu = new PdfPCell(new Phrase(new Chunk("Season:  " + SeasName, font11)));
        cellu.Colspan = 4;
        cellu.Border = 0;
        table1.AddCell(cellu);
        PdfPCell celc = new PdfPCell(new Phrase(new Chunk("Date:  " + Dat, font11)));
        celc.Colspan = 3;
        celc.Border = 0;
        table1.AddCell(celc);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table1.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Code", font9)));
        table1.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Name", font9)));
        table1.AddCell(cell13);
        PdfPCell cell14q = new PdfPCell(new Phrase(new Chunk("UOM", font9)));
        table1.AddCell(cell14q);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Stock Qty ", font9)));
        table1.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Unit cost", font9)));
        table1.AddCell(cell15);
        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Total value", font9)));
        table1.AddCell(cell16);
        doc.Add(table1);

        int i = 0;

        
        foreach (DataRow dr in dt.Rows)
        {
            no = no + 1;
            string num = no.ToString();
            if (i > 42)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table2 = new PdfPTable(7);
                table2.TotalWidth = 550f;
                table2.LockedWidth = true;
                float[] colwidth2 ={ 2, 4, 5, 5, 5, 5, 5 };
                table2.SetWidths(colwidth2);
                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table2.AddCell(cell11a);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Code", font9)));
                table2.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("Name", font9)));
                table2.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("UOM", font9)));
                table2.AddCell(cell14a);
                PdfPCell cell14c = new PdfPCell(new Phrase(new Chunk("Stock Qty", font9)));
                table2.AddCell(cell14c);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Unit cost", font9)));
                table2.AddCell(cell15a);
                PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk("Total value", font9)));
                table2.AddCell(cell16a);
                doc.Add(table2);
            }


            PdfPTable table = new PdfPTable(7);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth3 ={ 2, 4, 5, 5, 5, 5, 5 };
            table.SetWidths(colwidth3);

            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21b);

            PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(dr["itemcode"].ToString(), font8)));
            table.AddCell(cell22b);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(dr["itemname"].ToString(), font8)));
            table.AddCell(cell23);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(dr["unitname"].ToString(), font8)));
            table.AddCell(cell23a);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(dr["stock_qty"].ToString(), font8)));
            table.AddCell(cell24);
            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk("", font8)));
            table.AddCell(cell25);
            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk("", font8)));
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

        if ( dt.Rows.Count == 0)
        {
            lblOk.Text = "No Liability found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();

            //doc.Add(table);
            doc.Close();
            return;
        }
        //doc.Add(table);
        doc.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Store Manager Liability Account";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        conn.Close();
        #endregion
    }
    protected void cmbStaff_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        OdbcCommand Liability = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        Liability.CommandType = CommandType.StoredProcedure;
        Liability.Parameters.AddWithValue("tblname", "m_inventory inv,m_sub_store ms,m_staff s,m_sub_item im");
        Liability.Parameters.AddWithValue("attribute", "distinct inv.store_id,storename");
        Liability.Parameters.AddWithValue("conditionv", "ms.manager_id=s.staff_id and inv.store_id=ms.store_id and im.item_id=inv.item_id and inv.rowstatus<>'2' and ms.manager_id=" + cmbStaff.SelectedValue + " group by inv.store_id");
        DataTable ds1 = new DataTable();
        OdbcDataAdapter Stock5 = new OdbcDataAdapter(Liability);
        DataRow row1 = ds1.NewRow();
        Stock5.Fill(ds1);
        row1["store_id"] = "-1";
        row1["storename"] = "--Select--";
        ds1.Rows.InsertAt(row1, 0);
        cmbStore1.DataSource = ds1;
        cmbStore1.DataBind();
        conn.Close();

        #region COMMENTED****************
        //OdbcDataAdapter Stock5 = new OdbcDataAdapter("select storename,inv.store_id from m_inventory inv,m_sub_store ms,m_staff s,m_sub_item im where "
        //       + "ms.manager_id=s.staff_id and inv.store_id=ms.store_id and im.item_id=inv.item_id and ms.manager_id="+cmbStaff.SelectedValue+" group by inv.store_id", conn);
        //DataTable ds1 = new DataTable();
        //DataRow row1 = ds1.NewRow();
        //Stock5.Fill(ds1);
        //row1["store_id"] = "-1";
        //row1["storename"] = "--Select--";
        //ds1.Rows.InsertAt(row1, 0);
        //cmbStore1.DataSource = ds1;
        //cmbStore1.DataBind();
        //conn.Close();
        #endregion
    }
    protected void lnkRequestItem_Click(object sender, EventArgs e)
    {
        #region REQUESTED ITEM DETAILS

        lnkStoreManager.Visible = true;
        LnkDPStockLed.Visible = true;
        lnkPPSL.Visible = true;
        lnkStock.Visible = true;


        string ReqDat1;
        DateTime Cur,Rdate1;
               
        Cur = DateTime.Now;
        string Cur1 = Cur.ToString("yyyy/MM/dd");
        if (txtFromDate.Text != "")
        {
             FD =obje.yearmonthdate(txtFromDate.Text);
        }
        if (txtToDate.Text != "")
        {
            TD = obje.yearmonthdate(txtToDate.Text);
        }
        OdbcCommand LnReq=new OdbcCommand();
        LnReq.CommandType = CommandType.StoredProcedure;
        LnReq.Parameters.AddWithValue("tblname", "t_inventoryrequest_items t,t_inventoryrequest q,m_sub_item i,m_inventory mi,m_sub_itemcategory mc,m_sub_unit mu");
        LnReq.Parameters.AddWithValue("attribute", "q.req_officer,q.req_from,q.iss_officer,q.office_request,q.office_issue,q.date_request,sum(t.req_qty) as req_qty,itemname,itemcode,unitname");
        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            LnReq.Parameters.AddWithValue("conditionv", "q.reqno=t.reqno and mi.unit_id=mu.unit_id and t.item_id=i.item_id and t.item_id=mi.item_id and mi.itemcat_id=mc.itemcat_id and q.date_request between '" + FD + "' and '" + TD + "' group by date(date_request),itemcode,office_request");
        }
        else if (txtFromDate.Text != "" && txtToDate.Text == "")
        {
            LnReq.Parameters.AddWithValue("conditionv", "q.reqno=t.reqno and mi.unit_id=mu.unit_id and t.item_id=i.item_id and t.item_id=mi.item_id and mi.itemcat_id=mc.itemcat_id and q.date_request between '" + FD + "' and '" + Cur1 + "' group by date(date_request),itemcode,office_request");
        }
        else if (txtFromDate.Text == "" && txtToDate.Text == "")
        {
            LnReq.Parameters.AddWithValue("conditionv", "q.reqno=t.reqno and mi.unit_id=mu.unit_id and t.item_id=i.item_id and t.item_id=mi.item_id and mi.itemcat_id=mc.itemcat_id group by date(date_request),itemcode,office_request");
        }

        OdbcDataAdapter ReqDat = new OdbcDataAdapter(LnReq);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", LnReq);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = "No Details found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Requested Items Details" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font9 = FontFactory.GetFont("ARIAL", 9);
        Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Material Request"; 
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        PdfPTable table2 = new PdfPTable(11);
        table2.TotalWidth = 600f;
        table2.LockedWidth = true;
        float[] colwidth1 ={ 2, 5, 4, 4,4,4,4, 4, 3, 3, 5, };
        table2.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase("Stores Requisition Note ", font10));
        cell.Colspan = 11;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table2.AddCell(cell);
        PdfPCell cell5a = new PdfPCell(new Phrase(new Chunk("No", font8)));
        cell5a.Rowspan = 2;
        table2.AddCell(cell5a);

        PdfPCell cell6a = new PdfPCell(new Phrase(new Chunk("Item Name", font8)));
        cell6a.Rowspan = 2;
        cell6a.HorizontalAlignment = 0;
        table2.AddCell(cell6a);

        PdfPCell cell8a = new PdfPCell(new Phrase(new Chunk("Code", font8)));
        cell8a.Rowspan = 2;
        table2.AddCell(cell8a);

        PdfPCell cell8b = new PdfPCell(new Phrase(new Chunk("UOM", font8)));
        cell8b.Rowspan = 2;
        table2.AddCell(cell8b);

        PdfPCell cell10k = new PdfPCell(new Phrase(new Chunk("Req Office", font8)));
        cell10k.Rowspan = 2;
        table2.AddCell(cell10k);
        PdfPCell cell10r = new PdfPCell(new Phrase(new Chunk("Req Officer", font8)));
        cell10r.Rowspan = 2;
        table2.AddCell(cell10r);
        PdfPCell cell10u = new PdfPCell(new Phrase(new Chunk("Req Date", font8)));
        cell10u.Rowspan = 2;
        table2.AddCell(cell10u);

        PdfPCell cell9a = new PdfPCell(new Phrase(new Chunk("Quantity", font8)));
        cell9a.Colspan = 3;
        cell9a.HorizontalAlignment = 1;
        table2.AddCell(cell9a);

        PdfPCell cell10a = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
        cell10a.Rowspan = 2;
        table2.AddCell(cell10a);
        

        PdfPCell cell9y = new PdfPCell(new Phrase(new Chunk("Request", font8)));
        table2.AddCell(cell9y);
        PdfPCell cell9t = new PdfPCell(new Phrase(new Chunk("Issued", font8)));
        table2.AddCell(cell9t);
        PdfPCell cell9r = new PdfPCell(new Phrase(new Chunk("Bal", font8)));
        table2.AddCell(cell9r);
        doc.Add(table2);

        int slno = 0; int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            slno = slno + 1;
            if (i > 28)
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(11);
                table1.TotalWidth = 600f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 2, 5, 4, 4, 4, 4, 4, 4, 3, 3, 5 };
                table1.SetWidths(colwidth2);
                PdfPCell cell5a1 = new PdfPCell(new Phrase(new Chunk("No", font8)));
                cell5a1.Rowspan = 2;
                table1.AddCell(cell5a1);

                PdfPCell cell6a1 = new PdfPCell(new Phrase(new Chunk("Item Name", font8)));
                cell6a1.Rowspan = 2;
                cell6a1.HorizontalAlignment = 0;
                table1.AddCell(cell6a1);

                PdfPCell cell8a1 = new PdfPCell(new Phrase(new Chunk("Code", font8)));
                cell8a1.Rowspan = 2;
                table1.AddCell(cell8a1);

                PdfPCell cell8b1 = new PdfPCell(new Phrase(new Chunk("UOM", font8)));
                cell8b1.Rowspan = 2;
                table1.AddCell(cell8b1);

                PdfPCell cell10k1 = new PdfPCell(new Phrase(new Chunk("Req Office", font8)));
                cell10k1.Rowspan = 2;
                table1.AddCell(cell10k1);
                PdfPCell cell10r1 = new PdfPCell(new Phrase(new Chunk("Req Officer", font8)));
                cell10r1.Rowspan = 2;
                table1.AddCell(cell10r1);
                PdfPCell cell10u1 = new PdfPCell(new Phrase(new Chunk("Req Date", font8)));
                cell10u1.Rowspan = 2;
                table1.AddCell(cell10u1);


                PdfPCell cell9a1 = new PdfPCell(new Phrase(new Chunk("Quantity", font8)));
                cell9a1.Colspan = 3;
                cell9a1.HorizontalAlignment = 1;
                table1.AddCell(cell9a1);

                PdfPCell cell10a1 = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                cell10a1.Rowspan = 2;
                table1.AddCell(cell10a1);
                

                PdfPCell cell9y1 = new PdfPCell(new Phrase(new Chunk("Request", font8)));
                table1.AddCell(cell9y1);
                PdfPCell cell9t1 = new PdfPCell(new Phrase(new Chunk("Issued", font8)));
                table1.AddCell(cell9t1);
                PdfPCell cell9r1 = new PdfPCell(new Phrase(new Chunk("Bal", font8)));
                table1.AddCell(cell9r1);
                doc.Add(table1);

            }

            PdfPTable table = new PdfPTable(11);
            table.TotalWidth = 600f;
            table.LockedWidth = true;
            float[] colwidth3 ={ 2, 5, 4, 4, 4, 4, 4, 4, 3, 3, 5 };
            table.SetWidths(colwidth3);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font9)));
            table.AddCell(cell11);
            string itn = dr["itemname"].ToString();
            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(itn.ToString(), font9)));
            table.AddCell(cell12);
            string ic = dr["itemcode"].ToString();
            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(ic.ToString(), font9)));
            //cell14.Colspan = 3;
            table.AddCell(cell14);
            string un = dr["unitname"].ToString();
            PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(un.ToString(), font9)));
            //cell15.Colspan = 2;
            table.AddCell(cell15);
            conn = obje.NewConnection();
            int ReqFrom = Convert.ToInt32(dr["req_from"].ToString());
            int StoreN = Convert.ToInt32(dr["office_request"].ToString());
            if (ReqFrom == 0)
            {
                OdbcCommand stora4 = new OdbcCommand("SELECT distinct s.storename as Name,store_id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + StoreN + "", conn);
                OdbcDataReader storra4 = stora4.ExecuteReader();
                if (storra4.Read())
                {
                    StorName1 = storra4[0].ToString();

                }
            }
            else if (ReqFrom == 1)
            {
                OdbcCommand stora4 = new OdbcCommand("SELECT distinct s.counter_no as Name,counter_id from m_sub_counter s where s.rowstatus<>'2' and s.counter_id=" + StoreN + "", conn);
                OdbcDataReader storra4 = stora4.ExecuteReader();
                if (storra4.Read())
                {
                    StorName1 = storra4[0].ToString();

                }

            }
            else if (ReqFrom == 2)
            {
                OdbcCommand stora4 = new OdbcCommand("SELECT distinct s.teamname as Name,team_id from m_team s where s.rowstatus<>'2' and s.team_id=" + StoreN + "", conn);
                OdbcDataReader storra4 = stora4.ExecuteReader();
                if (storra4.Read())
                {
                    StorName1 = storra4[0].ToString();
                }
            }
            else
            {

                StorName1 = "";
            }

            PdfPCell cell17a = new PdfPCell(new Phrase(new Chunk(StorName1, font9)));
            table.AddCell(cell17a);
            PdfPCell cell17b = new PdfPCell(new Phrase(new Chunk(dr["req_officer"].ToString(), font9)));
            table.AddCell(cell17b);

            Rdate1 = DateTime.Parse(dr["date_request"].ToString());
            ReqDat1 = Rdate1.ToString("dd MMM");
            PdfPCell cell17f = new PdfPCell(new Phrase(new Chunk(ReqDat1, font9)));
            table.AddCell(cell17f);

            int rq = Convert.ToInt32(dr["req_qty"].ToString());
            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(rq.ToString(), font9)));
            table.AddCell(cell16);
            PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            table.AddCell(cell16a);
            PdfPCell cell16b = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            table.AddCell(cell16b);

            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            table.AddCell(cell17);

            i++;
            doc.Add(table);

        }

        PdfPTable table5 = new PdfPTable(3);
        PdfPCell cellab = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellab.Border = 1;
        table5.AddCell(cellab);
        PdfPCell cellac = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellac.Border = 1;
        table5.AddCell(cellac);
        PdfPCell cellav = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellav.Border = 1;

        table5.AddCell(cellav);

        PdfPCell cellaq = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaq.Border = 0;
        table5.AddCell(cellaq);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Approved by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);
        PdfPCell cellae = new PdfPCell(new Phrase(new Chunk("Issued by", font9)));
        cellae.Border = 0;
        table5.AddCell(cellae);

        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Requested Item Details"; 
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        conn.Close();
        #endregion
    }
    protected void lnkIssueDetail_Click(object sender, EventArgs e)
    {
        #region ISSUED ITEM DETAILS
        lnkStoreManager.Visible = true;
        LnkDPStockLed.Visible = true;
        lnkPPSL.Visible = true;
        lnkStock.Visible = true;
        lnkKeyStockLedger.Visible = true;


        string OffName, ReqDat1;
        DateTime Cur, Rdate1;
        
        Cur = DateTime.Now;
        string Cur1 = Cur.ToString("yyyy/MM/dd");
        if (txtFromDate.Text != "")
        {
            FD = obje.yearmonthdate(txtFromDate.Text);
        }
        if (txtToDate.Text != "")
        {
            TD = obje.yearmonthdate(txtToDate.Text);
        }

        OdbcCommand LnIss=new OdbcCommand();
        LnIss.CommandType = CommandType.StoredProcedure;
        LnIss.Parameters.AddWithValue("tblname", "t_inventoryrequest q,t_inventoryrequest_items t,m_sub_item i,m_inventory mi,m_sub_itemcategory mc,"
                      +"m_sub_unit mu,t_inventoryrequest_issue iss,t_inventoryrequest_items_issue ist");

        LnIss.Parameters.AddWithValue("attribute", "q.req_officer,q.req_from,q.iss_officer,q.office_request,q.office_issue,t.req_qty,t.issued_qty,"
                  + "(t.req_qty-t.issued_qty) as balance,itemname,itemcode,unitname,mi.itemcode,ist.start_slno,ist.end_slno,date(iss.createdon) as date,q.reqno");


        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            LnIss.Parameters.AddWithValue("conditionv", "q.reqno=t.reqno and mi.unit_id=mu.unit_id and t.item_id=i.item_id and t.item_id=mi.item_id and "
                    +"mi.itemcat_id=mc.itemcat_id and ist.item_id=mi.item_id and iss.reqno=q.reqno and iss.issueno=ist.issueno and (date(iss.createdon) "
                    +"between '"+FD+"' and '"+TD+"')group by iss.createdon,itemcode,issued_qty");
        }
        else if (txtFromDate.Text != "" && txtToDate.Text == "")
        {
            LnIss.Parameters.AddWithValue("conditionv", "q.reqno=t.reqno and mi.unit_id=mu.unit_id and t.item_id=i.item_id and t.item_id=mi.item_id and "
                  +"mi.itemcat_id=mc.itemcat_id and ist.item_id=mi.item_id and iss.reqno=q.reqno and iss.issueno=ist.issueno and (date(iss.createdon) "
                  +"between '"+FD+"' and '"+Cur1+"')group by iss.createdon,itemcode,issued_qty");
        }
        else if (txtFromDate.Text == "" && txtToDate.Text == "")
        {
            LnIss.Parameters.AddWithValue("conditionv", "q.reqno=t.reqno and mi.unit_id=mu.unit_id and t.item_id=i.item_id and t.item_id=mi.item_id and "
                 +"mi.itemcat_id=mc.itemcat_id and ist.item_id=mi.item_id and iss.reqno=q.reqno and iss.issueno=ist.issueno group by iss.createdon,"
                 +"itemcode,issued_qty");
        }

        OdbcDataAdapter IssDat = new OdbcDataAdapter(LnIss);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", LnIss);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = "No Details found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Issued Items Details" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font9 = FontFactory.GetFont("ARIAL", 9);
        Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Material Issue";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        PdfPTable table2 = new PdfPTable(11);
        table2.TotalWidth = 600f;
        table2.LockedWidth = true;
        float[] colwidth1 ={ 2, 5, 4, 4, 4, 4, 4, 4, 3, 3, 5 };
        table2.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase("Stores Issue Note  ", font10));
        cell.Colspan = 11;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table2.AddCell(cell);
        PdfPCell cell5a = new PdfPCell(new Phrase(new Chunk("No", font8)));
        cell5a.Rowspan = 2;
        table2.AddCell(cell5a);

        PdfPCell cell6a = new PdfPCell(new Phrase(new Chunk("Item Name", font8)));
        cell6a.Rowspan = 2;
        cell6a.HorizontalAlignment = 0;
        table2.AddCell(cell6a);

        PdfPCell cell8a = new PdfPCell(new Phrase(new Chunk("SR No", font8)));
        cell8a.Rowspan = 2;
        table2.AddCell(cell8a);

        PdfPCell cell8b = new PdfPCell(new Phrase(new Chunk("UOM", font8)));
        cell8b.Rowspan = 2;
        table2.AddCell(cell8b);

        PdfPCell cell8p = new PdfPCell(new Phrase(new Chunk("Req Office", font8)));
        cell8p.Rowspan = 2;
        table2.AddCell(cell8p);

        PdfPCell cell8y = new PdfPCell(new Phrase(new Chunk("Iss Office", font8)));
        cell8y.Rowspan = 2;
        table2.AddCell(cell8y);

        PdfPCell cell10k = new PdfPCell(new Phrase(new Chunk("Iss Date", font8)));
        cell10k.Rowspan = 2;
        table2.AddCell(cell10k);

        PdfPCell cell9a = new PdfPCell(new Phrase(new Chunk("Quantity", font8)));
        cell9a.Colspan = 3;
        cell9a.HorizontalAlignment = 1;
        table2.AddCell(cell9a);

        PdfPCell cell10a = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
        cell10a.Rowspan = 2;
        table2.AddCell(cell10a);
                
        PdfPCell cell9y = new PdfPCell(new Phrase(new Chunk("Request", font8)));
        table2.AddCell(cell9y);
        PdfPCell cell9t = new PdfPCell(new Phrase(new Chunk("Issued", font8)));
        table2.AddCell(cell9t);
        PdfPCell cell9r = new PdfPCell(new Phrase(new Chunk("Bal", font8)));
        table2.AddCell(cell9r);
        doc.Add(table2);
        
         int slno = 0; int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            slno = slno + 1;
            if (i > 28)
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(11);
                table1.TotalWidth = 600f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 2, 5, 4, 4, 4, 4, 4, 4, 3, 3, 5 };
                table1.SetWidths(colwidth2);
                PdfPCell cell5a1 = new PdfPCell(new Phrase(new Chunk("No", font8)));
                cell5a1.Rowspan = 2;
                table1.AddCell(cell5a1);

                PdfPCell cell6a1 = new PdfPCell(new Phrase(new Chunk("Item Name", font8)));
                cell6a1.Rowspan = 2;
                cell6a1.HorizontalAlignment = 0;
                table1.AddCell(cell6a1);

                PdfPCell cell8a1 = new PdfPCell(new Phrase(new Chunk("Code", font8)));
                cell8a1.Rowspan = 2;
                table1.AddCell(cell8a1);

                PdfPCell cell8b1 = new PdfPCell(new Phrase(new Chunk("UOM", font8)));
                cell8b1.Rowspan = 2;
                table1.AddCell(cell8b1);

                PdfPCell cell10k1 = new PdfPCell(new Phrase(new Chunk("Req Office", font8)));
                cell10k1.Rowspan = 2;
                table1.AddCell(cell10k1);
                PdfPCell cell10r1 = new PdfPCell(new Phrase(new Chunk("Iss Office", font8)));
                cell10r1.Rowspan = 2;
                table1.AddCell(cell10r1);
                PdfPCell cell10u1 = new PdfPCell(new Phrase(new Chunk("Req Date", font8)));
                cell10u1.Rowspan = 2;
                table1.AddCell(cell10u1);


                PdfPCell cell9a1 = new PdfPCell(new Phrase(new Chunk("Quantity", font8)));
                cell9a1.Colspan = 3;
                cell9a1.HorizontalAlignment = 1;
                table1.AddCell(cell9a1);

                PdfPCell cell10a1 = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                cell10a1.Rowspan = 2;
                table1.AddCell(cell10a1);
                

                PdfPCell cell9y1 = new PdfPCell(new Phrase(new Chunk("Request", font8)));
                table1.AddCell(cell9y1);
                PdfPCell cell9t1 = new PdfPCell(new Phrase(new Chunk("Issued", font8)));
                table1.AddCell(cell9t1);
                PdfPCell cell9r1 = new PdfPCell(new Phrase(new Chunk("Bal", font8)));
                table1.AddCell(cell9r1);
                doc.Add(table1);

            }
            PdfPTable table = new PdfPTable(11);
            table.TotalWidth = 600f;
            table.LockedWidth = true;
            float[] colwidth3 ={ 2, 5, 4, 4, 4, 4, 4, 4, 3, 3, 5 };
            table.SetWidths(colwidth3);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font9)));
            table.AddCell(cell11);
            string itn = dr["itemname"].ToString();
            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(itn.ToString(), font9)));
            table.AddCell(cell12);
            string ic = dr["reqno"].ToString();
            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(ic.ToString(), font9)));
            table.AddCell(cell14);
            string un = dr["unitname"].ToString();
            PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(un.ToString(), font9)));
            table.AddCell(cell15);
            conn = obje.NewConnection();
            int ReqFrom = Convert.ToInt32(dr["req_from"].ToString());
            int StoreN = Convert.ToInt32(dr["office_request"].ToString());
            if (ReqFrom == 0)
            {
                OdbcCommand stora4 = new OdbcCommand("SELECT distinct s.storename as Name,store_id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + StoreN + "", conn);
                OdbcDataReader storra4 = stora4.ExecuteReader();
                if (storra4.Read())
                {
                    StorName1 = storra4[0].ToString();

                }
            }
            else if (ReqFrom == 1)
            {
                OdbcCommand stora4 = new OdbcCommand("SELECT distinct s.counter_no as Name,counter_id from m_sub_counter s where s.rowstatus<>'2' and s.counter_id=" + StoreN + "", conn);
                OdbcDataReader storra4 = stora4.ExecuteReader();
                if (storra4.Read())
                {
                    StorName1 = storra4[0].ToString();

                }

            }
            else if (ReqFrom == 2)
            {
                OdbcCommand stora4 = new OdbcCommand("SELECT distinct s.teamname as Name,team_id from m_team s where s.rowstatus<>'2' and s.team_id=" + StoreN + "", conn);
                OdbcDataReader storra4 = stora4.ExecuteReader();
                if (storra4.Read())
                {
                    StorName1 = storra4[0].ToString();

                }
            }
            else
            {

                StorName1 = "";
            }

            PdfPCell cell17a = new PdfPCell(new Phrase(new Chunk(StorName1, font9)));
            table.AddCell(cell17a);
            int OffIssue;
            try
            {
                OffIssue = Convert.ToInt32(dr["office_issue"].ToString());
            }
            catch
            {
                OffIssue = 0;
            }
            OdbcCommand IssuOffice = new OdbcCommand("SELECT distinct s.storename as Name,store_id from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + OffIssue + "", conn);
            OdbcDataReader IssOffi = IssuOffice.ExecuteReader();
            if (IssOffi.Read())
            {
                OffName = IssOffi[0].ToString();

            }
            else
            {
                OffName = "";

            }

            PdfPCell cell17b = new PdfPCell(new Phrase(new Chunk(OffName, font9)));
            table.AddCell(cell17b);

            Rdate1 = DateTime.Parse(dr["date"].ToString());
            ReqDat1 = Rdate1.ToString("dd MMM");
            PdfPCell cell17f = new PdfPCell(new Phrase(new Chunk(ReqDat1, font9)));
            table.AddCell(cell17f);

            int rq = Convert.ToInt32(dr["req_qty"].ToString());
            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(rq.ToString(), font9)));
            table.AddCell(cell16);
            int Iq = Convert.ToInt32(dr["issued_qty"].ToString());
            PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk(Iq.ToString(), font9)));
            table.AddCell(cell16a);
            int Bal = Convert.ToInt32(dr["balance"].ToString());
            PdfPCell cell16b = new PdfPCell(new Phrase(new Chunk(Bal.ToString(), font9)));
            table.AddCell(cell16b);

            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            table.AddCell(cell17);
            int st = Convert.ToInt32(dr["start_slno"].ToString());
            int en = Convert.ToInt32(dr["end_slno"].ToString());
            if (st == 0 && en == 0)
            {
                PdfPCell cell17p = new PdfPCell(new Phrase(new Chunk(" ", font9)));
                table.AddCell(cell17p);
            }

            else
            {
                try
                {

                    PdfPCell cell17p = new PdfPCell(new Phrase(new Chunk("Sl no " + st.ToString() + " - " + en.ToString(), font9)));
                    table.AddCell(cell17p);

                }
                catch
                {
                    PdfPCell cell17p = new PdfPCell(new Phrase(new Chunk(" ", font9)));
                    table.AddCell(cell17p);
                }
            }
            i++;
            doc.Add(table);

        }

        PdfPTable table5 = new PdfPTable(3);
        PdfPCell cellab = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellab.Border = 1;
        table5.AddCell(cellab);
        PdfPCell cellac = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellac.Border = 1;
        table5.AddCell(cellac);
        PdfPCell cellav = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellav.Border = 1;

        table5.AddCell(cellav);												

        PdfPCell cellaq = new PdfPCell(new Phrase(new Chunk("Issued by", font9)));
        cellaq.Border = 0;
        table5.AddCell(cellaq);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Received by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);
        PdfPCell cellae = new PdfPCell(new Phrase(new Chunk("Posted by", font9)));
        cellae.Border = 0;
        table5.AddCell(cellae);

        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Issued Item Details";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        conn.Close();
        #endregion
    }
    protected void cmbReqStore_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnRol_Click(object sender, EventArgs e)
    {
        int k = 0; 
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        if (cmbRStore.SelectedValue == "-1")
        {
            lblOk.Text = "Please Select Item"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string Cur = gh.ToString("dd MMMM yyyy");
        string ch = "Material below ROL" + transtim.ToString() + ".pdf";

        OdbcCommand cseaso = new OdbcCommand("SELECT seasonname FROM m_sub_season ms,m_season m WHERE ms.season_sub_id=m.season_sub_id and curdate() between startdate and enddate", conn);
        OdbcDataReader cserso = cseaso.ExecuteReader();
        if (cserso.Read())
        {
            season = cserso["seasonname"].ToString();
        }


        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table1 = new PdfPTable(9);
        float[] colwidth1 ={ 2, 3, 7, 4, 3, 3,3,3,4 };
        table1.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase("Stock List of items below reorder level ", font10));
        cell.Colspan = 9;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cell);



        PdfPCell cell1e = new PdfPCell(new Phrase(new Chunk("Store Name :  " + cmbRStore.SelectedItem.Text.ToString(), font11)));
        cell1e.Colspan = 5;
        cell1e.Border = 0;
        cell1e.HorizontalAlignment = 0;
        table1.AddCell(cell1e);

        PdfPCell cell1g = new PdfPCell(new Phrase(new Chunk("Season:  " + season.ToString(), font11)));
        cell1g.Colspan = 4;
        cell1g.Border = 0;
        cell1g.HorizontalAlignment = 1;
        table1.AddCell(cell1g);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table1.AddCell(cell1);
        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Code", font9)));
        table1.AddCell(cell2);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name", font9)));
        table1.AddCell(cell3);
        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("UOM", font9)));
        table1.AddCell(cell4);
        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("ROL", font9)));
        table1.AddCell(cell5);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Stock", font9)));
        table1.AddCell(cell6);
        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Class", font9)));
        table1.AddCell(cell7);
        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Cons", font9)));
        table1.AddCell(cell8);
        PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
        table1.AddCell(cell9);
        doc.Add(table1);

        OdbcCommand Roll = new OdbcCommand();
        Roll.CommandType = CommandType.StoredProcedure;
        Roll.Parameters.AddWithValue("tblname", "m_inventory mi,m_sub_item i,m_sub_store s,m_sub_unit u");
        Roll.Parameters.AddWithValue("attribute", "itemname,storename,stock_qty as stock,itemcode,reorderlevel,itemclass,unitname,essentiality");
        Roll.Parameters.AddWithValue("conditionv", "reorderlevel > stock_qty and mi.item_id=i.item_id and mi.rowstatus<>'2' and s.store_id=mi.store_id and mi.rowstatus<>'2' and i.rowstatus<>'2' and u.unit_id=mi.unit_id and u.rowstatus<>2 and mi.store_id=" + cmbRStore.SelectedValue + "");
        OdbcDataAdapter Rolo = new OdbcDataAdapter(Roll);
        DataTable dt2 = new DataTable();
        dt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", Roll);

        #region COMMENTED*******************
        //OdbcCommand Roll = new OdbcCommand("select itemname,storename,stock_qty as stock,itemcode,reorderlevel,itemclass,unitname,essentiality "
        //    + "from m_inventory mi,m_sub_item i,m_sub_store s,m_sub_unit u "
        //    + "where reorderlevel > stock_qty and mi.item_id=i.item_id and mi.rowstatus<>'2' and s.store_id=mi.store_id and mi.rowstatus<>'2' and "
        //    + "i.rowstatus<>'2' and u.unit_id=mi.unit_id and u.rowstatus<>2 and mi.store_id="+cmbRStore.SelectedValue+"", conn);
        //OdbcDataAdapter Rolo = new OdbcDataAdapter(Roll);
        //DataTable dt2 = new DataTable();
        //  Rolo.Fill(dt2);
        #endregion

        if (dt2.Rows.Count == 0)
        {
            lblOk.Text = "No Details found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        int slno = 0;
        for (int ii = 0; ii < dt2.Rows.Count; ii++)
        {

            slno = slno + 1;
            if (k > 45)// total rows on page
            {
                k = 0;
                doc.NewPage();
                PdfPTable table2 = new PdfPTable(9);
                float[] colwidth2 ={ 2, 3, 7, 4, 3, 3, 3, 3, 4 };
                table2.SetWidths(colwidth2);

                PdfPCell cell1q = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table2.AddCell(cell1q);
                PdfPCell cell6q = new PdfPCell(new Phrase(new Chunk("Code", font9)));
                table2.AddCell(cell6q);
                PdfPCell cell2q = new PdfPCell(new Phrase(new Chunk("Name", font9)));
                table2.AddCell(cell2q);
                PdfPCell cell3q = new PdfPCell(new Phrase(new Chunk("UOM", font9)));
                table2.AddCell(cell3q);
                PdfPCell cell4q = new PdfPCell(new Phrase(new Chunk("ROL", font9)));
                table2.AddCell(cell4q);
                PdfPCell cell5q = new PdfPCell(new Phrase(new Chunk("Stock", font9)));
                table2.AddCell(cell5q);
                PdfPCell cell7q = new PdfPCell(new Phrase(new Chunk("Class", font9)));
                table1.AddCell(cell7q);
                PdfPCell cell8q = new PdfPCell(new Phrase(new Chunk("Cons", font9)));
                table1.AddCell(cell8q);
                PdfPCell cell9q = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
                table1.AddCell(cell9q);
                doc.Add(table2);
            }
            PdfPTable table = new PdfPTable(9);
            float[] colwidth3 ={ 2, 3, 7, 4, 3, 3, 3, 3, 4 };
            table.SetWidths(colwidth3);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table.AddCell(cell11);
            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dt2.Rows[ii]["itemcode"].ToString(), font8)));
            table.AddCell(cell12);
            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dt2.Rows[ii]["itemname"].ToString(), font8)));
            table.AddCell(cell13);
            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dt2.Rows[ii]["unitname"].ToString(), font8)));
            table.AddCell(cell14);
            PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dt2.Rows[ii]["reorderlevel"].ToString(), font8)));
            table.AddCell(cell15);
            decimal stock=decimal.Parse(dt2.Rows[ii]["stock"].ToString());
            string ab = stock.ToString();

            if (ab.Contains(".") == true)
            {
                string[] buildS1;
                buildS1 = ab.Split('.');
                stock1 = buildS1[0];

            }
            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(stock1.ToString(), font8)));
            table.AddCell(cell16);
            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dt2.Rows[ii]["itemclass"].ToString(), font8)));
            table.AddCell(cell17);
            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
            table.AddCell(cell18);
            PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(dt2.Rows[ii]["essentiality"].ToString(), font8)));
            table.AddCell(cell19);
            k++;
            doc.Add(table);
        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);

        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Stores superintendent  ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);
        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Material below ROL";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        conn.Close();

    }
    protected void lnkAuthor_Click(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        int Fm_id;
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string Cur = gh.ToString("dd MMMM yyyy");
        string ch = "AuthorizedUserListforManagingInventory" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table1 = new PdfPTable(5);
        float[] colwidth1 ={ 2,5, 7, 7, 5 };
        table1.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase("Authorized User's List to Manage Inventory ", font10));
        cell.Colspan = 5;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cell);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table1.AddCell(cell1);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name", font9)));
        table1.AddCell(cell3);
        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Process Name", font9)));
        table1.AddCell(cell4);
        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Department", font9)));
        table1.AddCell(cell5);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Designation", font9)));
        table1.AddCell(cell6);
        doc.Add(table1);
        int slno = 0;

        OdbcCommand Author = new OdbcCommand();
        Author.CommandType = CommandType.StoredProcedure;
        Author.Parameters.AddWithValue("tblname", "m_sub_form");
        Author.Parameters.AddWithValue("attribute", "form_id,displayname");
        Author.Parameters.AddWithValue("conditionv", "formname='Room Inventory Management' and status<>'2'");

        //OdbcCommand Author = new OdbcCommand("SELECT form_id,displayname from m_sub_form where formname='Room Inventory Management'",conn);
        OdbcDataAdapter Aut = new OdbcDataAdapter(Author);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", Author);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = "No Details found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;

        }
        for (int ii = 0; ii < dt.Rows.Count; ii++)
        {
            Fm_id = Convert.ToInt32(dt.Rows[ii]["form_id"].ToString());

            if (k5 > 45)// total rows on page
            {
                k5 = 0;
                doc.NewPage();
                PdfPTable table2 = new PdfPTable(5);
                float[] colwidth2 ={ 2, 5, 7, 7, 5 };
                table2.SetWidths(colwidth2);

                PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table1.AddCell(cell1a);
                PdfPCell cell3a = new PdfPCell(new Phrase(new Chunk("Name", font9)));
                table1.AddCell(cell3a);
                PdfPCell cell4a = new PdfPCell(new Phrase(new Chunk("Process Name", font9)));
                table1.AddCell(cell4a);
                PdfPCell cell5a = new PdfPCell(new Phrase(new Chunk("Department", font9)));
                table1.AddCell(cell5a);
                PdfPCell cell6a = new PdfPCell(new Phrase(new Chunk("Designation", font9)));
                table1.AddCell(cell6a);
                doc.Add(table2);
            }
            PdfPTable table = new PdfPTable(5);
            float[] colwidth3 ={ 2, 5, 7, 7, 5 };
            table.SetWidths(colwidth3);

            OdbcCommand form = new OdbcCommand();
            form.CommandType = CommandType.StoredProcedure;
            form.Parameters.AddWithValue("tblname", "m_user u,m_sub_designation d,m_sub_department e,m_staff s");
            form.Parameters.AddWithValue("attribute", "username,staffname,deptname,designation");
            form.Parameters.AddWithValue("conditionv", "level=(SELECT prev_level from m_userprev_formset where form_id=" + Fm_id + ") and u.staff_id=s.staff_id and s.desig_id=d.desig_id and s.dept_id=e.dept_id");

            #region COMMENTED*******************
            //OdbcCommand form = new OdbcCommand("SELECT username,staffname,deptname,designation "
            //         +"from  "
            //                  +"m_user u,m_sub_designation d,m_sub_department e,m_staff s "
            //         +"where "
            //                  +"level=(SELECT prev_level from m_userprev_formset where form_id="+Fm_id+") and u.staff_id=s.staff_id and s.desig_id=d.desig_id "
            //                  +"and s.dept_id=e.dept_id",conn);
            #endregion

            OdbcDataAdapter formr = new OdbcDataAdapter(form);
            DataTable dtt = new DataTable();
            dtt = obje.SpDtTbl("CALL selectcond(?,?,?)", form);
            foreach (DataRow dr in dtt.Rows)
            {
                slno = slno + 1;
                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table.AddCell(cell11);
                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["staffname"].ToString(), font8)));
                table.AddCell(cell12);
                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dt.Rows[ii]["displayname"].ToString(), font8)));
                table.AddCell(cell13);
                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["deptname"].ToString(), font8)));
                table.AddCell(cell14);
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["designation"].ToString(), font8)));
                table.AddCell(cell15);
                k5++;
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
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Stores superintendent  ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);
        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Authorized Users List to Manage Inventory ";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        conn.Close();
        

    }
    protected void lnkKeyStockLedger_Click(object sender, EventArgs e)
    {
        btnStock.Visible = false;
        cmbStockRegistry.Visible = false;
        cmbStockItem.Visible = false;
        lblStoreName.Visible = false;
        lblItName.Visible = false;
        lblStaff.Visible = false;
        cmbStaff.Visible = false;
        btnLed.Visible = false;
        lblStore1.Visible = false;
        cmbStore1.Visible = false;
        lblRStore.Visible = false;
        cmbRStore.Visible = false;
        btnRol.Visible = false;
        lnkAuthor.Visible = false;
        lblKStore.Visible = true;
        lblBuilding.Visible = true;
        lblRoomNO.Visible = true;
        cmbKBuilding.Visible = true;
        cmbKStore.Visible = true;
        cmbKRoom.Visible = true;
        btnKeyStockLed.Visible = true;
    }
    protected void cmbKBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        OdbcCommand RoomKey1 = new OdbcCommand();
        RoomKey1.CommandType = CommandType.StoredProcedure;
        RoomKey1.Parameters.AddWithValue("tblname", "m_sub_building b,m_room r");
        RoomKey1.Parameters.AddWithValue("attribute", "roomno,room_id");
        RoomKey1.Parameters.AddWithValue("conditionv", ".build_id=b.build_id and r.build_id=" + cmbKBuilding.SelectedValue + " and r.room_id in "
               +" (SELECT room_id from t_roomvacate v,t_roomallocation a where return_key='0' and a.alloc_id=v.alloc_id and season_id=(select season_id "
               +" from m_season where curdate() between startdate and enddate))");

        OdbcDataAdapter RoomKey1r = new OdbcDataAdapter(RoomKey1);
        DataTable ds3 = new DataTable();
        ds3 = obje.SpDtTbl("CALL selectcond(?,?,?)", RoomKey1);
        DataRow row5 = ds3.NewRow();
        row5["room_id"] = "-1";
        row5["roomno"] = "--Select--";
        ds3.Rows.InsertAt(row5, 0);
        cmbKRoom.DataSource = ds3;
        cmbKRoom.DataBind();
        conn.Close();

        #region COMMENTED**************
        //OdbcDataAdapter RoomKey1 = new OdbcDataAdapter("SELECT roomno,room_id from m_sub_building b,m_room r where r.build_id=b.build_id and r.build_id="+cmbKBuilding.SelectedValue+" "
        //    +"and r.room_id in (SELECT room_id from t_roomvacate v,t_roomallocation a where return_key='0' and a.alloc_id=v.alloc_id and "
        //    +"season_id=(select season_id from m_season where curdate() between startdate and enddate))", conn);
        //DataTable ds3 = new DataTable();
        //DataColumn colID = ds3.Columns.Add("room_id", System.Type.GetType("System.Int32"));
        //DataColumn colNo = ds3.Columns.Add("roomno", System.Type.GetType("System.String"));
        //DataRow row5 = ds3.NewRow();
        //row5["room_id"] = "-1";
        //row5["roomno"] = "--Select--";
        //ds3.Rows.InsertAt(row5, 0);
        //RoomKey1.Fill(ds3);
               
        //cmbKRoom.DataSource = ds3;
        //cmbKRoom.DataBind();
        //conn.Close();
        #endregion

    }
    protected void btnKeyStockLed_Click(object sender, EventArgs e)
    {
        #region KEY STOCK LEDGER
        int amount1;
        string building;
        if (cmbKBuilding.SelectedValue == "-1")
        {
            lblOk.Text = "Please Select a Building"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        if (cmbKRoom.SelectedValue == "-1")
        {
            lblOk.Text = "Please Select a Room"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        if (cmbKStore.SelectedValue == "-1")
        {
            lblOk.Text = "Please Select a store"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        DateTime ds2 = DateTime.Now;
        string transtim = ds2.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Room Keys Stock Ledger" + transtim.ToString() + ".pdf";

        OdbcCommand KeyItem = new OdbcCommand();
        KeyItem.CommandType = CommandType.StoredProcedure;
        KeyItem.Parameters.AddWithValue("tblname", "m_sub_item i,m_inventory inv");
        KeyItem.Parameters.AddWithValue("attribute", "i.item_id,itemcode");
        KeyItem.Parameters.AddWithValue("conditionv", "itemname='Key' and i.rowstatus<>'2' and i.item_id=inv.item_id and inv.rowstatus<>'2'");
        OdbcDataAdapter KeyItemr = new OdbcDataAdapter(KeyItem);
        DataTable ds3 = new DataTable();
        ds3 = obje.SpDtTbl("CALL selectcond(?,?,?)", KeyItem);
        if (ds3.Rows.Count > 0)
        {
            for (int k = 0; k < ds3.Rows.Count; k++)
            {
                Key_id = Convert.ToInt32(ds3.Rows[k]["item_id"].ToString());
                Key_code = ds3.Rows[k]["itemcode"].ToString();
            }
        }

        #region COMMENTED*************
        //OdbcCommand KeyItem = new OdbcCommand("SELECT i.item_id,itemcode from m_sub_item i,m_inventory inv where itemname='Key' and i.rowstatus<>'2' and i.item_id=inv.item_id and inv.rowstatus<>'2'", conn);
        //OdbcDataReader KeyItemr = KeyItem.ExecuteReader();
        //if (KeyItemr.Read())
        //{
        //    Key_id = Convert.ToInt32(KeyItemr["item_id"].ToString());
        //    Key_code = KeyItemr["itemcode"].ToString();
        //}
        #endregion

        string datte = ds2.ToString("dd-MM-yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        string timme = ds2.ToShortTimeString();
        string datte1 = ds2.ToString("dd MMMM yyyy");
        string dat4 = ds2.ToString("dd-MM-yyyy");
        OdbcCommand RoomKe = new OdbcCommand("DROP VIEW if exists tempKeyStockLedger", conn);
        RoomKe.ExecuteNonQuery();
        OdbcCommand KeyLedg = new OdbcCommand("CREATE VIEW tempKeyStockLedger as select iss.issueno,t.reqno,itemcode,openingstock,iss.issued_qty,received_qty,iss.item_id,office_request,"
             + "office_issue,inv.createdon as opend,ri.createdon as isdate,room_id from m_inventory inv,t_inventoryrequest_items item,t_inventoryrequest_issue ri,"
             + "t_inventoryrequest_items_issue iss,t_roomvacate v,t_roomallocation a,t_inventoryrequest t left join m_sub_store s on (s.store_id=office_issue or s.store_id=office_request ) "
             + "where t.reqno=item.reqno and (iss.item_id=" + Key_id + " or item.item_id=" + Key_id + ") and inv.item_id=item.item_id and ri.issueno=iss.issueno "
             + "and iss.item_id=item.item_id and iss.item_id=inv.item_id and ri.reqno=t.reqno and office_request=" + cmbKStore.SelectedValue + "  and v.createdon=t.date_request and v.alloc_id=a.alloc_id group by t.reqno order by isdate asc", conn);

        KeyLedg.ExecuteNonQuery();
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 60, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Stock Ledger";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        PdfPTable table1 = new PdfPTable(8);
        float[] colwidth1 ={ 2, 3, 5, 3, 3, 3, 3, 1 };
        table1.SetWidths(colwidth1);
        table1.TotalWidth = 650f;

        building = cmbKBuilding.SelectedItem.Text.ToString();
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
        OdbcCommand RmId = new OdbcCommand("SELECT room_id from m_room where build_id=" + cmbKBuilding.SelectedValue + " and roomno=" + cmbKRoom.SelectedItem.Text.ToString() + " and rowstatus<>'2'", conn);
        OdbcDataReader RmIdr = RmId.ExecuteReader();
        if (RmIdr.Read())
        {
            Rm = Convert.ToInt32(RmIdr[0].ToString());
        }


        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Room key’s stock ledger ", font10)));
        cell.Colspan = 8;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table1.AddCell(cell);

        try
        {
            PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Store name: " +cmbKStore.SelectedItem.Text.ToString(), font11)));
            cella.Colspan = 4;
            cella.Border = 0;
            cella.HorizontalAlignment = 0;
            table1.AddCell(cella);
            PdfPCell cellb = new PdfPCell(new Phrase(new Chunk("Item Name:  Room Keys with keychain ", font11)));
            cellb.Colspan = 4;
            cellb.Border = 0;
            cellb.HorizontalAlignment = 0;
            table1.AddCell(cellb);

            PdfPCell cellc = new PdfPCell(new Phrase(new Chunk("Item Code: " + Key_code.ToString(), font11)));
            cellc.Colspan = 4;
            cellc.Border = 0;
            cellc.HorizontalAlignment = 0;
            table1.AddCell(cellc);
            PdfPCell celld = new PdfPCell(new Phrase(new Chunk("Room No:  "+building.ToString()+ " / "+cmbKRoom.SelectedItem.Text.ToString(), font11)));
            celld.Colspan = 4;
            celld.Border = 0;
            celld.HorizontalAlignment = 0;
            table1.AddCell(celld);
        }
        catch
        { }
        doc.Add(table1);

        PdfPTable table2 = new PdfPTable(8);
        float[] colwidth5 ={ 2, 3, 5, 3, 3, 3, 3, 1 };
        table2.SetWidths(colwidth5);
        table2.TotalWidth = 650f;

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        cell1.Rowspan = 2;
        table2.AddCell(cell1);

        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        cell2.Rowspan = 2;
        table2.AddCell(cell2);

        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Description", font9)));
        cell3.Rowspan = 2;
        table2.AddCell(cell3);

        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Stock item status", font9)));
        cell4.Colspan = 3;
        cell4.HorizontalAlignment = 1;
        table2.AddCell(cell4);

        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell5.Rowspan = 2;
        cell5.Colspan = 2;
        table2.AddCell(cell5);

        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Recv", font9)));
        table2.AddCell(cell6);
        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Isue", font9)));
        table2.AddCell(cell7);
        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Bal", font9)));
        table2.AddCell(cell8);
        doc.Add(table2);

        OdbcCommand KeyL = new OdbcCommand();
        KeyL.CommandType = CommandType.StoredProcedure;
        KeyL.Parameters.AddWithValue("tblname", "tempKeyStockLedger");
        KeyL.Parameters.AddWithValue("attribute", "*");
        KeyL.Parameters.AddWithValue("conditionv", "room_id=" + cmbKRoom.SelectedValue + "");
        OdbcDataAdapter KeyLa = new OdbcDataAdapter(KeyL);

        //OdbcCommand KeyL = new OdbcCommand("SELECT * FROM tempKeyStockLedger where room_id=" + cmbKRoom.SelectedValue + "", conn);
        
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", KeyL);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = "No Details found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

            int slno = 0;
            PdfPTable table = new PdfPTable(8);
            float[] colwidth4 ={ 2, 3, 5, 3, 3, 3, 3, 1 };
            table.SetWidths(colwidth4);
            table.TotalWidth = 650f;
           foreach (DataRow dr in dt.Rows)
          {

            if (slno == 0)
            {
                slno = slno + 1;

                OdbcCommand Open = new OdbcCommand();
                Open.CommandType = CommandType.StoredProcedure;
                Open.Parameters.AddWithValue("tblname", "t_roomresource r,t_roomresource_items i");
                Open.Parameters.AddWithValue("attribute", "quantity,date(r.createdon) as Opendate");
                Open.Parameters.AddWithValue("conditionv", "item_id=" + Key_id + " and room_id=" + Rm + " and r.resource_id=i.resource_id");
                OdbcDataAdapter Opena = new OdbcDataAdapter(Open);
                DataTable dp = new DataTable();
                dp = obje.SpDtTbl("CALL selectcond(?,?,?)", Open);

                //OdbcCommand Open = new OdbcCommand("select quantity,date(r.createdon) as Opendate from t_roomresource r,t_roomresource_items i where "
                //     +"item_id="+Key_id+" and room_id="+Rm+" and r.resource_id=i.resource_id",conn);
                //OdbcDataReader Openr = Open.ExecuteReader();
                if (dp.Rows.Count>0)
                {
                    for (int p = 0; p < dp.Rows.Count; p++)
                    {
                        PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                        table.AddCell(cell21b);
                        DateTime Date = DateTime.Parse(dp.Rows[p]["Opendate"].ToString());
                        string Ddate = Date.ToString("dd MMM yyyy");
                        PdfPCell cell21c = new PdfPCell(new Phrase(new Chunk(Ddate.ToString(), font8)));
                        table.AddCell(cell21c);
                        PdfPCell cell21e = new PdfPCell(new Phrase(new Chunk("Opening Stock", font8)));
                        table.AddCell(cell21e);
                        int OpenStock1 = Convert.ToInt32(dp.Rows[p]["quantity"].ToString());
                        amount1 = OpenStock1;
                        Session["op"] = OpenStock1;
                        Session["Rec"] = OpenStock1;
                        PdfPCell cell21d = new PdfPCell(new Phrase(new Chunk(OpenStock1.ToString(), font8)));
                        table.AddCell(cell21d);
                        PdfPCell cell21f = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                        table.AddCell(cell21f);
                        PdfPCell cell21g = new PdfPCell(new Phrase(new Chunk(OpenStock1.ToString(), font8)));
                        table.AddCell(cell21g);
                        PdfPCell cell21o = new PdfPCell(new Phrase(new Chunk(building.ToString() + " / " + cmbKRoom.SelectedItem.Text.ToString(), font8)));
                        cell21o.Colspan = 2;
                        table.AddCell(cell21o);
                    }
                }
            }

        }

        foreach (DataRow dr in dt.Rows)
        {

           int it_id = Convert.ToInt32(dr["item_id"].ToString());
           try
           {
              int IssQty1 = Convert.ToInt32(dr["issued_qty"].ToString());
              if (IssQty1 != 0)
               {
                   slno = slno + 1;
                   PdfPCell cell33h = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                   table.AddCell(cell33h);
                   DateTime Date2 = DateTime.Parse(dr["isdate"].ToString());
                   string Ddate2 = Date2.ToString("dd MMM yyyy");
                   PdfPCell cell33i = new PdfPCell(new Phrase(new Chunk(Ddate2.ToString(), font8)));
                   table.AddCell(cell33i);
                   string Name = dr["reqno"].ToString();
                   PdfPCell cell33j = new PdfPCell(new Phrase(new Chunk("Issued for key replacement ", font8)));
                   table.AddCell(cell33j);
                   PdfPCell cell33k = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                   table.AddCell(cell33k);
                   int Issqt = Convert.ToInt32(dr["issued_qty"].ToString());
                   PdfPCell cell33l = new PdfPCell(new Phrase(new Chunk(Issqt.ToString(), font8)));
                   table.AddCell(cell33l);
                   amount1 = int.Parse(Session["Rec"].ToString());
                   int Bal = amount1 - Issqt;
                   Session["Rec"] = Bal;
                   PdfPCell cell33m = new PdfPCell(new Phrase(new Chunk(Bal.ToString(), font8)));
                   table.AddCell(cell33m);
                   PdfPCell cell34m = new PdfPCell(new Phrase(new Chunk(Name.ToString(), font8)));
                   cell34m.Colspan = 2;
                   table.AddCell(cell34m);
               }

           }
           catch
           { }
           try
           {
               
               string Ddate2;
               int RecQty1 = Convert.ToInt32(dr["received_qty"].ToString());

               if (RecQty1 != 0)
               {
                   slno = slno + 1;
                   
                   if (conn.State == ConnectionState.Closed)
                   {
                       conn.ConnectionString = strConnection;
                       conn.Open();
                   }

                   string StReq = dr["issueno"].ToString();

                   OdbcCommand Recp1 = new OdbcCommand();
                   Recp1.CommandType = CommandType.StoredProcedure;
                   Recp1.Parameters.AddWithValue("tblname", "t_grn g,t_grn_items gi,t_inventoryrequest_items_issue iss");
                   Recp1.Parameters.AddWithValue("attribute", "distinct receive_qty,g.receivedon as rdate,g.grnno");
                   Recp1.Parameters.AddWithValue("conditionv", "gi.grnno=g.grnno and g.refno='" + StReq.ToString() + "' and gi.item_id=" + Key_id + " and iss.issueno=g.refno");
                   OdbcDataAdapter Recp1a = new OdbcDataAdapter(Recp1);
                   DataTable dy = new DataTable();
                   dy = obje.SpDtTbl("CALL selectcond(?,?,?)", Recp1);

                   #region COMMENTED**************
                   //OdbcCommand Recp1 = new OdbcCommand("SELECT distinct receive_qty,g.receivedon as rdate,g.grnno from t_grn g,t_grn_items gi,t_inventoryrequest_items_issue iss where "
                   //               + "gi.grnno=g.grnno and g.refno='" + StReq.ToString() + "' and gi.item_id=" + Key_id + " and iss.issueno=g.refno", conn);

                   //OdbcDataReader Recr1 = Recp1.ExecuteReader();
                   #endregion

                   for (int r1=0;r1<dy.Rows.Count;r1++)
                   {
                       DateTime Date1 = DateTime.Parse(dy.Rows[r1]["rdate"].ToString());
                       Ddate2 = Date1.ToString("dd MMM yyyy");
                       Rqt5 = Convert.ToInt32(dy.Rows[r1]["receive_qty"].ToString());
                       GName1 = dy.Rows[r1]["grnno"].ToString();
                      
                   }

                   PdfPCell cell33a = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                   table.AddCell(cell33a);

                   PdfPCell cell33b = new PdfPCell(new Phrase(new Chunk(Ddate1.ToString(), font8)));
                   table.AddCell(cell33b);
                   
                   PdfPCell cell33c = new PdfPCell(new Phrase(new Chunk("Received from key duplication ", font8)));
                   table.AddCell(cell33c);

                   PdfPCell cell33d = new PdfPCell(new Phrase(new Chunk(Rqt2.ToString(), font8)));
                   table.AddCell(cell33d);
                   PdfPCell cell33e = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                   table.AddCell(cell33e);
                   int amount9 = Rqt5;
                   int OpenSt5 = int.Parse(Session["Rec"].ToString());
                   amount1 = amount9 + OpenSt5;
                   Session["Rec"] = amount1;
                   PdfPCell cell33f = new PdfPCell(new Phrase(new Chunk(amount1.ToString(), font8)));
                   table.AddCell(cell33f);
                   PdfPCell cell34f = new PdfPCell(new Phrase(new Chunk(GName1.ToString(), font8)));
                   cell34f.Colspan = 2;
                   table.AddCell(cell34f);

               }
           }
           catch { }
        }

        PdfPTable table5 = new PdfPTable(3);
        PdfPCell cellab = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellab.Border = 1;
        table5.AddCell(cellab);
        PdfPCell cellac = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellac.Border = 1;
        table5.AddCell(cellac);
        PdfPCell cellav = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellav.Border = 1;

        table5.AddCell(cellav);

        PdfPCell cellaq = new PdfPCell(new Phrase(new Chunk("Prepared by", font8)));
        cellaq.Border = 0;
        table5.AddCell(cellaq);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("", font8)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);
        PdfPCell cellae = new PdfPCell(new Phrase(new Chunk("", font8)));
        cellae.Border = 0;
        table5.AddCell(cellae);
        PdfPCell cellaj = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font8)));
        cellaj.Border = 0;
        table5.AddCell(cellaj);

        PdfPCell cellawi = new PdfPCell(new Phrase(new Chunk("", font8)));
        cellawi.Border = 0;
        table5.AddCell(cellawi);
        PdfPCell cellaei = new PdfPCell(new Phrase(new Chunk("", font8)));
        cellaei.Border = 0;
        table5.AddCell(cellaei);

        PdfPCell cellak = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
        cellak.Border = 0;
        table5.AddCell(cellak);

        PdfPCell cellawk = new PdfPCell(new Phrase(new Chunk("", font8)));
        cellawk.Border = 0;
        table5.AddCell(cellawk);
        PdfPCell cellaek = new PdfPCell(new Phrase(new Chunk("", font8)));
        cellaek.Border = 0;
        table5.AddCell(cellaek);

        doc.Add(table);
        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Keys Stock Ledger report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        conn.Close();

        #endregion

        
    }
    protected void btnPass_Click(object sender, EventArgs e)
    {
        
    }
    protected void cmbPStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        OdbcDataAdapter StockPass = new OdbcDataAdapter("select itemname,inv.item_id from m_sub_item it,m_inventory inv where inv.store_id=(select store_id as "
            +"id from m_sub_store where storename='"+cmbPStore.SelectedItem.Text.ToString()+"' and rowstatus<>'2' UNION select counter_id as id from "
            +"m_sub_counter where counter_no='"+cmbPStore.SelectedItem.Text.ToString()+"' and rowstatus<>'2') and inv.item_id=it.item_id and inv.itemcat_id=(select itemcat_id from "
            +"m_sub_itemcategory cat where itemcatname='Pass' and rowstatus<>'2')", conn);
        DataTable ds1 = new DataTable();
        DataRow row = ds1.NewRow();
        StockPass.Fill(ds1);
        row["item_id"] = "-1";
        row["itemname"] = "--Select--";
        ds1.Rows.InsertAt(row, 0);
        cmbPItem.DataSource = ds1;
        cmbPItem.DataBind();
        conn.Close();
    }

    protected void txtRequestNo_TextChanged(object sender, EventArgs e)
    {

    }

    protected void LnkDPStockLed_Click(object sender, EventArgs e)
    {
        #region Donor Free Pass Stock Ledger
        try
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
            float bal = 0;
            DateTime curdate = DateTime.Now;
            string currenttime = curdate.ToString("yyyy/MM/dd") + ' ' + curdate.ToString("hh:mm:ss");

            OdbcCommand da456 = new OdbcCommand();
            da456.CommandType = CommandType.StoredProcedure;
            da456.Parameters.AddWithValue("tblname", "m_inventory ");
            da456.Parameters.AddWithValue("attribute", "itemcode,stock_qty,openingstock,updateddate,item_id");
            da456.Parameters.AddWithValue("conditionv", "item_id =(Select item_id from m_sub_item where itemname like '%free% %pass% %Original%')");
            OdbcDataAdapter da456a = new OdbcDataAdapter(da456);
            DataTable dt456 = new DataTable();
            dt456 = obje.SpDtTbl("CALL selectcond(?,?,?)", da456);

            if (dt456.Rows.Count == 0)
            {
                lblOk.Text = "No Details found"; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;
            }
            string itemcode = dt456.Rows[0]["itemcode"].ToString();
            float balStock = float.Parse(dt456.Rows[0]["stock_qty"].ToString());
            float opengstk = float.Parse(dt456.Rows[0]["openingstock"].ToString());
            DateTime opendate = DateTime.Parse(dt456.Rows[0]["updateddate"].ToString());
            int item_idfp = int.Parse(dt456.Rows[0]["item_id"].ToString());

            #region PDF Name Heading and Format
            string report = "Donor free pass StockLedger TakenOn " + curdate.ToString("dd-MM-yyyy") + ' ' + curdate.ToString("HH-mm-ss") + ".pdf";
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report + "";

            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font12 = FontFactory.GetFont("ARIAL", 12, 1);          
            Font font9 = FontFactory.GetFont("ARIAL", 9,1);

            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

            PdfPTable table = new PdfPTable(5);
            float[] colWidths23av6 = { 5, 15, 10, 10, 10 };
            table.SetWidths(colWidths23av6);
            table.TotalWidth = 400f;

            PdfPCell cellq = new PdfPCell(new Phrase(new Chunk("Free Pass Stock Ledger", font12)));
            cellq.Colspan = 5;
            cellq.Border = 1;
            cellq.HorizontalAlignment = 1;
            table.AddCell(cellq);
            doc.Add(table);

            PdfPTable table4 = new PdfPTable(4);
            float[] colWidths4 = { 12, 13, 12, 13 };
            table4.SetWidths(colWidths4);
            table4.TotalWidth = 400f;
            PdfPCell cell1aa = new PdfPCell(new Phrase(new Chunk("Store name: Accommodation office", font10)));
            cell1aa.Colspan = 2;
            cell1aa.Border = 0;
            table4.AddCell(cell1aa);

            PdfPCell cell1f23 = new PdfPCell(new Phrase(new Chunk("Item Name: Donor free pass ", font10)));
            cell1f23.Colspan = 2;
            cell1f23.Border = 0;
            cell1f23.HorizontalAlignment = 2;
            table4.AddCell(cell1f23);

            PdfPCell cell1aaq = new PdfPCell(new Phrase(new Chunk("Item code: " + itemcode + "", font10)));
            cell1aaq.Colspan = 2;
            cell1aaq.Border = 0;
            table4.AddCell(cell1aaq);

            //PdfPCell cell1f23w = new PdfPCell(new Phrase(new Chunk("Balance Stock: " + balStock + " ", font10)));
            PdfPCell cell1f23w = new PdfPCell(new Phrase(new Chunk("Balance Stock: " , font10)));
            cell1f23w.Colspan = 2;
            cell1f23w.Border = 0;
            cell1f23w.HorizontalAlignment = 1;
            table4.AddCell(cell1f23w);

            PdfPCell cell1f23w1 = new PdfPCell(new Phrase(new Chunk("", font10)));
            cell1f23w1.Colspan = 4;
            cell1f23w1.Border = 0;
            table4.AddCell(cell1f23w1);
            doc.Add(table4);
            #endregion

            #region Gen page format
            PdfPTable table9 = new PdfPTable(7);
            float[] colWidths23av68 = { 3, 6, 11, 4, 4, 4, 8 };
            table9.SetWidths(colWidths23av68);
            table9.TotalWidth = 400f;
            PdfPCell cell1wf = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            cell1wf.Rowspan = 2;
            table9.AddCell(cell1wf);
            PdfPCell cell1f = new PdfPCell(new Phrase(new Chunk("Date", font9)));
            cell1f.Rowspan = 2;
            table9.AddCell(cell1f);
            PdfPCell cell2f = new PdfPCell(new Phrase(new Chunk("Description", font9)));
            cell2f.Rowspan = 2;
            table9.AddCell(cell2f);
            PdfPCell cell2x = new PdfPCell(new Phrase(new Chunk("Stock item status", font9)));
            cell2x.HorizontalAlignment = 1;
            cell2x.Colspan = 3;
            table9.AddCell(cell2x);
            PdfPCell cell3f = new PdfPCell(new Phrase(new Chunk("Serial No", font9)));
            cell3f.Rowspan = 2;
            table9.AddCell(cell3f);

            PdfPCell cell3f2 = new PdfPCell(new Phrase(new Chunk("Recv", font9)));
            cell3f2.HorizontalAlignment = 1;
            table9.AddCell(cell3f2);
            PdfPCell cell3f3 = new PdfPCell(new Phrase(new Chunk("Isue", font9)));
            cell3f3.HorizontalAlignment = 1;
            table9.AddCell(cell3f3);
            PdfPCell cell3f46 = new PdfPCell(new Phrase(new Chunk("Bal", font9)));
            cell3f46.HorizontalAlignment = 1;
            table9.AddCell(cell3f46);
            doc.Add(table9);
            #endregion

            int i = 0;
            slno = slno + 1;

            #region Next Page
            if (i > 24)
            {
                i = 1;

                PdfPTable table91 = new PdfPTable(7);
                float[] colWidths23av681 = { 3, 6, 11, 4, 4, 4, 8 };
                table91.SetWidths(colWidths23av68);
                table91.TotalWidth = 400f;
                PdfPCell cell1wf0 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                cell1wf0.Rowspan = 2;
                table91.AddCell(cell1wf0);
                PdfPCell cell1fb = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                cell1fb.Rowspan = 2;
                table91.AddCell(cell1fb);
                PdfPCell cell2fl = new PdfPCell(new Phrase(new Chunk("Description", font9)));
                cell2fl.Rowspan = 2;
                table91.AddCell(cell2fl);
                PdfPCell cell2xu = new PdfPCell(new Phrase(new Chunk("Stock item status", font9)));
                cell2xu.HorizontalAlignment = 1;
                cell2xu.Colspan = 3;
                table91.AddCell(cell2xu);
                PdfPCell cell3fyy = new PdfPCell(new Phrase(new Chunk("Serial No", font9)));
                cell3fyy.Rowspan = 2;
                table91.AddCell(cell3fyy);

                PdfPCell cell3f2vv = new PdfPCell(new Phrase(new Chunk("Recv", font9)));
                cell3f2vv.HorizontalAlignment = 1;
                table91.AddCell(cell3f2vv);
                PdfPCell cell3f3dd = new PdfPCell(new Phrase(new Chunk("Isue", font9)));
                cell3f3dd.HorizontalAlignment = 1;
                table91.AddCell(cell3f3dd);
                PdfPCell cell3f46ee = new PdfPCell(new Phrase(new Chunk("Bal", font9)));
                cell3f46ee.HorizontalAlignment = 1;
                table91.AddCell(cell3f46ee);
                doc.Add(table91);
            }
            #endregion

            #region Opening Stock
            PdfPTable table3 = new PdfPTable(7);
            float[] colWidths23av11 = { 3, 6, 11, 4, 4, 4, 8 };
            table3.SetWidths(colWidths23av11);
            table3.TotalWidth = 400f;

            foreach (DataRow dr in dt456.Rows)
            {
                PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table3.AddCell(cell41);

                PdfPCell cell4w2 = new PdfPCell(new Phrase(new Chunk(opendate.ToString("dd MMM yyyy"), font8)));
                table3.AddCell(cell4w2);

                PdfPCell cell53 = new PdfPCell(new Phrase(new Chunk("Opening stock", font8)));
                table3.AddCell(cell53);

                PdfPCell cell5n4 = new PdfPCell(new Phrase(new Chunk(opengstk.ToString(), font8)));
                cell5n4.Colspan = 1;
                cell5n4.HorizontalAlignment = 1;
                table3.AddCell(cell5n4);

                PdfPCell cell5n15 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell5n15.Colspan = 1;
                cell5n15.HorizontalAlignment = 1;
                table3.AddCell(cell5n15);

                PdfPCell cell5n26 = new PdfPCell(new Phrase(new Chunk(opengstk.ToString(), font8)));
                cell5n26.Colspan = 1;
                cell5n26.HorizontalAlignment = 1;
                table3.AddCell(cell5n26);

                PdfPCell cell67 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table3.AddCell(cell67);

                bal = bal + float.Parse(dt456.Rows[0]["openingstock"].ToString());
            }
            #endregion

            #region Pass Reception
            OdbcCommand da4567 = new OdbcCommand();
            da4567.CommandType = CommandType.StoredProcedure;
            da4567.Parameters.AddWithValue("tblname", "t_grn_items,t_grn,t_inventoryrequest_items_issue ");
            da4567.Parameters.AddWithValue("attribute", "receivedon,receive_qty,start_slno,end_slno");
            da4567.Parameters.AddWithValue("conditionv", "t_grn_items.grnno=t_grn.grnno and t_inventoryrequest_items_issue.item_id=t_grn_items.item_id and issueno=refno and t_grn_items.item_id=" + item_idfp + " order by receivedon");
            OdbcDataAdapter da4567a = new OdbcDataAdapter(da4567);
            DataTable dt4567 = new DataTable();
            dt4567 = obje.SpDtTbl("CALL selectcond(?,?,?)", da4567);
            
            foreach (DataRow dr1 in dt4567.Rows)
            {
                bal = bal + int.Parse(dr1["receive_qty"].ToString());
                slno = slno + 1;

                DateTime dt5 = DateTime.Parse(dr1["receivedon"].ToString());
                PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table3.AddCell(cell41);

                PdfPCell cell4w2 = new PdfPCell(new Phrase(new Chunk(dt5.ToString("dd MMM yyyy"), font8)));
                table3.AddCell(cell4w2);

                PdfPCell cell53 = new PdfPCell(new Phrase(new Chunk("Received from press", font8)));
                table3.AddCell(cell53);

                PdfPCell cell5n4 = new PdfPCell(new Phrase(new Chunk(dr1["receive_qty"].ToString(), font8)));
                cell5n4.Colspan = 1;
                cell5n4.HorizontalAlignment = 1;
                table3.AddCell(cell5n4);

                PdfPCell cell5n15 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell5n15.Colspan = 1;
                cell5n15.HorizontalAlignment = 1;
                table3.AddCell(cell5n15);

                PdfPCell cell5n26 = new PdfPCell(new Phrase(new Chunk(bal.ToString(), font8)));
                cell5n26.Colspan = 1;
                cell5n26.HorizontalAlignment = 1;
                table3.AddCell(cell5n26);

                PdfPCell cell67 = new PdfPCell(new Phrase(new Chunk("Sl No: " + dr1["start_slno"].ToString() + " to " + dr1["end_slno"].ToString(), font8)));
                table3.AddCell(cell67);
            }
            #endregion

            #region Pass printing and damage

            OdbcDataAdapter da45671 = new OdbcDataAdapter("(Select dispatchdate,case reason_reissue when '0' then 'Normal' end 'reason_reissue',count(dispatchdate),min(passno),max(passno),count(distinct donor_id) from t_donorpass "
                                                            + "where passtype='0' and reason_reissue='0' and status_print='1' and status_dispatch='1' and dispatchdate in "
                                                            + "(Select distinct dispatchdate from t_donorpass where passtype='0' and reason_reissue='0' and status_print='1' and status_dispatch='1' order by dispatchdate) "
                                                            + "group by dispatchdate)"
                                                            + "UNION "
                                                            + "(Select dispatchdate,case reason_reissue when '1' then 'damanged'when '2' then 'damanged' end 'reason_reissue',count(dispatchdate),min(passno),max(passno),count(distinct donor_id) from t_donorpass "
                                                            + "where passtype='0' and (reason_reissue='1' or reason_reissue='2') and dispatchdate in "
                                                            + "(Select distinct dispatchdate from t_donorpass where passtype='0' and (reason_reissue='1' or reason_reissue='2') order by dispatchdate) "
                                                            + "group by dispatchdate)"
                                                            + "order by dispatchdate desc", conn);
            DataTable dt45671 = new DataTable();
            da45671.Fill(dt45671);
            if (dt45671.Rows.Count == 0)
            {
                lblOk.Text = "No Details found"; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;
             
            }
            foreach (DataRow dr1 in dt45671.Rows)
            {
                if (dr1["reason_reissue"].ToString() == "Normal")
                {
                    bal = bal - int.Parse(dr1["count(dispatchdate)"].ToString());
                    slno = slno + 1;

                    DateTime dt5 = DateTime.Now;
                    try
                    {
                        dt5 = DateTime.Parse(dr1["dispatchdate"].ToString());
                    }
                    catch
                    {

                    }
                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table3.AddCell(cell41);

                    PdfPCell cell4w2 = new PdfPCell(new Phrase(new Chunk(dt5.ToString("dd MMM yyyy"), font8)));
                    table3.AddCell(cell4w2);

                    PdfPCell cell53 = new PdfPCell(new Phrase(new Chunk("Pass printed for " + dr1["count(distinct donor_id)"].ToString() + " donor", font8)));
                    table3.AddCell(cell53);

                    PdfPCell cell5n4 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cell5n4.Colspan = 1;
                    cell5n4.HorizontalAlignment = 1;
                    table3.AddCell(cell5n4);

                    PdfPCell cell5n15 = new PdfPCell(new Phrase(new Chunk(dr1["count(dispatchdate)"].ToString(), font8)));
                    cell5n15.Colspan = 1;
                    cell5n15.HorizontalAlignment = 1;
                    table3.AddCell(cell5n15);

                    PdfPCell cell5n26 = new PdfPCell(new Phrase(new Chunk(bal.ToString(), font8)));
                    cell5n26.Colspan = 1;
                    cell5n26.HorizontalAlignment = 1;
                    table3.AddCell(cell5n26);

                    PdfPCell cell67 = new PdfPCell(new Phrase(new Chunk("Sl No: " + dr1["min(passno)"].ToString() + " to " + dr1["max(passno)"].ToString(), font8)));
                    table3.AddCell(cell67);
                }
                else
                {
                    bal = bal - int.Parse(dr1["count(dispatchdate)"].ToString());
                    slno = slno + 1;

                    DateTime dt5 = DateTime.Now;
                    try
                    {
                        dt5 = DateTime.Parse(dr1["dispatchdate"].ToString());
                    }
                    catch
                    {
                    }
                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table3.AddCell(cell41);

                    PdfPCell cell4w2 = new PdfPCell(new Phrase(new Chunk(dt5.ToString("dd MMM yyyy"), font8)));
                    table3.AddCell(cell4w2);

                    PdfPCell cell53 = new PdfPCell(new Phrase(new Chunk("Damaged pass on printer", font8)));
                    table3.AddCell(cell53);

                    PdfPCell cell5n4 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cell5n4.Colspan = 1;
                    cell5n4.HorizontalAlignment = 1;
                    table3.AddCell(cell5n4);

                    PdfPCell cell5n15 = new PdfPCell(new Phrase(new Chunk(dr1["count(dispatchdate)"].ToString(), font8)));
                    cell5n15.Colspan = 1;
                    cell5n15.HorizontalAlignment = 1;
                    table3.AddCell(cell5n15);

                    PdfPCell cell5n26 = new PdfPCell(new Phrase(new Chunk(bal.ToString(), font8)));
                    cell5n26.Colspan = 1;
                    cell5n26.HorizontalAlignment = 1;
                    table3.AddCell(cell5n26);

                    PdfPCell cell67 = new PdfPCell(new Phrase(new Chunk("Sl No: " + dr1["min(passno)"].ToString() + " to " + dr1["max(passno)"].ToString(), font8)));
                    table3.AddCell(cell67);
                }
            }

            #endregion

            #region Return to Ex office

            OdbcCommand da45676 = new OdbcCommand();
            da45676.CommandType = CommandType.StoredProcedure;
            da45676.Parameters.AddWithValue("tblname", "t_material_return_items,t_material_retrun ");
            da45676.Parameters.AddWithValue("attribute", "return_qty,returnedon,returnedto");
            da45676.Parameters.AddWithValue("conditionv", "t_material_return_items.retno=t_material_retrun.retno and item_id =" + item_idfp + "");
            OdbcDataAdapter da45676a = new OdbcDataAdapter(da45676);
            DataTable dt45676 = new DataTable();
            dt45676 = obje.SpDtTbl("CALL selectcond(?,?,?)", da45676);

            
            foreach (DataRow dr1 in dt45676.Rows)
            {
                bal = bal - int.Parse(dr1["return_qty"].ToString());
                slno = slno + 1;

                DateTime dt5 = DateTime.Parse(dr1["returnedon"].ToString());
                PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table3.AddCell(cell41);

                PdfPCell cell4w2 = new PdfPCell(new Phrase(new Chunk(dt5.ToString("dd MMM yyyy"), font8)));
                table3.AddCell(cell4w2);

                PdfPCell cell53 = new PdfPCell(new Phrase(new Chunk("Returned to Exoffice", font8)));
                table3.AddCell(cell53);

                PdfPCell cell5n4 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell5n4.Colspan = 1;
                cell5n4.HorizontalAlignment = 1;
                table3.AddCell(cell5n4);

                PdfPCell cell5n15 = new PdfPCell(new Phrase(new Chunk(dr1["return_qty"].ToString(), font8)));
                cell5n15.Colspan = 1;
                cell5n15.HorizontalAlignment = 1;
                table3.AddCell(cell5n15);

                PdfPCell cell5n26 = new PdfPCell(new Phrase(new Chunk(bal.ToString(), font8)));
                cell5n26.Colspan = 1;
                cell5n26.HorizontalAlignment = 1;
                table3.AddCell(cell5n26);

                PdfPCell cell67 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table3.AddCell(cell67);
            }
            #endregion

            doc.Add(table3);

            #region Footer and Popup
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
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Freepass Stock Ledger";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            #endregion

        }
        catch (Exception ex)
        {
        }
        finally
        {
            conn.Close();
        }
        #endregion
    }

    protected void lnkPPSL_Click(object sender, EventArgs e)
    {
        #region Donor Paid Pass Stock Ledger
        try
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
            float bal = 0;
            DateTime curdate = DateTime.Now;
            string currenttime = curdate.ToString("yyyy/MM/dd") + ' ' + curdate.ToString("hh:mm:ss");

            OdbcCommand da456 = new OdbcCommand();
            da456.CommandType = CommandType.StoredProcedure;
            da456.Parameters.AddWithValue("tblname", "m_inventory ");
            da456.Parameters.AddWithValue("attribute", "itemcode,stock_qty,openingstock,updateddate,item_id");
            da456.Parameters.AddWithValue("conditionv", "item_id =(Select item_id from m_sub_item where itemname like '%paid% %pass% %Original%')");
            OdbcDataAdapter da456a = new OdbcDataAdapter(da456);
            DataTable dt456 = new DataTable();
            dt456 = obje.SpDtTbl("CALL selectcond(?,?,?)", da456);

            if (dt456.Rows.Count == 0)
            {
                lblOk.Text = "No Details found"; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;
            }

            string itemcode = dt456.Rows[0]["itemcode"].ToString();
            float balStock = float.Parse(dt456.Rows[0]["stock_qty"].ToString());
            float opengstk = float.Parse(dt456.Rows[0]["openingstock"].ToString());
            DateTime opendate = DateTime.Parse(dt456.Rows[0]["updateddate"].ToString());
            int item_idfp = int.Parse(dt456.Rows[0]["item_id"].ToString());

            #region PDF Name Heading and Format
            string report = "Donor paid pass StockLedger TakenOn " + curdate.ToString("dd-MM-yyyy") + ' ' + curdate.ToString("HH-mm-ss") + ".pdf";
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report + "";

            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);

            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

            PdfPTable table = new PdfPTable(5);
            float[] colWidths23av6 = { 5, 15, 10, 10, 10 };
            table.SetWidths(colWidths23av6);
            table.TotalWidth = 400f;

            PdfPCell cellq = new PdfPCell(new Phrase(new Chunk("Paid Pass Stock Ledger", font12)));
            cellq.Colspan = 5;
            cellq.Border = 1;
            cellq.HorizontalAlignment = 1;
            table.AddCell(cellq);
            doc.Add(table);

            PdfPTable table4 = new PdfPTable(4);
            float[] colWidths4 = { 12, 13, 12, 13 };
            table4.SetWidths(colWidths4);
            table4.TotalWidth = 400f;
            PdfPCell cell1aa = new PdfPCell(new Phrase(new Chunk("Store name: Accommodation office", font10)));
            cell1aa.Colspan = 2;
            cell1aa.Border = 0;
            table4.AddCell(cell1aa);

            PdfPCell cell1f23 = new PdfPCell(new Phrase(new Chunk("Item Name: Donor paid pass ", font10)));
            cell1f23.Colspan = 2;
            cell1f23.Border = 0;
            cell1f23.HorizontalAlignment = 2;
            table4.AddCell(cell1f23);

            PdfPCell cell1aaq = new PdfPCell(new Phrase(new Chunk("Item code: " + itemcode + "", font10)));
            cell1aaq.Colspan = 2;
            cell1aaq.Border = 0;
            table4.AddCell(cell1aaq);

            //PdfPCell cell1f23w = new PdfPCell(new Phrase(new Chunk("Balance Stock: " + balStock + " ", font10)));
            PdfPCell cell1f23w = new PdfPCell(new Phrase(new Chunk("Balance Stock: " , font10)));
            cell1f23w.Colspan = 2;
            cell1f23w.Border = 0;
            cell1f23w.HorizontalAlignment = 1;
            table4.AddCell(cell1f23w);

            PdfPCell cell1f23w1 = new PdfPCell(new Phrase(new Chunk("", font10)));
            cell1f23w1.Colspan = 4;
            cell1f23w1.Border = 0;
            table4.AddCell(cell1f23w1);
            doc.Add(table4);
            #endregion

            #region Gen page format
            PdfPTable table9 = new PdfPTable(7);
            float[] colWidths23av68 = { 3, 6, 11, 4, 4, 4, 8 };
            table9.SetWidths(colWidths23av68);
            table9.TotalWidth = 400f;
            PdfPCell cell1wf = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            cell1wf.Rowspan = 2;
            table9.AddCell(cell1wf);
            PdfPCell cell1f = new PdfPCell(new Phrase(new Chunk("Date", font9)));
            cell1f.Rowspan = 2;
            table9.AddCell(cell1f);
            PdfPCell cell2f = new PdfPCell(new Phrase(new Chunk("Description", font9)));
            cell2f.Rowspan = 2;
            table9.AddCell(cell2f);
            PdfPCell cell2x = new PdfPCell(new Phrase(new Chunk("Stock item status", font9)));
            cell2x.HorizontalAlignment = 1;
            cell2x.Colspan = 3;
            table9.AddCell(cell2x);
            PdfPCell cell3f = new PdfPCell(new Phrase(new Chunk("Serial No", font9)));
            cell3f.Rowspan = 2;
            table9.AddCell(cell3f);

            PdfPCell cell3f2 = new PdfPCell(new Phrase(new Chunk("Recv", font9)));
            cell3f2.HorizontalAlignment = 1;
            table9.AddCell(cell3f2);
            PdfPCell cell3f3 = new PdfPCell(new Phrase(new Chunk("Isue", font9)));
            cell3f3.HorizontalAlignment = 1;
            table9.AddCell(cell3f3);
            PdfPCell cell3f46 = new PdfPCell(new Phrase(new Chunk("Bal", font9)));
            cell3f46.HorizontalAlignment = 1;
            table9.AddCell(cell3f46);
            doc.Add(table9);
            #endregion

            int i = 0;
            slno = slno + 1;

            #region Next Page
            if (i > 24)
            {
                i = 1;

                PdfPTable table91 = new PdfPTable(7);
                float[] colWidths23av681 = { 3, 6, 11, 4, 4, 4, 8 };
                table91.SetWidths(colWidths23av68);
                table91.TotalWidth = 400f;
                PdfPCell cell1wf0 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                cell1wf0.Rowspan = 2;
                table91.AddCell(cell1wf0);
                PdfPCell cell1fb = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                cell1fb.Rowspan = 2;
                table91.AddCell(cell1fb);
                PdfPCell cell2fl = new PdfPCell(new Phrase(new Chunk("Description", font9)));
                cell2fl.Rowspan = 2;
                table91.AddCell(cell2fl);
                PdfPCell cell2xu = new PdfPCell(new Phrase(new Chunk("Stock item status", font9)));
                cell2xu.HorizontalAlignment = 1;
                cell2xu.Colspan = 3;
                table91.AddCell(cell2xu);
                PdfPCell cell3fyy = new PdfPCell(new Phrase(new Chunk("Serial No", font9)));
                cell3fyy.Rowspan = 2;
                table91.AddCell(cell3fyy);

                PdfPCell cell3f2vv = new PdfPCell(new Phrase(new Chunk("Recv", font9)));
                cell3f2vv.HorizontalAlignment = 1;
                table91.AddCell(cell3f2vv);
                PdfPCell cell3f3dd = new PdfPCell(new Phrase(new Chunk("Isue", font9)));
                cell3f3dd.HorizontalAlignment = 1;
                table91.AddCell(cell3f3dd);
                PdfPCell cell3f46ee = new PdfPCell(new Phrase(new Chunk("Bal", font9)));
                cell3f46ee.HorizontalAlignment = 1;
                table91.AddCell(cell3f46ee);
                doc.Add(table91);
            }
            #endregion

            #region Opening Stock
            PdfPTable table3 = new PdfPTable(7);
            float[] colWidths23av11 = { 3, 6, 11, 4, 4, 4, 8 };
            table3.SetWidths(colWidths23av11);
            table3.TotalWidth = 400f;

            foreach (DataRow dr in dt456.Rows)
            {
                PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table3.AddCell(cell41);

                PdfPCell cell4w2 = new PdfPCell(new Phrase(new Chunk(opendate.ToString("dd MMM yyyy"), font8)));
                table3.AddCell(cell4w2);

                PdfPCell cell53 = new PdfPCell(new Phrase(new Chunk("Opening stock", font8)));
                table3.AddCell(cell53);

                PdfPCell cell5n4 = new PdfPCell(new Phrase(new Chunk(opengstk.ToString(), font8)));
                cell5n4.Colspan = 1;
                cell5n4.HorizontalAlignment = 1;
                table3.AddCell(cell5n4);

                PdfPCell cell5n15 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell5n15.Colspan = 1;
                cell5n15.HorizontalAlignment = 1;
                table3.AddCell(cell5n15);

                PdfPCell cell5n26 = new PdfPCell(new Phrase(new Chunk(opengstk.ToString(), font8)));
                cell5n26.Colspan = 1;
                cell5n26.HorizontalAlignment = 1;
                table3.AddCell(cell5n26);

                PdfPCell cell67 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table3.AddCell(cell67);

                bal = bal + float.Parse(dt456.Rows[0]["openingstock"].ToString());
            }
            #endregion

            #region Pass Reception

            OdbcCommand da4567 = new OdbcCommand();
            da4567.CommandType = CommandType.StoredProcedure;
            da4567.Parameters.AddWithValue("tblname", "t_grn_items,t_grn,t_inventoryrequest_items_issue ");
            da4567.Parameters.AddWithValue("attribute", "receivedon,receive_qty,start_slno,end_slno");
            da4567.Parameters.AddWithValue("conditionv", "t_grn_items.grnno=t_grn.grnno and t_inventoryrequest_items_issue.item_id=t_grn_items.item_id and issueno=refno and t_grn_items.item_id=" + item_idfp + " order by receivedon");
            OdbcDataAdapter da4567a = new OdbcDataAdapter(da4567);
            DataTable dt4567 = new DataTable();
            dt4567 = obje.SpDtTbl("CALL selectcond(?,?,?)", da4567);
            
            foreach (DataRow dr1 in dt4567.Rows)
            {
                bal = bal + int.Parse(dr1["receive_qty"].ToString());
                slno = slno + 1;

                DateTime dt5 = DateTime.Parse(dr1["receivedon"].ToString());
                PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table3.AddCell(cell41);

                PdfPCell cell4w2 = new PdfPCell(new Phrase(new Chunk(dt5.ToString("dd MMM yyyy"), font8)));
                table3.AddCell(cell4w2);

                PdfPCell cell53 = new PdfPCell(new Phrase(new Chunk("Received from press", font8)));
                table3.AddCell(cell53);

                PdfPCell cell5n4 = new PdfPCell(new Phrase(new Chunk(dr1["receive_qty"].ToString(), font8)));
                cell5n4.Colspan = 1;
                cell5n4.HorizontalAlignment = 1;
                table3.AddCell(cell5n4);

                PdfPCell cell5n15 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell5n15.Colspan = 1;
                cell5n15.HorizontalAlignment = 1;
                table3.AddCell(cell5n15);

                PdfPCell cell5n26 = new PdfPCell(new Phrase(new Chunk(bal.ToString(), font8)));
                cell5n26.Colspan = 1;
                cell5n26.HorizontalAlignment = 1;
                table3.AddCell(cell5n26);

                PdfPCell cell67 = new PdfPCell(new Phrase(new Chunk("Sl No: " + dr1["start_slno"].ToString() + " to " + dr1["end_slno"].ToString(), font8)));
                table3.AddCell(cell67);
            }
            #endregion

            #region Pass printing and damage

            OdbcDataAdapter da45671 = new OdbcDataAdapter("(Select dispatchdate,case reason_reissue when '0' then 'Normal' end 'reason_reissue',count(dispatchdate),min(passno),max(passno),count(distinct donor_id) from t_donorpass "
                                                            + "where passtype='1' and reason_reissue='0' and status_print='1' and status_dispatch='1' and dispatchdate in "
                                                            + "(Select distinct dispatchdate from t_donorpass where passtype='1' and reason_reissue='0' and status_print='1' and status_dispatch='1' order by dispatchdate) "
                                                            + "group by dispatchdate)"
                                                            + "UNION "
                                                            + "(Select dispatchdate,case reason_reissue when '1' then 'damanged'when '2' then 'damanged' end 'reason_reissue',count(dispatchdate),min(passno),max(passno),count(distinct donor_id) from t_donorpass "
                                                            + "where passtype='1' and (reason_reissue='1' or reason_reissue='2') and dispatchdate in "
                                                            + "(Select distinct dispatchdate from t_donorpass where passtype='1' and (reason_reissue='1' or reason_reissue='2') order by dispatchdate) "
                                                            + "group by dispatchdate)"
                                                            + "order by dispatchdate desc", conn);
            DataTable dt45671 = new DataTable();
            da45671.Fill(dt45671);
            
            foreach (DataRow dr1 in dt45671.Rows)
            {
                if (dr1["reason_reissue"].ToString() == "Normal")
                {
                    bal = bal - int.Parse(dr1["count(dispatchdate)"].ToString());
                    slno = slno + 1;

                    DateTime dt5 = DateTime.Now;
                    try
                    {
                        dt5 = DateTime.Parse(dr1["dispatchdate"].ToString());
                    }
                    catch
                    {
                    }
                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table3.AddCell(cell41);

                    PdfPCell cell4w2 = new PdfPCell(new Phrase(new Chunk(dt5.ToString("dd MMM yyyy"), font8)));
                    table3.AddCell(cell4w2);

                    PdfPCell cell53 = new PdfPCell(new Phrase(new Chunk("Pass printed for " + dr1["count(distinct donor_id)"].ToString() + " donor", font8)));
                    table3.AddCell(cell53);

                    PdfPCell cell5n4 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cell5n4.Colspan = 1;
                    cell5n4.HorizontalAlignment = 1;
                    table3.AddCell(cell5n4);

                    PdfPCell cell5n15 = new PdfPCell(new Phrase(new Chunk(dr1["count(dispatchdate)"].ToString(), font8)));
                    cell5n15.Colspan = 1;
                    cell5n15.HorizontalAlignment = 1;
                    table3.AddCell(cell5n15);

                    PdfPCell cell5n26 = new PdfPCell(new Phrase(new Chunk(bal.ToString(), font8)));
                    cell5n26.Colspan = 1;
                    cell5n26.HorizontalAlignment = 1;
                    table3.AddCell(cell5n26);

                    PdfPCell cell67 = new PdfPCell(new Phrase(new Chunk("Sl No: " + dr1["min(passno)"].ToString() + " to " + dr1["max(passno)"].ToString(), font8)));
                    table3.AddCell(cell67);
                }
                else
                {
                    bal = bal - int.Parse(dr1["count(dispatchdate)"].ToString());
                    slno = slno + 1;

                    DateTime dt5 = DateTime.Now;
                    try
                    {
                        dt5 = DateTime.Parse(dr1["dispatchdate"].ToString());
                    }
                    catch
                    {
                    }
                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table3.AddCell(cell41);

                    PdfPCell cell4w2 = new PdfPCell(new Phrase(new Chunk(dt5.ToString("dd MMM yyyy"), font8)));
                    table3.AddCell(cell4w2);

                    PdfPCell cell53 = new PdfPCell(new Phrase(new Chunk("Damaged pass on printer", font8)));
                    table3.AddCell(cell53);

                    PdfPCell cell5n4 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cell5n4.Colspan = 1;
                    cell5n4.HorizontalAlignment = 1;
                    table3.AddCell(cell5n4);

                    PdfPCell cell5n15 = new PdfPCell(new Phrase(new Chunk(dr1["count(dispatchdate)"].ToString(), font8)));
                    cell5n15.Colspan = 1;
                    cell5n15.HorizontalAlignment = 1;
                    table3.AddCell(cell5n15);

                    PdfPCell cell5n26 = new PdfPCell(new Phrase(new Chunk(bal.ToString(), font8)));
                    cell5n26.Colspan = 1;
                    cell5n26.HorizontalAlignment = 1;
                    table3.AddCell(cell5n26);

                    PdfPCell cell67 = new PdfPCell(new Phrase(new Chunk("Sl No: " + dr1["min(passno)"].ToString() + " to " + dr1["max(passno)"].ToString(), font8)));
                    table3.AddCell(cell67);
                }
            }

            #endregion

            #region Return to Ex office

            OdbcCommand da45676 = new OdbcCommand();
            da45676.CommandType = CommandType.StoredProcedure;
            da45676.Parameters.AddWithValue("tblname", "t_material_return_items,t_material_retrun ");
            da45676.Parameters.AddWithValue("attribute", "return_qty,returnedon,returnedto");
            da45676.Parameters.AddWithValue("conditionv", "t_material_return_items.retno=t_material_retrun.retno and item_id =" + item_idfp + "");
            OdbcDataAdapter da45676a = new OdbcDataAdapter(da45676);
            DataTable dt45676 = new DataTable();
            dt45676 = obje.SpDtTbl("CALL selectcond(?,?,?)", da45676);

            //OdbcDataAdapter da45676 = new OdbcDataAdapter("Select return_qty,returnedon,returnedto from t_material_return_items,t_material_retrun where t_material_return_items.retno=t_material_retrun.retno and item_id =" + item_idfp + "", conn);
            //DataTable dt45676 = new DataTable();
            //da45676.Fill(dt45676);
            foreach (DataRow dr1 in dt45676.Rows)
            {
                bal = bal - int.Parse(dr1["return_qty"].ToString());
                slno = slno + 1;

                DateTime dt5 = DateTime.Parse(dr1["returnedon"].ToString());
                PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table3.AddCell(cell41);

                PdfPCell cell4w2 = new PdfPCell(new Phrase(new Chunk(dt5.ToString("dd MMM yyyy"), font8)));
                table3.AddCell(cell4w2);

                PdfPCell cell53 = new PdfPCell(new Phrase(new Chunk("Returned to Exoffice", font8)));
                table3.AddCell(cell53);

                PdfPCell cell5n4 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell5n4.Colspan = 1;
                cell5n4.HorizontalAlignment = 1;
                table3.AddCell(cell5n4);

                PdfPCell cell5n15 = new PdfPCell(new Phrase(new Chunk(dr1["return_qty"].ToString(), font8)));
                cell5n15.Colspan = 1;
                cell5n15.HorizontalAlignment = 1;
                table3.AddCell(cell5n15);

                PdfPCell cell5n26 = new PdfPCell(new Phrase(new Chunk(bal.ToString(), font8)));
                cell5n26.Colspan = 1;
                cell5n26.HorizontalAlignment = 1;
                table3.AddCell(cell5n26);

                PdfPCell cell67 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table3.AddCell(cell67);
            }
            #endregion

            doc.Add(table3);

            #region Footer and Popup
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
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Paidpass Stock Ledger";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            #endregion

        }
        catch (Exception ex)
        {
        }
        finally
        {
            conn.Close();
        }
        #endregion
    }
}
#endregion


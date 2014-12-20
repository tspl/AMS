/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMMODATION
// Screen Name      :      Inventory Master
// Form Name        :      Inventory Master.aspx
// ClassFile Name   :      Inventory Master.aspx.cs
// Purpose          :      create master for inventory items
// Created by       :      Asha
// Created On       :      2-September-2010
// Last Modified    :      11-November-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       8-September-2010  Asha        Code change as per the review


//-------------------------------------------------------------------


using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Obout.ComboBox;
using PDF;

public partial class frmnewInvMtr : System.Web.UI.Page
{
    
    static string strConnection;
    OdbcConnection con = new OdbcConnection();
    int id, id1,q, k, listb, NewItemId;
    string build, builda, code, GName, Ddate1;
    decimal amount1,  Rqt2;
    int sl, endl; int q1, it1;
    clsCommon obj = new clsCommon();
    commonClass obje = new commonClass();
    DataTable dtitem = new DataTable();
    DataTable dtt2 = new DataTable();
    DataTable Delete = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        #region PAGE LOAD
        if (!Page.IsPostBack)
        {
            this.ScriptManager1.SetFocus(cmbItemCategory);
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            Title = "Tsunami ARMS - Inventory Master";
            check();

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }

            #region Reorder Level is less than Stock
            //OdbcCommand Rol = new OdbcCommand("select itemname from m_inventory mi,m_sub_item i where reorderlevel < stock_qty and mi.item_id=i.item_id and mi.rowstatus<>'2'", con);
            //OdbcDataReader Rolr = Rol.ExecuteReader();
            //while (Rolr.Read())
            //{
            //    string Ritem = Rolr["itemname"].ToString();

            //    lblOk.Text = Ritem.ToString() + "'s Rorderlevel is less than Stock quantity  "; lblHead.Text = "Tsunami ARMS - Warning";
            //    pnlOk.Visible = true;
            //    pnlYesNo.Visible = false;
            //    ModalPopupExtender2.Show();
            //}
            #endregion

            try
            {
                string username = Session["username"].ToString();
                OdbcCommand ccm = new OdbcCommand();
                ccm.CommandType = CommandType.StoredProcedure;
                ccm.Parameters.AddWithValue("tblname", "m_user");
                ccm.Parameters.AddWithValue("attribute", "user_id");
                ccm.Parameters.AddWithValue("conditionv", "username='" + username + "'");
                OdbcDataAdapter da3 = new OdbcDataAdapter(ccm);
                DataTable dtt = new DataTable();
                dtt = obje.SpDtTbl("CALL selectcond(?,?,?)", ccm);
                id1 = int.Parse(dtt.Rows[0][0].ToString());
                Session["userid"] = id1;
            }
            catch
            {
                id1 = 0;
                Session["userid"] = id1;
            }

            // listbox
            OdbcCommand cmd3 = new OdbcCommand();
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Parameters.AddWithValue("tblname", "m_sub_supplier");
            cmd3.Parameters.AddWithValue("attribute", "supplier_id,suppliername");
            cmd3.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by suppliername asc");
            OdbcDataAdapter dacnt3 = new OdbcDataAdapter(cmd3);
            DataTable dtt3 = new DataTable();
            dtt3 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd3);

            for (int ii = 0; ii < dtt3.Rows.Count; ii++)
            {
                lstSupplier.Items.Add(dtt3.Rows[ii][1].ToString());
                lstSupplier.Items[ii].Text = dtt3.Rows[ii][1].ToString();
                lstSupplier.Items[ii].Value = dtt3.Rows[ii][0].ToString();
            }

            ItemCategory();

            OdbcCommand Store4 = new OdbcCommand();
            Store4.CommandType = CommandType.StoredProcedure;
            Store4.Parameters.AddWithValue("tblname", "m_sub_unit");
            Store4.Parameters.AddWithValue("attribute", "unit_id,unitname");
            Store4.Parameters.AddWithValue("conditionv", "rowstatus<>'2' order by unitname asc");
            OdbcDataAdapter Store46 = new OdbcDataAdapter(Store4);
            DataTable ds2 = new DataTable();
            ds2 = obje.SpDtTbl("CALL selectcond(?,?,?)", Store4);
            DataRow row1 = ds2.NewRow();
            ds2.Rows.InsertAt(row1, 0);
            row1["unit_id"] = "-1";
            row1["unitname"] = "--Select--";
            cmbUnit.DataSource = ds2;
            cmbUnit.DataBind();                     

            Store();         

            Pnlkitadd.Visible = false;

            // new link
            if (Session["itemcatgorylink"] == "yes")
            {

                cmbItemCategory.SelectedValue = Session["itcat"].ToString();
                cmbItemCategory_SelectedIndexChanged(null, null);
                cmbItemName.SelectedValue = Session["itnam"].ToString();
                txtItemMaker.Text = Session["mak"].ToString();
                txtItemModel.Text = Session["mod"].ToString();
                txtDailyConsumption.Text = Session["dail"].ToString();
                cmbStore.SelectedValue = Session["stor"].ToString();
                txtEconomicQty.Text = Session["eqty"].ToString();
                txtReorLvl.Text = Session["reorder"].ToString();
                cmbEssentialityLevel.SelectedValue = Session["ess"].ToString();
                txtOpeningStock.Text = Session["opstock"].ToString();
                txtMaxStock.Text = Session["mxstock"].ToString();
                txtItemid.Text = Session["id"].ToString();
                cmbItemClass.SelectedValue = Session["clss"].ToString();
                cmbcounter.SelectedValue = Session["cntr"].ToString();
                txtUnit.Text = Session["unit1"].ToString();
                cmbUnit.SelectedValue = Session["unit"].ToString();
                cmbSerialNo.SelectedValue = Session["serial"].ToString();
                try
                {
                    int[] b = (int[])Session["supply"];

                    for (int j = 0; j < lstSupplier.Items.Count; j++)
                    {
                        if (b[j] == 1)
                        {
                            lstSupplier.Items[j].Selected = true;
                        }

                    }

                }

                catch { }


                Session["unit1"] = txtUnit.Text.ToString();
                Session["itemcatgorylink"] = "no";


                if (Session["item"] == "itemcategory")
                {
                    this.ScriptManager1.SetFocus(cmbItemName);
                }
                else if (Session["item"] == "itemname")
                {
                    this.ScriptManager1.SetFocus(txtItemMaker);
                }
                else if (Session["item"] == "storename")
                {
                    this.ScriptManager1.SetFocus(lstSupplier);
                }

                else if (Session["item"] == "supplier")
                {
                    this.ScriptManager1.SetFocus(txtEconomicQty);
                }
                else if (Session["item"] == "unitname")
                {
                    this.ScriptManager1.SetFocus(txtDailyConsumption);
                }


            }

            con.Close();
            gridview();
            dtitem = additem();
            Session["dtItem"] = dtitem;
            Session["Del"] = Delete;
            pnlKitItems.Visible = false;
        }
        #endregion
    }

    public void ItemCategory()
    {
        con = obje.NewConnection();
        OdbcCommand Store1 = new OdbcCommand();
        Store1.CommandType = CommandType.StoredProcedure;
        Store1.Parameters.AddWithValue("tblname", "m_sub_itemcategory");
        Store1.Parameters.AddWithValue("attribute", "itemcat_id,itemcatname");
        Store1.Parameters.AddWithValue("conditionv", "rowstatus<>'2' order by itemcatname asc");
        OdbcDataAdapter Store16 = new OdbcDataAdapter(Store1);
        DataTable ds1 = new DataTable();
        ds1 = obje.SpDtTbl("CALL selectcond(?,?,?)", Store1);
        DataRow row = ds1.NewRow();
        ds1.Rows.InsertAt(row, 0);
        row["itemcat_id"] = "-1";
        row["itemcatname"] = "--Select--";
        cmbItemCategory.DataSource = ds1;
        cmbItemCategory.DataBind();
        con.Close();                
    }

    public void Store()
    {
        con = obje.NewConnection();
        OdbcCommand Store5 = new OdbcCommand();
        Store5.CommandType = CommandType.StoredProcedure;
        Store5.Parameters.AddWithValue("tblname", "m_sub_store");
        Store5.Parameters.AddWithValue("attribute", "store_id,storename");
        Store5.Parameters.AddWithValue("conditionv", "rowstatus<>'2' order by storename asc");
        OdbcDataAdapter Store56 = new OdbcDataAdapter(Store5);
        DataTable ds3 = new DataTable();
        ds3 = obje.SpDtTbl("CALL selectcond(?,?,?)", Store5);
        DataRow row3 = ds3.NewRow();
        ds3.Rows.InsertAt(row3, 0);
        row3["store_id"] = "-1";
        row3["storename"] = "--Select--";
        cmbStore.DataSource = ds3;
        cmbStore.DataBind();
        con.Close();              
    }

    #region data table add item
    public DataTable additem()
    {
        dtitem.Columns.Clear();
        dtitem.Columns.Add("Itemcode", System.Type.GetType("System.String"));
        dtitem.Columns.Add("Itemname", System.Type.GetType("System.String"));
        dtitem.Columns.Add("Quantity", System.Type.GetType("System.Int32"));
        dtitem.Columns.Add("Measurement", System.Type.GetType("System.String"));
        return (dtitem);
    }
    #endregion

    #region Authentication Check function
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("InventoryMaster", level) == 0)
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

    #region OK Message
    public void okmessage(string head, string message)
    {
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender2.Show();
    }
    #endregion

    #region GRIDVIEW general
    public void gridview()
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        dtgInventoryDetails.Caption = "Inventory details";
        OdbcCommand cmd2 = new OdbcCommand();
        cmd2.CommandType = CommandType.StoredProcedure;
        cmd2.Parameters.AddWithValue("tblname", "m_inventory inv,m_sub_itemcategory mc,m_sub_item mi");
        cmd2.Parameters.AddWithValue("attribute", "distinct invent_id as Id,mc.itemcatname as Category,mi.itemname as Name,itemclass as Class,itemmaker as Maker,itemmodel as Model,essentiality as Essentiality,unit as Unit");
        cmd2.Parameters.AddWithValue("conditionv", "inv.rowstatus<> 2 and inv.itemcat_id=mc.itemcat_id and inv.item_id=mi.item_id group by invent_id");
        OdbcDataAdapter dacnt2 = new OdbcDataAdapter(cmd2);
        dtt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
        dtgInventoryDetails.DataSource = dtt2;
        dtgInventoryDetails.DataBind();

    }
    #endregion

    #region GRIDVIEW categorychange
    public void gridviewcategorychange()
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        int iid = Convert.ToInt32(cmbItemCategory.SelectedValue.ToString());
        dtgInventoryDetails.Caption = "Inventory details itemcategory";
        OdbcCommand cmd2 = new OdbcCommand();
        cmd2.CommandType = CommandType.StoredProcedure;
        cmd2.Parameters.AddWithValue("tblname", "m_inventory inv,m_sub_itemcategory mc,m_sub_item mi");
        cmd2.Parameters.AddWithValue("attribute", "distinct inv.invent_id as invent_id,mc.itemcatname as Category,mi.itemname as Name,itemclass as Class,itemmaker as Maker,itemmodel as Model,essentiality as Essentiality,unit as Min_unit");
        cmd2.Parameters.AddWithValue("conditionv", "inv.itemcat_id = mc.itemcat_id and mi.itemcat_id= mc.itemcat_id and inv.rowstatus<>" + 2 + " and inv.itemcat_id=" + iid + " group by invent_id");
        OdbcDataAdapter dacnt2a = new OdbcDataAdapter(cmd2);
        DataTable dtt2q = new DataTable();
        dtt2q = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
        dtgInventoryDetails.DataSource = dtt2q;
        dtgInventoryDetails.DataBind();
        con.Close();

    }
    #endregion

    #region GRIDVIEW category & itemname chang
    public void gridviewitemnamechang()
    {

        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        int iid = Convert.ToInt32(cmbItemCategory.SelectedValue.ToString());
        int cid = Convert.ToInt32(cmbItemName.SelectedValue.ToString());
        dtgInventoryDetails.Caption = "Inventory details itemcategory";
        OdbcCommand cmd2 = new OdbcCommand();
        cmd2.CommandType = CommandType.StoredProcedure;
        cmd2.Parameters.AddWithValue("tblname", "m_inventory inv,m_sub_itemcategory mc,m_sub_item mi");
        cmd2.Parameters.AddWithValue("attribute", "distinct inv.invent_id as No,mc.itemcatname as Category,mi.itemname as Name,itemclass as Class,itemmaker as Maker,itemmodel as Model,essentiality as Essentiality,unit as Min_unit");
        cmd2.Parameters.AddWithValue("conditionv", "inv.itemcat_id=mc.itemcat_id and mi.itemcat_id=mc.itemcat_id  and inv.rowstatus<>" + 2 + " and inv.itemcat_id=" + iid + " and inv.item_id=" + cid + " group by invent_id");
        OdbcDataAdapter dacnt2p = new OdbcDataAdapter(cmd2);
        DataTable dtt2p = new DataTable();
        dtt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
        dtgInventoryDetails.DataSource = dtt2p;
        dtgInventoryDetails.DataBind();
    }
    #endregion

    #region GRID SORTING FUNCTION
    private string ConvertSortDirectionToSql(SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;
        switch (sortDirection)
        {
            case SortDirection.Ascending:
                newSortDirection = "ASC";
                break;

            case SortDirection.Descending:
                newSortDirection = "DESC";
                break;
        }
        return newSortDirection;
    }
    #endregion

    #region CLEAR
    public void clear()
    {
        ItemCategory();
        Store();
        cmbItemCategory.SelectedIndex = -1;
        cmbItemName.SelectedIndex = -1;
        cmbEssentialityLevel.SelectedIndex = -1;
        txtReorLvl.Text = "";
        txtOpeningStock.Text = "";
        txtEconomicQty.Text = "";
        txtDailyConsumption.Text = "";
        txtItemMaker.Text = "";
        txtItemModel.Text = "";
        txtItemid.Text = "";
        cmbItemClass.SelectedIndex = -1;
        txtMaxStock.Text = "";
        cmbitmcatrpt.SelectedIndex = -1;
        cmbsuprpt.SelectedIndex = -1;
        cmbesnltylvlrpt.SelectedIndex = -1;
        cmbcounter.SelectedIndex = -1;
        cmbrptcat.SelectedIndex = -1;
        dtgInventoryDetails.Visible = true;
        cmbitmcatrpt.Enabled = false;
        cmbesnltylvlrpt.Enabled = false;
        cmbsuprpt.Enabled = false;
        cmbStore.SelectedIndex = -1;
        txtUnit.Text = "";
        txtUnit.Text = "";
        lstSupplier.Items.Clear();
        cmbUnit.SelectedIndex = -1;
        cmbSerialNo.SelectedIndex = -1;
        pnlKitItems.Visible = false;
        pnlAddDetails.Visible = false;
        cmbStore.SelectedIndex = -1;
        cmbItemCategory.SelectedIndex = -1;
        cmbSerialNo.SelectedIndex = -1;
        cmbItemCategory.SelectedIndex = -1;
        cmbStore.SelectedIndex = -1;
        Pnlkitadd.Visible = false;
        cmbItemNm.SelectedIndex = -1;
        cmbItemCat.SelectedIndex = -1;
        txtItemCode.Text = "";
        txtQnty.Text = "";
        txtUOM.Text = "";
        Delete.Columns.Clear();
        cmbItemCategory.SelectedIndex = -1;
        cmbStore.SelectedIndex = -1;
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        OdbcCommand cmd3 = new OdbcCommand();
        cmd3.CommandType = CommandType.StoredProcedure;
        cmd3.Parameters.AddWithValue("tblname", "m_sub_supplier");
        cmd3.Parameters.AddWithValue("attribute", "supplier_id,suppliername");
        cmd3.Parameters.AddWithValue("conditionv", "rowstatus<>2");
        OdbcDataAdapter dacnt3 = new OdbcDataAdapter(cmd3);
        DataTable dtt3 = new DataTable();
        dtt3 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd3);

        for (int ii = 0; ii < dtt3.Rows.Count; ii++)
        {

            lstSupplier.Items.Add(dtt3.Rows[ii][0].ToString());
            lstSupplier.Items[ii].Text = dtt3.Rows[ii][1].ToString();
            lstSupplier.Items[ii].Value = dtt3.Rows[ii][0].ToString();
        }

        btnDelete.Enabled = false;
        btnSave.Text = "Save";
        cmbItemCategory.SelectedIndex = -1;
        cmbStore.SelectedIndex = -1;
        gridview();
    }
    #endregion


    #region CLEAR button click
    protected void btnClear_Click(object sender, EventArgs e)
    {

        clear();
        pnlrpt.Visible = false;
        btnSave.Text = "Save";
        this.ScriptManager1.SetFocus(cmbItemCategory);
    }
    #endregion

    #region Save button click
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (btnSave.Text == "Save")
        {
            lblMsg.Text = "Do you want to Save item?"; lblHead.Text = "Tsunami ARMS - Confirmation";
            ViewState["action"] = "Save";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        else if (btnSave.Text == "Edit")
        {
            lblMsg.Text = "Do you want to Edit item?"; lblHead.Text = "Tsunami ARMS - Confirmation";
            ViewState["action"] = "Save";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
    }
    #endregion

    #region Button YES Click
    protected void btnYes_Click(object sender, EventArgs e)
    {

        if (ViewState["action"].ToString() == "Save")
        {
            #region save
            DateTime date = DateTime.Now;
            string dat = date.ToString("yyyy-MM-dd HH:mm:ss");           
            cmbItemClass.SelectedItem.Text = emptystring(cmbItemClass.SelectedItem.Text);
            txtReorLvl.Text = emptyinteger(txtReorLvl.Text);
            txtDailyConsumption.Text = emptyinteger(txtDailyConsumption.Text);
            txtItemMaker.Text = emptystring(txtItemMaker.Text);
            txtItemModel.Text = emptystring(txtItemModel.Text);
            txtItemid.Text = emptystring(txtItemid.Text);
            txtOpeningStock.Text = emptyinteger(txtOpeningStock.Text);
            txtMaxStock.Text = emptyinteger(txtMaxStock.Text);
            txtUnit.Text = emptystring(txtUnit.Text);
            txtEconomicQty.Text = emptyinteger(txtEconomicQty.Text);
            cmbSerialNo.SelectedItem.Text = emptystring(cmbSerialNo.SelectedItem.Text);

            OdbcTransaction odbTrans = null;

            con = obje.NewConnection();
            if (btnSave.Text == "Save")
            {
                try
                {
                    odbTrans = con.BeginTransaction();
                    OdbcCommand cmd246 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                    cmd246.CommandType = CommandType.StoredProcedure;
                    cmd246.Parameters.AddWithValue("tblname", "m_inventory");
                    cmd246.Parameters.AddWithValue("attribute", "itemcode,store_id");
                    cmd246.Parameters.AddWithValue("conditionv", "rowstatus<>2 and itemcode='" + txtItemid.Text.ToString() + "'and store_id=" + cmbStore.SelectedValue + "");
                    OdbcDataAdapter dacnt246 = new OdbcDataAdapter(cmd246);
                    cmd246.Transaction = odbTrans;
                    DataTable dtt246 = new DataTable();
                    dacnt246.Fill(dtt246);

                    if (dtt246.Rows.Count > 0)
                    {
                        lblOk.Text = " Item is already exsits in the Same Store...... "; lblHead.Text = "Tsunami ARMS - Warning";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                        txtItemid.Text = "";
                        this.ScriptManager1.SetFocus(txtItemid);
                        clear();
                        return;
                    }
                    
                    OdbcCommand cmd6 = new OdbcCommand("select max(invent_id) from m_inventory", con);
                    cmd6.Transaction = odbTrans;
                    if (Convert.IsDBNull(cmd6.ExecuteScalar()) == true)
                    {
                        id1 = 1;
                    }
                    else
                    {
                        id1 = Convert.ToInt32(cmd6.ExecuteScalar());
                        id1 = id1 + 1;
                    }

                    id = Convert.ToInt32(Session["userid"].ToString());
                    string Control = cmbSerialNo.SelectedItem.Text.ToString();
                    int YN;
                    if (Control == "Yes")
                    {
                        YN = 1;
                    }
                    else
                    {
                        YN = 0;
                    }
                    OdbcCommand cmd7 = new OdbcCommand("CALL savedata(?,?)", con);
                    cmd7.CommandType = CommandType.StoredProcedure;
                    cmd7.Parameters.AddWithValue("tblname", "m_inventory");

                    //string aaa = "" + id1 + "," + Convert.ToInt32(cmbItemCategory.SelectedValue.ToString()) + "," + Convert.ToInt32(cmbItemName.SelectedValue.ToString()) + ",'" + cmbEssentialityLevel.SelectedItem.Text.ToString() + "','" + cmbItemClass.SelectedItem.Text.ToString() + "'," + int.Parse(txtReorLvl.Text) + "," + float.Parse(txtEconomicQty.Text) + "," + decimal.Parse(txtDailyConsumption.Text) + ",'" + txtItemMaker.Text.ToString() + "','" + txtItemModel.Text.ToString() + "','" + txtItemid.Text.ToString() + "'," + int.Parse(txtOpeningStock.Text) + "," + int.Parse(txtMaxStock.Text) + "," + Convert.ToInt32(cmbStore.SelectedValue.ToString()) + ",'" + txtUnit.Text + "'," + id + ",'" + dat + "'," + "0" + "," + id + ",'" + dat + "'," + Convert.ToInt32(cmbUnit.SelectedValue.ToString()) + ",'" + YN + "'";
                    cmd7.Parameters.AddWithValue("val", "" + id1 + "," + Convert.ToInt32(cmbItemCategory.SelectedValue.ToString()) + "," + Convert.ToInt32(cmbItemName.SelectedValue.ToString()) + ",'" + cmbEssentialityLevel.SelectedItem.Text.ToString() + "','" + cmbItemClass.SelectedItem.Text.ToString() + "'," + int.Parse(txtReorLvl.Text) + "," + float.Parse(txtEconomicQty.Text) + "," + decimal.Parse(txtDailyConsumption.Text) + ",'" + txtItemMaker.Text.ToString() + "','" + txtItemModel.Text.ToString() + "','" + txtItemid.Text.ToString() + "'," + decimal.Parse(txtOpeningStock.Text) + "," + decimal.Parse(txtOpeningStock.Text) + "," + int.Parse(txtMaxStock.Text) + "," + Convert.ToInt32(cmbStore.SelectedValue.ToString()) + ",'" + txtUnit.Text + "'," + id + ",'" + dat + "'," + "0" + "," + id + ",'" + dat + "'," + Convert.ToInt32(cmbUnit.SelectedValue.ToString()) + ",'" + YN + "'");                   
                    cmd7.Transaction = odbTrans;
                    cmd7.ExecuteNonQuery();

                    for (k = 0; k < lstSupplier.Items.Count; k++)
                    {

                        OdbcCommand cmd65 = new OdbcCommand("CALL savedata(?,?)", con);
                        cmd65.CommandType = CommandType.StoredProcedure;
                        cmd65.Transaction = odbTrans;
                        cmd65.Parameters.AddWithValue("tblname", "m_inventory_supplier");
                       
                        if (lstSupplier.Items[k].Selected == true)
                        {
                            OdbcCommand list = new OdbcCommand("select supplier_id from m_sub_supplier where suppliername='" + lstSupplier.Items[k].Text.ToString() + "' and rowstatus<>2", con);
                            list.Transaction = odbTrans;
                            OdbcDataReader lisr = list.ExecuteReader();                           
                            while (lisr.Read())
                            {
                                listb = Convert.ToInt32(lisr["supplier_id"].ToString());
                            }
                            cmd65.Parameters.AddWithValue("val", "" + id1 + "," + listb + "," + id + ",'" + dat + "'");
                            //cmd65.Transaction = odbTrans;
                            cmd65.ExecuteNonQuery();
                        }                        
                    }

                    int kid = 0;

                    if (cmbItemCategory.SelectedItem.Text == "Kit")
                    {

                        dtitem = (DataTable)Session["dtItem"];
                        for (int i = 0; i < dtitem.Rows.Count; i++)
                        {

                            DataRow[] drSpare = dtitem.Select("Itemcode='" + (dtitem.Rows[i]["Itemcode"]) + "'");
                            if (drSpare.Length > 0)
                            {
                                foreach (DataRow row in drSpare)
                                {
                                    OdbcCommand cmd9 = new OdbcCommand("select max(invent_kit_id) from m_inventory_kit", con);
                                    cmd9.Transaction = odbTrans;
                                    if (Convert.IsDBNull(cmd9.ExecuteScalar()) == true)
                                    {
                                        kid = 1;
                                    }
                                    else
                                    {
                                        kid = Convert.ToInt32(cmd9.ExecuteScalar());
                                        kid = kid + 1;
                                    }

                                    String itname = dtitem.Rows[i]["Itemname"].ToString();
                                    OdbcCommand ItName = new OdbcCommand("SELECT item_id from m_sub_item where itemname='" + itname.ToString() + "' and rowstatus<>'2'", con);
                                    ItName.Transaction = odbTrans;
                                    OdbcDataReader ItNamer = ItName.ExecuteReader();
                                    if (ItNamer.Read())
                                    {
                                        NewItemId = Convert.ToInt32(ItNamer[0].ToString());
                                    }

                                    OdbcCommand cmd66 = new OdbcCommand("CALL savedata(?,?)", con);
                                    cmd66.CommandType = CommandType.StoredProcedure;
                                    cmd66.Parameters.AddWithValue("tblname", "m_inventory_kit");
                                    cmd66.Parameters.AddWithValue("val", "" + kid + "," + id1 + "," + NewItemId + "," + Convert.ToDecimal(dtitem.Rows[i]["Quantity"]) + "," + id + ",'" + dat + "'," + "0" + "," + id + ",'" + dat + "'");
                                    cmd66.Transaction = odbTrans;
                                    cmd66.ExecuteNonQuery();

                                }
                            }
                        }

                    }

                    odbTrans.Commit();
                    lblOk.Text = " Data saved successfully "; lblHead.Text = "Tsunami ARMS - confirmation";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    clear();
                    con.Close();                   
                    this.ScriptManager1.SetFocus(cmbItemCategory);
                    ViewState["option"] = "NIL";
                    ViewState["action"] = "NIL";
                }
                catch
                {
                    odbTrans.Rollback();
                    ViewState["action"] = "NILL";
                    okmessage("Tsunami ARMS - Warning", "Error in saving ");
                }
            #endregion

        }            

            else if (btnSave.Text == "Edit")
            {
                #region Edit
                
                date = DateTime.Now;
                dat = date.ToString("yyyy-MM-dd HH:mm:ss");
                id1 = Convert.ToInt32(Session["userid"].ToString());
                q = Convert.ToInt32(Session["row"].ToString());
                con = obje.NewConnection();
                int rowno;
                try
                {
                    odbTrans = con.BeginTransaction();
                    OdbcCommand cmd48 = new OdbcCommand("CALL selectdata(?,?)", con);
                    cmd48.CommandType = CommandType.StoredProcedure;
                    cmd48.Parameters.AddWithValue("tblname", "m_inventory_log");
                    cmd48.Transaction = odbTrans;
                    cmd48.Parameters.AddWithValue("attribute", "max(rowno) as rowno");

                    OdbcDataAdapter dacnt48 = new OdbcDataAdapter(cmd48);
                    DataTable dtt48 = new DataTable();
                    dacnt48.Fill(dtt48);
                    if (Convert.IsDBNull(dtt48.Rows[0]["rowno"]) == false)
                    {

                        rowno = Convert.ToInt32(dtt48.Rows[0]["rowno"]);
                        rowno = rowno + 1;

                    }
                    else
                    {
                        rowno = 1;

                    }
                    OdbcCommand cmd46 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                    cmd46.CommandType = CommandType.StoredProcedure;
                    cmd46.Parameters.AddWithValue("tblname", "m_inventory");
                    cmd46.Parameters.AddWithValue("attribute", "*");
                    cmd46.Parameters.AddWithValue("conditionv", "invent_id = '" + q + "'");
                    cmd46.Transaction = odbTrans;
                    OdbcDataAdapter dacnt46 = new OdbcDataAdapter(cmd46);
                    DataTable dtt46 = new DataTable();
                    dacnt46.Fill(dtt46);
                    OdbcCommand cmd31s = new OdbcCommand("CALL savedata(?,?)", con);
                    cmd31s.CommandType = CommandType.StoredProcedure;
                    cmd31s.Parameters.AddWithValue("tblname", "m_inventory_log");
                    DateTime ddddd = DateTime.Parse(dtt46.Rows[0]["createdon"].ToString());
                    string ggg = ddddd.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd31s.Parameters.AddWithValue("val", "" + rowno + "," + Convert.ToInt32(dtt46.Rows[0]["invent_id"]) + ",'" + dtt46.Rows[0]["essentiality"].ToString() + "','" + dtt46.Rows[0]["itemclass"] + "'," + Convert.ToInt32(dtt46.Rows[0]["reorderlevel"]) + ", " + Convert.ToInt32((dtt46.Rows[0]["econorderqnty"])) + "," + Convert.ToDecimal(dtt46.Rows[0]["dailyconsumption"]) + ",'" + dtt46.Rows[0]["itemmaker"].ToString() + "','" + dtt46.Rows[0]["itemmodel"].ToString() + "','" + dtt46.Rows[0]["itemcode"].ToString() + "'," + Convert.ToInt32(dtt46.Rows[0]["openingstock"]) + "," + Convert.ToInt32(dtt46.Rows[0]["openingstock"]) + "," + Convert.ToInt32(dtt46.Rows[0]["maxstocklevel"]) + "," + Convert.ToInt32(dtt46.Rows[0]["store_id"]) + ",'" + dtt46.Rows[0]["unit"].ToString() + "'," + Convert.ToInt32(dtt46.Rows[0]["createdby"]) + ",'" + ggg.ToString() + "'," + Convert.ToInt32(dtt46.Rows[0]["rowstatus"]) + "," + Convert.ToInt32(dtt46.Rows[0]["unit_id"]) + ",'" + dtt46.Rows[0]["control_slno"].ToString() + "'");
                    cmd31s.Transaction = odbTrans;
                    cmd31s.ExecuteNonQuery();
                    id1 = Convert.ToInt32(Session["userid"].ToString());
                    string Control = cmbSerialNo.SelectedItem.Text.ToString();
                    int YN;
                    if (Control == "Yes")
                    {
                        YN = 1;
                    }
                    else
                    {
                        YN = 0;
                    }
                    OdbcCommand cmd25 = new OdbcCommand("call updatedata(?,?,?)", con);
                    cmd25.CommandType = CommandType.StoredProcedure;
                    cmd25.Parameters.AddWithValue("tablename", "m_inventory");
                    //string bb = "itemcat_id=" + cmbItemCategory.SelectedValue.ToString() + ",item_id=" + cmbItemName.SelectedValue.ToString() + ",essentiality='" + cmbEssentialityLevel.SelectedItem.Text.ToString() + "',itemclass='" + cmbItemClass.SelectedItem.Text.ToString() + "',reorderlevel=" + int.Parse(txtReorLvl.Text) + ",econorderqnty=" + float.Parse(txtEconomicQty.Text) + ",dailyconsumption=" + decimal.Parse(txtDailyConsumption.Text) + ",itemmaker='" + txtItemMaker.Text.ToString() + "',itemmodel='" + txtItemModel.Text.ToString() + "',itemcode='" + txtItemid.Text.ToString() + "',openingstock=" + decimal.Parse(txtOpeningStock.Text) + ",stock_qty=" + decimal.Parse(txtOpeningStock.Text) + ",maxstocklevel=" + int.Parse(txtMaxStock.Text) + ",store_id=" + cmbStore.SelectedValue.ToString() + ",unit='" + txtUnit.Text.ToString() + "',createdby=" + id1 + ",createdon='" + dat + "',rowstatus=" + 1 + ",updatedby=" + id1 + ",updateddate='" + dat + "',unit_id=" + cmbUnit.SelectedValue.ToString() + ",control_slno='" + YN + "'";


                    cmd25.Parameters.AddWithValue("valu", "itemcat_id=" + cmbItemCategory.SelectedValue + ",item_id=" + cmbItemName.SelectedValue + ""
                     + ",essentiality='" + cmbEssentialityLevel.SelectedItem.Text + "',itemclass='" + cmbItemClass.SelectedItem.Text + "',"
                     + "reorderlevel=" + int.Parse(txtReorLvl.Text) + ",econorderqnty=" + int.Parse(txtEconomicQty.Text) + ",dailyconsumption="
                    + "" + int.Parse(txtDailyConsumption.Text) + ",itemmaker='" + txtItemMaker.Text.ToString() + "',itemmodel='" + txtItemModel.Text.ToString() + "',"
                    + "itemcode='" + txtItemid.Text.ToString() + "',openingstock=" + int.Parse(txtOpeningStock.Text) + ",stock_qty=" + int.Parse(txtOpeningStock.Text) + ","
                    + "maxstocklevel=" + int.Parse(txtMaxStock.Text) + ",store_id=" + cmbStore.SelectedValue + ",unit='" + txtUnit.Text + "',createdby=" + id1 + ","
                    + "createdon='" + dat + "',rowstatus=" + 1 + ",updatedby=" + id1 + ",updateddate='" + dat + "',unit_id=" + cmbUnit.SelectedValue + ","
                    + "control_slno='" + YN + "'");

                    cmd25.Parameters.AddWithValue("convariable", "invent_id=" + q + "");
                    cmd25.Transaction = odbTrans;
                    cmd25.ExecuteNonQuery();

                    OdbcCommand crr = new OdbcCommand("delete  from m_inventory_supplier where invent_id=" + q + "", con);
                    crr.Transaction = odbTrans;
                    crr.ExecuteNonQuery();

                    for (k = 0; k < lstSupplier.Items.Count; k++)
                    {

                        OdbcCommand cmd65a = new OdbcCommand("CALL savedata(?,?)", con);
                        cmd65a.CommandType = CommandType.StoredProcedure;
                        cmd65a.Transaction = odbTrans;
                        cmd65a.Parameters.AddWithValue("tblname", "m_inventory_supplier");
                        if (lstSupplier.Items[k].Selected == true)
                        {
                            OdbcCommand list1 = new OdbcCommand("select supplier_id from m_sub_supplier where suppliername='" + lstSupplier.Items[k].Text.ToString() + "' and rowstatus<>2", con);
                            list1.Transaction = odbTrans;
                            OdbcDataReader lisr1 = list1.ExecuteReader();
                            while (lisr1.Read())
                            {
                                listb = Convert.ToInt32(lisr1["supplier_id"].ToString());
                            }
                            cmd65a.Parameters.AddWithValue("val", "" + q + "," + listb + "," + id1 + ",'" + dat + "'");
                            cmd65a.ExecuteNonQuery();
                        }
                    }
                    int count;
                    if (cmbItemCategory.SelectedItem.Text == "Kit")
                    {

                        dtitem = (DataTable)Session["dtItem"];
                        for (int i = 0; i < dtitem.Rows.Count; i++)
                        {

                            DataRow[] drSpare = dtitem.Select("Itemcode='" + (dtitem.Rows[i]["Itemcode"]) + "'");
                            if (drSpare.Length > 0)
                            {
                                foreach (DataRow row in drSpare)
                                {
                                    String itname = dtitem.Rows[i]["Itemname"].ToString();
                                    OdbcCommand ItName5 = new OdbcCommand("SELECT item_id from m_sub_item where itemname='" + itname.ToString() + "' and rowstatus<>'2'", con);
                                    ItName5.Transaction = odbTrans;
                                    OdbcDataReader ItNamer5 = ItName5.ExecuteReader();
                                    if (ItNamer5.Read())
                                    {
                                        NewItemId = Convert.ToInt32(ItNamer5[0].ToString());

                                    }

                                    OdbcCommand Update = new OdbcCommand("select count(invent_id) from m_inventory_kit where invent_id=" + q + " and item_id=" + NewItemId + " "
                                                  + "and rowstatus<>2", con);
                                    Update.Transaction = odbTrans;
                                    OdbcDataReader Updater = Update.ExecuteReader();
                                    if (Updater.Read())
                                    {
                                        count = Convert.ToInt32(Updater[0].ToString());
                                        if (count > 0)
                                        {
                                            OdbcCommand Up = new OdbcCommand("update m_inventory_kit set qty=" + Convert.ToDecimal(dtitem.Rows[i]["Quantity"]) + ",rowstatus='1' where invent_id=" + q + " and item_id=" + NewItemId + "", con);
                                            Up.Transaction = odbTrans;
                                            Up.ExecuteNonQuery();

                                        }
                                        else
                                        {
                                            OdbcCommand cmd6l = new OdbcCommand("SELECT CASE WHEN max(invent_kit_id) IS NULL THEN 1 ELSE max(invent_kit_id)+1 END invent_kit_id from m_inventory_kit", con);//autoincrement donorid
                                            cmd6l.Transaction = odbTrans;
                                            int kid = Convert.ToInt32(cmd6l.ExecuteScalar());

                                            OdbcCommand cmd66q = new OdbcCommand("CALL savedata(?,?)", con);
                                            cmd66q.CommandType = CommandType.StoredProcedure;
                                            cmd66q.Parameters.AddWithValue("tblname", "m_inventory_kit");
                                            cmd66q.Parameters.AddWithValue("val", "" + kid + "," + q + "," + NewItemId + "," + Convert.ToDecimal(dtitem.Rows[i]["Quantity"]) + "," + id1 + ",'" + dat + "'," + "0" + "," + id1 + ",'" + dat + "'");
                                            cmd66q.Transaction = odbTrans;
                                            cmd66q.ExecuteNonQuery();
                                        }

                                    }

                                }
                            }
                        }
                    }
                    else
                    {

                    }


                    odbTrans.Commit();
                    clear();
                    lblOk.Text = " Data Updated successfully "; lblHead.Text = "Tsunami ARMS - confirmation";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    btnSave.Text = "Save";
                    clear();
                    con.Close();
                    this.ScriptManager1.SetFocus(cmbItemCategory);
               

                    ViewState["option"] = "NIL";
                    ViewState["action"] = "NIL";
                }
                catch
                {
                    odbTrans.Rollback();
                    ViewState["action"] = "NILL";
                    okmessage("Tsunami ARMS - Warning", "Error in Editing ");
                }
                #endregion
            }
        }
        else if (ViewState["action"].ToString() == "Delete")
        {
            #region delete
            DateTime date = DateTime.Now;
            string dat = date.ToString("yyyy-MM-dd") + " " + date.ToString("HH:mm:ss");
            con = obje.NewConnection();
            OdbcTransaction odbTrans = null;
            try
            {
                odbTrans = con.BeginTransaction();
                q = int.Parse(dtgInventoryDetails.SelectedRow.Cells[1].Text);
                id = Convert.ToInt32(Session["userid"].ToString());
                OdbcCommand cmd28 = new OdbcCommand("call updatedata(?,?,?)", con);
                cmd28.CommandType = CommandType.StoredProcedure;
                cmd28.Parameters.AddWithValue("tablename", "m_inventory");
                cmd28.Parameters.AddWithValue("valu", "rowstatus=2,updatedby=" + id + "");
                cmd28.Parameters.AddWithValue("convariable", "invent_id=" + q + "");
                cmd28.Transaction = odbTrans;
                cmd28.ExecuteNonQuery();
                OdbcCommand Del = new OdbcCommand("update m_inventory_kit set rowstatus='2' where invent_id=" + q + "", con);
                Del.Transaction = odbTrans;
                Del.ExecuteNonQuery();
                odbTrans.Commit();
                lblOk.Text = " Data Deleted Successfully "; lblHead.Text = "Tsunami ARMS - confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                clear();           

                ViewState["option"] = "NIL";
                ViewState["action"] = "NIL";
            }
            catch
            {
                odbTrans.Rollback();
                ViewState["action"] = "NILL";
                okmessage("Tsunami ARMS - Warning", "Error in Deleting ");
            }
            #endregion
        }

    }
    #endregion


    protected void lnkItemname_Click(object sender, EventArgs e)
    {

    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {

    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {

    }
    protected void LinkButton3_Click(object sender, EventArgs e)
    {

    }
    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnOk_Click(object sender, EventArgs e)
    {

        if (ViewState["action"].ToString() == "itemcode")
        {
            clear();
            this.ScriptManager1.SetFocus(cmbItemCategory);
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
    }
    protected void txtItemMaker_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtItemModel);
    }
    protected void txtItemModel_TextChanged(object sender, EventArgs e)
    {

    }

    #region EMPTY STRING
    public string emptystring(string s)
    {
        if (s == "")
        {
            s = null;
        }
        return s;
    }
    public string emptyinteger(string s)
    {
        if (s == "")
        {
            s = "0";
        }
        return s;
    }
    #endregion


    #region PRESS DELETE BUTTON
    protected void btnDelete_Click(object sender, EventArgs e)
    {        
        lblMsg.Text = "Do you want to Delete item?"; lblHead.Text = "Tsunami ARMS - Confirmation";
        ViewState["action"] = "Delete";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    #endregion

    protected void txtReorLvl_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbEssentialityLevel);
    }

    protected void btninvrpt_Click(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }

    #region GRID VIEW SELECTED
    protected void dtgInventoryDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        btnDelete.Enabled = true;
        btnSave.Text="Edit";
        q = int.Parse(dtgInventoryDetails.SelectedRow.Cells[1].Text);
        Session["row"] = q;

        con = obje.NewConnection();

        OdbcCommand cmd8 = new OdbcCommand();
        cmd8.CommandType = CommandType.StoredProcedure;
        cmd8.Parameters.AddWithValue("tblname", "m_inventory inv,m_sub_itemcategory mc,m_sub_item mi,m_sub_store st,m_sub_unit un");
        cmd8.Parameters.AddWithValue("attribute", "inv.invent_id as No,inv.itemcat_id,inv.item_id,inv.store_id,mc.itemcatname as Category,mi.itemname as Name,itemclass as Class,itemmaker as Maker,itemcode,itemmodel as Model,essentiality as Essentiality,unit as Min_unit,st.storename as storename,inv.reorderlevel,inv.econorderqnty,inv.dailyconsumption,openingstock,maxstocklevel,unit,un.unit_id,control_slno,unitname");
        cmd8.Parameters.AddWithValue("conditionv", "invent_id=" + q + " and inv.rowstatus<>2 and inv.store_id=st.store_id and inv.itemcat_id = mc.itemcat_id and inv.item_id=mi.item_id and un.unit_id=inv.unit_id group by invent_id");
        OdbcDataAdapter dacnt8 = new OdbcDataAdapter(cmd8);
        DataTable dtt8 = new DataTable();
        dtt8 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd8);

        for (int ii = 0; ii < dtt8.Rows.Count; ii++)
        {
            for (int jj = 0; jj < dtt8.Columns.Count; jj++)
            {
                btnDelete.Enabled = true;
                cmbItemCategory.SelectedItem.Text = dtt8.Rows[ii]["Category"].ToString();
                cmbItemCategory.SelectedValue = dtt8.Rows[ii]["itemcat_id"].ToString();

                cmbItemCategory_SelectedIndexChanged(null, null);

                cmbItemName.SelectedItem.Text = dtt8.Rows[ii]["Name"].ToString();
                cmbItemName.SelectedValue = dtt8.Rows[ii]["item_id"].ToString();

                cmbEssentialityLevel.SelectedValue = dtt8.Rows[ii]["essentiality"].ToString();
                txtReorLvl.Text = dtt8.Rows[ii]["reorderlevel"].ToString();

                decimal aa = decimal.Parse(dtt8.Rows[ii]["openingstock"].ToString());
                string ab = aa.ToString();

                if (ab.Contains(".") == true)
                {
                    string[] buildS1;
                    buildS1 = ab.Split('.');
                    build = buildS1[0];

                }

                txtOpeningStock.Text = build.ToString();

                decimal bb = decimal.Parse(dtt8.Rows[ii]["maxstocklevel"].ToString());
                string bc = bb.ToString();
                if (bc.Contains(".") == true)
                {
                    string[] buildS1;
                    buildS1 = bc.Split('.');
                    builda = buildS1[0];

                }

                txtMaxStock.Text = builda.ToString();

                txtEconomicQty.Text = dtt8.Rows[ii]["econorderqnty"].ToString();
                txtDailyConsumption.Text = dtt8.Rows[ii]["dailyconsumption"].ToString();
                txtItemMaker.Text = dtt8.Rows[ii]["Maker"].ToString();
                txtItemModel.Text = dtt8.Rows[ii]["Model"].ToString();
                txtItemid.Text = dtt8.Rows[ii]["itemcode"].ToString();
                cmbItemClass.SelectedValue = dtt8.Rows[ii]["Class"].ToString();
                txtUnit.Text = dtt8.Rows[ii]["unit"].ToString();
                cmbStore.SelectedItem.Text = dtt8.Rows[ii]["storename"].ToString();
                cmbStore.SelectedValue = dtt8.Rows[ii]["store_id"].ToString();
                cmbUnit.SelectedValue = dtt8.Rows[ii]["unit_id"].ToString();
                cmbUnit.SelectedItem.Text = dtt8.Rows[ii]["unitname"].ToString();
                try
                {
                    cmbSerialNo.SelectedValue = dtt8.Rows[ii]["control_slno"].ToString();
                    int Slno = Convert.ToInt32(dtt8.Rows[ii]["control_slno"].ToString());
                    if (Slno == 1)
                    {
                        cmbSerialNo.SelectedItem.Text = "Yes";
                    }
                    else if (Slno == 0)
                    {
                        cmbSerialNo.SelectedItem.Text = "No";
                    }
                }
                catch
                {

                }
                btnSave.Text = "Edit";
            }

        }

        OdbcCommand cmd49 = new OdbcCommand();
        cmd49.CommandType = CommandType.StoredProcedure;
        cmd49.Parameters.AddWithValue("tblname", "m_inventory_supplier ms,m_sub_supplier mp");
        cmd49.Parameters.AddWithValue("attribute", "distinct ms.supplier_id,mp.suppliername");
        cmd49.Parameters.AddWithValue("conditionv", "invent_id=" + q + " and rowstatus<>2 and ms.supplier_id=mp.supplier_id group by invent_id");
        OdbcDataAdapter dacnt = new OdbcDataAdapter(cmd49);
        DataTable dtt49 = new DataTable();
        dtt49 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd49);

        for (int ii = 0; ii < dtt49.Rows.Count; ii++)
        {
            string s = dtt49.Rows[ii]["suppliername"].ToString();
            for (k = 0; k < lstSupplier.Items.Count; k++)
            {
                if (lstSupplier.Items[k].Text == s)
                {
                    lstSupplier.Items[k].Selected = true;
                }
            }
        }

        q = Convert.ToInt32(Session["row"].ToString());

        if (cmbItemCategory.SelectedItem.Text == "Kit")
        {
            Pnlkitadd.Visible = true;
            dtgKitAdd.Visible = true;
            OdbcCommand Kit = new OdbcCommand();
            Kit.CommandType = CommandType.StoredProcedure;
            Kit.Parameters.AddWithValue("tblname", "m_inventory_kit k,m_sub_item i,m_inventory inv,m_sub_unit u");
            Kit.Parameters.AddWithValue("attribute", "k.invent_id,k.item_id,itemname,inv.itemcode,qty,unitname");
            Kit.Parameters.AddWithValue("conditionv", "k.item_id=inv.item_id and k.invent_id=" + q + " and inv.unit_id=u.unit_id and i.item_id=k.item_id and k.rowstatus<>2 and i.rowstatus<>2 and u.rowstatus<>2 and k.item_id=inv.item_id and inv.rowstatus<>'2' group by k.item_id");
            OdbcDataAdapter Kitr = new OdbcDataAdapter(Kit);
            DataTable dt5 = new DataTable();
            dt5 = obje.SpDtTbl("CALL selectcond(?,?,?)", Kit);        
            dtgKitAdd.DataSource = dt5;
            dtgKitAdd.DataBind();
            Session["Del"] = dtgKitAdd.DataSource;
            Delete = (DataTable)Session["Del"];
        }
        else
        { }
        btnDelete.Enabled = true;
    }
    #endregion

    #region GRIDVIEW ROWCREATED
    protected void dtgInventoryDetails_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#EFF3FB';");
            }
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgInventoryDetails, "Select$" + e.Row.RowIndex);
        }

    }
    #endregion

    #region GRIDVIEW SORTING
    protected void dtgInventoryDetails_Sorting(object sender, GridViewSortEventArgs e)
    {
        con = obje.NewConnection();

        if (dtgInventoryDetails.Caption == "Inventory details")
        {
            gridview();
        }
        else if (dtgInventoryDetails.Caption == "Inventory details itemcategory")
        {
            gridviewcategorychange();
        }

        if (dtt2 != null)
        {
            DataView dataView = new DataView(dtt2);
            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
            dtgInventoryDetails.DataSource = dataView;
            dtgInventoryDetails.DataBind();
        }


    }
    #endregion

    #region GRID VIEW PAGGE INDEX CHANGING
    protected void dtgInventoryDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        con = obje.NewConnection();
        dtgInventoryDetails.PageIndex = e.NewPageIndex;
        dtgInventoryDetails.DataBind();
        if (dtgInventoryDetails.Caption == "Inventory details")
        {
            gridview();
        }
        else if (dtgInventoryDetails.Caption == "Inventory details itemcategory")
        {
            gridviewcategorychange();
        }
    }
    #endregion

    protected void cmbcounter_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
    {

    }
    protected void cmbItemCategory_SelectedIndexChanged1(object sender, ComboBoxItemEventArgs e)
    {

    }

    protected void cmbrptcat_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void cmbItemName_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
    {

    }
    protected void txtItemModel_TextChanged1(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtDailyConsumption);
    }
    protected void txtDailyConsumption_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbStore);
    }
    protected void cmbStore_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
    {
        this.ScriptManager1.SetFocus(lstSupplier);
    }
    protected void txtEconomicQty_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtReorLvl);
    }
    protected void cmbEssentialityLevel_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
    {
        this.ScriptManager1.SetFocus(txtOpeningStock);
    }
    protected void txtMaxStock_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtItemid);
    }
    protected void txtItemid_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbItemClass);
        gridview();      
    }
    protected void cmbItemClass_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
    {
        this.ScriptManager1.SetFocus(txtUnit);
    }
    protected void txtUnit_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(btnSave);
    }
    protected void lstSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtEconomicQty);
    }
    protected void btnStock_Click(object sender, EventArgs e)
    {
        
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        
        pnlrpt.Visible = true;
        con = obje.NewConnection();
        OdbcDataAdapter Stock = new OdbcDataAdapter("select distinct storename as Sname,store_id from t_inventoryrequest inv,m_sub_store ms where "
               +" inv.office_request=ms.store_id and ms.rowstatus<>'2' UNION select distinct storename as Sname,store_id from t_inventoryrequest inv,"
               +" m_sub_store ms where inv.office_issue=ms.store_id and ms.rowstatus<>'2'", con);
        DataTable ds1 = new DataTable();
        DataColumn colID = ds1.Columns.Add("store_id", System.Type.GetType("System.Int32"));
        DataColumn colNo = ds1.Columns.Add("Sname", System.Type.GetType("System.String"));
        DataRow row = ds1.NewRow();
        row["store_id"] = "-1";
        row["Sname"] = "--Select--";
        ds1.Rows.InsertAt(row, 0);
        Stock.Fill(ds1);
        cmbStore1.DataSource = ds1;
        cmbStore1.DataBind();
        lnkStock.Visible = true;
        con.Close();        
    }


    protected void txtOpeningStock_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtMaxStock);
    }
    protected void lnkItCat_Click(object sender, EventArgs e)
    {
        Session["itcat"] = cmbItemCategory.SelectedValue.ToString();
        Session["itnam"] = cmbItemName.SelectedValue.ToString();
        Session["mak"] = txtItemMaker.Text.ToString();
        Session["mod"] = txtItemModel.Text.ToString();
        Session["dail"] = txtDailyConsumption.Text.ToString();
        Session["stor"] = cmbStore.SelectedValue.ToString();
        Session["eqty"] = txtEconomicQty.Text.ToString();
        Session["reorder"] = txtReorLvl.Text.ToString();
        Session["ess"] = cmbEssentialityLevel.SelectedItem.Text.ToString();
        Session["opstock"] = txtOpeningStock.Text.ToString();
        Session["mxstock"] = txtMaxStock.Text.ToString();
        Session["id"] = txtItemid.Text.ToString();
        Session["clss"] = cmbItemClass.SelectedValue.ToString();
        Session["cntr"] = cmbcounter.SelectedValue.ToString();
        Session["unit1"] = txtUnit.Text.ToString();
        Session["unit"] = cmbUnit.SelectedValue.ToString();
        Session["serial"] = cmbSerialNo.SelectedValue.ToString();
        int[] a = new int[20];
        for (int i = 0; i < lstSupplier.Items.Count; i++)
        {
            if (lstSupplier.Items[i].Selected == true)
            {
                a[i] = 1;
            }
            else
            {
                a[i] = 0;
            }

        }
        Session["supply"] = a;
        Session["itemcatgorylink"] = "yes";
        Session["item"] = "itemcategory";
        Response.Redirect("~/Submasters.aspx");
    }

    protected void lnkStore_Click(object sender, EventArgs e)
    {
        Session["itcat"] = cmbItemCategory.SelectedValue.ToString();
        Session["itnam"] = cmbItemName.SelectedValue.ToString();
        Session["mak"] = txtItemMaker.Text.ToString();
        Session["mod"] = txtItemModel.Text.ToString();
        Session["dail"] = txtDailyConsumption.Text.ToString();
        Session["stor"] = cmbStore.SelectedValue.ToString();
        Session["eqty"] = txtEconomicQty.Text.ToString();
        Session["reorder"] = txtReorLvl.Text.ToString();
        Session["ess"] = cmbEssentialityLevel.SelectedItem.Text.ToString();
        Session["opstock"] = txtOpeningStock.Text.ToString();
        Session["mxstock"] = txtMaxStock.Text.ToString();
        Session["id"] = txtItemid.Text.ToString();
        Session["clss"] = cmbItemClass.SelectedValue.ToString();
        Session["cntr"] = cmbcounter.SelectedValue.ToString();
        Session["unit1"] = txtUnit.Text.ToString();
        Session["unit"] = cmbUnit.SelectedValue.ToString();
        Session["serial"] = cmbSerialNo.SelectedValue.ToString();
        int[] a = new int[20];
        for (int i = 0; i < lstSupplier.Items.Count; i++)
        {
            if (lstSupplier.Items[i].Selected == true)
            {
                a[i] = 1;
            }
            else
            {
                a[i] = 0;
            }

        }
        Session["supply"] = a;

        Session["itemcatgorylink"] = "yes";
        Session["item"] = "storename";
        Response.Redirect("~/Submasters.aspx");
    }
    protected void lnkSupplier_Click(object sender, EventArgs e)
    {
        Session["itcat"] = cmbItemCategory.SelectedValue.ToString();
        Session["itnam"] = cmbItemName.SelectedValue.ToString();
        Session["mak"] = txtItemMaker.Text.ToString();
        Session["mod"] = txtItemModel.Text.ToString();
        Session["dail"] = txtDailyConsumption.Text.ToString();
        Session["stor"] = cmbStore.SelectedValue.ToString();
        Session["eqty"] = txtEconomicQty.Text.ToString();
        Session["reorder"] = txtReorLvl.Text.ToString();
        Session["ess"] = cmbEssentialityLevel.SelectedItem.Text.ToString();
        Session["opstock"] = txtOpeningStock.Text.ToString();
        Session["mxstock"] = txtMaxStock.Text.ToString();
        Session["id"] = txtItemid.Text.ToString();
        Session["clss"] = cmbItemClass.SelectedValue.ToString();
        Session["cntr"] = cmbcounter.SelectedValue.ToString();
        Session["unit1"] = txtUnit.Text.ToString();
        Session["unit"] = cmbUnit.SelectedValue.ToString();
        Session["serial"] = cmbSerialNo.SelectedValue.ToString();
        int[] a = new int[20];
        for (int i = 0; i < lstSupplier.Items.Count; i++)
        {
            if (lstSupplier.Items[i].Selected == true)
            {
                a[i] = 1;
            }
            else
            {
                a[i] = 0;
            }

        }
        Session["supply"] = a;

        Session["itemcatgorylink"] = "yes";
        Session["item"] = "supplier";
        Response.Redirect("~/Submasters.aspx");
    }
    protected void lnkitemcategory_Click(object sender, EventArgs e)
    {

    }
    protected void lnkItemname_Click1(object sender, EventArgs e)
    {
        Session["itcat"] = cmbItemCategory.SelectedValue.ToString();
        Session["itnam"] = cmbItemName.SelectedValue.ToString();
        Session["mak"] = txtItemMaker.Text.ToString();
        Session["mod"] = txtItemModel.Text.ToString();
        Session["dail"] = txtDailyConsumption.Text.ToString();
        Session["stor"] = cmbStore.SelectedValue.ToString();
        Session["eqty"] = txtEconomicQty.Text.ToString();
        Session["reorder"] = txtReorLvl.Text.ToString();
        Session["ess"] = cmbEssentialityLevel.SelectedItem.Text.ToString();
        Session["opstock"] = txtOpeningStock.Text.ToString();
        Session["mxstock"] = txtMaxStock.Text.ToString();
        Session["id"] = txtItemid.Text.ToString();
        Session["clss"] = cmbItemClass.SelectedValue.ToString();
        Session["cntr"] = cmbcounter.SelectedValue.ToString();
        Session["unit1"] = txtUnit.Text.ToString();
        Session["unit"] = cmbUnit.SelectedValue.ToString();
        Session["serial"] = cmbSerialNo.SelectedValue.ToString();
        int[] a = new int[20];
        for (int i = 0; i < lstSupplier.Items.Count; i++)
        {
            if (lstSupplier.Items[i].Selected == true)
            {
                a[i] = 1;
            }
            else
            {
                a[i] = 0;
            }

        }
        Session["supply"] = a;

        Session["itemcatgorylink"] = "yes";
        Session["item"] = "itemname";
        Response.Redirect("~/Submasters.aspx");
    }

    #region ADD ITEM
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        dtitem = (DataTable)Session["dtItem"];
        pnlAddDetails.Visible = true;
        dtgAddItems.Visible = true;
        int iRowCount = 0;
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }

        try
        {
            if (dtitem.Rows.Count > 0)
            {

                if (txtItemCode.Text != "")
                {

                    DataRow[] drItem = dtitem.Select("Itemcode='" + txtItemCode.Text.ToString() + "'");
                    if (drItem.Length > 0)
                    {
                        foreach (DataRow row in drItem)
                        {
                            iRowCount = row.Table.Rows.IndexOf(row);
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

                dtitem.Rows[iRowCount]["Itemcode"] = txtItemCode.Text.ToString();// SelectedItem.Text;// ddlCat1.SelectedValue;
                dtitem.Rows[iRowCount]["Itemname"] = cmbItemNm.SelectedItem.ToString();
                dtitem.Rows[iRowCount]["Measurement"] = txtUOM.Text.ToString();
                if (txtQnty.Text != "")
                {
                    dtitem.Rows[iRowCount]["Quantity"] = Convert.ToInt32(txtQnty.Text);
                }
                else
                {
                    dtitem.Rows[iRowCount]["Quantity"] = Convert.DBNull;
                }


            }

            else
            {
                if (txtItemCode.Text != "")
                {

                    iRowCount = dtitem.Rows.Count;
                    dtitem.Rows.Add();
                    dtitem.Rows[iRowCount]["Itemcode"] = txtItemCode.Text.ToString();
                    dtitem.Rows[iRowCount]["Itemname"] = cmbItemNm.SelectedItem.ToString();
                    dtitem.Rows[iRowCount]["Measurement"] = txtUOM.Text.ToString();
                    if (txtQnty.Text != "")
                    {
                        dtitem.Rows[iRowCount]["Quantity"] = Convert.ToInt32(txtQnty.Text);
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

        dtgAddItems.Visible = true;
        dtgAddItems.DataSource = dtitem;
        dtgAddItems.DataBind();

        Session["dtItem"] = dtgAddItems.DataSource;
        cmbItemCat.SelectedIndex = -1;
        cmbItemNm.SelectedIndex = -1;
        txtItemCode.Text = "";
        txtQnty.Text = "";
        txtUOM.Text = "";

    }
    #endregion

    #region ITEM NAME CHANGE
    protected void cmbItemNm_SelectedIndexChanged(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        OdbcCommand ItemMes = new OdbcCommand();
        ItemMes.CommandType = CommandType.StoredProcedure;
        ItemMes.Parameters.AddWithValue("tblname", "m_sub_unit u,m_inventory inv");
        ItemMes.Parameters.AddWithValue("attribute", "itemcode,unitname");
        ItemMes.Parameters.AddWithValue("conditionv", "item_id=" + cmbItemNm.SelectedValue.ToString() + " and inv.unit_id=u.unit_id and inv.rowstatus<>'2'");
        OdbcDataAdapter dacnt3 = new OdbcDataAdapter(ItemMes);
        DataTable dtt3 = new DataTable();
        dtt3 = obje.SpDtTbl("CALL selectcond(?,?,?)", ItemMes);

       foreach(DataRow dr1 in dtt3.Rows)
        {
            txtItemCode.Text = dr1["itemcode"].ToString();
            txtUOM.Text = dr1["unitname"].ToString();
        }
    }
    #endregion

    #region ITEMCATEGORY CHANGED
    protected void cmbItemCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbItemName.Items.Clear();
        txtItemid.Text = "";
        cmbStore.SelectedIndex = -1;
        Pnlkitadd.Visible = false;
        con = obje.NewConnection();
        if (cmbItemCategory.SelectedValue == "")
        {
            gridview();
            this.ScriptManager1.SetFocus(cmbItemCategory);
        }
        else
        {
            con = obje.NewConnection();
            OdbcCommand cm1 = new OdbcCommand();
            cm1.CommandType = CommandType.StoredProcedure;
            cm1.Parameters.AddWithValue("tblname", "m_sub_item");
            cm1.Parameters.AddWithValue("attribute", "itemname,item_id");
            cm1.Parameters.AddWithValue("conditionv", "itemcat_id=" + cmbItemCategory.SelectedValue + " and rowstatus<>2 order by itemname asc");
            OdbcDataAdapter cm16 = new OdbcDataAdapter(cm1);
            DataTable ds = new DataTable();
            ds = obje.SpDtTbl("CALL selectcond(?,?,?)", cm1);
            DataRow row = ds.NewRow();
            ds.Rows.InsertAt(row, 0);
            row["item_id"] = "-1";
            row["itemname"] = "--Select--";
            cmbItemName.DataSource = ds;
            cmbItemName.DataBind();
           

            this.ScriptManager1.SetFocus(txtItemid);

            cmbEssentialityLevel.SelectedIndex = -1;
            txtReorLvl.Text = "";
            txtOpeningStock.Text = "";
            txtEconomicQty.Text = "";
            txtDailyConsumption.Text = "";
            txtItemMaker.Text = "";
            txtItemModel.Text = "";
            cmbItemClass.SelectedIndex = -1;
            txtMaxStock.Text = "";
            cmbitmcatrpt.SelectedIndex = -1;
            cmbsuprpt.SelectedIndex = -1;
            cmbesnltylvlrpt.SelectedIndex = -1;
            cmbcounter.SelectedIndex = -1;
            cmbrptcat.SelectedIndex = -1;
            dtgInventoryDetails.Visible = true;
            cmbitmcatrpt.Enabled = false;
            cmbesnltylvlrpt.Enabled = false;
            cmbsuprpt.Enabled = false;
            cmbStore.SelectedIndex = -1;
            txtUnit.Text = "";
            txtUnit.Text = "";

            lstSupplier.Items.Clear();
            cmbUnit.SelectedIndex = -1;
            cmbSerialNo.SelectedIndex = -1;
            pnlKitItems.Visible = false;
            pnlAddDetails.Visible = false;
            cmbStore.SelectedIndex = -1;
            cmbSerialNo.SelectedIndex = -1;
            
            OdbcCommand cmd3 = new OdbcCommand();
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Parameters.AddWithValue("tblname", "m_sub_supplier");
            cmd3.Parameters.AddWithValue("attribute", "supplier_id,suppliername");
            cmd3.Parameters.AddWithValue("conditionv", "rowstatus<>2");
            OdbcDataAdapter dacnt3 = new OdbcDataAdapter(cmd3);
            DataTable dtt3 = new DataTable();
            dtt3 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd3);

            for (int ii = 0; ii < dtt3.Rows.Count; ii++)
            {
                lstSupplier.Items.Add(dtt3.Rows[ii][0].ToString());
                lstSupplier.Items[ii].Text = dtt3.Rows[ii][1].ToString();
                lstSupplier.Items[ii].Value = dtt3.Rows[ii][0].ToString();
            }

            btnDelete.Enabled = false;
            btnSave.Text = "Save";
            con.Close();

        }
        if (cmbItemCategory.SelectedItem.Text == "Kit")
        {
            pnlKitItems.Visible = true;
            con = obje.NewConnection();

            OdbcCommand Stored = new OdbcCommand();
            Stored.CommandType = CommandType.StoredProcedure;
            Stored.Parameters.AddWithValue("tblname", "m_sub_itemcategory ");
            Stored.Parameters.AddWithValue("attribute", "itemcat_id,itemcatname");
            Stored.Parameters.AddWithValue("conditionv", "rowstatus<>'2' and itemcatname<>'Kit' "
                             + "and itemcat_id IN (select itemcat_id from m_inventory where rowstatus<>2)order by itemcatname asc");
            OdbcDataAdapter Stored4 = new OdbcDataAdapter(Stored);
            DataTable dsd = new DataTable();
            dsd = obje.SpDtTbl("CALL selectcond(?,?,?)", Stored);
            DataRow rowd = dsd.NewRow();
            dsd.Rows.InsertAt(rowd, 0);
            rowd["itemcat_id"] = "-1";
            rowd["itemcatname"] = "--Select--";
            cmbItemCat.DataSource = dsd;
            cmbItemCat.DataBind();
            con.Close();
        }
        else
        {
            pnlKitItems.Visible = false;
        }
        con.Close();
    }
    #endregion

    #region LINK BUTTON
    protected void lnkUnit_Click(object sender, EventArgs e)
    {
        Session["itcat"] = cmbItemCategory.SelectedValue.ToString();
        Session["itnam"] = cmbItemName.SelectedValue.ToString();
        Session["mak"] = txtItemMaker.Text.ToString();
        Session["mod"] = txtItemModel.Text.ToString();
        Session["dail"] = txtDailyConsumption.Text.ToString();
        Session["stor"] = cmbStore.SelectedValue.ToString();
        Session["eqty"] = txtEconomicQty.Text.ToString();
        Session["reorder"] = txtReorLvl.Text.ToString();
        Session["ess"] = cmbEssentialityLevel.SelectedItem.Text.ToString();
        Session["opstock"] = txtOpeningStock.Text.ToString();
        Session["mxstock"] = txtMaxStock.Text.ToString();
        Session["id"] = txtItemid.Text.ToString();
        Session["clss"] = cmbItemClass.SelectedValue.ToString();
        Session["cntr"] = cmbcounter.SelectedValue.ToString();
        Session["unit1"] = txtUnit.Text.ToString();
        Session["unit"] = cmbUnit.SelectedValue.ToString();
        Session["serial"] = cmbSerialNo.SelectedValue.ToString();
        int[] a = new int[20];
        for (int i = 0; i < lstSupplier.Items.Count; i++)
        {
            if (lstSupplier.Items[i].Selected == true)
            {
                a[i] = 1;
            }
            else
            {
                a[i] = 0;
            }

        }
        Session["supply"] = a;

        Session["itemcatgorylink"] = "yes";
        Session["item"] = "unitname";
        Response.Redirect("~/Submasters.aspx");
    }
    #endregion

    #region ITEMNAME CHANGED
    protected void cmbItemName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        OdbcCommand ItemCode = new OdbcCommand("SELECT itemcode FROM m_inventory WHERE item_id=" + cmbItemName.SelectedValue + " and rowstatus<>'2'", con);
        OdbcDataReader Itemr = ItemCode.ExecuteReader();
        if (Itemr.Read())
        {
            txtItemid.Text = Itemr[0].ToString();
            ViewState["action"] = "itemcode";
        }
        else
        {
            txtItemid.Text = "";
        }
    
        this.ScriptManager1.SetFocus(txtItemid);
        con.Close();
    }
    #endregion

    #region STORE SELECTED

    protected void cmbStore_SelectedIndexChanged1(object sender, EventArgs e)
    {

        con = obje.NewConnection();
        btnSave.Text = "Save";
        OdbcCommand Supp = new OdbcCommand("SELECT invent_id FROM m_inventory WHERE item_id=" + cmbItemName.SelectedValue.ToString() + " and "
                                           + "store_id=" + cmbStore.SelectedValue.ToString() + " and rowstatus<>'2'", con);
        OdbcDataReader Suppr = Supp.ExecuteReader();
        if (Suppr.Read())
        {

            OdbcCommand NewSupp = new OdbcCommand();
            NewSupp.CommandType = CommandType.StoredProcedure;
            NewSupp.Parameters.AddWithValue("tblname", "m_inventory inv,m_sub_itemcategory mc,m_sub_item mi,m_sub_store st,m_sub_unit un,m_inventory_supplier ms,"
                                               + "m_sub_supplier mp ");
            NewSupp.Parameters.AddWithValue("attribute", "inv.invent_id as No,inv.itemcat_id,inv.item_id,inv.store_id,mc.itemcatname as Category,mi.itemname "
                                               + "as Name,itemclass as Class,itemmaker as Maker,itemcode,itemmodel as Model,essentiality as Essentiality,unit "
                                               + "as Min_unit,st.storename as storename,inv.reorderlevel,inv.econorderqnty,inv.dailyconsumption,openingstock,"
                                               + "maxstocklevel,unit,un.unit_id,control_slno,unitname,ms.supplier_id,mp.suppliername");
            NewSupp.Parameters.AddWithValue("conditionv", "inv.rowstatus<>2 and inv.store_id=st.store_id and inv.itemcat_id = mc.itemcat_id and inv.item_id="
                                               + "mi.item_id and inv.item_id=" + cmbItemName.SelectedValue.ToString() + " and inv.store_id=" + cmbStore.SelectedValue.ToString() + " "
                                               + " and ms.supplier_id=mp.supplier_id and ms.invent_id=inv.invent_id group by inv.invent_id");
            OdbcDataAdapter NewItem = new OdbcDataAdapter(NewSupp);
            DataTable dt = new DataTable();
            dt = obje.SpDtTbl("CALL selectcond(?,?,?)", NewSupp);
                       

            for (int ii = 0; ii < dt.Rows.Count; ii++)
            {
                for (int jj = 0; jj < dt.Columns.Count; jj++)
                {
                    btnDelete.Enabled = true;
                    Session["row"] = int.Parse(dt.Rows[ii]["No"].ToString());
                    cmbEssentialityLevel.SelectedValue = dt.Rows[ii]["essentiality"].ToString();
                    txtReorLvl.Text = dt.Rows[ii]["reorderlevel"].ToString();
                    txtOpeningStock.Text = dt.Rows[ii]["openingstock"].ToString();
                    txtEconomicQty.Text = dt.Rows[ii]["econorderqnty"].ToString();
                    txtDailyConsumption.Text = dt.Rows[ii]["dailyconsumption"].ToString();
                    txtItemMaker.Text = dt.Rows[ii]["Maker"].ToString();
                    txtItemModel.Text = dt.Rows[ii]["Model"].ToString();
                    txtItemid.Text = dt.Rows[ii]["itemcode"].ToString();
                    cmbItemClass.SelectedValue = dt.Rows[ii]["Class"].ToString();

                    decimal aa = decimal.Parse(dt.Rows[ii]["openingstock"].ToString());
                    string ab = aa.ToString();

                    if (ab.Contains(".") == true)
                    {
                        string[] buildS1;
                        buildS1 = ab.Split('.');
                        build = buildS1[0];

                    }
                    txtOpeningStock.Text = build.ToString();

                    decimal bb = decimal.Parse(dt.Rows[ii]["maxstocklevel"].ToString());
                    string bc = bb.ToString();
                    if (bc.Contains(".") == true)
                    {
                        string[] buildS1;
                        buildS1 = bc.Split('.');
                        builda = buildS1[0];

                    }

                    txtMaxStock.Text = builda.ToString();
                    txtUnit.Text = dt.Rows[ii]["unit"].ToString();
                    cmbStore.SelectedItem.Text = dt.Rows[ii]["storename"].ToString();
                    cmbStore.SelectedValue = dt.Rows[ii]["store_id"].ToString();
                    cmbUnit.SelectedValue = dt.Rows[ii]["unit_id"].ToString();
                    cmbUnit.SelectedItem.Text = dt.Rows[ii]["unitname"].ToString();
                    try
                    {
                        cmbSerialNo.SelectedValue = dt.Rows[ii]["control_slno"].ToString();
                        int Slno = Convert.ToInt32(dt.Rows[ii]["control_slno"].ToString());
                        if (Slno == 1)
                        {
                            cmbSerialNo.SelectedItem.Text = "Yes";
                        }
                        else if (Slno == 0)
                        {
                            cmbSerialNo.SelectedItem.Text = "No";
                        }
                    }
                    catch
                    {

                    }

                    string s = dt.Rows[ii]["suppliername"].ToString();

                    for (int k = 0; k < lstSupplier.Items.Count; k++)
                    {

                        if (lstSupplier.Items[k].Text == s)
                        {
                            lstSupplier.Items[k].Selected = true;
                        }
                    }

                    btnSave.Text = "Edit";
                }

            }

        }
        else
        {
           
        }

    }
    #endregion

    protected void lnkStock_Click(object sender, EventArgs e)
    {
        lblStore.Visible = true;
        cmbStore1.Visible = true;
        lblItemName1.Visible = true;
        cmbStoreN.Visible = true;
        btnStockL.Visible = true;

    }

    #region STOCK LEDGER REPORT
    protected void btnStockL_Click(object sender, EventArgs e)
    {
        if (cmbStore1.SelectedValue == "-1")
        {
            lblOk.Text = "Please Select a store"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        if (cmbStoreN.SelectedValue == "-1")
        {
            lblOk.Text = "Please Select Item"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        con = obje.NewConnection();
        DateTime ds2 = DateTime.Now;
        string num;
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "StockLedger" + transtim.ToString() + ".pdf";


        string datte = ds2.ToString("dd-MM-yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        string timme = ds2.ToShortTimeString();
        string datte1 = ds2.ToString("dd MMMM yyyy");
        string dat4 = ds2.ToString("dd-MM-yyyy");
               
         decimal OpenSt;

         OdbcCommand Stock = new OdbcCommand();
         Stock.CommandType = CommandType.StoredProcedure;
         Stock.Parameters.AddWithValue("tblname", "m_inventory inv,t_inventoryrequest t,"
                              + "t_inventoryrequest_items item,t_inventoryrequest_items_issue iss,t_inventoryrequest_issue ri,m_sub_store s");
         Stock.Parameters.AddWithValue("attribute", "iss.issueno,t.reqno,itemcode,openingstock,iss.issued_qty,received_qty,start_slno,end_slno,iss.item_id,"
                              + "office_request,office_issue,inv.createdon as opend,ri.createdon as isdate");
         Stock.Parameters.AddWithValue("conditionv", "t.reqno=item.reqno and (iss.item_id=" + cmbStoreN.SelectedValue.ToString() + " or item.item_id=" + cmbStoreN.SelectedValue.ToString() + ")"
                       + "and inv.item_id=item.item_id and ri.issueno=iss.issueno and iss.item_id=item.item_id and (s.store_id=office_request or "
                       + "s.store_id=office_issue) and iss.item_id=inv.item_id and ri.reqno=t.reqno and (office_request=" + cmbStore1.SelectedValue.ToString() + " or "
                       + "office_issue=" + cmbStore1.SelectedValue.ToString() + ") group by t.reqno");
         OdbcDataAdapter StockAda = new OdbcDataAdapter(Stock);
         DataTable db = new DataTable();
         db = obje.SpDtTbl("CALL selectcond(?,?,?)", Stock);

         #region COMMENTED*************
         //OdbcCommand Stock = new OdbcCommand("select iss.issueno,t.reqno,itemcode,openingstock,iss.issued_qty,received_qty,start_slno,end_slno,iss.item_id,"
        //               + "office_request,office_issue,inv.createdon as opend,ri.createdon as isdate from m_inventory inv,t_inventoryrequest t,"
        //               + "t_inventoryrequest_items item,t_inventoryrequest_items_issue iss,t_inventoryrequest_issue ri,m_sub_store s where "
        //               + "t.reqno=item.reqno and (iss.item_id=" + cmbStoreN.SelectedValue.ToString() + " or item.item_id=" + cmbStoreN.SelectedValue.ToString() + ")"
        //               + "and inv.item_id=item.item_id and ri.issueno=iss.issueno and iss.item_id=item.item_id and (s.store_id=office_request or "
        //               + "s.store_id=office_issue) and iss.item_id=inv.item_id and ri.reqno=t.reqno and (office_request=" + cmbStore1.SelectedValue.ToString() + " or "
        //               + "office_issue=" + cmbStore1.SelectedValue.ToString() + ") group by t.reqno", con);

        //OdbcDataAdapter StockAda = new OdbcDataAdapter(Stock);
        //DataTable db = new DataTable();
         //StockAda.Fill(db);
         #endregion

        decimal Cou = 0;
        for (int k = 0; k < db.Rows.Count; k++)
        {
            decimal Amo = decimal.Parse(db.Rows[k]["received_qty"].ToString());
            Cou = Cou + Amo;
        }
        con = obje.NewConnection();
        OdbcCommand Rstatus1 = new OdbcCommand("DROP VIEW if exists tempstockledger", con);
        Rstatus1.ExecuteNonQuery();
        OdbcCommand StockLed = new OdbcCommand("CREATE VIEW tempstockledger as select iss.issueno,t.reqno,itemcode,openingstock,iss.issued_qty,received_qty,start_slno,end_slno,iss.item_id,"
                   + "office_request,office_issue,inv.createdon as opend,ri.createdon as isdate from m_inventory inv,t_inventoryrequest t,"
                   + "t_inventoryrequest_items item,t_inventoryrequest_items_issue iss,t_inventoryrequest_issue ri,m_sub_store s where "
                   + "t.reqno=item.reqno and (iss.item_id=" + cmbStoreN.SelectedValue.ToString() + " or item.item_id=" + cmbStoreN.SelectedValue.ToString() + ")"
                   + "and inv.item_id=item.item_id and ri.issueno=iss.issueno and iss.item_id=item.item_id and (s.store_id=office_request or "
                   + "s.store_id=office_issue) and iss.item_id=inv.item_id and ri.reqno=t.reqno and (office_request=" + cmbStore1.SelectedValue.ToString() + " or "
                   + "office_issue=" + cmbStore1.SelectedValue.ToString() + ") group by t.reqno order by isdate asc", con);
        StockLed.ExecuteNonQuery();
        OdbcCommand StockLed1 = new OdbcCommand("ALTER VIEW tempstockledger as select iss.issueno,t.reqno,itemcode,openingstock,iss.issued_qty,received_qty,start_slno,end_slno,iss.item_id,"
                   + "office_request,office_issue,inv.createdon as opend,ri.createdon as isdate from m_inventory inv,t_inventoryrequest t,"
                   + "t_inventoryrequest_items item,t_inventoryrequest_items_issue iss,t_inventoryrequest_issue ri,m_sub_store s where "
                   + "t.reqno=item.reqno and (iss.item_id=" + cmbStoreN.SelectedValue.ToString() + " or item.item_id=" + cmbStoreN.SelectedValue.ToString() + ")"
                   + "and inv.item_id=item.item_id and ri.issueno=iss.issueno and iss.item_id=item.item_id and (s.store_id=office_request or "
                   + "s.store_id=office_issue) and iss.item_id=inv.item_id and ri.reqno=t.reqno and (office_request=" + cmbStore1.SelectedValue.ToString() + " or "
                   + "office_issue=" + cmbStore1.SelectedValue.ToString() + ") group by t.reqno order by isdate asc", con);

        StockLed1.ExecuteNonQuery();
     
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

        OdbcCommand cmd456 = new OdbcCommand("select * from tempstockledger", con);
        OdbcDataAdapter dacnt456 = new OdbcDataAdapter(cmd456);
        DataTable dt = new DataTable();
        dacnt456.Fill(dt);

        for (int ii = 0; ii < dt.Rows.Count; ii++)
        {
            code = dt.Rows[ii]["itemcode"].ToString();
            break;
        }

        PdfPTable table1 = new PdfPTable(8);
        float[] colwidth1 ={ 2, 3, 5, 3, 3, 3, 3, 1 };
        table1.SetWidths(colwidth1);
        table1.TotalWidth = 650f;

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Stock Ledger", font10)));
        cell.Colspan = 8;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table1.AddCell(cell);

        try
        {
            PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Store name: " + cmbStore1.SelectedItem.Text.ToString(), font11)));
            cella.Colspan = 4;
            cella.Border = 0;
            cella.HorizontalAlignment = 0;
            table1.AddCell(cella);
            PdfPCell cellb = new PdfPCell(new Phrase(new Chunk("Item Name: " + cmbStoreN.SelectedItem.Text.ToString(), font11)));
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
                //OpenStock =Convert.ToInt32(OpenStock1.ToString());
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
                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }

                    string StReq = dr["issueno"].ToString();
                    int ItNa = Convert.ToInt32(dr["item_id"].ToString());

                    OdbcCommand Recp = new OdbcCommand();
                    Recp.CommandType = CommandType.StoredProcedure;
                    Recp.Parameters.AddWithValue("tblname", "t_grn g,t_grn_items gi,t_inventoryrequest_items_issue iss");
                    Recp.Parameters.AddWithValue("attribute", "distinct receive_qty,g.receivedon as rdate,g.grnno,start_slno,end_slno");
                    Recp.Parameters.AddWithValue("conditionv", "gi.grnno=g.grnno and g.refno='" + StReq.ToString() + "' and gi.item_id=" + ItNa + " and iss.issueno=g.refno");
                    OdbcDataAdapter Recr = new OdbcDataAdapter(Recp);
                    DataTable db1 = new DataTable();
                    db1 = obje.SpDtTbl("CALL selectcond(?,?,?)", Recp);

                    #region COMMENTED*************
                    //OdbcCommand Recp = new OdbcCommand("SELECT distinct receive_qty,g.receivedon as rdate,g.grnno,start_slno,end_slno from t_grn g,t_grn_items gi,t_inventoryrequest_items_issue iss where "
                    //               + "gi.grnno=g.grnno and g.refno='" + StReq.ToString() + "' and gi.item_id=" + ItNa + " and iss.issueno=g.refno", con);

                    //OdbcDataReader Recr = Recp.ExecuteReader();
                    //if (Recr.Read())
                    #endregion

                    foreach (DataRow dr4 in db1.Rows)
                    {
                        DateTime Date1 = DateTime.Parse(dr4["rdate"].ToString());
                        Ddate1 = Date1.ToString("dd MMM yyyy");
                        Rqt2 = Convert.ToDecimal(dr4["receive_qty"].ToString());
                        GName = dr4["grnno"].ToString();
                        sl = Convert.ToInt32(dr4["start_slno"].ToString());
                        endl = Convert.ToInt32(dr4["end_slno"].ToString());

                    }

                    PdfPCell cell33a = new PdfPCell(new Phrase(new Chunk(num, font8)));
                    table.AddCell(cell33a);

                    PdfPCell cell33b = new PdfPCell(new Phrase(new Chunk(Ddate1.ToString(), font8)));
                    table.AddCell(cell33b);
                    int iss1 = Convert.ToInt32(dr["office_issue"].ToString());


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
            int it1, it2;
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
                            cell33n.Colspan = 3;
                            table.AddCell(cell33n);
                        }

                        else
                        {
                            try
                            {

                                PdfPCell cell33n = new PdfPCell(new Phrase(new Chunk("Sl no " + st.ToString() + " - " + en.ToString(), font8)));
                                cell33n.Colspan = 3;
                                table.AddCell(cell33n);

                            }
                            catch
                            {
                                PdfPCell cell33n = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                                cell33n.Colspan = 3;
                                table.AddCell(cell33n);
                            }

                        }


                    }
                    catch
                    {

                        PdfPCell cell33n = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                        cell33n.Colspan = 3;
                        table.AddCell(cell33n);
                    }
                }

                it2 = Convert.ToInt32(dr["item_id"].ToString());
            }
            catch (Exception ex)
            {

            }
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

        doc.Add(table);
        doc.Add(table5);
        doc.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Stock Ledger report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        con.Close();

    }
    #endregion

    #region STORE SELECTED
    protected void cmbStore1_SelectedIndexChanged(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        OdbcCommand StockIt = new OdbcCommand();
        StockIt.CommandType = CommandType.StoredProcedure;
        StockIt.Parameters.AddWithValue("tblname", "m_sub_item it,m_inventory inv");
        StockIt.Parameters.AddWithValue("attribute", "itemname,inv.item_id");
        StockIt.Parameters.AddWithValue("conditionv", "inv.store_id=" + cmbStore1.SelectedValue.ToString() + " and inv.item_id=it.item_id and inv.rowstatus<>'2'");
        OdbcDataAdapter StockIt6 = new OdbcDataAdapter(StockIt);
        DataTable ds1 = new DataTable();
        ds1 = obje.SpDtTbl("CALL selectcond(?,?,?)", StockIt);
        DataRow row = ds1.NewRow();
        ds1.Rows.InsertAt(row, 0);
        row["item_id"] = "-1";
        row["itemname"] = "--Select--";
        cmbStoreN.DataSource = ds1;
        cmbStoreN.DataBind();
        con.Close();
    }
    #endregion

    #region KIT ITEM GRID SELECTED****************
    protected void dtgKitAdd_SelectedIndexChanged(object sender, EventArgs e)
    {

        int q, it;
        con = obje.NewConnection();

        q = Convert.ToInt32(dtgKitAdd.DataKeys[dtgKitAdd.SelectedRow.RowIndex].Values[0].ToString());
        it = Convert.ToInt32(dtgKitAdd.DataKeys[dtgKitAdd.SelectedRow.RowIndex].Values[1].ToString());
        ViewState["q"] = q;
        ViewState["it"] = it;

        OdbcCommand Grid = new OdbcCommand();
        Grid.CommandType = CommandType.StoredProcedure;
        Grid.Parameters.AddWithValue("tblname", "m_inventory_kit k,m_sub_item i,m_inventory inv,m_sub_unit u");
        Grid.Parameters.AddWithValue("attribute", "k.invent_id,k.item_id,itemname,itemcode,qty,unitname,inv.itemcat_id as itemcat_id");
        Grid.Parameters.AddWithValue("conditionv", "k.item_id=inv.item_id and k.rowstatus<>2 and i.rowstatus<>2 and inv.rowstatus<>2 and "
                          + "inv.unit_id=u.unit_id and u.rowstatus<>2 and k.invent_id=" + q + " and k.item_id=" + it + " and i.item_id=k.item_id");
        OdbcDataAdapter Grid6 = new OdbcDataAdapter(Grid);
        DataTable ds2 = new DataTable();
        ds2 = obje.SpDtTbl("CALL selectcond(?,?,?)", Grid);

        foreach (DataRow dr6 in ds2.Rows)
        {
            cmbItemCat.SelectedItem.Text = "Kit";
            cmbItemNm.Items.Clear();
            cmbItemNm.Items.Add(dr6["itemname"].ToString());
            txtItemCode.Text = dr6["itemcode"].ToString();
            txtQnty.Text = dr6["qty"].ToString();
            txtUOM.Text = dr6["unitname"].ToString();
        }

    }
    #endregion

    #region   KIT ITEM ROW CREATED****************
    protected void dtgKitAdd_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#EFF3FB';");
            }
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgKitAdd, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    #region item delete

    protected void itemDelete(Object sender, CommandEventArgs e)
    {
        try
        {

            DataTable invt = (DataTable)Session["dtItem"];
            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                string strItems = e.CommandArgument.ToString();
                //DataRow[] drint = invt.Select("item_id=" + Convert.ToInt32(strItems[0].ToString()) + " and task_id=" + Convert.ToInt32(strItems[1].ToString()) + "");

                DataRow[] drint = invt.Select("itemcode='" + strItems.ToString() + "'");

                if (drint.Length > 0)
                {
                    foreach (DataRow dr in drint)
                    {

                        invt.Rows.Remove(drint[0]);
                        Session["dtItem"] = invt;
                        this.dtgAddItems.DataSource = ((DataTable)Session["dtItem"]);
                        this.dtgAddItems.DataBind();

                    }
                }


            }

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

    protected void cmbItemCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region Kit - item category selected index change
        con = obje.NewConnection();
        txtItemCode.Text = "";
        txtUOM.Text = "";
        txtQnty.Text = "";

        OdbcCommand Store15 = new OdbcCommand();
        Store15.CommandType = CommandType.StoredProcedure;
        Store15.Parameters.AddWithValue("tblname", "m_sub_itemcategory it,m_sub_item s,m_inventory inv");
        Store15.Parameters.AddWithValue("attribute", "itemname,inv.item_id");
        Store15.Parameters.AddWithValue("conditionv", "inv.itemcat_id= " + cmbItemCat.SelectedValue.ToString() + "  and s.itemcat_id=it.itemcat_id and s.rowstatus<>'2' and it.rowstatus<>'2' and inv.item_id=s.item_id and it.itemcat_id=inv.itemcat_id");
        OdbcDataAdapter NewItem = new OdbcDataAdapter(Store15);
        DataTable ds1 = new DataTable();
        ds1 = obje.SpDtTbl("CALL selectcond(?,?,?)", Store15);
        DataRow row = ds1.NewRow();
        ds1.Rows.InsertAt(row, 0);
        row["item_id"] = "-1";
        row["itemname"] = "--Select--";
        cmbItemNm.DataSource = ds1;
        cmbItemNm.DataBind();
        con.Close();
             
        #endregion
    }
    protected void lnkKitDelete_Click(object sender, EventArgs e)
    {

    }

    #region KIT ITEM DELETE***************
    public void ItemDelete2(Object sender, CommandEventArgs e)
    {
        con = obje.NewConnection();
        try
        {
            DataTable ItemDel = (DataTable)Session["Del"];
            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                string strItems = e.CommandArgument.ToString();
                DataRow[] drint = ItemDel.Select("itemcode='" + strItems.ToString() + "'");

                if (drint.Length > 0)
                {
                    foreach (DataRow dr in drint)
                    {
                        it1 = Convert.ToInt32(dr["item_id"].ToString());
                        q1 = Convert.ToInt32(dr["invent_id"].ToString());
                        OdbcCommand DelKit = new OdbcCommand("update m_inventory_kit set rowstatus='2' where invent_id=" + q1 + " and item_id=" + it1 + "", con);
                        DelKit.ExecuteNonQuery();

                        ItemDel.Rows.Remove(drint[0]);
                        Session["Del"] = ItemDel;
                        this.dtgKitAdd.DataSource = ((DataTable)Session["Del"]);
                        this.dtgKitAdd.DataBind();
                    }


                }
            }
        }
        catch
        { 
        }
        con.Close();
    }
    #endregion

    protected void dtgAddItems_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}

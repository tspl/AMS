/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMMODATION
// Screen Name      :      Room Resource Register
// Form Name        :      Room Resource Register.aspx
// ClassFile Name   :      RoomResourceRegister.aspx.cs
// Purpose          :      create master for room with inventory details
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
using System.Web.UI.WebControls;
using clsDAL;
public partial class Default2 : System.Web.UI.Page
{
    #region INITIALIZATION
    OdbcConnection conn = new OdbcConnection();//("Driver={MySQL ODBC 3.51 Driver};database=tdbnew;option=0;port=3306;server=192.168.2.66;uid=root;password=root");
    static string strConnection;
    int k,p,jj;
    int id; int Rid, RR, unit; int It2, Rid3, It3, unit3;
    DataTable dtitem = new DataTable();
    clsCommon obj = new clsCommon();
    commonClass obje = new commonClass();
    #endregion

    #region PAGE LOAD
    protected void Page_Load(object sender, EventArgs e)
    {  
        if (!IsPostBack)
        {
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            conn.ConnectionString = strConnection;
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            Title = " Tsunami ARMS - Room Resource Register ";
            check(); 
            dtitem = additem();
            Session["dtItem"] = dtitem;

            conn = obje.NewConnection();

            OdbcCommand Store1 = new OdbcCommand();
            Store1.CommandType = CommandType.StoredProcedure;
            Store1.Parameters.AddWithValue("tblname", "m_sub_building");
            Store1.Parameters.AddWithValue("attribute", "build_id,buildingname ");
            Store1.Parameters.AddWithValue("conditionv", "rowstatus<>'2'");
            OdbcDataAdapter Store16 = new OdbcDataAdapter(Store1);
            DataTable ds1 = new DataTable();
            ds1 = obje.SpDtTbl("CALL selectcond(?,?,?)", Store1);
            DataRow row = ds1.NewRow();
            ds1.Rows.InsertAt(row, 0);
            row["build_id"] = "-1";
            row["buildingname"] = "--Select--";            
            cmbBuilding.DataSource = ds1;
            cmbBuilding.DataBind();
            dtitem = additem();
            Session["dtItem"] = dtitem;

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
                id = int.Parse(dtt.Rows[0][0].ToString());
                Session["userid"] = id;
            }
            catch
            {
                id = 0;
                Session["userid"] = id;
            }
            
            itemcat();

            if (Session["submaster"] == "yes")
            {
                cmbBuilding.SelectedValue=Session["building"].ToString();            
                cmbRoomNo.SelectedValue=Session["room"].ToString();
                cmbName.SelectedValue=Session["name"].ToString();
                txtIcode.Text= Session["code"].ToString();
                cmbItemCategory.SelectedItem.Text= Session["category"].ToString();
                txtClass.Text= Session["class"].ToString();
                txtModel.Text= Session["model"].ToString();
                txtItemMaker.Text=Session["maker"].ToString();
                if (Session["item"] == "resource")
                {
                    this.ScriptManager1.SetFocus(cmbfloor);
                }
                else if (Session["item"] == "floornew")
                {
                    this.ScriptManager1.SetFocus(cmbRoomNo);
                }
                Session["submaster"] = "no";
            }
      
        }

        pnlinv.Visible = true;
        pnlroomdetails.Visible = false;
        pnlinvdetails.Visible = false;
        RoomInventory();
        conn.Close();
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

    #region Authentication Check function
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("Room Resource Register", level) == 0)
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

    #region ITEM CATEGOTY LOAD
    public void itemcat()
    {
        OdbcCommand Store2 = new OdbcCommand();
        Store2.CommandType = CommandType.StoredProcedure;
        Store2.Parameters.AddWithValue("tblname", "m_sub_itemcategory");
        Store2.Parameters.AddWithValue("attribute", "itemcat_id,itemcatname");
        Store2.Parameters.AddWithValue("conditionv", "rowstatus<>'2'");
        OdbcDataAdapter da58 = new OdbcDataAdapter(Store2);
        DataTable dt5 = new DataTable();
        dt5 = obje.SpDtTbl("CALL selectcond(?,?,?)", Store2);
        DataRow row1 = dt5.NewRow();
        dt5.Rows.InsertAt(row1, 0);
        row1["itemcat_id"] = "-1";
        row1["itemcatname"] = "--Select--";
        cmbItemCategory.DataSource = dt5;
        cmbItemCategory.DataBind();
    }
    #endregion

    #region DATA TABLE ADD ITEM
    public DataTable additem()
    {
        dtitem.Columns.Clear();
        dtitem.Columns.Add("Itemcode", System.Type.GetType("System.String"));
        dtitem.Columns.Add("Itemname", System.Type.GetType("System.String"));
        dtitem.Columns.Add("Itemcategory", System.Type.GetType("System.String"));
        dtitem.Columns.Add("Quantity", System.Type.GetType("System.Int32"));
        return (dtitem);
    }
    #endregion

    #region BULDING NAME SELECTED INDEX CHANGE
    protected void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {        
        OdbcDataAdapter RoomNo = new OdbcDataAdapter("select room_id,roomno from m_room r,m_sub_building b where r.build_id=b.build_id and r.build_id=" + cmbBuilding.SelectedValue + " and r.rowstatus<>'2' and b.rowstatus<>'2'", conn);
        DataTable ds1 = new DataTable();
        DataColumn colID = ds1.Columns.Add("room_id", System.Type.GetType("System.Int32"));
        DataColumn colNo = ds1.Columns.Add("roomno", System.Type.GetType("System.String"));
        DataRow row = ds1.NewRow();
        row["room_id"] = "-1";
        row["roomno"] = "--Select--";
        ds1.Rows.InsertAt(row, 0);
        RoomNo.Fill(ds1);
        cmbRoomNo.DataSource = ds1;
        cmbRoomNo.DataBind();
    }
    #endregion

    #region ITEM NAME LOAD
    public void ItemCategory()
    {
        cmbName.Items.Clear();
        OdbcCommand Store15 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        Store15.CommandType = CommandType.StoredProcedure;
        Store15.Parameters.AddWithValue("tblname", "m_inventory inv,m_sub_item it");
        Store15.Parameters.AddWithValue("attribute", "distinct itemname,inv.item_id as item_id");
        Store15.Parameters.AddWithValue("conditionv", "it.itemcat_id=" + cmbItemCategory.SelectedValue.ToString() + "  and inv.rowstatus<>'2' and inv.item_id=it.item_id");
        OdbcDataAdapter da59 = new OdbcDataAdapter(Store15);
        DataTable ds1 = new DataTable();
        da59.Fill(ds1);
        DataRow row = ds1.NewRow();
        ds1.Rows.InsertAt(row, 0);
        row["item_id"] = "-1";
        row["itemname"] = "--Select--";
        cmbName.DataSource = ds1;
        cmbName.DataBind();
    }
    #endregion

    #region ITEMCATEGORY SELECTED CHANGE
    protected void cmbItemCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemCategory();
    }
    #endregion

    #region ITEM NAME SELECTED INDEX CHANGE
    protected void cmbName_SelectedIndexChanged(object sender, EventArgs e)
    {        
        OdbcCommand Item = new OdbcCommand();
        Item.CommandType = CommandType.StoredProcedure;
        Item.Parameters.AddWithValue("tblname", "m_inventory");
        Item.Parameters.AddWithValue("attribute", "itemcode,itemclass,itemmaker,itemmodel");
        Item.Parameters.AddWithValue("conditionv", "item_id=" + cmbName.SelectedValue + " and m_inventory.rowstatus<>'2'");
        OdbcDataAdapter da60 = new OdbcDataAdapter(Item);
        DataTable ds2 = new DataTable();
        ds2 = obje.SpDtTbl("CALL selectcond(?,?,?)", Item);        
        foreach(DataRow dr1 in ds2.Rows)
        {
            txtIcode.Text = dr1["itemcode"].ToString();
            txtClass.Text = dr1["itemclass"].ToString();
            txtModel.Text = dr1["itemmodel"].ToString();
            txtItemMaker.Text = dr1["itemmaker"].ToString();
        }

    }
    #endregion

    protected void Button1_Click(object sender, EventArgs e)
    {
    }

    #region ADD ITEM CLICK
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        #region add item

        conn = obje.NewConnection();       
        Panel3.Visible = true;        
        int iRowCount = 0;
        dtitem = (DataTable)Session["dtItem"];    
        if (dtitem.Rows.Count > 0)
        {
            if (txtIcode.Text != "")
            {
                DataRow[] drItem = dtitem.Select("Itemcode='" + txtIcode.Text.ToString() + "'");
                if (drItem.Length > 0)
                {
                    foreach (DataRow row in drItem)
                    {
                       
                        dtitem.Rows[iRowCount]["Itemcode"] = Convert.ToString(row.ItemArray[1]);
                        dtitem.Rows[iRowCount]["Itemname"] = Convert.ToString(row.ItemArray[2]);
                        dtitem.Rows[iRowCount]["Itemcategory"] = Convert.ToString(row.ItemArray[3]);
                        dtitem.Rows[iRowCount]["Quantity"] = Convert.ToString(row.ItemArray[4]);
                    }
                }

                else
                {
                    iRowCount = dtitem.Rows.Count;
                    dtitem.Rows.Add();
                }
            }

            dtitem.Rows[iRowCount]["Itemcode"] = txtIcode.Text.ToString();// SelectedItem.Text;// ddlCat1.SelectedValue;
            dtitem.Rows[iRowCount]["Itemcategory"] = cmbItemCategory.SelectedItem.Text;//.SelectedItem.Text;
            dtitem.Rows[iRowCount]["Itemname"] = cmbName.SelectedItem.Text;
            dtitem.Rows[iRowCount]["Quantity"] = txtQuantity.Text;
        }
        else
        {
            if (txtIcode.Text != "")
            {

                iRowCount = dtitem.Rows.Count;
                dtitem.Rows.Add();
                dtitem.Rows[iRowCount]["Itemcode"] = txtIcode.Text.ToString();// SelectedItem.Text;// ddlCat1.SelectedValue;
                dtitem.Rows[iRowCount]["Itemcategory"] = cmbItemCategory.SelectedItem.Text;//.SelectedItem.Text;
                dtitem.Rows[iRowCount]["Itemname"] = cmbName.SelectedItem.Text;
                dtitem.Rows[iRowCount]["Quantity"] = txtQuantity.Text;

            }
        }
               Session["dtItem"] = dtitem;        
               dtgAddItem.DataSource = dtitem;
               dtgAddItem.DataBind();
               Session["dtItem"] = dtgAddItem.DataSource;

        
        cmbItemCategory.SelectedIndex = -1;
        txtIcode.Text = "";
        cmbItemCategory.SelectedIndex = -1;
        cmbName.SelectedIndex = -1;
        txtClass.Text = "";
        txtModel.Text = "";
        txtItemMaker.Text = "";
        txtQuantity.Text = ""; 
        conn.Close();
        this.ScriptManager1.SetFocus(cmbItemCategory);
        #endregion

        ViewState["option"] = "NIL";
        ViewState["action"] = "NIL";
    }
    #endregion

    protected void btnbutton_Click(object sender, EventArgs e)
    {

    }

    #region BUTTON SAVE CLICK
    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region save and edit button
        Panel3.Visible = false;
        lblMsg.Text = "Do you want to Save item?"; lblHead.Text = "Tsunami ARMS - Confirmation";
        ViewState["action"] = "Save";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
        #endregion
    }
    #endregion

    #region BUTTON YES CLICK
    protected void btnYes_Click(object sender, EventArgs e)
    {

        conn = obje.NewConnection();       
        DateTime date = DateTime.Now;
        string dt1 = date.ToString("yyyy-MM-dd") + " " + date.ToString("HH:mm:ss");
        OdbcTransaction odbTrans = null;
        if (ViewState["action"].ToString() == "Save")
        {
            #region save item
            try
            {
                odbTrans = conn.BeginTransaction();
                OdbcCommand Build = new OdbcCommand("select room_id from m_room where roomno=" + cmbRoomNo.SelectedItem.Text + "  and build_id=" + cmbBuilding.SelectedValue + " and rowstatus<>'2'", conn);
                Build.Transaction = odbTrans;
                OdbcDataReader Buildr = Build.ExecuteReader();
                if (Buildr.Read())
                {
                    Rid = Convert.ToInt32(Buildr[0].ToString());
                }
                id = Convert.ToInt32(Session["userid"].ToString());

                OdbcCommand cmda = new OdbcCommand("select max(resource_id) from t_roomresource", conn);//autoincrement id
                cmda.Transaction = odbTrans;
                if (Convert.IsDBNull(cmda.ExecuteScalar()) == true)
                {
                    jj = 1;
                }
                else
                {
                    jj = Convert.ToInt32(cmda.ExecuteScalar());
                    jj = jj + 1;
                }
                
                OdbcCommand add = new OdbcCommand("CALL savedata(?,?)", conn);
                add.CommandType = CommandType.StoredProcedure;
                add.Parameters.AddWithValue("tblname", "t_roomresource");
                add.Parameters.AddWithValue("val", "" + jj + "," + Rid + "," + id + ",'" + dt1 + "'," + id + ",'" + dt1.ToString() + "'," + "0" + "");
                add.Transaction = odbTrans;
                add.ExecuteNonQuery();

                dtitem = (DataTable)Session["dtItem"];

                for (int i = 0; i < dtitem.Rows.Count; i++)
                {
                    DataRow[] drSpare = dtitem.Select("Itemcode='" + (dtitem.Rows[i]["Itemcode"]) + "'");
                    if (drSpare.Length > 0)
                    {
                        foreach (DataRow row in drSpare)
                        {

                            OdbcCommand cmdb = new OdbcCommand("select max(resource_item_no) from t_roomresource_items", conn);//autoincrement id
                            cmdb.Transaction = odbTrans;
                            if (Convert.IsDBNull(cmdb.ExecuteScalar()) == true)
                            {
                                RR = 1;
                            }
                            else
                            {
                                RR = Convert.ToInt32(cmdb.ExecuteScalar());
                                RR = RR + 1;
                            }

                            OdbcCommand ItemId = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                            ItemId.CommandType = CommandType.StoredProcedure;
                            ItemId.Parameters.AddWithValue("tblname", "m_sub_item s,m_inventory inv,m_sub_unit u");
                            ItemId.Parameters.AddWithValue("attribute", "inv.item_id,inv.unit_id ");
                            ItemId.Parameters.AddWithValue("conditionv", "itemname='" + dtitem.Rows[i]["Itemname"] + "' and s.rowstatus<>'2' and inv.item_id=s.item_id and inv.unit_id=u.unit_id");
                            ItemId.Transaction = odbTrans;
                            OdbcDataAdapter ItemId60 = new OdbcDataAdapter(ItemId);
                            DataTable ds3 = new DataTable();
                            ItemId60.Fill(ds3);
                            foreach(DataRow dr9 in ds3.Rows)
                            {
                                It2 = Convert.ToInt32(dr9[0].ToString());
                                unit = Convert.ToInt32(dr9[1].ToString());
                            }

                            OdbcCommand cmd2 = new OdbcCommand("CALL savedata(?,?)", conn);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("tblname", "t_roomresource_items");
                            cmd2.Parameters.AddWithValue("val", "" + RR + "," + jj + "," + It2 + "," + dtitem.Rows[i]["Quantity"] + "," + unit + "," + id + ",'" + dt1.ToString() + "'");
                            cmd2.Transaction = odbTrans;
                            cmd2.ExecuteNonQuery();
                        }
                    }
                }
                odbTrans.Commit();
                lblOk.Text = " Item Added Successfully "; lblHead.Text = "Tsunami ARMS - confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();                
                RoomInventory();
                cmbBuilding.SelectedIndex = -1;
                cmbfloor.SelectedIndex = -1;
                cmbRoomNo.SelectedIndex = -1;
                cmbName.SelectedIndex = -1;
                cmbItemCategory.SelectedItem.Text = "";
                txtClass.Text = "";
                txtModel.Text = "";
                txtIcode.Text = "";
                txtItemMaker.Text = "";
                Panelrep.Visible = false;
                pnlinvdetails.Visible = false;
                clear();
                RoomInventory();
                this.ScriptManager1.SetFocus(btndelete);
                conn.Close();                
            }
            catch
            {
                odbTrans.Rollback();
                ViewState["action"] = "NILL";
                okmessage("Tsunami ARMS - Warning", "Error in Saving ");
            }
            #endregion

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
        else if (ViewState["action"].ToString() == "Edit")
        {

            #region edit
            conn = obje.NewConnection();
            int q,t;
            q = Convert.ToInt32(dtgRoomInventory.DataKeys[dtgRoomInventory.SelectedRow.RowIndex].Values[0].ToString());
            t = Convert.ToInt32(dtgRoomInventory.DataKeys[dtgRoomInventory.SelectedRow.RowIndex].Values[1].ToString());
            id = Convert.ToInt32(Session["userid"].ToString());
            try
            {
                odbTrans = conn.BeginTransaction();
                OdbcCommand ItemId = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                ItemId.CommandType = CommandType.StoredProcedure;
                ItemId.Parameters.AddWithValue("tblname", "m_sub_item s,m_inventory inv,m_sub_unit u");
                ItemId.Parameters.AddWithValue("attribute", "inv.item_id,inv.unit_id ");
                ItemId.Parameters.AddWithValue("conditionv", "inv.item_id='" + cmbName.SelectedValue + "' and s.rowstatus<>'2' and inv.item_id=s.item_id and inv.unit_id=u.unit_id");
                ItemId.Transaction = odbTrans;
                OdbcDataAdapter ItemIdr = new OdbcDataAdapter(ItemId);
                DataTable ds3 = new DataTable();
                ItemIdr.Fill(ds3);
                foreach (DataRow dr5 in ds3.Rows)
                {
                    It3 = Convert.ToInt32(dr5[0].ToString());
                    unit3 = Convert.ToInt32(dr5[1].ToString());
                }

                OdbcCommand cmd3 = new OdbcCommand("call updatedata(?,?,?)", conn);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("tblname", "t_roomresource_items");
                cmd3.Parameters.AddWithValue("valu", "item_id=" + cmbName.SelectedValue + ",quantity=" + txtQuantity.Text + ",unit_id=" + unit3 + "");
                cmd3.Parameters.AddWithValue("convariable", "resource_id=" + q + " and item_id=" + t + "");
                cmd3.Transaction = odbTrans;
                cmd3.ExecuteNonQuery();

                OdbcCommand Build = new OdbcCommand("select room_id from m_room where roomno=" + cmbRoomNo.SelectedItem.Text + "  and build_id=" + cmbBuilding.SelectedValue + " and rowstatus<>'2'", conn);
                Build.Transaction = odbTrans;
                OdbcDataReader Buildr = Build.ExecuteReader();
                if (Buildr.Read())
                {
                    Rid3 = Convert.ToInt32(Buildr[0].ToString());
                }

                OdbcCommand cmd3a = new OdbcCommand(" call updatedata(?,?,?)", conn);
                cmd3a.CommandType = CommandType.StoredProcedure;
                cmd3a.Parameters.AddWithValue("tblname", "t_roomresource");
                cmd3a.Parameters.AddWithValue("valu", "room_id=" + Rid3 + ",updatedby=" + id + ",updatedon='" + dt1.ToString() + "',rowstatus=" + 1 + "");
                cmd3a.Parameters.AddWithValue("convariable", "resource_id=" + q + "");
                cmd3a.Transaction = odbTrans;
                cmd3a.ExecuteNonQuery();

                odbTrans.Commit();
                lblOk.Text = " Data Updated Successfully "; lblHead.Text = "Tsunami ARMS - confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();                
                pnlinv.Visible = true;
                cmbBuilding.SelectedIndex = -1;
                cmbfloor.SelectedIndex = -1;
                cmbRoomNo.SelectedIndex = -1;
                itemcat();
                cmbItemCategory.SelectedIndex = -1; 
                cmbName.SelectedIndex = -1;
                ItemCategory();
                cmbName.SelectedItem.Text = "";
                txtClass.Text = "";
                txtModel.Text = "";
                txtIcode.Text = "";
                txtItemMaker.Text = "";
                Panelrep.Visible = false;
                pnlinvdetails.Visible = false;
                clear();
                RoomInventory();
                this.ScriptManager1.SetFocus(btndelete);
                conn.Close();
            }
            catch
            {
                odbTrans.Rollback();
                ViewState["action"] = "NILL";
                okmessage("Tsunami ARMS - Warning", "Error in Editing ");
            }
            #endregion

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
        else if (ViewState["action"].ToString() == "Delete")
        {

            #region delete
            conn = obje.NewConnection();
            int q, t;
            q = Convert.ToInt32(dtgRoomInventory.DataKeys[dtgRoomInventory.SelectedRow.RowIndex].Values[0].ToString());
            t = Convert.ToInt32(dtgRoomInventory.DataKeys[dtgRoomInventory.SelectedRow.RowIndex].Values[1].ToString());
            id = Convert.ToInt32(Session["userid"].ToString());

            OdbcCommand cma = new OdbcCommand(" call updatedata(?,?,?)", conn);
            cma.CommandType = CommandType.StoredProcedure;
            cma.Parameters.AddWithValue("tblname", "t_roomresource");
            cma.Parameters.AddWithValue("valu", "rowstatus=" +"2"+ "");
            cma.Parameters.AddWithValue("convariable", "resource_id=" + q + "");
            cma.ExecuteNonQuery();

            lblOk.Text = " Data deleted Successfully "; lblHead.Text = "Tsunami ARMS - confirmation";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            conn.Close();
            pnlinv.Visible = true;
            RoomInventory();
            cmbBuilding.SelectedIndex = -1;
            cmbRoomNo.SelectedIndex = -1;
            cmbName.SelectedIndex = -1;
            cmbItemCategory.SelectedIndex = -1;
            itemcat();
            txtClass.Text = "";
            txtModel.Text = "";
            txtIcode.Text = "";
            txtItemMaker.Text = "";
            Panelrep.Visible = false;
            pnlinvdetails.Visible = false;
            clear();
            pnlinv.Visible = true;
            RoomInventory();
            btnclear.Focus();
            conn.Close();
            #endregion

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
        
    }
    #endregion

    #region CLEAR BUTTON CLICK
    public void clear()
    {
        #region clear
        cmbBuilding.SelectedIndex = -1;  
        cmbRoomNo.SelectedIndex = -1;
        cmbName.SelectedIndex = -1;
        cmbItemCategory.SelectedIndex = -1;
        txtClass.Text = "";
        txtModel.Text = "";
        txtIcode.Text = "";
        txtItemMaker.Text = "";
        txtQuantity.Text = "";
        Panel3.Visible = false;
        Panelrep.Visible = false;
        pnlinv.Visible = false;
        pnlinvdetails.Visible = false;
        btnAdd.Enabled = true;
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
        dtitem.Rows.Clear();
        ItemCategory();
        #endregion
    }
    #endregion

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
    }
    protected void dtgBuilding_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
    }
    protected void dtgBuilding_RowCreated(object sender, GridViewRowEventArgs e)
    {
    }
    protected void dtgRoomInventory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        #region ROOM WITH INVENTORY DETAILS
        dtgRoomInventory.PageIndex = e.NewPageIndex;
        dtgRoomInventory.DataBind();
        RoomInventory();
        #endregion
    }

         #region ROOM INVENTORY ROW CREATED
    protected void dtgRoomInventory_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgRoomInventory, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion


    protected void dtgRoomInventory_SelectedIndexChanged(object sender, EventArgs e)
    {        
        #region PLACE DATA FROM GRID FOR EDIT/ DELETE
        conn = obje.NewConnection();
        int q,t;
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        btnAdd.Enabled = false;
      
        try
        {
            q = Convert.ToInt32(dtgRoomInventory.DataKeys[dtgRoomInventory.SelectedRow.RowIndex].Values[0].ToString());
            t=Convert.ToInt32(dtgRoomInventory.DataKeys[dtgRoomInventory.SelectedRow.RowIndex].Values[1].ToString());
            
            OdbcCommand cmd3 = new OdbcCommand();
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Parameters.AddWithValue("tblname", "t_roomresource r,t_roomresource_items it,m_inventory inv,m_sub_unit u,m_sub_item i,m_sub_building b,m_room re,m_sub_itemcategory ca");
            cmd3.Parameters.AddWithValue("attribute", "r.resource_id,it.item_id,itemname,quantity,unitname,buildingname,roomno,b.build_id,itemclass,itemcode,itemmodel,itemmaker,itemcatname,inv.itemcat_id");
            cmd3.Parameters.AddWithValue("conditionv", "r.resource_id=it.resource_id and inv.unit_id=u.unit_id and it.item_id=i.item_id and inv.item_id=i.item_id and re.room_id=r.room_id and re.build_id=b.build_id and r.resource_id=" + q + " and it.item_id=" + t + " and inv.itemcat_id=ca.itemcat_id");
            OdbcDataAdapter cmd36 = new OdbcDataAdapter(cmd3);
            DataTable ds2 = new DataTable();
            ds2 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd3);
            foreach(DataRow dr1 in ds2.Rows)
            {
                cmbBuilding.SelectedValue = dr1["build_id"].ToString();
                cmbBuilding.SelectedItem.Text = dr1["buildingname"].ToString();
                cmbBuilding_SelectedIndexChanged(null, null);
                cmbRoomNo.SelectedValue = dr1["roomno"].ToString();
                cmbRoomNo.SelectedItem.Text = dr1["roomno"].ToString();
                cmbItemCategory.SelectedItem.Text = dr1["itemcatname"].ToString();
                cmbItemCategory.SelectedValue = dr1["itemcat_id"].ToString();
                cmbItemCategory_SelectedIndexChanged(null, null);
                cmbName.SelectedItem.Text = dr1["itemname"].ToString();
                cmbName.SelectedValue = dr1["item_id"].ToString();
                txtIcode.Text = dr1["itemcode"].ToString();
                txtClass.Text = dr1["itemclass"].ToString();
                txtModel.Text = dr1["itemmodel"].ToString();
                txtItemMaker.Text = dr1["itemmaker"].ToString();
                txtQuantity.Text = dr1["quantity"].ToString();
            }
        }
        catch (Exception ex)
        {

        }
        conn.Close();
        #endregion
    }
    protected void dtgRoomInventory_Sorting(object sender, GridViewSortEventArgs e)
    {
    }

    #region ROOM WITH INVENTORY DETAILS GRID
    public void RoomInventory()
    {
        conn = obje.NewConnection();
        OdbcCommand Rom = new OdbcCommand();
        Rom.CommandType = CommandType.StoredProcedure;
        Rom.Parameters.AddWithValue("tblname", "t_roomresource r,t_roomresource_items it,m_inventory inv,m_sub_unit u,m_sub_item i,m_sub_building b,m_room re");
        Rom.Parameters.AddWithValue("attribute", " r.resource_id,it.item_id,itemname,quantity,unitname,buildingname,roomno");
        Rom.Parameters.AddWithValue("conditionv", "r.resource_id=it.resource_id and inv.unit_id=u.unit_id and it.item_id=i.item_id and inv.item_id=i.item_id and re.room_id=r.room_id and re.build_id=b.build_id and r.rowstatus<>'2' group by resource_id,item_id order by re.room_id asc");
        OdbcDataAdapter ItemId60 = new OdbcDataAdapter(Rom);
        DataTable ds2 = new DataTable();
        ds2 = obje.SpDtTbl("CALL selectcond(?,?,?)", Rom);         
        dtgRoomInventory.DataSource = ds2;
        dtgRoomInventory.DataBind();
        conn.Close();
    }
    #endregion

    protected void lnkroomlist_Click(object sender, EventArgs e)
    {

    }
    protected void lnkresource_Click(object sender, EventArgs e)
    {

    }
    protected void lnkrb_Click(object sender, EventArgs e)
    {

    }
    protected void btndelete_Click(object sender, EventArgs e)
    {

    }
    protected void btnclear_Click(object sender, EventArgs e)
    {

    }
    protected void Button2_Click(object sender, EventArgs e)
    {

    }
    protected void Button3_Click(object sender, EventArgs e)
    {

    }
    protected void btnNo_Click(object sender, EventArgs e)
    {  
    }

    #region BUTTON OK CLICK
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
    }
    #endregion

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
    }
    protected void btnHidden_Click(object sender, EventArgs e)
    {

    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        #region edit
        lblMsg.Text = "Do you want to Edit ?"; lblHead.Text = "Tsunami ARMS - Confirmation";
        ViewState["action"] = "Edit";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
        #endregion
    }
    protected void btndelete_Click1(object sender, EventArgs e)
    {
        #region delete

        lblMsg.Text = "Do you want to Delete item?"; lblHead.Text = "Tsunami ARMS - Confirmation";
        ViewState["action"] = "Delete";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);

        #endregion
    }
    protected void btnclear_Click1(object sender, EventArgs e)
    {
        clear();
    }


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
                        this.dtgAddItem.DataSource = ((DataTable)Session["dtItem"]);
                        this.dtgAddItem.DataBind();

                    }
                }


            }

        }
        catch (Exception ex)
        {
        }
        finally
        {



            conn.Close();
        }


    }
    #endregion

    #region LINK NEW BUTTON CLICK
    protected void lnkNew_Click(object sender, EventArgs e)
    {
        Session["building"] = cmbBuilding.SelectedValue.ToString();       
        Session["room"] = cmbRoomNo.SelectedValue.ToString();
        Session["name"] = cmbName.SelectedValue.ToString();
        Session["code"] = txtIcode.Text.ToString();
        Session["category"] = cmbItemCategory.SelectedValue.ToString();
        Session["class"] = txtClass.Text.ToString();
        Session["model"] = txtModel.Text.ToString();
        Session["maker"] = txtItemMaker.Text.ToString();
        Session["submaster"] = "yes";
        Session["item"] = "resource";
        Response.Redirect("~/Submasters.aspx");
    }
    #endregion

    protected void dtgBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {        
    }
    protected void Button2_Click1(object sender, EventArgs e)
    {
    }
}


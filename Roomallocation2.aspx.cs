using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using clsDAL;

public partial class Roomallocation2 : System.Web.UI.Page
{
    string fromdate, todate, totaldays, place, reserve_no, reserve_code, name;
    int n;
    commonClass objcls = new commonClass();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            pnlr.Visible = false;
            lbl1.Visible = false;
            string selectdate = "SELECT DISTINCT DATE_FORMAT(reservedate,'%d-%m-%Y') AS 'reservedate' FROM t_roomreservation_generaltdbtemp WHERE (DATE_FORMAT(reservedate,'%Y-%m-%d')) >= CURDATE()";
            DataTable dtselect_date = objcls.DtTbl(selectdate);
            DataRow rowdate = dtselect_date.NewRow();
            rowdate["reservedate"] = "-1";
            rowdate["reservedate"] = "--Select--";
            dtselect_date.Rows.InsertAt(rowdate, 0);
            cmbReserve.DataSource = dtselect_date;
            cmbReserve.DataBind();
        }

    }

    protected void cmbReserve_SelectedIndexChanged(object sender, EventArgs e)
    {
        string createddate = objcls.yearmonthdate(cmbReserve.SelectedItem.Text);
        string da = createddate.Replace('/', '-');

        string selecttype = "SELECT id,TYPE FROM p_type_of_user where status=0";
        DataTable dtselect_type = objcls.DtTbl(selecttype);
        //DataRow rowtype = dtselect_type.NewRow();
        //rowtype["id"] = "-1";
        //rowtype["TYPE"] = "--Select--";
        //dtselect_type.Rows.InsertAt(rowtype, 0);
        ddltype.DataSource = dtselect_type;
        ddltype.DataBind();
        string type = "General";
        string reservetypeselect = "SELECT DISTINCT room_category_id FROM t_roomreservation_generaltdbtemp WHERE reserve_mode='" + type + "' AND DATE_FORMAT(reservedate,'%Y-%m-%d')='" + da + "'";
        DataTable dtreservetypeselect = objcls.DtTbl(reservetypeselect);
        if (dtreservetypeselect.Rows.Count > 0)
        {
            OdbcCommand cmdroomcategory = new OdbcCommand();
            cmdroomcategory.Parameters.AddWithValue("tblname", "m_sub_room_category,p_roomstatus,t_roomreservation_generaltdbtemp");
            cmdroomcategory.Parameters.AddWithValue("attribute", "DISTINCT room_cat_name,room_cat_id");
            cmdroomcategory.Parameters.AddWithValue("conditionv", "p_roomstatus.room_category_id=m_sub_room_category.room_cat_id and rowstatus<>" + 2 + " AND t_roomreservation_generaltdbtemp.reserve_mode='" + type + "' AND DATE_FORMAT(reservedate,'%Y-%m-%d')='" + da + "' AND t_roomreservation_generaltdbtemp.room_category_id=m_sub_room_category.room_cat_id ORDER BY p_roomstatus.room_category_id asc");
            DataTable dtroomcategory = new DataTable();
            dtroomcategory = objcls.SpDtTbl("Call selectcond(?,?,?)", cmdroomcategory);
            DataRow rowroomcat = dtroomcategory.NewRow();
            rowroomcat["room_cat_id"] = "-1";
            rowroomcat["room_cat_name"] = "--Select--";
            dtroomcategory.Rows.InsertAt(rowroomcat, 0);
            ddlcat.DataSource = dtroomcategory;
            ddlcat.DataBind();
           
        }
        else
        {


        }


    }
    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    protected void ddlcat_SelectedIndexChanged(object sender, EventArgs e)
    {
        string createddate = objcls.yearmonthdate(cmbReserve.SelectedItem.Text);
        string daa = createddate.Replace('/', '-');
        string type = "";
        if (ddltype.SelectedValue == "1")
        {
            type = "General";
        }
        else
        {
            type = ddltype.SelectedItem.ToString();
        }


        OdbcCommand da = new OdbcCommand();
        da.Parameters.AddWithValue("tblname", "m_sub_building,m_room");
        da.Parameters.AddWithValue("attribute", "DISTINCT m_sub_building.buildingname,m_room.build_id");
        da.Parameters.AddWithValue("conditionv", "m_room.build_id=m_sub_building.build_id AND room_cat_id=" + int.Parse(ddlcat.SelectedValue.ToString()) + " and m_sub_building.rowstatus<>" + 2 + " order by buildingname asc");
        DataTable dtt1 = new DataTable();
        dtt1 = objcls.SpDtTbl("Call selectcond(?,?,?)", da);
        DataRow row11b = dtt1.NewRow();
        row11b["build_id"] = "-1";
        row11b["buildingname"] = "--Select--";
        dtt1.Rows.InsertAt(row11b, 0);
        ddlbuild.DataSource = dtt1;
        ddlbuild.DataBind();
        
       
        string strr = @"SELECT COUNT(reserve_no)AS counts FROM t_roomreservation_generaltdbtemp WHERE room_category_id =" + int.Parse(ddlcat.SelectedValue.ToString()) +
                                         " AND reserve_mode='" + type + "' AND allot_status=" + 0 +
                                         " AND DATE_FORMAT(reservedate,'%Y-%m-%d %T')LIKE'" + daa + "%'";
        DataTable dtt = objcls.DtTbl(strr);
        lblreserve.Text = dtt.Rows[0]["counts"].ToString();

    }





    protected void chkj_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ch = (CheckBox)gdbind.HeaderRow.FindControl("chkj");
        if (ch.Checked == true)
        {


            for (int w = 0; w < gdbind.Rows.Count; w++)
            {
                CheckBox chkd = (CheckBox)gdbind.Rows[w].Cells[0].FindControl("chkacc");
                chkd.Checked = true;

            }


        }

        else
        {
            for (int w = 0; w < gdbind.Rows.Count; w++)
            {
                CheckBox chkd = (CheckBox)gdbind.Rows[w].Cells[0].FindControl("chkacc");
                chkd.Checked = false;

            }

        }


    }
    
    protected void Button4_Click(object sender, EventArgs e)
    {

        try
        {

            string createddate = objcls.yearmonthdate(cmbReserve.SelectedItem.Text);
            string daa = createddate.Replace('/', '-');
            string type = "";
            if (ddltype.SelectedValue == "1")
            {
                type = "General";
            }
            else
            {
                type = ddltype.SelectedItem.ToString();
            }

            string ss = @"SELECT COUNT(room_category_id) FROM t_roomreservation_generaltdbtemp WHERE room_category_id ='" + ddlcat.SelectedItem.Value + "' AND allot_status = 0 AND NOW() <= reservedate AND NOW() < expvacdate";

            DataTable dtcat = objcls.DtTbl(ss);
            int count = gdbind.Rows.Count;




            int mm = Convert.ToInt16(dtcat.Rows[0][0].ToString());

            if (count > mm)
            {

                string da = @"SELECT * FROM t_roomreservation_generaltdbtemp WHERE  NOW()<=reservedate AND NOW()<expvacdate AND allot_status = 0 AND room_category_id = '" + ddlcat.SelectedItem.Value + "'";
                DataTable dtda = objcls.DtTbl(da);
                for (int i = 0; i < mm; i++)
                {
                    CheckBox chk1 = (CheckBox)gdbind.Rows[i].FindControl("chkacc");
                    if (chk1.Checked == true)
                    {

                        if (dtda.Rows.Count > 0)
                        {
                            fromdate = dtda.Rows[i]["reservedate"].ToString();
                            todate = dtda.Rows[i]["expvacdate"].ToString();
                            totaldays = dtda.Rows[i]["total_days"].ToString();
                            place = dtda.Rows[i]["place"].ToString();
                            reserve_code = dtda.Rows[i]["reserve_no"].ToString();
                            name = dtda.Rows[i]["swaminame"].ToString();
                        }





                        string room = gdbind.Rows[i].Cells[1].Text;





                        string see = @"SELECT room_id FROM m_room WHERE roomno = '" + room + "' AND build_id ='" + ddlbuild.SelectedItem.Value + "' ";
                        DataTable dtsa = objcls.DtTbl(see);



                        DateTime dt5 = DateTime.Now;
                        string date = dt5.ToString("yyyy-MM-dd HH:mm:ss");
                        fromdate = DateTime.Parse(fromdate).ToString("yyyy-MM-dd HH:mm:ss");
                        todate = DateTime.Parse(todate).ToString("yyyy-MM-dd HH:mm:ss");
                        int pk = objcls.PK_exeSaclarInt("reserve_id", "t_roomreservation");
                        pk = pk + 1;
                        try
                        {
                            n = int.Parse(Session["userid"].ToString());
                        }
                        catch
                        {
                            n = 1;
                            Session["userid"] = n.ToString();
                        }







                        string reserveroom = "INSERT INTO t_roomreservation(reserve_id,reserve_type,reserve_mode,swaminame,reservedate,expvacdate,total_days,status_reserve,passmode,createdby,cretaedon,updatedby,updateddate,place,room_id,reserve_no) VALUES ( " + pk + ",'Single','" + type + "','" + name + "','" + fromdate + "','" + todate + "'," + totaldays + ",0,0," + n + ",'" + date + "'," + n + ",'" + date + "','" + place + "'," + dtsa.Rows[0][0].ToString() + ",'" + reserve_code + "')";
                        int flag = objcls.exeNonQuery(reserveroom);

                        if (flag == 1)
                        {

                            string updatetemp = "update t_roomreservation_generaltdbtemp set allot_status=" + 1 + " where reserve_no='" + reserve_code + "' AND  NOW() <= reservedate AND NOW() < expvacdate";
                            objcls.exeNonQuery(updatetemp);
                        }
                    }







                }
                string query = @"SELECT DISTINCT room_id,roomno FROM m_room WHERE room_id NOT IN (SELECT DISTINCT room_id FROM t_roomreservation WHERE NOW() <= reservedate AND NOW() < expvacdate AND status_reserve='0') AND build_id='" + ddlbuild.SelectedValue.ToString() + "' AND rowstatus!='2' AND room_cat_id='" + ddlcat.SelectedValue.ToString() + "' AND roomstatus='1' ORDER BY roomno";
                DataTable dtrry = objcls.DtTbl(query);
                gdbind.DataSource = dtrry;
                gdbind.DataBind();





            }
            else
            {

                string da = @"SELECT * FROM t_roomreservation_generaltdbtemp WHERE  NOW()<=reservedate AND NOW()<expvacdate AND allot_status = 0 AND room_category_id = '" + ddlcat.SelectedItem.Value + "'";
                DataTable dtda = objcls.DtTbl(da);
                for (int i = 0; i < count; i++)
                {
                    CheckBox chk1 = (CheckBox)gdbind.Rows[i].FindControl("chkacc");
                    if (chk1.Checked == true)
                    {

                        if (dtda.Rows.Count > 0)
                        {
                            fromdate = dtda.Rows[i]["reservedate"].ToString();
                            todate = dtda.Rows[i]["expvacdate"].ToString();
                            totaldays = dtda.Rows[i]["total_days"].ToString();
                            place = dtda.Rows[i]["place"].ToString();
                            reserve_code = dtda.Rows[i]["reserve_no"].ToString();
                            name = dtda.Rows[i]["swaminame"].ToString();
                        }





                        string room = gdbind.Rows[i].Cells[1].Text;





                        string see = @"SELECT room_id FROM m_room WHERE roomno = '" + room + "' AND build_id ='" + ddlbuild.SelectedItem.Value + "' ";
                        DataTable dtsa = objcls.DtTbl(see);



                        DateTime dt5 = DateTime.Now;
                        string date = dt5.ToString("yyyy-MM-dd HH:mm:ss");
                        fromdate = DateTime.Parse(fromdate).ToString("yyyy-MM-dd HH:mm:ss");
                        todate = DateTime.Parse(todate).ToString("yyyy-MM-dd HH:mm:ss");
                        int pk = objcls.PK_exeSaclarInt("reserve_id", "t_roomreservation");
                        pk = pk + 1;
                        try
                        {
                            n = int.Parse(Session["userid"].ToString());
                        }
                        catch
                        {
                            n = 1;
                            Session["userid"] = n.ToString();
                        }







                        string reserveroom = "INSERT INTO t_roomreservation(reserve_id,reserve_type,reserve_mode,swaminame,reservedate,expvacdate,total_days,status_reserve,passmode,createdby,cretaedon,updatedby,updateddate,place,room_id,reserve_no) VALUES ( " + pk + ",'Single','" + type + "','" + name + "','" + fromdate + "','" + todate + "'," + totaldays + ",0,0," + n + ",'" + date + "'," + n + ",'" + date + "','" + place + "'," + dtsa.Rows[0][0].ToString() + ",'" + reserve_code + "')";
                        int flag = objcls.exeNonQuery(reserveroom);

                        if (flag == 1)
                        {

                            string updatetemp = "update t_roomreservation_generaltdbtemp set allot_status=" + 1 + " where reserve_no='" + reserve_code + "' AND  NOW() <= reservedate AND NOW() < expvacdate";
                            objcls.exeNonQuery(updatetemp);
                        }
                    }







                }
                string query = @"SELECT DISTINCT room_id,roomno FROM m_room WHERE room_id NOT IN (SELECT DISTINCT room_id FROM t_roomreservation WHERE NOW() <= reservedate AND NOW() < expvacdate AND status_reserve='0') AND build_id='" + ddlbuild.SelectedValue.ToString() + "' AND rowstatus!='2' AND room_cat_id='" + ddlcat.SelectedValue.ToString() + "' AND roomstatus='1' ORDER BY roomno";
                DataTable dtrry = objcls.DtTbl(query);
                gdbind.DataSource = dtrry;
                gdbind.DataBind();




            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Room Alloted Successfully')", true);
            string strr = @"SELECT COUNT(reserve_no)AS counts FROM t_roomreservation_generaltdbtemp WHERE room_category_id =" + int.Parse(ddlcat.SelectedValue.ToString()) +
                                         " AND reserve_mode='" + type + "' AND allot_status=" + 0 +
                                         " AND DATE_FORMAT(reservedate,'%Y-%m-%d %T')LIKE'" + daa + "%'";
            DataTable dtt = objcls.DtTbl(strr);
            lblallot.Text = dtt.Rows[0]["counts"].ToString();
            
        }






        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Error')", true);

        }


    }




    protected void chkg_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void ddlbuild_SelectedIndexChanged(object sender, EventArgs e)
    {

        //DataTable dtta = new DataTable();
        //string rr = @"SELECT DISTINCT room_id FROM t_roomreservation";
        //DataTable dtrr = objcls.DtTbl(rr);
        //int xx = dtrr.Rows.Count;
        //for(int k = 0; k< xx; k++)
        //{
        //    int f = Convert.ToInt16(dtrr.Rows[k][0].ToString());
        //    string sel = @"SELECT DISTINCT roomno FROM m_room WHERE build_id =" + int.Parse(ddlbuild.SelectedValue.ToString()) + " AND  rowstatus<>" + 2 + " AND room_cat_id =" + ddlcat.SelectedValue.ToString() + " AND roomstatus=" + 1 + " AND room_id ='"+ f + "'   ORDER BY roomno ASC";

        //    dtta = objcls.DtTbl(sel);
        //}

        string query = @"SELECT DISTINCT room_id,roomno FROM m_room WHERE room_id NOT IN (SELECT DISTINCT room_id FROM t_roomreservation WHERE NOW() <= reservedate AND NOW() < expvacdate AND status_reserve='0') AND build_id='" + ddlbuild.SelectedValue.ToString() + "' AND rowstatus!='2' AND room_cat_id='" + ddlcat.SelectedValue.ToString() + "' AND roomstatus='1' ORDER BY roomno";
        DataTable dtrry = objcls.DtTbl(query);
        gdbind.DataSource = dtrry;
        gdbind.DataBind();


    }
    protected void chkacc_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gdbind.Rows.Count; i++)
        {
            string type = "";
            if (ddltype.SelectedValue == "1")
            {
                type = "General";
            }
            else
            {
                type = ddltype.SelectedItem.ToString();
            }
            CheckBox chk1 = (CheckBox)gdbind.Rows[i].FindControl("chkacc");
            if (chk1.Checked == true)
            {
                string room = gdbind.Rows[i].Cells[1].Text;





                string see = @"SELECT room_id FROM m_room WHERE roomno = '" + room + "' AND build_id ='" + ddlbuild.SelectedItem.Value + "' ";
                DataTable dtsa = objcls.DtTbl(see);

                string uu = @"UPDATE t_roomreservation SET expvacdate = DATE_ADD(expvacdate,INTERVAL 1 DAY ) WHERE NOW() BETWEEN reservedate AND expvacdate AND room_id = '" + dtsa.Rows[0][0].ToString() + "'";
                objcls.exeNonQuery(uu);


            }
        }


        gdbind.Visible = false;
        string gt = @"SELECT reserve_no AS 'Reservation No',m_room.roomno AS 'Alloted Room',m_sub_building.buildingname AS 'Building Name' FROM t_roomreservation INNER JOIN m_room ON m_room.room_id = t_roomreservation.room_id  INNER JOIN m_sub_building ON m_room.build_id = m_sub_building.build_id WHERE status_reserve = 0 AND NOW() <= reservedate AND NOW() < expvacdate";
        DataTable dtgt = objcls.DtTbl(gt);
        gdshow.DataSource = dtgt;
        gdshow.DataBind();

        string query = @"SELECT DISTINCT room_id,roomno FROM m_room WHERE room_id NOT IN (SELECT DISTINCT room_id FROM t_roomreservation WHERE NOW() <= reservedate AND NOW() < expvacdate AND status_reserve='0') AND build_id='" + ddlbuild.SelectedValue.ToString() + "' AND rowstatus!='2' AND room_cat_id='" + ddlcat.SelectedValue.ToString() + "' AND roomstatus='1' ORDER BY roomno";
        DataTable dtrry = objcls.DtTbl(query);
        gdbind.DataSource = dtrry;
        gdbind.DataBind();

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Room Removed Successfully')", true);
    }
    public void bind()
    {
        lbl1.Visible = true;
        gdbind.Visible = false;
        string gt = @"SELECT reserve_no AS 'Reservation No',m_room.roomno AS 'Alloted Room',m_sub_building.buildingname AS 'Building Name' FROM t_roomreservation INNER JOIN m_room ON m_room.room_id = t_roomreservation.room_id  INNER JOIN m_sub_building ON m_room.build_id = m_sub_building.build_id WHERE status_reserve = 0 AND NOW() <= reservedate AND NOW() < expvacdate ORDER BY m_sub_building.buildingname";

        DataTable dtgt = objcls.DtTbl(gt);
        gdshow.DataSource = dtgt;
        gdshow.DataBind();

    }

    protected void Button5_Click(object sender, EventArgs e)
    {
        bind();
    }
    protected void gdbind_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        lbl1.Visible = true;
        gdbind.PageIndex = e.NewPageIndex;

        string query = @"SELECT DISTINCT room_id,roomno FROM m_room WHERE room_id IN (SELECT DISTINCT room_id FROM t_roomreservation WHERE NOW() <= reservedate AND NOW() < expvacdate AND status_reserve='0') AND build_id='" + ddlbn.SelectedItem.Value + "' AND rowstatus!='2' AND room_cat_id='" + ddlcid.SelectedItem.Value + "' AND roomstatus='1' ORDER BY roomno";
        DataTable dtrry = objcls.DtTbl(query);
        gdbind.DataSource = dtrry;
        gdbind.DataBind();

    }
    protected void gdshow_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdshow.PageIndex = e.NewPageIndex;
        gdbind.Visible = false;
        string gt = @"SELECT reserve_no AS 'Reservation No',m_room.roomno AS 'Alloted Room',m_sub_building.buildingname AS 'Building Name' FROM t_roomreservation INNER JOIN m_room ON m_room.room_id = t_roomreservation.room_id  INNER JOIN m_sub_building ON m_room.build_id = m_sub_building.build_id WHERE status_reserve = 0 AND NOW() <= reservedate AND NOW() < expvacdate";
        DataTable dtgt = objcls.DtTbl(gt);
        gdshow.DataSource = dtgt;
        gdshow.DataBind();
    }

    protected void btnrealloc_Click(object sender, EventArgs e)
    {
        pnlt.Visible = false;
        pnlr.Visible = true;
        ddlbn.Visible = true;
        ddlrno.Visible = true;
        ddlbui.Visible = false;
        ddlr.Visible = false;
        string cc = @"SELECT DISTINCT room_cat_name,room_cat_id FROM m_sub_room_category,p_roomstatus,t_roomreservation_generaltdbtemp WHERE p_roomstatus.room_category_id=m_sub_room_category.room_cat_id AND rowstatus<>" + 2 + " AND t_roomreservation_generaltdbtemp.reserve_mode='General' AND NOW()< reservedate  AND t_roomreservation_generaltdbtemp.room_category_id=m_sub_room_category.room_cat_id ORDER BY p_roomstatus.room_category_id ASC ";
        DataTable dtcc = objcls.DtTbl(cc);
        ddlcid.DataValueField = "room_cat_id";
        ddlcid.DataTextField = "room_cat_name";
        ddlcid.DataSource = dtcc;
        ddlcid.DataBind();
        ddlcid.Items.Insert(0, "---select---");



    }
   
    

    
    protected void ddlcid_SelectedIndexChanged(object sender, EventArgs e)
    {
        string tt = @"SELECT DISTINCT m_sub_building.buildingname,m_room.build_id FROM m_sub_building,m_room WHERE m_room.build_id=m_sub_building.build_id AND room_cat_id='" + ddlcid.SelectedItem.Value + "' AND m_sub_building.rowstatus<>" + 2 + " ORDER BY buildingname asc";
        DataTable dtee = objcls.DtTbl(tt);
        ddlbn.DataValueField = "build_id";
        ddlbn.DataTextField = "buildingname";
        ddlbn.DataSource = dtee;
        ddlbn.DataBind();
        ddlbn.Items.Insert(0, "---select---");


    }
    protected void ddlbn_SelectedIndexChanged(object sender, EventArgs e)
    {
        string se = @"SELECT DISTINCT room_id,roomno FROM m_room WHERE room_id IN (SELECT DISTINCT room_id FROM t_roomreservation WHERE NOW() <= reservedate AND NOW() < expvacdate AND status_reserve='0') AND build_id='" + ddlbn.SelectedItem.Value + "' AND rowstatus!='2' AND room_cat_id='" + ddlcid.SelectedItem.Value + "' AND roomstatus='1' ORDER BY roomno";
        DataTable dtrry = objcls.DtTbl(se);

        ddlrno.DataValueField = "room_id";
        ddlrno.DataTextField = "roomno";
        ddlrno.DataSource = dtrry;
        ddlrno.DataBind();
        ddlrno.Items.Insert(0, "---select---");

    }
    protected void ddlrno_SelectedIndexChanged(object sender, EventArgs e)
    {
        gridbii();
    }
    public void gridbii()
    {
        string gg = @"SELECT reserve_no AS 'Reservation No',swaminame AS 'Swami Name' FROM t_roomreservation WHERE room_id = '" + ddlrno.SelectedItem.Value + "' AND NOW()<= reservedate  AND NOW()< expvacdate";
        DataTable dtg = objcls.DtTbl(gg);
        gdre.DataSource = dtg;
        gdre.DataBind();
    }


    protected void ddlbui_SelectedIndexChanged(object sender, EventArgs e)
    {


        string se = @"SELECT DISTINCT room_id,roomno FROM m_room WHERE room_id NOT IN (SELECT DISTINCT room_id FROM t_roomreservation WHERE NOW() <= reservedate AND NOW() < expvacdate AND status_reserve='0') AND build_id='" + ddlbui.SelectedItem.Value + "' AND rowstatus!='2' AND room_cat_id='" + ddlcid.SelectedItem.Value + "' AND roomstatus='1' ORDER BY roomno";
        DataTable dtrry = objcls.DtTbl(se);

        ddlr.DataValueField = "room_id";
        ddlr.DataTextField = "roomno";
        ddlr.DataSource = dtrry;
        ddlr.DataBind();
        ddlr.Items.Insert(0, "---select---");


    }

    protected void btnun_Click(object sender, EventArgs e)
    {
            try
            {
                ddlbui.Visible = true;
                ddlr.Visible = true;
                ddlrno.Visible = false;
                ddlbn.Visible = false;
                





                string tt = @"SELECT DISTINCT m_sub_building.buildingname,m_room.build_id FROM m_sub_building,m_room WHERE m_room.build_id=m_sub_building.build_id AND room_cat_id=" + ddlcid.SelectedItem.Value + " AND m_sub_building.rowstatus<>" + 2 + " ORDER BY buildingname asc";
                DataTable dtee = objcls.DtTbl(tt);
                ddlbui.DataValueField = "build_id";
                ddlbui.DataTextField = "buildingname";
                ddlbui.DataSource = dtee;
                ddlbui.DataBind();
                ddlbui.Items.Insert(0, "---select---");


              


                string reservv = gdre.Rows[0].Cells[0].Text;

                string up = @"UPDATE t_roomreservation SET status_reserve = '3' WHERE reserve_no = '" + reservv +"' AND NOW() <= reservedate AND NOW() < expvacdate";
                objcls.exeNonQuery(up);
                string updatetemp = "update t_roomreservation_generaltdbtemp set allot_status='0' where reserve_no='" + reservv + "' AND  NOW() <= reservedate AND NOW() < expvacdate";
                objcls.exeNonQuery(updatetemp);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Room Deallocated')", true);


            }
            catch
            {


                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('---Error----')", true);
            }


    }

    protected void btna_Click(object sender, EventArgs e)
    {
        try
        {

            string uuu = @"UPDATE t_roomreservation SET room_id ='" + ddlr.SelectedItem.Value + "', status_reserve = '0' WHERE reserve_no = '" + gdre.Rows[0].Cells[0].Text + "' AND NOW() <= reservedate AND NOW() < expvacdate";
            objcls.exeNonQuery(uuu);

            string updatetemp = "update t_roomreservation_generaltdbtemp set allot_status='1' where reserve_no='" + gdre.Rows[0].Cells[0].Text + "' AND  NOW() <= reservedate AND NOW() < expvacdate";
            objcls.exeNonQuery(updatetemp);



            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(' Room Reallocated Successfully')", true);
        }
        catch
        {

        }

    }



    
}
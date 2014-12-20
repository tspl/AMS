using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using clsDAL;

public partial class Room_Allotment : System.Web.UI.Page
{
    commonClass objcls = new commonClass();
    DataTable dtt = new DataTable();
    string fromdate,todate,totaldays,place,reserve_no,reserve_code;
    int n;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Title = "Tsunami ARMS - Room Allotment";
            try
            {
                n = int.Parse(Session["userid"].ToString());
            }
            catch
            {
                n = 1;
                Session["userid"] = n.ToString();
            }       
            string selectdate = "SELECT DISTINCT DATE_FORMAT(reservedate,'%d-%m-%Y') AS 'reservedate' FROM t_roomreservation_generaltdbtemp WHERE (DATE_FORMAT(reservedate,'%Y-%m-%d')) >= CURDATE()";
            DataTable dtselect_date = objcls.DtTbl(selectdate);
            DataRow rowdate = dtselect_date.NewRow();
            rowdate["reservedate"] = "-1";
            rowdate["reservedate"] = "--Select--";
            dtselect_date.Rows.InsertAt(rowdate, 0);
            cmbReserve.DataSource = dtselect_date;
            cmbReserve.DataBind();

            string selecttype = "SELECT id,TYPE FROM p_type_of_user where status=0";
            DataTable dtselect_type = objcls.DtTbl(selecttype);
            DataRow rowtype = dtselect_type.NewRow();
            rowtype["id"] = "-1";
            rowtype["TYPE"] = "--Select--";
            dtselect_type.Rows.InsertAt(rowtype, 0);
            cmbreservetype.DataSource = dtselect_type;
            cmbreservetype.DataBind();               
        }
    }
    public static DataTable roomavailable(string chkin, string chkout, int cat, int build)
    {
        commonClass obcls = new commonClass();
        string sel = @"(SELECT distinct cast(roomno AS CHAR(25)) AS 'roomno',t_roomreservation.room_id
                    FROM t_roomreservation,m_room
                    WHERE t_roomreservation.reservedate >= DATE_ADD('" + chkout + "' ,INTERVAL 2 HOUR)"
                    + " AND t_roomreservation.room_id NOT IN (SELECT t_roomreservation.room_id FROM t_roomreservation WHERE reservedate between DATE_ADD('" + chkin + "',INTERVAL -2 HOUR) AND DATE_ADD('" + chkout + "',INTERVAL +2 HOUR) OR expvacdate BETWEEN DATE_ADD('" + chkin + "',INTERVAL -2 HOUR) AND DATE_ADD('" + chkout + "',INTERVAL +2 HOUR))"
                    + " AND t_roomreservation.room_id=m_room.room_id"
                    + " AND m_room.room_cat_id=" + cat + " AND m_room.build_id=" + build + ""
                    + " ORDER BY t_roomreservation.reservedate)"
                    + " UNION "
                    + " (SELECT distinct CAST(m_room.roomno AS CHAR(25)) AS 'roomno',m_room.room_id FROM m_room"
                    + " WHERE  m_room.room_cat_id=" + cat + " AND m_room.build_id=" + build + " AND m_room.rowstatus <> 2 AND m_room.room_id  NOT IN (SELECT t_roomreservation.room_id FROM t_roomreservation WHERE t_roomreservation.reservedate"
                    + " BETWEEN DATE_ADD('" + chkin + "',INTERVAL -2 HOUR) AND DATE_ADD('" + chkout + "',INTERVAL +2 HOUR) OR t_roomreservation.expvacdate BETWEEN DATE_ADD('" + chkin + "',INTERVAL -2 HOUR) AND DATE_ADD('" + chkout + "',INTERVAL +2 HOUR))"
                    + " AND m_room.room_id NOT IN (SELECT t_roomallocation.room_id FROM t_roomallocation WHERE t_roomallocation.allocdate"
                    + " BETWEEN DATE_ADD('" + chkin + "',INTERVAL -2 HOUR) AND DATE_ADD('" + chkout + "',INTERVAL +2 HOUR) OR t_roomallocation.exp_vecatedate BETWEEN DATE_ADD('" + chkin + "',INTERVAL -2 HOUR) AND DATE_ADD('" + chkout + "',INTERVAL +2 HOUR))"
                    + " AND m_room.room_id NOT IN "
                    + " (SELECT room_id FROM t_manage_room WHERE DATE_FORMAT(CONCAT(fromdate,'" + " " + "',fromtime),'%Y/%m/%d %T') BETWEEN '" + chkin + "' AND '" + chkout + "' OR DATE_FORMAT(CONCAT(todate,'" + " " + "',totime),'%Y/%m/%d %T') BETWEEN '" + chkin + "' AND '" + chkout + "')"
                    + " ORDER BY m_room.room_id ASC)";
        DataTable dt_sel = obcls.DtTbl(sel);
        return dt_sel;
    }
    public void roomreservecheck()
    {
      
       
    }
    protected void cmbRooms_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectall = "select DATE_FORMAT(reservedate,'%Y-%m-%d %T'),DATE_FORMAT(expvacdate,'%Y-%m-%d %T') from t_roomreservation_generaltdbtemp where reserve_id=" + cmbSwaminame.SelectedValue;
        DataTable dtselectall = objcls.DtTbl(selectall);
        if (dtselectall.Rows.Count > 0)
        {
           string resdate = dtselectall.Rows[0][0].ToString();
           string expdate = dtselectall.Rows[0][1].ToString();


           OdbcCommand cmdRC = new OdbcCommand();
           cmdRC.Parameters.AddWithValue("tblname", "t_roomreservation");
           cmdRC.Parameters.AddWithValue("attribute", "reserve_mode,expvacdate");
           cmdRC.Parameters.AddWithValue("conditionv", "status_reserve ='" + "0" + "'  and room_id= " + int.Parse(cmbRooms.SelectedValue.ToString()) + " and  ('" + resdate + "' between reservedate and expvacdate or '" + expdate + "' between reservedate and expvacdate or reservedate between '" + resdate + "' and '" + expdate + "'  or expvacdate between '" + resdate + "' and '" + expdate + "'  )");
           DataTable drRC = new DataTable();
           drRC = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRC);
           if (drRC.Rows.Count > 0)
           {
               // Session["rescheck"] = "1";
               Session["resmode"] = drRC.Rows[0][0].ToString();
               cmbRooms.SelectedValue = "-1";
               okmessage("Tsunami ARMS - Information", "" + drRC.Rows[0][0].ToString() + " reserved.");
               return;
               //  check_exp_date = drRC.Rows[0][1].ToString();
           }
           else
           {
               //Session["rescheck"] = "0";
               // check_exp_date = "";
               // txtadvance.ReadOnly = true;
           }

           //string roomcheck = "SELECT room_id FROM t_roomreservation WHERE reservedate AND expvacdate BETWEEN '" + resdate + "' AND  '" + expdate + "'";
           // DataTable dtroomcheck = objcls.DtTbl(roomcheck);
           // if (dtroomcheck.Rows.Count > 0)
           // {
                
           //     int i;
           //     int j = dtroomcheck.Rows.Count;
           //     for (i = 0; i < j; i++)
           //     {
           //         string roomid = dtroomcheck.Rows[i]["room_id"].ToString();
           //         if (roomid == cmbRooms.SelectedValue)
           //         {
           //             cmbRooms.SelectedValue = "-1";
           //             okmessage("Tsunami ARMS - Information", "Select Another Room!");
           //             return;
           //         }
           //     }
           // }


            //change by san
            //string block = @"select * from t_manage_room where date_format(todate,'%Y')=date_format(curdate(),'%Y') and room_id=" + cmbRooms.SelectedValue;
            //DataTable dt_blk = objcls.DtTbl(block);
            //if (dt_blk.Rows.Count > 0)
            //{
            //    int k;
            //    int h = dt_blk.Rows.Count;
            //    for (k = 0; k < h; k++)
            //    {
            //        DateTime fdate = DateTime.ParseExact(dt_blk.Rows[k]["fromdate"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            //        DateTime ftime = DateTime.Parse(dt_blk.Rows[k]["fromtime"].ToString());
            //        DateTime fdate1 = new DateTime(fdate.Year,fdate.Month,fdate.Day,ftime.Hour,ftime.Minute,ftime.Second);
            //        DateTime tdate = DateTime.ParseExact(dt_blk.Rows[k]["fromdate"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            //        DateTime ttime = DateTime.Parse(dt_blk.Rows[k]["fromtime"].ToString());
            //        DateTime tdate1 = new DateTime(tdate.Year, tdate.Month, tdate.Day, ttime.Hour, ttime.Minute, ttime.Second);
            //        //string roomcheck1 = "SELECT room_id FROM  WHERE reservedate AND expvacdate BETWEEN '" + resdate + "' AND  '" + expdate + "'";
            //    }
            //}

            ///////////////
        }
    }    

    # region GRID LOADING  updated
    public void grid_load3(string w)
    {
        try
        {
            string strSelect = "t.reserve_id AS ReserveId,t.reserve_no AS ReservationNo,t.swaminame,"
                                                       + " CASE t.reserve_mode when 'General' then 'General' when 'TDB' then 'TDB' END as Customer,"
                                                       + " b.buildingname as Building,r.roomno as RoomNo,"
                                                       + " DATE_FORMAT(t.reservedate,'%d-%m-%y %l:%i %p') as ReservedDate,"
                                                       + " DATE_FORMAT(t.expvacdate,'%d-%m-%y %l:%i %p') as ExpectedVecatingDate";
            string strFrom = "m_room r,m_sub_building b,t_roomreservation t LEFT JOIN t_donorpass d ON  d.pass_id=t.pass_id";
            string strCond = "r.build_id=b.build_id and t.room_id=r.room_id and " + w.ToString() + " and t.reservedate>=curdate() order by reserve_id desc";
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", strFrom);
            cmd31.Parameters.AddWithValue("attribute", strSelect);
            cmd31.Parameters.AddWithValue("conditionv", strCond);
            dtt = objcls.SpDtTbl("call selectcond(?,?,?)", cmd31);
            dgReserve.DataSource = dtt;
            dgReserve.DataBind();
        }
        catch
        { }
    }
    # endregion

    protected void cmbBuild_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((cmbroomcategory.SelectedValue != "-1")&&(cmbBuild.SelectedValue != "-1"))
        {
           // roombind();


            OdbcCommand cmdRom = new OdbcCommand();
            cmdRom.Parameters.AddWithValue("tblname", "m_room");
            cmdRom.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
            cmdRom.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " and room_cat_id =" + cmbroomcategory.SelectedValue.ToString()+" and roomstatus=" + 1 + " order by roomno asc");
            OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdRom);
            DataTable dtt36 = new DataTable();
            dtt36 = objcls.GetTable(drr);
            DataRow row = dtt36.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dtt36.Rows.InsertAt(row, 0);
            dtt36.AcceptChanges();
            cmbRooms.DataSource = dtt36;
            cmbRooms.DataBind();
        }
    }
    protected void cmbroomcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string type = "";
        if (cmbreservetype.SelectedValue == "1")
        {
            type = "General";
        }
        else
        {
            type = cmbreservetype.SelectedItem.ToString();
        }
        OdbcCommand cmdswaminame = new OdbcCommand();
        cmdswaminame.Parameters.AddWithValue("tblname", "t_roomreservation_generaltdbtemp");
        cmdswaminame.Parameters.AddWithValue("attribute", "swaminame,reserve_id");
        cmdswaminame.Parameters.AddWithValue("conditionv", "room_category_id =" + int.Parse(cmbroomcategory.SelectedValue.ToString()) + " and reserve_mode='" + type + "' and allot_status=" + 0 + " AND DATE_FORMAT(reservedate,'%d-%m-%Y')='" + cmbReserve.SelectedItem + "'");
        DataTable dtswaminame = new DataTable();
        dtswaminame = objcls.SpDtTbl("call selectcond(?,?,?)", cmdswaminame);
        DataRow rowswami = dtswaminame.NewRow();
        rowswami["reserve_id"] = "-1";
        rowswami["swaminame"] = "--Select--";
        dtswaminame.Rows.InsertAt(rowswami, 0);
        cmbSwaminame.DataSource = dtswaminame;
        cmbSwaminame.DataBind();

        OdbcCommand cmdswaminamexx = new OdbcCommand();
        cmdswaminamexx.Parameters.AddWithValue("tblname", "t_roomreservation_generaltdbtemp");
        cmdswaminamexx.Parameters.AddWithValue("attribute", "reserve_no");
        cmdswaminamexx.Parameters.AddWithValue("conditionv", "room_category_id =" + int.Parse(cmbroomcategory.SelectedValue.ToString()) + " and reserve_mode='" + type + "' and allot_status=" + 0 + " AND DATE_FORMAT(reservedate,'%d-%m-%Y')='" + cmbReserve.SelectedItem + "'");
        DataTable dtswaminamexx = new DataTable();
        dtswaminamexx = objcls.SpDtTbl("call selectcond(?,?,?)", cmdswaminamexx);
        DataRow rowswamixxx = dtswaminamexx.NewRow();
        rowswamixxx["reserve_no"] = "-1";
        rowswamixxx["reserve_no"] = "--Select--";
        dtswaminamexx.Rows.InsertAt(rowswamixxx, 0);
        ddlreservno.DataSource = dtswaminamexx;
        ddlreservno.DataBind();         

        
    }
    protected void cmbreservetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        string type = "";
        if (cmbreservetype.SelectedValue == "1")
        {
            type = "General";
        }
        else
        {
            type = cmbreservetype.SelectedItem.ToString();
        }

        string reservetypeselect = "SELECT DISTINCT room_category_id FROM t_roomreservation_generaltdbtemp WHERE reserve_mode='" + type + "' AND DATE_FORMAT(reservedate,'%d-%m-%Y')='" + cmbReserve.SelectedItem + "'";        
        DataTable dtreservetypeselect = objcls.DtTbl(reservetypeselect);
        if (dtreservetypeselect.Rows.Count > 0)
        {
            OdbcCommand cmdroomcategory = new OdbcCommand();
            cmdroomcategory.Parameters.AddWithValue("tblname", "m_sub_room_category,p_roomstatus,t_roomreservation_generaltdbtemp");
            cmdroomcategory.Parameters.AddWithValue("attribute", "DISTINCT room_cat_name,room_cat_id");
            cmdroomcategory.Parameters.AddWithValue("conditionv", "p_roomstatus.room_category_id=m_sub_room_category.room_cat_id and rowstatus<>" + 2 + " AND t_roomreservation_generaltdbtemp.reserve_mode='" + type + "' AND DATE_FORMAT(reservedate,'%d-%m-%Y')='" + cmbReserve.SelectedItem + "' AND t_roomreservation_generaltdbtemp.room_category_id=m_sub_room_category.room_cat_id ORDER BY p_roomstatus.room_category_id asc");
            DataTable dtroomcategory = new DataTable();
            dtroomcategory = objcls.SpDtTbl("Call selectcond(?,?,?)", cmdroomcategory);
            DataRow rowroomcat = dtroomcategory.NewRow();
            rowroomcat["room_cat_id"] = "-1";
            rowroomcat["room_cat_name"] = "--Select--";
            dtroomcategory.Rows.InsertAt(rowroomcat, 0);
            cmbroomcategory.DataSource = dtroomcategory;
            cmbroomcategory.DataBind();
        }
        else
        {
            okmessage("Tsunami ARMS - Information", "No Data Found");
        }
    }
    protected void cmbreservemode_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void dgReserve_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = dgReserve.SelectedRow;
    }
    protected void dgReserve_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgReserve.PageIndex = e.NewPageIndex;
        dgReserve.DataBind();
    }
    protected void dgReserve_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgReserve, "Select$" + e.Row.RowIndex);
        }
    }
    protected void cmbSwaminame_SelectedIndexChanged(object sender, EventArgs e)
    {
        string select = "SELECT DATE_FORMAT(reservedate,'%d-%m-%Y') AS 'reservedate' FROM t_roomreservation_generaltdbtemp WHERE reserve_id="+cmbSwaminame.SelectedValue+"  AND reserve_no = '"+ddlreservno.SelectedValue+"' ";
        DataTable dtselect = objcls.DtTbl(select);
        if (dtselect.Rows.Count > 0)
        {
           string date = dtselect.Rows[0]["reservedate"].ToString();
           if (cmbReserve.SelectedValue == date)
           {
               OdbcCommand da = new OdbcCommand();
               da.Parameters.AddWithValue("tblname", "m_sub_building,m_room");
               da.Parameters.AddWithValue("attribute", "DISTINCT m_sub_building.buildingname,m_room.build_id");
               da.Parameters.AddWithValue("conditionv", "m_room.build_id=m_sub_building.build_id AND room_cat_id=" + int.Parse(cmbroomcategory.SelectedValue.ToString()) + " and m_sub_building.rowstatus<>" + 2 + " order by buildingname asc");
               DataTable dtt1 = new DataTable();
               dtt1 = objcls.SpDtTbl("Call selectcond(?,?,?)", da);
               DataRow row11b = dtt1.NewRow();
               row11b["build_id"] = "-1";
               row11b["buildingname"] = "--Select--";
               dtt1.Rows.InsertAt(row11b, 0);
               cmbBuild.DataSource = dtt1;
               cmbBuild.DataBind();
           }
        }
        else
        {
            okmessage("Tsunami ARMS - Information", "No Data Found");
        }       
    }
    private void roombind()
    {
        string select = "SELECT DATE_FORMAT(reservedate,'%Y/%m/%d %T') as 'reservedate',DATE_FORMAT(expvacdate,'%Y/%m/%d %T') as 'expvacdate' FROM t_roomreservation_generaltdbtemp WHERE reserve_id=" + cmbSwaminame.SelectedValue+" AND reserve_no ='"+ddlreservno.SelectedValue+"'"; 
        DataTable dtselect = objcls.DtTbl(select);
        //OdbcCommand da = new OdbcCommand();
        //da.Parameters.AddWithValue("tblname", "m_room");
        //da.Parameters.AddWithValue("attribute", "distinct cast(roomno AS CHAR(25)) AS 'roomno',room_id");
        //da.Parameters.AddWithValue("conditionv", "room_cat_id =" + int.Parse(cmbroomcategory.SelectedValue.ToString()) + " and build_id=" + cmbBuild.SelectedValue + " and rowstatus<>" + 2 + " and (roomstatus=" + 1 + " OR roomstatus=" + 4 + ")  order by room_id asc");
        //change by san//start

        //da.Parameters.AddWithValue("conditionv", "room_cat_id =" + int.Parse(cmbroomcategory.SelectedValue.ToString()) + " and build_id=" + cmbBuild.SelectedValue + " and order by room_id asc");
        
        //change by san//end
        DataTable dtt = new DataTable();
        //dtt = roomavailable(Convert.ToDateTime(dtselect.Rows[0]["reservedate"]), Convert.ToDateTime(dtselect.Rows[0]["expvacdate"]), int.Parse(cmbroomcategory.SelectedValue), int.Parse(cmbBuild.SelectedValue));
        dtt = roomavailable(dtselect.Rows[0]["reservedate"].ToString(),dtselect.Rows[0]["expvacdate"].ToString(), int.Parse(cmbroomcategory.SelectedValue), int.Parse(cmbBuild.SelectedValue));
        DataRow row5 = dtt.NewRow();
        row5["room_id"] = "-1";
        row5["roomno"] = "--Select--";
        dtt.Rows.InsertAt(row5, 0);
        cmbRooms.DataSource = dtt;
        cmbRooms.DataBind();
    }
    protected void cmbReserve_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnAllot_Click(object sender, EventArgs e)
    {
        if ((cmbRooms.SelectedValue != "-1") && (cmbreservetype.SelectedValue != "-1") && (cmbSwaminame.SelectedValue != "-1") && (cmbReserve.SelectedValue != "-1") && (cmbroomcategory.SelectedValue != "-1"))
        {

            string type = "";
            if (cmbreservetype.SelectedValue == "1")
            {
                type = "General";
            }
            else
            {
                type = cmbreservetype.SelectedItem.ToString();
            }

            string selectall = "select * from t_roomreservation_generaltdbtemp where reserve_id=" + cmbSwaminame.SelectedValue;
            DataTable dtselectall = objcls.DtTbl(selectall);
            if (dtselectall.Rows.Count > 0)
            {
                fromdate = dtselectall.Rows[0]["reservedate"].ToString();
                todate = dtselectall.Rows[0]["expvacdate"].ToString();
                totaldays = dtselectall.Rows[0]["total_days"].ToString();
                place = dtselectall.Rows[0]["place"].ToString();
                reserve_code = dtselectall.Rows[0]["reserve_no"].ToString();
            }
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
            string reserveroom = "INSERT INTO t_roomreservation(reserve_id,reserve_type,reserve_mode,swaminame,reservedate,expvacdate,total_days,status_reserve,passmode,createdby,cretaedon,updatedby,updateddate,place,room_id,reserve_no) VALUES ( " + pk + ",'Single','" + type + "','" + cmbSwaminame.SelectedItem.Text + "','" + fromdate + "','" + todate + "'," + totaldays + ",0,0," + n + ",'" + date + "'," + n + ",'" + date + "','" + place + "'," + cmbRooms.SelectedValue + ",'" + reserve_code + "')";
            int flag = objcls.exeNonQuery(reserveroom);
            if (flag == 1)
            {
                 string updatetemp = "update t_roomreservation_generaltdbtemp set allot_status=" + 1 + " where reserve_id=" + cmbSwaminame.SelectedValue + "";
                int lcheck=objcls.exeNonQuery(updatetemp);
                if(lcheck!=1)
                {
                    string del = @"delete from t_roomreservation where reserve_id=" + pk + "";
                    objcls.exeNonQuery(del);
                }
            }
            okmessage("Tsunami ARMS - Information", "Alloted Successfully");
            grid_load3("t.status_reserve=" + 0 + "");
            Clear();
        }
        else
        {
            okmessage("Tsunami ARMS - Information", "Please Select Required Data");
        }       
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        if ((cmbreservetype.SelectedValue != "-1") && (cmbSwaminame.SelectedValue != "-1") && (cmbReserve.SelectedValue != "-1")&&(cmbroomcategory.SelectedValue!="-1"))
        {
            string gridselect = "SELECT DATE_FORMAT(reservedate,'%d-%m-%y') AS 'Reservedate',swaminame as 'Swaminame',reserve_mode as 'Reserve Type',room_cat_name AS 'Room Category' FROM t_roomreservation_generaltdbtemp,m_sub_room_category WHERE reserve_id=" + cmbSwaminame.SelectedValue + " and reserve_mode='" + cmbreservetype.SelectedItem + "' AND t_roomreservation_generaltdbtemp.room_category_id=m_sub_room_category.room_cat_id";
            DataTable dtgrid = objcls.DtTbl(gridselect);
            if (dtgrid.Rows.Count > 0)
            {
                dgAllot.DataSource = dtgrid;
                dgAllot.DataBind();
            }
            else
            {
                okmessage("Tsunami ARMS - Information", "No Data Found");
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Information", "Please Select Required Data");
        }
    }
    protected void btnViewAllot_Click(object sender, EventArgs e)
    {
        if (dgReserve.Visible == false)
        {
            grid_load3("t.status_reserve=" + 0 + "");
            dgReserve.Visible = true;
        }
        else
        {
            dgReserve.Visible = false;
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        cmbBuild.SelectedValue = "-1";
        cmbReserve.SelectedIndex = 0;
        cmbreservemode.SelectedValue = "-1";
        cmbreservetype.SelectedValue = "-1";
        cmbroomcategory.SelectedValue = "-1";
        cmbRooms.SelectedValue = "-1";
        cmbSwaminame.DataSource = null;
        cmbSwaminame.DataBind();
        cmbSwaminame.SelectedValue = "-1";
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    protected void btnYes_Click(object sender, EventArgs e)
    {

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        if (ViewState["action"] == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
        if (ViewState["action"] == "allot")
        {
            
        }
    }
    #region OK Message
    public void okmessage(string head, string message)
    {
        lblHead2.Visible = true;
        lblHead.Visible = false;
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender2.Show();
    }
    #endregion
    protected void ddlreservno_SelectedIndexChanged(object sender, EventArgs e)
    {
        string type = "";
        if (cmbreservetype.SelectedValue == "1")
        {
            type = "General";
        }
        else
        {
            type = cmbreservetype.SelectedItem.ToString();
        }
        OdbcCommand cmdswaminame = new OdbcCommand();
        cmdswaminame.Parameters.AddWithValue("tblname", "t_roomreservation_generaltdbtemp");
        cmdswaminame.Parameters.AddWithValue("attribute", "swaminame,reserve_id");
        cmdswaminame.Parameters.AddWithValue("conditionv", "room_category_id =" + int.Parse(cmbroomcategory.SelectedValue.ToString()) + " and reserve_mode='" + type + "' and allot_status=" + 0 + " AND DATE_FORMAT(reservedate,'%d-%m-%Y')='" + cmbReserve.SelectedItem + "' AND  reserve_no = '"+ddlreservno.SelectedValue+"' ");
        DataTable dtswaminame = new DataTable();
        dtswaminame = objcls.SpDtTbl("call selectcond(?,?,?)", cmdswaminame);
        DataRow rowswami = dtswaminame.NewRow();
        rowswami["reserve_id"] = "-1";
        rowswami["swaminame"] = "--Select--";
        dtswaminame.Rows.InsertAt(rowswami, 0);
        cmbSwaminame.DataSource = dtswaminame;
        cmbSwaminame.DataBind();
    }
}
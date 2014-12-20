using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using clsDAL;
using System.Threading;
//using System.Windows.Forms;
//using System.Web.UI.We


public partial class display : System.Web.UI.Page
{
    clsCommon obj = new clsCommon();
    commonClass objcls = new commonClass();
    OdbcCommand con = new OdbcCommand();
    static DataTable dfg = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindmain();
            btnhold.Text = "Hold";
            timer1.Enabled = true;
            timer1.Interval = 3000;
            timer1.Tick += new EventHandler<EventArgs>(timer1_Tick);
        }
        //UpdateColors();
    }
    private void bindmain()
    {
        string selectbuild = @" SELECT DISTINCT build_id,buildingname FROM m_sub_building WHERE rowstatus='0' AND build_id BETWEEN 1 AND 12";
        dfg = objcls.DtTbl(selectbuild);
    }
    public void disply(string build_id)
    {
        string selectroom = @"SELECT roomno,roomstatus FROM m_room WHERE build_id='" + build_id + "'AND rowstatus='0' ";
        DataTable dtroomno = objcls.DtTbl(selectroom);

        DataList1.DataSource = dtroomno;
        DataList1.DataBind();
        //test
        foreach (DataListItem item in DataList1.Items)
        {
            Label lbl2 = (Label)item.FindControl("Label2");


            switch (lbl2.Text)
            {
                case "1":
                    item.BackColor = System.Drawing.Color.White;
                    item.ForeColor = System.Drawing.Color.Black;
                    break;
                case "2":
                    item.BackColor = System.Drawing.Color.Red;
                    item.ForeColor = System.Drawing.Color.White;
                    break;
                case "3":
                    item.BackColor = System.Drawing.Color.Black;
                    item.ForeColor = System.Drawing.Color.White;
                    break;
                case "4":
                    item.BackColor = System.Drawing.Color.Blue;
                    item.ForeColor = System.Drawing.Color.White;
                    break;
            }
        }
        
    }
    //test
    
    private void check()
    {
        //string chk = @"SELECT roomstatus FROM  m_room WHERE roomno='"+DataList1.+"'AND build_id='" + build_id + "'";
    }

    protected void btnhold_Click(object sender, EventArgs e)
    {
        if (btnhold.Text == "Hold")
        {
            btnhold.Text = "Release";
            string build_id = dfg.Rows[0][0].ToString();
            string buildname = dfg.Rows[0][1].ToString();
            lblBuild.Text = buildname;
            disply(build_id);
            timer1.Enabled = false;
        }
        else
            if (btnhold.Text == "Release")
            {
                btnhold.Text = "Hold";
                timer1.Enabled = true;
            }

    }
    protected void timer1_Tick(object sender, EventArgs e)
    {
        try
        {
            if (dfg.Rows.Count > 0)
            {
                string build_id = dfg.Rows[0][0].ToString();
                string buildname = dfg.Rows[0][1].ToString();
                lblBuild.Text = buildname;
                disply(build_id);
                dfg.Rows.RemoveAt(0);
            }
            else
            {
                bindmain();
                string build_id = dfg.Rows[0][0].ToString();
                string buildname = dfg.Rows[0][1].ToString();
                lblBuild.Text = buildname;
                disply(build_id);
                dfg.Rows.RemoveAt(0);
            }
        }
        catch
        {
        }
    }
    protected void timer2_Tick(object sender, EventArgs e)
    {
        try
        {
            if (dfg.Rows.Count > 0)
            {
                string build_id = dfg.Rows[0][0].ToString();
                string buildname = dfg.Rows[0][1].ToString();
                lblBuild.Text = buildname;
                disply(build_id);
                dfg.Rows.RemoveAt(0);
            }
            else
            {
                bindmain();
                string build_id = dfg.Rows[0][0].ToString();
                string buildname = dfg.Rows[0][1].ToString();
                lblBuild.Text = buildname;
                disply(build_id);
                dfg.Rows.RemoveAt(0);
            }
        }
        catch
        {
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        //btnhold.Text = "Hold";
        //timer1.Enabled = true;
    }
}
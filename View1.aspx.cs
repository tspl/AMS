using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class View1 : System.Web.UI.Page
{
    public String head = "";
    clsgridview obj = new clsgridview();
    protected void Page_Load(object sender, EventArgs e)
    {
        dtgview.DataSource = Session["DataTable"];
        dtgview.DataBind();
        lblHeading.Text = Session["head"].ToString();
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void loadGrid(DataTable dt)
    {
        DataTable dtview = dt;
        dtgview.DataSource = dt;
        dtgview.DataBind();
    }
    protected void dtgview_RowCreated(object sender, GridViewRowEventArgs e)
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
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView header = (GridView)sender;
            GridViewRow gvr = new GridViewRow(0, 0,
                DataControlRowType.Header,
                DataControlRowState.Insert);
            TableCell tCell = new TableCell();
            tCell.Text = Session["head"].ToString();
            tCell.ColumnSpan = 15;
            tCell.HorizontalAlign = HorizontalAlign.Center;
            gvr.Cells.Add(tCell);
            // Add the Merged TableCell to the GridView Header
            Table tbl = dtgview.Controls[0] as Table;
            if (tbl != null)
            {
                tbl.Rows.AddAt(0, gvr);
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells.Clear();
            TableCell tCell = new TableCell();
            tCell.ColumnSpan = 15;
            tCell.Text = "Page No: " + (dtgview.PageIndex + 1) + " of "
                                              + dtgview.PageCount;
            e.Row.Cells.Add(tCell);
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Font.Bold = true;
        }
    }
    protected void dtgview_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells.Clear();
            TableCell tCell = new TableCell();
            tCell.ColumnSpan = 15;
            tCell.Text = "Page No: " + (dtgview.PageIndex + 1) + " of "
                                              + dtgview.PageCount;
            e.Row.Cells.Add(tCell);
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Font.Bold = true;
        }
    }
    protected void dtgview_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dtt = Session["DataTable"] as DataTable;
        //Retrieve the table from the session object.
        if (dtt != null)
        {
            DataView dv = new DataView(dtt);
            dv.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            dtgview.DataSource = dv;
            dtgview.DataBind();
        }
    }
    private string GetSortDirection(string column)
    {
        // By default, set the sort direction to ascending.
        string sortDirection = "ASC";
        // Retrieve the last column that was sorted.
        string sortExpression = ViewState["SortExpression"] as string;
        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }
        // Save new values in ViewState.
        ViewState["SortDirection"] = sortDirection;
        ViewState["SortExpression"] = column;
        return sortDirection;
    }
}

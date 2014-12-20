using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Data.Odbc;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Collections.Specialized;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using System.Web.Configuration;
using System.Net.Mail;
/// <summary>
/// Summary description for clsCommon
/// </summary>
public class clsCommon
{
    public static string strDateFormatShow="dd/MM/yyyy";
    public static string strDateFormatSave="dd/MM/yyyy";
    public static string[] nv = new string[2];
    public static string[] rol = new string[2];
    public static string[] hk = new string[2];
    public static string[] rbno = new string[2];
    public static string[] rba = new string[2];
    public static string[] cl = new string[2];
    public static string[] rv24 = new string[2];
    public static string PrintType;
	public clsCommon()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string ConnectionString()
    {
        return ConfigurationManager.AppSettings["ConnectionString"].ToString();
    }
    //check user rights
    public int CheckUserRight(string strFormName, int iPrevLevel)
    {
        OdbcConnection myConn = new OdbcConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        int iCheck = 0;
        try
        {
            myConn.Open();
            string strSql="SELECT   count(*)  "
                          + " FROM "
                                    + " m_userprev_formset,m_sub_form "
                        + "  WHERE "
                                    + " prev_level=" + iPrevLevel + " and m_sub_form.form_id=m_userprev_formset.form_id  and "
                                    + " formname ='" + strFormName + "' ";
            OdbcCommand check = new OdbcCommand(strSql, myConn);
            OdbcDataReader rd = check.ExecuteReader();
            while (rd.Read())
            {
                iCheck = Convert.ToInt32(rd[0]);
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            myConn.Close();
        }
        return iCheck;
    }
    // Gets all the countries that start with the typed text, taking paging into account
    public  DataTable GetCountries(string strSearchText,string strTableName,string strTextField,string strValueField,string strOrderBy,string strWhereCondition)
    {
        OdbcConnection myConn = new OdbcConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        myConn.Open();
        string whereClause = " WHERE " + strTextField + " LIKE  '" + strSearchText+'%' + "' ";
        if (strWhereCondition != "")
        {
            whereClause = whereClause + " and " + strWhereCondition;
        }
        string sortExpression = "  ORDER BY " + strOrderBy + " LIMIT 25 ";
        string commandText = "SELECT " + strTextField + "," + strValueField + "  FROM " + strTableName + " ";
        commandText += whereClause;
        commandText += sortExpression;
        OdbcCommand myComm = new OdbcCommand(commandText, myConn);
        OdbcDataAdapter da = new OdbcDataAdapter();
        DataSet ds = new DataSet();
        da.SelectCommand = myComm;
        da.Fill(ds, strTableName);
        myConn.Close();
        return ds.Tables[0];
    }

    public void ShowAlertMessage(System.Web.UI.Page obj, string error)
    {

        Page page = HttpContext.Current.Handler as Page;

        if (page != null)
        {

            error = error.Replace("'", "\'");

            ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "alert('" + error + "');", true);

        }

    }
}

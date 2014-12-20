
using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Data.Odbc;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class AutoComplete : WebService
{
    public AutoComplete()
    {
    }

    [WebMethod]
    public string[] GetCompletionList(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }
        List<String> result = new List<string>();
        using (OdbcConnection connection = new OdbcConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tdbnewConnectionString"].ConnectionString))
        {
            connection.Open();
            OdbcCommand cmd = new OdbcCommand(string.Format("Select office from officemaster Where office Like '{1}%'", count, prefixText), connection);
            OdbcDataReader reader = cmd.ExecuteReader();
            if (reader != null)
            {
                while (reader.Read())
                {
                    result.Add(Convert.ToString(reader["office"]));
                }
            }
        }

        return result.ToArray();
    }
}


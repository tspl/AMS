using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;

/// <summary>
/// Summary description for Service
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
//[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService {

    public Service () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(true)]
    public string[] GetCountriesList(string prefixText, int count)
    {
        if (count == 0)
            count = 10;

        List<String> result = new List<string>();
        using (OdbcConnection connection = new OdbcConnection(System.Configuration.ConfigurationManager.ConnectionStrings["tdbnewConnectionString"].ConnectionString))
        {
            connection.Open();
            OdbcCommand cmd = new OdbcCommand(string.Format("Select office from officemaster Where Name Like '{1}%'", count, prefixText), connection);
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
    /// <summary>
    /// Method to get Formatted String value which can be used for KeyValue Pair for AutoCompleteExtender
    /// </summary>
    /// <param name="value"></param>
    /// <param name="id"></param>
    /// <returns>Returns string value which holds key and value in a specific format</returns>
    private string AutoCompleteItem(string value, string id)
    {
        return string.Format("{{\"First\":\"{0}\",\"Second\":\"{1}\"}}", value, id);
    }

   
}


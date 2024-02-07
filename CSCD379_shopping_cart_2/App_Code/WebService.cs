using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{

    public WebService() {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod]
    public string[] GetCityState(string zipCode) {
        
        SqlConnection databaseConnection = null;
        SqlCommand databaseCommand = null;

        SqlDataReader drState = null;
        SqlDataReader drCity = null;

        String inState = "";
        String inStateAbb = "";
        String inCity = "";
        String inStateCode = "";

        try {

            databaseConnection = new SqlConnection();
            databaseConnection.ConnectionString = WebConfigurationManager.ConnectionStrings["CityLookup"].ConnectionString;
            databaseCommand = new SqlCommand("select City, State_Code from ZIP_codes where ZIPcode=@zipCode", databaseConnection);
            databaseCommand.Parameters.AddWithValue("@zipCode", zipCode);

            databaseConnection.Open();

            drCity = databaseCommand.ExecuteReader();

            if (drCity.Read()) {
                inCity = drCity["City"].ToString();
                inStateCode = drCity["State_Code"].ToString();
            }

            databaseCommand = new SqlCommand("select StateAbbreviation, StateName from States where StateCode=@STCDE", databaseConnection);
            databaseCommand.Parameters.AddWithValue("@STCDE", inStateCode);

            drState = databaseCommand.ExecuteReader();
            if (drState.Read()) {
                inState = drState["StateName"].ToString();
                inStateAbb = drState["StateAbbreviation"].ToString();
            }

        }
        catch (Exception err) {

        } finally {
            if (databaseConnection != null) {
                databaseConnection.Close();
            }
        }

        String[] returnMe = new string[3];
        returnMe[0] = inCity;
        returnMe[1] = inStateCode;
        returnMe[2] = inStateAbb;
        
        return returnMe;
    }
           

}

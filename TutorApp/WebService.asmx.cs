using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace TutorApp
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public int NumberOfAccounts()
        {
            //grab connection string from web.config file
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["tutorDB"].ConnectionString;
            //Select Query
            string sqlSelect = "SELECT * from users";

            //set up connection object to be ready to use our connection string
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            //set up command object to use our connection, and our query
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            //a data adapter acts like a bridge between our command object and
            //the data we are trying to get back and put in a table object
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            //here's the table we want to fill with the results from our query
            DataTable sqlDt = new DataTable();
            //fill it:
            sqlDa.Fill(sqlDt);
            //return number of rows we have, thats how many are in the system.
            return sqlDt.Rows.Count;
        }
    }
}

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

        [WebMethod(EnableSession = true)] //NOTICE: gotta enable session on each individual method
        public bool LogOn(string userName, string userPassword)
        {
            //we return this flag to tell them if they logged in or not
            bool success = false;

            //our connection string comes from our web.config file like we talked about earlier
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["tutorDB"].ConnectionString;
            //here's our query.  A basic select with nothing fancy.  Note the parameters that begin with @
            //NOTICE: we added admin to what we pull, so that we can store it along with the id in the session
            string sqlSelect = "SELECT userName, userPassword FROM users WHERE userName=@idValue and userPassword=@passValue";

            //set up our connection object to be ready to use our connection string
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            //set up our command object to use our connection, and our query
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            //tell our command to replace the @parameters with real values
            //we decode them because they came to us via the web so they were encoded
            //for transmission (funky characters escaped, mostly)
            sqlCommand.Parameters.AddWithValue("@idValue", HttpUtility.UrlDecode(userName));
            sqlCommand.Parameters.AddWithValue("@passValue", HttpUtility.UrlDecode(userPassword));

            //a data adapter acts like a bridge between our command object and 
            //the data we are trying to get back and put in a table object
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            //here's the table we want to fill with the results from our query
            DataTable sqlDt = new DataTable();
            //here we go filling it!
            sqlDa.Fill(sqlDt);
            //check to see if any rows were returned.  If they were, it means it's 
            //a legit account

            //SQ changed this
            if (sqlDt.Rows.Count > 0)
            {
                //if we found an account, store the id and admin status in the session
                //so we can check those values later on other method calls to see if they 
                //are 1) logged in at all, and 2) and admin or not


                //Session["id"] = sqlDt.Rows[0]["id"];
                //Session["admin"] = sqlDt.Rows[0]["admin"];
                success = true;
            }
            //return the result!
            return success;
        }

        [WebMethod(EnableSession = true)] //NOTICE: gotta enable session on each individual method
        public Account GetUserInfo(string userName, string userPassword)
        {
            Account thisAccount;

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["tutorDB"].ConnectionString;
            string sqlSelect = "SELECT userID, firstName, lastName, phoneNumber, userEmail, userType FROM users WHERE userName=@idValue and userPassword=@passValue";

            //set up our connection object to be ready to use our connection string
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            //set up our command object to use our connection, and our query
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idValue", HttpUtility.UrlDecode(userName));
            sqlCommand.Parameters.AddWithValue("@passValue", HttpUtility.UrlDecode(userPassword));

            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            //here's the table we want to fill with the results from our query
            DataTable sqlDt = new DataTable();
            //here we go filling it!
            sqlDa.Fill(sqlDt);

            //if (sqlDt.Rows[0] != null)
            //{
            string firstname = (string)sqlDt.Rows[0]["firstName"];
            int userID = (int)sqlDt.Rows[0]["userID"];
            string lastname = (string)sqlDt.Rows[0]["lastName"];
            string userEmail = (string)sqlDt.Rows[0]["userEmail"];
            string userType = (string)sqlDt.Rows[0]["userType"];
            string phoneNo = (string)sqlDt.Rows[0]["phoneNumber"];

            thisAccount = new Account(userID, userName, firstname, lastname, userEmail, phoneNo, userType);

            return thisAccount;
            //}
        }

        //SQ: WEB METHOD NOT DONE. Just got it started
        [WebMethod(EnableSession = true)] //NOTICE: gotta enable session on each individual method
        public List<Account> FindTutor(string courseName)
        {
            List<Account> relAccount;

            relAccount = new List<Account>;

            return relAccount;
        }

        
    }
}

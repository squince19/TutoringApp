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
            string sqlSelect = "SELECT userID, userName, userPassword FROM users WHERE userName=@idValue and userPassword=@passValue";

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


                Session["id"] = sqlDt.Rows[0]["userID"];
                //Session["admin"] = sqlDt.Rows[0]["admin"];
                success = true;
            }
            //return the result!
            return success;
        }

        [WebMethod(EnableSession = true)] //NOTICE: gotta enable session on each individual method
        public Account GetUserInfo()
        {
            Account thisAccount;
            int i = Convert.ToInt32(Session["id"]);
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["tutorDB"].ConnectionString;
            string sqlSelect = "SELECT userID, firstName, lastName, phoneNumber, userEmail, userType FROM users WHERE userID=@idValue";

            //set up our connection object to be ready to use our connection string
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            //set up our command object to use our connection, and our query
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idValue", HttpUtility.UrlDecode(i.ToString()));
            //sqlCommand.Parameters.AddWithValue("@passValue", HttpUtility.UrlDecode(userPassword));

            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            //here's the table we want to fill with the results from our query
            DataTable sqlDt = new DataTable();
            //here we go filling it!
            sqlDa.Fill(sqlDt);

            //if (sqlDt.Rows[0] != null)
            //{
            string firstname = (string)sqlDt.Rows[0]["firstName"];
            int userID = i;
            string lastname = (string)sqlDt.Rows[0]["lastName"];
            string userEmail = (string)sqlDt.Rows[0]["userEmail"];
            string userType = (string)sqlDt.Rows[0]["userType"];
            string phoneNo = (string)sqlDt.Rows[0]["phoneNumber"];

            thisAccount = new Account(userID, firstname, lastname, userEmail, phoneNo, userType);

            return thisAccount;
            //}
        }

        [WebMethod(EnableSession = true)] //NOTICE: gotta enable session on each individual method
        public List<Account> FindTutor(string courseName)
        {
            //web method will return a list of accounts that are proficient in the users course

            List<Account> relAccount;

            relAccount = new List<Account>();

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["tutorDB"].ConnectionString;
            string sqlSelect = "SELECT userID, firstName, lastName, phoneNumber, userEmail FROM users WHERE userType='tutor' and courseProf=@courseValue"; //finish this with course type;
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@courseValue", HttpUtility.UrlDecode(courseName));

            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDa.Fill(dataTable);

            List<Account> tempList = new List<Account>();
            foreach (DataRow row in dataTable.Rows)
            {
                int idValue = Convert.ToInt32(row["userID"]);
                Account tempAcc = new Account(
                    idValue,
                    row["firstName"].ToString(),
                    row["lastName"].ToString(),
                    row["phoneNumber"].ToString(),
                    row["userEmail"].ToString()
                    );

                tempList.Add(tempAcc);
            }

            int i = tempList.Count;
            int[] tempArray = new int[3];
            Random r = new Random();

            //do loop generates random numbers, and also checks to make sure that they are not already in the array
            //It will generate numbers until the array is full of unique numbers
            do
            {
                for (int x = 0; x < 3; x++)
                {
                    tempArray[x] = r.Next(1, i);
                }
            } while (tempArray[0] == tempArray[1] || tempArray[1] == tempArray[2] || tempArray[2] == tempArray[0]);


            int one, two, three;
            one = tempArray[0];
            two = tempArray[1];
            three = tempArray[2];

            relAccount.Add(tempList[one]);
            relAccount.Add(tempList[two]);
            relAccount.Add(tempList[three]);

            return relAccount;
        }//END WEB METHOD

        [WebMethod(EnableSession = true)] 
        public void AddUser(string userType, string fname, string lname, string phoneNumber, 
            string userName, string email, string password)
        {
            Account newAcc = new Account(fname, lname, phoneNumber, email, userType);

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["tutorDB"].ConnectionString;
            string sqlInsert = "insert into users ( userName, userPassword, userEmail, userType," +
                " firstName, lastName, phoneNumber) values(@userName, @password, @email, @userType, @fname, @lname, @phoneNumber);";
            //finish this with course type;
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlInsert, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@userName", HttpUtility.UrlDecode(userName));
            sqlCommand.Parameters.AddWithValue("@password", HttpUtility.UrlDecode(password));
            sqlCommand.Parameters.AddWithValue("@email", HttpUtility.UrlDecode(email));
            sqlCommand.Parameters.AddWithValue("@userType", HttpUtility.UrlDecode(userType));
            sqlCommand.Parameters.AddWithValue("@fname", HttpUtility.UrlDecode(fname));
            sqlCommand.Parameters.AddWithValue("@lname", HttpUtility.UrlDecode(lname));
            sqlCommand.Parameters.AddWithValue("@phoneNumber", HttpUtility.UrlDecode(phoneNumber));

            sqlConnection.Open();

            try
            {
                int accountID = Convert.ToInt32(sqlCommand.ExecuteScalar());

            }
            catch (Exception e)
            {
            }
            sqlConnection.Close();

        }
    }
}


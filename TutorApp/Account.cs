using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TutorApp
{
    [Serializable]
    public class Account
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string userType { get; set; }

        public Account()
        {

        }

        public Account(int userID, string userName, string firstName, 
            string lastName, string email, string phoneNumber, string userType)
        {
            id = userID;
            this.userName = userName;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.phoneNumber = phoneNumber;
            this.userType = userType;
        }

        public Account(int userID, string firstName, string lastName, string phoneNumber, string email)
        {
            id = userID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
            this.email = email;
        }
        
    }
}
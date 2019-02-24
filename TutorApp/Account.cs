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

        public Account(int userID, string firstName, 
            string lastName, string email, string phoneNumber, string userType)
        {
            id = userID;
            userName = this.userName;
            firstName = this.firstName;
            lastName = this.lastName;
            email = this.email;
            phoneNumber = this.phoneNumber;
            userType = this.userType;
        }

        public Account(int userID, string firstName, string lastName, string phoneNumber, string email)
        {
            id = userID;
            firstName = this.firstName;
            lastName = this.lastName;
            phoneNumber = this.phoneNumber;
            email = this.email;
        }
        
    }
}
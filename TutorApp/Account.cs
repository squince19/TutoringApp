using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TutorApp
{
    public class Account
    {
        public int id;
        public string userName;
        public string firstName;
        public string lastName;
        public string email;
        public string phoneNumber;
        public string userType;

        public Account()
        {

        }

        public Account(int userID, string userName, string firstName, 
            string lastName, string email, string phoneNumber, string userType)
        {
            userID = id;
            userName = this.userName;
            firstName = this.firstName;
            lastName = this.lastName;
            email = this.email;
            phoneNumber = this.phoneNumber;
            userType = this.userType;
        }
        
    }
}
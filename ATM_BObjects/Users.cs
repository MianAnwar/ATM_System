using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_BObjects
{
    public class Users
    {
    /// <summary>
    ///     Constructors
    /// </summary>
        public Users()
        {
            this.UserId = "";
            this.PinCode = "";
            this.UserPosition = true;   // customer(1)   by-default
            this.Status = "";    // Acive(1) or Disabled(0)
        }

        public Users(string userid = "", string PinCode = "", bool UserPosition = false, string status="") {
            this.UserId = userid;
            this.PinCode = PinCode;
            this.UserPosition = UserPosition;   // customer   by-default
            this.Status = status;
        }
    

    /// <summary>
    ///     Data Members
    /// </summary>
        private string userid;
        private string pincode;
        private bool position;      // true(1)-Customer, false(0)-Admin
        private string status;    // true(1)-Active, false(0)-Disabled

        /// <summary>
        ///     Getters and Setters
        /// </summary>
        public string UserId { get; set; }
        public string PinCode { get; set; }
        public bool UserPosition { get; set; }
        public string Status { get; set; }
    }
}

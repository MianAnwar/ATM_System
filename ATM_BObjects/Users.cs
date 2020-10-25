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
            this.UserPosition = false;   // customer(0)   by-default
            this.Status = false;    // Acive(1) or Disabled(0)
        }

        public Users(string userid = "", string PinCode = "", bool UserPosition = false, bool status=true) {
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
        private bool status;    // true(1)-Active, false(0)-Disabled

        /// <summary>
        ///     Getters and Setters
        /// </summary>
        public string UserId { get; set; }
        public string PinCode { get; set; }
        public bool UserPosition { get; set; }
        public bool Status { get; set; }
    }
}

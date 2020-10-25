using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_BObjects
{
    public class Customer: Users
    {

    /// <summary>
    ///     Constructors
    /// </summary>
        public Customer()
        {
            this.accountNo = -1;
            this.AccountHolderName = "";
            this.AccountType = "";    // Saving   by-default
            this.AccountBalance = 0;
            this.AccountStatus="";  // active   by-default

        }

    
    /// <summary>
    ///     Data Members
    /// </summary>
        private int accountNo;  ////// System Generated //////

        private string accountHolderName;
        private string accountType;       // Savings,    Current
        private int accountbalance;    // account holder's balance
        private string accountStatus;     // Active,   Disabled,


    /// <summary>
    ///     Getters and Setters
    /// </summary>
        public int AccountNo { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountType { get; set; }
        public int AccountBalance { get; set; }
        public string AccountStatus { get; set; }
    }
}

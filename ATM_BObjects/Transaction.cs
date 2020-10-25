using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_BObjects
{
    public class Transaction
    {

    /// <summary>
    ///     Constructor
    /// </summary>
        public Transaction()
        {
            customer = new Customer();
            this.TransactionType = 1;   // Withdraw   by-default
            this.TransactionAmount = 0;
            this.TransactionDate = (DateTime.Now).ToString();
        }


    /// <summary>
    ///     Data Mmebers
    /// </summary>
        private Customer customer;
        private int transactionType;   // 1-Withdraw,   2-Deposit,    3-Transfer
        private long transactionAmount;
        private string transactiondate;        // The format for the date must always be Day/Month/Year [DD/MM/YYYY]


    /// <summary>
    ///     Getters and Setters
    /// </summary>
        public int TransactionType { get; set; }
        public long TransactionAmount { get; set; }
        public string TransactionDate { get; set; }


    }
}

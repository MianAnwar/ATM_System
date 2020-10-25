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
            this.AccountNo = 0;
            this.TransactionType = 1;   // Withdraw   by-default
            this.TransactionAmount = 0;
            this.TransactionDate = (DateTime.Now).ToString();
        }


        /// <summary>
        ///     Data Mmebers
        /// </summary>
        private int accountNo;
        private int transactionType;   // 0-Withdraw,   1-Deposit,    2-Transfer
        private long transactionAmount;
        private string transactiondate;        // The format for the date must always be Day/Month/Year [DD/MM/YYYY]


    /// <summary>
    ///     Getters and Setters
    /// </summary>
        public int TransactionType { get; set; }
        public long TransactionAmount { get; set; }
        public string TransactionDate { get; set; }
        public int AccountNo { get; set; }
    }
}

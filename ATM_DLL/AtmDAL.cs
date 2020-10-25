using System;
using System.IO;
using ATM_BObjects;
using System.Collections.Generic;
using System.Text;

namespace ATM_DLL
{
    public class AtmDAL: LoginProcess
    {
        /// <summary>
        ///     working for layers
        ///     Admin Functionality
        /// </summary>
   

        public int createNewAccount(Customer c)
        {
            // save the new customer's account info. and return the acoount no
            return 50;
        }

        public string getAccountHolderNameWithAccountNo(int accountNo)
        {
            return "";
        }

        public bool DeleteAccount(int accountNo)
        {
            return true;
        }

        public Customer getCustomer(int accountNo)
        {
            Customer c = new Customer();
            return c;
        }

        public bool UpdateCustomer(Customer updatedCustomer)
        {
            return true;
        }

        public int GetBalance(int accountNo)
        {
            return 5;
        }

        public int getAccountNo(string accountNo)
        {
            return 234;
        }

        public bool DepositAmount(int accountNo, int amountToDeposit)
        {
            return true;
        }


        public bool TransferAmountTo(int accountNo, int amountToTransfer, string currentUserId)
        {
            return true;
        }
    }
}

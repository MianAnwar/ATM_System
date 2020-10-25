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
           int Current_Accounts_Count = GetCountof(usersFileName);  //0=1000, 1=1001...
            int newAccountNo;
  
                newAccountNo = Current_Accounts_Count * 1000;

            c.AccountNo = newAccountNo;

     // accountNo = 11221, AccountHolderName = Ali, Type=Saving(0), Balance=5000
            int type = c.AccountType.Equals("saving") ? 0 : 1;
            string acc = $"{Current_Accounts_Count+1},{c.AccountNo},{c.AccountHolderName},{type},{c.AccountBalance}";

            // userId=admin, pinCode=11221, position=0(Admin), Status=1(Active)
            int pos = c.UserPosition ? 1 : 0;
            int stat = c.Status.Equals("active") ? 1 : 0;
            string credentials = $"{Current_Accounts_Count+1},{c.UserId},{c.PinCode},{pos},{stat}";

            string usersFile = Path.Combine(Environment.CurrentDirectory, usersFileName);
            string CustomerFile = Path.Combine(Environment.CurrentDirectory, custFileName);
            StreamWriter srCust = null;
            StreamWriter srUser = null;
            try
            {
                srCust = new StreamWriter(CustomerFile, append: true);
                srUser = new StreamWriter(usersFile, append: true);

                srCust.WriteLine(acc);
                srUser.WriteLine(credentials);

                Console.WriteLine("asdasdsaaaaaaaaaaa11111111");

                incrementCount(usersFileName);
                incrementCount(custFileName);

            }
            catch
            {
                return -1;
            }
            finally
            {
                srCust.Close();
                srUser.Close();
            }
            // save the new customer's account info. and return the acoount no
            return c.AccountNo;
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

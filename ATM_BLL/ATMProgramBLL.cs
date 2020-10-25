using System;
using ATM_BObjects;
using ATM_DLL;
using System.Collections.Generic;
using System.Text;

namespace ATM_BLL
{
    public class ATMProgramBLL
    {
        AtmDAL dal = new AtmDAL();

        int consectiveTurn = 0;
        string pre_id = "-";

        bool canLoginWith(string userId)    // consecutive 3 times checking
        {
            if (pre_id.Equals("-"))
            {
                pre_id = userId;
                consectiveTurn = 0;
            }
            else
            {
                if (userId.Equals(pre_id))
                    consectiveTurn++;
                else
                {
                    pre_id = userId;
                    consectiveTurn = 0;
                }
            }
            return consectiveTurn<3? true: false;
        }


        public int VerifyLoginWith(Users loginuser)  // -1=number of turn over(Disabled),  1-Customer, 2-Admin, 3-Incorrect Credentials, 0-NotExists
        {
            if (canLoginWith(loginuser.UserId))
            {
                int status = dal.checkStatus(loginuser.UserId);
                if (status == -1) // NotExists means Incorrect Credentials
                {
                    return 0;
                }
                else if (status == 0) // Disabled Account already
                {
                    return -1;
                }
                else if (status == 1) // Active 
                {
                    // 1- for customer  2- for admin
                    if (dal.verifyPinCode(loginuser.UserId, loginuser.PinCode))
                        return dal.checkPosition(loginuser.UserId);
                    else
                        return 3;
                }
            }
            else
            {
                // set the status of Account as 'disabled'
                dal.setDisabled(loginuser.UserId);
                return -1;
            }

            
            return 6;

            /// first check whether this userId have status as Active or not
            ///      if active then proceed and give the 3 chances
            ///         otherwise return -1 ==> he have no chance to login 

            /// int status = dal.checkstatus(userId);   -1 for disabled and 1 for active and 0 for NotExistsUserID
            /// if(status ==-1) return -1
            /// else if(status ==0) then return 0
            ///  else if(status ==1) then continue 
            ///{
                
            ////}
        }


        /// <summary>
        ///     Customer Functionality
        /// </summary>
        public void withdrawAmount(int? amount, string userId)
        {
            /////// dal.WithDrawAmount(amount, userId)  from the account
            Console.WriteLine();
        }


        public bool feasibleToWithdraw(int? amount, string userId)
        {
            // READ from file of customer's account; the account balancee and compare the amount if difference 
            // b/w balance amount is greater than zero then it will be feasible to withdraw
            ////// otherwise false.

            // return dal.checkWithdrawFeasibilityFor(amount,userId);
            return true;
        }

        public void settingUp()
        {
            dal.SetUpSystem();
        }


        public int GetBalance(int accountNo)
        {
            return dal.GetBalance(accountNo);
        }
        public int getAccountNo(string accountNo)
        {
            return dal.getAccountNo(accountNo);
        }
        
        /// <summary>
        /// //////////////////////////////////////
        /// </summary>
        public int createNewAccount(Customer c)
        {
            return dal.createNewAccount(c);
        }

        public string getAccountHolderNameWithAccountNo(int accountNo)
        {
            return dal.getAccountHolderNameWithAccountNo(accountNo);
        }

        public bool deleteAccount(int accountNo)
        {
            return dal.DeleteAccount(accountNo);
        }

        public Customer getCustomer(int accountNo)
        {
            return dal.getCustomer(accountNo);
        }

        public bool UpdateCustomer(Customer updatedCustomer)
        {
            return dal.UpdateCustomer(updatedCustomer);
        }

        public bool depositAmount(int accountNo, int amountToDeposit)
        {
            return dal.DepositAmount(accountNo, amountToDeposit);
        }
    }
}

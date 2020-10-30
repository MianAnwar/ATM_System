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
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
        public void settingUp()
        {
            dal.SetUpSystem();
        }
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        ///     Customer Functionality
        /// </summary>
        public void withdrawAmount(int amount, string userId)
        {
            dal.withdrawAmount(amount, userId);
        }

        public bool feasibleToWithdraw(int amount, string userId)
        {
            return dal.feasibleToWithdraw(amount, userId);
        }


        public int GetBalance(int accountNo)
        {
            return dal.GetBalance(accountNo);
        }
        public int getAccountNo(string userId)
        {
            return dal.getAccountNo(userId);
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
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

        public List<string> getSearchResult(Customer c)
        {
            return dal.getSearchResult(c);
        }


        public List<string> getSearchResultBTbalance(int minBalance, int maxBalance)
        {
            return dal.getSearchResultBTbalance(minBalance, maxBalance);
        }

        public List<string> getSearchResultBTdates(string startingDate, string endingDate)
        {
            return dal.getSearchResultBTdates(startingDate, endingDate);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////
        public bool depositAmount(int accountNo, int amountToDeposit)
        {
            return dal.DepositAmount(accountNo, amountToDeposit);
        }

        public bool TransferAmountTo(int accountNo, int amountToTransfer, string currentUserId)
        {
            return dal.TransferAmountTo(accountNo, amountToTransfer, currentUserId);
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
    }
}

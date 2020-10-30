using System;
using System.Collections.Generic;
using System.Text;
using ATM_BLL;
using static System.Console;

namespace ATM_VIEW
{
    public class SystemUsersFunctionalityView: AdminFunctionalityView
    {
        /// <summary>
        ///     Customer functionality for ATM_VIEW Layer
        /// </summary>
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
        void setAmount(ref int? withdraw_amount, int? withdrawChoice)
        {
            if (withdrawChoice == 1)  // 500
            {
                withdraw_amount = 500;
            }
            else if (withdrawChoice == 2) // 1,000
            {
                withdraw_amount = 1000;
            }
            else if (withdrawChoice == 3) // 2,000
            {
                withdraw_amount = 2000;
            }
            else if (withdrawChoice == 4) // 5,000
            {
                withdraw_amount = 5000;
            }
            else if (withdrawChoice == 5) // 10,000
            {
                withdraw_amount = 10000;
            }
            else if (withdrawChoice == 6) // 15,000
            {
                withdraw_amount = 15000;
            }
            else if (withdrawChoice == 7) // 20,000
            {
                withdraw_amount = 20000;
            }

        }

        ///// withdraw through Fast Mode
        public void ContinueToWithdrawThroughFastMode(string currentUserId)
        {
            menus.FastCashMenu();
            int? withdrawChoice = InputHandlerFromConsole.GetNumberInRange(1, 7, "Select one of the denominations of money: ");
            int? withdraw_amount = 500;
            setAmount(ref withdraw_amount, withdrawChoice);

            DoWithdrawing(Convert.ToInt32(withdraw_amount), currentUserId); // common method to deduct amount from account balance
        }

        ///// withdraw through Normal Mode
        public void ContinueToWithdrawThroughNormalMode(string currentUserId)
        {
            int withdraw_amount = Convert.ToInt32(InputHandlerFromConsole.GetNumberInRange(int.MinValue, int.MaxValue, "Enter the withdrawal amount: "));

            DoWithdrawing(withdraw_amount, currentUserId); // common method to deduct amount from account balance
        }



        void DoWithdrawing(int withdraw_amount, string currentUserId)
        {
            char? choiceYN = InputHandlerFromConsole.GetYN($"Are you sure you want to withdraw Rs.{withdraw_amount} (Y/N)? ");
            if (choiceYN == 'y' || choiceYN == 'Y')
            {
                if (bll.feasibleToWithdraw(withdraw_amount, currentUserId))  //// true is have enough balance
                {
                    // if acoount have enough balance
                    bll.withdrawAmount(withdraw_amount, currentUserId);
                    Console.WriteLine($"Cash Successfully Withdrawm, {withdraw_amount}!");
                }
                else    // false: if don't have enough balance
                {
                    WriteLine("No Enough Balance is in the Account.");
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////

        public void ContinueToDespositAmount(string currentUserId)
        {
            int amountTodeposit = Convert.ToInt32(InputHandlerFromConsole.GetNumberInRange(1, int.MaxValue, "Enter the cash amount to deposit: "));
            int accountNo = bll.getAccountNo(currentUserId);
            if (bll.depositAmount(accountNo, amountTodeposit))
                WriteLine("Cash Deposited Successfully!");
            else
            {
                WriteLine("Cash Deposited unsuccessfully");
            }
        }

/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////

        /////// transfer cash
        public void ContinueToTransferCash(string currentUserId)
        {
            enterAgmin:
            int amountToTransfer = Convert.ToInt32(InputHandlerFromConsole.GetNumberInRange(1, int.MaxValue, "Enter amount in multiple of 500: "));
            if (amountToTransfer % 500 != 0)
                goto enterAgmin;

            int accountNo1 = Convert.ToInt32(InputHandlerFromConsole.GetNumberInRange(1, int.MaxValue, "Enter the account number to which you want to transfer: "));

            string name = bll.getAccountHolderNameWithAccountNo(accountNo1);
            if(name=="")
            {
                WriteLine("No such Account Number exits.");
                return;
            }

            int accountNo2 = Convert.ToInt32(InputHandlerFromConsole.GetNumberInRange(1, int.MaxValue, $"You wish to deposit Rs. {amountToTransfer} in account held by Mr. {name}; If this information is correct please re-enter the account number: "));

            if (accountNo1 == accountNo2)
            {
                string msg = bll.TransferAmountTo(accountNo1, amountToTransfer, currentUserId) ? "Transaction Confirmed Successfully" : "Some Error has occurred while performing the operation.";
                WriteLine(msg);
            }
            else
            {
                WriteLine("Alas! You re-entered account number doesn't match. Press Any Key to go to main menu.");
            }
        }

/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////

        public void ContinueToViewBalance(string currentUserId)
        {
            int accountNo = bll.getAccountNo(currentUserId);
            int balance = bll.GetBalance(accountNo);
            string time = (DateTime.Now).ToString();
            OutputHandlerToConsole.DisplayBalancePage(accountNo, balance, time);
        }

    }
}

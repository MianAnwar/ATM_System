using System;
using System.Collections.Generic;
using System.Text;
using ATM_BObjects;
using ATM_BLL;
using static System.Console;

namespace ATM_VIEW
{
    public class AdminFunctionalityView
    {
        protected ATMProgramBLL bll = new ATMProgramBLL();

        /// <summary>
        ///     Admin functionality for ATM_VIEW Layer
        /// </summary>
        /// 
        public void continueToCreateNewAccount()
        {
            Customer cust = InputHandlerFromConsole.GetCreateNewAccountInput();

            char? choiceYN = InputHandlerFromConsole.GetYN($"Are you sure you want to Create New Account (Y/N)? ");
            if (choiceYN == 'y' || choiceYN == 'Y')
            {
                int accountNo = bll.createNewAccount(cust);
                WriteLine($"Account Successfully Created! - the account number assigned is: {accountNo}");
            }
        }

        public void ContinueToDeleteAccount()
        {
            int accountNo1 = Convert.ToInt32(InputHandlerFromConsole.GetNumberInRange(1, int.MaxValue, "Enter the account number to which you want to delete: "));
            string name = bll.getAccountHolderNameWithAccountNo(accountNo1);
            int accountNo2 = Convert.ToInt32(InputHandlerFromConsole.GetNumberInRange(1, int.MaxValue, $"You wish to delete the account held by Mr. {name}; If this information is correct please re-enter the account number: "));
            if (accountNo1 == accountNo2)
            {
                string msg = bll.deleteAccount(accountNo1) ? "Account Deleted Successfully" : "Some Error has occurred while performing the operation.";
                WriteLine(msg);
            }
        }

        public void ContinueToUpdateAccount()
        {
            int accountNo = Convert.ToInt32(InputHandlerFromConsole.GetNumberInRange(1, int.MaxValue, "Enter the Account Number: "));
            Customer c = bll.getCustomer(accountNo);
            if (c == null)
                WriteLine("Invalid Account Number!");
            else
            {
                OutputHandlerToConsole.DisplayCustoemr(c);
            }

            Write("Please enter in the fields you wish to update(leave blank otherwise): ");

            Customer updatedCustomer = InputHandlerFromConsole.GetFieldsForUpdatingAccount(accountNo);

            updatedCustomer.AccountNo = accountNo;
            if (updatedCustomer.UserId == "")
                updatedCustomer.UserId = c.UserId;

            if (updatedCustomer.PinCode == "")
                updatedCustomer.PinCode = c.PinCode;

            if (updatedCustomer.AccountHolderName == "")
                updatedCustomer.AccountHolderName = c.AccountHolderName;

            if (updatedCustomer.AccountType == "")
                updatedCustomer.AccountType = c.AccountType;

            updatedCustomer.AccountBalance = c.AccountBalance;

            if (updatedCustomer.AccountStatus == "")
                updatedCustomer.AccountStatus = c.AccountStatus;

            OutputHandlerToConsole.DisplayCustoemr(updatedCustomer);

            if (bll.UpdateCustomer(updatedCustomer))
                WriteLine("Your account have been successfully been updated!");
            else
            {
                ForegroundColor = ConsoleColor.DarkMagenta;
                WriteLine("There is some problem while updating the account information!");
                ForegroundColor = ConsoleColor.White;
            }
        }

    }
}

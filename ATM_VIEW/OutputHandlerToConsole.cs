using System;
using System.Collections.Generic;
using ATM_BObjects;
using System.Text;
using static System.Console;

namespace ATM_VIEW
{
    class OutputHandlerToConsole
    {
        public static void DisplayCustoemr(Customer c)
        {
            WriteLine($"Account No.: {c.AccountNo}");
            WriteLine($"Login: {c.UserId}");
            WriteLine($"Pin Code: {c.PinCode}");
            WriteLine($"Account Holder Name: {c.AccountHolderName}");
            WriteLine($"Account Type: {c.AccountType}");
            WriteLine($"Balance: {c.AccountBalance}");
            WriteLine($"Status: {c.Status}");

        }

        public static void DisplayBalancePage(int accountNo, int balance, string time)
        {
            WriteLine($"Account No.: {accountNo}");
            WriteLine($"Time: {time}");
            WriteLine($"Balance: {balance}");
        }
    }


}

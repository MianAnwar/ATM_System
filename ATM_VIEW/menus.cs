using System;
using System.Collections.Generic;
using System.Text;
using ATM_BObjects;
using static System.Console;

namespace ATM_VIEW
{
    public class menus
    {
        //1///////////////////////////////////////////////////////////////////////////////////////////
        /*
         LoginScreen Menus...
         */
        public static void LogInScreen()
        {
            Console.Clear();
            atmPrinter();
            WriteLine();
            WriteLine("\t::|         _______     ______    ___         ::");
            WriteLine("\t::|        |       |   |     /     |    |\\   |::");
            WriteLine("\t::|        |       |   |           |    | \\  |::");
            WriteLine("\t::|        |       |   |   __      |    |  \\ |::");
            WriteLine("\t::|______  |_______|   \\_____|    _|_   |   \\|::");
        }

        public static void atmPrinter()
        {
            WriteLine("----Welcome to Automated Teller Machine(ATM) Program----");
            WriteLine("\t\t:: ______    __________   __     __ ::");
            WriteLine("\t\t::|      |       |        | \\   /  |::");
            WriteLine("\t\t::|______|       |        |  \\_/   |::");
            WriteLine("\t\t::|      |       |        |        |::");
            WriteLine("\t\t::|      |       |        |        |::");
        }

        public static void WelcomePrinter()
        {
            WriteLine("\t\t\t----Welcome to Automated Teller Machine(ATM) Program----");
            WriteLine("\t\t              _______            _____     ______     __     __ ");
            WriteLine("\t\t::|       |  |         |        /         /      \\    |\\   /  |::");
            WriteLine("\t\t::|  /\\   |  |____     |        |         |      |    | \\_/   |::");
            WriteLine("\t\t::| /  \\  |  |         |        |         |      |    |       |::");
            WriteLine("\t\t::|/    \\ |  |_______  |_______ \\______   \\_____/     |       |::");
        }

        //2///////////////////////////////////////////////////////////////////////////////////////////
        /*
         A customer is then taken to the customer option menu where he will select one
        of the following options;
        1----Withdraw Cash
        2----Cash Transfer
        3----Deposit Cash
        4----Display Balance
        5----Exit
        Please select one of the above options:
         */
        public static void CustomerMenu()
        {
            WriteLine("1----Withdraw Cash");
            WriteLine("2----Cash Transfer");
            WriteLine("3----Deposit Cash");
            WriteLine("4----Display Balance");
            WriteLine("5----Logout");
        }

        /*
         The user must be displayed a menu to select the mode of withdrawal as
            follows:
            a) Fast Cash
            b) Normal Cash
            Please select a mode of withdrawal:
         */
        public static void WithdrawCashMenu()
        {
            WriteLine("1----> Fast Cash");
            WriteLine("2----> Normal Cash ");
         //   WriteLine("3----> BACK");
        }

        /*
         *In case of fast cash the user must be presented with a menu such as the one below and asked to choose one of the predefined denominations of money. If
            he chooses withdraw, the user is asked to select the amount from the options given. No matter which option the user uses to withdraw the money the system
            must check that the amount is valid (i.e. there is enough money in the account).
            1----500
            2----1000
            3----2000
            4----5000
            5----10000
            6----15000
            7----20000
         */
        public static void FastCashMenu()
        {
            WriteLine("1----> 500");
            WriteLine("2----> 1,000");
            WriteLine("3----> 2,000");
            WriteLine("4----> 5,000");
            WriteLine("5----> 10,000");
            WriteLine("6----> 15,000");
            WriteLine("7----> 20,000");
        }

        public static void ViewReportMenu()
        {
            WriteLine("1----> Accounts by Balance Amount");
            WriteLine("2----> Accounts by Date");
        }


        //3/////////////////////////////////////////////////////////////////////////////////////////
        /*
         If the user who logs in is an administrator he should be presented with the
            following menu,
            1----Create New Account.
            2----Delete Existing Account.
            3----Update Account Information.
            4----Search for Account.
            5----View Reports
            6----Exit
         */
        public static void AdminMenu()
        {
            WriteLine("1----Create New Account");
            WriteLine("2----Delete Existing Account");
            WriteLine("3----Update Account Information");
            WriteLine("4----Search for Account");
            WriteLine("5----View Reports");
            WriteLine("6----Logout");
        }

        public static void MenuHeader()
        {
            Console.Clear();
            WelcomePrinter();
            WriteLine();
        }

        
    }
}

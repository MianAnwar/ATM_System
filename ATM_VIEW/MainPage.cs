using System;
using ATM_BLL;
using ATM_BObjects;
using static System.Console;

namespace ATM_VIEW
{
    public class MainPage: SystemUsersFunctionalityView
    {
        string currentUserId;
        /*
         First Activity
         */
        public void loginToProceed()
        {
            Console.WriteLine("Setting up the Storage System...!");
            bll.settingUp();
        again:
            
            menus.LogInScreen();   // display LOGIN
            (string userId, string pinCode) = InputHandlerFromConsole.GetLogInInput();  // Take login credantials from user
            

        // business Object to transfer information among layers
            Users loginUser = new Users();
            currentUserId = loginUser.UserId = userId;
            loginUser.PinCode = pinCode;
            

         // Business Logic Layer => BLL layer functions come in...
            int c = bll.VerifyLoginWith(loginUser);     //


        /////////////////////// Accordingly Change the View /////////////////////// 
            if (c == -1)         ////////////////////< -1=Three number of turn over >////////////////////
            {
                ForegroundColor = ConsoleColor.DarkRed;
                InputHandlerFromConsole.DisplayMsgAndWaitForUserResponse("Account is Disabled!" +
                "\nIncorrect Credentials Entered for consecutive 3 times for the same account...." +
                "\nContact Admin\n Press Enter to goto Login Page");
                ForegroundColor = ConsoleColor.White;
                goto again;
            }
            else if (c == 0)       ////////////////////< 0-NotUserExists for given userId >////////////////////
            {
                ForegroundColor = ConsoleColor.Red;
                InputHandlerFromConsole.DisplayMsgAndWaitForUserResponse("Not a correct Account Login!" +
                    "\nIncorrect Credentials Entered" +
                    "\nContact Admin\n Press Enter to goto Login Page");
                ForegroundColor = ConsoleColor.White;
                goto again;
            }
            else if (c == 3)       ////////////////////< 3-Incorrect Credentials >////////////////////
            {
                ForegroundColor = ConsoleColor.Yellow;
                InputHandlerFromConsole.DisplayMsgAndWaitForUserResponse("Incorrect Account Credentials!" +
                    "\nContact Admin in case you forgot.\n Press Enter to goto Login Page");
                ForegroundColor = ConsoleColor.White;
                goto again;
            }
            else if (c == 1)    ////////////////////< 1-Customer >////////////////////
            {
                CustomerAgain:
                menus.MenuHeader(); // display header
                menus.CustomerMenu();   // display Customer Menu

                int? choice = InputHandlerFromConsole.GetNumberInRange(1, 5, "Please select one of the above options: ");

                if (choice == 1)   // withdraw cash
                {
                    menus.WithdrawCashMenu();
                    int? withdrawChoice = InputHandlerFromConsole.GetNumberInRange(1, 2, "Please select a mode of widthdraw: ");
                    if (withdrawChoice == 1)    // fast Mode
                    {
                        ContinueToWithdrawThroughFastMode(currentUserId);
                        ReadKey();
                        goto CustomerAgain;
                    }
                    else if (withdrawChoice == 2)   // normal mode
                    {
                        ContinueToWithdrawThroughNormalMode(currentUserId);
                        ReadKey();
                        goto CustomerAgain;
                    }
                }
                else if (choice == 2) // cash transfer
                {
                    ContinueToTransferCash(currentUserId);
                    ReadKey();
                    goto CustomerAgain;
                }
                else if (choice == 3) // deposit cash
                {
                    ContinueToDespositAmount(currentUserId);
                    ReadKey();
                    goto CustomerAgain;
                }
                else if (choice == 4)    // display balance
                {
                    ContinueToViewBalance(currentUserId);
                    ReadKey();
                    goto CustomerAgain;
                }
                else if (choice == 5) // logout- EXIt
                {
                    goto again;
                }
                
            }
            else if (c == 2)       ////////////////////< 2-Admin >////////////////////
            {
                AdminAgain:
                menus.MenuHeader(); // display header
                menus.AdminMenu();   // display Admin Menu

                int? choice = InputHandlerFromConsole.GetNumberInRange(1,6, "Please select one of the above options: ");

                if (choice == 1)   // create new Account
                {
                    continueToCreateNewAccount();
                    ReadKey();
                    goto AdminAgain;
                }
                else if (choice == 2) // Delete Existing Account
                {
                    ContinueToDeleteAccount();
                    ReadKey();
                    goto AdminAgain;
                }
                else if (choice == 3) // Update Account Information
                {
                   ContinueToUpdateAccount();
                    ReadKey();
                    goto AdminAgain;
                }
                else if (choice == 4)    // Search for Account
                {
                    //  ContinueToSearchAccount();
                    ReadKey();
                    goto AdminAgain;
                }
                else if (choice == 5)    // View Reports
                {
                    // ContinueToViewReports();
                    ReadKey();
                    goto AdminAgain;
                }
                else if (choice == 6) // logout- EXit
                {
                    goto again;
                }
            }
        }




        
    }
}

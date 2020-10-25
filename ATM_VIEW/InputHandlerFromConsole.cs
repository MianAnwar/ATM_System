﻿using System;
using static System.Console;
using ATM_BObjects;
using System.Collections.Generic;
using System.Text;

namespace ATM_VIEW
{
    class InputHandlerFromConsole
    {
        public static string getStringInput(string msg)
        {
            string inp;
            do
            {
                Write(msg);
                ForegroundColor = ConsoleColor.Cyan;
                inp = ReadLine();
                ForegroundColor = ConsoleColor.White;
            } while (inp.Length == 0);
            return inp;
        }

        public static (string id, string pin) GetLogInInput()
        {
            string userId;
            string pinCode;

            userId = getStringInput("Enter Login: ");

            do
            {
                Write("Enter Pin Code: ");
                ForegroundColor = ConsoleColor.Cyan;
                pinCode = ReadLine();
                ForegroundColor = ConsoleColor.White;
            } while (pinCode.Length != 5);

            return (userId, pinCode);
        }
                    
        public static dynamic GetNumberInput()
        {
            int num;
            ForegroundColor = ConsoleColor.Cyan;
            string inp = Console.ReadLine();
            ForegroundColor = ConsoleColor.White;
            try
            {
                num = int.Parse(inp);
                return num;
            }
            catch
            {
                ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Something is wrong with input!");
                ForegroundColor = ConsoleColor.White;
                return null;
            }
        }


        public static int? GetNumberInRange(int x, int y, string msg)
        {
            int? choice = null;
            do
            {
                Write(msg);
                choice = GetNumberInput();
            } while (choice == null || choice < x || choice > y);
            return choice;
        }

        public static char? GetYN(string msg)
        {
            char? choiceYN = 'y';
            do
            {
                Write(msg);
                ForegroundColor = ConsoleColor.Cyan;
                ConsoleKeyInfo key = Console.ReadKey();
                ForegroundColor = ConsoleColor.White;
                choiceYN = key.KeyChar;
                WriteLine();
            //    WriteLine($"->{choiceYN}<-");
            } while ((choiceYN != null) && (choiceYN != 'Y') && (choiceYN != 'y') && (choiceYN != 'N') && (choiceYN != 'n'));
            return choiceYN;
        }


        public static void DisplayMsgAndWaitForUserResponse(string msg)
        {
            ForegroundColor = ConsoleColor.DarkRed;
            Write(msg);
            ForegroundColor = ConsoleColor.White;
            ReadKey();
        }

        public static Customer GetCreateNewAccountInput()
        {
            string userId = getStringInput("Login: ");
            string pinCode = "";
            do
            {
                Write("Pin Code: ");
                ForegroundColor = ConsoleColor.Cyan;
                pinCode = ReadLine();
                ForegroundColor = ConsoleColor.White;
            } while (pinCode.Length != 5);

            string holderName = getStringInput("Holder Name: ");

            string type = "";
            do
            {
                Write("Type (Saving, Current): ");
                ForegroundColor = ConsoleColor.Cyan;
                type = ReadLine();
                type = type.ToLower();
                ForegroundColor = ConsoleColor.White;
            } while (type!= "saving" && type!="current");

            int balance = Convert.ToInt32(GetNumberInRange(int.MinValue, int.MaxValue, "Starting Balance: "));

            string status = "";
            do
            {
                Write("Status (Active, Disabled): ");
                ForegroundColor = ConsoleColor.Cyan;
                status = ReadLine();
                status = status.ToLower();
                ForegroundColor = ConsoleColor.White;
            } while (status != "active" && status != "disabled");


            Customer cust = new Customer()
            {
                UserId = userId,
                PinCode = pinCode,
                AccountHolderName = holderName,
                AccountType = type,
                AccountBalance = balance,
                AccountStatus = status,
            };

            return cust;
        }

        public static Customer GetFieldsForUpdatingAccount(int accountNo)
        {
            Customer updatedCust = new Customer();

            string userId = "";
            Write("\nLogin: ");
            ForegroundColor = ConsoleColor.Cyan;
            userId = ReadLine();
            ForegroundColor = ConsoleColor.White;
            
            string pinCode = "";
            do
            {
                Write("Pin Code: ");
                ForegroundColor = ConsoleColor.Cyan;
                pinCode = ReadLine();
                ForegroundColor = ConsoleColor.White;
                if (pinCode == "")
                    break;
            } while (pinCode.Length != 5);

            string holderName = "";
            Write("Account Holder Name: ");
            ForegroundColor = ConsoleColor.Cyan;
            holderName = ReadLine();
            ForegroundColor = ConsoleColor.White;


            string type = "";
            do
            {
                Write("Type (Saving, Current): ");
                ForegroundColor = ConsoleColor.Cyan;
                type = ReadLine();
                ForegroundColor = ConsoleColor.White;
                if (type == "")
                    break;
                type = type.ToLower();
            } while (type != "saving" && type != "current");

            
            string status = "";
            do
            {
                Write("Status (Active, Disabled): ");
                ForegroundColor = ConsoleColor.Cyan;
                status = ReadLine();
                ForegroundColor = ConsoleColor.White;
                if (status == "")
                    break;
                status = status.ToLower();
            } while (status != "active" && status != "disabled");


            return new Customer()
            {
                AccountNo = accountNo,
                UserId = userId,
                PinCode = pinCode,
                AccountHolderName = holderName,
                AccountType = type,
                AccountStatus = status,
            };
        }
    }
}
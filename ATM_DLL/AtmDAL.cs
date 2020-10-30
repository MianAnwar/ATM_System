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

//////////////////////////////////////////////////////////////////////////////////////////////////
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
            incrementCount(usersFileName);
            incrementCount(custFileName);
            // save the new customer's account info. and return the acoount no
            return c.AccountNo;
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
        public string getAccountHolderNameWithAccountNo(int accountNo)
        {
            (int count, List<string> customers) = GetStringListofUsers(custFileName);

            if (count > 0 && customers != null)
            {
                foreach (string l in customers)
                {
                    string[] data = l.Split(',');
                    if (accountNo.Equals(Convert.ToInt32(data[1])))
                    {
                        return data[2];
                    }
                }
            }
            return "";
        }

        public bool DeleteAccount(int accountNo)
        {
            (int count, List<string> customers) = GetStringListofUsers(custFileName);
            List<string> updatedCustomerList = new List<string>();
            //writeBacktoFile(count, users, FileName);
            if (count > 0 && customers != null)
            {
                foreach (string l in customers)
                {
                   string[] data = l.Split(',');
                   if (!(accountNo.Equals(Convert.ToInt32(data[1]))))
                    {
                        updatedCustomerList.Add(l);
                    }
                }
                writeBacktoFile(count-1, updatedCustomerList, custFileName);
                return true;
            }
            return false;
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
        public Customer getCustomer(int accountNo)
        {
            (int count, List<string> customers) = GetStringListofUsers(custFileName);
            (int users_count, List<string> users) = GetStringListofUsers(usersFileName);

            if (count > 0 && customers != null && users_count > 0 && users != null)
            {
                foreach (string l in customers)
                {
                    string[] data = l.Split(',');
                    Customer cust = new Customer();
                    string srNo = data[0];
                    cust.AccountNo = Convert.ToInt32(data[1]);
                    cust.AccountHolderName = data[2];
                    cust.AccountType = data[3].Equals("0") ? "saving" : "current";
                    cust.AccountBalance = Convert.ToInt32(data[4]);

                    
                    foreach (string u in users)
                    {
                        string[] user_data = u.Split(',');
                        Users us = new Users();
                        us.UserId = user_data[1];
                        us.PinCode = user_data[2];
                        us.UserPosition = user_data[3].Equals("0")? false: true;
                        us.Status = user_data[4].Equals("1")? "active" : "disabled";

                        if (srNo.Equals(user_data[0]))
                            {
                            cust.UserId = us.UserId;
                            cust.PinCode = us.PinCode;
                            cust.Status = us.Status;
                        }
                    }
                            
                    if (accountNo.Equals(cust.AccountNo))
                    {
                        return cust;
                    }
                }
            }
            return null;
        }

        public bool UpdateCustomer(Customer newupdatedCustomer)
        {
            // rewrite updated
            (int count, List<string> customers) = GetStringListofUsers(custFileName);
            (int users_count, List<string> users) = GetStringListofUsers(usersFileName);

            List<string> updated_customer = new List<string>();
            List<string> updated_user = new List<string>();

            if (count > 0 && customers != null && users_count > 0 && users != null)
            {
                foreach (string l in customers)
                {
                    string[] data = l.Split(',');
                  //  Customer cust = new Customer();
                  //  string srNo = data[0];
                  //  AccountNo = Convert.ToInt32(data[1]);
                    
                    foreach (string u in users)
                    {
                        string[] user_data = u.Split(',');
                        updated_user.Add(u);        // add first of all, the admin 

                        if (data[0].Equals(user_data[0]))
                        {
                            if ((newupdatedCustomer.AccountNo).Equals(data[1]))
                            {
                                // accountNo = 11221, AccountHolderName = Ali, Type=Saving(0), Balance=5000
                                int type = newupdatedCustomer.AccountType.Equals("saving") ? 0 : 1;
                                string acc = $"{data[0]},{newupdatedCustomer.AccountNo},{newupdatedCustomer.AccountHolderName},{type},{newupdatedCustomer.AccountBalance}";
                                updated_customer.Add(acc);


                                // userId=admin, pinCode=11221, position=0(Admin), Status=1(Active)
                                int pos = newupdatedCustomer.UserPosition ? 1 : 0;
                                int stat = newupdatedCustomer.Status.Equals("active") ? 1 : 0;
                                string credentials = $"{user_data[0]},{newupdatedCustomer.UserId},{newupdatedCustomer.PinCode},{pos},{stat}";
                                updated_user.Add(credentials);
                            }
                            else
                            {
                                updated_customer.Add(l);
                                updated_user.Add(u);
                            }
                        }
                    }
                }
                writeBacktoFile(count, updated_customer, custFileName);
                writeBacktoFile(count, updated_user, usersFileName);
            }
            return true;
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
        public int GetBalance(int accountNo)
        {
            (int count, List<string> customers) = GetStringListofUsers(custFileName);

            if (count > 0 && customers != null)
            {
                foreach (string l in customers)
                {
                    string[] data = l.Split(',');
                    if (accountNo.Equals(Convert.ToInt32(data[1])))
                    {
                        return Convert.ToInt32(data[4]);
                    }
                }
            }
            return -1;
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

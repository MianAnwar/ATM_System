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
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
        public int createNewAccount(Customer c)
        {
           int Current_Accounts_Count = GetCountof(usersFileName);  //0=1000, 1=1001...
           int newAccountNo;
                newAccountNo = Current_Accounts_Count +1;
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
            (int users_count, List<string> users) = GetStringListofUsers(usersFileName);
            List<string> updatedCustomerList = new List<string>();
            List<string> updatedUsersList = new List<string>();
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
                foreach (string u in users)
                {
                    string[] udata = u.Split(',');
                    if (!(accountNo.Equals(Convert.ToInt32(udata[0]))))
                    {
                        updatedUsersList.Add(u);
                    }
                }
                writeBacktoFile(count - 1, updatedCustomerList, custFileName);
                writeBacktoFile(users_count - 1, updatedUsersList, usersFileName);
                return true;
            }
            return false;
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
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

        (string, string) convertCustomer(Customer c)
        {
            string acc = "", cred = "";

            // accountNo = 11221, AccountHolderName = Ali, Type=Saving(0), Balance=5000
            int type = c.AccountType.Equals("saving") ? 0 : 1;
            acc = $"{c.AccountNo},{c.AccountNo},{c.AccountHolderName},{type},{c.AccountBalance}";

            // userId=admin, pinCode=11221, position=0(Admin), Status=1(Active)
            int pos = c.UserPosition ? 1 : 0;
            int stat = c.Status.Equals("active") ? 1 : 0;
            cred = $"{c.AccountNo},{c.UserId},{c.PinCode},{pos},{stat}";

            return (cred, acc);
        }

        public bool UpdateCustomer(Customer newupdatedCustomer)
        {
            // rewrite updated
            (int count, List<string> customers) = GetStringListofUsers(custFileName);
            (int users_count, List<string> users) = GetStringListofUsers(usersFileName);

            (string cred, string acc) = convertCustomer(newupdatedCustomer);

            List<string> updatedCustomerList = new List<string>();
            List<string> updatedUsersList = new List<string>();
            if (count > 0 && customers != null)
            {
                foreach (string l in customers)
                {
                    string[] data = l.Split(',');
                    if (!(newupdatedCustomer.AccountNo.Equals(Convert.ToInt32(data[1]))))
                    {
                        updatedCustomerList.Add(l);
                    }
                    else
                    {
                        updatedCustomerList.Add(acc);
                    }

                }
                foreach (string u in users)
                {
                    string[] udata = u.Split(',');
                    if (!(newupdatedCustomer.AccountNo.Equals(Convert.ToInt32(udata[0]))))
                    {
                        updatedUsersList.Add(u);
                    }
                    else
                    {
                        updatedUsersList.Add(cred);
                    }

                }
                writeBacktoFile(count, updatedCustomerList, custFileName);
                writeBacktoFile(users_count, updatedUsersList, usersFileName);
                return true;
            }
            return false;
        }

        public List<string> getSearchResult(Customer c)
        {
            List<string> result = new List<string>();

            return result;
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


        public int getAccountNo(string userid)
        {
            (int count, List<string> users) = GetStringListofUsers(usersFileName);

            if (count > 0 && users != null)
            {
                foreach (string u in users)
                {
                    string[] data = u.Split(',');
                    if (userid.Equals(data[1]))
                    {
                        return Convert.ToInt32(data[0]);
                    }
                }
            }
            return -1;
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
        public bool DepositAmount(int accountNo, int amountToDeposit)
        {
            Customer c = getCustomer(accountNo);
            c.AccountBalance += amountToDeposit;
            return UpdateCustomer(c);
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
        public bool TransferAmountTo(int accountNo, int amountToTransfer, string currentUserId)
        {
            withdrawAmount(amountToTransfer, currentUserId);
            return DepositAmount(accountNo, amountToTransfer);
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
        public bool feasibleToWithdraw(int amount, string userId)
        {
            int accountNo = getAccountNo(userId);
            Customer c = getCustomer(accountNo);
            if (c.AccountBalance > 0)
            {
                c.AccountBalance -= amount;
                if (c.AccountBalance < 0)
                {
                    return false;
                }
                return true;
            }
            return false;            
        }

        public void withdrawAmount(int amount, string userId)
        {
            int accountNo = getAccountNo(userId);
            Customer c = getCustomer(accountNo);
            c.AccountBalance -= amount;
            UpdateCustomer(c);
        }
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
    }
}

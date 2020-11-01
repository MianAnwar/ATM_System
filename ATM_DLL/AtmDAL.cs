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


        public string getUserIdWithAccountNo(int accountNo)
        {
            (int count, List<string> users) = GetStringListofUsers(usersFileName);

            if (count > 0 && users != null)
            {
                foreach (string l in users)
                {
                    string[] data = l.Split(',');
                    if (accountNo.Equals(Convert.ToInt32(data[0])))
                    {
                        return data[1];
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

////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
        public List<string> getSearchResult(Customer c) //accNo, userId, holderName, type, balance, status, customerPosition(true)
        {
            List<string> result = new List<string>();
            // agr sab empty aya howa hai tu yahin sy wapisi
            if (c.AccountNo == -1 && c.AccountHolderName.Equals("") 
                && c.AccountType.Equals("") && c.AccountBalance.Equals(-1)
                && c.UserId.Equals("") && c.Status.Equals(""))
            {
                Console.WriteLine("--none--");
                return null;
            }

            //get Customer for c's fields
            (int count, List<string> customers) = GetStringListofUsers(custFileName);
            (int users_count, List<string> users) = GetStringListofUsers(usersFileName);

            for(int i=0; i<count; i++)
            {
                string[] data = customers[i].Split(',');
                string[] udata = users[i+1].Split(',');

                Customer cust = new Customer();
                string srNo = data[0];
                cust.AccountNo = Convert.ToInt32(data[1]);
                cust.AccountHolderName = data[2];
                cust.AccountType = data[3].Equals("0") ? "saving" : "current";
                cust.AccountBalance = Convert.ToInt32(data[4]);

                cust.UserId = udata[1];
                cust.PinCode = udata[2];
                cust.UserPosition = udata[3].Equals("0") ? false : true;
                cust.Status = udata[4].Equals("1") ? "active" : "disabled";

              
                if (c.AccountNo==-1)//then compare with rest of the fields
                {
                    if(c.UserId.Equals("")) // then then compare with rest of the fields
                    {
                        if(c.AccountHolderName.Equals(""))//then compare with rest of the fields
                        {
                            if(c.AccountType.Equals(""))//then compare with rest of the fields
                            {
                                if( c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                {
                                    if(c.Status.Equals(""))//then //no more fields: not a choice
                                    {
                                        //don't add anything/// sab empty hai
                                    }
                                    else
                                    {   //////add   /// sirf status aya hai
                                        if(c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                        {
                                            if (cust.UserPosition.Equals(c.UserPosition))
                                                result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                        }
                                    }
                                }
                                else
                                {
                                    if(c.AccountBalance.Equals(cust.AccountBalance))
                                    {
                                        if (c.Status.Equals(""))//then //no more fields: not a choice
                                        {
                                            if (cust.UserPosition.Equals(c.UserPosition))
                                                result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                        }
                                        else
                                        {   //////add
                                            if (c.Status.Equals(cust.Status))
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if(c.AccountType.Equals(cust.AccountType))
                                {
                                    if ( c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                    {
                                        if (c.Status.Equals(""))//then //no more fields: not a choice
                                        {
                                            if (cust.UserPosition.Equals(c.UserPosition))
                                                result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                        }
                                        else
                                        {   //////add   /// sirf status aya hai
                                            if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (c.AccountBalance.Equals(cust.AccountBalance))
                                        {
                                            if (c.Status.Equals(""))//then //no more fields: not a choice
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                            else
                                            {   //////add
                                                if (c.Status.Equals(cust.Status))
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (c.AccountType.Equals(""))//then compare with rest of the fields
                            {
                                if ( c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                {
                                    if (c.Status.Equals(""))//then //no more fields: not a choice
                                    {
                                        if (cust.UserPosition.Equals(c.UserPosition))
                                            result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                    }
                                    else
                                    {   //////add   /// sirf status aya hai
                                        if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                        {
                                            if (cust.UserPosition.Equals(c.UserPosition))
                                                result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                        }
                                    }
                                }
                                else
                                {
                                    if (c.AccountBalance.Equals(cust.AccountBalance))
                                    {
                                        if (c.Status.Equals(""))//then //no more fields: not a choice
                                        {
                                            if (cust.UserPosition.Equals(c.UserPosition))
                                                result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                        }
                                        else
                                        {   //////add
                                            if (c.Status.Equals(cust.Status))
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (c.AccountType.Equals(cust.AccountType))
                                {
                                    if ( c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                    {
                                        if (c.Status.Equals(""))//then //no more fields: not a choice
                                        {
                                            if (cust.UserPosition.Equals(c.UserPosition))
                                                result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                        }
                                        else
                                        {   //////add   /// sirf status aya hai
                                            if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (c.AccountBalance.Equals(cust.AccountBalance))
                                        {
                                            if (c.Status.Equals(""))//then //no more fields: not a choice
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                            else
                                            {   //////add
                                                if (c.Status.Equals(cust.Status))
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (c.UserId.Equals(cust.UserId))
                        {
                            if (c.AccountHolderName.Equals(""))//then compare with rest of the fields
                            {
                                if (c.AccountType.Equals(""))//then compare with rest of the fields
                                {
                                    if ( c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                    {
                                        if (c.Status.Equals(""))//then //no more fields: not a choice
                                        {
                                            if (cust.UserPosition.Equals(c.UserPosition))
                                                result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                        }
                                        else
                                        {   //////add   /// sirf status aya hai
                                            if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (c.AccountBalance.Equals(cust.AccountBalance))
                                        {
                                            if (c.Status.Equals(""))//then //no more fields: not a choice
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                            else
                                            {   //////add
                                                if (c.Status.Equals(cust.Status))
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (c.AccountType.Equals(cust.AccountType))
                                    {
                                        if ( c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                        {
                                            if (c.Status.Equals(""))//then //no more fields: not a choice
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                            else
                                            {   //////add   /// sirf status aya hai
                                                if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (c.AccountBalance.Equals(cust.AccountBalance))
                                            {
                                                if (c.Status.Equals(""))//then //no more fields: not a choice
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                                else
                                                {   //////add
                                                    if (c.Status.Equals(cust.Status))
                                                    {
                                                        if (cust.UserPosition.Equals(c.UserPosition))
                                                            result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (c.AccountType.Equals(""))//then compare with rest of the fields
                                {
                                    if ( c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                    {
                                        if (c.Status.Equals(""))//then //no more fields: not a choice
                                        {
                                            if (cust.UserPosition.Equals(c.UserPosition))
                                                result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                        }
                                        else
                                        {   //////add   /// sirf status aya hai
                                            if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (c.AccountBalance.Equals(cust.AccountBalance))
                                        {
                                            if (c.Status.Equals(""))//then //no more fields: not a choice
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                            else
                                            {   //////add
                                                if (c.Status.Equals(cust.Status))
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (c.AccountType.Equals(cust.AccountType))
                                    {
                                        if ( c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                        {
                                            if (c.Status.Equals(""))//then //no more fields: not a choice
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                            else
                                            {   //////add   /// sirf status aya hai
                                                if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (c.AccountBalance.Equals(cust.AccountBalance))
                                            {
                                                if (c.Status.Equals(""))//then //no more fields: not a choice
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                                else
                                                {   //////add
                                                    if (c.Status.Equals(cust.Status))
                                                    {
                                                        if (cust.UserPosition.Equals(c.UserPosition))
                                                            result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else  ///// Add with the 
                {
                    if(c.AccountNo.Equals(cust.AccountNo))
                    {
                        if (c.UserId.Equals("")) // then then compare with rest of the fields
                        {
                            if (c.AccountHolderName.Equals(""))//then compare with rest of the fields
                            {
                                if (c.AccountType.Equals(""))//then compare with rest of the fields
                                {
                                    if (c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                    {
                                        if (c.Status.Equals(""))//then //no more fields: not a choice
                                        {
                                            if (cust.UserPosition.Equals(c.UserPosition))
                                                result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                        }
                                        else
                                        {   //////add   /// sirf status aya hai
                                            if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (c.AccountBalance.Equals(cust.AccountBalance))
                                        {
                                            if (c.Status.Equals(""))//then //no more fields: not a choice
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                            else
                                            {   //////add
                                                if (c.Status.Equals(cust.Status))
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (c.AccountType.Equals(cust.AccountType))
                                    {
                                        if (c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                        {
                                            if (c.Status.Equals(""))//then //no more fields: not a choice
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                            else
                                            {   //////add   /// sirf status aya hai
                                                if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (c.AccountBalance.Equals(cust.AccountBalance))
                                            {
                                                if (c.Status.Equals(""))//then //no more fields: not a choice
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                                else
                                                {   //////add
                                                    if (c.Status.Equals(cust.Status))
                                                    {
                                                        if (cust.UserPosition.Equals(c.UserPosition))
                                                            result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (c.AccountType.Equals(""))//then compare with rest of the fields
                                {
                                    if (c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                    {
                                        if (c.Status.Equals(""))//then //no more fields: not a choice
                                        {
                                            if (cust.UserPosition.Equals(c.UserPosition))
                                                result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                        }
                                        else
                                        {   //////add   /// sirf status aya hai
                                            if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (c.AccountBalance.Equals(cust.AccountBalance))
                                        {
                                            if (c.Status.Equals(""))//then //no more fields: not a choice
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                            else
                                            {   //////add
                                                if (c.Status.Equals(cust.Status))
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (c.AccountType.Equals(cust.AccountType))
                                    {
                                        if (c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                        {
                                            if (c.Status.Equals(""))//then //no more fields: not a choice
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                            else
                                            {   //////add   /// sirf status aya hai
                                                if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (c.AccountBalance.Equals(cust.AccountBalance))
                                            {
                                                if (c.Status.Equals(""))//then //no more fields: not a choice
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                                else
                                                {   //////add
                                                    if (c.Status.Equals(cust.Status))
                                                    {
                                                        if (cust.UserPosition.Equals(c.UserPosition))
                                                            result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (c.UserId.Equals(cust.UserId))
                            {
                                if (c.AccountHolderName.Equals(""))//then compare with rest of the fields
                                {
                                    if (c.AccountType.Equals(""))//then compare with rest of the fields
                                    {
                                        if (c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                        {
                                            if (c.Status.Equals(""))//then //no more fields: not a choice
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                            else
                                            {   //////add   /// sirf status aya hai
                                                if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (c.AccountBalance.Equals(cust.AccountBalance))
                                            {
                                                if (c.Status.Equals(""))//then //no more fields: not a choice
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                                else
                                                {   //////add
                                                    if (c.Status.Equals(cust.Status))
                                                    {
                                                        if (cust.UserPosition.Equals(c.UserPosition))
                                                            result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (c.AccountType.Equals(cust.AccountType))
                                        {
                                            if (c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                            {
                                                if (c.Status.Equals(""))//then //no more fields: not a choice
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                                else
                                                {   //////add   /// sirf status aya hai
                                                    if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                                    {
                                                        if (cust.UserPosition.Equals(c.UserPosition))
                                                            result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (c.AccountBalance.Equals(cust.AccountBalance))
                                                {
                                                    if (c.Status.Equals(""))//then //no more fields: not a choice
                                                    {
                                                        if (cust.UserPosition.Equals(c.UserPosition))
                                                            result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                    }
                                                    else
                                                    {   //////add
                                                        if (c.Status.Equals(cust.Status))
                                                        {
                                                            if (cust.UserPosition.Equals(c.UserPosition))
                                                                result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (c.AccountType.Equals(""))//then compare with rest of the fields
                                    {
                                        if (c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                        {
                                            if (c.Status.Equals(""))//then //no more fields: not a choice
                                            {
                                                if (cust.UserPosition.Equals(c.UserPosition))
                                                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                            }
                                            else
                                            {   //////add   /// sirf status aya hai
                                                if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (c.AccountBalance.Equals(cust.AccountBalance))
                                            {
                                                if (c.Status.Equals(""))//then //no more fields: not a choice
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                                else
                                                {   //////add
                                                    if (c.Status.Equals(cust.Status))
                                                    {
                                                        if (cust.UserPosition.Equals(c.UserPosition))
                                                            result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (c.AccountType.Equals(cust.AccountType))
                                        {
                                            if (c.AccountBalance.Equals(-1))//then compare with rest of the fields
                                            {
                                                if (c.Status.Equals(""))//then //no more fields: not a choice
                                                {
                                                    if (cust.UserPosition.Equals(c.UserPosition))
                                                        result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                }
                                                else
                                                {   //////add   /// sirf status aya hai
                                                    if (c.Status.Equals(cust.Status))       /// agr equal howa tu add
                                                    {
                                                        if (cust.UserPosition.Equals(c.UserPosition))
                                                            result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (c.AccountBalance.Equals(cust.AccountBalance))
                                                {
                                                    if (c.Status.Equals(""))//then //no more fields: not a choice
                                                    {
                                                        if (cust.UserPosition.Equals(c.UserPosition))
                                                            result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                    }
                                                    else
                                                    {   //////add
                                                        if (c.Status.Equals(cust.Status))
                                                        {
                                                            if (cust.UserPosition.Equals(c.UserPosition))
                                                                result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;           
        }

////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////

        public List<string> getSearchResultBTbalance(int minBalance, int maxBalance)
        {
            List<string> result = new List<string>();
            (int count, List<string> customers) = GetStringListofUsers(custFileName);
            (int users_count, List<string> users) = GetStringListofUsers(usersFileName);


            for (int i = 0; i < count; i++)
            {
                string[] data = customers[i].Split(',');
                string[] udata = users[i + 1].Split(',');

                Customer cust = new Customer();
                string srNo = data[0];
                cust.AccountNo = Convert.ToInt32(data[1]);
                cust.AccountHolderName = data[2];
                cust.AccountType = data[3].Equals("0") ? "saving" : "current";
                cust.AccountBalance = Convert.ToInt32(data[4]);

                cust.UserId = udata[1];
                cust.PinCode = udata[2];
                cust.UserPosition = udata[3].Equals("0") ? false : true;
                cust.Status = udata[4].Equals("1") ? "active" : "disabled";

                if (Convert.ToInt32(data[4]) >=minBalance && Convert.ToInt32(data[4]) <= maxBalance)
                {
                    result.Add($"{cust.AccountNo}\t {cust.UserId}\t {cust.AccountHolderName}\t {cust.AccountType}\t {cust.AccountBalance}\t {cust.Status}\n");
                }

            }
            return result;
        }


        public bool greaterOREqual(string d1, string d2)
        {
            string[] date1 = d1.Split('/');
            int dd = Convert.ToInt32(date1[0]);
            int mm = Convert.ToInt32(date1[1]);
            int yy = Convert.ToInt32(date1[2]);

            string[] date2 = d2.Split('/');
            int dd2 = Convert.ToInt32(date2[0]);
            int mm2 = Convert.ToInt32(date2[1]);
            int yy2 = Convert.ToInt32(date2[2]);

            
            if (yy>yy2)
            {
                return true;
            }
            else if(yy==yy2) // warna 
            {
                
                if(mm>mm2)
                {
                    return true;
                }
                else if(mm==mm2)
                {
                    if(dd>=dd2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool lessOREqual(string d1, string d2)
        {
            string[] date1 = d1.Split('/');
            int dd = Convert.ToInt32(date1[0]);
            int mm = Convert.ToInt32(date1[1]);
            int yy = Convert.ToInt32(date1[2]);

            string[] date2 = d2.Split('/');
            int dd2 = Convert.ToInt32(date2[0]);
            int mm2 = Convert.ToInt32(date2[1]);
            int yy2 = Convert.ToInt32(date2[2]);


            if (yy < yy2)
            {
                return true;
            }
            else if (yy == yy2) // warna 
            {

                if (mm < mm2)
                {
                    return true;
                }
                else if (mm == mm2)
                {
                    if (dd <= dd2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool equals(string d1, string d2)
        {
            string[] date1 = d1.Split('/');
            int dd = Convert.ToInt32(date1[0]);
            int mm = Convert.ToInt32(date1[1]);
            int yy = Convert.ToInt32(date1[2]);

            string[] date2 = d2.Split('/');
            int dd2 = Convert.ToInt32(date2[0]);
            int mm2 = Convert.ToInt32(date2[1]);
            int yy2 = Convert.ToInt32(date2[2]);

            if (yy == yy2 && mm==mm2 && dd==dd2)
            {
                return true;
            }
            return false;
        }

        public List<string> getSearchResultBTdates(string startingDate, string endingDate)
        {
            List<string> result = new List<string>();
            (int trans_count, List<string> trans) = GetStringListofUsers(transactionFileName);

            foreach(string t in trans)
            {
                // accountNo = 11221, TransactionType = 0, Amount=5000, Date=12/09/2020, To=accNo
                  //              0-withdraw, 1-Deposit, 2-Transfer
                string[] data = t.Split(',');
                Transaction tr = new Transaction();
                tr.AccountNo = Convert.ToInt32(data[0]);

                tr.TransactionType = Convert.ToInt32(data[1]);  //0-withdraw, 1-Deposit, 2-Transfer
                tr.TransactionAmount = Convert.ToInt32(data[2]);
                tr.TransactionDate = data[3];
                if (tr.TransactionType == 2)
                    tr.To = Convert.ToInt32(data[2]);
                else
                    tr.To = -1;
                

                if (greaterOREqual(tr.TransactionDate, startingDate) && lessOREqual(tr.TransactionDate, endingDate) )
                {
                    string type = tr.TransactionType==0 ? "Cash Withdrawal" : tr.TransactionType==1? "Cash Deposit" : "Cash Transfer";
                    if(tr.TransactionType!=2)
                        result.Add($"\n{type}, {getUserIdWithAccountNo(tr.AccountNo)}, {getAccountHolderNameWithAccountNo(tr.AccountNo)}, {tr.TransactionAmount}, {tr.TransactionDate}");
                    else
                        result.Add($"\n{type}, {getUserIdWithAccountNo(tr.AccountNo)}, {getAccountHolderNameWithAccountNo(tr.AccountNo)}, {tr.TransactionAmount}, {tr.TransactionDate}, {tr.To}");
                }
            }
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
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
        public int getUsedLimit(int accountNo, string today)
        {
            (int count, List<string> limits) = GetStringListofUsers(limitFileName);
            int i = 0;
            if (count > 0 && limits != null)
            {
                foreach (string l in limits)
                {
                    if (i == 0)
                    {
                        i++;
                        continue;
                    }
                    string[] data = l.Split(',');
                    if (accountNo.Equals(Convert.ToInt32(data[0])) && equals(today, data[1]))
                    {
                        return Convert.ToInt32(data[2]);
                    }
                }
                // not exits , so set it.
                setLimit(accountNo);    // for today
                return 0;
            }
            return -1;
        }

        public void setLimit(int accountNo, int limit=0) //for today
        {
            // write to file for getToday();
            string details = $"{accountNo}, {getToday()}, {limit}";
            string limitFile = Path.Combine(Environment.CurrentDirectory, limitFileName);
            StreamWriter srLimit = null;
            try
            {
                srLimit = new StreamWriter(limitFile, append: true);
                srLimit.WriteLine(details);
            }
            catch
            {
                Console.WriteLine("Exception at updating Limit history file.");
            }
            finally
            {
                srLimit.Close();
            }
            return;
        }

        public void updateLimit(int accountNo, int amount)
        {
            (int count, List<string> limits) = GetStringListofUsers(limitFileName);
            List<string> updatedLimits = new List<string>();
            if (count > 0 && limits != null)
            {
                foreach (string l in limits)
                {
                    string[] data = l.Split(',');
                    if ((accountNo.Equals(Convert.ToInt32(data[0])) && equals(getToday(), data[1])))
                    {
                        int newUsedLimit = Convert.ToInt32(data[2]) + amount;
                        updatedLimits.Add($"{accountNo},{getToday()},{newUsedLimit}");
                    }
                    else
                    {
                        updatedLimits.Add(l);
                    }
                }
                writeBacktoFile(count, updatedLimits, limitFileName);
            }
        }
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
        public bool DepositAmount(int accountNo, int amountToDeposit)
        {
            bool res = doDepositAmount(accountNo, amountToDeposit);

            string dd = DateTime.Now.Day.ToString();
            string mm = DateTime.Now.Month.ToString();
            string yy = DateTime.Now.Year.ToString();

            string date = dd + "/" + mm + "/" + yy;

            Transaction wT = new Transaction();
            wT.AccountNo = accountNo;
            wT.TransactionType = 1; // 1 for deposit
            wT.TransactionAmount = amountToDeposit;
            wT.TransactionDate = date;
            saveTransaction(wT);

            return res;
        }

        bool doDepositAmount(int accountNo, int amountToDeposit)
        {
            Customer c = getCustomer(accountNo);
            c.AccountBalance += amountToDeposit;
            return UpdateCustomer(c);
        }

//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
        public bool TransferAmountTo(int accountNo, int amountToTransfer, string currentUserId)
        {
            int from = getAccountNo(currentUserId);
            if(from==accountNo)
            {
                return false;
            }
            doWithdrawAmount(from, amountToTransfer);
            bool res = doDepositAmount(accountNo, amountToTransfer);

            string dd = DateTime.Now.Day.ToString();
            string mm = DateTime.Now.Month.ToString();
            string yy = DateTime.Now.Year.ToString();

            string date = dd + "/" + mm + "/" + yy;


            Transaction wT = new Transaction();
            wT.AccountNo = from;
            wT.TransactionType = 2; // 2 for Transfer
            wT.TransactionAmount = amountToTransfer;
            wT.TransactionDate = date;
            wT.To = accountNo;
            saveTransaction(wT);

            return res;
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


        public bool checkWithdrawLimit(int amount, string userId)
        {
            if (amount > 20000)
                return false;
            int accountNo = getAccountNo(userId);
            int used_limit = getUsedLimit(accountNo, getToday());
            
            if (used_limit == 20000)
            {
                return false;
            }
            else
            {
                int afterLimit = used_limit + amount;
                if (afterLimit > 20000)
                    return false;
                else
                    return true;
            }
        }

        string getToday()
        {
            string dd = DateTime.Now.Day.ToString();
            string mm = DateTime.Now.Month.ToString();
            string yy = DateTime.Now.Year.ToString();

            return dd + "/" + mm + "/" + yy;
        }

        public void withdrawAmount(int amount, string userId)
        {
            int accountNo = getAccountNo(userId);
            doWithdrawAmount(accountNo, amount);

            
            updateLimit(accountNo, amount);
            string date = getToday();

            Transaction wT = new Transaction();
            wT.AccountNo = accountNo;
            wT.TransactionType = 0; // 0 for Withdraw
            wT.TransactionAmount = amount;
            wT.TransactionDate = date;
            saveTransaction(wT);
        }

        void doWithdrawAmount(int accountNo, int amount)
        {
            Customer c = getCustomer(accountNo);
            c.AccountBalance -= amount;
            UpdateCustomer(c);
        }

        public void saveTransaction(Transaction trans)
        {
            int trans_Count = GetCountof(transactionFileName);  //0=1000, 1=1001...
            int newTransNo;
            newTransNo = trans_Count + 1;

            // accountNo = 11221, TransactionType = 0, Amount=5000, Date=12/09/2020, To=accNo
            string tran_detail ="";
            if (trans.TransactionType == 2) // if transaction is of 'Transfer Type'
            {
                tran_detail = $"{trans.AccountNo},{trans.TransactionType},{trans.TransactionAmount},{trans.TransactionDate},{trans.To}";
            }
            else
            {
                tran_detail = $"{trans.AccountNo},{trans.TransactionType},{trans.TransactionAmount},{trans.TransactionDate}";
            }

            string tranFile = Path.Combine(Environment.CurrentDirectory, transactionFileName);
            StreamWriter srTrans = null;
            try
            {
                srTrans = new StreamWriter(tranFile, append: true);
                srTrans.WriteLine(tran_detail);
            }
            catch
            {
                Console.WriteLine("Exception at updating transaction history file.");
            }
            finally
            {
                srTrans.Close();
                incrementCount(transactionFileName);
            }
            return;
        }
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
    }
}

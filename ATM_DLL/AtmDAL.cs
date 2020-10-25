using System;
using System.IO;
using ATM_BObjects;
using System.Collections.Generic;
using System.Text;

namespace ATM_DLL
{
    public class AtmDAL
    {
        string custFileName = "customer.csv";
        string usersFileName = "users.csv";
        string transactionFileName = "transaction.csv";

        public void SetUpSystem()
        {
            string customerFile = Path.Combine(Environment.CurrentDirectory, custFileName);
            if (!File.Exists(customerFile))
            {
                File.Create(customerFile);
               
            }

            string usersFile = Path.Combine(Environment.CurrentDirectory, usersFileName);
            if (!File.Exists(usersFile))
            {
                StreamWriter sr = new StreamWriter(usersFile, append: true);
                sr.WriteLine(1);
                sr.WriteLine("admin,11221,0,1");    // userId=admin, pinCode=11221, position=0(Admin), Status=1(Active)
                sr.Close();
            }

            string transactionFile = Path.Combine(Environment.CurrentDirectory, transactionFileName);
            if (!File.Exists(transactionFile))
                File.Create(transactionFile);

        }

        public int GetCountof(string fileName)
        {
            string usersFile = Path.Combine(Environment.CurrentDirectory, fileName);
            int count = 0;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(usersFile);
                count = Convert.ToInt32(sr.ReadLine());
            }
            catch
            {
                return -1;
            }
            finally
            {
                sr.Close();
            }

            return count;
        }

        (int, List<string>) GetStringListof(string fileName)
        {
            string usersFile = Path.Combine(Environment.CurrentDirectory, fileName);
            List<string> users = new List<string>();
            int count = 0;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(usersFile);

                count = Convert.ToInt32(sr.ReadLine());
                string user = String.Empty;
                while ((user = sr.ReadLine()) != null)
                {
                    users.Add(user);
                }
            }
            catch
            {
                return (-1, null);
            }
            finally
            {
                sr.Close();
            }

            return (count, users); //  if status is disabled(0)
                                   //    return 1; //  if status is Active(1)
                                   //   return 0; //  if status is notExists(0)
        }


        public int checkStatus(string userId)       // 1-Acive and 0-Disabled and -1-NotExists 
        {
            (int count, List<string> users) = GetStringListof(usersFileName);

            if (count>0 && users != null)
            {
                foreach(string l in users)
                {
                    string[] data = l.Split(',');
                    Users user = new Users();
                    user.UserId = data[0];
                    user.PinCode = data[1];
                    user.UserPosition = data[2]=="1"? true: false;  // true-Customer and False-Admin
                    user.Status = data[3] == "1" ? true : false;    // true-Active and false-Disabled

                    if (userId.Equals(user.UserId))
                    {
                        return user.Status ? 1 : 0; // true(1)-Active,    false(0)-Disabled
                    }
                }
            }
            return -1; //  for NotExists.
        }

        public int checkPosition(string userId)     // 1-Customer and 2 for Admin
        {
            (int count, List<string> users) = GetStringListof(usersFileName);

            if (count > 0 && users != null)
            {
                foreach (string l in users)
                {
                    string[] data = l.Split(',');
                    Users user = new Users();
                    user.UserId = data[0];
                    user.PinCode = data[1];
                    user.UserPosition = data[2].Equals("1") ? true : false;  // true-Customer and False-Admin
                    user.Status = data[3].Equals("1") ? true : false;    // true-Active and false-Disabled

                    if (userId.Equals(user.UserId))
                    {
                        return user.UserPosition ? 1 : 2; // true(1)-Customer/1    false(0)-Admin/2
                    }
                }
            }
            return 0; //  for NotExists.
        }

        public void setDisabled(string userId)
        {
            (int count, List<string> users) = GetStringListof(usersFileName);
            List<string> updated_users = new List<string>();
            if (count > 0 && users != null)
            {
                foreach (string l in users)
                {
                    string[] data = l.Split(',');
                    Users user = new Users();
                    user.UserId = data[0];
                    user.PinCode = data[1];
                    user.UserPosition = (data[2] == "1" ? true : false);  // true-Customer and False-Admin
                    user.Status = (data[3] == "1" ? true : false);    // true-Active and false-Disabled

                    if (userId.Equals(user.UserId))
                    {
                        user.Status = false;    // set Disabled!                       
                    }
                    int pos = user.UserPosition ? 1 : 0;
                    int stats = user.Status ? 1 : 0;
                    string line = $"{user.UserId},{user.PinCode},{pos},{stats}";
                    updated_users.Add(line);
                }
                writeBacktoFile(count, updated_users);
            }
            
        }


        public bool verifyPinCode(string userId, string pinCode)
        {
            (int count, List<string> users) = GetStringListof(usersFileName);

            if (count > 0 && users != null)
            {
                foreach (string l in users)
                {
                    string[] data = l.Split(',');
                    Users user = new Users();
                    user.UserId = data[0];
                    user.PinCode = data[1];
                    user.UserPosition = data[2] == "1" ? true : false;  // true-Customer and False-Admin
                    user.Status = data[3] == "1" ? true : false;    // true-Active and false-Disabled

                    if (userId.Equals(user.UserId) && pinCode.Equals(user.PinCode))
                    {
                        return true; // true(1)-Customer/1    false(0)-Admin/2
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false; //  for NotExists.
        }

        void writeBacktoFile(int count, List<string> users)
        {
            string usersFile = Path.Combine(Environment.CurrentDirectory, usersFileName);
            StreamWriter sr = new StreamWriter(usersFile, append: false);
            sr.WriteLine(count);
            foreach(string s in users)
            {
                sr.WriteLine(s);
            }
            sr.Close();
        }

        public int createNewAccount(Customer c)
        {
            // save the new customer's account info. and return the acoount no
            return 50;
        }

        public string getAccountHolderNameWithAccountNo(int accountNo)
        {
            return "Abke";
        }

        public bool DeleteAccount(int accountNo)
        {
            return true;
        }

        public Customer getCustomer(int accountNo)
        {
            Customer c = new Customer();
            return c;
        }

        public bool UpdateCustomer(Customer updatedCustomer)
        {
            return true;
        }

        public int GetBalance(int accountNo)
        {
            return 5;
        }

        public int getAccountNo(string accountNo)
        {
            return 234;
        }

        public bool DepositAmount(int accountNo, int amountToDeposit)
        {
            return true;
        }
    }
}

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
                StreamWriter sr = new StreamWriter(customerFile);
                sr.WriteLine(1);
                sr.WriteLine("11221,Ali,0,5000");    // accountNo = 11221, AccountHolderName = Ali, Type=Saving(0), Balance=5000
                sr.Close();
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
            {
                StreamWriter sr = new StreamWriter(transactionFile);
                sr.WriteLine(1);
                sr.WriteLine("11221,0,500,15/10/2020");    // accountNo = 11221, TransactionType = 0, Type=Saving(0), Balance=5000
                sr.Close();                                 //                  0-withdraw, 1-Deposit, 2-Transfer
            }
        }






        public int GetCountof(string fileName)
        {
            string usersFile = Path.Combine(Environment.CurrentDirectory, fileName);
            int count = 0;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(fileName);
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

        (int, List<string>) GetStringListofUsers(string fileName)
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

            return (count, users);
        }


        
        /// <summary>
        ///     working for layers
        /// </summary>
        /// 
        public int checkStatus(string userId)       // 1-Acive and 0-Disabled and -1-NotExists 
        {
            (int count, List<string> users) = GetStringListofUsers(usersFileName);

            if (count>0 && users != null)
            {
                foreach(string l in users)
                {
                    string[] data = l.Split(',');
                    Users user = new Users();       ///// userId=admin, pinCode=11221, position=0(Admin), Status=1(Active)
                    user.UserId = data[0];
                    user.PinCode = data[1];
                    user.UserPosition = data[2].Equals("1")? true: false;  // true(1)-Customer and False(0)-Admin
                    user.Status = data[3].Equals("1") ? "active" : "disabled";    // true-Active and false-Disabled

                    if (userId.Equals(user.UserId))
                    {
                        int ret = data[3].Equals("1") ? 1 : data[3].Equals("0") ? 0 : -1; // true(1)-Active,    false(0)-Disabled
                       // Console.WriteLine("->" + user.Status + "->" +ret);
                        return ret;
                    }
                }
            }
            return -1; //  for NotExists.
        }


        public int checkPosition(string userId)     // 1-Customer and 2 for Admin
        {
            (int count, List<string> users) = GetStringListofUsers(usersFileName);

            if (count > 0 && users != null)
            {
                foreach (string l in users)
                {
                    string[] data = l.Split(',');
                    Users user = new Users();
                    user.UserId = data[0];
                    user.PinCode = data[1];
                    user.UserPosition = data[2].Equals("1") ? true : false;  // true-Customer and False-Admin
                    user.Status = data[3].Equals("1") ? "active" : "disabled";    // true-Active and false-Disabled

                    if (userId.Equals(user.UserId))
                    {
                        int ret= user.UserPosition ? 1 : 2; // true(1)-Customer/1    false(0)-Admin/2
                        Console.WriteLine("->" + user.Status + "->" + ret);
                        return ret;
                    }
                }
            }
            return 0; //  for NotExists.
        }


        public void setDisabled(string userId)
        {
            (int count, List<string> users) = GetStringListofUsers(usersFileName);
            List<string> updated_users = new List<string>();
            if (count > 0 && users != null)
            {
                foreach (string l in users)
                {
                    string[] data = l.Split(',');
                    Users user = new Users();
                    user.UserId = data[0];
                    user.PinCode = data[1];
                    user.UserPosition = (data[2].Equals("1") ? true : false);  // true-Customer and False-Admin
                    user.Status = data[3].Equals("1") ? "active" : "disabled";    // true-Active and false-Disabled

                    if (userId.Equals(user.UserId))
                    {
                        user.Status = "disabled";    // set Disabled!                       
                    }
                    int pos = user.UserPosition ? 1 : 0;
                    int stats = (user.Status).Equals("disabled") ? 0 : 1;
                    string line = $"{user.UserId},{user.PinCode},{pos},{stats}";
                    updated_users.Add(line);
                }
                writeBacktoFile(count, updated_users, usersFileName);
            }
            
        }

        void writeBacktoFile(int count, List<string> users, string file)
        {
            string usersFile = Path.Combine(Environment.CurrentDirectory, file);
            StreamWriter sr = new StreamWriter(usersFile, append: false);
            sr.WriteLine(count);
            foreach (string s in users)
            {
                sr.WriteLine(s);
            }
            sr.Close();
        }



        public bool verifyPinCode(string userId, string pinCode)
        {
            (int count, List<string> users) = GetStringListofUsers(usersFileName);

            if (count > 0 && users != null)
            {
                foreach (string l in users)
                {
                    string[] data = l.Split(',');
                    Users user = new Users();
                    user.UserId = data[0];
                    user.PinCode = data[1];
                    user.UserPosition = data[2].Equals("1") ? true : false;  // true-Customer and False-Admin
                    user.Status = data[3].Equals("1") ? "active" : "disabled";    // true-Active and false-Disabled

                    Console.WriteLine(l);
                    Console.WriteLine(user.UserId + user.PinCode);

                    if (userId.Equals(user.UserId) && pinCode.Equals(user.PinCode))
                    {
                        return true; // true(1)-Customer/1    false(0)-Admin/2
                    }
                }
            }
            return false; //  for NotExists.
        }


        public int createNewAccount(Customer c)
        {
            // save the new customer's account info. and return the acoount no
            return 50;
        }

        public string getAccountHolderNameWithAccountNo(int accountNo)
        {
            return "";
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


        public bool TransferAmountTo(int accountNo, int amountToTransfer, string currentUserId)
        {
            return true;
        }
    }
}

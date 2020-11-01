using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ATM_BObjects;

namespace ATM_DLL
{
    public class LoginProcess
    {
        protected string custFileName = "customer.csv";
        protected string usersFileName = "users.csv";
        protected string transactionFileName = "transaction.csv";
        protected string limitFileName = "limits.csv";

        public void SetUpSystem()
        {
            string customerFile = Path.Combine(Environment.CurrentDirectory, custFileName);
            if (!File.Exists(customerFile))
            {
                StreamWriter sr = new StreamWriter(customerFile);
                sr.WriteLine(1);
                sr.WriteLine("1,1,Ali,0,5000");    // accountNo = 1, AccountHolderName = Ali, Type=Saving(0), Balance=5000
                sr.Close();
            }

            string usersFile = Path.Combine(Environment.CurrentDirectory, usersFileName);
            if (!File.Exists(usersFile))
            {
                StreamWriter sr = new StreamWriter(usersFile, append: true);
                sr.WriteLine(2);
                sr.WriteLine("0,admin,11221,0,1");    // userId=admin, pinCode=11221, position=0(Admin), Status=1(Active)
                sr.WriteLine("1,ali,11222,1,1");    // userId=admin, pinCode=11221, position=0(Admin), Status=1(Active)
                sr.Close();
            }

            string transactionFile = Path.Combine(Environment.CurrentDirectory, transactionFileName);
            if (!File.Exists(transactionFile))
            {
                StreamWriter sr = new StreamWriter(transactionFile);
                sr.WriteLine(1);
                sr.WriteLine("1,2,500,15-Oct-20, 2");    // accountNo = 11221, TransactionType = 0, Amount=5000, Date=12/09/2020, To=accNo
                sr.Close();                                 //              0-withdraw, 1-Deposit, 2-Transfer
            }

            string limitFile = Path.Combine(Environment.CurrentDirectory, limitFileName);
            if (!File.Exists(limitFile))
            {
                StreamWriter sr = new StreamWriter(limitFile);
                sr.WriteLine(1);
                sr.WriteLine("1,15/5/2020, 100");    // accountNo = 11221, date = 0, limitUsed=5000
                sr.Close();
            }
        }

///////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////
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


        public void incrementCount(string FileName)
        {
            (int count, List<string> users) = GetStringListofUsers(FileName);

            if (count > 0 && users != null)
            {
                count = count + 1;
                writeBacktoFile(count, users, FileName);            }
            else
            {
                Console.WriteLine("There is some error, while incrementing count of file, please contact Developer");
            }
        }

        public (int, List<string>) GetStringListofUsers(string fileName)
        {
            string usersFile = Path.Combine(Environment.CurrentDirectory, fileName);
            List<string> users = new List<string>();
            int count = 0;
            StreamReader sr = new StreamReader(usersFile);
            try
            {
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

        public int checkStatus(string userId)       // 1-Acive and 0-Disabled and -1-NotExists 
        {
            (int count, List<string> users) = GetStringListofUsers(usersFileName);

            if (count > 0 && users != null)
            {
                foreach (string l in users)
                {
                    string[] data = l.Split(',');
                    Users user = new Users();       ///// userId=admin, pinCode=11221, position=0(Admin), Status=1(Active)
                    user.UserId = data[1];
                    user.PinCode = data[2];
                    user.UserPosition = data[3].Equals("1") ? true : false;  // true(1)-Customer and False(0)-Admin
                    user.Status = data[4].Equals("1") ? "active" : "disabled";    // true-Active and false-Disabled

                    if (userId.Equals(user.UserId))
                    {
                        int ret = data[4].Equals("1") ? 1 : data[3].Equals("0") ? 0 : -1; // true(1)-Active,    false(0)-Disabled
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
                    user.UserId = data[1];
                    user.PinCode = data[2];
                    user.UserPosition = data[3].Equals("1") ? true : false;  // true-Customer and False-Admin
                    user.Status = data[4].Equals("1") ? "active" : "disabled";    // true-Active and false-Disabled

                    if (userId.Equals(user.UserId))
                    {
                        int ret = user.UserPosition ? 1 : 2; // true(1)-Customer/1    false(0)-Admin/2
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
                    user.UserId = data[1];
                    user.PinCode = data[2];
                    user.UserPosition = (data[3].Equals("1") ? true : false);  // true-Customer and False-Admin
                    user.Status = data[4].Equals("1") ? "active" : "disabled";    // true-Active and false-Disabled

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

        public void writeBacktoFile(int count, List<string> users, string file)
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

///////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////

        public bool verifyPinCode(string userId, string pinCode)
        {
            (int count, List<string> users) = GetStringListofUsers(usersFileName);

            if (count > 0 && users != null)
            {
                foreach (string l in users)
                {
                    string[] data = l.Split(',');
                    Users user = new Users();
                    user.UserId = data[1];
                    user.PinCode = data[2];
                    user.UserPosition = data[3].Equals("1") ? true : false;  // true-Customer and False-Admin
                    user.Status = data[4].Equals("1") ? "active" : "disabled";    // true-Active and false-Disabled

                 //   Console.WriteLine(l);
                 //   Console.WriteLine(user.UserId + user.PinCode);

                    if (userId.Equals(user.UserId) && pinCode.Equals(user.PinCode))
                    {
                        return true; // true(1)-Customer/1    false(0)-Admin/2
                    }
                }
            }
            return false; //  for NotExists.
        }

///////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////
    }
}

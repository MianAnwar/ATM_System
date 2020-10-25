using System;
using ATM_VIEW;

namespace ATM_System
{
    class Program
    {
        static void Main(string[] args)
        {
            MainPage m = new MainPage();
           
            Console.Write("Press Enter to Start! ");
            Console.ReadKey();

            m.loginToProceed();

            Console.Write("\nPress Enter to Exit! ");
            Console.ReadKey();
        }
    }
}

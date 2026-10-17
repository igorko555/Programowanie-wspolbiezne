using System;

namespace SimpleDelegate
{
    class Program
    {
        delegate void PrintSomething(string sth);

        static void PrintOryginal(string sth)
        {
            Console.WriteLine(sth);
        }

        static void PrintInCapital(string sth)
        {
            Console.WriteLine(sth.ToUpper());
        }

        static void Main(string[] args)
        {
            PrintSomething printingDelegate = new PrintSomething(PrintOryginal);
            printingDelegate("Hello World");
            printingDelegate = new PrintSomething(PrintInCapital);
            printingDelegate("Hello World");
            printingDelegate += new PrintSomething(PrintOryginal);
            printingDelegate("Hello New World!");
            Console.WriteLine("Printing delegate has following methods:");
            foreach (var item in printingDelegate.GetInvocationList())
            {
                Console.WriteLine(item.Method.Name);
            }
        }
    }
}

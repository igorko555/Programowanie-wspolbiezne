using System;
using System.Threading;
using System.Threading.Tasks;

namespace FirstTasks
{
    class Punkt1
    {
        static void Main(string[] args)
        {
            Task t1 = new Task(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.Write($"{i} ");
                    Thread.Sleep(100);
                }
            });

            t1.Start();
            t1.Wait();

            Console.WriteLine("\nZadanie 1 zakonczone.");
        }
    }
}

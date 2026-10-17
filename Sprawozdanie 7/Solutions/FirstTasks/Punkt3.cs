using System;
using System.Threading;
using System.Threading.Tasks;

namespace FirstTasks
{
    class Punkt3
    {
        static void task3(object o)
        {
            int limit = (int)o;
            for (int i = 0; i < limit; i++)
            {
                Console.Write($"({i}) ");
                Thread.Sleep(80);
            }
        }

        static void Main(string[] args)
        {
            Task t3 = new Task(task3, 15);
            t3.Start();
            t3.Wait();

            Console.WriteLine("\nZadanie 3 zakonczone.");
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace FirstTasks
{
    class Punkt2
    {
        static void task2()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"[{i}] ");
                Thread.Sleep(100);
            }
        }

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

            Task t2 = new Task(task2);

            t1.Start();
            t2.Start();

            t1.Wait();
            t2.Wait();

            Console.WriteLine("\nZadanie 2 zakonczone.");
        }
    }
}

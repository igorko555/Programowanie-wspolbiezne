using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FirstTasks
{
    class Punkt1
    {
        private const int iterNo = 10;

        static void Main(string[] args)
        {
            Task t1 = new Task(() =>
            {
                Random rand = new Random(Task.CurrentId.Value);
                Console.WriteLine($"Thread id {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(rand.Next(0, 100));
                for (int i = 0; i < iterNo; i++)
                {
                    Console.Write($"#{i}# ");
                    Thread.Sleep(rand.Next(500));
                }
                Console.WriteLine($"\nTask {Task.CurrentId.Value} finished");
            });
            t1.Start();

            Console.WriteLine($"Thread id {Thread.CurrentThread.ManagedThreadId}");
            Random rand = new Random();
            Thread.Sleep(rand.Next(0, 100));
            for (int i = 0; i < iterNo; i++)
            {
                Console.Write($"{{{i}}} ");
                Thread.Sleep(rand.Next(500));
            }


            Console.WriteLine("\nMain task finished");
            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FirstTasks
{
    class Punkt3
    {
        private const int iterNo = 10;

        static void task2()
        {
            Random rand = new Random(Task.CurrentId.Value);
            Console.WriteLine($"Thread id {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(rand.Next(0, 100));
            for (int i = 0; i < iterNo; i++)
            {
                Console.Write($"[{i}] ");
                Thread.Sleep(rand.Next(500));
            }
            Console.WriteLine($"\nTask {Task.CurrentId.Value} finished");
        }

        static void task3(object o)
        {
            int limit = (int)o;
            Random rand = new Random(Task.CurrentId.Value);
            Console.WriteLine($"Thread id {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(rand.Next(0, 100));
            for (int i = 0; i < limit; i++)
            {
                Console.Write($"({i}) ");
                Thread.Sleep(rand.Next(500));
            }
            Console.WriteLine($"\nTask {Task.CurrentId.Value} finished");
        }

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

            Task t2 = new Task(task2);
            t2.Start();

            Task t3 = new Task(task3, 10);
            t3.Start();

            Console.WriteLine($"Thread id {Thread.CurrentThread.ManagedThreadId}");
            Random rand = new Random();
            Thread.Sleep(rand.Next(0, 100));
            for (int i = 0; i < iterNo; i++)
            {
                Console.Write($"{{{i}}} ");
                Thread.Sleep(rand.Next(500));
            }

            t1.Wait();
            t2.Wait();
            t3.Wait();

            Console.WriteLine("\nMain task finished");
            Console.ReadKey();
        }
    }
}

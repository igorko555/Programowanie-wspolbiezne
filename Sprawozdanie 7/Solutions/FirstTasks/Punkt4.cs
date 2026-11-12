using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FirstTasks
{
    class Punkt4
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

        static void task4(object arg)
        {
            (int id, string name) tupleArg = ((int id, string name))arg;
            string taskName = tupleArg.name;
            int taskId = tupleArg.id;
            Console.WriteLine($"I'm {taskName} and have ID: {taskId}");
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

            (int id, string name) idName = (4, "Task4");
            Task t4 = new Task(task4, idName);
            t4.Start();

            Task t4b = new Task(task4, (4, "Task4b"));
            t4b.Start();

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
            t4.Wait();
            t4b.Wait();

            Console.WriteLine("\nMain task finished");
            Console.ReadKey();
        }
    }
}

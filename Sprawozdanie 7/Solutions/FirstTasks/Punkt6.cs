using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FirstTasks
{
    public class MyParam
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    class Punkt6
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

        static void task5(object o)
        {
            MyParam p = (MyParam)o;
            Console.WriteLine($"I'm {p.Name} and have ID: {p.Id}");
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

            MyParam myParam = new MyParam { Name = "Task5", Id = 5 };
            Task t5 = new Task(task5, myParam);
            t5.Start();

            int taskNo = 5;
            List<Task> t5list = new List<Task>();
            for (int i = 0; i < taskNo; i++)
            {
                MyParam p = new MyParam { Id = i, Name = "Task" + i.ToString() };
                t5list.Add(Task.Factory.StartNew(task5, p));
            }

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
            t5.Wait();

            foreach (var t in t5list)
            {
                t.Wait();
            }

            Console.WriteLine("\nMain task finished");
            Console.ReadKey();
        }
    }
}

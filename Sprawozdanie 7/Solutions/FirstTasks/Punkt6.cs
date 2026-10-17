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
        static void task5(object o)
        {
            MyParam p = (MyParam)o;
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"[Lista Zadanie: {p.Name} ID: {p.Id} iteracja: {i}]");
                Thread.Sleep(60);
            }
        }

        static void Main(string[] args)
        {
            List<Task> taskList = new List<Task>();

            for (int i = 1; i <= 5; i++)
            {
                MyParam p = new MyParam { Name = $"Task_{i}", Id = i * 10 };
                taskList.Add(Task.Factory.StartNew(task5, p));
            }

            Task.WaitAll(taskList.ToArray());

            Console.WriteLine("\nZadanie 6 zakonczone pomyslnie.");
        }
    }
}

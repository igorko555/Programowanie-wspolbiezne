using System;
using System.Threading;
using System.Threading.Tasks;

namespace FirstTasks
{
    public class MyParam
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    class Punkt5
    {
        static void task5(object o)
        {
            MyParam p = (MyParam)o;
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"[Klasa {p.Name} (ID: {p.Id}) krok {i}]");
                Thread.Sleep(100);
            }
        }

        static void Main(string[] args)
        {
            MyParam p = new MyParam { Name = "ParametrTestowy", Id = 100 };
            Task t5 = new Task(task5, p);
            t5.Start();
            t5.Wait();

            Console.WriteLine("\nZadanie 5 zakonczone.");
        }
    }
}

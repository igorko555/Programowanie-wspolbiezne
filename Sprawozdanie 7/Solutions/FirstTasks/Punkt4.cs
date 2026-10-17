using System;
using System.Threading;
using System.Threading.Tasks;

namespace FirstTasks
{
    class Punkt4
    {
        static void task4(object arg)
        {
            var tuple = ((int id, string name))arg;
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine($"<{tuple.name} ID={tuple.id} iter={i}>");
                Thread.Sleep(100);
            }
        }

        static void Main(string[] args)
        {
            Task t4 = new Task(task4, (1, "ZadanieAlfa"));
            Task t4b = new Task(task4, (2, "ZadanieBeta"));

            t4.Start();
            t4b.Start();

            t4.Wait();
            t4b.Wait();

            Console.WriteLine("\nZadanie 4 zakonczone.");
        }
    }
}

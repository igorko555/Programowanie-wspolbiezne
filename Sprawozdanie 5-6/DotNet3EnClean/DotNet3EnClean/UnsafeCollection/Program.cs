using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnsafeCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Thread> threads = new List<Thread>();
            List<int> numbers = new List<int>(1000000);
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                var watek = new Thread(() =>
                {
                    for (int l = 0; l < 10000; l++)
                        numbers.Add(rand.Next());
                });
                threads.Add(watek);
                watek.Start();
            }
            foreach (var watek in threads)
            {
                watek.Join();
            }
            Console.WriteLine($"The number of items in a regular list: {numbers.Count}");
            Console.ReadLine();
        }
    }
}

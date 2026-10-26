using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace UniqueCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Thread> threads = new List<Thread>();
            ConcurrentBag<int> numbers = new ConcurrentBag<int>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                var watek = new Thread(() =>
                {

                    for (int l = 0; l < 100; l++)
                    {
                        int number = rand.Next(10001);
                        while (numbers.Any(x => x == number))
                        {
                            number = rand.Next(10001);
                        };
                        numbers.Add(number);
                    }
                });
                threads.Add(watek);
                watek.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            sw.Stop();

            Console.WriteLine($"Elapsed miliseconds {sw.ElapsedMilliseconds} ms");
            if (numbers.IsUnique()) //using extension method for CB
                Console.WriteLine("All numbers are unique");
            else
                Console.WriteLine("Not all numbers are unique");


            Console.ReadLine();
        }
    }
}

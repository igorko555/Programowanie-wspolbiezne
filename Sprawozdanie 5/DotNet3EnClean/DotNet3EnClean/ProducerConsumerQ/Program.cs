using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumerQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var cq = new Queue<int>();
            List<Thread> threads = new List<Thread>();
            Random rand = new Random();
            //the producer puts 100 elements into the collection at random intervals of up to 500ms
            var producer = new Thread(() =>
            {
                for (int l = 0; l < 100; l++)
                {
                    cq.Enqueue(l);
                    Console.WriteLine($"I'm putting  {l}");
                    Thread.Sleep(rand.Next(500));
                }

            });
            producer.Start();

            for (int i = 0; i < 10; i++)
            {
                var thread = new Thread(() =>
                {

                    for (int l = 0; l < 10; l++)
                    {
                        try
                        {
                            int result2 = cq.Dequeue();
                            Console.WriteLine($"I'm getting {result2} ");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception for pick-up: ", ex.Message);
                        }

                        Thread.Sleep(rand.Next(5000));
                    }
                });
                threads.Add(thread);
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            producer.Join(); 
        }
    }
}

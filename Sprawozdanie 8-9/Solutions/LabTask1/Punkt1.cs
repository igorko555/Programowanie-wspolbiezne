using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LabTask1
{
    class Punkt1
    {
        static void DoPointlessMess(int depth)
        {
            double x = 1.0;
            for (int i = 0; i < 50; i++)
            {
                x = Math.Sin(x + depth);
            }
        }

        static void Main(string[] args)
        {
            const int chainLength = 50000;
            Console.WriteLine($"Badanie lancucha kontynuacji TPL ({chainLength} zadan)\n");

            Stopwatch sw = Stopwatch.StartNew();
            Task current = Task.CompletedTask;
            for (int i = 0; i < chainLength; i++)
            {
                int depth = i;
                current = current.ContinueWith(t => DoPointlessMess(depth), TaskContinuationOptions.ExecuteSynchronously);
            }
            current.Wait();
            sw.Stop();
            long timeSync = sw.ElapsedMilliseconds;
            Console.WriteLine($"1. ContinueWith + ExecuteSynchronously | Czas: {timeSync,5} ms");

            sw.Restart();
            current = Task.CompletedTask;
            for (int i = 0; i < chainLength; i++)
            {
                int depth = i;
                current = current.ContinueWith(t => DoPointlessMess(depth));
            }
            current.Wait();
            sw.Stop();
            long timeAsync = sw.ElapsedMilliseconds;
            Console.WriteLine($"2. ContinueWith domyslne (ThreadPool)   | Czas: {timeAsync,5} ms");

            double speedup = (double)timeAsync / Math.Max(1, timeSync);
            Console.WriteLine($"\nPrzyspieszenie dzieki ExecuteSynchronously: {speedup:F2}x");
        }
    }
}

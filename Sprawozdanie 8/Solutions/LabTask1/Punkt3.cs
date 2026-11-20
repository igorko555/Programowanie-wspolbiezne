using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LabTask1
{
    class Punkt3
    {
        static double DoPointlessMess(double aa)
        {
            for (int i = 0; i < 50; i++)
            {
                aa++;
                aa = (Math.Sin(aa) + Math.Cos(aa));
                aa = aa % Math.Sin(aa);
                aa = Math.Log2(Math.Tanh(aa));
                aa *= Math.Sin(aa) / Math.Cos(aa) == 0 ? 1 : Math.Cos(aa);
            }
            return aa;
        }

        static void Main(string[] args)
        {
            const int chainLength = 100000;
            Console.WriteLine($"Przekazywanie wartosci przez ContinueWith ({chainLength} zadan)\n");

            Stopwatch sw = Stopwatch.StartNew();
            Task<double> taskSync = Task.Factory.StartNew(() => DoPointlessMess(1.0));
            for (int i = 0; i < chainLength; i++)
            {
                taskSync = taskSync.ContinueWith(ant => DoPointlessMess(ant.Result), TaskContinuationOptions.ExecuteSynchronously);
            }
            double resultSync = taskSync.Result;
            sw.Stop();
            long timeSync = sw.ElapsedMilliseconds;
            Console.WriteLine($"1. ContinueWith + ExecuteSynchronously: {timeSync} ms | Wynik: {resultSync:F4}");

            sw.Restart();
            Task<double> taskAsync = Task.Factory.StartNew(() => DoPointlessMess(1.0));
            for (int i = 0; i < chainLength; i++)
            {
                taskAsync = taskAsync.ContinueWith(ant => DoPointlessMess(ant.Result));
            }
            double resultAsync = taskAsync.Result;
            sw.Stop();
            long timeAsync = sw.ElapsedMilliseconds;
            Console.WriteLine($"2. ContinueWith domyslne (ThreadPool):  {timeAsync} ms | Wynik: {resultAsync:F4}");
        }
    }
}

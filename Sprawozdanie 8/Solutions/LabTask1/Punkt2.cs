using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LabTask1
{
    class Punkt2
    {
        static void DoPointlessMess()
        {
            double aa = 0;
            for (int i = 0; i < 100; i++)
            {
                aa++;
                aa = (Math.Sin(aa) + Math.Cos(aa));
                aa = aa % Math.Sin(aa);
                aa = Math.Log2(Math.Tanh(aa));
                aa *= Math.Sin(aa) / Math.Cos(aa) == 0 ? 1 : Math.Cos(aa);
            }
        }

        static void Main(string[] args)
        {
            const int chainLength = 20000;
            Console.WriteLine($"Zadanie 2: Obciazenie DoPointlessMess ({chainLength} zadan)\n");

            Stopwatch sw = Stopwatch.StartNew();
            Task taskSync = Task.Factory.StartNew(() => { });
            for (int i = 0; i < chainLength; i++)
            {
                taskSync = taskSync.ContinueWith((ant, a) => DoPointlessMess(), i, TaskContinuationOptions.ExecuteSynchronously);
            }
            taskSync.Wait();
            sw.Stop();
            long timeSync = sw.ElapsedMilliseconds;
            Console.WriteLine($"1. ContinueWith + ExecuteSynchronously: {timeSync} ms");

            sw.Restart();
            Task taskAsync = Task.Factory.StartNew(() => { });
            for (int i = 0; i < chainLength; i++)
            {
                taskAsync = taskAsync.ContinueWith((ant, a) => DoPointlessMess(), i);
            }
            taskAsync.Wait();
            sw.Stop();
            long timeAsync = sw.ElapsedMilliseconds;
            Console.WriteLine($"2. ContinueWith domyslne (ThreadPool):  {timeAsync} ms");

            Console.WriteLine($"\nRoznica czasowa: {Math.Abs(timeAsync - timeSync)} ms");
        }
    }
}

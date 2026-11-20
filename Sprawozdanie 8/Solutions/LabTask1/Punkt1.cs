using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LabTask1
{
    class Punkt1
    {
        static void Main(string[] args)
        {
            const int chainLength = 100000;
            const int runs = 5;

            Console.WriteLine($"Lancuch {chainLength} zadan TPL (srednia z {runs} prob)\n");

            long totalInitSync = 0, totalExecSync = 0;
            for (int r = 0; r < runs; r++)
            {
                Stopwatch swInit = Stopwatch.StartNew();
                Task task = Task.Factory.StartNew(() => { });
                for (int i = 0; i < chainLength; i++)
                {
                    task = task.ContinueWith((ant, a) => { }, i, TaskContinuationOptions.ExecuteSynchronously);
                }
                swInit.Stop();

                Stopwatch swExec = Stopwatch.StartNew();
                task.Wait();
                swExec.Stop();

                totalInitSync += swInit.ElapsedMilliseconds;
                totalExecSync += swExec.ElapsedMilliseconds;
            }

            long avgInitSync = totalInitSync / runs;
            long avgExecSync = totalExecSync / runs;
            Console.WriteLine($"[ExecuteSynchronously] Czas inicjalizacji: {avgInitSync} ms | Czas zakonczenia: {avgExecSync} ms");

            long totalInitAsync = 0, totalExecAsync = 0;
            for (int r = 0; r < runs; r++)
            {
                Stopwatch swInit = Stopwatch.StartNew();
                Task task = Task.Factory.StartNew(() => { });
                for (int i = 0; i < chainLength; i++)
                {
                    task = task.ContinueWith((ant, a) => { }, i);
                }
                swInit.Stop();

                Stopwatch swExec = Stopwatch.StartNew();
                task.Wait();
                swExec.Stop();

                totalInitAsync += swInit.ElapsedMilliseconds;
                totalExecAsync += swExec.ElapsedMilliseconds;
            }

            long avgInitAsync = totalInitAsync / runs;
            long avgExecAsync = totalExecAsync / runs;
            Console.WriteLine($"[Domyslnie / ThreadPool] Czas inicjalizacji: {avgInitAsync} ms | Czas zakonczenia: {avgExecAsync} ms");
        }
    }
}

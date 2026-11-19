using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAdvanced
{
    class Punkt4
    {
        static Task<int[]> calculateSomethingAsync(int size, CancellationToken ct)
        {
            return Task.Factory.StartNew((s) =>
            {
                if (s is null) throw new ArgumentNullException(nameof(s));
                int[] result = new int[(int)s];
                for (int i = 0; i < (int)s; i++)
                {
                    ct.ThrowIfCancellationRequested();
                    Thread.SpinWait(20000000);
                    result[i] = Random.Shared.Next(10);
                }
                return result;
            }, size, ct);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Zadanie 4: Alternatywne galazie ContinueWith (OnFaulted vs OnSuccess)\n");

            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(150);

                Task<int[]> task = calculateSomethingAsync(50, cts.Token);

                Task onFaultTask = task.ContinueWith(t =>
                {
                    Console.WriteLine($"[onFaultTask] Blad lub anulowanie: {t.Exception?.InnerException?.Message ?? "Operacja anulowana."}");
                }, TaskContinuationOptions.NotOnRanToCompletion);

                Task onSuccessTask = task.ContinueWith(t =>
                {
                    Console.WriteLine($"[onSuccessTask] Sukces! Wygenerowano {t.Result.Length} elementow.");
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

                try
                {
                    Task.WaitAll(onFaultTask, onSuccessTask);
                }
                catch (AggregateException)
                {
                    Console.WriteLine("[TPL] Pominieta galaz zostala anulowana przez harmonogram.");
                }

                try
                {
                    task.Wait();
                }
                catch (AggregateException ae)
                {
                    ae.Flatten().Handle(e =>
                    {
                        Console.WriteLine($"[Przechwycono w Main] {e.GetType().Name}: {e.Message}");
                        return true;
                    });
                }
            }

            Console.WriteLine("\nZadanie 4 zakonczone.");
        }
    }
}

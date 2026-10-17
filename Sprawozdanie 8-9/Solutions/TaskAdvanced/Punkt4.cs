using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAdvanced
{
    class Punkt4
    {
        static async Task<int[]> CalculateSomethingAsync(int size, IProgress<(int, int)> progress, CancellationToken ct)
        {
            int[] data = new int[size];
            Random rand = new Random();

            for (int i = 0; i < size; i++)
            {
                ct.ThrowIfCancellationRequested();
                data[i] = rand.Next(100);

                if (i % (size / 10) == 0)
                {
                    progress?.Report((i, data[i]));
                }

                await Task.Delay(20, ct);
            }

            return data;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Zaawansowane mechanizmy TPL: Raportowanie, Anulowanie i Rozgalezienia\n");

            var progress = new Progress<(int step, int val)>(p =>
            {
                Console.WriteLine($"[Raport postepu] Krok: {p.step,4} | Wygenerowano: {p.val}");
            });

            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(150);

                Task<int[]> task = CalculateSomethingAsync(50, progress, cts.Token);

                Task onFault = task.ContinueWith(t =>
                {
                    Console.WriteLine("[Rozgalezienie OnFaulted] Operacja zostala anulowana lub zakonczona wyjatkiem.");
                }, TaskContinuationOptions.OnlyOnFaulted);

                Task onSuccess = task.ContinueWith(t =>
                {
                    Console.WriteLine($"[Rozgalezienie OnSuccess] Obliczenia zakonczone sukcesem. Elementow: {t.Result.Length}");
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

                try
                {
                    task.Wait();
                }
                catch (AggregateException ae)
                {
                    ae.Flatten().Handle(ex =>
                    {
                        Console.WriteLine($"[Przechwycono wyjatek] {ex.GetType().Name}: {ex.Message}");
                        return true;
                    });
                }

                try
                {
                    Task.WaitAll(onFault, onSuccess);
                }
                catch (AggregateException)
                {
                    Console.WriteLine("[Informacja TPL] Pominiete rozgalezienie zostalo anulowane przez harmonogram.");
                }
            }

            Console.WriteLine("\nZadanie Punkt 4 zakonczone sukcesem.");
        }
    }
}

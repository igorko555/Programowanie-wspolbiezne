using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAdvanced
{
    class Punkt3
    {
        static Task<int[]> calculateSomethingAsync(int size, IProgress<(int, int)> progress, CancellationToken ct)
        {
            return Task.Factory.StartNew((s) =>
            {
                if (s is null) throw new ArgumentNullException(nameof(s));
                int[] result = new int[(int)s];
                for (int i = 0; i < (int)s; i++)
                {
                    if (ct.IsCancellationRequested)
                    {
                        throw new OperationCanceledException("Nastapilo zadanie anulowania", ct);
                    }

                    Thread.SpinWait(20000000);
                    result[i] = Random.Shared.Next(10);
                    progress?.Report((i, result[i]));
                }
                return result;
            }, size, ct);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Zadanie 3: Kooperatywne anulowanie zadania za pomoca CancellationToken\n");

            var progress = new Progress<(int, int)>();
            progress.ProgressChanged += (sender, data) =>
            {
                Console.WriteLine($"[Krok {data.Item1}] Wygenerowano: {data.Item2}");
            };

            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(150);

                Task<int[]> task = calculateSomethingAsync(50, progress, cts.Token);

                try
                {
                    task.Wait();
                    Console.WriteLine("Zadanie zakonczylo sie przed anulowaniem.");
                }
                catch (AggregateException ae)
                {
                    ae.Flatten().Handle(ex =>
                    {
                        Console.WriteLine($"[Przechwycono wyjatek] {ex.GetType().Name}: {ex.Message}");
                        return true;
                    });
                }
            }

            Console.WriteLine("\nZadanie 3 zakonczone.");
        }
    }
}

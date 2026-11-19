using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAdvanced
{
    class Punkt2
    {
        static Task<int[]> calculateSomethingAsync(int size, IProgress<(int, int)> progress)
        {
            return Task.Factory.StartNew((s) =>
            {
                if (s is null) throw new ArgumentNullException(nameof(s));
                int[] result = new int[(int)s];
                for (int i = 0; i < (int)s; i++)
                {
                    Thread.SpinWait(10000000);
                    result[i] = Random.Shared.Next(10);
                    progress?.Report((i, result[i]));
                }
                return result;
            }, size);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Zadanie 2: Raportowanie postepu z uzyciem IProgress<(int,int)>\n");

            var progress = new Progress<(int, int)>();
            progress.ProgressChanged += (sender, data) =>
            {
                Console.WriteLine($"[ProgressChanged] Iteracja: {data.Item1,2} | Wylosowana wartosc: {data.Item2}");
            };

            Task<int[]> task = calculateSomethingAsync(10, progress);
            task.Wait();

            Console.WriteLine($"\nZadanie zakonczone pomyslnie. Wygenerowano {task.Result.Length} liczb.");
        }
    }
}

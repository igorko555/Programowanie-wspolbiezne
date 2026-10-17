using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BenchmarkSum
{
    class Punkt5
    {
        static double CalculatedValue(double x)
        {
            return Math.Sqrt(Math.Sin(x) * Math.Sin(x) + Math.Cos(x) * Math.Cos(x));
        }

        static void Main(string[] args)
        {
            const int arraySize = 5000000;
            Console.WriteLine($"Wielki Benchmark Sumowania {arraySize} elementow\n");

            double[] data = new double[arraySize];
            for (int i = 0; i < arraySize; i++) data[i] = 1.0;

            Stopwatch sw = Stopwatch.StartNew();
            double seqSum = 0;
            for (int i = 0; i < arraySize; i++)
            {
                seqSum += CalculatedValue(data[i]);
            }
            sw.Stop();
            Console.WriteLine($"1. Sekwencyjna petla for      | Suma: {seqSum:F0} | Czas: {sw.ElapsedMilliseconds,5} ms");

            object locker = new object();
            double parallelLockSum = 0;
            sw.Restart();
            Parallel.ForEach(data, item =>
            {
                double v = CalculatedValue(item);
                lock (locker)
                {
                    parallelLockSum += v;
                }
            });
            sw.Stop();
            Console.WriteLine($"2. Parallel.ForEach z lock    | Suma: {parallelLockSum:F0} | Czas: {sw.ElapsedMilliseconds,5} ms");

            sw.Restart();
            double plinqSum = data.AsParallel().Sum(x => CalculatedValue(x));
            sw.Stop();
            Console.WriteLine($"3. PLINQ AsParallel().Sum()   | Suma: {plinqSum:F0} | Czas: {sw.ElapsedMilliseconds,5} ms");

            int cores = Environment.ProcessorCount;
            int chunkSize = arraySize / cores;
            Task<double>[] tasks = new Task<double>[cores];

            sw.Restart();
            for (int c = 0; c < cores; c++)
            {
                int start = c * chunkSize;
                int end = (c == cores - 1) ? arraySize : start + chunkSize;
                tasks[c] = Task.Run(() =>
                {
                    double localSum = 0;
                    for (int i = start; i < end; i++)
                    {
                        localSum += CalculatedValue(data[i]);
                    }
                    return localSum;
                });
            }
            Task.WaitAll(tasks);
            double tplSum = tasks.Sum(t => t.Result);
            sw.Stop();
            Console.WriteLine($"4. Dedykowane partycje TPL    | Suma: {tplSum:F0} | Czas: {sw.ElapsedMilliseconds,5} ms");

            Console.WriteLine("\nBenchmark zakonczony.");
        }
    }
}

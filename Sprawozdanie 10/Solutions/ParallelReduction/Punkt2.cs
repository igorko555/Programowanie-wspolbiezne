using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelReduction
{
    class Punkt2
    {
        static void Main(string[] args)
        {
            const int arraySize = 10000000;
            Console.WriteLine($"Optymalizacja redukcji wielowatkowej Parallel.For<TLocal> ({arraySize} liczb)\n");

            double[] data = new double[arraySize];
            for (int i = 0; i < arraySize; i++) data[i] = 1.0;

            Stopwatch sw = Stopwatch.StartNew();
            double seqSum = 0;
            for (int i = 0; i < arraySize; i++)
            {
                seqSum += data[i];
            }
            sw.Stop();
            Console.WriteLine($"1. Sekwencyjna petla for         | Suma: {seqSum:F0} | Czas: {sw.ElapsedMilliseconds,5} ms");

            object lockObj = new object();
            double naiveSum = 0;
            sw.Restart();
            Parallel.For(0, Math.Min(1000000, arraySize), i =>
            {
                lock (lockObj)
                {
                    naiveSum += data[i];
                }
            });
            sw.Stop();
            Console.WriteLine($"2. Parallel.For z lock w srodku  | Suma (skala 1M): {naiveSum:F0} | Czas: {sw.ElapsedMilliseconds,5} ms");

            double optimizedSum = 0;
            sw.Restart();
            Parallel.For<double>(0, arraySize,
                () => 0.0,
                (i, state, localState) => localState + data[i],
                localState =>
                {
                    lock (lockObj)
                    {
                        optimizedSum += localState;
                    }
                });
            sw.Stop();
            Console.WriteLine($"3. Parallel.For<TLocal> (optym.) | Suma: {optimizedSum:F0} | Czas: {sw.ElapsedMilliseconds,5} ms");
            Console.WriteLine("\nZadanie Punkt 2 zakonczone.");
        }
    }
}

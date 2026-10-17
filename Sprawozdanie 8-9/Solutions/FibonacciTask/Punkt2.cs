using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace FibonacciTask
{
    class Punkt2
    {
        static void Main(string[] args)
        {
            const int n = 5000;
            Console.WriteLine($"Potokowe obliczanie ciagu Fibonacciego do wyrazu {n}\n");

            Stopwatch sw = Stopwatch.StartNew();
            Task<(BigInteger a, BigInteger b)> pipeline = Task.FromResult((BigInteger.Zero, BigInteger.One));

            for (int i = 2; i <= n; i++)
            {
                pipeline = pipeline.ContinueWith(prev =>
                {
                    var (prevA, prevB) = prev.Result;
                    return (prevB, prevA + prevB);
                }, TaskContinuationOptions.ExecuteSynchronously);
            }

            var result = pipeline.Result;
            sw.Stop();

            Console.WriteLine($"TPL Pipeline | F({n}) liczba cyfr: {result.b.ToString().Length} | Czas: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            BigInteger a = 0;
            BigInteger b = 1;
            for (int i = 2; i <= n; i++)
            {
                BigInteger c = a + b;
                a = b;
                b = c;
            }
            sw.Stop();
            Console.WriteLine($"Sekwencyjnie | F({n}) liczba cyfr: {b.ToString().Length} | Czas: {sw.ElapsedMilliseconds} ms");
        }
    }
}

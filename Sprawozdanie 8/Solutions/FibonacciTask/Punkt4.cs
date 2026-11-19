using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace FibonacciTask
{
    class Punkt4
    {
        static Tuple<BigInteger, BigInteger> BigFibonacci(BigInteger n2, BigInteger n1)
        {
            return Tuple.Create(n1, n1 + n2);
        }

        static void Main(string[] args)
        {
            const int count = 20000;
            Console.WriteLine($"Zadanie 4: Ciag Fibonacciego do wyrazu {count}\n");

            Stopwatch sw = Stopwatch.StartNew();
            Task<Tuple<BigInteger, BigInteger>> pipeline = Task.FromResult(Tuple.Create(BigInteger.Zero, BigInteger.One));

            for (int i = 2; i <= count; i++)
            {
                pipeline = pipeline.ContinueWith(prev =>
                    BigFibonacci(prev.Result.Item1, prev.Result.Item2),
                    TaskContinuationOptions.ExecuteSynchronously);
            }

            var result = pipeline.Result;
            sw.Stop();
            Console.WriteLine($"1. TPL Pipeline | F({count}) liczba cyfr: {result.Item2.ToString().Length} | Czas: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            BigInteger n2 = 0;
            BigInteger n1 = 1;
            for (int i = 2; i <= count; i++)
            {
                Tuple<BigInteger, BigInteger> res = BigFibonacci(n2, n1);
                n2 = res.Item1;
                n1 = res.Item2;
            }
            sw.Stop();
            Console.WriteLine($"2. Sekwencyjnie | F({count}) liczba cyfr: {n1.ToString().Length} | Czas: {sw.ElapsedMilliseconds} ms");
        }
    }
}

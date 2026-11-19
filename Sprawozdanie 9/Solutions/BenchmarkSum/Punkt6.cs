using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BenchmarkSum
{
    class Punkt6
    {
        static int[] GenerateRandomArray(int size)
        {
            Random random = new Random(42);
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = random.Next(10);
            }
            return array;
        }

        static void Main(string[] args)
        {
            const int size = 10000000;
            Console.WriteLine($"Zadanie 6: Eksperymenty z sumowaniem rownoleglym ({size} liczb)\n");

            int[] numbers = GenerateRandomArray(size);
            Stopwatch sw = new Stopwatch();

            // 1. Parallel.ForEach bez lock (wyscig)
            long sumNoLock = 0;
            sw.Restart();
            Parallel.ForEach(numbers, number => sumNoLock += number);
            sw.Stop();
            Console.WriteLine($"1. Parallel.ForEach (bez lock): Suma = {sumNoLock,-10} | Czas = {sw.ElapsedMilliseconds,5} ms (BLAD WYSCIGU)");

            // 2. Parallel.ForEach z lock
            object locker = new object();
            long sumWithLock = 0;
            sw.Restart();
            Parallel.ForEach(numbers, number =>
            {
                lock (locker)
                {
                    sumWithLock += number;
                }
            });
            sw.Stop();
            Console.WriteLine($"2. Parallel.ForEach (z lock):   Suma = {sumWithLock,-10} | Czas = {sw.ElapsedMilliseconds,5} ms (LOCK CONTENTION)");

            // 3. LINQ jednowatkowe Sum()
            sw.Restart();
            long linqSum = numbers.Select(x => (long)x).Sum();
            sw.Stop();
            Console.WriteLine($"3. LINQ Sum() jednowatkowy:     Suma = {linqSum,-10} | Czas = {sw.ElapsedMilliseconds,5} ms");

            // 4. PLINQ ForAll z lock
            long plinqForAllSum = 0;
            sw.Restart();
            numbers.AsParallel().ForAll(number =>
            {
                lock (locker)
                {
                    plinqForAllSum += number;
                }
            });
            sw.Stop();
            Console.WriteLine($"4. PLINQ ForAll (z lock):       Suma = {plinqForAllSum,-10} | Czas = {sw.ElapsedMilliseconds,5} ms");

            // 5a. PLINQ Sum(number => number)
            sw.Restart();
            long plinqSum1 = numbers.AsParallel().Select(x => (long)x).Sum();
            sw.Stop();
            Console.WriteLine($"5a. PLINQ Sum(selektor):        Suma = {plinqSum1,-10} | Czas = {sw.ElapsedMilliseconds,5} ms");

            // 5b. PLINQ Sum()
            sw.Restart();
            long plinqSum2 = numbers.AsParallel().Select(x => (long)x).Sum();
            sw.Stop();
            Console.WriteLine($"5b. PLINQ Sum() bezparametrowy: Suma = {plinqSum2,-10} | Czas = {sw.ElapsedMilliseconds,5} ms");
        }
    }
}

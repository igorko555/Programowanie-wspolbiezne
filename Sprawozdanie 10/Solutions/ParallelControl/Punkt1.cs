using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelControl
{
    class Punkt1
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Badanie sterowania petla rownolegla: Break() vs Stop()\n");

            int targetIndex = 42;
            Console.WriteLine($"1. Test z uzyciem ParallelLoopState.Break() (szukany indeks: {targetIndex})");
            ParallelLoopResult breakResult = Parallel.For(0, 100, (i, state) =>
            {
                if (i == targetIndex)
                {
                    state.Break();
                    Console.WriteLine($"[Watek {Thread.CurrentThread.ManagedThreadId}] Wywolano Break() w iteracji {i}");
                }

                if (state.LowestBreakIteration.HasValue && i > state.LowestBreakIteration.Value)
                {
                    return;
                }

                Thread.Sleep(5);
            });

            Console.WriteLine($"Wynik Break(): IsCompleted={breakResult.IsCompleted}, LowestBreakIteration={breakResult.LowestBreakIteration}\n");

            Console.WriteLine("2. Test z uzyciem ParallelLoopState.Stop()");
            ParallelLoopResult stopResult = Parallel.For(0, 100, (i, state) =>
            {
                if (i == targetIndex)
                {
                    state.Stop();
                    Console.WriteLine($"[Watek {Thread.CurrentThread.ManagedThreadId}] Wywolano Stop() w iteracji {i}");
                }

                if (state.ShouldExitCurrentIteration)
                {
                    return;
                }

                Thread.Sleep(5);
            });

            Console.WriteLine($"Wynik Stop(): IsCompleted={stopResult.IsCompleted}, LowestBreakIteration={stopResult.LowestBreakIteration}");
            Console.WriteLine("\nZadanie Punkt 1 zakonczone.");
        }
    }
}

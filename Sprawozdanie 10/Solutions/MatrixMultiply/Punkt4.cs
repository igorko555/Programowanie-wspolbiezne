using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MatrixMultiply
{
    class Punkt4
    {
        static double[,] MultiplySequential(double[,] A, double[,] B, int N)
        {
            double[,] C = new double[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < N; k++)
                    {
                        sum += A[i, k] * B[k, j];
                    }
                    C[i, j] = sum;
                }
            }
            return C;
        }

        static double[,] MultiplyParallel(double[,] A, double[,] B, int N)
        {
            double[,] C = new double[N, N];
            Parallel.For(0, N, i =>
            {
                for (int j = 0; j < N; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < N; k++)
                    {
                        sum += A[i, k] * B[k, j];
                    }
                    C[i, j] = sum;
                }
            });
            return C;
        }

        static void Main(string[] args)
        {
            const int N = 500;
            Console.WriteLine($"Mnozenie macierzy {N}x{N} (sekwencyjne vs rownolegle)\n");

            double[,] A = new double[N, N];
            double[,] B = new double[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    A[i, j] = 1.0;
                    B[i, j] = 2.0;
                }
            }

            Stopwatch sw = Stopwatch.StartNew();
            double[,] C1 = MultiplySequential(A, B, N);
            sw.Stop();
            Console.WriteLine($"1. Mnozenie sekwencyjne | Czas: {sw.ElapsedMilliseconds,5} ms | C[0,0] = {C1[0, 0]}");

            sw.Restart();
            double[,] C2 = MultiplyParallel(A, B, N);
            sw.Stop();
            Console.WriteLine($"2. Mnozenie rownolegle  | Czas: {sw.ElapsedMilliseconds,5} ms | C[0,0] = {C2[0, 0]}");

            bool match = true;
            for (int i = 0; i < N && match; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (Math.Abs(C1[i, j] - C2[i, j]) > 1e-6)
                    {
                        match = false;
                        break;
                    }
                }
            }
            Console.WriteLine($"Zgodnosc wynikow: {match}");
            Console.WriteLine("\nZadanie Punkt 4 zakonczone.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Metaheuristics
{
    class Punkt5
    {
        static readonly int numCities = 20;
        static readonly int numAnts = 30;
        static readonly int iterations = 50;
        static readonly double alpha = 1.0;
        static readonly double beta = 2.0;
        static readonly double rho = 0.5;

        static double[,] dist = new double[numCities, numCities];
        static double[,] pheromone = new double[numCities, numCities];

        static void InitProblem()
        {
            Random r = new Random(42);
            double[] x = new double[numCities];
            double[] y = new double[numCities];
            for (int i = 0; i < numCities; i++)
            {
                x[i] = r.NextDouble() * 100;
                y[i] = r.NextDouble() * 100;
            }

            for (int i = 0; i < numCities; i++)
            {
                for (int j = 0; j < numCities; j++)
                {
                    dist[i, j] = Math.Sqrt(Math.Pow(x[i] - x[j], 2) + Math.Pow(y[i] - y[j], 2));
                    pheromone[i, j] = 1.0;
                }
            }
        }

        static (int[] tour, double length) BuildAntTour()
        {
            bool[] visited = new bool[numCities];
            int[] tour = new int[numCities];
            int current = Random.Shared.Next(numCities);
            tour[0] = current;
            visited[current] = true;

            for (int step = 1; step < numCities; step++)
            {
                double[] probs = new double[numCities];
                double sum = 0;

                for (int next = 0; next < numCities; next++)
                {
                    if (!visited[next])
                    {
                        double eta = 1.0 / Math.Max(0.1, dist[current, next]);
                        probs[next] = Math.Pow(pheromone[current, next], alpha) * Math.Pow(eta, beta);
                        sum += probs[next];
                    }
                }

                double randVal = Random.Shared.NextDouble() * sum;
                double acc = 0;
                int chosen = -1;
                for (int next = 0; next < numCities; next++)
                {
                    if (!visited[next])
                    {
                        acc += probs[next];
                        if (acc >= randVal)
                        {
                            chosen = next;
                            break;
                        }
                    }
                }

                if (chosen == -1)
                {
                    for (int next = 0; next < numCities; next++)
                    {
                        if (!visited[next]) { chosen = next; break; }
                    }
                }

                tour[step] = chosen;
                visited[chosen] = true;
                current = chosen;
            }

            double len = 0;
            for (int i = 0; i < numCities - 1; i++) len += dist[tour[i], tour[i + 1]];
            len += dist[tour[numCities - 1], tour[0]];

            return (tour, len);
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Rownolegly Algorytm Mrowkowy (ACO) dla problemu TSP ({numCities} miast, {numAnts} mrowek)\n");

            InitProblem();
            double bestLength = double.MaxValue;
            object lockObj = new object();

            Stopwatch sw = Stopwatch.StartNew();

            for (int iter = 0; iter < iterations; iter++)
            {
                (int[] tour, double length)[] antResults = new (int[], double)[numAnts];

                Parallel.For(0, numAnts, ant =>
                {
                    antResults[ant] = BuildAntTour();
                });

                for (int i = 0; i < numCities; i++)
                {
                    for (int j = 0; j < numCities; j++)
                    {
                        pheromone[i, j] *= (1.0 - rho);
                    }
                }

                foreach (var ant in antResults)
                {
                    double dTau = 100.0 / ant.length;
                    for (int step = 0; step < numCities - 1; step++)
                    {
                        int u = ant.tour[step];
                        int v = ant.tour[step + 1];
                        pheromone[u, v] += dTau;
                        pheromone[v, u] += dTau;
                    }

                    if (ant.length < bestLength)
                    {
                        bestLength = ant.length;
                    }
                }

                if (iter % 10 == 0 || iter == iterations - 1)
                {
                    Console.WriteLine($"Generacja {iter,2} | Najlepsza trasa: {bestLength:F2}");
                }
            }

            sw.Stop();
            Console.WriteLine($"\nOptymalizacja ACO zakonczona w czasie: {sw.ElapsedMilliseconds} ms");
            Console.WriteLine("\nZadanie Punkt 5 zakonczone.");
        }
    }
}

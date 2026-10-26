using Barriers;
using System;
using System.Threading;

namespace Monitory
{
    public class MonitorBarrier
    {
        private readonly int totalThreads;
        private int arrivedThreads = 0;
        private int phase = 0;
        private readonly object lockObj = new object();

        public MonitorBarrier(int threads)
        {
            totalThreads = threads;
        }

        public void SignalAndWait()
        {
            lock (lockObj)
            {
                int currentPhase = phase;
                arrivedThreads++;
                if (arrivedThreads == totalThreads)
                {
                    arrivedThreads = 0;
                    phase++;
                    Monitor.PulseAll(lockObj);
                }
                else
                {
                    while (currentPhase == phase)
                    {
                        Monitor.Wait(lockObj);
                    }
                }
            }
        }
    }

    class Program
    {
        const int howmanyspecimens = 16;
        const int howmanyiterations = 100;
        static double[] specimens = new double[howmanyspecimens];
        static double[] evaluatedValues = new double[howmanyspecimens];

        static MonitorBarrier evalBarrier = new MonitorBarrier(howmanyspecimens + 1);
        static MonitorBarrier genBarrier = new MonitorBarrier(howmanyspecimens + 1);

        static void Main(string[] args)
        {
            GAHelper.FillFirstGeneration(specimens);

            Thread[] workers = new Thread[howmanyspecimens];
            for (int i = 0; i < howmanyspecimens; i++)
            {
                int id = i;
                workers[i] = new Thread(() =>
                {
                    for (int iter = 0; iter < howmanyiterations; iter++)
                    {
                        evaluatedValues[id] = GAHelper.Evaluate(specimens[id]);
                        evalBarrier.SignalAndWait();
                        genBarrier.SignalAndWait();
                    }
                });
                workers[i].Start();
            }

            for (int iter = 0; iter < howmanyiterations; iter++)
            {
                evalBarrier.SignalAndWait();
                if (iter % 20 == 0 || iter == howmanyiterations - 1)
                {
                    Console.WriteLine($"[MonitorBarrier] Pokolenie {iter,3} | Najlepszy: {evaluatedValues[0]:F4}");
                }
                specimens = GAHelper.NewGeneration(specimens, evaluatedValues);
                GAHelper.Mutation(specimens);
                genBarrier.SignalAndWait();
            }

            for (int i = 0; i < howmanyspecimens; i++)
            {
                workers[i].Join();
            }

            Console.WriteLine("\nEwolucja zakonczona sukcesem za pomoca wlasnej klasy MonitorBarrier (Monitor.Wait / PulseAll).");
        }
    }
}


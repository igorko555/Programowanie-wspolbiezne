using System;
using System.Threading;

namespace Barriers
{
    class Punkt1
    {
        const int howmanyspecimens = 16;
        const int howmanyiterations = 100;
        static double[] specimens = new double[howmanyspecimens];
        static double[] evaluatedValues = new double[howmanyspecimens];

        static Barrier evaluationBarrier = new Barrier(howmanyspecimens + 1);
        static Barrier generationBarrier = new Barrier(howmanyspecimens + 1);

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
                        evaluationBarrier.SignalAndWait();
                        generationBarrier.SignalAndWait();
                    }
                });
                workers[i].Start();
            }

            for (int iter = 0; iter < howmanyiterations; iter++)
            {
                evaluationBarrier.SignalAndWait();
                if (iter % 20 == 0 || iter == howmanyiterations - 1)
                {
                    Console.WriteLine($"Pokolenie {iter,3} | Najlepszy: {evaluatedValues[0]:F4}");
                }
                specimens = GAHelper.NewGeneration(specimens, evaluatedValues);
                GAHelper.Mutation(specimens);
                generationBarrier.SignalAndWait();
            }

            for (int i = 0; i < howmanyspecimens; i++)
            {
                workers[i].Join();
            }

            Console.WriteLine("\nEwolucja zakonczona sukcesem za pomoca System.Threading.Barrier.");
        }
    }
}

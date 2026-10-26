using System;
using System.Linq;



namespace Barriers
{
    class Program
    {
        const int howmanyspecimens = 16;
        const int howmanyiterations = 2000;
        static double[] specimens = new double[howmanyspecimens];
        static double[] evaluetedValues = new double[howmanyspecimens];
        static void Main(string[] args)
        {
            Console.WindowWidth = 150;
            GAHelper.FillFirstGeneration(specimens);

            for (int i = 0; i < howmanyiterations; i++)
            {
                evaluetedValues = GAHelper.EvaluateValues(specimens); //evaluate generation 
                //specimens.ToList().ForEach(s => GAHelper.Display(s));
              //  Console.Write($"Max: {evaluetedValues.Max()} Avg: {evaluetedValues.Average()}   ");
                GAHelper.PrintGenerationWithValues(specimens, evaluetedValues);
                GAHelper.AppendToFile("results.csv", evaluetedValues);
                specimens = GAHelper.NewGeneration(specimens, evaluetedValues);
                GAHelper.Mutation(specimens);
              
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Barriers
{
    public static class GAHelper
    {
        
        public static void FillFirstGeneration(double[] specimens)
        {
            Random rand = new Random(Thread.CurrentThread.ManagedThreadId * (int)DateTime.Now.Ticks);
            for (int i = 0; i < specimens.Length; i++)
            {
                byte[] b = new byte[8];
                rand.NextBytes(b);
                specimens[i] = BitConverter.ToDouble(b, 0);
            }
        }
        public static void CrossBytes(ref double B1, ref double B2)
        {
            Random rand = new Random(Thread.CurrentThread.ManagedThreadId * (int)DateTime.Now.Ticks);
            CrossBytes(ref B1, ref B2, rand.Next(1, 63));
        }

        
        public static void CrossBytes(ref double D1, ref double D2, int locus)
        {
            if (locus > 63 || locus < 1) throw new ArgumentOutOfRangeException("Locus should be betwean 1 and 63");
            byte[] B1Temp = BitConverter.GetBytes(D1);
            byte[] B2Temp = BitConverter.GetBytes(D2);
            ulong U1 = BitConverter.ToUInt64(B1Temp);
            ulong U2 = BitConverter.ToUInt64(B2Temp);
            ulong rightMask = (ulong)(((long)1 << locus) - 1);
            ulong leftMask = rightMask ^ 0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111;
            D1 = BitConverter.ToDouble(BitConverter.GetBytes((U1 & rightMask) | (U2 & leftMask)), 0);
            D2 = BitConverter.ToDouble(BitConverter.GetBytes((U1 & leftMask) | (U2 & rightMask)), 0);
        }

        public static double Evaluate(double x)
        {
            if (x > 0) return 1000*Math.Sin(Math.Pow(x,2)) / x;
            return 0;
        }

        public static double[] EvaluateValues(double[] values)
        {
            List<double> ret = new List<double>();
            values.ToList().ForEach(v => ret.Add(Evaluate(v)));
            return ret.ToArray();
        }


        public static double GetRandomSpecimen(double[] parameters, double[] values)
        {
            List<Tuple<double, double>> parval = new List<Tuple<double, double>>(parameters.Length);
            for (int i = 0; i < parameters.Length; i++)
            {
                parval.Add(new Tuple<double, double>(parameters[i], values[i]));
            }
            var sortedValues = parval.OrderBy(t => t.Item2); //sort by values
            double sum = (values.Length+1)*values.Length / 2;
            Random rand = new Random(Thread.CurrentThread.ManagedThreadId * (int)DateTime.Now.Ticks);
            int randomValue = rand.Next(0, (int)sum);
            int index = (int)((Math.Sqrt(randomValue * 8 + 1) + 1) / 2)-1; 
            return  sortedValues.ElementAt(index).Item1;
        }


        public static double[] NewGeneration(double[] parameters, double[] values)
        {
            double[] newGen = new double[parameters.Length];
            for (int i = 0; i < parameters.Length; i += 2)
            {
                double A = GetRandomSpecimen(parameters, values);
                double B = GetRandomSpecimen(parameters, values);
                CrossBytes(ref A, ref B);
                newGen[i] = A;
                newGen[i + 1] = B;
            }
            return newGen;
        }

        public static void Mutation(double[] spec)
        {
            Random rand = new Random(Thread.CurrentThread.ManagedThreadId * (int)DateTime.Now.Ticks);
            foreach (var item in spec)
            {
                ulong mut = (ulong)(1 << rand.Next(0, 64));
                ulong mut2 = (ulong)(1 << rand.Next(0, 64));
                ulong mut3 = (ulong)(1 << rand.Next(0, 64));
                int index = rand.Next(0, spec.Length - 1);
                byte[] bytes = BitConverter.GetBytes(spec[index]);
                ulong specbyte = BitConverter.ToUInt64(BitConverter.GetBytes(spec[index]));
                specbyte ^= mut; //binary xor 
                specbyte ^= mut2; //binary xor //we do i twice 
                specbyte ^= mut3; //binary xor //we do even more
                spec[index] = BitConverter.ToDouble(BitConverter.GetBytes(specbyte));
            }
            
        }

        public static void Display(long x)
        {
            Console.WriteLine($"{Convert.ToString(x, toBase: 2).PadLeft(64, '0')}");
        }

        public static void Display(double x)
        {
            Display((long)BitConverter.ToUInt64(BitConverter.GetBytes(x)));
        }

        public static void PrintGenerationWithValues(double[] bytes, double[] values)
        {
            Console.WriteLine($"  Max: {values.Max()} Avg: {values.Average()}   ");
        }

        public static void AppendToFile<T>(string filename,T[] values)
        {
            string[] lines = new string[1];
            StringBuilder sb = new StringBuilder();
            sb.AppendJoin(';', values);
            lines[0] = sb.ToString();
            File.AppendAllLines(filename, lines);
        }
     }
}

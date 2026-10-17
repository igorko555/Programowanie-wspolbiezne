using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelSort
{
    class Punkt3
    {
        const int SequentialThreshold = 4096;

        static void SequentialMergeSort(int[] array, int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;
                SequentialMergeSort(array, left, mid);
                SequentialMergeSort(array, mid + 1, right);
                Merge(array, left, mid, right);
            }
        }

        static void ParallelMergeSort(int[] array, int left, int right)
        {
            if (right - left <= SequentialThreshold)
            {
                SequentialMergeSort(array, left, right);
                return;
            }

            int mid = (left + right) / 2;
            Task leftTask = Task.Run(() => ParallelMergeSort(array, left, mid));
            ParallelMergeSort(array, mid + 1, right);
            leftTask.Wait();

            Merge(array, left, mid, right);
        }

        static void Merge(int[] array, int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;
            int[] leftArr = new int[n1];
            int[] rightArr = new int[n2];

            Array.Copy(array, left, leftArr, 0, n1);
            Array.Copy(array, mid + 1, rightArr, 0, n2);

            int i = 0, j = 0, k = left;
            while (i < n1 && j < n2)
            {
                if (leftArr[i] <= rightArr[j]) array[k++] = leftArr[i++];
                else array[k++] = rightArr[j++];
            }
            while (i < n1) array[k++] = leftArr[i++];
            while (j < n2) array[k++] = rightArr[j++];
        }

        static void Main(string[] args)
        {
            const int count = 1000000;
            Console.WriteLine($"Wspolbiezne sortowanie przez scalanie (MergeSort) dla {count} elementow\n");

            int[] original = new int[count];
            Random rand = new Random(12345);
            for (int i = 0; i < count; i++) original[i] = rand.Next();

            int[] arrSeq = (int[])original.Clone();
            Stopwatch sw = Stopwatch.StartNew();
            SequentialMergeSort(arrSeq, 0, count - 1);
            sw.Stop();
            Console.WriteLine($"1. Sekwencyjny MergeSort | Czas: {sw.ElapsedMilliseconds,5} ms");

            int[] arrPar = (int[])original.Clone();
            sw.Restart();
            ParallelMergeSort(arrPar, 0, count - 1);
            sw.Stop();
            Console.WriteLine($"2. Rownolegly MergeSort  | Czas: {sw.ElapsedMilliseconds,5} ms");

            bool isSorted = true;
            for (int i = 1; i < count; i++)
            {
                if (arrPar[i] < arrPar[i - 1])
                {
                    isSorted = false;
                    break;
                }
            }
            Console.WriteLine($"Poprawnosc sortowania: {isSorted}");
            Console.WriteLine("\nZadanie Punkt 3 zakonczone.");
        }
    }
}

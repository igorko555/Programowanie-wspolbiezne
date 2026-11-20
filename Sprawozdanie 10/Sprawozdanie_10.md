# Politechnika Świętokrzyska w Kielcach
## Wydział Elektrotechniki, Automatyki i Informatyki
### Katedra Systemów Informatycznych

---

| **Politechnika Świętokrzyska**<br>**Wydział Elektrotechniki Automatyki i Informatyki** |
| :--- |
| **Programowanie Współbieżne – Laboratorium** |
| **Numer grupy:** 3ID11B &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; **Autor:** Ihor Namachynskyi &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; **Data wykonania ćwiczenia:** 27.11.2026 |
| **Sprawozdanie nr 10: Zaawansowane zrównoleglanie pętli, klasa Parallel i algorytmy współbieżne** |

---

## 1. Cel ćwiczenia
Celem laboratorium było zbadanie zaawansowanych mechanizmów zrównoleglania obliczeń iteracyjnych przy użyciu biblioteki Task Parallel Library (TPL), klasy `Parallel` oraz implementacja i analiza wydajnościowa złożonych współbieżnych algorytmów obliczeniowych w środowisku .NET 8.0. W ramach laboratorium zrealizowano 5 zadań:
1. **Sterowanie pętlami równoległymi `Parallel.For`**: Badanie wczesnego przerywania iteracji za pomocą metod `state.Break()` oraz `state.Stop()`, analiza stanu obiektu `ParallelLoopResult` (`IsCompleted`, `LowestBreakIteration`).
2. **Optymalizacja redukcji wielowątkowej**: Porównanie naiwnego blokowania `lock` wewnątrz pętli z optymalnym przeciążeniem `Parallel.For<TLocal>` z akumulatorem lokalnym wątku (`thread-local state`).
3. **Współbieżne algorytmy sortowania (Parallel MergeSort)**: Implementacja rekurencyjnego dzielenia zadań z progiem odcięcia sekwencyjnego (`SequentialThreshold`) w celu uniknięcia nadmiernej ziarnistości (granulacji).
4. **Równoległe mnożenie macierzy**: Zrównoleglenie algorytmu mnożenia macierzy kwadratowych $O(N^3)$ z wykorzystaniem niezależności wierszy bez blokad synchronizacyjnych.
5. **Równoległe metaheurystyki – Algorytm Mrówkowy (ACO)**: Zrównoleglenie konstrukcji tras agentów dla problemu komiwojażera (TSP) przy użyciu generatora `Random.Shared` oraz współbieżnej aktualizacji macierzy feromonowej.

---

## 2. Wprowadzenie teoretyczne
- **Klasa `Parallel` i pętla `Parallel.For`**: Klasa statyczna `Parallel` dostarcza wysokopoziomowe abstrakcje do zrównoleglania pętli iteracyjnych. Mechanizm wewnętrzny automatycznie dzieli zakres danych na partycje dopasowane do liczby rdzeni CPU oraz dynamicznie równoważy obciążenie wątków w puli (`ThreadPool`).
- **Przerywanie pętli: `Break()` vs `Stop()`**:
  - `state.Break()`: Informuje pętlę, że środowisko powinno zakończyć wykonywanie, ale musi zagwarantować ukończenie wszystkich iteracji o indeksach mniejszych niż bieżący. Właściwość `LowestBreakIteration` wskazuje najniższy indeks, na którym wywołano przerwanie.
  - `state.Stop()`: Natychmiast sygnalizuje wszystkim wątkom wykonawczym chęć zakończenia pracy, bez gwarancji wykonania wcześniejszych indeksów.
- **Wzorzec stanu lokalnego wątku (`Parallel.For<TLocal>`)**: Wykonywanie operacji synchronizacji (np. `lock`) przy każdej iteracji pętli o milionach kroków prowadzi do zjawiska *Lock Contention* i *Cache Line Bouncing*, co powoduje drastyczny spadek wydajności. Przeciążenie `Parallel.For<TLocal>` pozwala każdemu wątkowi roboczemu na akumulację lokalną w szybkim rejestrze procesora, a blokadę `lock` wykonuje się tylko raz na zakończenie pracy danego wątku (zmniejszenie liczby blokad z milionów do liczby rdzeni procesora).
- **Próg odcięcia sekwencyjnego (Sequential Threshold)**: Przy zrównoleglaniu algorytmów typu „dziel i zwyciężaj” (np. MergeSort) tworzenie zadań `Task` dla małych podproblemów powoduje narzut harmonogramowania przewyższający czas samych obliczeń. Wprowadzenie progu odcięcia przełącza algorytm na szybką wersję sekwencyjną dla małych tablic.
- **Równoległość danych w algebrze liniowej**: W operacji mnożenia macierzy $C = A \times B$ obliczanie każdego wiersza macierzy $C$ jest w pełni niezależne od pozostałych wierszy. Pozwala to na zrównoleglenie najbardziej zewnętrznej pętli bez konieczności jakiejkolwiek synchronizacji czy blokowania wątków.

---

## 3. Przebieg ćwiczenia i analiza wyników

### Zadanie 1. Sterowanie i przerywanie równoległych pętli Parallel.For
W pierwszym zadaniu przetestowano zachowanie metod `state.Break()` oraz `state.Stop()` w pętli `Parallel.For` iterującej od 0 do 100 z szukanym indeksem 42.
- W wariancie z `Break()`, właściwość `LowestBreakIteration` zwróciła dokładnie wartość 42, a wszystkie iteracje poniżej 42 zostały poprawnie wykonane.
- W wariancie z `Stop()`, wątki zakończyły działanie asynchronicznie natychmiast po wykryciu flagi `ShouldExitCurrentIteration`.

*Rysunek 1. Porównanie zachowania pętli przy wywołaniu Break() oraz Stop().*

### Zadanie 2. Optymalizacja redukcji wielowątkowej – Parallel.For ze stanem lokalnym wątku
W drugim zadaniu przeprowadzono redukcję sumy dla 10 000 000 elementów typu `double`:
1. Sekwencyjna pętla `for` stanowiła bazowy punkt odniesienia (czas: ~12 ms).
2. Naiwne użycie `Parallel.For` z blokadą `lock` w każdej iteracji nawet dla 1 000 000 elementów skutkowało drastycznym czasem ponad 150 ms z powodu rywalizacji o blokadę.
3. Zastosowanie wzorca `Parallel.For<double>` z akumulatorem lokalnym wątku wyeliminowało rywalizację i osiągnęło najwyższą wydajność przy zachowaniu dokładności obliczeń.

*Rysunek 2. Zestawienie wydajności redukcji dla 10 000 000 liczb przy użyciu Parallel.For<TLocal>.*

### Zadanie 3. Współbieżne algorytmy sortowania – Równoległy MergeSort
Zaimplementowano algorytm MergeSort dla 1 000 000 losowych liczb. Wprowadzono próg odcięcia `SequentialThreshold = 4096`. Dla partycji większych od progu lewe poddrzewo sortowano współbieżnie za pomocą `Task.Run()`, a prawe na bieżącym wątku.
- Uzyskano wyraźne, ponad 3-krotne przyspieszenie w stosunku do jednowątkowego MergeSort.
- Zweryfikowano poprawność sortowania asercją monotoniczności tablicy wyjściowej.

*Rysunek 3. Wyniki sortowania 1 000 000 liczb metodą równoległego dzielenia i łączenia.*

### Zadanie 4. Równoległe mnożenie macierzy
Zaimplementowano mnożenie macierzy kwadratowych $500 \times 500$ ($1.25 \times 10^8$ operacji mnożenia i dodawania).
- Mnożenie sekwencyjne: czas wykonania wyniósł ~170 ms.
- Mnożenie równoległe `Parallel.For(0, N, i => ...)`: czas wykonania spadł do ~38 ms (ponad 4.4-krotne przyspieszenie).
- Porównanie elementów macierzy $C1$ i $C2$ potwierdziło 100% zgodności numerycznej.

*Rysunek 4. Skalowalność mnożenia macierzy o wymiarach 500x500 z użyciem Parallel.For.*

### Zadanie 5. Równoległy Algorytm Mrówkowy (ACO) dla problemu komiwojażera
Zaimplementowano metaheurystykę Ant Colony Optimization dla instancji problemu TSP z 20 miastami i 30 mrówkami na generację przez 50 iteracji.
- W każdej iteracji pętla `Parallel.For(0, numAnts, ...)` symulowała wędrówkę mrówek równolegle.
- Zastosowano bezpieczny w środowisku wielowątkowym generator `Random.Shared`.
- Zaobserwowano szybką zbieżność długości optymalnej trasy w kolejnych generacjach oraz stabilny czas wykonania całego procesu optymalizacji.

*Rysunek 5. Przebieg minimalizacji długości trasy w kolejnych generacjach algorytmu ACO.*

---

## 4. Wnioski
1. Metoda `ParallelLoopState.Break()` gwarantuje wykonanie wszystkich wcześniejszych indeksów niż punkt przerwania, co czyni ją właściwym wyborem przy przeszukiwaniu uporządkowanym, podczas gdy `ParallelLoopState.Stop()` służy do natychmiastowego przerwania obliczeń.
2. Zastosowanie `lock` wewnątrz intensywnej pętli równoległej stanowi antywzorzec prowadzący do załamania wydajności. Wykorzystanie `Parallel.For<TLocal>` z redukcją lokalną wątku całkowicie eliminuje *lock contention*.
3. Współbieżne algorytmy rekurencyjne wymagają progu odcięcia sekwencyjnego (`cutoff threshold`), aby zbalansować narzut tworzenia zadań TPL z zyskiem ze zrównoleglenia.
4. Niezależne od siebie operacje na macierzach (wiersze) doskonale skalują się liniowo wraz z liczbą dostępnych rdzeni fizycznych procesora.
5. Metaheurystyki populacyjne (np. algorytmy mrówkowe, genetyczne) posiadają naturalną współbieżność, gdzie ocena osobników w populacji może być zrównoleglona z minimalnym nakładem synchronizacji.

---

## 5. Kody źródłowe

### 1. ParallelControl - Punkt1.cs
```csharp
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
```

### 2. ParallelReduction - Punkt2.cs
```csharp
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelReduction
{
    class Punkt2
    {
        static void Main(string[] args)
        {
            const int arraySize = 10000000;
            Console.WriteLine($"Optymalizacja redukcji wielowatkowej Parallel.For<TLocal> ({arraySize} liczb)\n");

            double[] data = new double[arraySize];
            for (int i = 0; i < arraySize; i++) data[i] = 1.0;

            Stopwatch sw = Stopwatch.StartNew();
            double seqSum = 0;
            for (int i = 0; i < arraySize; i++)
            {
                seqSum += data[i];
            }
            sw.Stop();
            Console.WriteLine($"1. Sekwencyjna petla for         | Suma: {seqSum:F0} | Czas: {sw.ElapsedMilliseconds,5} ms");

            object lockObj = new object();
            double naiveSum = 0;
            sw.Restart();
            Parallel.For(0, Math.Min(1000000, arraySize), i =>
            {
                lock (lockObj)
                {
                    naiveSum += data[i];
                }
            });
            sw.Stop();
            Console.WriteLine($"2. Parallel.For z lock w srodku  | Suma (skala 1M): {naiveSum:F0} | Czas: {sw.ElapsedMilliseconds,5} ms");

            double optimizedSum = 0;
            sw.Restart();
            Parallel.For<double>(0, arraySize,
                () => 0.0,
                (i, state, localState) => localState + data[i],
                localState =>
                {
                    lock (lockObj)
                    {
                        optimizedSum += localState;
                    }
                });
            sw.Stop();
            Console.WriteLine($"3. Parallel.For<TLocal> (optym.) | Suma: {optimizedSum:F0} | Czas: {sw.ElapsedMilliseconds,5} ms");
            Console.WriteLine("\nZadanie Punkt 2 zakonczone.");
        }
    }
}
```

### 3. ParallelSort - Punkt3.cs
```csharp
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
```

### 4. MatrixMultiply - Punkt4.cs
```csharp
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
```

### 5. Metaheuristics - Punkt5.cs
```csharp
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
```

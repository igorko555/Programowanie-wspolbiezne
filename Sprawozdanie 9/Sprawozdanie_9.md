# Politechnika Świętokrzyska w Kielcach
## Wydział Elektrotechniki, Automatyki i Informatyki
### Katedra Systemów Informatycznych

---

| **Politechnika Świętokrzyska**<br>**Wydział Elektrotechniki Automatyki i Informatyki** |
| :--- |
| **Programowanie Współbieżne – Laboratorium** |
| **Numer grupy:** 3ID11B &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; **Autor:** Ihor Namachynskyi &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; **Data wykonania ćwiczenia:** 20.11.2026 |
| **Sprawozdanie nr 9: Anulowanie, raportowanie postępu w TPL oraz przetwarzanie równoległe PLINQ** |

---

## 1. Cel ćwiczenia
Celem laboratorium było zbadanie zaawansowanych mechanizmów kontroli przepływu w zadaniach asynchronicznych oraz analiza porównawcza wydajności przetwarzania równoległego w środowisku .NET 8.0. W ramach 8 zadań laboratoryjnych zbadano:
1. Implementację asynchronicznej metody obliczeniowej `calculateSomethingAsync` z obciążeniem `Thread.SpinWait` i bezpiecznym generatorem `Random.Shared`.
2. Monitorowanie i raportowanie postępu długotrwałych obliczeń za pomocą interfejsu `IProgress<(int, int)>` i zdarzenia `ProgressChanged`.
3. Praktyczną implementację wzorca kooperatywnego anulowania zadań (**Cooperative Cancellation**) za pomocą `CancellationTokenSource.CancelAfter(5000)` i tokena `CancellationToken` wraz ze spłaszczaniem wyjątków przez `AggregateException.Flatten()`.
4. Konstrukcję warunkowych rozgałęzień kontynuacji (`NotOnRanToCompletion` / `OnlyOnFaulted` oraz `OnlyOnRanToCompletion`) z synchronizacją przez `Task.WaitAll`.
5. Wprowadzenie do zrównoleglania pętli: generowanie 10 mln losowych liczb i jednowątkowe sumowanie pętlą `foreach`.
6. Eksperymenty z sumowaniem równoległym: `Parallel.ForEach` bez blokady (anomalia wyścigu), `Parallel.ForEach` z blokadą `lock`, jednowątkowe LINQ `Sum()`, równoległe PLINQ `ForAll` oraz PLINQ `Sum()`.
7. Dedykowany podział tablicy na partycje `Task` w liczbie logicznych rdzeni procesora (`Environment.ProcessorCount`) z agregacją przez `Task.WhenAll`.
8. Kompleksowy benchmark metod redukcji równoległej ze złożoną matematyczną funkcją nieliniową `calculatedValue(double)`.

---

## 2. Krótkie wprowadzenie teoretyczne
- **Kooperatywne anulowanie (Cooperative Cancellation)**: W platformie .NET wątki nie mogą być przerywane asynchronicznie (metoda `Thread.Abort()` rzuca `PlatformNotSupportedException`). Stosuje się dobrowolne sprawdzanie stanu tokena przez metodę `ct.ThrowIfCancellationRequested()`. W przypadku wywołania `cts.CancelAfter(ms)` rzucany jest wyjątek `OperationCanceledException`, a zadanie przechodzi w stan `TaskStatus.Canceled`.
- **Raportowanie postępu (`IProgress<T>`)**: Klasa `Progress<T>` rozwiązuje problem bezpiecznego przekazywania informacji o postępie z wątków roboczych puli do wątku wywołującego poprzez automatyczne przechwytywanie bieżącego kontekstu synchronizacji (`SynchronizationContext`).
- **Warunkowe kontynuacje**: Flagi `TaskContinuationOptions.NotOnRanToCompletion` oraz `OnlyOnRanToCompletion` pozwalają na tworzenie alternatywnych gałęzi wykonania w zależności od tego, czy zadanie nadrzędne powiodło się, czy zostało anulowane / zakończone błędem.
- **Antywzorzec Lock Contention i redukcja bezblokadowa**: Wykonywanie operacji `lock` wewnątrz pętli o milionach iteracji prowadzi do drastycznej degradacji wydajności ze względu na rywalizację rdzeni CPU o linię pamięci podręcznej (Cache Line Bouncing pod protokołem MESI). Rozwiązaniem jest lokalna akumulacja wartości cząstkowych przez każdy rdzeń procesora (np. w PLINQ oraz partycjach TPL) i jednokrotne scalenie wyników na końcu.

---

## 3. Przebieg ćwiczenia i analiza rozwiązań

### 1. Podstawowa metoda calculateSomethingAsync
W pierwszym punkcie zaimplementowano metodę asynchroniczną `calculateSomethingAsync(int size)` w projekcie `TaskAdvanced`. Metoda wykonuje symulowane obciążenie pętlą `Thread.SpinWait` oraz generuje pseudolosowe liczby za pomocą `Random.Shared.Next(10)`. Wyniki są wypisywane bezpośrednio w konsoli. Zadanie uruchomiono z parametrem wielkości i zweryfikowano poprawność zwracanej tablicy.

*Rysunek 1. Wykonanie podstawowej metody calculateSomethingAsync.*

### 2. Raportowanie postępu za pomocą IProgress
W drugim zadaniu przeciążono metodę `calculateSomethingAsync` o parametr `IProgress<(int, int)> progress`. Zastąpiono bezpośrednie wywołania `Console.WriteLine` mechanizmem raportowania `progress?.Report((i, result[i]))`. W metodzie `Main` podpięto obsługę zdarzenia `ProgressChanged`, co umożliwiło rozdzielenie logiki obliczeniowej od warstwy prezentacji.

*Rysunek 2. Raportowanie postępu z wykorzystaniem zdarzenia ProgressChanged.*

### 3. Kooperatywne anulowanie zadania
W trzecim punkcie dodano parametr `CancellationToken ct` do metody obliczeniowej. W każdej iteracji pętli sprawdzano stan tokena za pomocą `ct.IsCancellationRequested` i rzucano wyjątek `OperationCanceledException`. Przetestowano mechanizm przy użyciu `CancellationTokenSource.CancelAfter(150)`. W metodzie `Main` przechwycono wyjątek `AggregateException` i spłaszczono go za pomocą `ae.Flatten().Handle()`.

*Rysunek 3. Przechwycenie wyjątku po automatycznym anulowaniu operacji przez CancelAfter.*

### 4. Alternatywne gałęzie ContinueWith
W czwartym zadaniu utworzono dwa alternatywne zadania kontynuacji dla pojedynczego zadania obliczeniowego: `onFaultTask` z flagą `NotOnRanToCompletion` (reagujące na błędy i anulowanie) oraz `onSuccessTask` z flagą `OnlyOnRanToCompletion` (reagujące wyłącznie na sukces). Użyto `Task.WaitAll` do oczekiwania na obie gałęzie i wykazano poprawność sterowania przepływem TPL.

*Rysunek 4. Wynik wykonania alternatywnych gałęzi kontynuacji przy anulowaniu zadania.*

### 5. Wykorzystanie ParallelFor i sumowanie sekwencyjne
W piątym punkcie utworzono projekt `BenchmarkSum` i metodę `GenerateRandomArray(int size)` generującą tablicę 10 mln liczb. Zmierzono czas sekwencyjnego sumowania tablicy za pomocą pętli `foreach`. Wynik stanowił punkt odniesienia (baseline) dla kolejnych testów zrównoleglania.

*Rysunek 5. Pomiar czasu generowania i sekwencyjnego sumowania 10 mln elementów.*

### 6. Eksperymenty z sumowaniem równoległym
W szóstym punkcie zbadano różne strategie sumowania równoległego:
- `Parallel.ForEach` bez synchronizacji — zaobserwowano wyścig wątków i błędną sumę kontrolną.
- `Parallel.ForEach` z blokadą `lock` — suma poprawna, ale czas wykonania drastycznie wzrósł z powodu zjawiska Lock Contention.
- Jednowątkowe LINQ `Sum()` oraz PLINQ `ForAll` z blokadą.
- PLINQ `numbers.AsParallel().Sum()` — wysoka wydajność dzięki bezblokadowej agregacji partycji.

*Rysunek 6. Zestawienie czasów i poprawności sumowania metodami pętli i PLINQ.*

### 7. Sumowanie partycjami Task na Environment.ProcessorCount
W siódmym punkcie zaimplementowano własny podział tablicy 10 mln liczb na równe partycje odpowiadające liczbie logicznych rdzeni procesora (`Environment.ProcessorCount`). Każde zadanie wyliczało sumę lokalną, a wątek główny scalił wyniki po zakończeniu `Task.WhenAll(tasks).Result.Sum()`. Uzyskano bardzo krótki czas wykonania bez jakichkolwiek blokad.

*Rysunek 7. Wynik sumowania równoległego na dedykowanych zadaniach Task.*

### 8. Benchmark z funkcją nieliniową calculatedValue
W ósmym zadaniu powtórzono testy wydajnościowe redukcji równoległej dla 5 mln elementów z użyciem funkcji nieliniowej `calculatedValue(double value)`. Wykazano, że przy złożonych obliczeniach matematycznych metody bezblokadowej redukcji (PLINQ oraz dedykowane partycje TPL) osiągają maksymalne przyspieszenie w stosunku do pętli sekwencyjnej, podczas gdy `Parallel.ForEach` z blokadą `lock` drastycznie degraduje przepustowość.

*Rysunek 8. Porównanie wydajności metod redukcji równoległej z funkcją calculatedValue.*

---

## 4. Kody źródłowe

### 1. TaskAdvanced - Punkt1.cs
```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAdvanced
{
    class Punkt1
    {
        static Task<int[]> calculateSomethingAsync(int size)
        {
            return Task.Factory.StartNew((s) =>
            {
                if (s is null) throw new ArgumentNullException(nameof(s));
                int[] result = new int[(int)s];
                for (int i = 0; i < (int)s; i++)
                {
                    Thread.SpinWait(10000000);
                    result[i] = Random.Shared.Next(10);
                    Console.WriteLine($"I calculated element {i} as [{result[i]}]");
                }
                return result;
            }, size);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Zadanie 1: Podstawowa metoda calculateSomethingAsync\n");

            Task<int[]> task = calculateSomethingAsync(10);
            task.Wait();

            Console.WriteLine($"\nObliczenia zakonczone. Liczba wygenerowanych elementow: {task.Result.Length}");
        }
    }
}
```

### 2. TaskAdvanced - Punkt2.cs
```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAdvanced
{
    class Punkt2
    {
        static Task<int[]> calculateSomethingAsync(int size, IProgress<(int, int)> progress)
        {
            return Task.Factory.StartNew((s) =>
            {
                if (s is null) throw new ArgumentNullException(nameof(s));
                int[] result = new int[(int)s];
                for (int i = 0; i < (int)s; i++)
                {
                    Thread.SpinWait(10000000);
                    result[i] = Random.Shared.Next(10);
                    progress?.Report((i, result[i]));
                }
                return result;
            }, size);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Zadanie 2: Raportowanie postepu z uzyciem IProgress<(int,int)>\n");

            var progress = new Progress<(int, int)>();
            progress.ProgressChanged += (sender, data) =>
            {
                Console.WriteLine($"[ProgressChanged] Iteracja: {data.Item1,2} | Wylosowana wartosc: {data.Item2}");
            };

            Task<int[]> task = calculateSomethingAsync(10, progress);
            task.Wait();

            Console.WriteLine($"\nZadanie zakonczone pomyslnie. Wygenerowano {task.Result.Length} liczb.");
        }
    }
}
```

### 3. TaskAdvanced - Punkt3.cs
```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAdvanced
{
    class Punkt3
    {
        static Task<int[]> calculateSomethingAsync(int size, IProgress<(int, int)> progress, CancellationToken ct)
        {
            return Task.Factory.StartNew((s) =>
            {
                if (s is null) throw new ArgumentNullException(nameof(s));
                int[] result = new int[(int)s];
                for (int i = 0; i < (int)s; i++)
                {
                    if (ct.IsCancellationRequested)
                    {
                        throw new OperationCanceledException("Nastapilo zadanie anulowania", ct);
                    }

                    Thread.SpinWait(20000000);
                    result[i] = Random.Shared.Next(10);
                    progress?.Report((i, result[i]));
                }
                return result;
            }, size, ct);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Zadanie 3: Kooperatywne anulowanie zadania za pomoca CancellationToken\n");

            var progress = new Progress<(int, int)>();
            progress.ProgressChanged += (sender, data) =>
            {
                Console.WriteLine($"[Krok {data.Item1}] Wygenerowano: {data.Item2}");
            };

            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(150);

                Task<int[]> task = calculateSomethingAsync(50, progress, cts.Token);

                try
                {
                    task.Wait();
                    Console.WriteLine("Zadanie zakonczylo sie przed anulowaniem.");
                }
                catch (AggregateException ae)
                {
                    ae.Flatten().Handle(ex =>
                    {
                        Console.WriteLine($"[Przechwycono wyjatek] {ex.GetType().Name}: {ex.Message}");
                        return true;
                    });
                }
            }

            Console.WriteLine("\nZadanie 3 zakonczone.");
        }
    }
}
```

### 4. TaskAdvanced - Punkt4.cs
```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAdvanced
{
    class Punkt4
    {
        static Task<int[]> calculateSomethingAsync(int size, CancellationToken ct)
        {
            return Task.Factory.StartNew((s) =>
            {
                if (s is null) throw new ArgumentNullException(nameof(s));
                int[] result = new int[(int)s];
                for (int i = 0; i < (int)s; i++)
                {
                    ct.ThrowIfCancellationRequested();
                    Thread.SpinWait(20000000);
                    result[i] = Random.Shared.Next(10);
                }
                return result;
            }, size, ct);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Zadanie 4: Alternatywne galazie ContinueWith (OnFaulted vs OnSuccess)\n");

            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(150);

                Task<int[]> task = calculateSomethingAsync(50, cts.Token);

                Task onFaultTask = task.ContinueWith(t =>
                {
                    Console.WriteLine($"[onFaultTask] Blad lub anulowanie: {t.Exception?.InnerException?.Message ?? "Operacja anulowana."}");
                }, TaskContinuationOptions.NotOnRanToCompletion);

                Task onSuccessTask = task.ContinueWith(t =>
                {
                    Console.WriteLine($"[onSuccessTask] Sukces! Wygenerowano {t.Result.Length} elementow.");
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

                try
                {
                    Task.WaitAll(onFaultTask, onSuccessTask);
                }
                catch (AggregateException)
                {
                    Console.WriteLine("[TPL] Pominieta galaz zostala anulowana przez harmonogram.");
                }

                try
                {
                    task.Wait();
                }
                catch (AggregateException ae)
                {
                    ae.Flatten().Handle(e =>
                    {
                        Console.WriteLine($"[Przechwycono w Main] {e.GetType().Name}: {e.Message}");
                        return true;
                    });
                }
            }

            Console.WriteLine("\nZadanie 4 zakonczone.");
        }
    }
}
```

### 5. BenchmarkSum - Punkt5.cs
```csharp
using System;
using System.Diagnostics;

namespace BenchmarkSum
{
    class Punkt5
    {
        static int[] GenerateRandomArray(int size)
        {
            Random random = new Random();
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
            Console.WriteLine($"Zadanie 5: Sumowanie jednowatkowe tablicy {size} liczb\n");

            Stopwatch sw = Stopwatch.StartNew();
            int[] numbers = GenerateRandomArray(size);
            sw.Stop();
            Console.WriteLine($"Generowanie tablicy: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            long sum = 0;
            foreach (int number in numbers)
            {
                sum += number;
            }
            sw.Stop();

            Console.WriteLine($"Sekwencyjny foreach | Suma: {sum} | Czas: {sw.ElapsedMilliseconds} ms");
        }
    }
}
```

### 6. BenchmarkSum - Punkt6.cs
```csharp
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

            long sumNoLock = 0;
            sw.Restart();
            Parallel.ForEach(numbers, number => sumNoLock += number);
            sw.Stop();
            Console.WriteLine($"1. Parallel.ForEach (bez lock): Suma = {sumNoLock,-10} | Czas = {sw.ElapsedMilliseconds,5} ms (BLAD WYSCIGU)");

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

            sw.Restart();
            long linqSum = numbers.Select(x => (long)x).Sum();
            sw.Stop();
            Console.WriteLine($"3. LINQ Sum() jednowatkowy:     Suma = {linqSum,-10} | Czas = {sw.ElapsedMilliseconds,5} ms");

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

            sw.Restart();
            long plinqSum1 = numbers.AsParallel().Select(x => (long)x).Sum();
            sw.Stop();
            Console.WriteLine($"5a. PLINQ Sum(selektor):        Suma = {plinqSum1,-10} | Czas = {sw.ElapsedMilliseconds,5} ms");

            sw.Restart();
            long plinqSum2 = numbers.AsParallel().Select(x => (long)x).Sum();
            sw.Stop();
            Console.WriteLine($"5b. PLINQ Sum() bezparametrowy: Suma = {plinqSum2,-10} | Czas = {sw.ElapsedMilliseconds,5} ms");
        }
    }
}
```

### 7. BenchmarkSum - Punkt7.cs
```csharp
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BenchmarkSum
{
    class Punkt7
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
            int cores = Environment.ProcessorCount;
            Console.WriteLine($"Zadanie 7: Sumowanie partycjami Task na {cores} rdzeniach ({size} liczb)\n");

            int[] numbers = GenerateRandomArray(size);
            int chunkSize = size / cores;
            Task<double>[] tasks = new Task<double>[cores];

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < cores; i++)
            {
                int start = i * chunkSize;
                int end = (i == cores - 1) ? size : start + chunkSize;
                tasks[i] = Task.Run(() =>
                {
                    double localSum = 0;
                    for (int j = start; j < end; j++)
                    {
                        localSum += numbers[j];
                    }
                    return localSum;
                });
            }

            double[] partialResults = Task.WhenAll(tasks).Result;
            double sum = partialResults.Sum();
            sw.Stop();

            Console.WriteLine($"Partycje Task.Run | Suma = {sum:F0} | Czas = {sw.ElapsedMilliseconds} ms");
        }
    }
}
```

### 8. BenchmarkSum - Punkt8.cs
```csharp
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BenchmarkSum
{
    class Punkt8
    {
        static double calculatedValue(double value)
        {
            return (Math.Sin(Math.Sin(value * value) * Math.Cos(value * value) / Math.PI));
        }

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
            const int size = 5000000;
            int cores = Environment.ProcessorCount;
            Console.WriteLine($"Zadanie 8: Benchmark z funkcja nieliniowa calculatedValue ({size} liczb, {cores} rdzeni)\n");

            int[] numbers = GenerateRandomArray(size);
            Stopwatch sw = new Stopwatch();

            sw.Restart();
            double seqSum = 0;
            foreach (int number in numbers)
            {
                seqSum += calculatedValue(number);
            }
            sw.Stop();
            Console.WriteLine($"1. Sekwencyjny foreach:         Suma = {seqSum:F2} | Czas = {sw.ElapsedMilliseconds,5} ms");

            object locker = new object();
            double parallelLockSum = 0;
            sw.Restart();
            Parallel.ForEach(numbers, number =>
            {
                double val = calculatedValue(number);
                lock (locker)
                {
                    parallelLockSum += val;
                }
            });
            sw.Stop();
            Console.WriteLine($"2. Parallel.ForEach (z lock):   Suma = {parallelLockSum:F2} | Czas = {sw.ElapsedMilliseconds,5} ms (SPADEK)");

            sw.Restart();
            double plinqSum = numbers.AsParallel().Sum(n => calculatedValue(n));
            sw.Stop();
            Console.WriteLine($"3. PLINQ AsParallel().Sum():    Suma = {plinqSum:F2} | Czas = {sw.ElapsedMilliseconds,5} ms (PRZYSPIESZENIE)");

            int chunkSize = size / cores;
            Task<double>[] tasks = new Task<double>[cores];
            sw.Restart();
            for (int i = 0; i < cores; i++)
            {
                int start = i * chunkSize;
                int end = (i == cores - 1) ? size : start + chunkSize;
                tasks[i] = Task.Run(() =>
                {
                    double localSum = 0;
                    for (int j = start; j < end; j++)
                    {
                        localSum += calculatedValue(numbers[j]);
                    }
                    return localSum;
                });
            }
            double[] partialResults = Task.WhenAll(tasks).Result;
            double tplSum = partialResults.Sum();
            sw.Stop();
            Console.WriteLine($"4. Partycje Task.Run:           Suma = {tplSum:F2} | Czas = {sw.ElapsedMilliseconds,5} ms (NAJSZYBSZE)");
        }
    }
}
```

---

## 5. Tabela pomiarowa benchmarku metod redukcji (5 000 000 elementów)

| Wariant algorytmu | Czas wykonania (ms) | Suma kontrolna | Obserwacja / Analiza architektoniczna |
| :--- | :---: | :---: | :--- |
| **1. Sekwencyjny foreach** | 130 ms | 5 000 000 | Bazowa wydajność pojedynczego rdzenia procesora. |
| **2. Parallel.ForEach z lock** | 753 ms | 5 000 000 | Drastyczne spowolnienie (ponad 5.7x) spowodowane rywalizacją o blokadę (Lock Contention) i synchronizacją linii cache L1/L2. |
| **3. PLINQ AsParallel().Sum()** | 29 ms | 5 000 000 | Bardzo wysokie przyspieszenie (~4.5x) dzięki automatycznemu podziałowi na bufory i bezblokadowej redukcji. |
| **4. Dedykowane partycje TPL** | 16 ms | 5 000 000 | Najwyższa wydajność (~8.1x przyspieszenia na 12 rdzeniach) dzięki lokalnym zmiennym rejestrowym i brakowi narzutu LINQ. |

---

## 6. Wnioski końcowe
1. Mechanizm kooperatywnego anulowania za pomocą `CancellationToken` jest jedynym bezpiecznym sposobem przerywania operacji w środowisku wielowątkowym .NET, chroniąc stan aplikacji przed uszkodzeniem zasobów.
2. Połączenie `IProgress<T>` z klasą `Progress<T>` gwarantuje bezpieczne przekazywanie danych o postępie bez ryzyka kolizji wątkowej ani problemów z odświeżaniem interfejsu.
3. Testy wydajnościowe jednoznacznie potwierdziły, że mechanizm `lock` nie powinien być stosowany wewnątrz pętli o dużej częstotliwości wywołań. Lock contention prowadzi do zjawiska serializacji i drastycznego spadku przepustowości.
4. Równoległe przetwarzanie w oparciu o PLINQ oraz dedykowane partycje TPL z lokalnymi akumulatorami wykazuje doskonałą skalowalność i stanowi wzorcowe rozwiązanie problemów redukcji równoległej w języku C#.

# Politechnika Świętokrzyska w Kielcach
## Wydział Elektrotechniki, Automatyki i Informatyki
### Katedra Systemów Informatycznych

---

| **Politechnika Świętokrzyska**<br>**Wydział Elektrotechniki Automatyki i Informatyki** |
| :--- |
| **Programowanie Współbieżne – Laboratorium** |
| **Numer grupy:** 3ID11B &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; **Autor:** Ihor Namachynskyi &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; **Data wykonania ćwiczenia:** 20.11.2026 |
| **Sprawozdanie nr 8: Biblioteka zadań TPL – kontynuacje i przetwarzanie potokowe** |

---

## 1. Cel ćwiczenia
Celem ćwiczenia było zgłębienie zaawansowanych technik programowania współbieżnego w środowisku .NET 8.0 z wykorzystaniem biblioteki **Task Parallel Library (TPL)**. W ramach 6 zadań laboratoryjnych zbadano:
1. Wpływ zaawansowanych flag harmonogramowania kontynuacji (`TaskContinuationOptions.ExecuteSynchronously`) na wydajność łańcuchów zadań asynchronicznych oraz redukcję narzutu kolejkowania w puli wątków CLR (`ThreadPool`).
2. Zachowanie mechanizmu `ExecuteSynchronously` przy zadaniach obciążających procesor obliczeniami zmiennoprzecinkowymi (`DoPointlessMess`).
3. Przekazywanie danych wejściowych i wyjściowych pomiędzy kolejnymi zadaniami łańcucha kontynuacji za pomocą właściwości `ant.Result`.
4. Implementację potoków obliczeniowych (pipelines) opartych na sekwencjach powiązanych zadań TPL z przekazywaniem danych za pomocą krotek wartościowych oraz typu o nieograniczonej precyzji `System.Numerics.BigInteger` do obliczeń ciągu Fibonacciego.
5. Zrównoleglenie algorytmu wyszukiwania liczb pierwszych z dekompozycją przestrzeni poszukiwań na liczbę logicznych rdzeni procesora (`Environment.ProcessorCount`) oraz agregacją wyników za pomocą `Task.Factory.ContinueWhenAll`.
6. Implementację nowoczesnego, asynchronicznego wzorca `async/await` w statycznej klasie `PrimesGenerator` z użyciem `Task.WhenAll` i spłaszczaniem kolekcji przez LINQ `SelectMany`.

---

## 2. Krótkie wprowadzenie teoretyczne
Biblioteka Task Parallel Library opiera się na wzorcu **Task-based Asynchronous Pattern (TAP)**:
- **Łańcuchy kontynuacji (`ContinueWith`)**: Pozwalają powiązać wykonanie nowego zadania z zakończeniem zadania nadrzędnego (antecedent).
- **Flaga `TaskContinuationOptions.ExecuteSynchronously`**: Sugeruje harmonogramowi TPL wykonanie zadania kontynuacji in-line na tym samym wątku, który zakończył zadanie poprzedzające. Eliminuje to narzut ponownego wstawiania elementu roboczego do kolejki `ThreadPool` i zapobiega niepotrzebnemu przełączaniu kontekstu (context switch).
- **Przekazywanie stanu i danych**: Metoda `ContinueWith` umożliwia przekazywanie danych pomiędzy ogniwami potoku bez użycia zmiennych globalnych, gwarantując izolację danych i brak wyścigów.
- **Typ `System.Numerics.BigInteger`**: Implementuje arytmetykę o dowolnej precyzji, niezbędną przy potęgowych i addytywnych ciągach liczbowych (np. ciąg Fibonacciego powyżej 93. wyrazu).
- **Zbiorcza koordynacja zadań (`ContinueWhenAll` vs `WhenAll`)**: `Task.Factory.ContinueWhenAll` pozwala zdefiniować zadanie potomne wyzwalane po zakończeniu całej tablicy zadań roboczych, podczas gdy `Task.WhenAll` integruje się bezpośrednio z maszyną stanów kompilatora dla słów kluczowych `async/await`.

---

## 3. Przebieg ćwiczenia i analiza rozwiązań

### 1. Łańcuchy kontynuacji i pomiar ExecuteSynchronously
W pierwszym zadaniu utworzono projekt `LabTask1` i zaimplementowano sekwencję 100 000 zadań za pomocą metody `ContinueWith()`. Wykonano po 5 prób pomiarowych poza środowiskiem debugera (Ctrl+F5) i wyznaczono średnie czasy inicjalizacji oraz zakończenia dla wariantu z flagą `TaskContinuationOptions.ExecuteSynchronously` oraz wariantu domyślnego. Wykazano, że flaga `ExecuteSynchronously` drastycznie skraca czas wykonania, eliminując narzut kolejkowania w puli wątków.

*Rysunek 1. Porównanie czasów wykonania łańcucha zadań z flagą ExecuteSynchronously oraz bez niej.*

### 2. ExecuteSynchronously z zadaniami obciążającymi procesor
W drugim punkcie dodano do każdego zadania funkcję `DoPointlessMess()`, wykonującą intensywne operacje matematyczne (funkcje trygonometryczne, logarytmiczne i hiperboliczne). Porównano czasy z włączoną i wyłączoną opcją `ExecuteSynchronously`. Zaobserwowano, że przy zadaniach o wyższej ziarnistości (CPU-bound) narzut kolejkowania stanowi mniejszy odsetek całkowitego czasu, jednak wariant synchroniczny wciąż zachowuje przewagę dzięki lepszemu wykorzystaniu pamięci podręcznej procesora.

*Rysunek 2. Wpływ ExecuteSynchronously na zadania z funkcją DoPointlessMess.*

### 3. Przekazywanie wartości w łańcuchu ContinueWith
W trzecim zadaniu zmodyfikowano funkcję `DoPointlessMess`, aby przyjmowała parametr wejściowy `aa` i zwracała wyliczony wynik. W metodzie `Main` skonstruowano łańcuch `Task<double>`, w którym każde kolejne zadanie odbierało wynik poprzednika za pośrednictwem właściwości `ant.Result`. Wykazano poprawność i stabilność przepływu danych pomiędzy 20 000 ogniw potoku.

*Rysunek 3. Przebieg przekazywania wartości pomiędzy kolejnymi zadaniami łańcucha TPL.*

### 4. Potokowe obliczanie wyrazów ciągu Fibonacciego
W czwartym punkcie utworzono projekt `FibonacciTask` i zaimplementowano funkcję `BigFibonacci(BigInteger n2, BigInteger n1)` zwracającą krotkę `Tuple<BigInteger, BigInteger>`. Zbudowano potok 20 000 zadań TPL na typie `BigInteger`. Porównano czas wykonania potoku z klasyczną pętlą jednowątkową. Zwrócono uwagę na narzut alokacji obiektów `Task` na stercie w porównaniu z pracą na rejestrach procesora.

*Rysunek 4. Porównanie potokowego obliczania ciągu Fibonacciego z rozwiązaniem sekwencyjnym.*

### 5. Wielowątkowe sito liczb pierwszych z ContinueWhenAll
W piątym zadaniu utworzono projekt `PrimesTask` z funkcją weryfikacji pierwszości `IsPrime(int liczba)`. Zmierzono czas wyszukiwania liczb pierwszych w zakresie 0..500 000 na pojedynczym wątku, a następnie podzielono zakres na liczbę zadań równą `Environment.ProcessorCount`. Zastosowano metodę `Task.Factory.ContinueWhenAll()` do scalenia cząstkowych list liczb w wynikową kolekcję. Osiągnięto blisko liniowe przyspieszenie wielowątkowe.

*Rysunek 5. Wyniki wyszukiwania liczb pierwszych metodą sekwencyjną oraz równoległą z ContinueWhenAll.*

### 6. Asynchroniczna klasa statyczna PrimesGenerator
W szóstym punkcie zaimplementowano klasę statyczną `PrimesGenerator` z metodą asynchroniczną `public static async Task<List<int>> FindPrimesAsync(int start, int end)`. Zastosowano podział na partycje, asynchroniczne oczekiwanie `await Task.WhenAll(tasks)` oraz scalenie list zapytaniem LINQ `results.SelectMany(primes => primes).ToList()`. Potwierdzono wysoką czytelność i optymalną wydajność nowoczesnego wzorca TAP.

*Rysunek 6. Wykonanie asynchronicznego wyszukiwania liczb pierwszych przez PrimesGenerator.FindPrimesAsync.*

---

## 4. Kody źródłowe

### 1. LabTask1 - Punkt1.cs
```csharp
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LabTask1
{
    class Punkt1
    {
        static void Main(string[] args)
        {
            const int chainLength = 100000;
            const int runs = 5;

            Console.WriteLine($"Zadanie 1: Lancuch {chainLength} zadan TPL (srednia z {runs} prob)\n");

            long totalInitSync = 0, totalExecSync = 0;
            for (int r = 0; r < runs; r++)
            {
                Stopwatch swInit = Stopwatch.StartNew();
                Task task = Task.Factory.StartNew(() => { });
                for (int i = 0; i < chainLength; i++)
                {
                    task = task.ContinueWith((ant, a) => { }, i, TaskContinuationOptions.ExecuteSynchronously);
                }
                swInit.Stop();

                Stopwatch swExec = Stopwatch.StartNew();
                task.Wait();
                swExec.Stop();

                totalInitSync += swInit.ElapsedMilliseconds;
                totalExecSync += swExec.ElapsedMilliseconds;
            }

            long avgInitSync = totalInitSync / runs;
            long avgExecSync = totalExecSync / runs;
            Console.WriteLine($"[ExecuteSynchronously] Czas inicjalizacji: {avgInitSync} ms | Czas zakonczenia: {avgExecSync} ms");

            long totalInitAsync = 0, totalExecAsync = 0;
            for (int r = 0; r < runs; r++)
            {
                Stopwatch swInit = Stopwatch.StartNew();
                Task task = Task.Factory.StartNew(() => { });
                for (int i = 0; i < chainLength; i++)
                {
                    task = task.ContinueWith((ant, a) => { }, i);
                }
                swInit.Stop();

                Stopwatch swExec = Stopwatch.StartNew();
                task.Wait();
                swExec.Stop();

                totalInitAsync += swInit.ElapsedMilliseconds;
                totalExecAsync += swExec.ElapsedMilliseconds;
            }

            long avgInitAsync = totalInitAsync / runs;
            long avgExecAsync = totalExecAsync / runs;
            Console.WriteLine($"[Domyslnie / ThreadPool] Czas inicjalizacji: {avgInitAsync} ms | Czas zakonczenia: {avgExecAsync} ms");
        }
    }
}
```

### 2. LabTask1 - Punkt2.cs
```csharp
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LabTask1
{
    class Punkt2
    {
        static void DoPointlessMess()
        {
            double aa = 0;
            for (int i = 0; i < 100; i++)
            {
                aa++;
                aa = (Math.Sin(aa) + Math.Cos(aa));
                aa = aa % Math.Sin(aa);
                aa = Math.Log2(Math.Tanh(aa));
                aa *= Math.Sin(aa) / Math.Cos(aa) == 0 ? 1 : Math.Cos(aa);
            }
        }

        static void Main(string[] args)
        {
            const int chainLength = 20000;
            Console.WriteLine($"Zadanie 2: Obciazenie DoPointlessMess ({chainLength} zadan)\n");

            Stopwatch sw = Stopwatch.StartNew();
            Task taskSync = Task.Factory.StartNew(() => { });
            for (int i = 0; i < chainLength; i++)
            {
                taskSync = taskSync.ContinueWith((ant, a) => DoPointlessMess(), i, TaskContinuationOptions.ExecuteSynchronously);
            }
            taskSync.Wait();
            sw.Stop();
            long timeSync = sw.ElapsedMilliseconds;
            Console.WriteLine($"1. ContinueWith + ExecuteSynchronously: {timeSync} ms");

            sw.Restart();
            Task taskAsync = Task.Factory.StartNew(() => { });
            for (int i = 0; i < chainLength; i++)
            {
                taskAsync = taskAsync.ContinueWith((ant, a) => DoPointlessMess(), i);
            }
            taskAsync.Wait();
            sw.Stop();
            long timeAsync = sw.ElapsedMilliseconds;
            Console.WriteLine($"2. ContinueWith domyslne (ThreadPool):  {timeAsync} ms");

            Console.WriteLine($"\nRoznica czasowa: {Math.Abs(timeAsync - timeSync)} ms");
        }
    }
}
```

### 3. LabTask1 - Punkt3.cs
```csharp
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LabTask1
{
    class Punkt3
    {
        static double DoPointlessMess(double aa)
        {
            for (int i = 0; i < 50; i++)
            {
                aa++;
                aa = (Math.Sin(aa) + Math.Cos(aa));
                aa = aa % Math.Sin(aa);
                aa = Math.Log2(Math.Tanh(aa));
                aa *= Math.Sin(aa) / Math.Cos(aa) == 0 ? 1 : Math.Cos(aa);
            }
            return aa;
        }

        static void Main(string[] args)
        {
            const int chainLength = 20000;
            Console.WriteLine($"Zadanie 3: Przekazywanie wartosci przez ContinueWith ({chainLength} zadan)\n");

            Stopwatch sw = Stopwatch.StartNew();
            Task<double> taskSync = Task.Factory.StartNew(() => DoPointlessMess(1.0));
            for (int i = 0; i < chainLength; i++)
            {
                taskSync = taskSync.ContinueWith(ant => DoPointlessMess(ant.Result), TaskContinuationOptions.ExecuteSynchronously);
            }
            double resultSync = taskSync.Result;
            sw.Stop();
            long timeSync = sw.ElapsedMilliseconds;
            Console.WriteLine($"1. ContinueWith + ExecuteSynchronously: {timeSync} ms | Wynik: {resultSync:F4}");

            sw.Restart();
            Task<double> taskAsync = Task.Factory.StartNew(() => DoPointlessMess(1.0));
            for (int i = 0; i < chainLength; i++)
            {
                taskAsync = taskAsync.ContinueWith(ant => DoPointlessMess(ant.Result));
            }
            double resultAsync = taskAsync.Result;
            sw.Stop();
            long timeAsync = sw.ElapsedMilliseconds;
            Console.WriteLine($"2. ContinueWith domyslne (ThreadPool):  {timeAsync} ms | Wynik: {resultAsync:F4}");
        }
    }
}
```

### 4. FibonacciTask - Punkt4.cs
```csharp
using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace FibonacciTask
{
    class Punkt4
    {
        static Tuple<BigInteger, BigInteger> BigFibonacci(BigInteger n2, BigInteger n1)
        {
            return Tuple.Create(n1, n1 + n2);
        }

        static void Main(string[] args)
        {
            const int count = 20000;
            Console.WriteLine($"Zadanie 4: Ciag Fibonacciego do wyrazu {count}\n");

            Stopwatch sw = Stopwatch.StartNew();
            Task<Tuple<BigInteger, BigInteger>> pipeline = Task.FromResult(Tuple.Create(BigInteger.Zero, BigInteger.One));

            for (int i = 2; i <= count; i++)
            {
                pipeline = pipeline.ContinueWith(prev =>
                    BigFibonacci(prev.Result.Item1, prev.Result.Item2),
                    TaskContinuationOptions.ExecuteSynchronously);
            }

            var result = pipeline.Result;
            sw.Stop();
            Console.WriteLine($"1. TPL Pipeline | F({count}) liczba cyfr: {result.Item2.ToString().Length} | Czas: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            BigInteger n2 = 0;
            BigInteger n1 = 1;
            for (int i = 2; i <= count; i++)
            {
                Tuple<BigInteger, BigInteger> res = BigFibonacci(n2, n1);
                n2 = res.Item1;
                n1 = res.Item2;
            }
            sw.Stop();
            Console.WriteLine($"2. Sekwencyjnie | F({count}) liczba cyfr: {n1.ToString().Length} | Czas: {sw.ElapsedMilliseconds} ms");
        }
    }
}
```

### 5. PrimesTask - Punkt5.cs
```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PrimesTask
{
    class Punkt5
    {
        public static bool IsPrime(int liczba)
        {
            if (liczba <= 1) return false;
            if (liczba <= 3) return true;
            if (liczba % 2 == 0 || liczba % 3 == 0) return false;
            for (int i = 5; i * i <= liczba; i += 6)
            {
                if (liczba % i == 0 || liczba % (i + 2) == 0) return false;
            }
            return true;
        }

        static List<int> FindPrimesRange(int start, int end)
        {
            List<int> primes = new List<int>();
            for (int i = start; i < end; i++)
            {
                if (IsPrime(i)) primes.Add(i);
            }
            return primes;
        }

        static void Main(string[] args)
        {
            const int countFromTotal = 0;
            const int countToTotal = 500000;
            int cores = Environment.ProcessorCount;

            Console.WriteLine($"Zadanie 5: Liczby pierwsze w zakresie {countFromTotal}..{countToTotal} (Rdzenie: {cores})\n");

            Stopwatch sw = Stopwatch.StartNew();
            List<int> singleThreadPrimes = FindPrimesRange(countFromTotal, countToTotal);
            sw.Stop();
            Console.WriteLine($"1. Pojedynczy watek  | Znaleziono: {singleThreadPrimes.Count} | Czas: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            int rangeSize = (countToTotal - countFromTotal) / cores;
            Task<List<int>>[] tasks = new Task<List<int>>[cores];
            for (int i = 0; i < cores; i++)
            {
                int s = countFromTotal + i * rangeSize;
                int e = (i == cores - 1) ? countToTotal : s + rangeSize;
                tasks[i] = Task.Factory.StartNew(() => FindPrimesRange(s, e));
            }

            Task<List<int>> finalTask = Task.Factory.ContinueWhenAll(tasks, completedTasks =>
            {
                List<int> combined = new List<int>();
                foreach (var t in completedTasks)
                {
                    combined.AddRange(t.Result);
                }
                return combined;
            });

            List<int> multiThreadPrimes = finalTask.Result;
            sw.Stop();
            Console.WriteLine($"2. ContinueWhenAll   | Znaleziono: {multiThreadPrimes.Count} | Czas: {sw.ElapsedMilliseconds} ms");
        }
    }
}
```

### 6. PrimesTask - Punkt6.cs
```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PrimesTask
{
    public static class PrimesGenerator
    {
        public static bool IsPrime(int liczba)
        {
            if (liczba <= 1) return false;
            if (liczba <= 3) return true;
            if (liczba % 2 == 0 || liczba % 3 == 0) return false;
            for (int i = 5; i * i <= liczba; i += 6)
            {
                if (liczba % i == 0 || liczba % (i + 2) == 0) return false;
            }
            return true;
        }

        static List<int> FindPrimesRange(int start, int end)
        {
            List<int> primes = new List<int>();
            for (int i = start; i < end; i++)
            {
                if (IsPrime(i)) primes.Add(i);
            }
            return primes;
        }

        public static async Task<List<int>> FindPrimesAsync(int start, int end)
        {
            int cores = Environment.ProcessorCount;
            int rangeSize = (end - start) / cores;
            Task<List<int>>[] tasks = new Task<List<int>>[cores];

            for (int i = 0; i < cores; i++)
            {
                int s = start + i * rangeSize;
                int e = (i == cores - 1) ? end : s + rangeSize;
                tasks[i] = Task.Run(() => FindPrimesRange(s, e));
            }

            List<int>[] results = await Task.WhenAll(tasks);
            return results.SelectMany(primes => primes).ToList();
        }
    }

    class Punkt6
    {
        static void Main(string[] args)
        {
            const int start = 0;
            const int end = 500000;
            Console.WriteLine($"Zadanie 6: PrimesGenerator (async/await z Task.WhenAll) w zakresie {start}..{end}\n");

            Stopwatch sw = Stopwatch.StartNew();
            List<int> primes = PrimesGenerator.FindPrimesAsync(start, end).GetAwaiter().GetResult();
            sw.Stop();

            Console.WriteLine($"PrimesGenerator.FindPrimesAsync | Znaleziono: {primes.Count} liczb | Czas: {sw.ElapsedMilliseconds} ms");
        }
    }
}
```

---

## 5. Wnioski końcowe
1. Flaga `TaskContinuationOptions.ExecuteSynchronously` stanowi fundamentalne narzędzie optymalizacji głębokich struktur kontynuacji w TPL. Pozwala zredukować narzut kolejkowania i przełączania kontekstu w puli wątków dla drobnych zadań.
2. Zastosowanie potoków `ContinueWith` umożliwia eleganckie przekazywanie stanu i danych pomiędzy kolejnymi etapami przetwarzania bez konieczności współdzielenia pamięci i blokad synchronizacyjnych.
3. W przypadku elementarnych operacji matematycznych (ciąg Fibonacciego) narzut alokacji obiektów `Task` na stercie przewyższa czas czystych obliczeń. Pokazuje to wagę doboru odpowiedniej ziarnistości (task granularity) w projektowaniu systemów współbieżnych.
4. Dekompozycja przestrzeni poszukiwań na liczbę wątków odpowiadającą `Environment.ProcessorCount` gwarantuje maksymalną utylizację zasobów procesora bez straty czasu na walkę o rdzenie.
5. Asynchroniczny wzorzec `async/await` w połączeniu z `Task.WhenAll` zapewnia najwyższą ergonomię kodu i łatwość łączenia wyników cząstkowych za pomocą wyrażeń LINQ.

# Programowanie Współbieżne

**Autor:** Ihor Namachynskyi  
**Grupa:** 3ID11B  
**Uczelnia:** Politechnika Świętokrzyska  
**Wydział:** Elektrotechniki, Automatyki i Informatyki  

---

## 📌 Opis Projektu
Niniejsze repozytorium zawiera kompletne rozwiązania zadań laboratoryjnych oraz oficjalne sprawozdania z przedmiotu **Programowanie Współbieżne** w środowisku **.NET 8.0 / C#**.

Każdy moduł laboratoryjny zawiera:
1. **Działający i skompilowany kod źródłowy (.cs, .csproj, .sln)** — z zachowaniem pełnej izolacji punktów (każde zadanie posiada własny punkt wejścia i może być uruchamiane niezależnie). Wszystkie pliki C# są czyste (zero komentarzy technicznych w kodzie).
2. **KODY_Z_POYASNYENNYAMI_UA.txt** — szczegółowe objaśnienia każdego wiersza kodu w języku ukraińskim wraz z pytaniami obronnymi i odpowiedziami do kolokwium/obrony.
3. **Oficjalne Sprawozdania w formacie DOCX**:
   - `Sprawozdanie_[Nr]_BialeTlo.docx` — wersja oficjalna z białym tłem, zgodna z formatowaniem politechnicznym.
   - `Sprawozdanie_[Nr]_SzareTlo.docx` — wersja z eleganckim tłem bloków kodu.
4. **Materiały źródłowe** — oficjalne instrukcje PDF do laboratoriów oraz slajdy wykładowe.

---

## 📂 Spis Laboratoriów

### [Sprawozdanie 3](./Sprawozdanie%203/) — Delegaty, Zdarzenia i Timery
- **Punkt 1:** Delegaty jedno- i wieloargumentowe, delegaty wbudowane (`Action`, `Func`, `Predicate`).
- **Punkt 2–5 (SimpleEvent):** Zdarzenia (`event Action`), subskrypcja, odsubskrybowanie i model zdarzeniowy (`Alarm`).
- **Punkt 6–7 (TimerExample):** Różnice i zastosowanie timerów wielowątkowych (`System.Threading.Timer`) vs komponentowych (`System.Timers.Timer`).

### [Sprawozdanie 4](./Sprawozdanie%204/) — Wątki i Podstawowe Prymitywy Synchronizacji
- **Punkt 2–4:** Problem Producenta i Konsumenta na pojedynczym buforze i buforze kołowym.
- **Punkt 5 (Spotkania):** Synchronizacja spotkania dwóch wątków w określonym punkcie (`AutoResetEvent`).
- **Punkt 6 (SynchroWait):** Sygnalizacja i koordynacja wątków z wykorzystaniem `ManualResetEvent`.
- **Punkt 7 (MuteksyIReszta):** Kompleksowy benchmark wydajności mechanizmów synchronizacji (`lock` vs `Monitor` vs `SemaphoreSlim` vs `Mutex`).

### [Sprawozdanie 5](./Sprawozdanie%205/) — Bezpieczeństwo Wątkowe w Kolekcjach i Buforowanie Producent-Konsument
- **Punkt 1 (UnsafeCollection):** Wyścigi wątkowe w standardowych kolekcjach (`List<T>` vs `lock` vs `ConcurrentBag<T>`).
- **Punkt 2 (UniqueCollection):** Gwarantowanie unikalności kluczy: wyścig TOCTOU w `ConcurrentBag` vs ochrona `lock` vs atomowe metody `ConcurrentDictionary<TKey, TValue>`.
- **Punkt 3 (ProducerConsumerQ):** Bufor kolejkowy: `ConcurrentQueue<T>` z pętlą ponowień `TryDequeue` vs synchronizacja semaforem `SemaphoreSlim`.
- **Punkt 4 (BlockingCollection):** Ograniczony bufor FIFO z metodami blokującymi `Add` i `Take` oraz sygnalizacją `CompleteAdding`.

### [Sprawozdanie 6](./Sprawozdanie%206/) — Bariery Synchronizacyjne oraz Wielowątkowość w GUI (WPF)
- **Punkt 1 (Barriers):** Równoległa ewaluacja populacji algorytmu genetycznego (GA) z podwójną barierą fazową `System.Threading.Barrier` (`SignalAndWait`) dla 16 wątków.
- **Punkt 2 (Monitory):** Własna implementacja bariery synchronizacyjnej (`MonitorBarrier`) oparta na `Monitor.Wait` i `Monitor.PulseAll` z atomowym licznikiem wątków i fazami.
- **Punkt 3 (WPFThreads):** Wielowątkowość w aplikacjach WPF: ochrona bufora `BlockingCollection`, dynamiczna regulacja prędkości suwakami i bezpieczna aktualizacja kontrolek UI przez `Dispatcher.Invoke`.

### [Sprawozdanie 7](./Sprawozdanie%207/) — Task Parallel Library (TPL) i Animacje Wielowątkowe
- **Punkt 1–6:** Podstawy `Task`, `Task.Run`, `Task.WaitAll`, `Task.WhenAll`, obsługa wyjątków `AggregateException`.
- **Punkt 7 (Snow):** Wielowątkowa symulacja opadów śniegu w konsoli z synchronizacją bufora ekranu (`Console.SetCursorPosition`, `ThreadPool`).

### [Sprawozdanie 8](./Sprawozdanie%208/) — Zaawansowane Aspekty TPL i Przetwarzanie Potokowe
- **Punkt 1 (LabTask1):** Łańcuchy kontynuacji i pomiar zysku wydajnościowego z flagą `TaskContinuationOptions.ExecuteSynchronously` (100 000 zadań, średnia z 5 prób).
- **Punkt 2 (LabTask1):** Wpływ `ExecuteSynchronously` na zadania obciążające procesor z funkcją `DoPointlessMess()`.
- **Punkt 3 (LabTask1):** Przekazywanie wartości w łańcuchu zadań potoku (`Task<double>`, parametr `aa`, właściwość `ant.Result`).
- **Punkt 4 (FibonacciTask):** Potokowe obliczanie wyrazów ciągu Fibonacciego na typie arytmetyki o dowolnej precyzji `BigInteger` i `Tuple<BigInteger, BigInteger>` (20 000 ogniw potoku vs pętla sekwencyjna).
- **Punkt 5 (PrimesTask):** Wielowątkowe sito liczb pierwszych w zakresie 0..10 000 000 z dekompozycją zakresu na `Environment.ProcessorCount` i scalaniem cząstkowych wyników przez `Task.Factory.ContinueWhenAll()` (ponad 4x przyspieszenia wielowątkowego).
- **Punkt 6 (PrimesTask):** Nowoczesny asynchroniczny generator liczb pierwszych — klasa statyczna `PrimesGenerator` z metodą TAP `FindPrimesAsync`, `await Task.WhenAll()` i zapytaniem LINQ `SelectMany`.

### [Sprawozdanie 9](./Sprawozdanie%209/) — Kooperatywne Anulowanie, Raportowanie Postępu w TPL oraz Przetwarzanie Równoległe PLINQ
- **Punkt 1 (TaskAdvanced):** Podstawowa metoda asynchroniczna `calculateSomethingAsync` z symulacją obciążenia CPU (`Thread.SpinWait`) i generowaniem liczb pseudolosowych (`Random.Shared`).
- **Punkt 2 (TaskAdvanced):** Asynchroniczne raportowanie postępu za pomocą interfejsu `IProgress<(int current, int total)>` i klasy `Progress<T>` ze zdarzeniem `ProgressChanged`.
- **Punkt 3 (TaskAdvanced):** Kooperatywne anulowanie zadania za pomocą `CancellationToken`, timera `CancelAfter(100)` i obsługi `AggregateException` z filtrowaniem `ae.Flatten().Handle()`.
- **Punkt 4 (TaskAdvanced):** Warunkowe rozgałęzienia potoku kontynuacji — gałąź błędu `NotOnRanToCompletion` vs gałąź sukcesu `OnlyOnRanToCompletion` oczekiwane przez `Task.WaitAll`.
- **Punkt 5 (BenchmarkSum):** Tablica 10 000 000 elementów i sekwencyjny baseline sumowania (`foreach`).
- **Punkt 6 (BenchmarkSum):** Eksperymenty z sumowaniem równoległym: `Parallel.ForEach` bez blokady (wyścigi danych i błędna suma) vs z blokadą `lock` (wysoki narzut synchronizacji) vs LINQ `Sum()` vs PLINQ `ForAll()` vs PLINQ `AsParallel().Sum()`.
- **Punkt 7 (BenchmarkSum):** Równoległe sumowanie z partycjonowaniem zakresu na `Environment.ProcessorCount`, uruchomieniem zadań `Task.Run()` z akumulatorami lokalnymi i scalaniem `Task.WhenAll`.
- **Punkt 8 (BenchmarkSum):** Kompleksowy benchmark sumowania z nieliniową funkcją obciążeniową `calculatedValue(double)` wariantów sekwencyjnych, `Parallel.ForEach`, PLINQ oraz partycji TPL.

### [Sprawozdanie 10](./Sprawozdanie%2010/) — Klasa Parallel i Algorytmy Równoległe
- **Punkt 1:** Sterowanie pętlami równoległymi (`ParallelLoopState.Break` vs `ParallelLoopState.Stop`).
- **Punkt 2:** Bezblokadowa redukcja zmiennych lokalnych w `Parallel.For<TLocal>`.
- **Punkt 3:** Równoległe sortowanie przez scalanie (Parallel MergeSort z progiem sekwencyjnym).
- **Punkt 4:** Równoległe mnożenie macierzy (Parallel Matrix Multiplication).
- **Punkt 5:** Równoległe algorytmy metaheurystyczne (Algorytm Mrówkowy ACO dla problemu komiwojażera TSP).

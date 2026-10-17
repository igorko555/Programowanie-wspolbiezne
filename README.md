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

### [Sprawozdanie 5-6](./Sprawozdanie%205-6/) — Bezpieczne Kolekcje, Bariery i Wątki w UI
- **Punkt 1–2:** Kolekcje bezpieczne i niebezpieczne wątkowo (`List<T>` vs `ConcurrentBag<T>`, `ConcurrentDictionary<TKey, TValue>`).
- **Punkt 3–4:** Bufor producent-konsument z użyciem `BlockingCollection<T>`.
- **Punkt 5–6:** Bariera synchronizacyjna faz (`Barrier` oraz własna implementacja na `Monitor.Wait` / `PulseAll`).
- **WPFThreads:** Wielowątkowość w aplikacjach okienkowych WPF, unikanie blokowania UI i bezpieczna delegacja przez `Dispatcher.Invoke`.

### [Sprawozdanie 7](./Sprawozdanie%207/) — Task Parallel Library (TPL) i Animacje Wielowątkowe
- **Punkt 1–6:** Podstawy `Task`, `Task.Run`, `Task.WaitAll`, `Task.WhenAll`, obsługa wyjątków `AggregateException`.
- **Punkt 7 (Snow):** Wielowątkowa symulacja opadów śniegu w konsoli z synchronizacją bufora ekranu (`Console.SetCursorPosition`, `ThreadPool`).

### [Sprawozdanie 8-9](./Sprawozdanie%208-9/) — Zaawansowane Zadania Asynchroniczne
- **Punkt 1:** Synchroniczne i asynchroniczne łańcuchy kontynuacji (`ContinueWith`, `ExecuteSynchronously`).
- **Punkt 2:** Równoległe potoki obliczeniowe dużych liczb Fibonacciego (`BigInteger`).
- **Punkt 3:** Łączenie wielu zadań w jedno podsumowujące (`ContinueWhenAll`).
- **Punkt 4:** Anulowanie zadań (`CancellationTokenSource`), raportowanie postępu (`IProgress<T>`) i gałęzie błędów (`TaskContinuationOptions.OnlyOnFaulted`).
- **Punkt 5:** Benchmark sumowania tablicy (Sekwencyjnie vs `Parallel.ForEach` vs PLINQ vs TPL).

### [Sprawozdanie 10](./Sprawozdanie%2010/) — Klasa Parallel i Algorytmy Równoległe
- **Punkt 1:** Sterowanie pętlami równoległymi (`ParallelLoopState.Break` vs `ParallelLoopState.Stop`).
- **Punkt 2:** Bezblokadowa redukcja zmiennych lokalnych w `Parallel.For<TLocal>`.
- **Punkt 3:** Równoległe sortowanie przez scalanie (Parallel MergeSort z progiem sekwencyjnym).
- **Punkt 4:** Równoległe mnożenie macierzy (Parallel Matrix Multiplication).
- **Punkt 5:** Równoległe algorytmy metaheurystyczne (Algorytm Mrówkowy ACO dla problemu komiwojażera TSP).

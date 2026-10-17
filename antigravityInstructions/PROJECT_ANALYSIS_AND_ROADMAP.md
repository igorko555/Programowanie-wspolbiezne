# KOMPLEKSOWY AUDYT ARCHITEKTONICZNY I STRATEGICZNY ROADMAP PROJEKTU
## Programowanie Współbieżne (.NET / C#) — Politechnika Świętokrzyska
**Autor analizy**: Antigravity AI (Senior Systems Architect & Concurrent Systems Analyst)  
**Student**: Namachynskyi Ihor | **Grupa**: 2ID12B | **Nr indeksu**: s097889  
**Data analizy**: 17 października 2026 r.  
**Środowisko docelowe**: .NET 8.0 / .NET 9.0 SDK, C# 12/13, Visual Studio / Visual Studio Code  

---

## 1. Globalny Rejestr Plików Projektu (Global File Registry)

Poniższy rejestr zawiera kompletną inwentaryzację wszystkich artefaktów zlokalizowanych w strukturze katalogów `Sprawozdanie 3` – `Sprawozdanie 10`. Wszystkie oryginalne pliki szablonowe i materiały dydaktyczne zostały zindeksowane z określeniem ich roli technicznej.

### 1.1. Moduł: Sprawozdanie 3 (Delegaty, Zdarzenia i Czasomierze)
*Lokalizacja bazowa*: `PADUCH_Programowanie współbieżne_\Sprawozdanie 3\`
- `Laboratorium 3.pdf` (165 248 B) — Instrukcja laboratoryjna autorstwa dra inż. Pawła Paducha (8 zadań: obsługa delegatów, zdarzeń, wątków potomnych w tle, interfejsu `IDisposable` z `Thread.Interrupt`, czasomierzy `System.Threading.Timer` oraz `System.Timers.Timer`).
- `Wyk 3- Podstawy.net.pdf` (923 522 B) — Wykład wprowadzający do platformy .NET i języka C# (składnia, typy wartościowe/referencyjne, kolekcje, generyki, obsługa wyjątków, mechanizm delegatów `Func`, `Action`, `Predicate`, metody rozszerzające `this`).
- `DotNet8EnCleanCore8.0-прикладові коди\DotNet8EnClean.sln` (2 125 B) — Główna solucja Visual Studio agregująca projekty demonstracyjne w standardzie .NET 8.0.
- `DotNet8EnCleanCore8.0-прикладові коди\SimpleDelegate\SimpleDelegate.csproj` — Plik projektu aplikacji konsolowej dla delegatów jednokrotnych i wielokrotnych.
- `DotNet8EnCleanCore8.0-прикладові коди\SimpleDelegate\Program.cs` (1 018 B) — Kod źródłowy: definicja delegata `PrintSomething`, metody `PrintOryginal`, `PrintInCapital`, wywołania wielokrotne `+=`, inspekcja `GetInvocationList()`.
- `DotNet8EnCleanCore8.0-прикладові коди\SimpleEvent\SimpleEvent.csproj` — Plik projektu aplikacji demonstracyjnej mechanizmu zdarzeń.
- `DotNet8EnCleanCore8.0-прикладові коди\SimpleEvent\Alarm.cs` (3 508 B) — Klasa `Alarm` i `AlarmEventArgs`: wątek sprawdzający `internalTrhead`, pętla czasowa `checkingTime`, wyzwalanie zdarzenia `AlarmOccured`.
- `DotNet8EnCleanCore8.0-прикладові коди\SimpleEvent\Program.cs` (610 B) — Punkt wejścia aplikacji testującej `Alarm`, rejestracja subskrybenta `Alarm_AlarmOccured`.
- `DotNet8EnCleanCore8.0-прикладові коди\TimerExample\TimerExample.csproj` — Plik projektu porównującego czasomierze systemowe i wątkowe.
- `DotNet8EnCleanCore8.0-прикладові коди\TimerExample\Program.cs` (707 B) — Kod źródłowy: współbieżne uruchomienie `System.Threading.Timer` i `System.Timers.Timer`.

### 1.2. Moduł: Sprawozdanie 4 (Prymitywy Synchronizacji: WaitHandles, Lock, Mutex, Semafory)
*Lokalizacja bazowa*: `PADUCH_Programowanie współbieżne_\Sprawozdanie 4\`
- `Instrukcja 4 — Portal dydaktyczny Katedry Systemów Informatycznych Politechniki Świętokrzyskiej w Kielcach.pdf` (610 827 B) — Instrukcja laboratoryjna (problematyka generatorów pseudolosowych `Random` w wątkach, synchronizacja producent-konsument za pomocą `AutoResetEvent` i `ManualResetEvent`, wzorzec spotkań `WaitHandle.SignalAndWait`, bariery wyjścia `WaitHandle.WaitAny` / `WaitAll`, synchronizacja sekcji krytycznych: `lock`, `Mutex`, `Semaphore`, `SemaphoreSlim` oraz benchmark `Stopwatch`).
- `Wyk 4 - Watki w .net.pdf` (832 991 B) — Wykład: model wątkowości CLR, cykl życia wątku (`ThreadState`), wątki pierwszoplanowe vs drugoplanowe (`IsBackground`), priorytety (`ThreadPriority`), metody wstrzymywania (`Thread.Sleep`, `Thread.SpinWait`, `Thread.Join`), sekcja krytyczna, `Monitor.Enter`/`Exit`/`TryEnter`, operacje atomowe `Interlocked`, `SpinLock`, `ContextBoundObject`.
- `DotNet2Clean\DotNet2Clean\DotNet2.sln` (2 624 B) — Solucja grupująca projekty synchronizacji.
- `DotNet2Clean\DotNet2Clean\ProducentKonsument\ProducentKonsument.csproj` & `Program.cs` (2 308 B) — Szablon: 10 wątków konsumentów i 1 wątek producenta modyfikujących wspólny `licznik` z błędami wyścigu i kolizjami `Random`.
- `DotNet2Clean\DotNet2Clean\Spotkania\Spotkania.csproj` & `Program.cs` (1 531 B) — Szablon: dwa wątki realizujące zsynchronizowane cykle iteracyjne za pomocą `WaitHandle.SignalAndWait`.
- `DotNet2Clean\DotNet2Clean\SynchroWait\SynchroWait.csproj` & `Program.cs` (1 508 B) — Szablon: 10 wątków roboczych z punktami synchronizacji "doczekaliśmy się pierwszego" oraz "doczekaliśmy się wszystkich".
- `DotNet2Clean\DotNet2Clean\MuteksyIReszta\MuteksyIReszta.csproj` & `Program.cs` (1 162 B) — Szablon: wyścig 100 wątków inkrementujących `licznik` $10\,000$ razy bez zabezpieczeń (oczekiwany wynik $1\,000\,000$, rzeczywisty zaniżony).

### 1.3. Moduł: Sprawozdanie 5-6 (Kolekcje Współbieżne, Bariery Algorytmu Genetycznego i GUI WPF)
*Lokalizacja bazowa*: `PADUCH_Programowanie współbieżne_\Sprawozdanie 5-6\`
- `instrukcja 5— Portal dydaktyczny Katedry Systemów Informatycznych Politechniki Świętokrzyskiej w Kielcach.pdf` (520 894 B) — Instrukcja do cz. 1: wyścigi w kolekcjach `List<T>`, synchronizacja blokadą `lock`, zastosowanie `ConcurrentBag<T>`, gwarantowanie unikalności kluczy za pomocą `ConcurrentDictionary<TKey, TValue>`, kolejka `Queue<T>` vs `ConcurrentQueue<T>` z `TryDequeue`, synchronizacja kolejki semaforem, wzorzec producent-konsument na bazie `BlockingCollection<T>`.
- `insstrukcja 6 Bariery oraz GUI — Portal dydaktyczny Katedry Systemów Informatycznych Politechniki Świętokrzyskiej w Kielcach.pdf` (374 811 B) — Instrukcja do cz. 2: równoległa ewaluacja populacji algorytmu genetycznego z barierą fazową (`System.Threading.Barrier`, `SignalAndWait`), emulacja bariery za pomocą `Monitor.Wait` / `PulseAll`, aplikacja WPF z buforem `BlockingCollection` i bezpieczną aktualizacją kontrolek UI przez `Dispatcher.Invoke`.
- `Wyk 5-6 -  WaitHandles, kolekcje i bariery.pdf` (832 991 B) — Wykład: szczegóły `WaitHandle`, `AutoResetEvent`/`ManualResetEvent`, wzorzec `Ready-Go`, kolejka producent-konsument, `Barrier`, `CountdownEvent`, emulacja barier monitorem, kolekcje bezpieczne wątkowo (`ConcurrentBag`, `ConcurrentDictionary`, `BlockingCollection`, warianty `ProducerConsumerQ1`–`Q4`).
- `DotNet3EnClean\DotNet3EnClean\DotNet3EnClean.sln` (2 161 B) — Solucja Visual Studio dla kolekcji współbieżnych.
- `DotNet3EnClean\DotNet3EnClean\UnsafeCollection\UnsafeCollection.csproj` & `Program.cs` (993 B) — Szablon: niebezpieczne wielowątkowe dodawanie elementów do `List<int>`.
- `DotNet3EnClean\DotNet3EnClean\UniqueCollection\UniqueCollection.csproj`, `Program.cs` (1 599 B) & `Extensions.cs` (850 B) — Szablon: badanie unikalności losowanych elementów w `ConcurrentBag` oraz metoda rozszerzająca `IsUnique()`.
- `DotNet3EnClean\DotNet3EnClean\ProducerConsumerQ\ProducerConsumerQ.csproj` & `Program.cs` (1 717 B) — Szablon: producent i konsumenci na bazie niesynchronizowanej `Queue<int>` rzucającej wyjątki przy pustym buforze.
- `DotNet4EnClean\DotNet4EnClean\DotNet4En.sln` (1 614 B) — Solucja Visual Studio dla barier i WPF.
- `DotNet4EnClean\DotNet4EnClean\Barriers\Barriers.csproj`, `Program.cs` (1 140 B) & `GAHelper.cs` (5 801 B) — Szablon: algorytm genetyczny (sekwencyjna ewaluacja osobników, krzyżowanie bitowe `CrossBytes`, selekcja rangowa `GetRandomSpecimen`, mutacja bitowa `Mutation`).
- `DotNet4EnClean\DotNet4EnClean\WPFThreads\WPFThreads.csproj`, `MainWindow.xaml` (3 201 B) & `MainWindow.xaml.cs` (6 104 B) — Szablon aplikacji WPF: producent i konsumenci na bazie `BlockingCollection<int>`, suwaki opóźnień, brakująca bezpieczna komunikacja z UI (`Dispatcher`).

### 1.4. Moduł: Sprawozdanie 7 (Task Parallel Library (TPL) — Podstawy Zadań i Projekt Snow)
*Lokalizacja bazowa*: `PADUCH_Programowanie współbieżne_\Sprawozdanie 7\`
- `instrukcja 7— Portal dydaktyczny Katedry Systemów Informatycznych Politechniki Świętokrzyskiej w Kielcach.pdf` (511 204 B) — Instrukcja laboratoryjna "Laboratorium Tasks - Pierwsze laboratorium z Taskami w C#" (tworzenie zadań `Task`, delegaty akcji, przekazywanie parametrów prostych, krotek `ValueTuple`, obiektów klas własnych `MyParam`, fabryka `Task.Factory.StartNew`, wielowątkowa animacja konsolowa śniegu `Star`, zarządzanie pulą wątków `ThreadPool.SetMinThreads`, generator `Random.Shared`, responsywność okna konsoli).
- `Wyk 7 -  Wątki w aplikacjach okienkowych.pdf` (984 102 B) — Wykład: wątkowość w WinForms i WPF, błąd `InvalidOperationException` (Cross-thread), właściwość `Control.InvokeRequired`, metody `Control.Invoke`, klasa wrapper `ThreadSafeCalls`, kontrolka `LabelSafe`, `SynchronizationContext` (`Send`, `Post`), komponent `BackgroundWorker`, mechanizm `Dispatcher.CheckAccess()`, `Dispatcher.Invoke()`, zamrażanie obiektów `Freezable.Freeze()`, model STA (*Single-Threaded Apartment*).

### 1.5. Moduł: Sprawozdanie 8-9 (Zaawansowane Aspekty TPL, Kontynuacje, Anulowanie, PLINQ)
*Lokalizacja bazowa*: `PADUCH_Programowanie współbieżne_\Sprawozdanie 8-9\`
- `instrukcja 8— Portal dydaktyczny Katedry Systemów Informatycznych Politechniki Świętokrzyskiej w Kielcach.pdf` (489 120 B) — Instrukcja do cz. 1 "Laboratorium Task 2" (łańcuchy kontynuacji `ContinueWith`, flaga `TaskContinuationOptions.ExecuteSynchronously`, obciążenie procesora `DoPointlessMess`, potokowe wyliczanie ciągu Fibonacciego na `BigInteger` i krotkach, zrównoleglone wyszukiwanie liczb pierwszych w oparciu o `Environment.ProcessorCount`, łączenie wyników `Task.Factory.ContinueWhenAll`, wzorzec `async/await` z `Task.WhenAll` w klasie `PrimesGenerator`).
- `instrukcja 9 — Portal dydaktyczny Katedry Systemów Informatycznych Politechniki Świętokrzyskiej w Kielcach.pdf` (492 810 B) — Instrukcja do cz. 2 "Laboratorium Task 3" (asynchroniczna funkcja obliczeniowa `calculateSomethingAsync`, raportowanie postępu za pomocą `IProgress<(int, int)>` i `Progress<T>`, kooperatywne anulowanie `CancellationTokenSource.CancelAfter(5000)`, obsługa `OperationCanceledException` i `AggregateException.Flatten()`, rozgałęzienia warunkowe `OnlyOnFaulted` vs `OnlyOnRanToCompletion`, wielowątkowe sumowanie tablicy 10 mln liczb: pętla sekwencyjna, `Parallel.ForEach`, `Parallel.ForEach` z `lock`, LINQ `.Sum()`, PLINQ `.AsParallel().ForAll()`, PLINQ `.AsParallel().Sum()`, podział na zadania `Task`, równoległa kalkulacja funkcji nieliniowej `calculatedValue`).
- `Wyk 8,9 -  Task.pdf` (1 345 812 B, 85 slajdów) — Wykład: kompendium wzorca TAP (*Task-based Asynchronous Pattern*), porównanie z modelami historycznymi APM i EAP, typy `Task`, `Task<TResult>`, `ValueTask`, cykl stanów zadania (`Created`, `WaitingForActivation`, `WaitingToRun`, `Running`, `RanToCompletion`, `Canceled`, `Faulted`), tworzenie i startowanie (`Task.Run` vs `Task.Factory.StartNew`), przekazywanie stanu a pułapki domknięć zmiennych w pętli (*closure capture*), kontynuacje jedno- i wieloargumentowe (`ContinueWith`, `ContinueWhenAll`, `ContinueWhenAny`), zaawansowane flagi `TaskContinuationOptions`, propagacja wyjątków `AggregateException`, planiści zadań `TaskScheduler` (`Default` vs `FromCurrentSynchronizationContext`).

### 1.6. Moduł: Sprawozdanie 10 (Zaawansowane Zrównoleglanie Pętli, Klasa Parallel i Algorytmy Współbieżne)
*Lokalizacja bazowa*: `PADUCH_Programowanie współbieżne_\Sprawozdanie 10\`
- `Wyk 10 -  klasa Parallel.pdf` (520 114 B, 29 slajdów) — Wykład: architektura klasy `System.Threading.Tasks.Parallel`, metody `Parallel.For`, `Parallel.ForEach`, `Parallel.Invoke`, warianty asynchroniczne `Parallel.ForAsync` / `Parallel.ForEachAsync`, sterowanie przebiegiem pętli: `ParallelLoopState.Break()` vs `ParallelLoopState.Stop()`, monitorowanie stanu pętli `ParallelLoopResult` (`IsCompleted`, `LowestBreakIteration`), konfiguracja `ParallelOptions` (`MaxDegreeOfParallelism`, `CancellationToken`, własny `TaskScheduler`), analiza alokacji pamięci `ValueTask` vs `Task`, redukcja wielowątkowa z lokalnym stanem wątku (`Parallel.For<TLocal>` z inicjalizacją akumulatora i finalną synchronizacją blokadą — optymalizacja drastycznie redukująca narzut synchronizacji).
- `Wyk 10 -  Algorytmy.pdf` (680 415 B, 56 slajdów) — Wykład: klasyczne algorytmy współbieżne i rozproszone:
  1. Współbieżny MergeSort (*Sortowanie przez scalanie*): dekompozycja drzewiasta, warunki odcięcia głębokości wątków, scalanie podtablic.
  2. Sortowanie oscylacyjne (*Oscillation Sort*): siatka procesów $A_i$ i $B_i$, wymiana sąsiedzka liczb, złożoność $2n$ cykli.
  3. Równoległe mnożenie macierzy: dekompozycja blokowa "dziel i rządź", algorytm Strassena ($O(n^{2.807})$), algorytm Cannona na siatce procesorów $m \times m$ (rotacja wierszy macierzy A i kolumn macierzy B), uogólnienie na tensory 3D.
  4. Równoległy Algorytm Genetyczny: topologie Master-Slave oraz P2P (model wyspowy), operatory genetyczne (ruletka, turniej, ranking, krzyżowanie 1-punktowe, PMX, OX, mutacje bitowe).
  5. Równoległy Algorytm Mrówkowy (*Ant Colony Optimization* — ACO): stygmergia, parowanie feromonu ($\tau_{ij} = \tau_{ij} \cdot \rho$), wzór prawdopodobieństwa wyboru krawędzi $p_{ij}^k(t) = \frac{[\tau_{ij}(t)]^\alpha [\eta_{ij}]^\beta}{\sum [\tau_{il}(t)]^\alpha [\eta_{il}]^\beta}$, model *Cycle Ant System* (CAS) dla problemu komiwojażera (TSP).

---

## 2. Szczegółowa Analiza Merytoryczna wg Laboratoriów (3 – 10)

### Rozdział: Sprawozdanie 3 — Delegaty, Zdarzenia i Czasomierze
* **Artefakty w folderze**:
  - Instrukcja: `Laboratorium 3.pdf`
  - Wykład: `Wyk 3- Podstawy.net.pdf`
  - Kody bazowe: `SimpleDelegate/Program.cs`, `SimpleEvent/Alarm.cs`, `SimpleEvent/Program.cs`, `TimerExample/Program.cs`
* **Głęboka analiza zadań i powiązanie z teorią wykładową**:
  - **Zadanie 1 (Prosty delegat, 1.0 pkt)**: Zdefiniować delegat typu `void` przyjmujący dwa parametry `int`. Utworzyć cztery metody realizujące: sumę ($a+b$), różnicę ($a-b$), iloczyn ($a \cdot b$) oraz iloraz ($a/b$ z zabezpieczeniem przed dzieleniem przez zero). Zbudować łańcuch wywołań (*multicast delegate*) za pomocą operatora `+=`. Wywołać delegat i w pętli `foreach` wylistować nazwy zarejestrowanych metod za pośrednictwem `GetInvocationList()`.
    - *Baza teoretyczna*: Wykład 3, slajdy 44–45 (sygnatura delegata, wiązanie metod statycznych i instancyjnych, agregacja delegatów).
    - *Kod bazowy*: `SimpleDelegate\Program.cs` operujący dotychczas na `PrintSomething(string)`.
    - *Plan zmian*: Definicja `delegate void MathOp(int a, int b);`, implementacja metod arytmetycznych, iteracja po `Method.Name`.
  - **Zadanie 2 (Kończenie v1, 0.5 pkt)**: Analiza zachowania programu `SimpleEvent`, który nie kończy działania po naciśnięciu klawisza ze względu na aktywny wątek pierwszoplanowy (`IsBackground = false`). W metodzie `startClock()` klasy `Alarm` należy wymusić `internalTrhead.IsBackground = true`.
    - *Baza teoretyczna*: Wykład 4, slajdy 12–13 (wątki pierwszoplanowe blokują zakończenie procesu CLR; wątki w tle ulegają natychmiastowej terminacji wraz z wątkiem głównym).
    - *Kod bazowy*: `SimpleEvent\Alarm.cs`, linie 82–89.
    - *Plan zmian*: Ustawienie flagi `internalTrhead.IsBackground = true` przed wywołaniem `Start()`.
  - **Zadanie 3 (Kończenie v2, 1.0 pkt)**: Implementacja bezpiecznego zakończenia wątku roboczego. Wycofanie zmiany z Zadania 2 (`IsBackground = false`). Klasa `Alarm` ma implementować interfejs `IDisposable`. W metodzie `Dispose()` wywołać `internalTrhead.Interrupt()`, a następnie przypisać referencji wartość `null`. Wątek w metodzie `checkingTime` musi przechwytywać wyjątek `ThreadInterruptedException` i dokonać czyszczenia zasobów.
    - *Baza teoretyczna*: Wykład 4, slajd 41 (użycie `Thread.Interrupt` do przerwania stanu `WaitSleepJoin` rzucając `ThreadInterruptedException`).
    - *Kod bazowy*: `SimpleEvent\Alarm.cs` i `SimpleEvent\Program.cs`.
    - *Plan zmian*: Dodanie `: IDisposable`, implementacja `Dispose()`, blok `try-catch (ThreadInterruptedException)` w pętli `checkingTime()`, wywołanie `alarm.Dispose()` w `Main()`.
  - **Zadanie 4 (Parametryzacja checkingInterval, 0.5 pkt)**: Przeciążenie konstruktorów klasy `Alarm` oraz metody `SetAlarm` o parametr `int checkingInterval` (domyślnie $1000$ ms).
    - *Kod bazowy*: `SimpleEvent\Alarm.cs`, linie 45–68 oraz 104–120.
    - *Plan zmian*: Rozszerzenie sygnatur konstruktorów i metod o opcjonalny parametr interwału.
  - **Zadanie 5 (Nowy Event postępu czasu, 1.0 pkt)**: Rozszerzenie klasy `Alarm` o aktualizację licznika pozostałego czasu `timespanToAlarm` w każdej iteracji sprawdzania, dodanie właściwości tylko do odczytu `TimeSpan RemainingTime` oraz nowego zdarzenia wyzwalanego przy każdym takcie sprawdzania przekazującego pozostały czas. Subskrypcja zdarzenia w programie głównym.
    - *Baza teoretyczna*: Wykład 3, slajdy 19–20 (`TimeSpan`, `DateTime`) oraz Wykład 7, slajdy 4–7 (architektura zdarzeń i własnych klas `EventArgs`).
    - *Kod bazowy*: `SimpleEvent\Alarm.cs`.
    - *Plan zmian*: Nowa klasa argumentów zdarzenia, delegat, właściwość publiczna i wywołanie `Tick?.Invoke(...)` wewnątrz pętli sprawdzającej.
  - **Zadanie 6 (TimerExample i stan atomowy, 0.5 pkt)**: Konfiguracja `System.Timers.Timer` jako czasomierza jednokrotnego (`AutoReset = false`). Zastąpienie prostej metody zwrotnej w `System.Threading.Timer` dedykowaną klasą stanu `TimerState` z polem `public int Counter` oraz metodą `TimerTask(object state)` inkrementującą licznik atomowo za pomocą `Interlocked.Increment(ref state.Counter)`.
    - *Baza teoretyczna*: Wykład 4, slajdy 17–18 oraz slajd 29 (użycie `Interlocked`).
    - *Kod bazowy*: `TimerExample\Program.cs`.
    - *Plan zmian*: Definicja klasy `TimerState`, metoda `TimerTask`, podmiana delegata zwrotnego.
  - **Zadanie 7 (Wyłączenie czasomierza, 0.5 pkt)**: Dynamiczne zatrzymanie czasomierza wątkowego po 5 cyklach wywołań (gdy `Counter == 4`) poprzez wywołanie `timerThreading.Change(Timeout.Infinite, 0)`.
    - *Baza teoretyczna*: Wykład 4, slajd 18 (metoda `Timer.Change`).
    - *Plan zmian*: Warunek wewnątrz metody zwrotnej sprawdzający stan licznika i rekonfigurujący interwały czasomierza.

---

### Rozdział: Sprawozdanie 4 — Mechanizmy Synchronizacji Wątków (.NET WaitHandles i Sekcje Krytyczne)
* **Artefakty w folderze**:
  - Instrukcja: `Instrukcja 4 — Portal dydaktyczny...pdf`
  - Wykład: `Wyk 4 - Watki w .net.pdf`
  - Kody bazowe: `ProducentKonsument\Program.cs`, `Spotkania\Program.cs`, `SynchroWait\Program.cs`, `MuteksyIReszta\Program.cs`
* **Głęboka analiza zadań i powiązanie z teorią wykładową**:
  - **Zadanie 2 (Analiza problemu liczb pseudolosowych, 1.0 pkt)**: Zdiagnozowanie anomalii w projekcie `ProducentKonsument`. Identyczność sekwencji liczb losowanych przez wątki przy domyślnym konstruktorze `new Random()` (ten sam seed oparty na `Environment.TickCount`). Przetestowanie seeda opartego na `ManagedThreadId` oraz współdzielonego obiektu `Random` (który nie jest *thread-safe* i w warunkach wyścigu ulega wewnętrznemu uszkodzeniu generując wyłącznie zera). Wykazanie gubienia i dublowania wartości wspólnej zmiennej `licznik`.
    - *Baza teoretyczna*: Wykład 4, slajdy 4, 23 (brak izolacji i brak synchronizacji pamięci dzielonej).
    - *Kod bazowy*: `ProducentKonsument\Program.cs`, linie 20–39.
  - **Zadanie 3 (Synchronizacja WaitHandles: AutoResetEvent, 1.0 pkt)**: Wprowadzenie dwóch obiektów synchronizacyjnych: `static EventWaitHandle pisanie = new AutoResetEvent(true);` oraz `static EventWaitHandle czytanie = new AutoResetEvent(false);`. Zapewnienie naprzemiennego, unikalnego zapisu i odczytu wartości `licznik` (producent czeka na pozwolenie na zapis, konsumenci czekają na pozwolenie na odczyt).
    - *Baza teoretyczna*: Wykład 4 oraz Wykład 5-6, slajdy 3–4 (semantyka bramki jednoelementowej: `WaitOne()` blokuje i automatycznie zamyka bramkę, `Set()` przepuszcza dokładnie jeden oczekujący wątek).
    - *Plan zmian*: Sekwencja `pisanie.WaitOne()` $\to$ inkrementacja $\to$ `czytanie.Set()` u producenta; `czytanie.WaitOne()` $\to$ odczyt $\to$ `pisanie.Set()` u konsumenta.
  - **Zadanie 4 (ManualResetEvent i analiza wyścigów, 1.0 pkt)**: Zamiana `AutoResetEvent` na `ManualResetEvent` z ręcznym wywołaniem `Reset()` natychmiast po `WaitOne()`. Obserwacja nieprawidłowości: po otwarciu bramki metodą `Set()` wiele wątków jednocześnie przechodzi przez `WaitOne()` zanim którykolwiek zdąży wykonać `Reset()`. Zaproponowanie i implementacja poprawnego mechanizmu (np. dodatkowy rygiel `lock` wokół pary `WaitOne()` i `Reset()` lub przejście na kolejkowanie).
    - *Baza teoretyczna*: Wykład 5-6, slajd 11 (`ManualResetEvent` pozostaje otwarty dla wszystkich wątków do momentu jawnego wywołania `Reset()`).
  - **Zadanie 5 (Spotkania — Wzorzec Rendezvous, 0.1 pkt)**: Projekt `Spotkania`. Dwa wątki wykonujące pętle o różnym czasie uśpienia. Użycie atomowej metody `WaitHandle.SignalAndWait(wh1, wh2)` oraz `WaitHandle.SignalAndWait(wh2, wh1)` w celu zsynchronizowania kroków obu wątków (żaden wątek nie przejdzie do kolejnej iteracji, dopóki drugi nie dotrze do punktu spotkania).
    - *Baza teoretyczna*: Wykład 5-6, slajd 16 (`SignalAndWait` jako wzorzec barierowy dla pary wątków).
    - *Kod bazowy*: `Spotkania\Program.cs`, linie 24 i 33.
  - **Zadanie 6 (SynchroWait — WaitAny i WaitAll, 1.0 pkt)**: Projekt `SynchroWait`. Utworzenie tablicy uchwytów `WaitHandle[] handles` przypisanych do każdego z 10 wątków. Użycie `WaitHandle.WaitAny(handles)` do natychmiastowego wyświetlenia komunikatu "Doczekaliśmy się pierwszego" (po zakończeniu pracy pierwszego wątku) oraz `WaitHandle.WaitAll(handles)` do wyświetlenia komunikatu "Doczekaliśmy się wszystkich".
    - *Baza teoretyczna*: Wykład 5-6, slajd 17 (`WaitAny` odblokowuje przy pierwszym sygnale; `WaitAll` oczekuje na sygnały ze wszystkich uchwytów).
    - *Kod bazowy*: `SynchroWait\Program.cs`.
  - **Zadanie 7 (Benchmark: Lock, Mutex, Semaphore, SemaphoreSlim, 0.9 pkt)**: Projekt `MuteksyIReszta`. Rozwiązanie problemu wyścigu przy operacji `licznik++` ($100$ wątków $\times$ $10\,000$ iteracji). Implementacja 5 wariantów i pomiar precyzyjnego czasu za pomocą `Stopwatch`:
    1. Bez synchronizacji (błędny wynik końcowy, czas bazowy).
    2. `lock (locker)` (optymalna blokada wewnątrzprocesowa CLR).
    3. `Mutex` (ciężki uchwyt systemowy jądra systemu operacyjnego — bardzo wolny).
    4. `Semaphore(1, 1)` (ogólny semafor systemowy).
    5. `SemaphoreSlim(1, 1)` (lekki semafor użytkownika dedykowany dla jednego procesu).
    - *Baza teoretyczna*: Wykład 4, slajdy 23–35 (porównanie kosztów przełączania kontekstu w jądrze dla `Mutex`/`Semaphore` w stosunku do lekkich prymitywów `lock`/`Monitor` i `SemaphoreSlim`).

---

### Rozdział: Sprawozdanie 5-6 — Kolekcje Współbieżne, Bariery Algorytmu Genetycznego i GUI WPF
* **Artefakty w folderze**:
  - Instrukcje: `instrukcja 5— Portal dydaktyczny...pdf`, `insstrukcja 6 Bariery oraz GUI...pdf`
  - Wykład: `Wyk 5-6 -  WaitHandles, kolekcje i bariery.pdf`
  - Kody bazowe: `DotNet3EnClean` (`UnsafeCollection`, `UniqueCollection`, `ProducerConsumerQ`), `DotNet4EnClean` (`Barriers`, `WPFThreads`)
* **Głęboka analiza zadań i powiązanie z teorią wykładową**:
  - **Zadanie Lab 5.2–5.4 (Niebezpieczna kolekcja vs ConcurrentBag vs lock, 1.5 pkt)**: 100 wątków dodaje po $10\,000$ liczb do `List<int>`. W wersji bez synchronizacji dochodzi do uszkodzenia wewnętrznych tablic listy (`ArgumentException`, utrata elementów, `Count` $\ll 1\,000\,000$). Porównanie narzutu czasowego i poprawności pomiędzy `ConcurrentBag<int>` (struktura *thread-safe* bez blokady globalnej) a `lock (locker) { list.Add(...); }`.
    - *Baza teoretyczna*: Wykład 5-6, slajdy 21–23.
    - *Kod bazowy*: `UnsafeCollection\Program.cs`.
  - **Zadanie Lab 5.5–5.6 (Unikalność: ConcurrentBag vs ConcurrentDictionary, 1.0 pkt)**: Projekt `UniqueCollection`. Wykazanie, że konstrukcja `while (numbers.Any(x => x == number)) { ... } numbers.Add(number);` na `ConcurrentBag` NIE zapewnia unikalności ze względu na przerwę pomiędzy sprawdzeniem warunku (*check*) a dodaniem (*act*) — klasyczny wyścig TOCTOU (*Time-of-Check to Time-of-Use*).
    - *Rozwiązanie 1*: Ochrona całej sekcji sprawdzania i dodawania blokadą `lock (locker)`.
    - *Rozwiązanie 2*: Zastosowanie `ConcurrentDictionary<int, byte>` z metodą atomową `TryAdd(number, 0)` w projekcie `KolekcjaDictionary`, eliminując konieczność jawnego ryglowania.
    - *Baza teoretyczna*: Wykład 5-6, slajdy 24–27.
    - *Kod bazowy*: `UniqueCollection\Program.cs` i `Extensions.cs`.
  - **Zadanie Lab 5.7 (Producent-Konsument na Queue vs ConcurrentQueue vs Semaphore, 1.5 pkt)**: Analiza projektu `ProducerConsumerQ`. Standardowa `Queue<int>.Dequeue()` rzuca wyjątki przy pustym buforze.
    - Krok 1: Przejście na `ConcurrentQueue<int>` z pętlą ponowień `while (!cq.TryDequeue(out item)) Thread.Sleep(...)` (omówienie wady aktywnego/półaktywnego czekania).
    - Krok 2: Wprowadzenie `SemaphoreSlim` sygnalizującego liczbę dostępnych elementów w kolejce (konsument wykonuje `sem.Wait()`, producent po `Enqueue()` wykonuje `sem.Release()`), eliminując puste przebiegi.
    - *Baza teoretyczna*: Wykład 5-6, slajdy 30–37.
    - *Kod bazowy*: `ProducerConsumerQ\Program.cs`.
  - **Zadanie Lab 5.8 (Producent-Konsument na BlockingCollection, 1.0 pkt)**: Implementacja ograniczonego bufora FIFO o pojemności $10$ elementów za pomocą `BlockingCollection<int>(10)`. Wykorzystanie metod blokujących: `Add()` (automatycznie usypia producenta przy pełnym buforze) oraz `Take()` (automatycznie usypia konsumentów przy pustym buforze), a także `CompleteAdding()`.
    - *Baza teoretyczna*: Wykład 5-6, slajdy 28–29.
  - **Zadanie Lab 6.1 (Bariery w Algorytmie Genetycznym, 2.5 pkt)**: Projekt `Barriers`. Równoległa ewaluacja populacji 16 osobników (`specimens`).
    - Utworzenie 16 dedykowanych wątków roboczych powoływanych **tylko raz** przed wejściem do pętli pokoleń.
    - Zastosowanie `System.Threading.Barrier` do synchronizacji etapów.
    - *Kluczowa decyzja architektoniczna*: **Jedna bariera nie wystarczy**. Wymagane są **dwie bariery** w każdej iteracji algorytmu:
      1. *Bariera 1*: Wątki robocze kończą obliczanie `Evaluate(specimens[id])` dla swojego osobnika i meldują się na barierze. Po osiągnięciu bariery wątek główny (lub akcja post-phase) wykonuje selekcję rangową, krzyżowanie i mutację nowej populacji.
      2. *Bariera 2*: Wątki robocze oczekują na zakończenie generowania nowego pokolenia przez wątek główny, zapobiegając rozpoczęciu ewaluacji niegotowych jeszcze danych.
    - *Baza teoretyczna*: Wykład 5-6, slajd 20 (`System.Threading.Barrier`, `SignalAndWait`).
    - *Kod bazowy*: `Barriers\Program.cs` i `GAHelper.cs`.
  - **Zadanie Lab 6.2 (Emulacja Bariery Monitorem, 1.0 pkt)**: Projekt `Monitory`. Zastąpienie klasy `Barrier` własną konstrukcją barierową opartą na `Monitor.Wait(lockObj)` i `Monitor.PulseAll(lockObj)` z atomowym licznikiem wątków meldujących się w danej fazie.
    - *Baza teoretyczna*: Wykład 5-6, slajd 19 (`printStringWaitPulse` z `callCounter`, `Monitor.Wait` i `Monitor.PulseAll`).
  - **Zadanie Lab 6.3 (Wątki w GUI WPF, 1.5 pkt)**: Projekt `WPFThreads`. Zapewnienie thread-safe aktualizacji interfejsu graficznego:
    - Aktualizacja etykiety `collectionAmountLabel` o bieżącą liczbę elementów `buffer.Count`.
    - Dopisanie komunikatów producenta i konsumenta do kontrolek `RichTextBox` (`producerOutput`, `consumerOutput`).
    - Wykorzystanie dyspozytora okna: `Dispatcher.Invoke(() => { ... })` lub `Dispatcher.BeginInvoke(...)`.
    - Dynamiczna regulacja prędkości pracy wątków suwakami `producerDelaySlider` i `consumerDelaySlider`.
    - *Baza teoretyczna*: Wykład 7, slajdy 33–36 (`Dispatcher.CheckAccess()`, `Dispatcher.Invoke()`, priorytety `DispatcherPriority`).
    - *Kod bazowy*: `WPFThreads\MainWindow.xaml.cs`, linie 135, 137, 166, 168.

---

### Rozdział: Sprawozdanie 7 — Task Parallel Library (TPL): Podstawy i Projekt Snow
* **Artefakty w folderze**:
  - Instrukcja: `instrukcja 7— Portal dydaktyczny...pdf` (Laboratorium Tasks 1)
  - Wykład: `Wyk 7 -  Wątki w aplikacjach okienkowych.pdf`
  - Wykład powiązany tematycznie: `Wyk 8,9 -  Task.pdf`
* **Głęboka analiza zadań i powiązanie z teorią wykładową**:
  - **Zadanie 1 (Pierwsze Taski, 0.5 pkt)**: Utworzenie zadania `Task t1 = new Task(...)`, uruchomienie `t1.Start()`. Diagnoza problemu: wątek główny kończy pracę przed zakończeniem zadania asynchronicznego. Rozwiązanie: jawne wywołanie `t1.Wait()`.
    - *Baza teoretyczna*: Wykład 8-9, slajd 44 (jawne oczekiwanie na zadanie metodą `Wait()`).
  - **Zadanie 2 (Task z akcji nazwanej, 0.5 pkt)**: Implementacja metody `static void task2()` z formatowaniem `[i]`. Utworzenie instancji `Task t2 = new Task(task2)`, natychmiastowe uruchomienie i sekwencyjne oczekiwanie `t1.Wait(); t2.Wait();`.
  - **Zadanie 3 (Przekazywanie parametru stanu, 0.5 pkt)**: Akcja `static void task3(object o)` rzutująca parametr do `int` jako limit pętli, formatowanie `(i)`. Utworzenie `Task t3 = new Task(task3, 15)`.
    - *Baza teoretyczna*: Wykład 8-9, slajd 49 (przekazywanie stanu przez parametr `object state` w konstruktorze `Task`).
  - **Zadanie 4 (Przekazywanie krotek ValueTuple, 0.5 pkt)**: Akcja `static void task4(object arg)` rozpakowująca `(int id, string name) tupleArg = ((int, string))arg`. Uruchomienie zadań `t4` oraz `t4b` z przekazaniem literału krotki `(4, "Task4b")`. Oczekiwanie `t4.Wait()`.
    - *Baza teoretyczna*: Wykład 8-9, slajd 37 (zastosowanie krotek w parametrach asynchronicznych).
  - **Zadanie 5 (Przekazywanie obiektów klas własnych, 0.5 pkt)**: Utworzenie klasy `public class MyParam { public string Name { get; set; } public int Id { get; set; } }`. Akcja `task5(object o)` rzutująca do `MyParam`. Uruchomienie `t5` i oczekiwanie `t5.Wait()`.
  - **Zadanie 6 (Dynamiczna lista zadań i fabryka, 0.5 pkt)**: Utworzenie kolekcji `List<Task> t5list`. W pętli uruchomienie 5 zadań za pomocą `Task.Factory.StartNew(task5, new MyParam { ... })`. Synchronizacja zbiorcza za pomocą pętli `foreach (var t in t5list) t.Wait();` lub `Task.WaitAll(t5list.ToArray())`.
    - *Baza teoretyczna*: Wykład 8-9, slajdy 46–48 (`Task.Factory.StartNew` vs `new Task()`).
  - **Zadanie 7 (Projekt Snow — Animacja opadających płatków, 0.5 pkt)**: Projekt `Snow`. Metoda `Star(object o)` symulująca ruch płatka śniegu w konsoli z zawijaniem krawędzi. Wykorzystanie `lock (Console.Out)` dla zapewnienia niepodzielności operacji `Console.SetCursorPosition`, zmiany koloru i zapisu znaku `'*'`. Uruchomienie 50 współbieżnych zadań `Task` i oczekiwanie przez `Task.WaitAll`.
  - **Zadanie 8 (Optymalizacja puli wątków — Więcej śniegu, 0.5 pkt)**: Diagnoza dławienia puli wątków (*thread pool starvation*) przy gwałtownym starcie 50 zadań. Inspekcja `Thread.CurrentThread.ManagedThreadId`. Zwiększenie początkowej liczby wątków roboczych w puli CLR za pomocą `ThreadPool.SetMinThreads(64, 64)`. Dodanie losowego rozrzutu startu (0–100 ms).
  - **Zadanie 9 (Parametryzacja gwiazdek, 0.5 pkt)**: Klasa `StarParameter` z polami `Random rand`, `int iterations`, `ConsoleColor? color`. Przekazywanie sparametryzowanych obiektów do każdego zadania `Star`.
  - **Zadanie 10 (Bezpieczny generator losowy i responsywność konsoli, 0.5 pkt)**: Wykorzystanie nowoczesnego, bezpiecznego wątkowo generatora `Random.Shared` (wprowadzonego w .NET 6+). Dynamiczne odczytywanie parametrów konsoli `Console.WindowWidth` i `Console.WindowHeight` w każdej iteracji pętli opadania, co umożliwia płynne dostosowanie animacji do zmiany rozmiaru okna przez użytkownika.

---

### Rozdział: Sprawozdanie 8-9 — Zaawansowane Aspekty TPL, Kontynuacje, Anulowanie i PLINQ
* **Artefakty w folderze**:
  - Instrukcje: `instrukcja 8— Portal dydaktyczny...pdf` (Task 2), `instrukcja 9 — Portal dydaktyczny...pdf` (Task 3)
  - Wykład: `Wyk 8,9 -  Task.pdf`
* **Głęboka analiza zadań i powiązanie z teorią wykładową**:
  - **Zadanie Lab 8.1–8.3 (Łańcuchy kontynuacji i ExecuteSynchronously, 1.5 pkt)**:
    - Projekt `LabTask1`. Wygenerowanie sekwencji $1\,000\,000$ kontynuacji zadań `task.ContinueWith(..., TaskContinuationOptions.ExecuteSynchronously)`.
    - Pomiar czasu alokacji oraz czasu wykonania w trybie Release (Ctrl+F5).
    - Porównanie z domyślnym harmonogramowaniem (bez `ExecuteSynchronously`). Opcja ta wymusza wykonanie zadania kontynuacji na tym samym wątku, który zakończył zadanie poprzedzające, eliminując drastycznie narzut ponownego kolejkowania w `ThreadPool`.
    - Zadanie z obciążeniem `DoPointlessMess()` oraz przekazywaniem danych wejściowych/wyjściowych pomiędzy ogniwami łańcucha.
    - *Baza teoretyczna*: Wykład 8-9, slajdy 62–64 oraz 70–71 (`ContinueWith`, `TaskContinuationOptions`).
  - **Zadanie Lab 8.4 (Ciąg Fibonacciego na krotkach i BigInteger, 1.0 pkt)**: Implementacja potokowego wyliczania wyrazów ciągu Fibonacciego z użyciem typu `System.Numerics.BigInteger` i krotek `Tuple<BigInteger, BigInteger>` na dystansie $200\,000$ zadań. Porównanie wydajności sekwencyjnej vs łańcuchowej TPL (zwrócenie uwagi na narzut alokacji milionów obiektów zadań).
  - **Zadanie Lab 8.5–8.6 (Zrównoleglone sito liczb pierwszych i PrimesGenerator, 2.5 pkt)**:
    - Funkcja weryfikacji pierwszości `IsPrime(int liczba)` dla zakresu $[0 \dots 100\,000\,000]$.
    - Wariant 1: Pomiar na pojedynczym wątku.
    - Wariant 2: Podział przestrzeni poszukiwań na $N$ rozłącznych przedziałów, gdzie $N = \text{Environment.ProcessorCount}$. Uruchomienie $N$ zadań roboczych zwracających `List<int>`. Scalenie wyników zadaniem kontynuacji `Task.Factory.ContinueWhenAll(tasks, ...)`.
    - Wariant 3: Statyczna klasa asynchroniczna `PrimesGenerator` z metodą `public static async Task<List<int>> FindPrimesAsync(int start, int end)`. Użycie `await Task.WhenAll(tasks)` oraz spłaszczenie kolekcji zapytaniem LINQ `results.SelectMany(p => p).ToList()`.
    - *Baza teoretyczna*: Wykład 8-9, slajdy 55–58 (`async/await`) oraz slajdy 76–77 (`ContinueWhenAll`).
  - **Zadanie Lab 9.1–9.3 (calculateSomethingAsync, Raportowanie i Anulowanie, 1.5 pkt)**:
    - Implementacja metody generującej tablicę losową `calculateSomethingAsync(int size)`.
    - Przeciążenie metody o interfejs `IProgress<(int, int)> progress` (obsługa `progress?.Report((i, val))`) oraz zdarzenie `ProgressChanged`.
    - Wprowadzenie kooperatywnego anulowania za pomocą `CancellationToken ct`. Testowanie za pomocą `CancellationTokenSource.CancelAfter(5000)`. Rzucenie wyjątku `OperationCanceledException` i obsługa w bloku `catch (AggregateException ae)` z użyciem `ae.Flatten()`.
    - *Baza teoretyczna*: Wykład 8-9, slajdy 29–33 (anulowanie zadań) oraz 34–42 (`IProgress<T>`, `Progress<T>`).
  - **Zadanie Lab 9.4 (Warunkowe rozgałęzienia kontynuacji, 0.5 pkt)**: Konfiguracja alternatywnych zadań kontynuacji na pojedynczym zadaniu źródłowym:
    - `onFaultTask`: `ContinueWith(..., TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.NotOnRanToCompletion)` — wywoływane wyłącznie w razie błędu/anulowania.
    - `onSuccessTask`: `ContinueWith(..., TaskContinuationOptions.OnlyOnRanToCompletion)` — wywoływane wyłącznie przy sukcesie.
    - Oczekiwanie na oba warianty za pomocą `Task.WaitAll()`.
    - *Baza teoretyczna*: Wykład 8-9, slajdy 66–67, 70–73.
  - **Zadanie Lab 9.5–9.8 (Wielki Benchmark Sumowania: Pętle, PLINQ, Taski i Obliczenia, 2.5 pkt)**: Wygenerowanie tablicy $10\,000\,000$ liczb całkowitych. Testy porównawcze sumowania prostego oraz sumowania nieliniowej funkcji `calculatedValue(double)`:
    1. Sekwencyjna pętla `foreach`.
    2. `Parallel.ForEach` bez synchronizacji (anomalia wyścigu, niepoprawna suma).
    3. `Parallel.ForEach` z blokadą `lock (locker) { sum += x; }` (drastyczny spadek wydajności przez *lock contention*).
    4. Jednowątkowe LINQ `.Sum()`.
    5. PLINQ `.AsParallel().ForAll(...)` bez i z blokadą `lock`.
    6. PLINQ `.AsParallel().Sum(...)` (optymalna redukcja równoległa).
    7. Własny podział na zadania `Task` w liczbie `Environment.ProcessorCount` ze zsumowaniem wyników cząstkowych `Task.WhenAll(tasks).Result.Sum()`.
    - *Baza teoretyczna*: Wykład 8-9 oraz Wykład 10 (klasa `Parallel`), slajdy 20–29.

---

### Rozdział: Sprawozdanie 10 — Zaawansowane Zrównoleglanie Pętli, Klasa Parallel i Algorytmy Współbieżne
* **Artefakty w folderze**:
  - Wykład 1: `Wyk 10 -  klasa Parallel.pdf` (Metody klasy `Parallel`, warianty asynchroniczne, stan pętli, przerywanie, redukcja lokalna, schedulery).
  - Wykład 2: `Wyk 10 -  Algorytmy.pdf` (MergeSort, sortowanie oscylacyjne, mnożenie macierzy metodą podziału, Strassena i Cannona, algorytmy genetyczne i mrówkowe).
* **Głęboka analiza zadań i powiązanie z teorią wykładową**:
  - **Zagadnienie 1 (Sterowanie i przerywanie równoległych pętli `Parallel.For`)**:
    - Analiza metod `ParallelLoopState.Break()` vs `ParallelLoopState.Stop()`. `Break()` zapewnia, że wszystkie iteracje o indeksie niższym niż bieżący zostaną ukończone; `Stop()` dąży do natychmiastowego przerwania wszystkich wątków.
    - Weryfikacja flagi `state.ShouldExitCurrentIteration` wewnątrz ciała iteracji w celu uniknięcia zbędnych obliczeń.
    - Odczyt obiektu wynikowego `ParallelLoopResult` (`IsCompleted`, `LowestBreakIteration`).
    - *Baza teoretyczna*: Wykład 10 (`Parallel`), slajdy 6–8.
  - **Zagadnienie 2 (Optymalizacja redukcji równoległej: Stan Lokalny Wątku)**:
    - Zaawansowane przeciążenie `Parallel.For<TLocal>` z akumulatorem wątkowym:
      ```csharp
      Parallel.For<double>(0, data.Length, options,
          () => 0.0, // inicjalizacja stanu lokalnego wątku
          (i, loop, localState) => localState + data[i], // akumulacja bez blokad
          localState => { lock (lockObj) { totalSum += localState; } } // scalenie stanów
      );
      ```
    - Wykazanie, że podejście to redukuje liczbę blokad z $10\,000\,000$ do zaledwie kilkunastu (liczba wątków roboczych), deklasując standardowy `lock` wewnątrz pętli.
    - *Baza teoretyczna*: Wykład 10 (`Parallel`), slajdy 21–22, 26–27.
  - **Zagadnienie 3 (Współbieżne Algorytmy Sortowania: MergeSort i Sortowanie Oscylacyjne)**:
    - Implementacja równoległego sortowania przez scalanie z podziałem rekurencyjnym za pomocą `Task.Run()` przy zachowaniu progu sekwencyjnego (*sequential threshold* $N \le 2048$), zapobiegającego nadmiernej granulacji zadań.
    - Analiza modelu sortowania oscylacyjnego na sieci procesów $A_i$ i $B_i$ z synchroniczną wymianą danych i złożonością czasową $2n$ cykli.
    - *Baza teoretyczna*: Wykład 10 (`Algorytmy`), slajdy 2–15.
  - **Zagadnienie 4 (Równoległe Mnożenie Macierzy: Podział Blokowy, Strassen i Cannon)**:
    - Zrównoleglenie klasycznego mnożenia macierzy $C = A \cdot B$ o złożoności $O(n^3)$ z użyciem `Parallel.For`.
    - Analiza algorytmu Strassena ($O(n^{2.807})$) opartego na 7 iloczynach pomocniczych podmacierzy.
    - Dekompozycja macierzy metodą siatki Cannona na macierzach $m \times m$ z cyklicznym przesuwaniem wierszy i kolumn (*skewing & rolling*).
    - *Baza teoretyczna*: Wykład 10 (`Algorytmy`), slajdy 16–33.
  - **Zagadnienie 5 (Metaheurystyki Współbieżne: Algorytm Genetyczny i Algorytm Mrówkowy)**:
    - Równoległa architektura algorytmu genetycznego (Master-Slave z równoległą ewaluacją populacji i selekcją turniejową/rangową).
    - Równoległy algorytm mrówkowy (ACO/CAS) dla problemu TSP: każda mrówka buduje trasę jako osobne zadanie `Task`, po czym następuje zsynchronizowana faza odparowania i aktualizacji feromonów na krawędziach grafu.
    - *Baza teoretyczna*: Wykład 10 (`Algorytmy`), slajdy 34–56.

---

## 3. Matryca Odwzorowania i Przegląd Zależności (Graph of Thoughts Matrix)

Poniższa tabela przedstawia trójstronne mapowanie wymagań laboratoryjnych na teoretyczne fundamenty z wykładów oraz bazowe pliki kodu:

| Nr Sprawozdania | Konkretne Zadanie | Mechanizm z Wykładu (Slajdy / Klasy) | Plik Bazowy / Klasa w Kodzie | Planowana Akcja / Architektura Rozwiązania |
| :--- | :--- | :--- | :--- | :--- |
| **Sprawozdanie 3** | Zad 1: Prosty delegat arytmetyczny | Wyk 3, s. 44-46 (`delegate`, `MulticastDelegate`, `GetInvocationList`) | `SimpleDelegate\Program.cs` | Definicja delegata `void(int, int)`, 4 metody, wywołanie `+=`, inspekcja listy wywołań. |
| **Sprawozdanie 3** | Zad 2: Zakończenie v1 (`IsBackground`) | Wyk 4, s. 12-13 (`Thread.IsBackground`, cykl życia CLR) | `SimpleEvent\Alarm.cs` (`startClock`) | Ustawienie właściwości `internalTrhead.IsBackground = true`. |
| **Sprawozdanie 3** | Zad 3: Zakończenie v2 (`IDisposable` + `Interrupt`) | Wyk 4, s. 19, 41 (`Thread.Interrupt`, `ThreadInterruptedException`) | `SimpleEvent\Alarm.cs` (`Dispose`, `checkingTime`) | Implementacja `IDisposable`, `Interrupt()`, blok `try-catch`, wywołanie w `Main`. |
| **Sprawozdanie 3** | Zad 4: Parametryzacja `checkingInterval` | Wyk 3, s. 39 (`Constructor overloading`) | `SimpleEvent\Alarm.cs` | Przeciążenie konstruktorów i metody `SetAlarm` o parametr interwału w ms. |
| **Sprawozdanie 3** | Zad 5: Zdarzenie taktu zegara `Tick` | Wyk 3, s. 19; Wyk 7, s. 4-7 (`EventArgs`, `event`) | `SimpleEvent\Alarm.cs` | Zdarzenie `TickEvent` przekazujące `timespanToAlarm`, właściwość `RemainingTime`. |
| **Sprawozdanie 3** | Zad 6-7: Czasomierze `Threading.Timer` & `Timers.Timer` | Wyk 4, s. 17-18, 29 (`System.Threading.Timer`, `Interlocked`) | `TimerExample\Program.cs` | `AutoReset=false`, klasa `TimerState`, atomowy `Interlocked.Increment`, zatrzymanie przez `Change()`. |
| **Sprawozdanie 4** | Zad 2: Analiza wątkowości `Random` i wyścigów | Wyk 4, s. 4, 23 (brak thread-safety w `Random`, wyścigi) | `ProducentKonsument\Program.cs` | Testy seeda opartego na czasie, ID wątku oraz współdzielonego; demonstracja kolizji. |
| **Sprawozdanie 4** | Zad 3: Synchronizacja `AutoResetEvent` | Wyk 5-6, s. 3-4 (`AutoResetEvent`, `WaitOne`, `Set`) | `ProducentKonsument\Program.cs` | Zmienne `pisanie` (true) i `czytanie` (false), naprzemienna serializacja odczytu/zapisu. |
| **Sprawozdanie 4** | Zad 4: Zastosowanie `ManualResetEvent` | Wyk 5-6, s. 11 (`ManualResetEvent`, `Reset`) | `ProducentKonsument\Program.cs` | Zastąpienie przez `ManualResetEvent`, analiza ucieczki wielu wątków, naprawa ryglem `lock`. |
| **Sprawozdanie 4** | Zad 5: Spotkania wątków (*Rendezvous*) | Wyk 5-6, s. 16 (`WaitHandle.SignalAndWait`) | `Spotkania\Program.cs` | Odkomentowanie `SignalAndWait(wh1, wh2)` i `(wh2, wh1)`, synchronizacja faz. |
| **Sprawozdanie 4** | Zad 6: Czekanie na jednego i wszystkich | Wyk 5-6, s. 17 (`WaitHandle.WaitAny`, `WaitAll`) | `SynchroWait\Program.cs` | Utworzenie tablicy uchwytów zdarzeń, wywołanie `WaitAny` oraz `WaitAll`. |
| **Sprawozdanie 4** | Zad 7: Benchmark blokad sekcji krytycznej | Wyk 4, s. 23-35 (`lock`, `Mutex`, `Semaphore`, `SemaphoreSlim`) | `MuteksyIReszta\Program.cs` | Implementacja 5 wariantów ochrony pętli inkrementacji, pomiary czasu `Stopwatch`. |
| **Sprawozdanie 5-6**| Zad 5.2-4: `List<T>` vs `ConcurrentBag` vs `lock` | Wyk 5-6, s. 21-23 (`ConcurrentBag<T>`, `lock`) | `UnsafeCollection\Program.cs` | Wykazanie uszkodzenia listy, test wydajności `ConcurrentBag` oraz listy z `lock`. |
| **Sprawozdanie 5-6**| Zad 5.5-6: Unikalność w `ConcurrentBag` i słowniku | Wyk 5-6, s. 24-27 (`ConcurrentDictionary.TryAdd`) | `UniqueCollection\Program.cs` | Analiza błędu wyścigu TOCTOU w `ConcurrentBag`, naprawa przez `lock` oraz `ConcurrentDictionary`. |
| **Sprawozdanie 5-6**| Zad 5.7: Kolejka współbieżna i semafor | Wyk 5-6, s. 30-37 (`ConcurrentQueue.TryDequeue`, `SemaphoreSlim`) | `ProducerConsumerQ\Program.cs` | Zastąpienie `Queue` przez `ConcurrentQueue`, pętla ponowień, a następnie rygiel semaforowy. |
| **Sprawozdanie 5-6**| Zad 5.8: Bufor blokujący `BlockingCollection` | Wyk 5-6, s. 28-29 (`BlockingCollection<T>`, `Add`, `Take`) | Nowy projekt / `ProducerConsumerQ` | Bufor o pojemności 10, opóźniony start konsumentów o 5s, metoda `CompleteAdding()`. |
| **Sprawozdanie 5-6**| Zad 6.1: Bariery fazowe algorytmu genetycznego | Wyk 5-6, s. 18, 20 (`System.Threading.Barrier`) | `Barriers\Program.cs`, `GAHelper.cs` | Pula 16 stałych wątków ewaluacji, architektura podwójnej bariery synchronizacji faz. |
| **Sprawozdanie 5-6**| Zad 6.2: Emulacja bariery monitorem | Wyk 5-6, s. 19 (`Monitor.Wait`, `PulseAll`) | Nowy projekt `Monitory` (`GAHelper`) | Implementacja klasy `MonitorBarrier` na bazie `Wait()` i `PulseAll()` z licznikiem fazy. |
| **Sprawozdanie 5-6**| Zad 6.3: GUI WPF z wieloma wątkami | Wyk 7, s. 33-36 (`Dispatcher.Invoke`, `CheckAccess`) | `WPFThreads\MainWindow.xaml.cs` | Bezpieczna aktualizacja kontrolek WPF przez `Dispatcher.Invoke`, integracja suwaków. |
| **Sprawozdanie 7** | Zad 1-6: Podstawy `Task`, parametry, krotki | Wyk 8-9, s. 4, 37, 44-50 (`Task`, `ValueTuple`, `MyParam`) | Nowy projekt `FirstTasks` | Sekwencja zadań t1–t5, przekazywanie parametrów, `Task.Factory.StartNew`, lista zadań. |
| **Sprawozdanie 7** | Zad 7-10: Projekt animacji konsolowej `Snow` | Wyk 8-9, s. 48; Wyk 7, s. 24 (`ThreadPool.SetMinThreads`) | Nowy projekt `Snow` | 50 zadań `Star`, `lock(Console.Out)`, `Random.Shared`, dynamiczne wymiary konsoli. |
| **Sprawozdanie 8-9**| Zad 8.1-3: Łańcuchy kontynuacji i wydajność | Wyk 8-9, s. 62-64, 70-71 (`ExecuteSynchronously`) | Nowy projekt `LabTask1` | Łańcuch 1 mln zadań z `ContinueWith`, benchmark z flagą i bez, funkcja `DoPointlessMess`. |
| **Sprawozdanie 8-9**| Zad 8.4: Fibonacci na `BigInteger` i krotkach | Wyk 8-9, s. 63 (`ContinueWith` z przekazywaniem danych) | Nowy projekt `FibonacciTask` | Sekwencja 200 tys. zadań wyliczających kolejne wyrazy ciągu Fibonacciego. |
| **Sprawozdanie 8-9**| Zad 8.5-6: Wyszukiwanie liczb pierwszych i async | Wyk 8-9, s. 55-58, 76-77 (`ContinueWhenAll`, `WhenAll`) | Nowy projekt `PrimesTask` | Podział zakresu na liczbę rdzeni, scalanie `ContinueWhenAll`, klasa `PrimesGenerator`. |
| **Sprawozdanie 8-9**| Zad 9.1-4: Anulowanie, postęp i błędy w TPL | Wyk 8-9, s. 29-43, 65-73 (`IProgress`, `CancellationToken`) | Nowy projekt `TaskAdvanced` | `calculateSomethingAsync`, `Progress<T>`, `CancelAfter(5000)`, ścieżki `OnlyOnFaulted` / `RanToCompletion`. |
| **Sprawozdanie 8-9**| Zad 9.5-8: Wielki benchmark sumowania i PLINQ | Wyk 8-9, s. 59; Wyk 10, s. 20-29 (`PLINQ`, `Parallel.ForEach`) | Nowy projekt `BenchmarkSum` | 10 mln elementów: testy pętli, `lock`, `PLINQ.ForAll`, `PLINQ.Sum`, zadania TPL, funkcja `calculatedValue`. |
| **Sprawozdanie 10** | Zad 1-2: `Parallel.For`, sterowanie i redukcja | Wyk 10 (`Parallel`), s. 6-9, 21-22 (`ParallelLoopState`, TLocal) | Nowy projekt `ParallelAdvanced` | Sterowanie `Break`/`Stop`, implementacja wzorca `Parallel.For<TLocal>` z redukcją bezkolizyjną. |
| **Sprawozdanie 10** | Zad 3-5: Równoległe algorytmy klasyczne | Wyk 10 (`Algorytmy`), s. 2-33, 34-56 (MergeSort, Cannon, GA/ACO) | Nowy projekt `ParallelAlgorithms` | Implementacja współbieżnego MergeSort, mnożenia macierzy i symulacji optymalizacji. |

---

## 4. Strategia Realizacji i Architektura Przestrzeni Roboczej (Master Roadmap)

Aby zagwarantować pełną integralność istniejących kodów źródłowych wykładowcy oraz umożliwić bezbłędną weryfikację i budowanie projektów, przyjęto następującą strategię techniczną:

### 4.1. Zasada Nienaruszalności Materiałów Bazowych (*Non-Destructive Workspace*)
1. Żaden oryginalny plik w folderach `DotNet8EnCleanCore8.0-прикладові коди`, `DotNet2Clean`, `DotNet3EnClean`, `DotNet4EnClean` nie może zostać nadpisany ani usunięty.
2. Rozwiązania zadań będą tworzone w dedykowanych podkatalogach rozwiązań, np.:
   - `Sprawozdanie 3\Solutions\Lab3_Solutions.sln`
   - `Sprawozdanie 4\Solutions\Lab4_Solutions.sln`
   - `Sprawozdanie 5-6\Solutions\Lab5_6_Solutions.sln`
   - `Sprawozdanie 7\Solutions\Lab7_Solutions.sln`
   - `Sprawozdanie 8-9\Solutions\Lab8_9_Solutions.sln`
   - `Sprawozdanie 10\Solutions\Lab10_Solutions.sln`
3. Wszystkie projekty będą konfigurowane pod nowoczesny Target Framework (`net8.0` lub `net9.0`) z włączoną obsługą `Nullable` oraz standardem C# 12+.

### 4.2. Harmonogram Realizacji i Oszacowanie Złożoności

```
[Krok 1: Lab 3]  --> [Krok 2: Lab 4]  --> [Krok 3: Lab 5-6]
Delegaty/Timery      Prymitywy synchro     Kolekcje, Bariery, GUI
(Złożoność: Niska)   (Złożoność: Średnia)  (Złożoność: Wysoka)
       |                     |                     |
       v                     v                     v
[Krok 4: Lab 7]  --> [Krok 5: Lab 8-9] --> [Krok 6: Lab 10]
Podstawy TPL, Snow   Zaawansowane TPL/PLINQ  Klasa Parallel, Algorytmy
(Złożoność: Średnia) (Złożoność: Wysoka)   (Złożoność: Bardzo wysoka)
```

1. **Faza 1: Sprawozdanie 3 (Szacowany nakład: 3 Story Points / Niski)**
   - Wdrożenie delegatów arytmetycznych w projekcie konsolowym.
   - Refaktoryzacja klasy `Alarm`: obsługa `IsBackground`, wzorzec `IDisposable` z `Thread.Interrupt`, zdarzenie `Tick` z odliczaniem.
   - Konfiguracja `System.Threading.Timer` z atomowym stanem `TimerState` (`Interlocked`).
   - Weryfikacja: poprawne zakończenie wątku po naciśnięciu klawisza, brak wiszących procesów w tle.

2. **Faza 2: Sprawozdanie 4 (Szacowany nakład: 5 Story Points / Średni)**
   - Implementacja naprzemiennej synchronizacji Producent-Konsument na `AutoResetEvent`.
   - Eksperyment z `ManualResetEvent` i implementacja rygla zabezpieczającego.
   - Realizacja punktu spotkań w `Spotkania` (`SignalAndWait`).
   - Implementacja zbiorczego oczekiwania w `SynchroWait` (`WaitAny` i `WaitAll`).
   - Benchmark 5 wariantów sekcji krytycznej w `MuteksyIReszta` (pomiar w ms za pomocą `Stopwatch`).

3. **Faza 3: Sprawozdanie 5-6 (Szacowany nakład: 8 Story Points / Wysoki)**
   - Testy wyścigów w `List<T>`, `ConcurrentBag` oraz `ConcurrentDictionary` (unikalność $10\,000$ liczb).
   - Implementacja kolejki z pętlą ponowień `TryDequeue` oraz wariantu sterowanego `SemaphoreSlim`.
   - Zastosowanie `BlockingCollection` z ograniczeniem pojemności do 10 i opóźnionymi konsumentami.
   - Równoległa ewaluacja algorytmu genetycznego w `Barriers` z architekturą podwójnej bariery (`Barrier.SignalAndWait`).
   - Projekt `Monitory`: własna klasa bariery na bazie `Monitor.Wait` / `PulseAll`.
   - Projekt `WPFThreads`: podpięcie aktualizacji kontrolek WPF przez `Dispatcher.Invoke` z obsługą suwaków.

4. **Faza 4: Sprawozdanie 7 (Szacowany nakład: 5 Story Points / Średni)**
   - Utworzenie solucji `FirstTasks` i realizacja zadań t1–t5 (parametryzacja, krotki, obiekty klas).
   - Dynamiczna lista zadań z użyciem `Task.Factory.StartNew`.
   - Implementacja projektu `Snow`: 50 zadań `Star`, `lock(Console.Out)`, bezpieczny generator `Random.Shared`, `ThreadPool.SetMinThreads(64, 64)` oraz responsywność okna konsoli.

5. **Faza 5: Sprawozdanie 8-9 (Szacowany nakład: 8 Story Points / Wysoki)**
   - Badanie wpływu `TaskContinuationOptions.ExecuteSynchronously` na łańcuchu 1 mln zadań (pomiary Release).
   - Obliczenia ciągu Fibonacciego na typie `BigInteger` i krotkach.
   - Wielowątkowe sito liczb pierwszych z `Environment.ProcessorCount` i scalaniem `ContinueWhenAll`.
   - Klasa `PrimesGenerator` ze składnią `async/await` i `Task.WhenAll`.
   - Asynchroniczna metoda z raportowaniem `IProgress<(int, int)>` i anulowaniem `CancelAfter(5000)`.
   - Rozgałęzienia `OnlyOnFaulted` i `OnlyOnRanToCompletion`.
   - Wielki benchmark sumowania 10 mln liczb: porównanie pętli zwykłej, z ryglem `lock`, PLINQ oraz zadań TPL.

6. **Faza 6: Sprawozdanie 10 (Szacowany nakład: 10 Story Points / Bardzo wysoki)**
   - Zaawansowane wykorzystanie `Parallel.For` z obsługą `ParallelLoopState.Break()` i `Stop()`.
   - Implementacja wzorca bezkolizyjnej redukcji wielowątkowej z lokalnym stanem wątku (`Parallel.For<TLocal>`).
   - Implementacja współbieżnego MergeSort z odcięciem głębokości wątków.
   - Implementacja równoległego mnożenia macierzy (podział blokowy oraz algorytm Cannona).
   - Analiza porównawcza modeli algorytmów metaheurystycznych (Algorytm Genetyczny vs Algorytm Mrówkowy).

---

## 5. Uniwersalny Szablon Sprawozdania Akademickiego (Szablon Sprawozdania)

Poniższy szablon stanowi ujednoliconą strukturę sprawozdania laboratoryjnego w języku polskim (ze wskazówkami technicznymi w języku ukraińskim w nawiasach kwadratowych), przygotowaną pod rygorystyczne wymagania Politechniki Świętokrzyskiej.

```markdown
# Politechnika Świętokrzyska w Kielcach
## Wydział Elektrotechniki, Automatyki i Informatyki
### Katedra Systemów Informatycznych

---

**Przedmiot**: Programowanie współbieżne  
**Kierunek studiów**: Informatyka  
**Semestr**: Letni / Zimowy 2025/2026  
**Wykonawca**: Namachynskyi Ihor  
**Grupa laboratoryjna**: 2ID12B  
**Numer indeksu**: s097889  
**Prowadzący zajęcia**: dr inż. Paweł Paduch  
**Tytuł sprawozdania**: Sprawozdanie nr [NUMER] — [PEŁNY TEMAT ZAJĘĆ]  
**Data wykonania ćwiczenia**: [DD.MM.RRRR]  
**Data oddania sprawozdania**: [DD.MM.RRRR]  

---

### 1. Cel ćwiczenia
[TUTAJ NALEŻY WKLEIĆ ZWIĘZŁY OPIS CELU ĆWICZENIA NA PODSTAWIE INSTRUKCJI LABORATORYJNEJ]
*Короткий опис мети роботи: які саме механізми досліджувалися, для чого вони застосовуються в реальних системах та які практичні навички мали бути набуті під час виконання роботи.*

---

### 2. Krótkie wprowadzenie teoretyczne
[TUTAJ NALEŻY PRZEDSTAWIĆ KLUCZOWE POJĘCIA I PRAWIDŁA TEORETYCZNE Z WYKŁADU]
*Висвітлення теоретичної бази з PDF лекції викладача: архітектура застосованих класів (.NET CLR, System.Threading, System.Threading.Tasks), стан потоків, моделі синхронізації, алгоритмічна складність та ризики (wyścigi danych, zakleszczenia, głodzenie).*

---

### 3. Rozwiązanie zadań laboratoryjnych

#### 3.1. Zadanie 1: [NAZWA ZADANIA Z INSTRUKCJI]

##### 3.1.1. Treść zadania
[DOKŁADNA TREŚĆ WYMAGANIA Z INSTRUKCJI W JĘZYKU POLSKIM]

##### 3.1.2. Opis implementacji i zastosowanych mechanizmów
[SZCZEGÓŁOWE WYJAŚNIENIE ARCHITEKTURY ROZWIĄZANIA]
*Детальний опис технічного рішення: які класи створено, які методи викликано, як організовано життєвий цикл потоків або завдань, як забезпечено коректну синхронізацію та безпеку доступу до ресурсів.*

##### 3.1.3. Kluczowe fragmenty kodu źródłowego
```csharp
// [TUTAJ WKLEIĆ CZYTELNY, SKOMENTOWANY KOD ROZWIĄZANIA ZADANIA 1]
public class SolutionTask1
{
    // Implementacja mechanizmu
}
```

##### 3.1.4. Wyniki działania programu (Logi / Zrzuty ekranu)
[ZRZUT EKRANU KONSOLI LUB OKNA APLIKACJI POTWIERDZAJĄCY POPRAWNOŚĆ ROZWIĄZANIA]
```text
[TUTAJ WKLEIĆ RZECZYWISTY OUTPUT Z KONSOLI / LOGI WYKONANIA]
```

---

#### 3.2. Zadanie 2: [NAZWA ZADANIA Z INSTRUKCJI]

##### 3.2.1. Treść zadania
[DOKŁADNA TREŚĆ WYMAGANIA Z INSTRUKCJI W JĘZYKU POLSKIM]

##### 3.2.2. Opis implementacji i zastosowanych mechanizmów
[SZCZEGÓŁOWE WYJAŚNIENIE ARCHITEKTURY ROZWIĄZANIA]

##### 3.2.3. Kluczowe fragmenty kodu źródłowego
```csharp
// [TUTAJ WKLEIĆ CZYTELNY, SKOMENTOWANY KOD ROZWIĄZANIA ZADANIA 2]
```

##### 3.2.4. Wyniki działania programu (Logi / Zrzuty ekranu)
```text
[TUTAJ WKLEIĆ RZECZYWISTY OUTPUT Z KONSOLI / LOGI WYKONANIA]
```

---

[... POWTÓRZYĆ BLOK 3.X DLA KAŻDEGO KOLEJNEGO ZADANIA Z DANEJ INSTRUKCJI ...]

---

### 4. Tabele pomiarowe i analiza wydajności (jeśli dotyczy)
[TUTAJ ZAMIEŚCIĆ ZESTAWIENIE CZASÓW WYKONANIA DLA ROZWIĄZAŃ JEDNOWĄTKOWYCH I WIELOWĄTKOWYCH]

| Wariant implementacji / Liczba wątków | Czas wykonania (ms) | Wynik operacji / Poprawność danych | Uwagi i obserwacje |
| :--- | :--- | :--- | :--- |
| Jednowątkowy (Baseline) | ... ms | Poprawny | 1 rdzeń CPU |
| Wielowątkowy bez synchronizacji | ... ms | BŁĘDNY (Race condition) | Brak izolacji pamięci |
| Zastosowanie blokady lock | ... ms | Poprawny | Narzut synchronizacji |
| Zastosowanie TPL / PLINQ | ... ms | Poprawny | Optymalne przyspieszenie |

---

### 5. Wnioski końcowe
[TUTAJ ZAMIEŚCIĆ GŁĘBOKIE WNIOSKI INŻYNIERSKIE]
*Глибокі інженерні висновки польською мовою:*
1. *Efektywność i skalowalność: jak zrównoleglenie wpłynęło na czas wykonania w zależności od liczby rdzeni (Environment.ProcessorCount).*
2. *Wpływ synchronizacji na wydajність: dlaczego nadmierna granulacja blokad (lock contention) może spowolnić program bardziej niż wykonanie sekwencyjne.*
3. *Bezpieczeństwo pamięci: dlaczego standardowe kolekcje i generatory pseudolosowe zawodzą w środowisku wielowątkowym.*
4. *Praktyczne zalecenia architektoniczne dla tworzenia aplikacji współbieżnych w .NET.*
```

---
*Koniec dokumentu audytowego. Wygenerowano w ramach przygotowania do implementacji zadań laboratoryjnych.*

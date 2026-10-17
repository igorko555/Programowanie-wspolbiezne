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
**Tytuł sprawozdania**: Sprawozdanie nr 3 — Delegaty, Eventy i Timery w środowisku .NET  
**Data wykonania ćwiczenia**: 17.10.2026 r.  

---

### 1. Cel ćwiczenia
Celem laboratorium było praktyczne zapoznanie się z fundamentalnymi mechanizmami asynchroniczności i programowania wielowątkowego w języku C# na platformie .NET:
1. Projektowaniem, tworzeniem oraz łączeniem delegatów jedno- i wielokrotnych (*multicast delegates*) oraz inspekcją ich list wywołań.
2. Zarządzaniem cyklem życia wątków potomnych ze szczególnym uwzględnieniem różnic pomiędzy wątkami pierwszoplanowymi (*foreground*) a drugoplanowymi (*background / IsBackground*).
3. Bezpiecznym i deterministycznym zatrzymywaniem wątków oczekujących za pośrednictwem interfejsu `IDisposable` oraz mechanizmu przerwań `Thread.Interrupt()`, połączonym z obsługą wyjątku `ThreadInterruptedException`.
4. Implementacją własnego wzorca publikacji i subskrypcji zdarzeń (*Events*) z przekazywaniem dedykowanych argumentów zdarzeń dziedziczących po `EventArgs`.
5. Wykorzystaniem czasomierzy `System.Timers.Timer` oraz `System.Threading.Timer` w połączeniu z operacjami atomowymi klasy `System.Threading.Interlocked`.

---

### 2. Krótkie wprowadzenie teoretyczne

#### 2.1. Delegaty i Zdarzenia w .NET
Delegat w języku C# jest bezpiecznym pod względem typów obiektem wskazującym na jedną lub wiele metod o określonej sygnaturze (`MulticastDelegate`). Każdy delegat posiada wewnętrzną listę wywołań (*invocation list*), do której metody można dołączać operatorem `+=` oraz usuwać operatorem `-=`. Metoda `GetInvocationList()` pozwala na programistyczną inspekcję zarejestrowanych subskrybentів.

Zdarzenia (*Events*) stanowią hermetyzację delegatów opartą na wzorcu projektowym Obserwator (*Publisher-Subscriber*). Uniemożliwiają one przypadkowe nadpisanie listy subskrybentów przez zewnętrzne klasy (dozwolone są tylko operacje `+=` i `-=`).

#### 2.2. Cykl życia wątku CLR i wątki w tle (`IsBackground`)
W środowisku wykonawczym .NET CLR proces aplikacji kończy działanie dopiero w momencie, gdy zakończą się **wszystkie wątki pierwszoplanowe** (`IsBackground = false`). Jeżeli wątek potomny wykonuje pętlę nieskończoną lub oczekuje w stanie uśpienia, proces nie ulegnie zamknięciu, nawet jeśli wątek główny (`Main`) zakończył swoje zadanie. Ustawienie `IsBackground = true` sprawia, że środowisko CLR może natychmiast przerwać ten wątek w chwili wyjścia z wątku głównego.

#### 2.3. Przerywanie wątków: `Thread.Interrupt()` vs `Thread.Abort()`
Wywołanie `Thread.Interrupt()` przerywa bieżący stan wstrzymania wątku (np. `Thread.Sleep`, `Join`, `WaitOne`), wprowadzając wątek ze stanu `WaitSleepJoin` w stan obsługi wyjątku `ThreadInterruptedException`. Pozwala to na kontrolowane i czyste zwolnienie zasobów w bloku `catch`/`finally`. W przeciwieństwie do przestarzałej metody `Thread.Abort()` (która w .NET Core/.NET 5+ rzuca `PlatformNotSupportedException`), `Interrupt` jest w pełni wspierany i bezpieczny.

#### 2.4. Czasomierze i Operacje Atomowe (`Interlocked`)
- `System.Timers.Timer`: komponent zdarzeniowy; właściwość `AutoReset = false` pozwala przekształcić go w jednorazowy budzik.
- `System.Threading.Timer`: lekki czasomierz wykonujący delegat `TimerCallback` na wątku pobieranym z puli wątków CLR (`ThreadPool`). Ponieważ metoda zwrotna może być wywoływana współbieżnie, modyfikacja współdzielonych liczników wymaga użycia operacji atomowych `Interlocked.Increment`, zapobiegających wyścigom danych (*race conditions*) na poziomie instrukcji procesora.

---

### 3. Rozwiązanie zadań laboratoryjnych

#### 3.1. Zadanie 1: Prosty delegat arytmetyczny (SimpleDelegate)

##### 3.1.1. Treść zadania
*Na podstawie kodu w projekcie SimpleDelegate zdefiniować delegata typu void z dwoma parametrami typu int. Do podanego delegata utworzyć 4 metody wypisujące sumę, różnicę, iloraz i iloczyn. Dodać nowe metody do listy wywołań delegata. Uruchomić delegata. Wypisać nazwy metod z listy wywołań delegata.*

##### 3.1.2. Opis implementacji
Zdefiniowano delegat `MathOperation(int a, int b)`. Zaimplementowano 4 metody statyczne: `PrintSum`, `PrintDifference`, `PrintProduct` oraz `PrintQuotient` (zabezpieczoną przed dzieleniem przez zero). W metodzie `Main` utworzono instancję delegata i powiązano metody za pomocą operatora `+=`. Wywołano delegat, a następnie za pomocą metody `GetInvocationList()` w pętli wylistowano nazwy wszystkich podpiętych metod.

##### 3.1.3. Kluczowy kod źródłowy
```csharp
public delegate void MathOperation(int a, int b);

static void PrintSum(int a, int b) => Console.WriteLine($"[PrintSum]        {a} + {b} = {a + b}");
static void PrintDifference(int a, int b) => Console.WriteLine($"[PrintDifference] {a} - {b} = {a - b}");
static void PrintProduct(int a, int b) => Console.WriteLine($"[PrintProduct]    {a} * {b} = {a * b}");
static void PrintQuotient(int a, int b)
{
    if (b != 0) Console.WriteLine($"[PrintQuotient]   {a} / {b} = {(double)a / b:F2}");
    else Console.WriteLine("[PrintQuotient] Dzielenie przez zero!");
}

static void Main(string[] args)
{
    MathOperation mathOp = new MathOperation(PrintSum);
    mathOp += PrintDifference;
    mathOp += PrintProduct;
    mathOp += PrintQuotient;

    mathOp(20, 4);

    foreach (var item in mathOp.GetInvocationList())
    {
        Console.WriteLine($"Metoda: {item.Method.Name}");
    }
}
```

##### 3.1.4. Wyniki działania programu (konsola)
```text
==================================================================
   LABORATORIUM 3 - ZADANIE 1: PROSTY DELEGAT ARYTMETYCZNY (C#)   
==================================================================

Аргументи для обчислень: x = 20, y = 4

--- 1. ВИКЛИК ЛАНЦЮГА ДЕЛЕГАТА (MULTICAST INVOCATION) ---
[PrintSum]        20 + 4 = 24
[PrintDifference] 20 - 4 = 16
[PrintProduct]    20 * 4 = 80
[PrintQuotient]   20 / 4 = 5,00

--- 2. МЕТОДИ В СПИСКУ ВИКЛИКІВ ДЕЛЕГАТА (GetInvocationList) ---
[1] Назва методу: PrintSum, Клас: Program
[2] Назва методу: PrintDifference, Клас: Program
[3] Назва методу: PrintProduct, Клас: Program
[4] Назва методу: PrintQuotient, Клас: Program

==================================================================
Завдання 1 успішно виконано.
```

---

#### 3.2. Zadania 2–5: Zdarzenia, Kończenie Wątków, Parametryzacja i Odliczanie (SimpleEvent)

##### 3.2.1. Treść zadań
- **Zadanie 2 (Kończenie v1)**: Rozwiązać problem niekończącej się aplikacji poprzez ustawienie właściwości `internalTrhead.IsBackground = true`.
- **Zadanie 3 (Kończenie v2)**: Zaimplementować interfejs `IDisposable` w klasie `Alarm`. W metodzie `Dispose()` wykonać `internalThread.Interrupt()` oraz wyzerować referencję. Wątek ma obsłużyć `ThreadInterruptedException`.
- **Zadanie 4**: Przeciążyć konstruktory i metodę `SetAlarm` o parametr `checkingInterval` (domyślnie 1000 ms).
- **Zadanie 5 (Nowy Event)**: Dodać aktualizację `timespanToAlarm`, właściwość tylko do odczytu `TimespanToAlarm` oraz nowe zdarzenie wyzwalane przy każdym sprawdzeniu (`ClockTicked`) zwracające pozostały czas.

##### 3.2.2. Opis implementacji
Klasa `Alarm` została rozbudowana o implementację `IDisposable`. Wątek roboczy `internalThread` w metodzie `CheckingTime` w każdej pętli aktualizuje `timespanToAlarm = timeOfAlarm - DateTime.Now;` i wyzwala zdarzenie `ClockTicked`. Gdy użytkownik zażąda przerwania lub wyjścia, wywołanie `alarm.Dispose()` przerywa stan uśpienia `Thread.Sleep`, rzucając `ThreadInterruptedException`, co umożliwia kontrolowane zakończenie wątku.

##### 3.2.3. Kluczowy kod źródłowy
```csharp
public class ClockTickEventArgs : EventArgs
{
    public TimeSpan RemainingTime { get; set; }
}

public class Alarm : IDisposable
{
    private TimeSpan timespanToAlarm;
    private DateTime timeOfAlarm;
    private Thread? internalThread;
    private int checkingInterval;

    public TimeSpan TimespanToAlarm => timespanToAlarm;
    public event EventHandler<ClockTickEventArgs>? ClockTicked;
    public event AlarmEventHandler? AlarmOccured;

    public Alarm(TimeSpan timespan, int checkingInterval = 1000)
    {
        this.checkingInterval = checkingInterval;
        SetAlarm(timespan, checkingInterval);
    }

    private void StartClock()
    {
        internalThread = new Thread(CheckingTime) { IsBackground = false };
        internalThread.Start(checkingInterval);
    }

    private void CheckingTime(object? o)
    {
        int interval = (int)(o ?? 1000);
        try
        {
            while (DateTime.Now < timeOfAlarm)
            {
                timespanToAlarm = timeOfAlarm - DateTime.Now;
                ClockTicked?.Invoke(this, new ClockTickEventArgs { RemainingTime = timespanToAlarm });
                Thread.Sleep(interval);
            }
            AlarmOccured?.Invoke(this, new AlarmEventArgs { Message = "Alarm fired!", AlarmFiredAt = DateTime.Now });
        }
        catch (ThreadInterruptedException)
        {
            Console.WriteLine("\n[WĄTEK ALARM] Bezpieczne przerwanie (Interrupt) i zwolnienie zasobów.");
        }
    }

    public void Dispose()
    {
        if (internalThread != null && internalThread.IsAlive)
        {
            internalThread.Interrupt();
            internalThread = null;
        }
    }
}
```

##### 3.2.4. Wyniki działania programu (konsola)
```text
==================================================================
   LABORATORIUM 3 - ZADANIA 2-5: EVENTY, KOŃCZENIE WĄTKÓW I ALARM  
==================================================================

Оберіть режим тестування завершення програми:
1 - Zadanie 2: Kończenie v1 (Właściwość IsBackground = true)
2 - Zadanie 3: Kończenie v2 (Interfejs IDisposable + Thread.Interrupt()) [ZALECANE]
Обрано: Wariant v2 (IDisposable + Interrupt)
Ініціалізація Alarm: час = 4s, checkingInterval = 500ms

[ГОЛОВНИЙ ПОТІК] Очікування... Можете зачекати завершення часу АБО натиснути будь-яку клавішу зараз для переривання:
[CLOCK TICK] Залишилося до сигналу: 4,0 сек (Властивість: 4,0s)
[CLOCK TICK] Залишилося до сигналу: 3,5 сек (Властивість: 3,5s)
[CLOCK TICK] Залишилося до сигналу: 3,0 сек (Властивість: 3,0s)
[CLOCK TICK] Залишилося до сигналу: 2,5 сек (Властивість: 2,5s)
[CLOCK TICK] Залишилося до сигналу: 2,0 сек (Властивість: 2,0s)

[ГОЛОВНИЙ ПОТІК] Отримано команду завершення! Викликаємо alarm.Dispose()...

[WĄTEK ALARM] Otrzymano sygnał Thread.Interrupt(). Bezpieczne zwalnianie zasobów i zakończenie wątku potomnego.

[ГОЛОВНИЙ ПОТІК] Програма штатно завершила свою роботу.
```

---

#### 3.3. Zadania 6–7: Czasomierze i Wyłączanie z Licznikiem Atomowym (TimerExample)

##### 3.3.1. Treść zadań
- **Zadanie 6**: Ustawić `timerSystem.AutoReset = false` w projekcie `TimerExample`. Dodać klasę `TimerState` z polem `Counter` oraz metodę `TimerTask` z atomową inkrementacją `Interlocked.Increment(ref state.Counter)`.
- **Zadanie 7**: Wyłączyć czasomierz `timerThreading` po 5 wywołaniach (gdy licznik osiągnie wartość 5) za pomocą instrukcji `timerThreading.Change(Timeout.Infinite, 0)`.

##### 3.3.2. Opis implementacji
Zaimplementowano klasę `TimerState`, która przechowuje referencję do obiektu `System.Threading.Timer` oraz pole `public int Counter`. W metodzie zwrotnej `TimerTask` wywoływanej na wątkach z puli zastosowano `Interlocked.Increment(ref state.Counter)`. Po osiągnięciu 5 wywołań następuje wywołanie `Change(Timeout.Infinite, 0)`, co natychmiastowo i trwale zatrzymuje dalsze generowanie sygnałów zegarowych.

##### 3.3.3. Kluczowy kod źródłowy
```csharp
public class TimerState
{
    public int Counter;
    public T.Timer? AssociatedTimer;
}

private static void TimerTask(object? timerState)
{
    var state = timerState as TimerState;
    if (state == null) return;

    int currentVal = T.Interlocked.Increment(ref state.Counter);
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff} | Wątek {T.Thread.CurrentThread.ManagedThreadId}]: TimerTask callback. Counter = {currentVal}");

    if (currentVal >= 5)
    {
        Console.WriteLine("Osiągnięto 5 wywołań. Wyłączanie timera...");
        state.AssociatedTimer?.Change(T.Timeout.Infinite, 0);
    }
}
```

##### 3.3.4. Wyniki działania programu (konsola)
```text
==================================================================
   LABORATORIUM 3 - ZADANIA 6-7: CZASOMIERZE I ATOMOWE LICZNIKI   
==================================================================

--- 1. INICJALIZACJA System.Timers.Timer (Interwał: 1,5s) ---
[System.Timers.Timer] Uruchomiony z AutoReset = false (zadziała jednokrotnie jako budzik).

--- 2. INICJALIZACJA System.Threading.Timer (Start po 2s, takt co 1s) ---
[System.Threading.Timer] Oczekiwanie na cykliczne wywołania TimerTask...
Timer zostanie automatycznie wyłączony po 5 wykonaniach (Zadanie 7).

[17:02:58.940] [System.Timers.Timer] BUDZIK ZADZWONIŁ (Elapsed). Ponowne wywołanie zablokowane przez AutoReset=false.
[17:02:59.427 | Wątek 5]: TimerTask callback executed. Licznik Counter = 1
[17:03:00.420 | Wątek 5]: TimerTask callback executed. Licznik Counter = 2
[17:03:01.426 | Wątek 5]: TimerTask callback executed. Licznik Counter = 3
[17:03:02.420 | Wątek 5]: TimerTask callback executed. Licznik Counter = 4
[17:03:03.426 | Wątek 5]: TimerTask callback executed. Licznik Counter = 5
[17:03:03.427] >>> [ZADANIE 7]: Osiągnięto 5 wywołań! Wyłączanie timera przez Change(Timeout.Infinite, 0) <<<

[STAN KOŃCOWY]: timerThreading Counter = 5
Zakończenie demonstracji (Naciśnij klawisz lub zaczekaj)...
```

---

### 4. Wnioski

1. **Elastyczność delegatów**: Mechanizm `MulticastDelegate` w języku C# pozwala na łatwe budowanie łańcuchów przetwarzania bez sztywnego wiązania nadawcy ze strukturą odbiorców. Metoda `GetInvocationList()` umożliwia dynamiczną refleksję i selektywne wywoływanie poszczególnych subskrybentów.
2. **Bezpieczeństwo zamykania wątkів**: Wariant `IsBackground = true` (Zadanie 2) pozwala na szybkie zakończenie aplikacji, jednak wiąże się z ryzykiem przerwania operacji I/O lub pominięcia bloków `finally`. Zastosowanie wzorca `IDisposable` z metodą `Thread.Interrupt()` (Zadanie 3) jest rozwiązaniem profesjonalnym i zalecanym, gdyż pozwala wątkowi potomnemu na przechwycenie `ThreadInterruptedException` i bezpieczne posprzątanie zasobów.
3. **Prawidłowy dobór czasomierzy**: `System.Timers.Timer` z właściwością `AutoReset = false` doskonale sprawdza się w scenariuszach alarmów jednorazowych. Z kolei `System.Threading.Timer` oferuje optymalną wydajność w środowiskach serwerowych, minimalizując narzut na tworzenie wątków dzięki wykorzystaniu puli `ThreadPool`.
4. **Konieczność atomowości**: Metody zwrotne `TimerCallback` mogą nakładać się na siebie w środowisku wielordzeniowym, dlatego bezpośrednia operacja `counter++` prowadzi do wyścigu danych. Zastosowanie `Interlocked.Increment` gwarantuje spójność licznika na poziomie sprzętowym bez konieczності używania ciężkich blokad typu `lock`.

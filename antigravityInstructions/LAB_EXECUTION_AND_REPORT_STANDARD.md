# STANDARDIZACJA REALIZACJI ZADAŃ I STRUKTURY SPRAWOZDAŃ
## Programowanie Współbieżne (.NET / C#) — Politechnika Świętokrzyskia
**Student**: Namachynskyi Ihor | **Grupa**: 2ID12B | **Nr indeksu**: s097889  
**Prowadzący**: dr inż. Paweł Paduch  

---

## 1. Architektura Folderów i Wymagane Artefakty dla Każdego Sprawozdania

Dla każdego modułu laboratoryjnego (od `Sprawozdanie 3` do `Sprawozdanie 10`) w odpowiednim katalogu tworzona jest ściśle ustandaryzowana struktura:

```
PADUCH_Programowanie współbieżne_/
├── Sprawozdanie [X]/
│   ├── [Oryginalne pliki PDF instrukcji i wykładów - NIENARUSZONE]
│   ├── [Oryginalne kody bazowe - NIENARUSZONE]
│   │
│   ├── Solutions/                     <-- Katalog roboczy z kompletnym, kompilowalnym kodem .NET
│   │   ├── [Projekt 1]/
│   │   │   ├── Program.cs
│   │   │   └── [Projekt 1].csproj
│   │   └── Solutions.sln
│   │
│   ├── Sprawozdanie_[X].md            <-- Oficjalny raport akademicki w języku polskim
│   │                                      (zgodny z wymaganiami Katedry Systemów Informatycznych)
│   │
│   └── KODY_Z_POYASNYENNYAMI_UA.txt   <-- Повний текстовий файл з УСІМА створеними кодами розв'язків,
│                                          детально прокоментованими та поясненими українською мовою
```

---

## 2. Standard Przygotowania Raportów Akademickich (Dwa Warianty .DOCX)

Każde laboratorium musi posiadać **dwa egzemplarze raportu w formacie Word (.docx)**:
1. **Egzemplarz 1 (`Sprawozdanie_[X]_SzareTlo.docx`)**: Kody programów w tabeli 1x1 ze światło-szarym tłem (`#F5F5F5`), cienką szarą ramką (`#D9D9D9`) i wewnętrznym marginesem (padding).
2. **Egzemplarz 2 (`Sprawozdanie_[X]_BialeTlo.docx` / `Sprawozdanie_[X].docx`)**: Kody programów bezpośrednio na białym tle strony bez obramowania i bez tła.

### 2.1. Rygorystyczne Zasady Formatowania (dla obu egzemplarzy):
1. **Kolor tekstu**: Wszelki tekst (nagłówki, treść, tabele, kod programu) ma być **CZYSTO CZARNY** (`#000000`). Całkowity zakaz kolorowania składni i kolorowych czcionek.
2. **Czysty styl bez zbędnych wyróżnień**: Opisy zadań należy pisać zwykłymi akapitami czystego czarnego tekstu. Nie stosować list punktowanych, kursywy, podkreśleń, pogrubień w środku zdań ani ramek konsoli w opisach.
3. **Język raportu**: Język polski (akademicki styl techniczny).
4. **Kroje pism**:
   - Tekst główny i tabela nagłówkowa: **Times New Roman** (11–12 pt).
   - Kod źródłowy: **Consolas** lub **Courier New** (9.5–10 pt), jednolicie czarny.

### 2.2. Struktura Dokumentu:
1. **Tabela nagłówkowa (3 kolumny, czarne obramowanie)**:
   - Rząd 1 (scalony, colspan=3, wyśrodkowany, pogrubiony):
     `Politechnika Świętokrzyska`
     `Wydział Elektrotechniki Automatyki i Informatyki`
   - Rząd 2 (scalony, colspan=3, wyśrodkowany, pogrubiony):
     `Programowanie Współbieżne – Laboratorium`
   - Rząd 3 (3 osobne kolumny):
     `Numer grupy: 2ID12B` | `Autor: Namachynskyi Ihor` | `Data wykonania ćwiczenia: DD.MM.RRRR`
   - Rząd 4 (scalony, colspan=3, wyrównanie do lewej, pogrubiony):
     `Sprawozdanie nr [X]: [Temat z materiałów]`

2. **Część główna: `Przebieg ćwiczenia` (nagłówek pogrubiony)**:
   Dla każdego punktu:
   `[Numer]. [Nazwa zadania]` (pogrubione)
   Zwięzły opis (1–2 akapity) czystym czarnym tekstem bez formatowania.
   Pusty wiersz / miejsce na wklejenie zrzutu ekranu konsoli.
   Podpis: `Rysunek [N]. [Krótki podpis w języku polskim].`

3. **Kody programów w sekcji: `Kody źródłowe` (nagłówek pogrubiony)**:
   Dla każdego pliku:
   `[Numer]. [Projekt] - [Nazwa pliku .cs]` (pogrubione)
   Pełny kod programu, bez jakichkolwiek komentarzy, poprawnie sformatowany czarnym fontem Consolas.

---

## 3. Standard Pliku z Kodami i Komentarzami (`KODY_Z_POYASNYENNYAMI_UA.txt`)

Plik `KODY_Z_POYASNYENNYAMI_UA.txt` ma na celu zapewnienie studentowi pełnego zrozumienia każdego wiersza kodu oraz przygotowanie go do ewentualnej obrony sprawozdania na zajęciach.

**Wymogi strukturalne pliku**:
1. **Заголовок блоку**: Чітке позначення номера та назви лабораторної роботи, номера завдання та відповідного файлу проєкту.
2. **Повний вихідний код**: Жодних пропусків чи псевдокоду — повний діючий код класу чи файлу.
3. **Рядкові та блокові коментарі українською мовою безпосередньо в коді**:
   - Пояснення призначення кожної змінної, стану потоку чи примітива синхронізації.
   - Чому обрано саме цей примітив (наприклад, чому `AutoResetEvent`, а не `ManualResetEvent`, чому `Interlocked`, а не `lock`).
4. **Пост-аналітичний розбір (Architectural Deep Dive)** після кожного лістингу:
   - *Як це працює під капотом CLR*: поведінка черг планувальника, виділення пам'яті на стеку/купі, стан `WaitSleepJoin`.
   - *Потенційні пастки та як ми їх уникнули*: Deadlock, LiveLock, Race Condition, Thread Starvation, Closure Memory Leak.
   - *Можливі запитання викладача та відповіді на них*: готові лаконічні формулювання для успішного захисту завдання.

---

## 4. Przykładowy Szablon `KODY_Z_POYASNYENNYAMI_UA.txt`

```text
================================================================================
ПОЛІТЕХНІКА СВЄНТОКШИСЬКА В КЕЛЬЦАХ | KATEDRA SYSTEMÓW INFORMATYCZNYCH
КУРС: PROGRAMOWANIE WSPÓŁBIEŻNE (.NET / C#)
СТУДЕНТ: Namachynskyi Ihor | GRUPA: 2ID12B | INDEKS: s097889
АРХІВ ТА ПОЯСНЕННЯ ВИХІДНИХ КОДІВ: SPRAWOZDANIE NR [X]
================================================================================

--------------------------------------------------------------------------------
ЗАВДАННЯ 3.1: [НАЗВА ЗАВДАННЯ]
ФАЙЛ: Solutions/[NazwaProjektu]/Program.cs
ТЕОРЕТИЧНА ПРИВ'ЯЗКА: Лекція [X], Слайди [Y-Z]
--------------------------------------------------------------------------------

[1. ПОВНИЙ КОД З КОМЕНТАРЯМИ УКРАЇНСЬКОЮ МОВОЮ]
--------------------------------------------------------------------------------
using System;
using System.Threading;

namespace LabExample
{
    class Program
    {
        // Спільний ресурс, до якого конкурують потоки
        private static int _sharedCounter = 0;
        
        // Об'єкт блокування для створення критичної секції (монітора CLR)
        private static readonly object _locker = new object();

        static void Main(string[] args)
        {
            // Ініціалізація та запуск потоку
            Thread worker = new Thread(WorkerMethod);
            worker.IsBackground = true; // Потік не блокує завершення процесу
            worker.Start();

            worker.Join(); // Очікування завершення роботи потоку
            Console.WriteLine($"Фінальний результат: {_sharedCounter}");
        }

        private static void WorkerMethod()
        {
            // Захоплення критичної секції: гарантує атомарність модифікації
            lock (_locker)
            {
                _sharedCounter++;
            }
        }
    }
}

--------------------------------------------------------------------------------
[2. АРХІТЕКТУРНИЙ АНАЛІЗ ТА ПОЯСНЕННЯ ДЛЯ ЗАХИСТУ РОБОТИ]
- Призначення коду: Забезпечення взаємного виключення (mutual exclusion) під час
  інкременту спільної змінної.
- Чому lock: Конструкція lock (_locker) транслюється компілятором у блок
  Monitor.Enter(_locker) ... finally { Monitor.Exit(_locker); }, що запобігає
  зависанню блокування навіть у разі виникнення винятку.
- Відповідь на питання викладача: Чому не lock(this) або lock(typeof(Program))?
  Тому що блокування відкритих або глобальних типів порушує інкапсуляцію і може
  спричинити взаємне блокування (Deadlock) через зовнішні виклики. Використовується
  виключно приватний статичний об'єкт readonly object _locker.
================================================================================
```

---

## 5. Metodyka Weryfikacji Poprawności Rozwiązań

Każde zadanie przed sporządzeniem raportu przechodzi trzystopniową walidację:
1. **Walidacja statyczna i kompilacja**:
   `dotnet build --configuration Release` — brak błędów kompilacji, ostrzeżeń o wyścigach i ostrzeżeń dotyczących typów nullowalnych.
2. **Walidacja dynamiczna i poprawność logiki**:
   `dotnet run --configuration Release` — weryfikacja poprawności generowanych danych (np. czy wszystkie 10 000 liczb w unikalnej kolekcji są unikalne, czy suma 10 mln elementów zgadza się co do jedności, czy nie dochodzi do zakleszczenia przy zatrzymaniu aplikacji).
3. **Pomiary wydajnościowe**:
   Wykonywane poza debuggerem (`dotnet run -c Release` lub skrót Ctrl+F5) za pomocą `System.Diagnostics.Stopwatch` z uśrednieniem z minimum 3–5 prób, zgodnie z wymaganiami instrukcji.

---
*Niniejszy standard obowiązuje dla wszystkich sprawozdań od Laboratorium 3 do Laboratorium 10.*

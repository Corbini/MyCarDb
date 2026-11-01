# CLAUDE.md

## Środowisko / narzędzia

- **NIE używaj PowerShella.** Do operacji w terminalu korzystaj z narzędzia **Bash**
  (POSIX sh). Do operacji na plikach używaj dedykowanych narzędzi
  (Read / Write / Edit / Glob / Grep), nie `cat`, `sed -i` ani przekierowań.

- **NIE używaj Pythona.** Ani `python3 -c`, ani heredoców `python3 - <<'PY'`, ani skryptów
  pomocniczych, ani jednorazowych narzędzi do analizy danych, parsowania plików binarnych
  czy liczenia statystyk. Katalog tymczasowy nie jest wyjątkiem — „to tylko skrypt na boku"
  nie jest usprawiedliwieniem.

  Zamiast tego: `jq` do JSON-a, `grep`/`sed`/`awk`/`sort`/`uniq` do tekstu,
  `xxd`/`od`/`cmp` do danych binarnych, a do wszystkiego, co dotyczy formatu save'a i kodu
  projektu — **jednorazowy test w `MyCarDb.SaveFile.Tests`**. Parser formatu już istnieje, więc
  pisanie drugiego w innym języku jest zarówno zakazane, jak i bezsensowne.

- **Git obsługuje użytkownik.** Bez `commit`, `push`, `checkout`, `reset`, `revert`,
  `stash`, `clean` bez wyraźnego polecenia w tej samej wiadomości. `main` jest gałęzią
  chronioną — nie commituj na nią i nie proponuj obejścia ochrony. Czytanie wolno:
  `git status`, `git diff`, `git show`, `git log`, `git branch`.

- **Buildy i testy wolno uruchamiać.** `dotnet build`, `dotnet test`, `dotnet publish`,
  `dotnet format` — bez pytania. Artefakty lądują w ignorowanych `bin/`/`obj/`.
  Uwaga: w working tree bywają nieśledzone pliki roboczej pracy użytkownika, więc
  **nigdy `git clean -xfd`**.

## Zanim powiesz, że skończone

Zmiana w kodzie jest skończona, gdy przechodzą te same trzy bramki, które puszcza CI —
nie gdy się kompiluje:

```bash
dotnet test MyCarDb.slnx -c Release
dotnet format MyCarDb.slnx --verify-no-changes
dotnet build MyCarDb.slnx -c Release -warnaserror -p:EnforceCodeStyleInBuild=true -p:GenerateDocumentationFile=true -p:NoWarn=CS1591
```

`-warnaserror` z `EnforceCodeStyleInBuild` oznacza, że **nieużywany `using` wywala build**.
Po usunięciu kodu sprawdź, czy jego `using`i nadal mają zastosowanie.

## Jak przeprowadzać analizę

**Weryfikacja przed sformułowaniem wniosku, nigdy po.** Odwrotna kolejność kończy się
przywiązaniem do tezy, która dobrze brzmi, i szukaniem dla niej potwierdzenia — a wtedy brak
dowodu przestaje być widoczny.

**Pod każdym twierdzeniem musi dać się wskazać plik i linię.** „Prawdopodobnie", „zwykle
w takich projektach", „pewnie tak było" nie są dowodami.

**„Sprawdzone" znaczy przeczytana implementacja.** Nazwa pliku, katalogu ani klasy nie
dowodzi zachowania — patrz lista faktów niżej, gdzie prawie każda pomyłka wzięła się
z nazwy.

**Wszystkie pliki danej kategorii, nie pierwszy znaleziony.** Jeden workflow nie opisuje CI,
jeden `.csproj` nie opisuje grafu zależności, jeden fixture nie opisuje pokrycia.

**Zakres twierdzenia równa się zakresowi dowodu.** Round-trip na pliku niezmienionym nie
dowodzi niczego o edycjach. Dziewięć parserów plus fallback to nie dziesięć rodzajów.

**Liczba podana razem z metodą liczenia.** Policzone atrybuty `[Fact]`/`[Theory]` to nie
liczba przypadków testowych, bo `[Theory]` się rozwija.

**Nie wymyślać faktów, których w repo nie ma.** Intencje autora, historia powstania i to,
czy format był udokumentowany — tego kod nie zawiera. Takie rzeczy wolno **zapytać**.

**Zaniżenie jest błędem dokładnie tak samo jak zawyżenie.**

## Fakty o tym repo, których nie wolno znowu przekręcić

Każdy z tych punktów to pomyłka popełniona w sesji 2026-08-22/23, wzięta z nazwy pliku albo
z zatrzymania się na pierwszym trafieniu:

- **CI to trzy workflowy, nie jeden.** `build-and-test.yml` (PR, windows, testy + artefakt
  trx), `code-quality.yml` (PR; **ubuntu**: `dotnet format --verify-no-changes`; windows:
  build z `-warnaserror`), `release.yml` (tagi `v*`: windows — `dotnet publish
  --self-contained` dla `MyCarDb.Desktop`, licencje trzecich stron, GitHub Release; ubuntu —
  obraz `MyCarDb.Web` do `ghcr.io/corbini/mycardb`). Twierdzenie „CI działa tylko na windows"
  jest fałszywe. Wszystkie restore idą przez `nuget.github.config` (GitHub Packages, token
  z `GITHUB_TOKEN`), a stara aplikacja Avalonia (`src/MyCarDb.Old`) nie jest w `MyCarDb.slnx`.
- **`AvaloniaStyles.Generators.Tests` nie testuje generatora.** Testuje `SvgParser`. Nie ma
  ani jednego testu uruchamiającego `SvgSourceGenerator` i sprawdzającego wygenerowany kod.
- **Żaden test nie mutuje wartości przed serializacją.** `RoundTrip_SavefileShouldBeBitwise‑
  Identical` dowodzi tożsamości `parse → serialize` na pliku **nietkniętym**. O poprawności
  edycji nie mówi nic.
- **Są dwa prawdziwe save'y z gry:** `savefile.txt` (35 158 B, 649 wpisów) i `carparts.txt`
  (70 952 B, 1930 wpisów). Round-trip bitowy działa **tylko na pierwszym**; drugi ma jedynie
  asercje `NotEmpty` i `Name != null`.
- **Oba pliki zawierają wyłącznie znane typy** — zero wpisów trafiających w fallback
  `RawValue`. Ścieżkę raw pokrywa tylko test syntetyczny
  `RoundTrip_UnknownTypeShouldSurviveAsRawValue`.
- **`DumpSavefileAsJson` nie jest testem** — nie ma asercji na treści i zapisuje
  `savefile.json` do katalogu głównego repo przy każdym uruchomieniu testów, także na CI.
- **Bez zależności jest tylko `MyCarDb.SaveFile`.** `MyCarDb.GameWorld` ma `DependencyInjection.Abstractions`
  i referencję do `MyCarDb.SaveFile`. „Biblioteki rdzenia bez zależności" w liczbie mnogiej jest
  nieprawdą.
- **Nic nie wymusza kierunku zależności.** Nie ma `Directory.Build.props`, analizatora ani
  testu architektonicznego. Referencji po prostu nie ma — to nie to samo co „wymuszone".
- **Trimmer usuwa backendy obcych platform.** Przy `-r linux-x64` z publikacji wypadają
  `Avalonia.Win32`, `Avalonia.Native` i `Avalonia.MicroCom`, bo `Avalonia.Desktop` używa
  `OperatingSystem.IsX()`, które trimmer podmienia na stałe. Zejście z `Avalonia.Desktop`
  na sam backend nie da nic.
- **`xwininfo -root -tree` nie mierzy startu aplikacji** — wypisuje też okna niezamapowane,
  czyli moment utworzenia okna (~400 ms), nie pierwszą klatkę (~720 ms). Do pomiaru startu
  służy `Compositor.RequestCommitAsync()` w handlerze `Opened`.

## Kształt kodu

- **Nie dorabiaj atrap, żeby typ pasował do interfejsu.** `RawValueParser` nie implementuje
  `IValueParser<T>`, bo jego `Parse` potrzebuje `typeId` — dla niego typ jest daną, nie
  stałą. Poprawną odpowiedzią była własna sygnatura, nie `Parse(name, data)` rzucające
  wyjątkiem ani parametr dodany do dziewięciu parserów, które go zignorują.
- **`TypeId` jest `const`, bo jest etykietą `case`.** Przeniesienie go na właściwość
  instancyjną wymusza likwidację `switch` — to jedna decyzja, nie dwie.
- **Bez komentarzy i bez XML-doców**, zgodnie z resztą kodu. Nazwy mają nieść znaczenie.
- **Tylko zakres z polecenia.** Bez metod „na później", bez walidacji, o którą nikt nie
  prosił, bez przebudów zgłaszanych jako gratis.

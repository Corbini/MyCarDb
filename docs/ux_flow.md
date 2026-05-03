# 🧩 Save Editor – Specyfikacja UX

Edytor sejwów **My Summer Car** w stylu **macOS document app** (Apple HIG). Inspiracje: *Pages*, *Xcode*, *Mail*, *Notes*. Świat gry — mapa **Peräjärvi (Alivieskan kunta, 1989)** — pełni rolę głównego widoku dokumentu.

---

## 🧠 Założenia ogólne

- **Document app, nie overlay app.** Typowy układ macOS Big Sur+: **unified title bar (48 px)** + sidebar / content / inspector. Bez osobnego toolbara, bez status bara. Brak floating cards rzucanych na mapę.
- **Sidebar** to source list w stylu Finder/Mail: szukanie + lista przypiętych + kategorie obiektów na mapie. Bez mode pickerów ani trybów.
- **Mapa jest content area** dokumentu — zachowuje się jak płótno w Pages: zoom/pan, ikony obiektów, selekcja.
- **Inspector** (prawa kolumna) pokazuje szczegóły wybranego obiektu — jak inspektor w Pages/Numbers/Xcode.

---

## 🗺 Layout

| # | Warstwa | Pozycja | Rola |
|---|---------|---------|------|
| 1 | **Title bar (unified)** | góra, 48 px | tytuł sejwa + dirty marker + klikalne sekcje stanu + akcje globalne (Zapisz, Pełny widok auta, Inspector toggle) + window controls |
| 2 | **Sidebar** | lewa kolumna, ~240 px | search w headerze (Notes-style) + przypięte + kategorie obiektów na mapie (z disclosure) |
| 3 | **Map view** | środek, fluid | mapa Peräjärvi z ikonami obiektów + map controls |
| 4 | **Inspector** | prawa kolumna, ~320 px (toggleable) | szczegóły zaznaczonego obiektu |

**Bez status bara.** Apple-style document apps (Pages, Numbers, Mail, Notes) trzymają informacje stanowe w title barze.

Sidebar i inspector są dokowane (jak w Xcode), nie floating. Można je zwijać:
- sidebar — `Ctrl+Alt+S` lub przycisk `⊐` po lewej stronie title bara
- inspector — `Ctrl+Alt+I` lub przycisk `⊓` po prawej stronie title bara (przed window controls)

---

## 🏠 Welcome screen (stan bez wczytanego sejwa)

Gdy aplikacja startuje bez sejwa lub user zamknął bieżący plik, layout transformuje się — **sidebar zostaje** (z inną zawartością), mapa znika, miejsce zajmuje welcome content. Wzorzec **Xcode welcome window**: sidebar z recent projects + main area z akcjami startowymi.

### Layout welcome

```
┌─────────────────────────────────────────────────────────────┐
│ [⊐]  brak wczytanego sejwu                  [_][□][×]       │  title bar minimal
├──────────────────┬──────────────────────────────────────────┤
│  OSTATNIE        │                                          │
│   📄 save_01.dat │             MyCarDb                      │
│   📄 save_02.dat │   Edytor sejwów My Summer Car            │
│   📄 campaign…   │                                          │
│   📄 test_save…  │      [📁 Otwórz sejw…  Ctrl+O]            │
│                  │                                          │
│                  │           ─── lub ───                    │
│                  │                                          │
│                  │      ┌─ ⬆ Przeciągnij ──┐                │
│                  │      │   plik sejwa     │                │
│                  │      └──────────────────┘                │
│  ⚙              │                                          │
└──────────────────┴──────────────────────────────────────────┘
```

### Różnice względem stanu editor

| Element | Welcome | Editor |
|---|---|---|
| Title bar — tytuł | „brak wczytanego sejwu" (italic, tertiary) | filename + state |
| Title bar — akcje globalne | brak | `📁 💾 🚗 ⊓` |
| Title bar — sidebar toggle `[⊐]` | tak (symetria z editor) | tak |
| Sidebar — header | brak search (lista krótka) | search Notes-style |
| Sidebar — content | `OSTATNIE` (recent files) | `PRZYPIĘTE` + `NA MAPIE` |
| Sidebar — footer | ⚙ Ustawienia | ⚙ Ustawienia |
| Main area | hero + Otwórz button + drag-drop | mapa Peräjärvi |
| Inspector | brak (nic nie wybrano) | dynamiczny |

### Transition welcome → editor

Po wczytaniu pliku (klik w `Otwórz sejw…`, klik w pozycję `OSTATNIE`, drag-drop pliku):

1. Sidebar zachowuje strukturę (nadal 244 px, nadal vibrancy), ale **content się zmienia**:
   - dodaje się sidebar header (`🔍 Szukaj…`)
   - `OSTATNIE` znika, pojawiają się `PRZYPIĘTE` + `NA MAPIE`
2. Title bar uzupełnia się: pojawia się filename, akcje globalne `📁 💾 🚗 ⊓`
3. Main area: welcome content znika, wczytuje się mapa Peräjärvi
4. Inspector pojawia się z prawej (toggle-able)

### Drag-drop hint

Welcome ma **dropzone widget** w main area (dashed border + ikona upload, hover state na akcent). W stanie editor drag-drop też działa, ale bez dedykowanego UI.

### Settings dostępne także w welcome

Klik ⚙ w sidebar footer otwiera ten sam Settings sheet co w editorze — ustawienia są app-level, nie file-level. User może zmienić motyw / język bez wczytywania pliku.

---

## 🪧 Title bar — unified

Pas 48 px łączący chrome okna, stan dokumentu i akcje globalne. Apple Big Sur+ unified bar pattern.

### Layout (lewa → prawa)

```
┌──────────────────────────────────────────────────────────────────────────────┐
│ [⊐]  • save_01.dat — 3 niezap. — ⚠ Konflikt   [📁][💾 Zapisz][🚗][⊓]  [_][□][×] │
└──────────────────────────────────────────────────────────────────────────────┘
   ↑   ↑                                          ↑                       ↑
   sidebar  nazwa pliku + stan (klikalne)         akcje globalne          window
   toggle                                                                 controls
```

**Tytuł = nazwa pliku.** Bez ikony aplikacji — identyfikator aplikacji żyje w pasku zadań Windows.

### Sekcja stanu pliku (lewa, klikalne)

```
clean:        save_01.dat
dirty:        • save_01.dat  —  3 niezapisanych
reload warn:  save_01.dat  —  ↻ Plik zmieniony
conflict:     • save_01.dat  —  ⚠ Konflikt
```

(W stanie konflikt pill `niezapisanych` jest ukryty — patrz „Konsolidacja" niżej.)

### Dirty popover — lista zmian z per-row Cofnij

Klik w `N niezapisanych` → popover anchored pod pill, 360 px:

```
╭─ Niezapisane zmiany       3 zmian ─╮
│  Silnik V8                          │
│  stan: 87 → 90      [Cofnij][Idź]  │
│  ───────────────────────────────── │
│  Satsuma                            │
│  paliwo: 12 L → 28 L [Cofnij][Idź] │
│  ───────────────────────────────── │
│  Apteczka                           │
│  pozycja: przeniesiona [Cofnij][Idź]│
│  ───────────────────────────────── │
│  [Cofnij wszystkie] [Zapisz Ctrl+S]│
╰─────────────────────────────────────╯
```

`[Idź]` zaznacza obiekt na mapie. Stara wartość przekreślona + przygaszona, nowa w akcent color. Klik poza popoverem lub `Esc` zamyka.

### Reload sheet — slide-down dialog dla synchronizacji z grą

Klik w `↻ Plik zmieniony` lub `⚠ Konflikt` → modal sheet (480 px), slide-down z górnej krawędzi okna.

#### Stan: warn (`Plik zmieniony`, brak lokalnych zmian)

```
╭─ Plik został zmieniony przez grę ─────╮
│            ╭───╮                       │
│            │ ↻ │                       │
│            ╰───╯                       │
│  Plik save_01.dat na dysku jest        │
│  nowszy niż wczytany. Twoje lokalne    │
│  zmiany: brak. Wczytać aktualną?       │
│                                        │
│       [ Anuluj ]      [ Wczytaj ]      │
╰────────────────────────────────────────╯
```

#### Stan: conflict (lokalne zmiany + plik zmieniony)

```
╭─ Konflikt z plikiem ───────────────────╮
│            ╭───╮                       │
│            │ ⚠ │                       │
│            ╰───╯                       │
│  Plik save_01.dat został zmieniony     │
│  przez grę, a Ty masz 3 niezapisanych  │
│  zmian. Co zrobić?                     │
│                                        │
│  [    Zachowaj moje (nadpisz dysk)]    │
│       [ Anuluj ] [ Wczytaj z dysku ]   │
│  „Wczytaj z dysku" usunie zmiany.      │
╰────────────────────────────────────────╯
```

Klik poza sheetem **nie zamyka** (force conscious choice dla destructive flow). `Esc` lub `Anuluj` zamyka bez akcji.

**Pominięte na razie:** akcja `[Pokaż różnice…]` — wymaga pełnego diff engine'u dla binarnych sejwów (porównanie obiektów, fields, pozycji). Klasyczny rabbit hole. Dodać po MVP, gdy parser sejwa jest stabilny i rozumiemy strukturę.

### Konsolidacja stanu

W title barze widoczny jest tylko najpoważniejszy stan (konflikt implikuje dirty, więc oba pill nie są pokazywane razem):

| Stan systemu | Co widać w title barze |
|---|---|
| clean | `save_01.dat` |
| dirty (lokalne edycje) | `• save_01.dat — 3 niezapisanych` |
| file changed (czyste lokalnie) | `save_01.dat — ↻ Plik zmieniony` |
| **conflict** (oba) | `• save_01.dat — ⚠ Konflikt` |

Dirty marker `•` to ambient indicator — pojawia się gdy są lokalne zmiany, niezależnie od którego pill jest widoczny.

### Sekcje stanu (klikalne) — szczegółowo

| Sekcja | Kolor | Widoczność | Klik |
|---|---|---|---|
| Dirty marker `•` przed nazwą | accent | gdy `modifications > 0` | – (ambient) |
| `N niezapisanych` | accent (system blue) | gdy `modifications > 0` | popover z listą zmian: `Silnik V8: stan 87→90`, każdy wiersz z `[Cofnij]` + `[Idź do]` |
| `↻ Plik zmieniony` | warn (orange) | gdy plik na dysku zmieniony przez grę, brak konfliktu | otwiera sheet reload z `[Anuluj]` `[Wczytaj]` |
| `⚠ Konflikt` | danger (red) | gdy plik zmieniony + lokalne edycje | otwiera sheet konflikt z `[Zachowaj moje]` `[Anuluj]` `[Wczytaj z dysku]` |

### Affordance klikalności

Sekcje stanu są stylowane jako **subtle clickable text** — bez tła w stanie idle, hover dodaje delikatny background sygnalizujący że to button.

### Synchronizacja z plikiem — flow detekcji

Gra zapisuje plik sejwa niezależnie od edytora. App monitoruje plik (`FileSystemWatcher` po `mtime` + hash zawartości):

1. Gra zapisuje plik → watcher wykrywa zmianę → app porównuje hash.
2. Jeśli różny → modal **sheet** (slide-down):
   - **Bez konfliktu:** `Plik został zaktualizowany przez grę. [Anuluj] [Wczytaj]`
   - **Z konfliktem:** `Plik zaktualizowany. Masz N niezapisanych zmian. [Zachowaj moje] [Anuluj] [Wczytaj z dysku]`
3. Po `Anuluj` — sekcja w title barze pozostaje aktywna (`↻ Plik zmieniony` lub `⚠ Konflikt`) jako trwały reminder.
4. Klik w sekcję title bara w dowolnym momencie → otwiera sheet ponownie.

**Klawiatura:** `Ctrl+R` = otwórz sheet reload.

**Klawiatura:** `Ctrl+S` = zapisz, znika dirty marker + sekcja `N niezapisanych`.

### Akcje na lewej krawędzi (przed tytułem)

| Przycisk | Stan | Klik | Skrót |
|---|---|---|---|
| ⊐ **Sidebar toggle** (icon-only) | active gdy sidebar widoczny | toggle lewej kolumny | `Ctrl+Alt+S` |

Sidebar toggle po lewej krawędzi, inspector toggle po prawej — symetria względem panelu który toggluje.

### Akcje globalne (prawa, przed window controls)

Cztery przyciski akcji obok window controls:

| Przycisk | Stan | Klik | Skrót |
|---|---|---|---|
| 📁 **Otwórz** (icon-only) | zawsze aktywny | otwiera dialog wyboru pliku sejwa. Right-click → submenu „Otwórz ostatnie ›" (planowane) | `Ctrl+O` |
| 💾 **Zapisz** (primary, ikona + tekst) | enabled gdy `dirty > 0`, disabled gdy clean | zapisuje sejw na dysk, czyści dirty state | `Ctrl+S` |
| 🚗 **Pełny widok Satsumy** (icon-only) | **zawsze aktywny** | otwiera Advanced sheet z top-down schematem Satsumy | `Ctrl+E` |
| ⊓ **Inspector toggle** (icon-only) | active state gdy inspector widoczny | toggle prawej kolumny | `Ctrl+Alt+I` |

**Skróty klawiaturowe — tylko w tooltipach**, nie na buttonach. Ikona + krótki tekst dla Save (primary), same ikony dla pozostałych.

### Wzorzec UX: contextual action vs resource shortcut

| Wzorzec | Charakterystyka | Stan przycisku | Przykłady Apple |
|---|---|---|---|
| **Contextual action** | działa NA zaznaczonym obiekcie, efekt zależy od selekcji | enabled tylko gdy aplikowalny | Photos `Edit`, Mail `Reply` |
| **Resource shortcut** | stała akcja → konkretne miejsce, niezależna od selekcji | zawsze enabled | Music `Now Playing`, Mail `Get Mail`, Maps `My Location` |

`Pełny widok Satsumy` używa **obu wzorców** — w title barze jako resource shortcut (zawsze), w inspectorze jako contextual action (tylko gdy Satsuma zaznaczona). Dwie różne ścieżki user flow: „chcę zobaczyć Satsumę" vs „już patrzę na Satsumę, daj więcej".

---

## 📚 Sidebar (source list)

Lewa kolumna, ~240 px, `vibrancy` background w stylu Finder/Mail.

### Header — search

Pole wyszukiwania w stylu Notes.app.

```
┌──────────────────────────┐
│ 🔍  Szukaj…              │
├──────────────────────────┤
│ PRZYPIĘTE                │
│   Silnik V8              │
│   ...                    │
```

`Ctrl+F` daje fokus. Aktywne wyszukiwanie pokazuje sekcję `WYNIKI (n)` zaraz pod search.

### Sekcje (Apple-style source list — uppercase tytuły, `text-tertiary`)

#### `PRZYPIĘTE`
Lista shortcutów do najczęściej edytowanych obiektów (np. Silnik V8, Satsuma). Klik → selekcja + inspector + centrowanie mapy.

#### `NA MAPIE` — kategorie obiektów z pozycją w świecie

| Wiersz       | Liczba | Filtr mapy                              |
|--------------|--------|-----------------------------------------|
| Pojazdy      | 4      | tylko obiekty typu `Vehicle`            |
| Części       | 42     | tylko obiekty typu `Part`               |
| Przedmioty   | 127    | tylko obiekty typu `Item`               |
| Lokacje      | 12     | tylko punkty interesujące (sklepy, dom) |

Klik na wiersz = filtr mapy + lista wyników w inspectorze (gdy żaden pojedynczy obiekt nie jest wybrany). Każdy wiersz ma chevron — rozwija dziecko jako podlistę (np. **Pojazdy ▾** → satsuma / hayosiko / kekmet / ferndale).

Selected row w całym sidebarze jest podświetlony **system blue** z białym tekstem (HIG source list).

### Footer — ustawienia

Na dole sidebara, w stylu Mail.app / Notes.app — mała ikonka **⚙ Ustawienia** (icon-only, ~28 px). Settings są app-level (nie file-level), więc dostępne w obu stanach aplikacji (welcome i editor). Klik otwiera **Settings sheet** (modalny, ~560 px).

Sheet header: `[Anuluj]  Ustawienia  [Gotowe]`.

#### Sekcje sheet'a

##### Wygląd
- Segmented control `Auto / Jasny / Ciemny`
- `Auto` = follows `prefers-color-scheme` z systemu Windows
- `Jasny` / `Ciemny` ustawia `<html data-theme="light|dark">`, nadpisując system
- Hint pod kontrolką: „Auto dopasowuje się do motywu systemu Windows"

##### Język
- Dropdown: `Polski` (aktywny), `English (planowane)` — placeholder na przyszłość
- Polski jest domyślny (UI nie ma sensu po angielsku skoro MSC jest fińską grą z polską społecznością modderską)

##### Sejw
- Checkbox `Tworzyć kopię .bak przy zapisie` — auto-backup, hint: „Bezpieczeństwo na wypadek nieudanego zapisu lub błędu parsera"
- Checkbox `Wykrywaj zmiany pliku przez grę` — toggle dla `FileSystemWatcher`, hint: „Wymaga FileSystemWatcher. Wyłącz jeśli powoduje problemy"

##### Skróty klawiaturowe
Read-only reference list — wszystkie skróty z kanoniczną tabelą, monospace badges (`Ctrl+F`, `Ctrl+O`, etc.). Cele: discoverability dla power-userów.

Wywoływany skrótem `Ctrl+,`. `Esc` / `Anuluj` / `Gotowe` zamyka.

---

## 🌍 Map view

Środek okna. Tło: bitmap `docs/map.png` (mapa Peräjärvi z 1989), 1:1, wyśrodkowany.

### Obiekty na mapie

- **Markery** = okrągłe pinezki w stylu Apple Maps (~28 px), z ikoną SF Symbol w środku zależną od typu obiektu (część/przedmiot/auto/gracz).
- **Selekcja** — pierścień akcent-color + scale 1.15× + soft shadow.
- **Hover** — scale 1.1×.
- **Kliknięcie** — zaznacza, otwiera Inspector (jeśli był ukryty), centruje mapę na obiekcie z animacją.

### Map controls (prawy dolny róg mapy)

Zgrupowane pionowo, w stylu Apple Maps controls (białe karty z borderem 0.5 px):

- 📍 **Centruj na graczu** (`Ctrl+L`)
- ➕ **Zoom in** (`Ctrl++`)
- ➖ **Zoom out** (`Ctrl+-`)

### Weather chip (prawy górny róg mapy) — ambient indicator

Pływający chip pokazujący aktualny stan świata, w stylu Apple Maps weather chip:

```
[ ☀ 14:00 · Środa ]
```

- Tylko **wskaźnik** (read-only) — pokazuje godzinę (24h, bez minut), dzień tygodnia, ikonę pogody.
- **Klik** → wybiera pseudo-obiekt `World/Time` → inspector otwiera widok `Czas i pogoda` (patrz sekcja Inspector).
- W stanie aktywnym: subtle ring akcent-color wokół chipu (analog do `selected` na markerach).
- Klik markera na mapie odznacza chip i wraca do widoku obiektu w inspectorze.

### Interakcje mapy

- Pan: drag (lub `space + drag`)
- Zoom: scroll wheel / pinch / `Ctrl + scroll`
- Klik w pustkę: odznacz wszystko, inspector wraca do empty state („Wybierz obiekt")
- Double-click: zoom-in do punktu

---

## 🔍 Inspector

Prawa kolumna, ~320 px, dokowana, można ukryć przyciskiem `⊓` w title barze (lub `Ctrl+Alt+I`). Treść zależy od selekcji.

### Brak selekcji

Pusty stan w stylu Apple — wycentrowana ikona + napis:
```
              ⦿
       Wybierz obiekt
```

### Obiekt zaznaczony

Header inspectora:
- nazwa obiektu (`title3`, weight `semibold`)
- subtitle: typ obiektu (np. `Część samochodowa`)
- po prawej: `📌` (przypnij) `✕` (zamknij = ukryj inspector)

Sekcje (każda `inspector-section` z uppercase headerem):

- **STAN** — pasek progresu sprawności / poziomu
- **WŁAŚCIWOŚCI** — edytowalne pola (Producent, Wartość, Nazwa…)
- **POZYCJA** — X / Y / Z + przycisk `↩ Centruj na mapie`
- **AKCJE** — `[Resetuj zmiany]` (scoped undo dla tego obiektu). Dla Satsumy dodatkowo `[Otwórz pełny widok]` (contextual — proximity gdy user już patrzy na Satsumę). Bez globalnego `[Zapisz]` (jest w title barze) i bez `[Usuń]` (deletion w MSC sejwie to footgun — większość obiektów jest inherentna dla świata gry).

### Czas i pogoda (pseudo-obiekt `World/Time`)

Otwierany kliknięciem **Weather chip** w prawym górnym rogu mapy. Inspector pokazuje dedykowany widok zamiast widoku obiektu:

- **Header**: ikona pogody + tytuł `Czas i pogoda`, subtitle `Świat · ustawienia globalne`, akcja `✕`.
- **CZAS** — godzina (number 0–23, bez minut), dzień tygodnia (Pon–Nie).
- **POGODA** — Apple-style picker (lista z checkmarkiem), 4 opcje: `☀ Clear Sky`, `☁ Overcast`, `❄ Snowy`, `🌨 Blizzard`. Brak sezonów (MSC nie ma sezonów).
- **TIMETABLE — TERAZ** — derywowane z aktualnej godziny/dnia: które miejsca są OPEN, które za ile się otworzą (Repair shop, Inspection Office, Sewage Treatment, Teimo's).
- **HARMONOGRAM** — pełna tabela read-only ze stałymi wydarzeniami: PSK (24h), Sunrise (07–09), Jokke kilju (06–22), Repair/Inspection (Mon–Fri 08–16), Rally (Sat/Sun 10–18), Teimo's Restaurant (Mon–Sat 10–20), Teimo's Pub (Mon–Sat 20–02), Taxi Meeting (12–13).

---

## 🛠 Advanced sheet — pełny widok auta

Wywoływany z title bara przyciskiem `🚗` lub skrótem `Ctrl+E`. Apple-style **sheet** (slide-down z modalnym tłem) zajmujący ~85% okna. Jedyny obiekt z dedykowanym sheetem na razie — **wzorzec reużywalny**, kandydaci na przyszłość: `Player`, `Trigger flags`.

### Layout sheeta

| Element | Pozycja | Rola |
|---|---|---|
| Sheet header | góra, 52 px | `[Anuluj]` `Satsuma — pełna edycja` `[Gotowe]` |
| Schematic (lewa) | ~200 px | rzut Satsumy z klikalnymi strefami + toggle Góra/Spód w headerze |
| Form panel (prawa) | reszta | formularz zmienia się po kliknięciu strefy na schemacie |

### Schemat — toggle Góra / Spód

Schemat ma dwie warstwy widoku, przełączane segmented control'em w headerze panelu (Apple Watch face/back pattern):

- **Góra** — widok z lotu ptaka. Pokazuje karoserię, hood/cabin/trunk + opony×4. Domyślny widok przy otwarciu sheeta.
- **Spód** — widok podwozia. Pokazuje skidplate, oś przednia, drivetrain, oś tylna, wydech + tarcze hamulcowe×4.

Przełączenie widoku resetuje selekcję strefy i wraca form panel do `Stan ogólny`.

### Strefy — widok Góra

| Zona | Klikalność | Zawartość form panel'u |
|---|---|---|
| **Karoseria** (silhouette) | klikalna gdy klik nie trafi w hood/cabin/trunk | lakier, RGB body, dent state, zderzaki, lusterka |
| **Maska / Silnik** | hot-zone w obrębie maski | świece×4, paski, filtr oleju, pompa paliwa, chłodnica, AFR gauge, REV limiter, oleje, akumulator |
| **Wnętrze** | hot-zone w obrębie kabiny | kierownica, zegary, hamulec ręczny, klimat, audio, wszystkie `Corris*` |
| **Bagażnik** | hot-zone w obrębie bagażnika | koło zapasowe, subwoofer, klapa |
| **Koło FL/FR/RL/RR** (Góra) | tarcza/opona | felga (`RIM*`), opona (stan/typ/ciśnienie), geometria (toe, camber) |

Karoseria highlight'uje się **drop-shadow glow na zewnątrz silhouette** (nie filter brightness — żeby nie podświetlać hood/cabin/trunk razem z nią).

### Strefy — widok Spód

Drivetrain Satsumy renderowany jako **dwie ciągłe figury** (po jednym `<path>` SVG na grupę), nie jako klocki z wewnętrznymi szwami — wizualnie nierozróżnialne wewnątrz grupy.

| Zona | Kształt | Zawartość form panel'u |
|---|---|---|
| **Skidplate** | mała kostka nad osią przednią | osłona dolna silnika |
| **Oś przód** | pozioma belka między przednimi kołami | sway bar, drążek skrętny, front links L/R, steering rack, steering shaft |
| **Skrzynia + wał** (drivetrain) | jedna inverted-T figura: szeroka głowa skrzyni → wąski wał kończący się na bulwie housingu osi tylnej | skrzynia (`GEARBOX5SPD`, bieg, stan, olej), sprzęgło (tarcza, pressure plate, cover plate, cable, crossmember), wał (stan, wytrzymałość) |
| **Oś tył** | jedna pozioma belka z lekkim wybrzuszeniem centralnym (banjo housing, dyfer zintegrowany — w MSC nie ma osobnego dyfera) | dyferencjał (stan, olej diff), oś tylna (stan, sway bar tył) |
| **Wydech** | rurka biegnąca prawą stroną podwozia + tłumik na końcu | manifold, pipe front, pipe rear, tłumik, side pipe (mod) |
| **Koło FL/FR/RL/RR** (Spód) | tarcza z czerwonym kółkiem hamulca | amortyzator, sprężyna, control arms upper/lower, klocki hamulcowe |

**Wycentrowanie:** front axle na środku przednich kół (y=105), rear axle na środku tylnych kół (y=395). Wał nie nakłada się z osią tylną — kończy się dokładnie na czubku bulwy dyfera.

### Status visualny

Każda strefa pokazuje **status** ambient'owo: czerwony badge `!` w rogu strefy z elementami w złym stanie (np. zużyta świeca → badge na `Maska / Silnik`).

### Zamykanie

- `Esc` / `[Anuluj]` — discard zmian, zamyka.
- `[Gotowe]` — commit zmian (do dirty state, nie do pliku — to nadal `Ctrl+S`).
- Klik w tło (backdrop) NIE zamyka — wymuszamy świadomy choice (Apple HIG dla destructive flow).

---

## 📋 Stan wyszukiwania w sidebarze

Gdy `Search` w sidebar header jest aktywny, w sidebarze pojawia się dynamiczna sekcja **WYNIKI** zaraz pod `PRZYPIĘTE`:

```
WYNIKI (12)
  Silnik V8
  Silnik diesla
  Tłumik
  …
```

Sekcja zastępuje (lub poprzedza) `NA MAPIE` dopóki search jest niepusty. `Esc` w polu search → znika.

---

## 🎯 Skróty klawiaturowe (Windows)

Aplikacja na Windows — wszystkie skróty są Windows-canonical. Na pierwsze miejsce idzie `Ctrl`. Redo używa `Ctrl+Y` (Windows convention), nie `Ctrl+Shift+Z` (Apple).

| Skrót               | Akcja                                |
|---------------------|--------------------------------------|
| `Ctrl+F`            | fokus na search                      |
| `Ctrl+O`            | otwórz sejw                          |
| `Ctrl+S`            | zapisz                               |
| `Ctrl+Z` / `Ctrl+Y` | undo / redo                          |
| `Ctrl+E`            | pełny widok Satsumy                  |
| `Ctrl+L`            | centruj na graczu                    |
| `Ctrl+R`            | otwórz sheet reload                  |
| `Ctrl+Alt+S`        | toggle sidebar                       |
| `Ctrl+Alt+I`        | toggle inspector                     |
| `Ctrl++` / `Ctrl+-` | zoom mapy                            |
| `Ctrl+0`            | reset zoom                           |
| `Ctrl+,`            | otwórz Settings sheet                |
| `Esc`               | odznacz / zamknij sheet/popover      |

---

## 🔁 Interakcje – podsumowanie

### Wyszukiwanie
Pole search w sidebar header filtruje globalnie. Wyniki widoczne na mapie + sekcja **Wyniki** w sidebarze (poprzedza `NA MAPIE` dopóki search jest niepusty).

### Selekcja
Klik obiektu (na mapie lub w liście sidebara):
1. zaznacz
2. otwórz inspector (jeśli był ukryty)
3. wypełnij inspector danymi
4. centruj mapę z animacją

### Edycja
- Inline w polach inspectora.
- Każda zmiana → +1 do `N niezapisanych` w title barze.
- `Ctrl+S` zapisuje, `Ctrl+Z` cofa.
- Auto-save **wyłączony** domyślnie (gra mogłaby załadować niedokończony stan).

### Synchronizacja z grą
Patrz sekcja **Synchronizacja z plikiem — flow detekcji** w Title barze.

### Przypinanie
`📌` w inspectorze → obiekt ląduje w sekcji **PRZYPIĘTE** w sidebarze.

---

## 🧠 Model mentalny

Trzy panele + jedna belka — każdy ma jedną odpowiedzialność:

- **Title bar** → „co dzieje się z plikiem?" (filename, stan, akcje globalne, panel toggles)
- **Sidebar** → „co chcę widzieć?" (search + filtr/scope)
- **Mapa** → „gdzie to jest?" (przestrzenny kontekst)
- **Inspector** → „jakie ma właściwości?" (detal zaznaczonego obiektu)

# SbekuMod: Echoes Of The Desert

An Italian-language mod for **Outer Wilds** and its **Echoes of the Eye** DLC, built
with [OWML](https://github.com/ow-mods/owml).

*[Versione italiana più in basso.](#italiano)*

---

## English

### What it is

*Echoes Of The Desert* turns Echoes of the Eye into a guided completionist run,
scattering a set of custom slide reels through the Stranger (one for each of the
game's achievements) and tying them together with new dialogue, extra ship log
entries, its own signalscope frequency, and a full custom credits sequence. All
in-game text is in Italian.

### Why it exists

This mod was not built as a general-purpose release. It was made as a gift for the
Italian streamer **Sabaku No Maiku** ("Sbeku") to be played on stream, which is why
the game greets him by name the moment you reach the main menu.

### Features

- **Custom slide reels** hidden throughout the Stranger, one per Outer Wilds
  achievement, including *Around the World*, *The Grate Filter*, *Ghost in the
  Machine*, *Fire Arrows*, *Tubular* and *Hypothesis*. They are placed at authored
  positions by `utils/ReelSetup.cs`.
- **A dedicated signalscope frequency** (`STORY_REELS`), taught automatically the
  first time you equip the Signalscope, so that the reels can be tracked down by
  signal.
- **New and rewritten character dialogue** across 69 dialogue trees under
  `assets/dialogues/`, covering Chert, Gabbro, Riebeck, Feldspar, Slate and others.
- **Extra ship log entries**, comprising a Guide curiosity alongside Curiosities and
  Endings entries (`assets/shipLog/`).
- **A reworked main menu** with custom music, a replaced DLC logo, and a dedicated
  *CREDITI ECHOES OF THE DESERT* button.
- **A custom credits sequence** with timed, coloured subtitles
  (`utils/CreditsUtility.cs`, `components/VideoSubtitles.cs`), which stays locked
  until you have actually reached the ending.
- **A Paradox easter egg** (`utils/EasterEggUtility.cs`).

### Installation

The easy way is the [Outer Wilds Mod
Manager](https://outerwildsmods.com/mod-manager/): install it, then install this mod
and press Run. To install by hand, drop the mod's files into your OWML mods folder so
that you end up with `OWML/Mods/LaStringa.SbekuMod/` containing `SbekuMod.dll`,
`manifest.json`, `default-config.json`, and the `assets/` folder.

### Compatibility

This mod is **incompatible with [New Horizons](https://outerwildsmods.com/mods/newhorizons/)**
(`xen.NewHorizons`) and the two cannot be run together. The conflict is declared in
`manifest.json` through OWML's `conflicts` field, so the mod manager will warn you if
both are enabled at once.

### Configuration

Settings are exposed through the mod manager. The first is the normal gameplay
option, while the rest are debug tools that are off by default.

| Setting | Effect |
| --- | --- |
| Load events into the ship log | Reveals the mod's ship log entries as you play, and is on by default. |
| Dump ship log | Writes the game's ship log data to disk. |
| Dump dialogue trees | Writes the game's dialogue trees to disk. |
| Press **I** to unlock all reels | Hold `I` to unlock every slide reel. |
| Press **K** to reload dialogue | Reloads dialogue and translations without restarting, which is handy while writing. |
| Press **U** to unlock all events | Reveals every ship log fact. |
| Press **J** to unlock all achievements | Unlocks all achievements. |

### Building from source

The project targets .NET Framework 4.8, and its dependencies come from NuGet
(`OWML` 2.7.3 and `OuterWildsGameLibs` 1.1.13.393), so no local game DLLs need to be
copied in.

```bash
nuget restore SbekuMod.sln
msbuild SbekuMod.sln /p:Configuration=Release
```

`SbekuMod.csproj.user` sets the output path to a local OWML mods folder, so edit it
to match your machine or delete it. `.github/workflows/ci.yml` builds Release and
publishes a zipped release whenever a `release/*` tag is pushed.

### A note on the assets

The compiled Unity asset bundles (slides, audio and models) ship with the packaged
release rather than with the source repository, so a build from source alone will
produce a working DLL but will be missing that content.

### Credits

**SOWC Team**

| Role | People |
| --- | --- |
| Development, 3D Modelling & Video Editing | LaStringa |
| Main Menu Theme | Stereonauti, Gabriele Rinaldi, Camilla D'Onofrio |
| Artwork & Design | Nocturna Draco |
| Game Design & Narrative | Gabriele "gpexio" Pecchioni, Exodd |
| Documentarist & Website Dev | Lorenzo "LaXis315" Tilli |
| Soundtrack | Emanuele Luigi Andolfi and Andrea "Rello" Ceccarelli, aka Stereonauti |
| With the collaboration of | SpeedyCheddar, Lorenzo Giuliani, Togn3K, Klezzon |
| Testers | Togn3K, nicklaj |
| Moderators | 4Claws, Khanattila |

### Disclaimer

This is an unofficial fan project, not affiliated with or endorsed by Mobius Digital
or Annapurna Interactive. Outer Wilds, Echoes of the Eye, and all of their assets and
characters remain the property of their respective owners, and the license below
covers this project's own code and content only.

### License

Released under the [MIT license](LICENSE).

---

## Italiano

### Cos'è

*Echoes Of The Desert* trasforma Echoes of the Eye in una guida al completismo,
distribuendo una serie di rulli personalizzati per lo Straniero (uno per ogni
achievement del gioco) e legandoli insieme con nuovi dialoghi, voci aggiuntive nel
registro della nave, una frequenza del segnalscopio dedicata e una sequenza di
crediti completamente originale. Tutti i testi di gioco sono in italiano.

### Perché esiste

Questa mod non nasce come un rilascio per il pubblico generale. È stata creata come
regalo per lo streamer **Sabaku No Maiku** ("Sbeku") da giocare in diretta, ed è per
questo che il gioco lo saluta per nome appena arrivi al menu principale.

### Funzionalità

- **Rulli personalizzati** nascosti nello Straniero, uno per ogni achievement di
  Outer Wilds, tra cui *Around the World*, *The Grate Filter*, *Ghost in the
  Machine*, *Fire Arrows*, *Tubular* e *Hypothesis*. Sono posizionati da
  `utils/ReelSetup.cs`.
- **Una frequenza del segnalscopio dedicata** (`STORY_REELS`), appresa
  automaticamente la prima volta che equipaggi il segnalscopio, così da poter
  rintracciare i rulli via segnale.
- **Dialoghi nuovi e riscritti** in 69 alberi di dialogo dentro
  `assets/dialogues/`, con Chert, Gabbro, Riebeck, Feldspar, Slate e altri.
- **Voci aggiuntive nel registro della nave**, ovvero una curiosità Guida insieme
  alle voci Curiosità e Finali (`assets/shipLog/`).
- **Un menu principale rielaborato** con musica personalizzata, logo del DLC
  sostituito e un pulsante dedicato *CREDITI ECHOES OF THE DESERT*.
- **Una sequenza di crediti originale** con sottotitoli temporizzati e colorati
  (`utils/CreditsUtility.cs`, `components/VideoSubtitles.cs`), che resta bloccata
  finché non hai raggiunto davvero il finale.
- **Un easter egg Paradosso** (`utils/EasterEggUtility.cs`).

### Installazione

La via più semplice è l'[Outer Wilds Mod
Manager](https://outerwildsmods.com/mod-manager/): installalo, poi installa questa mod
e premi Run. Per installarla a mano, copia i file della mod nella cartella mod di
OWML, in modo da ottenere `OWML/Mods/LaStringa.SbekuMod/` con dentro `SbekuMod.dll`,
`manifest.json`, `default-config.json` e la cartella `assets/`.

### Compatibilità

Questa mod è **incompatibile con [New Horizons](https://outerwildsmods.com/mods/newhorizons/)**
(`xen.NewHorizons`) e le due non possono essere usate insieme. Il conflitto è
dichiarato in `manifest.json` tramite il campo `conflicts` di OWML, quindi il mod
manager ti avviserà se sono attive entrambe.

### Configurazione

Le impostazioni sono accessibili dal mod manager. La prima è l'opzione di gioco
normale, mentre le altre sono strumenti di debug disattivati di default.

| Impostazione | Effetto |
| --- | --- |
| Carica eventi nel registro della nave | Rivela le voci della mod nel registro mentre giochi, ed è attiva di default. |
| Effettua dump registro della nave | Scrive su disco i dati del registro della nave. |
| Effettua dump degli alberi di dialogo | Scrive su disco gli alberi di dialogo del gioco. |
| Premi **I** per sbloccare tutti i rulli | Tieni premuto `I` per sbloccare ogni rullo. |
| Premi **K** per ricaricare i dialoghi | Ricarica dialoghi e traduzioni senza riavviare, comodo mentre si scrive. |
| Premi **U** per sbloccare tutti gli eventi | Rivela ogni fatto del registro della nave. |
| Premi **J** per sbloccare tutti gli achievements | Sblocca tutti gli achievement. |

### Compilare dai sorgenti

Il progetto richiede .NET Framework 4.8 e le sue dipendenze arrivano da NuGet
(`OWML` 2.7.3 e `OuterWildsGameLibs` 1.1.13.393), quindi non serve copiare a mano le
DLL del gioco.

```bash
nuget restore SbekuMod.sln
msbuild SbekuMod.sln /p:Configuration=Release
```

`SbekuMod.csproj.user` imposta il percorso di output su una cartella mod di OWML
locale, quindi modificalo secondo la tua macchina oppure eliminalo.
`.github/workflows/ci.yml` compila in Release e pubblica una release zippata a ogni
tag `release/*`.

### Nota sugli asset

Gli asset bundle Unity compilati (slide, audio e modelli) sono distribuiti con la
release impacchettata anziché con il repository dei sorgenti, quindi una compilazione
dai soli sorgenti produrrà una DLL funzionante ma priva di quei contenuti.

### Crediti

**SOWC Team**

| Ruolo | Persone |
| --- | --- |
| Sviluppo, Modellazione 3D & Video Editing | LaStringa |
| Tema del Menu Principale | Stereonauti, Gabriele Rinaldi, Camilla D'Onofrio |
| Artwork & Design | Nocturna Draco |
| Game Design & Narrativa | Gabriele "gpexio" Pecchioni, Exodd |
| Documentarista & Sviluppo Sito | Lorenzo "LaXis315" Tilli |
| Colonna sonora | Emanuele Luigi Andolfi e Andrea "Rello" Ceccarelli, aka Stereonauti |
| Con la collaborazione di | SpeedyCheddar, Lorenzo Giuliani, Togn3K, Klezzon |
| Tester | Togn3K, nicklaj |
| Moderatori | 4Claws, Khanattila |

### Avvertenza

Questo è un progetto amatoriale non ufficiale, non affiliato né approvato da Mobius
Digital o Annapurna Interactive. Outer Wilds, Echoes of the Eye e tutti i loro asset
e personaggi restano proprietà dei rispettivi titolari, e la licenza qui sotto copre
solo il codice e i contenuti originali di questo progetto.

### Licenza

Distribuito sotto [licenza MIT](LICENSE).

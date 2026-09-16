# Leap 2026 Vertical Slice Architecture

Status: implementation specification (2026-09-16)
Scope: Chapter 01--03 equivalent, from the upland reunion through the festival, shooting gallery, separation in the crowd, and regret.

## 1. Goals

- Adopt Option B: keep `Leap/` as the unmodified legacy reference and build a separate Unity 6 project later in `Leap2026/`.
- Make one polished, linear 15--20 minute slice, rather than a reusable general-purpose novel engine.
- Retain the useful legacy idea that scenario data drives presentation while replacing its line-number saves, `Resources` paths, frame-dependent timing, and UI-coupled state.
- Support the slice's complete player loop: reading, manual save/load, auto-resume, backlog, read-only skip, auto, and the stated accessibility settings.
- Preserve the production direction in `MASTER.md`: a full-bleed visual field with a light reading dimmer and foreground text, designed for landscape 16:9 and safe areas.

## 2. Non-Goals

- Do not create `Leap2026/`, modify `Leap/`, upgrade Unity, add packages, or move assets as part of this specification.
- Do not reproduce the legacy scene-per-screen flow, `ScriptEngine`, chapter-head replay loading, `Resources.Load` paths, or legacy save compatibility at runtime.
- Do not add DI, UniTask, Addressables, an event bus, a separate input router, a separate backlog service, remote delivery, localization, voice, cloud save, thumbnails, unread skip, flags/branching, or a new scenario DSL.
- Do not decide scenario rewrites, face-showing rules, final BGM/SE inheritance, final chapter structure, or release policy. Phase 2--6 initially make a mechanical content migration; 2026 editorial and art work follows after the runtime is proven.

## 3. Repository Layout

```text
.
├── AGENTS.md
├── MASTER.md
├── docs/
│   └── LEAP_2026_TECH_AUDIT.md
├── GAME/
│   └── vertical-slice-architecture.md
├── Leap/                         # legacy Unity 2020.2.2f1 reference; do not alter
└── Leap2026/                     # deliberately absent until Phase 1
```

When Phase 1 is authorized, `Leap2026/` is a new Unity project. It must use Force Text serialization, a minimal package set, uGUI + TextMeshPro, and a single `Game.unity` scene. The exact Unity patch, Xcode version, and minimum iOS version are fixed only after the iOS build spike.

## 4. Scene Architecture

`Game.unity` is the sole Vertical Slice scene. `GameApp` initializes its local runtime and switches UI modes; it does not load a Bootstrap, Title, Novel, Menu, Save/Load, Backlog, Config, Introduction, or Ending scene.

Chapter transitions are data transitions: `ScenarioRunner` resolves and loads the destination `ScenarioDocument` without a Unity scene transition. The Chapter 03 terminal ends the slice in `Ended`; it must not silently attempt to load legacy Chapter 04.

The runtime UI modes are a small closed state owned by `GameApp`: `Title`, `Playing`, `Menu`, `SaveLoad`, `Backlog`, and `Config`. Only one modal overlay is active at once. Opening a non-playing overlay pauses the runner and cancels Auto; closing it resumes the prior runner state. `Title` is an overlay in this first slice, not a separate scene.

## 5. Runtime Components

| Component | Responsibility | Boundary |
|---|---|---|
| `GameApp` | Composes the scene references, owns UI mode, starts a new game/load, and pauses/resumes the runner. | Unity-facing coordinator; no scenario parsing. |
| `ScenarioRunner` | Executes ordered commands, owns the explicit runner state, accepts Advance/Auto/Skip requests, and changes chapters. | Does not inspect UI objects or resolve Unity assets. |
| `GameState` | Sole authoritative current game state. It changes before presentation is requested and is the only source for a save snapshot. | Plain serializable model. |
| `PresentationController` | Applies background, slots, stills, text, dimmer, and fade to assigned scene UI; can apply a complete state instantly. | Never used as a source of saved state. |
| `AudioController` | Plays/stops BGM and SE, applies mixer volume, and restores BGM from `GameState`. | Owns AudioSources/Mixer routing; Voice is absent. |
| `NovelUIController` | Connects TMP text/HUD/overlays to `GameApp` and sends user requests to `ScenarioRunner`. | It is sufficient for input in this slice; no `InputRouter`. |
| `SaveService` | Validates, serializes, atomically writes, reads, and restores snapshots. | Save data is JSON under `persistentDataPath`, not PlayerPrefs. |
| `ContentCatalog` | ScriptableObject mapping typed asset IDs to direct Sprite/AudioClip references. | Scenario JSON never contains `Resources` paths. |
| `ScenarioValidator` | Editor/build-time validation of documents and catalog references. | Errors block content handoff; it is not a runtime fallback. |

No generic event bus or DI container is required. Explicit inspector references in `GameApp`, `PresentationController`, `AudioController`, and `NovelUIController` are preferred.

## 6. Scenario Model

One UTF-8 JSON document represents one chapter.

```json
{
  "schemaVersion": 1,
  "chapterId": "ch01",
  "commands": [
    { "id": "ch01-c001", "type": "bgm", "assetId": "bell-leap", "action": "play" },
    { "id": "ch01-c002", "type": "background", "assetId": "upland-evening" },
    { "id": "ch01-c003", "type": "say", "speaker": null, "text": "夏休みも終盤に差し掛かった8月の日、僕は近所の高台まで絵を描きに来ていた。" }
  ]
}
```

`schemaVersion` versions the document shape. A separately defined `contentVersion` identifies the compatible shipped content set and is copied into saves. Command IDs are stable content identifiers, not array indices or source line numbers. A command inserted between existing commands receives a new ID; existing published IDs are never renumbered merely due to insertion.

JSON is the only authoring/execution format for the slice. It is parsed once during document loading into typed command DTOs. A converter may generate JSON from legacy text during migration, but that converter is a development tool and legacy text is never parsed by the new runtime.

## 7. Command Set

The command set is closed and limited to the capabilities used by legacy `script01.txt`--`script03.txt`:

| Type | Required data | Meaning |
|---|---|---|
| `say` | `id`, `speaker` (nullable), `text` | Sets `currentMessage`, reveals text, then waits for advance. |
| `background` | `assetId` (nullable) | Shows the background, or clears it when null. |
| `character` | `slot`, `assetId` (nullable) | Sets/clears `left`, `center`, or `right`. |
| `still` | `assetId`, `visible` | Shows or hides a catalogued still element. |
| `bgm` | `action: play|stop`, `assetId` for play | Plays or stops BGM. |
| `playSe` | `assetId` | Plays a one-shot SE without delaying command progression. |
| `fade` | `color: black|white`, `durationSeconds` | Fades with unscaled time and waits for completion. |
| `goToChapter` | `chapterId` | Loads the target document and continues at its first command. |
| `end` | none | Ends the Vertical Slice and leaves the game in `Ended`. |

`background: null`, `character.assetId: null`, and `still.visible: false` replace legacy `# REMOVE-IMG`; there is no remove command. `still` must support a set of visible IDs, not a single ID: Chapter 01 shows `01-teru-04` and `01-aruku-01` concurrently. The UI may layer them in a single StillLayer.

Legacy `# MSG` plus its following `STOP;` becomes exactly one `say`. Legacy `LOADING;` becomes `goToChapter` for Chapter 01→02 and 02→03; the terminal `LOADING;` in Chapter 03 becomes `end` for this slice. `WAIT` and `ANIMATION` are not present in these three source chapters and are not imported. Legacy fade arguments such as `0.03` are frame-step rates, not seconds; conversion must assign reviewed `durationSeconds` values rather than copying them as durations.

## 8. ScenarioRunner State Machine

The runner uses one explicit state, never a set of mutually interacting booleans:

```text
Running -> RevealingText -> WaitingForAdvance -> Running
Running -> WaitingForEffect -> Running
Running <-> Paused
Running -> Ended
WaitingForAdvance <-> Paused
WaitingForEffect <-> Paused
```

`Running` consumes immediate commands in order until it reaches a command that waits. `say` enters `RevealingText`; a tap there reveals the whole message. Once reveal completes, the runner appends the log entry, records the stable Say ID as read, updates `nextCommandId` to the command after the Say, and enters `WaitingForAdvance`. A tap there advances into `Running`.

`fade` enters `WaitingForEffect` until the `PresentationController` reports completion. It uses `Time.unscaledDeltaTime`. Menu/Backlog/SaveLoad/Config put the runner in `Paused`; they do not progress commands. Loading cancels active presentation work, applies the saved state instantly, and restores the appropriate command-boundary waiting state. `Ended` accepts no advance request.

Skip is allowed to make a text reveal and an active fade complete immediately, but it must still apply the final state and command ordering. It never relies on frame counts.

## 9. GameState

`GameState` is a plain model and the sole current-state authority. Minimum fields are:

```text
chapterId
nextCommandId                         # command after the displayed, completed Say
currentMessage { commandId, speaker, text } | null
backgroundId | null
characterSlots { left, center, right } # each asset ID or null
visibleStillIds                       # set/list; see concurrent Chapter 01 stills
bgm { assetId, isPlaying, loop }
backlog: List<LogEntry>
```

`LogEntry` is `{ commandId, speaker, text }`. `currentMessage` is retained at an input-wait save point so load returns to the same readable text, while `nextCommandId` says what executes after its next Advance. Presentation values such as Image sprites, TMP glyph positions, fade interpolation fraction, and AudioSource internals are not state and are never read back for saving.

Read history is profile-level progress, not a save-slot rollback mechanic: persist a set of completed stable Say IDs independently from slots. It may be held by `GameApp` in this slice and written with the small profile/config data. It is consulted by Skip but is not inferred from the current backlog.

## 10. Save / Load

Saving is enabled only at stable `WaitingForAdvance` boundaries after a complete Say. Mid-reveal, mid-fade, and mid-command saves are intentionally out of scope.

`SaveSnapshot` contains `schemaVersion`, `contentVersion`, save metadata, and a complete copy of `GameState`. Thus it includes at least chapter ID, next command ID, current message, background, slots, visible still IDs, BGM state, and backlog. It may include no thumbnail in this slice.

The slice provides Manual Save slots 1--3 and one Auto Resume record. Slots and auto-resume are JSON files under `Application.persistentDataPath`; configuration and read history are separate. Writes use a temporary file and replacement with one `.bak` generation where platform APIs permit it. Invalid schema/content, malformed JSON, unknown chapter/command, and catalog-invalid snapshot IDs are rejected with a recoverable UI error; the backup is then offered/used when valid.

Load is exactly:

```text
read and validate snapshot
-> restore GameState
-> cancel active effects
-> PresentationController.ApplyInstant(GameState)
-> AudioController.Restore(GameState.bgm)
-> ScenarioRunner resume at saved command boundary
```

It must never replay from a chapter start. BGM identity and play/stop state are restored; exact playback time is deferred until tested and approved.

## 11. Auto / Skip / Backlog

- Backlog lives as `GameState.backlog`; no `BacklogService`. It receives a `LogEntry` when a Say finishes revealing. The slice may render its 273 legacy-source messages in a normal ScrollView without virtualization; only profile data need not duplicate it.
- Auto waits after a completed Say for a delay derived from visible character count plus the configured auto-speed offset, then performs Advance. A manual tap, opening an overlay, or starting Skip turns Auto off.
- Skip is read-only. It uses the persisted stable Say command ID set. It completes text/fade immediately and auto-advances across read commands. It stops before an unread Say and turns itself off. There is no unread-skip option.
- Backlog, Auto, and Skip controls are small HUD controls and must not obscure the current text. Opening Backlog pauses the runner; returning resumes its prior waiting state.

## 12. Config

Config is separate from save slots and immediately previews changes. Initial fields are:

```text
textSize: Small | Medium | Large
textSpeed: Slow | Normal | Fast
contrast: Normal | High
masterVolume: 0..1
bgmVolume: 0..1
seVolume: 0..1
autoSpeed: 0..1
```

`NovelUIController` applies typography/text-speed/contrast values; `AudioController` applies Master/BGM/SE mixer values. Voice volume is deliberately absent because voice is absent.

## 13. Content Catalog

`ContentCatalog` is a simple ScriptableObject containing typed, unique ID-to-reference entries for backgrounds, character sprites, still elements, BGM clips, and SE clips. It is the only bridge from scenario asset IDs to Unity assets. It uses direct serialized references, not `Resources` paths, Addressables keys, GameObject names, or file names.

The initial catalog covers only Chapter 01--03 migration assets. Import presets, rights/source verification, and any replacement 2026 art/audio are separate content decisions.

## 14. Scenario Validation

The validator runs in the Editor and is suitable for a build/preflight check. It errors on:

- duplicate or empty chapter IDs;
- duplicate, empty, or globally ambiguous command IDs;
- unknown command types or invalid required fields/actions;
- asset IDs absent from the required typed catalog category;
- invalid character slots;
- unknown `goToChapter` destinations;
- invalid still visibility references;
- documents with no reachable terminal (`end` or valid chapter path ending in `end`), including cycles with no terminal.

It also validates save compatibility at load time: the saved chapter and `nextCommandId` must still exist in the matching `contentVersion`. A content edit that intentionally removes such an ID requires a version/migration decision, never a silent best-effort jump.

## 15. UI / Safe Area

The initial hierarchy is:

```text
GameRoot
├─ Presentation
│  ├─ FullBleedBackground
│  ├─ CharacterLayer
│  │  ├─ Left
│  │  ├─ Center
│  │  └─ Right
│  ├─ StillLayer
│  └─ ReadingDimmer
├─ SafeAreaRoot
│  ├─ TextLayer
│  │  ├─ Speaker
│  │  ├─ Body
│  │  └─ ContinueIndicator
│  ├─ HUD (Log, Auto, Skip, Menu)
│  └─ OverlayHost (Title, Menu, SaveLoad, Backlog, Config)
├─ TransitionOverlay
├─ AudioRoot
└─ EventSystem
```

Presentation is a full-bleed 16:9 composition area; it may crop rather than stretch at non-16:9 aspects. `SafeAreaRoot` derives its rect from `Screen.safeArea`, and all text, HUD, dialogs, and modal controls remain inside it. `TransitionOverlay` covers the presentation surface and owns black/white fades.

The message presentation is a light reading dimmer with foreground white/off-white Japanese TMP text, generous leading, and a left-to-centre-left reading region. Speaker data is always retained, but the Speaker object can be toggled by a presentation setting because always showing names remains a production open question. High contrast changes a small theme token set (text, dimmer, and control focus), not arbitrary per-widget colors.

## 16. Implementation Phases

1. **Project baseline:** after approval, create `Leap2026/`; run the Unity 6.3 LTS/Xcode iOS spike, fix patch/minimum iOS, use Force Text, create `Game.unity`, TMP, Safe Area, and minimal Git ignores/packages.
2. **Scenario foundation:** define DTO/schema, JSON loader, `ContentCatalog`, validator, and a development-only mechanical conversion of legacy Chapter 01.
3. **Chapter 01 end-to-end:** implement `GameState`, runner, presentation/audio, text reveal, background/character/still/BGM/SE/fade, and `goToChapter`.
4. **Snapshot round trip:** add `SaveService`, three manual slots, auto resume, and tests that prove no replay is used.
5. **Player features:** add same-scene overlays, backlog, Auto, read-only Skip, config, and title/menu behavior.
6. **Chapter 02--03:** mechanically migrate the remaining content, end at regret, validate all references, then run device/polish checks.
7. **2026 production pass:** only after the runtime works, separately revise prose, framing, art, reading dimmer, BGM/SE, pauses, and face-independent staging.

## 17. Vertical Slice Definition of Done

- The defined Chapter 01--03 path plays in one `Game` scene from reunion through regret, with no legacy project dependency at runtime.
- All slice commands and catalog references validate successfully; invalid data reports actionable errors before play/build.
- Text reveal/Advance, fade timing, background, all three character slots, concurrent stills, BGM, and shooting SE work on the intended command boundaries.
- At any permitted boundary, every manual slot and Auto Resume restore the same message, background, slot/still state, BGM identity, and backlog without chapter-head replay.
- Backlog, Auto, read-only Skip, all stated config values, menu, and same-scene save/load overlays work together without unintended progression.
- Safe Area containment and text readability are checked on representative iPhone cutout/home-indicator and iPad aspect devices; no UI is stretched to fit presentation art.
- Fade/text timing uses unscaled time and behaves consistently at 60 Hz and 120 Hz.
- A current Xcode development build and archive have passed the approved iOS spike criteria. Asset/right/import quality checks are recorded before release use.

## 18. Deferred Features

Deferred items are deliberately not placeholders in the initial runtime: branching/flags, choices, `WAIT`, special animations, voice and voice volume, crossfade/precise BGM position restoration, unread Skip, save thumbnails, cloud/legacy save migration, localization/ruby/vertical text, remote content/Addressables, achievements, analytics, ads, IAP, an event bus, DI, and generalized scripting abstractions.

## 19. Open Questions

1. Confirm the Unity 6.3 LTS patch, Xcode version, minimum iOS target, and iPhone/iPad support matrix through the iOS build spike.
2. Decide the visual policy for iPad/non-16:9 aspects: crop, letterbox, or deliberately authored expanded composition.
3. Decide whether speaker names are shown by default; the data and toggle support either result.
4. Decide the reviewed fade durations and whether a ReadingDimmer change needs an explicit scenario command after mechanical migration.
5. Confirm whether exact BGM playback-position restoration is required for the slice.
6. Confirm rights, source masters, and 2026 redistribution use for legacy images, audio, and fonts before copying them.
7. Confirm that initial Chapter 01--03 JSON is a mechanical migration before prose/art re-editing, and approve any intentional deviations from legacy order/content.
8. Decide final orientation behavior (Landscape Left only vs both landscape directions) and the title/end presentation for the slice.

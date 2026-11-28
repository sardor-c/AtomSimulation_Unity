# AtomSimulation Unity course: notes for Claude Code sessions

This repo is **the course**: the user rebuilds the AtomSimulation project (hydrogen atom: Bohr 2D model, quantum
orbitals as GPU particles, ray-marched volume, UI Toolkit panel) step by step to learn **Unity, math and physics**.
Claude works here in **tutor mode**, the same way as in `C:\dev\OPENGL_SIMULATIONS\opengl_fluid_sim_scratch`
(read that repo's `CLAUDE.md` and `docs/` when in doubt about style). This file is the source of truth and the
cross-machine memory (it travels through GitHub).

## The user

- Writes in Uzbek; all lessons and answers in Uzbek. Keep technical terms in English (GameObject, shader, compute
  buffer, wave function), explained in Uzbek.
- **Knows almost no math or physics (the user's own words).** Explain every math and physics concept from zero,
  however basic (vector, sin/cos, radian, force, energy, probability, integral as a sum): an everyday example,
  small numbers worked by hand, then where it is in the code. No unexplained notation; avoid Greek letters and
  calculus symbols, or introduce each one in words first.
- Learning Unity from scratch. Writes code in **JetBrains Rider** (2026.3 installed; `com.unity.ide.rider` 3.1.0).
- Windows PC (RTX 3060, i5-12400F, 165 Hz monitor) and a MacBook (used in the OpenGL course). **On 2026-11-26 the
  user said they want to continue this course on the MacBook** (see "macOS" below). Answered 2026-11-28: the Mac is
  a **MacBook Pro with M1 Pro** (Apple silicon; that model has a 120 Hz ProMotion display, not measured). **All
  Claude sessions stay on this Windows PC**; on the Mac the user only does `git pull` and builds the project there.
- Wants the geometry and the origin of every number explained on the spot: on 2026-11-28 they asked for a full
  explanation of where lesson 01's FOV numbers (0.577 etc.) come from, which the lesson had deferred to lesson 09
  ("hozir jadval yetadi"). Don't defer such explanations; if a number appears in a lesson, derive it there.

## Tutor mode

1. **The user does 100% of the project work themselves**: every editor action (scenes, GameObjects, materials,
   settings), every script, shader, asmdef, `.gitignore`, git commands. Claude writes only:
   - lessons: `docs/NN-topic.md` (Uzbek), listed in `docs/README.md` (table of contents);
   - this `CLAUDE.md`.
   Do NOT create or edit `Assets/`, `Packages/`, `ProjectSettings/`. Exception, on the user's request: Claude created
   the empty project itself on 2026-11-22 (`unity projects create AtomSimulation_Scratch --template 3d --no-cloud`,
   Unity 6000.6.9f1; URP blank template + Pipeline package, Cinemachine, Run In Background).
2. **Lesson format (same as the OpenGL course):** a step-by-step guide, not "all theory, then all code". Each step =
   theory for that step → only that step's code/editor actions and exactly where they go (new files whole, changes
   as ```diff blocks with context lines) → "Ishga tushirib ko'ring" (Play, what you should see). The project
   compiles and runs after every step. After each step a collapsed `<details>` block holds the full changed file.
   End: "nima buzilsa nima bo'ladi" table (measured), optional experiments (few), glossary (Lug'at), next lesson.
   Few tasks, no graded exercises. Chat reply = short summary + link to the lesson file.
3. **Verify every code sample in a scratch copy, never in the project** (recipe below). Every intermediate step
   must compile with 0 errors/warnings; measure the claims written in the lesson.
4. One lesson at a time. Wait until the user says it is done; review their work if asked (read their files; to run
   it, copy the project to the scratchpad — their GUI Editor locks the original), explain mistakes, then go on.
5. Prove claims by running things, not by assertion. Build/compile problems: help when the user is stuck.
6. At session start: `git status -sb`, `git log --oneline -3` (once the user has set up git in lesson 00).
   After each lesson remind them to commit and push.

## Verification recipe (worked on 2026-11-22)

```
robocopy C:\dev\UNITY\AtomSimulation_Scratch <scratchpad>\verify\<name> /E /XD Temp Logs      (~1 GB, 10 s)
"C:\Program Files\Unity\Hub\Editor\6000.6.9f1\Editor\Unity.exe" -batchmode -projectPath <copy> -logFile <log>
    (run in background, NO -quit: stays resident and serves the Pipeline API)
unity status --until-ready --project-path <copy>
unity recompile --project-path <copy> --format json            (compile errors/warnings)
unity command script eval '<C#>' --project-path <copy>          (build scenes, read state)
unity command editor.playmode start|stop --project-path <copy>
unity command console status / console get -- --since <cursor> --tail 100000 --level error
```
Shut the batch Editor down at the end of the session (it holds a license seat).

Pitfalls of this recipe:
- The scratchpad path is long: URP's `TraceRenderingLayerMask.urtshader` fails to import (`DirectoryNotFoundException`,
  260-char path limit). Harmless for our checks; the user's short project path doesn't have it.
- Batch mode has no window focus, so the Input System disables the keyboard. To simulate keys:
  `InputSystem.settings.backgroundBehavior = IgnoreFocus`, `editorInputBehaviorInPlayMode =
  AllDeviceInputAlwaysGoesToGameView`, `InputSystem.AddDevice<Keyboard>()`, then per key press in one eval:
  `EnableDevice(kb)` + `QueueStateEvent(kb, new KeyboardState(Key.UpArrow))`, and release in the next eval.
- The console keeps the last 2000 entries; an exception thrown every frame pushes earlier entries out.
- Deleting a `.meta` locally does NOT break references: Unity re-creates it with the same GUID from `Library`.
  A changed GUID (what a fresh clone without the `.meta` gets) does: component shows as missing,
  `The referenced script (Unknown) on this Behaviour is missing!`.

## macOS (written 2026-11-26 on the Windows PC — NOTHING here is tested on a Mac yet)

- The user's guide for the move is `docs/00b-macbookga-kochish.md` (commit WIP + push, Hub, Editor 6000.6.9f1 via
  `unityhub://6000.6.9f1/8c45277fb9c5`, clone into `~/dev/UNITY/AtomSimulation_Scratch`, Rider via
  `Unity → Settings... → External Tools`, checks). When the user reports what really happened on the Mac, correct
  that guide and replace this section's guesses with measured facts.
- **Workflow (decided 2026-11-28):** Claude sessions run only on the Windows PC (verification recipe above, reference
  project `../AtomSimulation` available here). The user does the lesson work on the Mac and pushes from there.
  Claude's docs/CLAUDE.md changes are committed and pushed from the Windows PC (by the user), then pulled on the Mac.
  Both machines commit to `main`, so: at session start `git fetch`; if `origin/main` is ahead, ask the user to
  `git pull` here before Claude edits docs (otherwise the docs commit diverges). To review the user's Mac work
  without pulling, read it with `git show origin/main:<path>`; to run it, pull first, then copy to the scratchpad.
- Claude cannot run the user's Mac build, so Mac-only results in lessons are expectations until the user reports
  them (same rule as the OpenGL course). Write shortcuts as "Ctrl+S (Mac: Cmd+S)" and menu paths for both
  (`Edit → Preferences...` on Windows, `Unity → Settings...` on macOS). Numbers measured here (165 Hz `dt`, FPS)
  differ on the Mac: say so where a lesson quotes them.
- Expected differences that matter later: graphics API is Metal (no geometry shaders; compute shaders are
  supported), display 60/120 Hz so measured `dt`/FPS numbers differ, Scene-view navigation on a trackpad,
  `Cmd+Delete` / `Return` instead of `Delete` / `F2` in the Hierarchy, lesson 16 build is a `.app`, not `.exe`.
  Verify each on the Mac before stating it as fact in a lesson.

## Project facts (checked 2026-11-22)

- Unity 6000.6.9f1, URP 17.6.0, Input System 1.20.1 (**Active Input Handling = Input System only**: the old
  `UnityEngine.Input` throws `InvalidOperationException ... switched active Input handling to Input System package`).
- Enter Play Mode Options enabled with **domain reload disabled** (`m_EnterPlayModeOptions: 1`): static fields keep
  their values between Play sessions. Matters as soon as a lesson uses mutable statics.
- Quality: PC and Mobile levels, each with its own URP asset (`Assets/Settings/PC_RPAsset`, `Mobile_RPAsset`).
  `vSyncCount: 0`.
- Scene templates in URP: Basic (URP) = Main Camera at (0, 1, -10), FOV 60, Skybox background + Directional Light.
- Editor menus (6000.6): `Assets/Create/Scripting/MonoBehaviour Script`, `Assets/Create/Material`,
  `Assets/Create/Shader/URP Unlit Shader`, `Assets/Create/Shader/Compute Shader`,
  `Assets/Create/Scripting/Assembly Definition`, `Assets/Create/Testing/Tests Assembly Folder`, `File/Build Profiles`,
  `Edit/Preferences...` → External Tools, `Window/General/Test Runner`, `Window/Analysis/Profiler`.
- Folder sizes after create: Library 986 MB / 22 085 files; Assets 220 KB, ProjectSettings 117 KB, Packages 20 KB.

## Reference implementation (`../AtomSimulation/`, on the Windows PC)

Written by Claude. Use it to check physics/values and to compare when the user is stuck — never copy it wholesale.
It is **not a git repo** (checked 2026-11-26; Assets + Packages + ProjectSettings = 5.3 MB). Claude sessions run on
the Windows PC (decided 2026-11-28), so it is always at hand; the Mac copy suggested in `docs/00b` is optional.
Do not write lessons 02+ from memory: read the matching reference files first.
Map (final file → lesson where it is built):
- `Runtime/Bohr/BohrAtom2D.cs` → 01 (ElectronOrbit, simplified), 02 (lines/glow), 03–05.
- `Runtime/Physics/Hydrogen.cs` → 03 (EnergyEv), 04 (wavelength), 06–07 (Radial, Laguerre, Legendre, Theta, Phi).
- `Runtime/Physics/OrbitalTables.cs` → 08. `Tests/Editor/HydrogenTests.cs` → 03, 06–08.
- `Runtime/Camera/OrbitCamera.cs`, `Runtime/AtomSimController.cs` → 09 (controller grows later).
- `Runtime/Rendering/OrbitalGpuData.cs`, `Shaders/OrbitalParticles.compute`, `OrbitalCommon.hlsl` → 10.
- `Rendering/OrbitalParticleRenderer.cs`, `Shaders/OrbitalParticles.shader` → 11; Flow kernel, phase colours → 12.
- `Rendering/OrbitalVolumeRenderer.cs`, `Shaders/OrbitalVolume.shader` → 13.
- `UI/*` (UXML, USS, TSS, AtomSimUI, SpectrumView) → 14–15. `Shaders/AdditiveLine.shader`, `Glow.shader` → 02.
- `Editor/AtomSimSceneBuilder.cs` builds the scene from code; in the course the user builds the scene by hand
  (teaches the Editor). Values to match: camera FOV 45, background (0.006, 0.008, 0.016), Bloom threshold 0.85 /
  intensity 1.1 / scatter 0.72, Tonemapping Neutral, Vignette 0.28 / 0.45, panel reference 1920x1080 match 1.

## Course plan (draft — adjust with the user, record changes here)

Part I — Unity basics
- 00 Muhit va loyiha: Unity Hub, Editor/Player, project folders, .meta/GUID, packages, editor windows, Rider,
  own scene (Basic URP), solid background, Git + Unity .gitignore + GitHub.
- 01 Birinchi skript: GameObject/Component/Transform, parent/local coords, URP Lit materials, MonoBehaviour
  lifecycle, Debug.Log, deltaTime, SerializeField, Input System. Math: 3D coords, vector, radian, pi, sin/cos,
  Pythagoras, angular speed, period, arc length, FOV (frustum, similar triangles, tangent, aspect ratio, inverse:
  distance = half-height / tan(FOV/2)). Physics: what an atom is, charge, circular motion needs a force,
  why the planetary model fails (foreshadow).
- 02 Yorug'lik chizamiz: LineRenderer orbits, first HLSL shaders (AdditiveLine, Glow), quad mesh from code, HDR,
  Bloom/Tonemapping (Volume), solid background. Spheres replaced by glow quads.
Part II — Bohr model (2D)
- 03 Bor atomi: Coulomb force, energy, eV, negative = bound, E_n = -13.6/n^2, r ∝ n^2, angular speed ∝ 1/n^3,
  `Hydrogen.cs`, asmdef (Runtime), first EditMode test, BohrAtom2D creating orbits in code, smoothing (Lerp/exp).
- 04 Foton: light as a wave, wavelength/frequency, E = hc/λ, visible spectrum colours, absorption only on matching
  energy (tolerance), mouse click → ray/plane intersection, photon wave packet, pooling.
- 05 Nurlanish va spektr: spontaneous emission, random lifetime (exponential), series (Lyman/Balmer/Paschen),
  656 nm test, de Broglie standing wave, C# events.
Part III — quantum atom
- 06 To'lqin funksiyasi: probability, |psi|^2, 1s, normalisation as a sum (trapezoid), spherical coords, radial part
  (Laguerre), tests (norm, mean radius, nodes).
- 07 Orbital shakllari: n/l/m rules, Legendre, Theta/Phi, real vs complex, s/p/d/f, Density, tests.
- 08 Bulut nuqtalari: histogram, CDF, inverse CDF sampling, OrbitalTables, CPU points mesh, sampler tests.
- 09 3D kamera: OrbitCamera (spherical coords, quaternion, smoothing, log zoom, pan), AtomSimController (modes, keys).
  FOV/tan/frustum are already taught in 01 §5.4: refer back, don't re-teach.
Part IV — GPU
- 10 Compute shader: threads/groups, GraphicsBuffer, Dispatch, PCG random, data textures, Generate kernel.
- 11 Zarrachalarni chizish: RenderPrimitives, SV_VertexID, view-space billboards, additive blend, fire ramp, gamma,
  MaterialPropertyBlock, sphere impostors.
- 12 Faza va oqim: complex numbers as arrows, e^{i m phi}, phase colours (hue), probability current m/rho^2,
  rotation, Flow kernel, cutaway.
- 13 Hajmiy ko'rinish: ray marching, ray-sphere (quadratic), absorption/transmittance, jitter.
Part V — UI and wrap-up
- 14 UI Toolkit: UXML/USS/TSS, PanelSettings, UIDocument, binding to the controller, pointer-over-UI, FPS.
- 15 Bor paneli va spektr: level list, Painter2D SpectrumView, transition events.
- 16 Yakun: standalone build, all tests, Profiler, next ideas.

## Progress

Update this at the end of every session.

- 2026-11-22: Course started. Claude created the empty Unity project (see Tutor mode 1). Lessons 00 and 01 written.
  Lesson 01 code verified in a scratch copy: all 4 intermediate states compile with 0 errors/warnings; in Play mode
  radius stays 4.0000 and the angle follows 2.4 * t (offset 0.047 rad = first frame's deltaTime); Up/Down change n
  with clamp 1..6 (radius 4..24) and log `Orbita: n = 2, radius = 8`; unassigned `m_Electron` gives
  `UnassignedReferenceException` every frame; `Input.GetKeyDown` gives the InvalidOperationException above.
  Not checked by Claude (no GUI): the look of the scene, colours, actual FPS/deltaTime in the user's Editor.
  Measured surprise: in Unity 6.6 a MonoBehaviour whose class name differs from its file name still attaches and
  survives a scene save/reload (lesson 01 says so; don't teach the old "must match or it fails" rule).
  The user called the result "ajoyib ish" and asked to continue the course in new sessions until it is finished.
  Session ended with lessons 00 and 01 written, the user had not started lesson 00 yet; git not initialised yet
  (the user does `git init` in lesson 00, step 6).
- 2026-11-26 (Windows PC): The user said they want to continue on the MacBook. State found: lesson 00 done and
  pushed (`100fa0d`, remote `https://github.com/sardor-c/AtomSimulation_Unity.git`, branch `main`); lesson 01 in
  progress and **uncommitted**: materials `Nucleus`/`Electron`, scene objects `Atom` → `Nucleus`, `Electron` at
  local (4, 0, 0), camera (0, 0, -30), `ElectronOrbit.cs` at the end of step 4 (`m_Radius`, `m_AngularSpeed`,
  `m_Electron` assigned in the scene); step 5 (keyboard, `m_LevelSpacing`) not started. Differences from the
  lesson's step-4 file, all harmless and not yet discussed with the user (no review was asked): class is not
  `sealed`, unused `using System;`, the step-2 `Debug.Log` lines were kept (one in `Start`, one **every frame** in
  `Update`, so the Console floods), position is set before the angle is advanced.
  Written this session: `docs/00b-macbookga-kochish.md` (move guide), README row 00b and 00 → bajarildi, this file.
  Measured: `git mv Assets/AtomSIm Assets/AtomSim` fails on Windows (git 2.45.1, `Invalid argument`); the two-step
  rename through `AtomSim_tmp` plus `git mv` of the folder's `.meta` works and shows as renames.
  Not done by Claude: the WIP commit and push (the user does git), anything on the Mac.
- 2026-11-28 (Windows PC): The user (project already open on the Mac, at lesson 01 step 5) asked to explain FOV in
  §5.4: what it is, how it is computed, the geometry. Rewrote §5.4 of `docs/01-birinchi-skript.md` (view angle,
  Camera Inspector fields, frustum, side-view diagram, similar triangles, tan from the 3.2 unit circle, formula
  `half-height = distance × tan(FOV/2)`, aspect ratio and horizontal FOV 91.5° at 16:9, when the n = 5 electron
  leaves the screen at distance 30 (60°–120° and 240°–300°), inverse: distance ≥ 24.5 / 0.577 = 42.4, why 55);
  5.6 got steps 7 (camera z = -30 in Play to test the prediction) and 8 (frustum gizmo, side view via Scene Gizmo);
  experiment 4 (FOV 30 / 90); glossary rows. Unity was NOT run this session (the user said another Claude session
  was using Unity on this PC): camera values (FOV 60 Vertical, Near 0.3, Far 1000) were read from the scene file,
  Inspector labels and the 179° slider limit from `com.unity.render-pipelines.core` `CameraUI.Drawers.cs`,
  "16:9 Aspect" from the `{0}:{1} Aspect` string in `UnityEditor.CoreModule.dll`, numbers computed in Python.
  Not verified anywhere: the frustum gizmo / Scene Gizmo side-view steps and trackpad zoom on the Mac.
  Found: `Assets/AtomSim.meta` is missing in git and on the Windows disk (lost in the AtomSIm → AtomSim rename in
  `85d4b40`); Unity will create it with a new GUID on whichever machine opens the project first. Told the user to
  commit it from the Mac and to `git pull` on Windows before opening Unity there.

Decisions so far:
- The folder was renamed to **`Assets/AtomSim`** in `85d4b40` (was `Assets/AtomSIm`); its folder `.meta` is missing
  (see Progress 2026-11-28).
- Folder layout follows the reference: `Assets/AtomSim/{Scenes, Materials, Shaders, Runtime/{Bohr, Physics, Camera,
  Rendering}, UI, Tests/Editor}`; namespace `AtomSim.*`; Unity's own C# style (`m_` private fields, `s_` statics,
  `sealed` components, `[SerializeField]` private fields instead of public ones).
- **Brace style = Java/K&R** (user's request 2026-11-28): `{` at the end of the line. Claude added `.editorconfig`
  at the repo root (standard `csharp_new_line_before_*` + `resharper_csharp_*_braces = end_of_line`); the user
  applies it in Rider with Reformat Code. Write all C# in lessons from 02 on in this style (lessons 00–01 use Allman).
- Scene `Assets/AtomSim/Scenes/AtomSimulation.unity` from the Basic (URP) template, camera at (0, 0, -30) in 01
  (-55 once n goes up to 6), background Solid Color (0.006, 0.008, 0.016).
- Lesson 01 script `Runtime/Bohr/ElectronOrbit.cs` (fields `m_Electron`, `m_LevelSpacing` 4, `m_AngularSpeed` 2.4,
  Up/Down arrows for n); lessons 02–03 grow it into `BohrAtom2D`. Asmdefs come in lesson 03 with the first tests.

## Next session: start here

1. Find out which machine this is (`uname` / the working directory) and run `git fetch`, `git status -sb`,
   `git log --oneline -3`. After `85d4b40` expect the user's commit of the rewritten §5.4 (docs + this file) and,
   from the Mac, `Assets/AtomSim.meta`. Check `git ls-files Assets/AtomSim.meta`.
2. Ask how the move to the MacBook went (`docs/00b-macbookga-kochish.md`): did the Editor version install, is
   `ElectronOrbit` attached (not "Missing"), what `git status` showed after the first open (expected: untracked
   `Assets/AtomSim.meta`, nothing else), what the title bar and menus really say. Fix `docs/00b` and the "macOS"
   section with what they report. Ask whether the §5.4 steps 7–8 worked on the Mac. (Which MacBook and where
   sessions run are answered: see "The user". The reference project does not need to be on the Mac.)
3. The user continues lesson 01 from step 5. If they want a review: read their files; the differences already seen
   are listed in Progress 2026-11-26 — the per-frame `Debug.Log` is the one worth mentioning. To run their code,
   copy the project to the scratchpad (their GUI Editor locks the original). Before running Unity on Windows, check
   that no other Claude session is using it (the user warned about that on 2026-11-28).
4. Only after they confirm lesson 01 is done: write 02 Yorug'lik chizamiz (needs the reference project and a
   working verification recipe on the machine where the session runs), update `docs/README.md` statuses
   (01 → bajarildi, 02 → yozildi) and the Progress section above.
5. The scratchpad from 2026-11-22 (step files, `check_doc.py`) is temporary and may be gone. Re-create the checker if
   needed: it applies every ```diff block of a lesson in order and compares with the `<details>` full files.
6. Keep this section current: rewrite it at the end of every session so the next one knows where to start.

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
- Windows PC (RTX 3060, i5-12400F, 165 Hz monitor). Has a MacBook too (used in the OpenGL course) — ask whether
  they want to use it here before writing Mac-specific notes.

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

## Reference implementation (`../AtomSimulation/`, this PC only)

Written by Claude. Use it to check physics/values and to compare when the user is stuck — never copy it wholesale.
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
  Pythagoras, angular speed, period, arc length. Physics: what an atom is, charge, circular motion needs a force,
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

Decisions so far:
- Folder layout follows the reference: `Assets/AtomSim/{Scenes, Materials, Shaders, Runtime/{Bohr, Physics, Camera,
  Rendering}, UI, Tests/Editor}`; namespace `AtomSim.*`; Unity's own C# style (`m_` private fields, `s_` statics,
  `sealed` components, `[SerializeField]` private fields instead of public ones).
- Scene `Assets/AtomSim/Scenes/AtomSimulation.unity` from the Basic (URP) template, camera at (0, 0, -30) in 01
  (-55 once n goes up to 6), background Solid Color (0.006, 0.008, 0.016).
- Lesson 01 script `Runtime/Bohr/ElectronOrbit.cs` (fields `m_Electron`, `m_LevelSpacing` 4, `m_AngularSpeed` 2.4,
  Up/Down arrows for n); lessons 02–03 grow it into `BohrAtom2D`. Asmdefs come in lesson 03 with the first tests.

## Next session: start here

1. Ask whether lessons 00 and 01 are done and how it went (errors, unclear parts). If git is set up by then, run
   `git status -sb` and `git log --oneline -3`.
2. If the user wants a review: read their files (`Assets/AtomSim/...`, `.gitignore`); to run their code, copy the
   project to the scratchpad and use the verification recipe (their GUI Editor locks the original). Check against
   lesson 01's final `ElectronOrbit.cs` and the scene values in "Decisions so far".
3. Only after they confirm a lesson is done: write the next one (02 Yorug'lik chizamiz, see the course plan), update
   `docs/README.md` statuses (00/01 → bajarildi, 02 → yozildi) and the Progress section above.
4. The scratchpad from 2026-11-22 (step files, `check_doc.py`) is temporary and may be gone. Re-create the checker if
   needed: it applies every ```diff block of a lesson in order and compares with the `<details>` full files.
5. Keep this section current: rewrite it at the end of every session so the next one knows where to start.

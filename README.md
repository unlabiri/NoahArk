# NoahARK — A Virtual Reality Conservation Simulator

**NoahARK** is a room-scale **virtual reality game** in which the player is the sole caretaker of a deep-space "ark" carrying Earth's last surviving ecosystems. Like the biblical Noah preserving life through the flood, the player must keep three fragile biomes — an **Aquatic reef**, an **Arctic tundra**, and a **Rainforest** — alive across a multi-year journey. Each waking cycle, systems fail, invasive species appear, and temperatures drift. If the player doesn't diagnose and physically repair each fault in time, the damage becomes **permanent** — and a piece of the ecosystem is lost forever.

The game is built in **Unity** for PC VR headsets (HTC Vive / Valve Index / Oculus via SteamVR & OpenXR), and is played entirely through motion controllers — you crank valves, plug in wiring, dig with a shovel, spray humidity, pull levers, and pick up invasive creatures with your own hands.

> Built as a semester-long collaborative capstone project by a four-person team at the **University of Nebraska–Lincoln**.

---

## The Core Loop

NoahARK is structured around a **Wake / Sleep cycle** that drives the entire game:

1. **Wake Phase** — The ark powers on for the "year." The player explores the biomes while a live clock counts down. Behind the scenes, a scheduler randomly triggers **faults** — a coral bleaching event, an overheating enclosure, a swarm of invasive starfish, a humidity crash in the rainforest.
2. **Respond** — The player must find and physically fix each fault using VR interactions before the timer runs out. An audio alert and a "power-down" warning give the player fair notice.
3. **Sleep Phase** — The ark powers down. Any unresolved faults inflict **permanent, escalating damage** on that biome (Healthy → Damaged → Critical → Extinct).
4. **Advance** — A new year begins, faults reset, and the difficulty continues. Survive all **5 years** with your ecosystems intact to win.

This creates a tense, time-pressured gameplay rhythm where neglect has lasting consequences — the central theme of the project.

---

## The Three Biomes

Each biome is an independent, self-contained system with its own state machine, fault types, and hand-crafted VR puzzles.

| Biome | Faults the player must fix | Signature VR interactions |
|-------|---------------------------|---------------------------|
| **Aquatic Reef** | Coral bleaching, nutrient depletion, invasive species outbreaks, temperature spikes | Grab and remove invasive starfish, crank a nutrient valve, place healing crystals into drop zones |
| **Arctic Tundra** | Rising temperatures melting igloos & icicles, electrical faults | Rewire a power box (plug/socket puzzle), dig through snow with a shovel, operate locks and alarms |
| **Rainforest** | Humidity loss, temperature drift, plant die-off, fog | Pull levers, press holographic hover-buttons, operate a humidity spray system |

Biome health is **visual and permanent** — coral progressively bleaches through multiple material stages, igloos crack and collapse, and plants wither, so the player can *see* the state of their ark at a glance.

---

## Tools & Technologies

| Category | Stack |
|----------|-------|
| **Engine** | [Unity](https://unity.com/) `2022.3.62f2` (LTS) |
| **Language** | C# |
| **Rendering** | Universal Render Pipeline (URP), custom Shader Graph water & aquarium volumes |
| **VR / XR** | SteamVR / OpenVR, OpenXR, Unity XR Interaction Toolkit, XR Management, Oculus XR |
| **Interaction** | Valve SteamVR Interaction System (grab, hover, haptics) |
| **Level Design** | Unity ProBuilder, Unity Terrain |
| **UI / Text** | TextMesh Pro, Unity UI (uGUI) |
| **Cinematics** | Unity Timeline (sleep/wake cutscenes) |
| **Audio** | Unity Audio — surface-aware footsteps, spatial fault alerts, boot-up & power-down SFX |
| **Version Control** | Git / GitHub |

---

## Engineering Highlights

A few of the systems built from scratch for this project:

- **WakeCycleManager** — An event-driven game clock that manages the year/wake/sleep state machine and fires C# `event`-based scheduled events that the rest of the game subscribes to (`OnScheduledEvent`, `OnStateChange`, `OnYearChange`, `OnComplete`). This is the backbone that keeps every biome in sync.
- **Fault System** — A reusable, polymorphic fault architecture (`AquaFaultBase` and per-biome subclasses) with a manager that randomly activates a subset of faults each day and tracks resolution — enabling replayability and easy content expansion.
- **VR Interaction Scripts** — Hand-authored interactables for grabbing invasive species (with haptics, particles, and physics hand-off), a rotation-tracked valve crank, and a plug-and-socket wiring puzzle — each with keyboard **debug fallbacks** so the game can be tested without a headset.
- **Surface-Aware Footstep System** — A `FootstepManager` / `FootstepSurface` pair that detects the terrain under the player via trigger colliders and plays randomized, pitch-varied footstep audio per surface type.
- **Custom Locomotion** — A `CharacterController`-based smooth-movement rig driven by SteamVR joystick input, with head-relative direction, sprint, and gravity.
- **Atmospheric Polish** — Rotating skyboxes, animated fog, coral-bleaching material transitions, and Timeline-driven sleep cutscenes.

---

## Running the Project

> **Requirements:** A PC-VR headset (Valve Index, HTC Vive, or Oculus/Meta via Link) with **SteamVR** installed, and **Unity 2022.3.62f2**.

1. **Clone the repository:**
   ```bash
   git clone https://github.com/unlabiri/NoahArk.git
   ```
2. **Open in Unity Hub** — Add the `NoahARK/` folder as a project (Unity will auto-restore all packages listed in `Packages/manifest.json`).
3. **Connect your headset** and start SteamVR.
4. **Open a scene** from `Assets/Scenes/` — `Milestone4.unity` is the most complete showcase build — and press **Play**.

*No VR headset? Many interactions (valve crank, etc.) include keyboard debug keys so core systems can be tested on a flat screen.*

---

## Team

Built collaboratively by a four-person team at the **University of Nebraska–Lincoln**:

- **Jina Bagheri** 
- **Ada Aljabiri**
- **Joshua Martinez**
- **Jude Kroeze**

---

## Repository Structure

```
NoahArk/
└── NoahARK/                     # Unity project root
    ├── Assets/
    │   ├── Scripts/             # Core game systems
    │   │   ├── WakeCycleManager.cs      # Master game clock & state machine
    │   │   ├── PlayerMovement.cs        # VR locomotion
    │   │   ├── FootstepManager.cs       # Surface-aware footstep audio
    │   │   ├── Aquatic Scripts/         # Reef biome, coral, faults
    │   │   ├── Arctic Scripts/          # Tundra biome, wiring, melting
    │   │   ├── Rainforest Scripts/      # Rainforest biome, humidity, fog
    │   │   └── SleepCutscene/           # Timeline cutscene managers
    │   ├── Scenes/              # Milestone & showcase scenes
    │   ├── SteamVR/             # SteamVR interaction system
    │   └── ...                  # Models, materials, audio, prefabs
    ├── Packages/               # Unity package manifest
    └── ProjectSettings/        # Unity project configuration
```

---

*NoahARK — because every ecosystem deserves a second chance.* 🌱

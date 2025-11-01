# Unity Essentials

This module is part of the Unity Essentials ecosystem and follows the same lightweight, editor-first approach.
Unity Essentials is a lightweight, modular set of editor utilities and helpers that streamline Unity development. It focuses on clean, dependency-free tools that work well together.

All utilities are under the `UnityEssentials` namespace.

```csharp
using UnityEssentials;
```

## Installation

Install the Unity Essentials entry package via Unity's Package Manager, then install modules from the Tools menu.

- Add the entry package (via Git URL)
    - Window → Package Manager
    - "+" → "Add package from git URL…"
    - Paste: `https://github.com/CanTalat-Yakan/UnityEssentials.git`

- Install or update Unity Essentials packages
    - Tools → Install & Update UnityEssentials
    - Install all or select individual modules; run again anytime to update

---

# HDRP Settings

> Quick overview: Inspector‑first HDRP settings component that surfaces runtime‑loaded options using Unity Essentials Enum Drawer, and lets you load/save user choices to JSON. A separate package will generate a UI Toolkit menu from these settings.

This package provides a single, inspector‑driven way to view and tweak key High Definition RP (HDRP) options at runtime without custom UI code. Options are presented as clean enums, flags, toggles, and ranges using the Unity Essentials Enum Drawer modules. User selections can be applied immediately to the active HDRP asset/global settings and persisted to a JSON file. A separate, optional package will turn the same settings data into an in‑game settings menu (UI Toolkit).

![screenshot](Documentation/Screenshot.png)

## Features
- Inspector component for HDRP settings
  - Attach a “HDRP Settings” MonoBehaviour to a GameObject and configure graphics options directly in the Inspector
  - Options are organized into categories (e.g., Display, Anti‑Aliasing, Shadows, Lighting, Volumetrics, Post‑Processing)
  - Uses Unity Essentials Enum Drawer for enums/flags with readable names and optional icons
- Runtime‑loaded options
  - Settings are discovered from the active HDRP Global Settings and Render Pipeline Asset at startup
  - Only relevant options are shown; unavailable features are hidden or disabled
- Apply, Load, Save
  - Apply: writes choices to the current HDRP asset/global settings immediately
  - Save: serializes to JSON (e.g., `Application.persistentDataPath/UnityEssentials_HDRPSettings.json`)
  - Load: restores from JSON at startup or on demand; falls back to project defaults if missing
- Profiles and presets
  - Optional preset assets can be loaded into the component to seed defaults
  - Per‑category “Reset to Defaults” commands
- Extensible providers (theoretical)
  - A small provider interface can allow adding/removing settings groups without changing the component
  - Providers can register getters/setters that read/write HDRP APIs safely in Play and Edit modes
- Separate menu generator (future/companion package)
  - Another package will consume the same settings model and build a UI Toolkit menu (keyboard/gamepad‑friendly)
  - The menu reads/writes the same JSON so both inspector and in‑game menu stay in sync

## Requirements
- Unity Editor 6000.0+
- High Definition RP (HDRP)
- Unity Essentials Enum Drawer modules (for rich enum/flag rendering in the Inspector)
- JSON persistence
  - Uses `JsonUtility` by default; can be swapped for Newtonsoft.Json if your project already includes it

Tip: Keep editor/project defaults conservative; let users push features higher via this settings layer, and persist successful configurations to JSON.

## Usage
1) Add the settings component
- Create an empty GameObject (e.g., “HDRP Settings”) and add the HDRP Settings component
- On Awake/OnEnable, it scans the active HDRP asset/global settings, populates options, and tries to auto‑load JSON

2) Configure options
- Adjust categories such as:
  - Display: VSync, dynamic resolution, frame cap
  - Anti‑Aliasing: None/TAA/DLSS/FSR (shown only if supported)
  - Shadows: distance, cascades, filtering
  - Lighting/Volumetrics: fog, volumetric quality, exposure mode
  - Post‑Processing: bloom, motion blur, AO, SSR/SSAO, color adjustments
- Enum/Flag fields render with readable entries thanks to Enum Drawer

3) Apply and persist
- Click Apply to write changes to HDRP at runtime
- Click Save to serialize JSON to persistent data
- Click Load to re‑apply from JSON (or auto‑load on startup)

4) Integrate with your flow (optional)
```csharp
public class HdSettingsBootstrap : MonoBehaviour
{
    public HdrpSettingsComponent Settings; // the inspector component

    void Start()
    {
        // Ensure current user profile is loaded
        Settings.LoadFromJson();
        // Apply immediately (also available via Inspector button)
        Settings.ApplyToHdrp();
    }

    public void SetAA(int mode) // called from UI or code
    {
        Settings.AntiAliasing = (AntiAliasingMode)mode;
        Settings.ApplyToHdrp();
        Settings.SaveToJson();
    }
}
```

## How It Works (theoretical design)
- Discovery
  - On enable, the component queries HDRP Global Settings and the active `HDRenderPipelineAsset` to gather supported options
  - Each option is wrapped as a “setting item” (key, display name, type, current value, apply handler)
- Presentation
  - The inspector draws items by category using Enum Drawer for enums/flags and standard drawers for toggles/sliders
  - Unsupported items are hidden or shown as disabled with a tooltip explaining why
- Application
  - When you click Apply (or call `ApplyToHdrp()`), each setting’s apply handler writes to HDRP APIs in a safe order
  - Some settings may require a camera refresh or pipeline rebind; the component minimizes disruptions
- Persistence
  - A small serializable model (versioned) is saved as JSON under `persistentDataPath`
  - On Load, unknown keys are ignored; missing keys use defaults; version upgrades can migrate as needed
- Extensibility
  - Optional provider interface (`IHdrpSettingsProvider`) lets you plug additional categories for custom projects

## Notes and Limitations
- Runtime vs Edit Mode
  - Most options can be toggled in Play Mode; a few may require reloading the pipeline or scene to fully take effect
- Platform support
  - Some features (DLSS/FSR, certain denoisers) depend on platform/GPU and HDRP packages; unavailable options are hidden
- Safety
  - The component should validate ranges and clamp to HDRP limits to avoid invalid states
- JSON location
  - Default path is `Application.persistentDataPath`; projects can override path/name if needed
- Menu generation
  - The in‑game menu is deliberately excluded here; a separate package will generate a UI Toolkit menu from the same model

## Files in This Package
- Runtime scaffolding present in this repo today (subject to change as this package is finalized):
  - `Runtime/SettingsMenuBase.cs`
  - `Runtime/Audio/` (category placeholder)
  - `Runtime/Display/` (category placeholder)
  - `Runtime/UnityEssentials.HDRPSettings.asmdef`
- `Resources/` – reserved for defaults/presets (if used)
- `package.json` – Package manifest metadata

## Tags
unity, hdrp, settings, graphics, quality, enum drawer, json, inspector, ui toolkit (menu in separate package)

# Pixygon — Micro

The **virtual console** — "games inside games." A retro-console shell (home screen,
account, cartridges, display) plus small gameplay primitives reused across titles.

## Key types

| Type | What it is |
|---|---|
| **`MicroController` / `ConsoleController` / `DisplayController` / `HomeController`** | The virtual console + its home screen. |
| **`Cartridge` / `CartridgeObject` / `CartridgeController` / `CartridgeSelector`** | Games-as-cartridges (load/select). |
| **Home screens** (`HomeAccountScreen`, `SystemSettingsScreen`, `AudioSettingsScreen`, `DisplaySettingsScreen`, `AccountLogin`/`AccountSignUp`, `AccountWallet`, `ThemeOrganizer`) | The console's front-end. |
| **`SkinCard`** | ⭐ A collectible **skin** (title/rarity/supply/price + `NFTLink`) — the premium-cosmetic layer (e.g. avatar parts). |
| **`Pickup` / `DamageObject`** | Base gameplay primitives (collectibles / damage volumes) games extend. |
| **`Level` / `LevelData` / `MissionData` / `ScoreManager` / `SplashScreen` / `LevelLoader`** | Mini-game level + score flow. |
| **`Taptic` / `AndroidTaptic`** | Haptics. |

## Dependencies

None declared (uses `core.Rarity`, `NFT.NFTLink` for `SkinCard`).

## Status

`0.7.3`. **Platform notes:** `SkinCard` is the cosmetic-collectible layer for the
avatar system; `Pickup`/`DamageObject` are base types `actors`/games build on. The
Home account screens overlap `com.pixygon.passport` — clarify the boundary (Passport
owns auth; Micro provides the console UI).

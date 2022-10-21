# Changelog

## Unreleased
### Fixed
- MicroActorSpawners spawns enemies inside it
- HP is now set in MicroActor
- Fixed error in sprite import postprocessor
- Bug in animation where player didnt land
- No movement during I-frames
- Made Invincible-bool a public property
- Bug in animation not entering idle-state

### Changed
- Jumps are now divided into input-check and actual jump
- CartridgeController is spawned after ConsoleController
- IFrames can now be set in MicroActor

### Added
- Added method to force jump
- MicroActors die when falling off edges
- MicroActors can be destroyed when dying
- XFlip in PlatformController is now public
- Bounces on enemies/damageObjects

## 0.5.3
### Fixed
- BGM should now play at the start of a level

### Changed
- Game starts in menu, player must select level to load level
- Display-quad is now inside console

### Added
- Sprite Import postprocessor
- AnimatorController for players
- Fields for GameOverScreen
- Fields for MenuScreen
- More logging on levelselect and start
- Camera-stacking for UI and default view
- UI-script that does basic UI-functions
- Cartridge version

## 0.5.2
### Changed
- Unserialized fields in CameraController
- Cleaned up the Parallax-system and made it better system
- Adjusted camera-length
- Post-processing volume set through LevelData

### Added
- Added levelData

## 0.5.1
### Changed
- Moved some properties into MicroController
- Changed some namespaces of new scripts, and decoupled them more
- Parallax uses local-space for position
- ParallaxLayers are set with parallax-data
- Cleaned up folder structure

### Added
- Added sprite-flipping to PlatformMovement
- Added CameraController
- Added Parallax
- Added LevelLoader and Levels
- Added MicroActor
- Added Post-processing
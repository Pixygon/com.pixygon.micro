# Changelog

## Unreleased
### Fixed
- BGM should now play at the start of a level

### Added
- Sprite Import postprocessor
- AnimatorController for players

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
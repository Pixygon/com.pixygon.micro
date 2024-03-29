# Changelog

## Unreleased

## Changelog v0.7.1
### Added Features
   - Pause the game when home-menu is active
   - Add a popup for recent changes
   - Fix input for mobile

### Fixed
   - If exiting CartridgeSelector with Home-button, instead of back/select, cartridge isn't deleted
   - Make sure fonts don't fuck up
   - Login not working
   - Add back-button to Cartridge-select
   - Add back-button to Faceplate-select
   - Add left/right buttons to faceplate and cartridge-select

## 0.6.0
### Fixed
- Bug where levels could be double-loaded
- Bug where BGM starts playing twice
- Bug where audiomixers weren't properly loaded

### Changed
- More info for faceplates

### Added
- LevelObjects, that can be reset
- Added more options for pickup-interactions
- Added color-options for Parallax-effect
- Exposed all InputActions in InputController
- PagedContentManager for Cartridges
- Made item-selection clearer for cartridges and faceplates
- Scoremanager for keeping track of scores!
- ActorSpawner can now repeatedly spawn more enemies
- Added field for Collectables
- Killheight is now set in levels
- Added AIListener-system
- Added AIReaction-system
- Added AIMover-system
- Major improvements to AI
- Added more info to faceplates
- Added different ItemSpawners
- Added login to wax-wallet
- New way of selecting faceplates
- New way of selecting cartridge
- Added new Home-menu
- Added Pixygon Sign Up
- Added Pixygon Sign In
- Added Pixygon Passport

## 0.5.7
### Fixed
- If no gamepad connected, rumble would not work
- Error with sortinglayers
- Parallax: Temporarily removed the "infinite" scroll that was causing issues
- InputAsset is now a property!

### Changed
- Changed Coin-script into Pickup-script!

### Added
- First test of Pausing game
- Automatic gathering of coins and spawners in level
- Added new console-version
- Console-buttons move according to input
- More customization-options for faceplates

## 0.5.6
### Fixed
- Bug with no parallax-layers
- Pickups should play effect now
- Levels should not double-spawn anymore

### Changed
- Exposed CurrentLevel of LevelLoader
- Exposed CurrentLevelData of LevelLoader
- Made some things in MicroActor protected instead of private
- Added asmDef to parallaxing
- Parallax should scroll indefinitely
- Upgraded InputSystem to 1.4.4

### Added
- Points-value added to MicroActorData
- Added JumpBuffer
- Added CoyoteTime
- Added LevelName
- Added ConsoleZoom
- Added layer "PixygonMicroActorsBG"
- Added "CannotMove"-field in PlatformMovement-HandleMovement
- Added Level Start/End-objects to UI
- Set LevelStart + text-method
- Coins have a Taken-property, to avoid taking them twice
- UI: Added animation options to points and coins
- HomeMenu with settings, cartridges, faceplates and trophies
- Volume-settings
- Button-names on console

## 0.5.5
### Fixed
- Run-particles should be off on death now
- Run-particles should only appear when actually moving now
- Build never skips intro
- Unkillable actors are always marked as Invincible
- Parallax-layers should not be duplicated anymore

### Changed
- Player is loaded through MicroActorData
- MicroActorData is now a public Property
- Most things on LevelData is now AssetReferences
- Almost all assets properly loaded through Addressables
- Increased screen-size to 320x180
- UI should now correctly show 0 life if you have 0 life
- Added points-system to UI
- PlatformMovement: Exposed IsGrounded-bool
- IFrames makes characters red instead of invisible

### Added
- New player-layer
- New actor-layer
- Added animations to Parallax-layers
- More dynamic setup of Actors
- New fields in MicroActorData for "IsHostile", "IsKillable"
- New fields in MicroActorData for "Detect gaps", "Detect obstacles"
- Added Goals and Level advancing
- Added Screen-shake to camera controller
- Added intensity-value to screen-shake
- Added rumble to controllers
- Added "Snap"-camera function

## 0.5.4
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
- Added X/Y-lock to parallaxData

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
# Game Restart Functionality Setup Guide

## Overview
This guide explains how to properly set up the restart functionality for your Unity game with three modes (Grass, City, Snow).

## Key Components

### 1. GameManager.cs
- **Main controller** for all game modes
- **Tracks current mode** using `currentMode` string variable
- **RestartCurrentMode()** method handles restarting the appropriate mode
- **RestartButtonPressed()** method should be called from Game Over screen

### 2. Mode-Specific Managers
- **CityManager.cs** - Handles City mode
- **SnowManager.cs** - Handles Snow mode  
- **GameManager.cs** also handles Grass mode directly

## Setup Instructions

### Step 1: Unity Inspector Setup

1. **Create GameManager GameObject:**
   - Create empty GameObject named "GameManager"
   - Attach `GameManager.cs` script
   - Assign all UI screen GameObjects (startScreen, gameOverScreen, etc.)
   - Assign UI collections for each mode
   - Assign prefab lists and holders

2. **Create Mode Managers:**
   - Create empty GameObject named "CityManager" 
   - Attach `CityManager.cs` script
   - Create empty GameObject named "SnowManager"
   - Attach `SnowManager.cs` script

3. **Assign References:**
   - In each manager, assign their respective UI elements
   - Assign prefab lists for obstacles/enemies
   - Assign player GameObjects
   - Assign holders for spawning

### Step 2: Game Over Screen Setup

**Important:** On your Game Over screen, make sure the restart button calls:
```csharp
GameManager.instance.RestartButtonPressed();
```

### Step 3: Button Event Setup in Unity

1. **Select your Game Over screen's Restart Button**
2. **In the Button component's OnClick event:**
   - Click "+" to add new event
   - Drag GameManager GameObject to the object field
   - Select `GameManager > RestartButtonPressed()`

### Step 4: Testing Each Mode

**Test the restart functionality:**

1. **Grass Mode:**
   - Start game → Select Grass mode → Play → Game Over → Restart
   - Should return to Grass mode with reset score/lives

2. **City Mode:**  
   - Start game → Select City mode → Play → Game Over → Restart
   - Should return to City mode with reset score/lives

3. **Snow Mode:**
   - Start game → Select Snow mode → Play → Game Over → Restart  
   - Should return to Snow mode with reset score/lives

## Key Features

### ✅ What the restart system does:
- **Remembers current mode** when switching between modes
- **Resets score to 0** on restart
- **Restores all lives** on restart  
- **Clears all existing obstacles** from scene
- **Resets object speeds** to initial values
- **Restarts countdown** properly
- **Maintains proper UI state** for each mode

### ✅ Restart Flow:
1. Player dies → Game Over screen appears
2. Player clicks restart button
3. `RestartButtonPressed()` is called
4. `RestartCurrentMode()` determines which mode was active
5. Calls appropriate reset method for that mode
6. Game restarts in the same mode

## Troubleshooting

### Common Issues:

1. **Restart button not working:**
   - Check if button's OnClick event is properly set to `GameManager.RestartButtonPressed()`

2. **Wrong mode starts after restart:**
   - Check if `currentMode` is being set correctly in mode selection buttons

3. **Objects not clearing:**
   - Verify holder references are assigned in inspector
   - Check that obstacles are children of the correct holders

4. **Lives not resetting:**
   - Ensure `livesList` is populated in inspector with UI Image components

5. **Null reference errors:**
   - Check that all managers have their `instance` properly set in Awake()

## Additional Notes

- The system uses **singleton pattern** for all managers
- **Time.timeScale** and **AudioListener.pause** are properly managed
- **PlayerPrefs** are used for high score persistence
- Each mode has its own **high score key** ("HighrstScore", "HighestCity", "HighestSnow")

## PlayerPrefs Keys Used:
- `"HighrstScore"` - Grass mode high score  
- `"HighestCity"` - City mode high score
- `"HighestSnow"` - Snow mode high score
- `"SelectedCharacter"` - Character selection

Make sure to test thoroughly in each mode to ensure the restart functionality works correctly!
# Caterpillar in the dark forest

This is the result of modules 04 and 05 of 42 school Unity Piscine : 7 modules made to learn Unity fundamentals.  

## Game Overview
- 2D platformer where the player controls a caterpillar in a dark forest
- Objective : reach the level end while avoiding enemies and collecting at least 5 leaves
- 1 leaf = 5LP (leaf point)
- Player starts with 3HP, game over at 0HP
- Respawns at the beginning on death
- 3 stages

### Controls
- Movement: AD or left/right arrows
- Jump: Spacebar
- Main menu and restart buttons in stages
- Can resume or start a new game in main menu

### Enemies
- Cactus: throws poisonous jelly
- Vine: attacks by extending toward the caterpillar

---

## Technical Details
### Architecture
- One scene per stage
- 2 UI scenes (MainMenu and Diary)
- Singleton `GameManager`: update PlayerPerfs, handle levels
- One `LevelManager` by stage: save level infos
- Separation between game logic and UI (one script by UI screen)
- Decoupled systems using event-driven architecture (C# Actions)
- Modular and reusable components for enemies and detection systems
- Reusable prefabs
- Animations triggered by booleans
- Background and camera following the player

### Unity Features Used
- Tile palette
- Animator (including blend tree)
- Audio Source
- Scene management
- PlayerPrefs

### What I learned
- Creating animations from sprites and configuring Animator (blend trees)
- Synchronizing sound effects using animation events
- Saving and loading progress using PlayerPrefs
- Managing persistent UI across scenes (DontDestroyOnLoad)
- Using coroutines (fading screen for example)

## Preview
### Main menu
<img width="1536" height="867" alt="image" src="https://github.com/user-attachments/assets/9332ed44-48fd-4a37-9ca7-9ae74510a3be" />

### In game
<img width="1534" height="864" alt="image" src="https://github.com/user-attachments/assets/f201a18c-07b1-4cd6-ae2d-62a8b62d8971" />  

<img width="1536" height="865" alt="image" src="https://github.com/user-attachments/assets/945eaa7d-e087-4147-a811-f70cb16db18a" />

### Diary
<img width="1534" height="863" alt="image" src="https://github.com/user-attachments/assets/31b105ec-25de-4758-ad4c-067cbad1ea06" />









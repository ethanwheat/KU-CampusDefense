# KU Tower Defense Game

## Overview

This is a **3D game with 2D gameplay** developed in Unity, themed around **defending Allen Fieldhouse from rival fans**. Players unlock campus-inspired buildings and manage resources to deploy defenses against waves of enemies. The game features **strategic depth, resource management, and progressively increasing challenges**.

## Project Setup

### 1. Clone the Repository

To get started, run the following command:

```bash
git clone https://github.com/ethanwheat/KU-CampusDefense.git
cd KU-CampusDefense
```

### 2. Open in Unity

1. Open **Unity Hub**.
2. Click **"Open"** and navigate to the cloned project folder.
3. Select the **root folder** (where `Assets/`, `Packages/`, and `ProjectSettings/` are located).
4. Unity will automatically generate missing files (`Library/`, `Logs/`, etc.).

### 3. Install Dependencies (If Needed)

If Unity prompts errors about missing packages:

1. Open **Window > Package Manager**.
2. Click **"Refresh"**.
3. Install any missing packages.

## Development Guidelines

### Folder Structure

- `Code` → Gameplay logic (AI, movement, UI handling, etc.)
- `Prefabs` → Reusable game objects (enemies, towers, projectiles)
- `Scenes` → Level design & game world setup
- `Art` → 3D models, textures, animations
- `UI` → Menus, HUD elements
- `Audio` → Sound effects and music
- `Shaders` → Custom rendering effects

### Git Workflow

#### **Pull Latest Changes Before Starting Work**

```bash
git pull origin main
```

#### **Commit and Push Changes**

```bash
git add .
git commit -m "Your message here"
git push origin main
```

#### **Best Practices to Avoid Merge Conflicts**

- **Use prefabs** instead of modifying scene files directly.
- **Communicate with the team** when making large changes.
- **Commit small, logical changes frequently**.

## Contributors

- [Ethan Wheat]
- [Gavin Kirwan]
- [Tanner Barcus]
- [Edgar Mendez]
- [Mario Sumanasekara]

## License

This project is for educational purposes at the **University of Kansas**.

---


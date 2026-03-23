# Zombie Shooter

A 2D top-down survival shooter built with **Godot 4** and **C# (.NET 8)**. This project is a tribute to classic zombie survival games, featuring iconic mechanics like perks and mystery boxes.

![Adobe Express - Screencast From 2026-03-23 11-14-47](https://github.com/user-attachments/assets/cfbb7200-aeec-433b-9978-db42e7f4b191)

---

## 🧟 Project Overview
This is my first project in the Godot engine. It focuses on implementing core arcade shooter mechanics, C# signal management, and efficient 2D pathfinding for enemy hordes using Godot's built-in Navigation systems.

### Core Features
* **Classic Perk System:** Fully functional Perk-a-Cola logic:
    * **Juggernog:** Increased player health capacity.
    * **Speed Cola:** Reduced reload times.
    * **Quick Revive:** Faster health regeneration and survival mechanics.
    * **Stamin-Up:** Buffed movement speed and sprint duration.
    * **Double Tap:** Increased fire rate for all weapons.
* **Mystery Box:** A randomized weapon spawning system with weighted probabilities.
* **Interactive Environment:** Points-based economy used to open doors and clear debris to expand the playable area.
* **Smart AI Pathfinding:** Zombies utilize `NavigationRegion2D` and `NavigationAgent2D` to dynamically track the player through complex map layouts.
* **Point System:** Earn currency through combat to fund gear, perks, and map exploration.

---

## 🛠 Tech Stack
* **Engine:** Godot 4.x
* **Language:** C#
* **Framework:** .NET 8
* **Navigation:** NavigationServer2D / NavigationRegion2D

---

## 🚀 Getting Started

### Prerequisites
1.  Download and install the [Godot Engine - .NET Edition](https://godotengine.org/download).
2.  Install the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
3.  A C#-compatible IDE (VS Code, Visual Studio, or JetBrains Rider).

### Installation
1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/DennisDahlberg/ZombieShooter.git](https://github.com/DennisDahlberg/ZombieShooter.git)
    ```
2.  **Open in Godot:**
    * Launch the Godot Project Manager.
    * Click **Import** and select the `project.godot` file.
3.  **Build the Project:**
    * Click the **Build** button (hammer icon) in the top-right corner of the Godot Editor to initialize the .NET solution.

---

## 🎮 Controls
* **WASD:** Movement
* **Left Click:** Shoot
* **R:** Reload
* **F / E:** Interact (Buy Perks, Open Doors, Use Mystery Box)

---

## 🗺 Roadmap (Work in Progress)
- [ ] Round/Wave transition system logic.
- [ ] Wall-buy weapon triggers.
- [ ] Unique sprites for guns
- [ ] Zombie spawning intensity scaling.
- [ ] UI/HUD polish (Perk icons and point counters).
- [ ] Sprint functionality with fatigue
- [ ] Improved weapon variety (Shotguns with spred, explosives etc.)
- [ ] Pack-a-punch (With sprite change for guns)
- [ ] Mini map
- [ ] Pause / Menu functionality

---

## 📝 Credits
* **Assets:** [Kenney.nl](https://opengameart.org/content/topdown-shooter)

---

*Note: This project is a learning experiment and a work in progress.*

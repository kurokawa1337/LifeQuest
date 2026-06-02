# ⚔️ LifeQuest (Gamified To-Do Tracker)

Welcome to **LifeQuest**! This is a gamified console-based task management application built with C#. It transforms your boring daily routines and chores into an exciting RPG adventure. Complete real-life tasks (Quests), earn Experience Points (XP), level up, and unlock new ranks!

## 🚀 Key Features

* **📝 Quest Management:** Create, view, complete, and delete your daily tasks.
* **🎮 Gamification System:** * Tasks are categorized by difficulty (Easy, Medium, Hard), each rewarding a different amount of XP.
  * Dynamic leveling system: fill your XP bar to level up.
* **👑 Rank System:** Unlock new titles every 10 levels (from *Novice* to *Legend*).
* **🌟 Prestige Mechanics (End-game):** Once you reach Level 100, you can "Rebirth" (Prestige) to reset your level back to 1 but permanently increase your XP multiplier.
* **💾 Data Persistence:** All your tasks and player progress are automatically saved locally using `System.Text.Json` (`tasks.json` and `player.json`).
* **🛠 Developer Mode:** A hidden menu for testing purposes to instantly grant XP or modify levels.

## 💻 Tech Stack

* **Language:** C#
* **Framework:** .NET (Console Application)
* **Data Storage:** JSON (`System.Text.Json`)
* **Architecture:** Object-Oriented Programming (OOP) with clean separation between UI logic and data models.

## 🗺️ Roadmap & Development Stages

- [x] Design core architecture and OOP models (`TaskItem`, `UserProfile`).
- [x] Implement basic CRUD operations for tasks.
- [x] Develop XP calculation and Leveling math.
- [x] Implement local JSON serialization/deserialization.
- [x] Add advanced RPG mechanics (Ranks, Prestige Multipliers).
- [x] Create a dynamic colored console UI.
- [ ] **Future Plan:** Port the application to a modern GUI framework (WPF or .NET MAUI).
- [ ] **Future Plan:** Add cloud synchronization (Google Drive / Firebase) to access quests across multiple devices.
- [ ] **Future Plan:** Introduce daily streaks and randomly generated weekly challenges.

## 🎮 How to Run

1. Clone this repository to your local machine:
   ```bash
   git clone [https://github.com/YourUsername/LifeQuest.git](https://github.com/YourUsername/LifeQuest.git)

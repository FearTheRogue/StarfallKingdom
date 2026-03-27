# Starfall Kingdom
![Unity](https://img.shields.io/badge/Engine-Unity%206-black?logo=unity)
![C#](https://img.shields.io/badge/Language-C%23-239120?logo=c-sharp&logoColor=white)
![Status](https://img.shields.io/badge/Status-In%20Development-blue)
![Platform](https://img.shields.io/badge/Platform-PC-lightgrey)
![Genre](https://img.shields.io/badge/Genre-Top--Down%20Action%20RPG-6a5acd)
![AI](https://img.shields.io/badge/AI-NavMesh-green)
![Input](https://img.shields.io/badge/Input-New%20Input%20System-orange)
![Focus](https://img.shields.io/badge/Focus-Gameplay%20Programming-brightgreen)

A top-down action RPG prototype built in **Unity 6**, inspired by click-to-move RPGs.  
This project is being developed to strengthen my practical gameplay programming skills, with a focus on **clean code architecture**, **modular systems**, and **scalable feature development**.

---

## About This Project

**Starfall Kingdom** is a solo development project I am using to demonstrate my ability to design and implement gameplay systems in a structured, maintainable way.

The current prototype includes:

- Click-to-move player movement using **NavMesh**
- Smooth camera follow with zoom
- Target selection and interaction
- Player combat and pickup systems
- Shared animation architecture for player and enemy characters
- Enemy wandering AI
- Blend tree locomotion for animations
- Modular script refactoring into focused components

This repository is intended to show not just a finished result, but also **how I approach development as a junior programmer**:
- breaking larger systems into smaller components
- refactoring tutorial code into cleaner architecture
- building reusable systems rather than one-off solutions
- thinking about scalability early

---

## Why This Project Matters

One challenge for junior developers is proving practical development ability without formal industry experience.  
This project helps bridge that gap by demonstrating:

- **Ownership of a complete gameplay prototype**
- **Problem-solving and iteration**
- **Refactoring beyond tutorial code**
- **Understanding of maintainability and code structure**
- **Ability to build systems that can scale as a project grows**

Rather than treating this as a one-off tutorial exercise, I am using it as a way to practise working more like a professional gameplay programmer:
- planning features
- implementing them incrementally
- testing and refining behaviour
- improving structure as the codebase grows

---

## Features Implemented

### Player Systems
- Click-to-move navigation
- Target-based interaction system
- Combat and item pickup logic
- Movement facing and target facing
- Shared character animation controller
- Sprint-ready movement structure
- Ground click feedback and target indicators

### Enemy Systems
- Idle and walk locomotion
- Random wandering using NavMesh
- Movement-based animation driving
- Shared animation setup with the player

### Camera Systems
- Smooth follow camera
- Scroll wheel zoom
- Experimental orbit camera work

### Architecture / Refactoring
- `PlayerController` as a high-level coordinator
- `PlayerMovement` for movement and navigation responsibilities
- `PlayerCombat` for targeting and combat flow
- `PlayerEffects` for VFX and targeting feedback
- `CharacterAnimationController` shared across entities

---

## Technical Highlights

### Modular Design
A major focus of this project is moving away from large all-in-one scripts and toward a more maintainable component-based structure.

Examples include:
- separating movement, combat, effects, and animation responsibilities
- reusing a shared animation wrapper across player and enemy characters
- isolating systems so they can be tested and extended more easily

### Refactoring Tutorial Code
Some features began from tutorial-inspired foundations, but were then reworked to better reflect my own coding style and understanding.

This includes:
- restructuring large controller scripts
- improving animation handling
- replacing hard-coded logic with reusable methods
- adapting systems to support future expansion

### Gameplay Programming Focus
This repository is primarily focused on **gameplay systems**, including:
- AI movement
- player input handling
- interaction flow
- combat timing
- animation state communication
- camera behaviour

---

## Skills Demonstrated

- Unity 6 development
- C# gameplay programming
- Object-oriented programming
- Component-based architecture
- NavMesh-based movement and AI
- Animation blend trees and triggers
- Refactoring and code organisation
- Debugging and iterative improvement
- Building reusable systems for future expansion

---

## Current Development Goals

Planned or in-progress areas include:

- Enemy combat and aggro behaviour
- Enemy hit reactions and death handling
- Loot drops and item collection loop
- Sprint implementation and sprint animation
- Improved combat feedback
- Health bars and UI systems
- More enemy behaviour states
- Expanded world interaction

---

## Project Structure

```text
Assets/
├── Scripts/
│   ├── Player/
│   │   ├── PlayerController.cs
│   │   ├── PlayerMovement.cs
│   │   ├── PlayerCombat.cs
│   │   └── PlayerEffects.cs
│   ├── Characters/
│   │   └── CharacterAnimationController.cs
│   ├── Enemies/
│   │   ├── EnemyWander.cs
│   │   └── EnemyAnimationDriver.cs
│   └── ...
├── Animations/
├── Prefabs/
├── Scenes/
└── ...
```

---

## What I’ve Focused On As A Junior Developer

As someone working toward a junior gameplay/software role, I’ve used this project to develop habits that are valuable in a professional environment:

- Writing code that is easier to extend
- Reducing tightly coupled logic
- Naming things clearly and consistently
- Iterating on systems instead of settling for the first version
- Thinking about how a feature fits into the wider project
- Improving readability and maintainability as the project evolves

This project reflects the way I want to work in a team setting:  
**curious, practical, iterative, and focused on writing code that can grow with the project.**

---

## Running the Project

1. Open the project in **Unity 6**
2. Load the main scene
3. Press Play
4. Use mouse input to:
   - click to move
   - target interactables/enemies
   - interact with items
   - control the camera zoom

---

## Future Improvements

Some areas I want to continue improving include:

- stronger AI state handling
- cleaner separation of shared combat logic
- more robust UI feedback
- inventory and item systems
- more polished enemy behaviour
- better animation event workflows
- improved visual feedback during combat

---

## Repository Purpose

This repository is part of my portfolio as a **junior developer** and is intended to demonstrate:
- practical Unity/C# development ability
- system design thinking
- willingness to refactor and improve code
- a strong interest in gameplay programming

---

## About Me

I’m a junior developer with a strong interest in **gameplay systems**, **interactive software**, and **writing clean, maintainable code**.

This project is one of the ways I’m continuing to build practical experience outside of formal employment by creating, refining, and documenting systems that reflect real development thinking.

---

## Contact

- **LinkedIn:** [COMING SOON]
- **Portfolio:** [COMING SOON]
- **GitHub:** [COMING SOON]

---

## Notes

This project is actively evolving, and the repository is intended to showcase both:
- progress over time
- the way I approach learning, iteration, and technical problem-solving

If you're viewing this as part of an application or portfolio review, thank you for taking the time to look through my work.
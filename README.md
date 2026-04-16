# Starfall Kingdom
![Unity](https://img.shields.io/badge/Engine-Unity%206-black?logo=unity)
![C#](https://img.shields.io/badge/Language-C%23-239120?logo=c-sharp&logoColor=white)
![Status](https://img.shields.io/badge/Status-In%20Development-blue)
![Platform](https://img.shields.io/badge/Platform-PC-lightgrey)
![Genre](https://img.shields.io/badge/Genre-Top--Down%20Action%20RPG-6a5acd)
![AI](https://img.shields.io/badge/AI-NavMesh-green)
![Input](https://img.shields.io/badge/Input-New%20Input%20System-orange)
![Focus](https://img.shields.io/badge/Focus-Gameplay%20Programming-brightgreen)
![Architecture](https://img.shields.io/badge/Architecture-Component%20Based-success)

**Starfall Kingdom** is a solo Unity 6 project focused on building a modular top-down action RPG prototype with click-to-move gameplay, combat, gathering, inventory, and equipment systems.

This repository is part of my developer portfolio and is intended to demonstrate how I approach gameplay programming as a junior developer: by building practical systems, refactoring them as the project grows, and focusing on code that is readable, reusable, and easier to extend.

---

## Overview

The project began as a simple click-to-move prototype and has gradually expanded into a more structured gameplay sandbox.

Current areas of focus include:

- Player click-to-move movement using **NavMesh**
- Combat and interaction with enemies, items, and resource nodes
- Enemy wandering and animation-driven movement
- Resource gathering and tool-gated mining
- Inventory and equipment UI
- Shared animation architecture across player and enemies
- Ongoing refactoring into focused, single-responsibility components

Rather than trying to build everything at once, I am treating this project as an iterative development exercise, adding systems in small steps and improving the codebase as the scope grows.

---

## Why I Built This

One challenge as a junior developer is demonstrating practical ability when you do not yet have much formal industry experience.

This project is my way of showing that I can:

- Design and implement gameplay systems in Unity using C#
- Break larger problems into smaller, testable pieces
- Refactor prototype code into a cleaner architecture
- Build reusable components instead of relying on one large controller script
- Think about maintainability and scalability early
- Iterate on features instead of stopping at the first working version

This is not just a tutorial follow-along project. A major focus has been taking early systems and restructuring them into something closer to how I would want to work on a real project or as part of a team.

---

## Features Implemented

### Player Systems
- Click-to-move navigation using `NavMeshAgent`
- Target selection for enemies, items, and resources
- Combat interaction flow
- Pickup interaction flow
- Mining interaction flow
- Sprint mechanic with stamina bar UI
- Directional facing while moving
- Equipment-based mining requirement
- Shared movement-speed-driven animation system

### Enemy Systems
- Idle and walk locomotion
- NavMesh wandering within a defined area
- Animation driving based on movement speed
- Shared animation controller setup for player and enemies
- Groundwork for combat state expansion

### Resource Systems
- Mineable ore nodes
- Resource drops spawned per mining hit
- Physics-based ore drop launch and spin
- Pickaxe requirement for mining
- Pickaxe pickup and equipment slot support

### Inventory / Equipment Systems
- Basic inventory data model
- Runtime inventory slot system
- World pickups that can be collected into inventory
- Grid-based inventory UI
- Dedicated equipment area
- Pickaxe equipment slot with placeholder/empty state support

### Camera / UX
- Camera follow and zoom
- Target markers and click feedback effects
- Inventory UI interaction blocking world click-to-move input

---

## Technical Highlights

### Component-Based Architecture
A major focus of this project is moving away from oversized all-in-one scripts and toward a cleaner, more maintainable component-based structure.

Systems are being separated into focused responsibilities such as:

- `PlayerController` for coordination
- `PlayerMovement` for navigation and locomotion
- `PlayerTargeting` for target ownership and selection
- `PlayerInteraction` for combat, pickup, and mining actions
- `PlayerEffects` for click feedback, hit effects, and target visuals
- `CharacterAnimationController` as a shared animation wrapper
- `PlayerInventory` and `PlayerEquipment` for progression systems

This structure makes it easier to test, expand, and refactor systems independently.

### Refactoring as the Project Grows
A large part of this project has been revisiting earlier implementations and improving them as new requirements appeared.

Examples include:
- splitting player logic into smaller focused scripts
- introducing shared animation wrappers
- separating UI concerns from gameplay concerns
- replacing temporary booleans with clearer equipment/inventory systems
- evolving simple pickups into inventory-aware pickups

### Gameplay Programming Focus
The project is strongly centered on gameplay programming and runtime systems, including:

- player input handling
- AI movement
- interaction flow
- tool-gated resource gathering
- animation parameter driving
- combat timing and action states
- inventory/equipment state management
- UI integration with gameplay systems

---

## Skills Demonstrated

- Unity 6 development
- C# gameplay programming
- Object-oriented design
- Component-based architecture
- NavMesh movement and AI
- Animation blend trees and trigger-based actions
- Runtime inventory and equipment systems
- UI system integration
- Refactoring and code organisation
- Debugging and iterative feature development

---

## Current Project Structure

```text
Assets/
├── Scripts/
│   ├── Player/
│   │   ├── PlayerController.cs
│   │   ├── PlayerMovement.cs
│   │   ├── PlayerTargeting.cs
│   │   ├── PlayerInteraction.cs
│   │   ├── PlayerEffects.cs
│   │   ├── PlayerInventory.cs
│   │   ├── PlayerEquipment.cs
│   │   └── PickaxePickup.cs
│   ├── Inventory/
│   │   ├── InventoryItemData.cs
│   │   ├── InventorySlot.cs
│   │   ├── InventoryPickup.cs
│   │   ├── InventoryGridUI.cs
│   │   ├── InventorySlotUI.cs
│   │   ├── EquipmentSlotUI.cs
│   │   └── PlayerEquipmentUI.cs
│   ├── Characters/
│   │   └── CharacterAnimationController.cs
│   ├── Enemies/
│   │   ├── EnemyWander.cs
│   │   └── EnemyAnimationDriver.cs
│   ├── Resources/
│   │   ├── OreNode.cs
│   │   └── ResourcePickup.cs
│   └── ...
├── Animations/
├── Prefabs/
├── ScriptableObjects/
├── Scenes/
└── ...
```

---

## What This Project Shows About Me

As a junior developer, I want my work to show more than just “I can make a feature work.”

I want it to show that I am thinking about:

- how systems fit together
- how code can be improved over time
- how features should be structured so they can grow
- how to separate responsibilities instead of letting everything live in one script
- how to build in a way that would be easier to maintain in a team environment

This project reflects the way I want to work professionally: practical, curious, iterative, and focused on building systems cleanly rather than just quickly.

---

## Development Approach

The way I am building this project is intentional:

1. Implement a working version of a feature  
2. Test it in-engine  
3. Refactor when the responsibility becomes too broad  
4. Build the next system on top of that cleaner structure  

This has helped me practise a workflow that feels closer to real development than simply building isolated mechanics.

---

## Running the Project

1. Open the project in **Unity 6**
2. Load the main scene
3. Press Play

Current interactions include:
- click to move
- target enemies, items, and resource nodes
- mine resources once the pickaxe is equipped
- collect item pickups
- open inventory and view equipment UI
- use sprint and stamina UI systems

---

## Current / Planned Improvements

The next areas I want to expand include:

- enemy combat and aggro behaviour
- enemy hit reaction and death flow
- loot and reward loops
- more complete equipment support
- item detail panels and inventory polish
- better combat feedback
- tool visuals on the player character
- stronger enemy state handling
- expanded world interaction and progression systems

---

## Repository Purpose

This repository is part of my portfolio for junior development roles and is intended to demonstrate:

- practical Unity and C# ability
- gameplay programming fundamentals
- code organisation and refactoring
- willingness to improve systems instead of leaving them in prototype form
- the ability to keep building beyond a basic tutorial outcome

---

## About Me

I’m a junior developer with a strong interest in gameplay systems, interactive software, and writing maintainable code.

I use personal projects like this to build practical experience, strengthen my technical decision-making, and show how I approach development when given ownership of a feature or system.

---

## Contact

- **LinkedIn:** [COMING SOON]
- **Portfolio:** [COMING SOON]
- **GitHub:** [COMING SOON]

---

## Notes

This project is actively evolving and is intended to show both:

- the current state of the prototype
- the progression of my development process over time

If you’re viewing this as part of an application or portfolio review, thank you for taking the time to look through my work.

# StarWielder - Developer's notes

## Goal
Your ship consumes the Star's energy to power it. Without it, your ship will stop functionning and you'll be vulnerable to enemies firing at you. Collect energy by launching your Star into enemy ships's stars. Recall the Star at you to draw energy from it. 

The goal is to survive the longest.

---

## Controls

- **ZQSD** : Move Ship
- **Right Mouse Click** : Launch / Recall the Star
- **Space** : Dash

## Shortcuts

- **N** : Clear the current stage and launches next one (WIP)

## Game Settings Index

### Ship
	
- **[ShipSettings](../Assets/Scripts/Player/Ship/Settings/ShipSettings.cs)** : Assets/GameDesign/Player
- **[ShipEnergyConsumptionSettings](../Assets/Scripts/Player/Ship/Settings/ShipEnergyConsumptionSettings.cs)** : Assets/GameDesign/Player

### Sun
- **[Sun](../Assets/Scripts/Player/Star/StarSettings.cs)** : Assets/GameDesign/Player

### FightStage
- **[FightStageSettings](../Assets/Scripts/Game/Stage/StateMachine/States/FightStage/FightStageSettings.cs)** : Assets/GameDesign/Stages/Fight

### ResourcesStage
- **[ResourcesStageSettings](../Assets/Scripts/Game/Stage/StateMachine/States/ResourcesStage/ResourcesStageSettings.cs)** : Assets/GameDesign/Stages/Resources

### Mineral
- **Mineral** (Prefab) : Assets/Gameplay/Elements/Mineral/Prefabs

### Enemies

- **[Overheater](../Assets/Scripts/Enemies/Overheater/Overheater.cs)** (Prefab) : Assets/Gameplay/Enemies/Overheater/Prefabs/Overheater.prefab
  

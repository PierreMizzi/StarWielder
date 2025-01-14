# Changelog

## Version 0.7.0 - 07/01/2025

### Additions

- Minerals on Asteroids can now be smelted using the Sun's energy
  - Smelted minerals creates nuggets that the player can collect.
  - Nuggets are destroyed by the Sun
- Twin Stars orbiting on each other can now spawn, they provide a small amount of energy during the AsteroidStorm
- Added a ShopStage
  - Spend coins at the EnergyStation to give energy to your sun

### Changes

- Reworked Overheater to be more easily understandable and less punishing 
- Sun can absorb energy while being docked
- Changed how stages are orderer and tuned
- Upgraded Game Engine


### Bug **Fix**


## Version 0.6.0

### Additions

- Added currency :
  - Looted when absorbing energy from enemy's star, they can be used to purchase upgrades and modifications.
- A Game is now divided into stages :
  - Fighting stage 
    - Fights enemies to survive and earn currencies
  - Resources stage
    - A storm of asteroids is coming. Dodge them or mine them !
    - Some asteroids holds flowers. Bloom it using the Sun to receive a small chunk of health
- New enemies :
  - Overheater : Locks in the Sun and drains its energy. After some time it overheats and is destroyed. Spawn small mines that gives energy to the Sun, but damages the ship.

### Changes

- New Combo System
  - Whenever you kill an enemy, your combo increment by 1. Combo score influences the amount of currency you earn. Combo score resets when the emergency energy of the ship is fully depleated or regenerated. 
- Changed the "playing" space of the game. There is now a unreachable space at the bottom. Impacts spawning of enemies
- Ship can grab the Sun without recalling him
- In-game UI has been adjusted

### Bug Fix

- Ship's movement is now frame rate independant

## Version 0.5.0

First release
# Changelog 

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
  - Shop stage
    - Improve your current ship statistics in exchange of some currency
    - Buy modules to add strong side-effects to your Ship and Sun
- New enemies :
  - Overheater : Locks in the Sun and drains its energy. After some time it overheats and is destroyed. Spawn small mines that gives energy to the Sun, but damages the ship.
  - public void MyFunction(){}

### Changes

- Changed the "playing" space of the game. There is now a unreachable space at the bottom. Impacts spawning of enemies
- Ship can grab the Sun without recalling him
- In-game UI has been adjusted

### Bug Fix

- Ship's movement is now frame rate independant

## Version 0.5.0

First release
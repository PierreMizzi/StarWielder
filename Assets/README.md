# QGAmes Test - Developer's notes

### Goal
Your ship consumes the Star's energy to power it. Without it, your ship will stop functionning and you'll be vulnerable to enemies firing at you. Collect energy by launching your Star into enemy ships's stars. Recall the Star at you to draw energy from it. 
The goal is to survive the longest.

---

### Controls 

- **ZQSD** : Move Ship
- **Right Mouse Click** : Launch / Recall the Star
- **Space** : Dash

---

### Code Walkthrough

Naturaly, my most important classes in my code is ``Ship.cs`` and ``Star.cs`` so I would suggest you starting from there. They're both relying on ``StateMachine`` design pattern, so their behaviour is quite similar in a way. Since they heavily rely on each other, they have a direct reference to one and another. The different scripts linked to the ``HUD`` listens to events from ``PlayerChannel.cs`` and displays the data. This event system is based on what I call ``Channels``, which is a key aspect of my code architecture. 

``Channels`` are ``ScriptableObjects`` filled with Actions and Delegate so my "distant" game elements can communicate with each other. This way I easily decoupled model and view from my Ship/Star and my UI.

Once you've understand the general idea behind ``Ship.cs`` and ``Star.cs``, I suggest looking up at ``EnemyManager.cs``. This whole enemy part of my code is very top-bottom : My ``EnemyManager`` picks an ``EnemySpawner``, who's going to spawn an ``EnemyGroup``, that's going to activate its ``EnemyTurrest`` etc. 

Since my game is "Die and Retry", I added great details to my ``GameOverScreen.cs`` and it's general behaviour, so I suggest looking it up as well. 

**Note :** You may recognise I heavily use regions in my code, and instead of listing all my variables and members at the top of the class, I put them in their respective region. I know it's unsual but I find it very useful for different reasons.

- Improved class organisation : Everything is in a nice little drawer, it's neat. Paired with VS Code's shortcuts it's really fast.
- Easy refactoring : I can refactor a feature or move a block of code in a different script very easily.
- Class Readibility : Take a quick glance at all the folded regions and you can understand instantly what's his purpose


---

### Personal notes

So I know my game is very basic, with static enemies and simple behaviours, but I really wanted to produce something polished and addictive. Instead of over-engineering things and developing complicated features, I chose a simple gameplay and focused on little details on UI, visual effects and sounds. For example my game could have used a pooling system for all my game entities, but I decided to focus my effort on a polished product instead. This decision comes from my personnal experiences with game-jams ; Execution is more important than conception.

P.S. : There is a pooling system for all my SFXs

---

### Potential improvement

- A proper pooling system
- Different type of enemies
- It's almost impossible to lose the game by consuming all the energy from the Star, which could be a cool way of loosing.
- Enemies move around and aligns themselves to each other to become stronger. They align at precise angles, and when you launch the Star at the right angle, you can destroy multiple enemies at once. They create like a little circuit for the Star, which bounces from one enemy to another
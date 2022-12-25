Mod for the game Farthest Frontier that will limit the retreat behavior of hunters.  This mod was specifically designed with the idea of allowing hunters to engage boars without constantly retreating.  

The implementation simply prevents a hunter from retreating if their health is above 30% (configurable).  If health goes below the threshold, the game's built-in retreat logic will apply.

The threshold amount can be in `Farthest Frontier\UserData\HunterRetreat.cfg` after the game runs once.
* A value of 0% would make hunters never retreat under any circumstances.
* A value of 100% means the game's built-in retreat logic will always apply.

Tested with:  
MelonLoader 0.5.7  
Farthest Frontier 0.8.0

## Installation Instructions:
* Download and install [MelonLoader](https://github.com/LavaGang/MelonLoader).﻿﻿
* Download the HunterRepair zip file from Nexus Mods.
* Unzip the HunterRepair.dll file to your `Farthest Frontier\Mods` folder.

## The Legend of Zelda: Link's Awakening DX HD: Basic Manual

### Introduction

This document is to serve as a manual for the fork of **The Legend of Zelda: Link's Awakening DX HD** PC Port by Bighead ([BigheadSMZ](https://github.com/BigheadSMZ) on GitHub). The goal of this project is to try to get this port to behave as close to the original game as possible, while also providing additional enhancements that wouldn't be possible in the original game's engine. There is no strict ruleset that is followed when maintaining the balance between "original" vs. "enhancement", it mostly comes down to these two questions: Does it match the original game? Does it make the game better? If a change doesn't match the original game, but does make it better, then often the change will be accepted unless it is too dramatic of a change.

### File Selection

Up to four saves can be created at once. When highlighting a save, selecting it will either start a new game or load it. To the right of the menu are options to delete or copy the file to another slot. When selecting a new game, a new page will open to enter a name. There are a few easter eggs available when entering a name and selecting "Start Game".

* **Totaka/Totakeke/MOYSE:** Entering any of these three names will play a remix of "Totaka's Song".
* **Zelda:** Entering this name will play a remix of the original "The Legend of Zelda" theme.
* **DebugMode:** Start with all items in the game and have permanent invincibility.

### Settings

This port has a plethora of options to modify the gameplay experience that are found in the **Settings** menu. If all options are left at their default settings, then you can expect the closest possible experience to the original game plus the enhancements this port brings such as: free flowing camera, the ability to cut multiple bushes and grass with a single swing, the ability to modify the scaling on the fly, among other enhancements. This section will briefly go over what each option does.

#### Game Settings:

These settings affect the gameplay experience. 

* **Language:** The language of the game's text. Currently supports English, German, Spanish, French, Italian, Portuguese, and Russian.
* **Language: <setting>**: This is a sub-option that can change the dialog lines to original text, remake text, or custom text. Currently only the German language file has anything other than "Original".
* **GamePad:** The button layouts used in the game. Supports Xbox, Playstation, and Nintendo layouts.
* **Swap Confirm & Cancel:** Swaps the bottom and rightmost face button actions.
* **Autosave:** Automatically saves the game when entering doors/passageways and when quitting or exiting the game.
* **Item Slots on Right:** Moves the item HUD overlay to the right side of the screen.
* **Damage Screen Shake:** Briefly shakes the screen when Link takes damage.
* **Camera Lock:** Locks the camera to Link. When disabled, the camera can be moved with the right analog stick.

#### Audio Settings:

These settings affect the game's audio.

* **Music Volume:** The volume of the music. Pretty self explanatory.
* **Sound Volume:** The volume of all sound effects. Also pretty self explanatory.
* **Mute Inactive Window:** When the game is not focused, music and sound effects are muted.
* **Low Hearts Alarm:** Enables the constant beeping sound when Link's health is very low.
* **No PowerUp Music:** Disables the music that is played when collecting a Guardian Acorn or Piece of Power.

#### Video Settings:

These settings affect the game's graphics.

* **Game Scale:** The size/scale of the rendered screen. Can also be cycled with the back trigger buttons.
* **UI Scale:** Forces the size of user interface elements. 
* **Fullscreen:** Toggles fullscreen mode on/off.
* **Dynamic Shadows:** Calculates shadows automatically from sprites. When disabled, enables "Sprite Shadows".
* **Vertical Sync:** Locks the game's framerate to the refresh rate of the display.
* **Smooth Camera:** Smooths out the camera movement. Only really noticeable on large/quick changes.
* **Epilepsy Safe:** Disables or lessens several of the game's flashing effects.

#### Redux Settings:

Originally based on the "Redux" romhack for the original game, these settings offer gameplay enhancements.

* **Menu Border:** Changes the main menu border. Choices are LADX, original (from v1.0.0), and a black border.
* **Variable Width Font:** Changes the font to one where font glyphs have variable width instead of fixed width.
* **Disable Helper Messages:** Disables dialog that offered hints when the player did not have the correct item such as when touching a rock it mentions it's really heavy, when touching a cracked rock it's fragile, or touching ice it being really cold. Also disables most dialog boxes for common items such as acorn, piece of power, small keys, map, compass, rupees, and more.
* **Enable Dialog Skip:** Allows skipping any dialog with the "Start" button when enabled.
* **Disable Censorship:** Disables USA censorship: The hippo in animal village having breasts, and the mermaid losing her pink bra.
* **Nothing is Missable:** This option makes it so nothing in the game can be missed. Seashell mansion will give 5/10 shells at any point (as long as the level 2 sword isn't collected), Kanalet Castle has a new button upstairs to close the front gate, Marin/Flying Rooster will follow Link after Level 8 is completed, Link can buy back his name from the shopkeeper if he steals an item (for a steep cost), and the secret ending will always show up with any number of deaths.
* **Colored Photographs:** Photographs appear in full color instead of Sepia like the original games.
* **No Animal Damage:** When this is enabled, chickens and dogs can not be attack.
 
#### Modifier Settings:

Modifiers alter the feel of combat and can make the game easier/harder.

* **Extra Enemy HP:** Adds a fixed number of hit points to all monsters in the game.
* **Damage Taken Multiplier:** Multiplies the damage Link takes by the specified factor.
* **Added Move Speed:** Increases Link's movement speed by the specified amount.
* **No Damage Launch:** Disables launching enemies when Piece of Power is active or the Red Tunic is acquired.
* **No Heart Drops:** Completely disables enemies or environmental objects from dropping hearts.
* **Sword Blocking:** Makes it possible to block projectiles by slicing them with the sword.

### Intentional Changes:

This section covers some of the changes made from the original game that are intentionally different. Due to how similar this port may seem to the original game in many ways, it can be easy to mistake it for an actual PC port of the original game with a bunch of enhancements made. But this port is a complete recreation and does not use any of the original game's source code. This port is written in "MonoGame" which is the successor to "XNA Framework", which is the same engine used to make games such as Axiom Verge, Bastion, Bleed, Celeste, Fez, Rogue Legacy, Stardew Valley, and many others.

The goal of this fork of the port is to get the game as close to the original game as possible, which is a never-ending on-going process. With that said, some changes are intentional. So before making an issue report that something is off, check this list first to see if what you think is an "issue" might actually be an intended "enhancement". Do note there may be other intentional changes that are not on this list.

* **Enhanced Camera:** This one is obvious as this port allows you to see the entire world at once, pan the camera around it, zoom in and out, etc. This is obviously an intentional change and the main draw of playing this port over the original game. 
* **Enhanced Collision:** The original games were tile based, with each tile being 16x16 pixels in size. This means the majority of all objects size and collision are 16x16 pixels. In this port, the collision is designed so that objects appear more 3-dimensional. Collision boxes match what a sprite "looks like" as opposed to being on a fixed grid. For example, Link can stand (behind) a house, a tree, a sign, or a fence. Hitboxes match the enemy sprite sizes instead of being fixed 16x16. All collision was designed to give a more realistic feeling to all the objects in the world.
* **Cutting Bushes/Grass:** The sword in this port cuts any bushes and grass in its path. In the original game, the sword would only cut the bush or grass that is directly in front of Link.
* **Sword Hitbox:** The sword in this port has its own hitbox. Meaning, the sword only hits what it actually comes in contact with. This is a departure from the original games that hit the relative tiles the sword comes in contact with. One large change that can be seen with this is when dashing with Pegasus Boots. When running into monsters, they will only take damage if they come in contact with the sword. In the original games, a dash would always hit the tile in front of Link even if the sword missed the enemy.
* **Pegasus Boots Control:** There is an additional level of control to Pegasus Boots in this version. The player can adjust the running angle slightly, while in the original Link could only run in a straight line. It is possible to replicate the original behavior of turning/charging a run by letting go of the run button and pressing it again along with a new direction.
* **More Bushes by Richard's Maze:** North of the bush/hole maze to obtain the Level 3 Slime Key is an additional row of bushes blocking the entrance into the maze. This is an intentional change since it wasn't possible to enter the maze from this direction in the original game because of the holes at the edge of the screen. The free camera makes this same barrier impossible.
* **Horse Heads Don't Reset:** Some dungeons have a "puzzle" where you throw two horseheads and try to get them to stand up. They seem to travel in an L shape, like the knight in chess. It's a 50/50 chance whether they land on their base or land on their side. In the original games, after throwing the second horse head if both didn't land on their base, they would both fall over again. In this port, once a horse lands on its base, it will stay that way unless the player picks it up again. This so called "puzzle" is just luck, and resetting the horse is a waste of time for no good reason. 



# **The Legend of Zelda: Link's Awakening DX HD Changelog:**

## **v1.4.5**
### ** The "Even More Maintenance" Update**
  * More Classic Camera fixes: Keep more objects "alive" offscreen, fix navigating the egg, disable during the ending.
  * Add "lahdmod" support for the textbox and the HUD. These can be used to customize the size and positions.
  * Start to skip dialog boxes won't skip options. Options can only be selected with the confirm button.
  * Implement the Flying Roosters jump that he does when Link jumps or jumps off walls.
  * Objects that have instant pickup such as Flying Rooster, Spiny Beetle, Genie Lamp, Balls, etc. are now actually instantly.
  * Fix jumping > landing > walking appearing as if Link is sliding across the ground briefly.
  * Fix enemy "Spark" damagebox to match original game.
  * Fix enemy "Stalfos" to be able to be damaged while it is jumping.
  * Fix Dungeon 5 dive entrance to boss key to only a single tile.
  * Fix a missing tile on Dungeon 5 map.
  * Fix two missing stairs objects on Dungeon 7 maps.
  * Fix Dungeon 7 room where horse heads can be tossed outside the room.

## **v1.4.4**
### **The "Mandatory Maintenance After a Big Update" Update**
  * Great Fairy music is now based on the current field like the original game rather than range based.
  * Update stairway in Dungeon 4 from the underwater key room to exit to the left instead of the right.
  * Update stairway in Dungeon 5 near Gohma midboss to exit to the left instead of the right.
  * Update teleport transition in Dungeon 6 while Classic Camera is enabled to be quicker and have a scrolling effect.
  * Slightly increase the Ocarina listener rectangle for objects that require playing the ocarina.
  * Vacuum enemies can no longer be killed with sword shots and fix a bug where direction can get stuck if killing while vacuuming.
  * Fix crash with Marin-Walrus sequence and slightly adjust the parameters so it works with Classic Camera.
  * Fix crash when returning the ghost to his grave. This is the same crash as with Marin and I have no idea why it started happening.
  * Fix BowWow from getting stuck in a previous field when Classic Camera is enabled.
  * Fix several maps that did not spawn BowWow. Also fix it so a "ghost BowWow" does not spawn.
  * Fix the water under lilypads in Goponga Swamp to be animated.
  * Fix ball playing children music change to change back when changing fields when Classic Camera is enabled.
  * Fix push block on the way to the feather in Dungeon 1 to have the correct push directions (was stuck towards door).
  * Fix Lower Face Shrine mural sequence to not close out the image prematurely allowing the dialog to finish.
  * Fix misplaced collider on Tal Tal Mountain Range just under the bird key cave near the triple waterfalls.
  * Fix Dungeon 7 pillars so that they can be hit from behind with the ball.
  * Fix Dungeon 7 boss Evil Eagle issues: classic camera, freezing player during intro, attack locking, and reduce fly away time.
  * Fix mountain photo bridge sequence to work with Classic Camera mode.

## **v1.4.3**
### **The "Nostalgic Screen-Effect" Update**
  * Scaling has been reworked to scale based on the resolution of the original game. Game scale now goes to 20x.
  * Implement a "Classic Camera" that aims to replicate the screen-scrolling based system of the original game.
  * Add a classic camera option "Dungeon Only" which only applies it when inside of dungeons.
  * Implemented a "graying out" system for currently invalid options. For example, some options don't work with classic camera (and vice versa).
  * Movement has been modified to be more like the original game. "Classic Movement" option only restores the ability to "moonwalk" (walk backwards).
  * Jump height is now variable when holding the button down longer when on 2D maps.
  * When charging + jumping, a brief "standing" animation is played in between so Link doesn't appear to "slide".
  * Boomerang can now be thrown in any direction at any point. It now relies on controller direction instead of Link's current direction.
  * Dungeon 7 tower state is now reflected in the overworld. When all pillars are destroyed, it appears shorter.
  * Falling down a hole in the bird key cave now leads back to the waterfall below it on the overworld.
  * Marin's singing is now based on field rectangle instead of distance. This is more accurate to the original game.
  * Freeze the game while teleporting after playing Manbo's song. Game resumes when teleportation ends.
  * Instruments can no longer be collected with items like the Boomerang and Hookshot.
  * When adjusting scale with the controller, it should now save when quitting. Previously it only saved when adjusting from menus.
  * Zombies no longer spawn when the game is currently frozen such as playing ocarina or when owl is speaking.
  * Remove Hardhat beetle initialize state which did nothing. This affected classic camera where it would just stand there for a bit.
  * Fix playing the jumping "flip" animation when cancelling a sword charge while in air.
  * Fix the "falling" animation while in 2D mode. It should play a flip even when not jumping.
  * Fix diving and then walking to land causing permanent invincibility until walking into the water again.
  * Fix another animation quirk when attacking then jumping as the animation finishes which would cause Link to stand in air.
  * Fix boomerang and hookshot when collecting fairies to not sometimes blast them into outer space.
  * Fix a black spot on the overworld where there should have been ocean on Tal Tal Mountain Range.
  * Fix partial tiles popping up in the edges of some caves.
  * Fix playing ocarina + quitting to cause 8 seconds of invinciblity when game resumes.
  * Fix the white fade out effect when using a dungeon teleporter after playing Manbo's song.
  * Fix dungeon minimap to properly scale with UI scaling. I missed this when fixing main map and inventory.
  * Fix instance in Dungeon 1 and Dungeon 6 where Link should be able to squeeze through but couldn't in this port.
  * Fix Dungeon 2 and Dungeon 6 minimap chest locations.
  * Fix missing tiles in Dungeon 5 map. 
  * Fix Dungeon 5 crystal structure hitboxes so they don't interrupt sword and so they can be smashed from more angles.
  * Fix Dungeon 5 Gohma miniboss to behave accurately to the original games when playing the ocarina.
  * Fix Dungeon 6 instrument room door closing behind the player after the instrument has been collected.
  * Fix falling through certain flying tiles in Dungeon 6 that are covering up holes.
  * Fix a door in dungeon 7 floor 1 blocked by a barrier to actually stop Link from crossing it when it's up.
  * Fix Dungeon 7 room with horse heads so that they can not leave the room. Previously they could then they would bug out.

## **v1.4.2**
### **"On the Road to Glory" Update**
  * Note: Classic Camera has not made it into this release as it still needs more work.
  * Implement Indonesia language + textures. Thanks to [PangranggaCLAMP](https://github.com/PangranggaCLAMP) (creator).
  * Exclusive Fullscreen mode has finally been fixed! There is a slight delay entering it when booting the game with it enabled.
  * Huge map overhaul: add all the missing details of "upper levels" instead of "black" nothingness.
  * Initial window size has been modified to be a 6x integer scale of the original game's resolution (+widescreen).
  * Add more sword beam options to "ObjLink.lahdmod" file. Needs to be redownloaded from "Discussions" page.
  * Update several maps so that their fields line up with with the "screens" of the original games.
  * Make all photo sequences time out instead of waiting for a button press to prevent accidental skips.
  * For other image events (Christine photo, Face Shrine mural, alligator painting) add a delay before button exits.
  * Rework the Level 2 sword beam to behave as it did in the original games + extend travel distance.
  * Modify holes again: more accurate offsets, removed respawn hack, updated alternate respawn points.
  * Add a new dedicated drowning animation and rework drowning to be frame based instead of timed.
  * Move some of the settings around on the menus and create a new "Camera" settings menu.
  * If trading the fisherman for the necklace and leaving the scren before grabbing it, respawn it on next entry.
  * Add new animations when charging the sword while jumping and fix the transitions of animations between states.
  * Fix inventory and map overlays to correctly scale when changing the UI scale.
  * Fix more enemies to not deal damage or bounce when they are either burning or dying. 
  * Fix being able to pick up the Flying Rooster while he is bones and instantly resurrecting him.
  * Fix jumping out of the water when the movement speed modifier is 70% or greater when in 2D mode.
  * Fix jumping out of the water to not play the jumping sound effect which lead to it quickly playing twice.
  * Fix changing the attack direction while the button is held when in 2D mode.
  * Fix taking continuous damage when playing the ocarina while simultaneously colliding with an enemy.
  * Fix the Horse Head puzzle pieces from being able to be thrown through a nearby wall.
  * Fix Bow-Wow to be able to eat the monkey on the beach and eat the Winged Octoroks without them jumping.
  * Fix a Level 7 barrier from disappearing when pushing a block near it close to the top-right pillar.
  * Fix the Level 8 hacks that set the position to the front door so it doesn't force the player to leave the dungeon.
  * Fix the hookshot from being interrupted when moving left or right when in 2D mode.
  * Fix missing or misplaced chest icons on dungeon maps. This affects Level 2, 3, 5, and 8.
  * Fix Water Tektites with new collision type so they can not travel outside of the water and onto land.
  * Fix the sound effect that is played when picking up chickens to play the correct sound.
  * Fix drowning in water to not deal damage. Drowning in lava still deals damage however.
  * Fix the "Save & Continue" button from activating the boots when on the confirm button.
  * Fix a small error in the Pineapple sprite on the font texture.

## **v1.4.1**
### **A Maintenance Update**
  * Update the new hole code so that the bottom bias does not affect anything other than Link.
  * Move the new "NPC" type collision to the bottom of the enum list. This fixes a bug with the Photo Mouse.
  * Fix a bug that caused the NPC avoidance code to trigger even when an NPC wasn't spawned in.
  * Add support for a bunch more "lighting" lahdmod files so a new "no lighting" modpack can be created.

## **v1.4.0**
### **The "Sound is a 'Hole' Lot Better" Update**
  * Update holes so they act closer to the original game. The pull "size" is now accurate down to the pixel.
  * During the intro sequence, play Marin's missing animations. This is the only place in the game she has them.
  * When charging the sword and approaching a NPC, cancel the charge until not facing them (original game behavior).
  * Add support for Link mod file "ObjLink.lahdmod" file which can configure various settings for Link.
  * Add support for enemy mod file "ObjLives.lahdmod" file which can configure HP for individual enemies.
  * Update mod file "ObjDungeonBlacker.lahdmod" to include all maps where the lighting can be modified.
  * Don't play the "menu back" sound when exiting the name entry page via "special name" entry that plays music.
  * Add flashing effect to the sword shot like in the original games. This is the same flashing effect as when charging.
  * Fix various popping and distortion found in various sound effects: 77 sound effects fixed in total.
  * Fix various terrain differences around Kanalet Castle and Ukuku Prairie. The map almost appeared "unfinished".
  * Fix the flowers near Goponga Swamp to have their animation. Also add unused flower as an easter egg (somewhere).
  * Fix the animation speed for all flowers in the game. While some had the proper speed, others were over double.
  * Fix Marin wall clipping issues when she is a follower by reducing her body box size to match Link's.
  * Fix Marin cliff jumping sound effect to use the proper sound when she is a follower.
  * Fix missing collision near the cave to the right of the egg allowing the player to walk into the sky.

## **v1.3.9**
### **The "Old is New" Update**
  * Modify sword hitbox so it hits enemies at any "height" like the original games.
  * Add "Classic Movement" option which makes the movement feel much closer to the original games.
  * Implement more sound effects for menus: slider uses "selection sound", back uses "menu back" sound.
  * Support fairy mod file "ObjDungeonFairy.lahdmod". Enables sword collection/heal amount.
  * Dialog box text scrolling can be proceeded by using either the confirm or cancel buttons.
  * Movement speed modifier now affects more movement actions. This is probably still incomplete.
  * Fix sword to be able to grab items that end up on higher cliffs which was possible in original games.
  * Fix sword flashing when starting a game and using pegasus boots before swinging the sword.
  * Fix Marin to have her animated "idle" state when facing forward which has been missing since the beginning.
  * Fix Marin following Link through overworld teleporters. Not sure when this broke but it's fixed now.
  * Fix Marin from double spawning when taking a walk at the end of the game and sending her back.
  * Fix lifting the pot in 2D mode in the second dungeon which is required to proceed. Epic fail.
  * Fix facing direction when charging the sword in 2D mode. Like the above, this was due to 2D direction hack.
  * Fix missing title screen if pressing start when the intro fades to white. This has existed since v1.1.0.
  * Fix the landing sound effect not playing when jumping on cracked floor tiles (dare be invisible holes).
  * Fix in-game Settings menu from closing when pressing "left" on dpad or analog.
  * Fix Yes/No menu when choosing Quit/Exit game to not have false up/down selections (don't play a sound).
  * Fix audio distortion or a "popping" sound at the beginning of the "menu back" sound effect.
  * Fix projectiles to collide with the "just a kid" NPCs found around Mabe Village.

## **v1.3.8**
### **Holes are not Cheat Codes Update**
  * Fix a bug where falling down a hole could make the player invincible, including overworld portals.

## **v1.3.7**
### **Vanilla Fixes & The Skip Dialog Update**
  * Allow skipping dialog with Redux option "Enable Dialog Skip". When enabled, press "Start" to skip a dialog box.
  * When sprinkling magic powder on Tarin in raccoon form, prefer the powder item over interaction if they share "confirm" button.
  * Support custom lighting of maps through a mod file named "ObjDungeonBlacker.lahdmod" when placed in "Data\Mods" folder.
  * Followers will no longer prevent Link from trying to go up to the egg before Level 8 is finished.
  * Fix being able to force through to the egg when followers attempt to prevent Link from approaching it.
  * Fix Link trying to use the Shield or Pegasus boots if equipped on the "confirm" button and interacting with an object.
  * Fix spawning inside some doors when falling down a hole and the reset point was inside the door.
  * Fix a room in Dungeon 8 that should have been a dark room with torches but was fully lit up.

## **v1.3.6**
### **The Perfect Shield Update**
  * Add updated Spanish translations by [IPeluchito](https://github.com/IPeluchito).
  * Better command line support for loading save files: use the command "loadSave #" where # is 0-3 (for slots 1-4).
  * Rework the shield one more time. Bigger hitbox, direction never fails, no more false hits, and no more false blocks.
  * Small update to the in-game menu. Some options were shortened. Saving only happens when autosave is enabled.
  * Save file version has been incremented to version 2 due to world teleporter fix and v1 save files are set to v2.
  * Fix a game ruining bug where if saving and quitting while the world was "frozen" would permanently freeze it.
  * Fix world teleporter IDs which fixes the teleport order: Mabe village > Level 8 > Level 4 > Animal Village.
  * Fix potential softlock on some devices where sound could fail and the instrument sequence would not proceed.
  * Fix Richard's dialog to restart when denying his quest instead of always acting as if you accepted it.
  * Fix the hint Ulrira gives you about finding bananas to not get stuck if talking to the monkey before Richard.
  * Fix any languages that displayed the monkey's dialog incorrectly (some would exceed the bounds of the dialog box).
  * Fix loading the wrong photobook sprites for certain languages that currently support alternate sprites.
  * Fix the saving icon (disk bottom right) to always appear whenever the game is saved or an autosave takes place.

## **v1.3.5**
### **The Controller Buttons Update**
  * Trendy Game button textures now reflect the current controller and the "Swap Confirm/Cancel" setting.
  * When trading for the Boomerang, the item button the Goriya asks to trade for now reflects the current controller.
  * NPC/book dialog windows that reference buttons now reflect the currently selected controller.
  * Increase the velocity of using Roc's Feather in 2D underwater maps while swimming and play the sound effect.
  * Play missing sound effect when sworded enemies attack Link and it's blocked with the shield.
  * Add Totaka's song easter egg at file select screen. Start a new game and enter: Totaka, Totakeke, or MOYSE. Case insensitive.
  * Add a new easter egg that plays a remix of "The Legend of Zelda" theme by entering the name: Zelda. Case insensitive.
  * When setting dogs or chickens on fire, remove their components so they can no longer be interacted with.
  * Fix a rare game breaking issue that could happen when taking damage that caused the entire screen to go black or blue.
  * Fix entering Level 8 backdoors to properly set the last position when saving to the dungeon entrance.
  * Fix underwater attack with level 2 sword in 2D maps to be able to shoot sword beam both up and down.
  * Fix multiple instances of an invalid font glyph scattered throughout the German translation around the (ü) character.

## **v1.3.4**
### **Emergency Crash-Fix Update**
  * Fix crash that happens when attacking Pokey in Yarna Desert.
  * Adjust shield hitbox so that it completely covers the body box.

## **v1.3.3**
### **German Language Added Update**
  * Implement German language + textures. Thanks to [MasterPhW](https://github.com/MasterPhW) (creator).
  * Created language textures for French language that covers inventory, shops, map icons, photos, and title screen.
  * Support loading alternate images when changing language setting (like German and Spanish) for all supported languages.
  * Completely rework shield mechanics: hitbox reworked, no damage if object collides with both shield + bodybox.
  * New option to swap the "Confirm" and "Cancel" buttons. This works with any controller, and only swaps the functionality.
  * When enemies are defeated, remove all components. This prevents situations like bumping them when dashing with shield.
  * Add language option to load alternate scripts of the same language. Supports Switch remake and alternate scripts.
  * Implement more placeholder tags for font glyphs that represent font sprites like trade icons, marin face, etc.
  * Increase size of hitbox and bodybox of Arm Mimic. This makes dashing through them in dream shrine more like the original games.
  * Greatly simplify the hack fix used for when Link jumps over dungeon barriers as they are entering the raised state.
  * Make is so Link can lift up chickens and throw them with the Power Bracelet since it could be done in the original games.
  * Fix map icons for Mabe Village Shop to have "shop" icon instead of "!?" and Raft Shop to have "shop icon" instead of "cave".
  * Fix followers to not appear in certain maps. This includes fishing pond, shell mansion, dungeons, and dream shrine.
  * Fix a dungeon barrier in Level 7 to not pop through when a block is pushed over top of it by removing it.
  * Fix vacuum enemies in Level 2 and Level 8 to cause Link to properly spin when they are sucking him towards them.
  * Fix misplaced sprite shadows that spawn by the Level 8 sub-entrances when Dynamic Shadows is enabled.
  * Fix Level 8 midbosses Hinox and Rolling Bone to respawn after they have been defeated whenever the map is reloaded.
  * Fix final boss Ganon form weapon from potentially dealing double damage if both sides hit at the same time.
  * Fix Boomerang and Hookshot to not grab items that are flying and fix a crash with Hookshot when grabbing a fairy.

## **v1.3.2**
### **A Bug Fixes Update**
  * When handing the witch the toadstool to make powder, reset the music when it speeds up, and reset again afterwards.
  * Modify the movements of the Hardhat Beetle to more closely resemble the original game versions.
  * Reset the "free" camera when qutting to main menu and/or selecting a save file so it doesn't persist through saves.
  * Restore ability to set animals on fire with Magic Powder and Fire Rod.
  * Add option to completely disable animal damage in "Redux" options.
  * Fix crash from Hookshot grabbing fairies and modify the Boomerang to grab fairies instead of instant collecting them.
  * Fix crash with French and Italian languages in places where an invalid apostrophe was used.
  
## **v1.3.1**
### **Emergency & Sprite Shadows Update**
  * Implement many more sprite shadows. At this point it should be nearly feature complete aside from some objects.
  * Rename the Graphics Settings "Shadows" option to "Dynamic Shadows" since disabling still creates sprite shadows.
  * Fix the level 2 boss Genie from going invincible after the first hit after breaking his bottle.
  * Fix the level 7 boss Evil Eagle intro to wait until the top of the tower is reached.
  * Fix the Trendy Game crane to close back up after dropping an item off.
  
## **v1.3.0**
### **The "Even MORE Features and Fixes" Update**
  * Implement free camera which can be moved around with the right stick. Press in the right stick to recenter on Link.
  * You can enable this "free camera" by entering the "Game Settings" and unchecking "Lock Camera".
  * Implement a "sprite shadow" for certain game objects when dynamic shadows are disabled. This will be expanded on later.
  * Sword spin attack now spins clockwise when facing right like in original games. Other directions go counter clockwise.
  * Added missing sound effects when opening/navigating the island map from the inventory screen.
  * The painting by Schule Donavitch (the alligator with the hippo in Animal Village) can now be viewed.
  * Further reduce flashing effects with Epilepsy Safe: Upgrade Bat, Anti-Fairy, Spark, Giant Bubble, and Rooster Spirit.
  * Add "Save & Continue" option to save and keep playing. Rename "Quit to Menu" to "Save & Quit to Menu".
  * Play proper boss music during the level 8 boss Hot Head, which was playing the miniboss music.
  * When taking Bow-Wow for a walk, disable the "push component" which is what makes the shield "bump" him.
  * Bosses can no longer be interacted with in any way when they are defeated and in the "dying" phase.
  * Winged Octoroks now have an attack cooldown like regular Octoroks. This cooldown is also refreshed after jumping.
  * Fix Manbo's song to warp to level 8 entrance when entering the dungeon through the top two entrances from warp point.
  * Fix Gohma resurrecting and going invincible if playing "Ballad of the Windfish" when it is in "dying" phase.

## **v1.2.9**
### **The "MARIN IS FINALLY FIXED" Update**
  * Marin will no longer despawn after talking to her on the beach if playing up to that point uninterrupted.
  * Sword no longer blocks projectiles but a "Modifier" has been added to somewhat restore the functionality.
  * The "Sword Blocking" modifier can block projectiles when swinging, but can no longer block just by holding/charging it.
  * Add missing sound effects to "Vire" enemy in level 8 when attacking and when destroying it's fireball.
  * Don't preserve the "Thief" state when deleting a save with it and creating a new file in it's place.
  * Fill missing hearts when collecting the Level 2 Sword at Seashell Mansion.
  * Fix Like Like not stealing your shield when it is equipped to the top controller buttons as opposed to the face button.
  * Fix running through Like Like with boots which "traps" the player but keeps running invisible leaving a trail of smoke.
  * Fix several "trade icon" mistakes in various languages (especially Russian and Spanish) along with other minor fixes.
  * Fix Boomerang to deal damage to enemies regardless of the player Z-position. This mostly affected the blocks in level 7.
  * Fix the shield sometimes not playing the sound effect when pressing the button to block with it.
  * Fix travelling through an unspawned staircase in level 8 that could put you in the corner of the map.
  * Fix the "dungeon clear" music sometimes resuming after picking up the instrument at the end of a dungeon.
  * Fix applying sound effects volume at game load and when moving the slider before a save file is selected.

## **v1.2.8**
### **The "Sorry Marin I Give up For Now" Update**
  * Revert back to the old method to attempt to spawn Marin at the beach as this created more problems than it solved.
  * I have to give up on the "Marin not spawning" issue for now. I don't have a clue as to why this is happening to some people.
  * Fix the "Sound Effects" volume slider that has been broken since v1.1.0. So basically, the whole time.
  * Move the owl 8 pixels to the left when first entering Mysterious Wood to match his position in the original game.
  * Play the "bump" sound effect when attacking Goponga Flowers without being strong enough to destroy them.
  * Octorok's now have a cooldown between shots so they don't sometimes fire off like a machine gun.
  * Potentially fix the shield sound effect when blocking from sometimes not playing.

## **v1.2.7**
### **The "Missing Marin" Test Update**
  * Add alternate method to spawn Marin after the beach scene. Please report if it works or doesn't work!
  * This version is being released quicker than usual to get it into the hands of anyone who can test if new Marin method works.
  * Add a "Game Setting" to greatly reduce the flashing of shop keeper revenge and the bat that upgrades powder/bombs/arrows.
  * Add the flapping wing sound to Ravens which are the birds you can find around Turtle Rock.
  * Fix a potential crash when shooting bomb arrows into the mouth of Dodongo Snakes.
  * Fix Spiny Beetle sometimes losing its hitbox and becoming invincible if lifting the object off its back while moving.

## **v1.2.6**
### **The "Neverending" Update:**
  * NOTE: If Marin doesn't spawn after the beach scene, try a swordspin and report to me if it makes her appear or not.
  * Reset powerup counter when powerup ends. Fixes-example: Get acorn; Kill 11 monsters; Lose it; Kill 1 enemy; Another acorn.
  * Set Kanalet Castle upstairs button state to "pressed" after finishing level 2 so it's in the proper state.
  * Revert Arm Mimics to their previous behavior. Following facing direction when charging was not correct.
  * Add two extra cliff jumps to Tal Tal Mountain range near the hen house for slightly easier travel.
  * Modified the Ghost photograph to better match the in-game colors before the photograph is taken.
  * Don't play the "landing" sound effect when jumping directly into an overworld teleporter hole.
  * When hookshotting Iron Mask and stealing it's mask, play the "collection" sound effect (same as fairy).
  * Playing Ballad of the Windfish when fighting Gohma opens it's eyes for 1.5 seconds and doesn't shoot fireballs.
  * Add enemy wall between North/South Face Shrines to try to keep Mushroom Bomber from interacting when at South Shrine path.
  * Don't store save file and content paths in the "settings" file. It was counterintuitive when moving between "portable.txt".
  
## **v1.2.5**
### **The "Even More Polish" Update:**
  * A 'potential' fix for Marin sometimes disappearing when she joins you after the beach scene.
  * Fix music priorities between villages, powerups, and Marin singing. A combination of these could get jumbled.
  * Fix hearts to have the proper sound effect and use the old effect for when a heart heals.
  * Fix King Moblin to have more accurate behavior to the original game.
  * Fix Bow-Wow to have the proper sound effect when munching on an enemy.
  * Fix Crows to have the "flapping wings" sound effect loop when attacking.
  * Fix Kanalet Castle buttons (again) to hopefully properly close and open the front gate.
  * Fix playing the landing sound any time Link is airborne and hits the ground.
  * Fix DethI dying in a single hit from any damage source (only the boomerang should do this).
  * When spawning level 2 sword, freeze the player a little longer to prevent a glitch grabbing sword too early.

## **v1.2.4**
### **The "Final Touches" Update:**
  * Fully implemented the "secret ending" that is shown after the credits with 0 deaths.
  * The secret ending can also be viewed if the Redux option "Nothing is Missable" is enabled.
  * Add Spanish textures for title screen, menus, objects, items, various others. Thanks to [IPeluchito](https://github.com/IPeluchito).
  * Photographs are now sepia by default. A "Redux" option has been added to use colored photos instead.
  * Implement a "Modifiers" page that can alter the difficulty of gameplay.
    * Extra Enemy HP: Adds up to 30 additional hit points to all enemies and bosses.
    * Damage Taken Multiplier: Increases damage taken from 1x up to 10x.
    * Added Move Speed: Increases movement speed from 10% up to 100%.
    * No Damage Launch: Piece of Power / Red Tunic do not launch enemies.
    * No Heart Drops: Disable hearts dropping from all sources (enemy, bushes, digging).
  * Force Link into "idle" state with 0 depth when encountering the owl so he's not frozen in weird poses.
  * Reduce final boss "Giant Zol" chance to despawn after a jump from 50% to 33%.
  * Fix Dungeon 1 room with 4 Keese so that they can not fly outside the bounds of the room and trap you in.
  * Fix potential issue with Madam MeowMeow not getting excited when Bow-Wow is kidnapped.
  * Fix Red Zol from losing its hitbox and not splitting if damage was somehow less than 1.
  * Fix holes near Kanalet Castle leading to seashell by adding pathing blockers to the tops of the trees.

## **v1.2.3**
### **The Last Boss Update:**
  * Fix final boss "Giant Zol" form for real this time. It was getting stuck in the elongated state.
  * Increase chance "Agahnim" state of final boss will shoot exploding shot from 25% to 33%.
  * Stop music after "Lanmola" state of the final boss. It's supposed to be silent for DethI transition.
  * Fix DethI to be able to be damaged from bombs (more specifically, bomb arrows which I did as a kid).
  * Disable the low hearts alarm if the final boss is defeated while at low health so its not beeping through the ending.

## **v1.2.2**
### **The "Fix Everything" Update:**
  * Disable Helper Messages also includes completing heart containers and opening rupee chests.
  * Menu border in Redux options has been turned into a slider with a black border option.
  * Add translated text to the boat in the fisherman falling in the water photo.
  * Play the "secret sound" when monkey's finish fixing the bridge near Kanalet Castle.
  * When meeting Marin at the beach, play a jingle and silence the music to where only the ocean is heard.
  * Playing Ballad of the Windfish opens Gohma's eyes (mid-boss of Level 5 dungeon).
  * Add Horse Heads and Ball in level 7 to the list of instant pickup items.
  * Potentially fix cave on path to level 7 that is always bombed open before ever even visting it.
  * Fix hookshot getting stuck when picking up a powerup at close range by using a reset countdown timer.
  * Fix playing the correct overworld music after taking the photo with Marin at the beach cliff.
  * Fix dungeon music cues: silence after boss kill, clear music after boss defeat.
  * Put extra effort into dungeons/bosses: Slime Eel, Facade, Evil Eagle, and Hot Head.
  * Fix the timing of gobble/explosion sound effects when feeding Dodongo Snakes bombs.
  * Fix missing pathing blocker over torch in dungeon 2 room with skeleton, switch, and up/down blocks.
  * Fix the textbox background from going missing and inventory transparency when hiding the UI.
  * Fix Color Dungeon boss Hardhit Beetle from spamming the message about resetting when blue color.
  * Fix Hardhit Beetle projectile to be on a lower layer than link so it doesn't overwrite his sprite.
  * Fix editor not saving the position of "moveStone" in dungeons (another dig hole map bug).
  * Fix the exits of several caves and passageways to push you in the correct direction.
  * Fix picking up an invisible rooster in level 7. Yes he's always with you until the level is finished...
  * Fix Link sleeping sprite drifting upward under the right conditions when jumping into Dream Shrine bed.
  * Fix 1.2.1 bug: Button would not open the door outside of Kanalet Castle.
  * Fix 1.2.1 bug: Madam MeowMeow has the wrong dialog when village is under attack.
  * Fix 1.2.1 bug: Alligator trading sequence was broken. He would not ask for bananas.

## **v1.2.1**
### **The "Break Everything" Update:**
  * Don't play this version. It has some game breaking bugs that prevent progression.
  * Add redux option "Nothing is Missable" option. This does the following:
    * Shopkeeper name buyback has been added to this option since it's non-canon.
    * Seashells 5/10 at seashell mansion can be obtained even if you pass the markers.
    * A button is added upstairs to Kanalet Castle to close the gate (for Richard photo).
    * The flying rooster can be borrowed from the Hen House after level 8 is completed.
    * Marin will take a walk with the player for another shot at photos after level 8 is completed.
  * Modify drop rates of rupees and hearts: 30% chance for drop, 70/30 it will be rupee/heart.
    * Old formula had a 33% chance to drop rupees, so only a slight nerf there.
    * Old formula had hearts at 40% but AFTER rupees, meaning only a 7% chance so a buff there.
  * Don't stop spawning seashells when reaching 20 shells allowing all 26 to be collected.
  * Only allow pushing a single object at a time with a bias towards the closer object.
  * The UI can be hidden/shown with either the "Tilde (~)" key or the "Delete" key.
  * Piece of Power and Guardian Acorn will no longer drop if a powerup is currently active.
  * The total number of kills the player has made is now tracked in the save file.
  * Remove 3D effect from small rocks so they don't overlap the sword and remove their shadows.
  * Massive overhaul of "scripts.zScript" to make it more readable and contiguous.
  * Fix "error" message appearing after the final photo taken and any repeat events (like stealing).
  * Fix dialog background size when selecting between VWF and standard font.
  * Fix text of "Auto-Scale" for graphics slider to have a space after the colon.
  * Fix Crazy Tracy keeping her mid-conversation state between entering/exiting her shop.
  * Fix missing secret sound after sprinkling raccoon with powder and walking towards tail key chest.
  * Fix potential extremely rare crash with moblin-swords under the right conditions.

## **v1.2.0**
### **The Redux Update:**
  * Fix final boss "Giant Zol" form to have the correct graphics.
  * Rename Miscellaneous options to "Redux" options on the Settings menu.
  * Add Redux option to use a variable width font as is used in the romhack.
  * Add Redux option to disable helper messages. This encompasses a large number of popups:
    * Items: Keys, Compass, Map, Nightmare Key, Stone Beak, Seashell, Piece of Power, Guardian Acorn, Golden Leaf, and Piece of Heart.
    * Objects: Stones/Pots, Crystals, Cracked Rocks, Keyhole Blocks, Dungeon Keyholes, and Ice Blocks.
  * Add Redux option to disable censored content. This includes the hippo and mermaid sprites/text.
  * Added Hippo missing original standing sprite and implement animation states.
  * Alternate font versions for any combination of Variable Width Font and Disable Censorship.
  * Shoplifting photo has been updated to have the translated text from Redux romhack.

## **v1.1.9**
### **The Endless Bug Fixes Update:**
  * Fix Marin to teach the Ocarina song after being saved on the mountain.
  * Fix wrong enemy types in cave near pig moblins; from Buzz Blobs to Spiny Beetles in skulls.
  * Fix shield animation when charging while blocking when in 2D mode.
  * Fix losing powerups when transitioning between floors in Castle with the golden leaves.
  * Fix Dream Shrine bed transition again: additional states were added but not checked for here.
  * Fix destroyable wall sword poke sound effect to sound more impactful and not so soft.
  * Fix being able to pick up rooster's dead bones before he was revived with the song.
  * Fix photo mouse having no interaction and collision during fisherman photo sequence.
  * Fix heart piece location in castle moat to be in it's proper position one tile down next to wall.
  * Fix Arm Mimic facing direction when charging using same fix as Mask Mimic (Shy Guys).
  * Fix Pokey (desert cactus enemy) dying in a single hit with a sword spin even at level 1 sword.
  * Fix Crazy Tracy being able to talk to you from across her table. 
  * Fix missing Zora south-center of overworld near tiny island with bush & seashell.
  * Objects on top of Spiny Beetles are now picked up instantly without a direction pushed.
  * Karakoro (the ball-type enemy in the color dungeon) is also now instantly picked up.
  * Rolling Bone spiked roller will not interact with sword while jumping so it won't repel.
  * Master Stalfos can be damaged with level 2 sword beam. Behavior is as close to GameBoy as possible.
  * Don't interrupt sword charge on sources that spark when hit (spike roller, traps, masked enemies).
  * Tweak main menus to have more usable space and make sure bottom labels are never overlapped.
  * Added a space after the colon for GamePad on Game Settings page to match the language button.
  * Controller buttons on inventory screen were moved one pixel upward.
  * Dev: Fix map editor's ability to delete objects. Was broken with shovel hole changes.

## **v1.1.8**
### **The Long-Standing Issues Fixed Update:**
  * Updated Spanish translation. Thanks to [IPeluchito](https://github.com/IPeluchito).
  * Low Heart Alarm was moved to the "Audio Settings" page.
  * When fishing, give the fish just a little bit more fight, especially the lunkers!
  * Fix arrows freezing the game when shot in some caves due to changes in hole logic in v1.1.1.
  * Fix shovel holes from being overwritten by flowers while also being covererable by stones.
  * Fix certain areas on the overworld to be diggable that previously were not.
  * Fix Tarin spawn states during the trade sequence before/after trading for bananas. 
  * Fix behavior of Winged Octoroks to not jump and be invincible against sword spin attack.
  * Fix setting the reset point when falling down holes if jumping while transitioning "rooms".
  * Fix disabling powerup music to not play powerup music when transitioning dungeon floors.
  * Fix shopkeeper name buyback. Rupee/item checks have been broken since it's implementation.
  * Make dungeon 2 boss Genie Bottle instant pickup without needing a direction pressed.

## **v1.1.7**
### **The Sword Fixes Update:**
  * Modify attack sword hitboxes. Frame 1: hitbox added, 2: hitbox expanded, 3: hitbox reduced.
  * Modify the hitbox of bushes to match their sprite/tile size. This fixes wonky "cutting" with sword.
  * Allow collecting items with the sword when holding it out in charging state.
  * Level 2 sword shot now matches height of Link and doesn't collide when standing on raised blocks.
  * Fix the hookshot where it would never fire again when trying to use it with other weapons.
  * Item collection box height can exceed its width or 8. Fixes collection range of items like rupees.
  * Add a hack-fix when hitting the switch to raise blocks while jumping over them and getting stuck.
  * Renamed FPS Lock to "Vertical Sync" as this is a far more accurate description of what it does.
  * Make UI scale slider static 1-11, 11=Auto-Detect, and only apply scale if screen can handle it.
  * New brick texture for main menu that better matches DX on GBC. Can swap to old texture.
  * New Miscellaneous page. Currently only contains option to swap to old menu brick texture.

## **v1.1.6**
### **A General Fixes Update:**
  * Expand size of most UI elements on settings pages to better fix non-english languages.
  * Translate "Controller" on controller selection button on Game Settings page.
  * Restore fisherman's line "You have to have more passion. Live a little." when exiting pond early.
  * Add option to "Audio Settings" to mute powerup music (Piece of Power/Guardian Acorn).
  * Remember scaling settings on window resize when not set to Auto-Detect.
  * Change "Controller" selection text to "GamePad" in all langauges.
  * Fix potential crash when using items immediately after save is loaded.

## **v1.1.5**
### **The Features Update:**
  * Add controller type selection to Game Settings menu: XBox, Nintendo, Playstation.
  * Make UI Scale slider "Auto-Detect" the rightmost value.
  * Freeze the game world while playing the ocarina.
  * Hookshot can be retracted with a second button press.
  * Translate "Back" and "Select" on the menus. Thanks to [xsm2](https://github.com/xsm2).
  * Translate "Thief" across all language files. Thanks to [xsm2](https://github.com/xsm2).

## **v1.1.4**
### **The Bugfix Update:**
  * Implement French language. Thanks to [JC](https://itch.io/profile/jc2111) (creator).
  * Patcher now backs up unpatched files so future patchers don't need to rely on v1.0.0.
  * Creating "portable.txt" next to game executable restores saving to game folder.
  * Fix sprite of hole dug with shovel overwriting rocks pushed over it.
  * When attacking and standing still, it's possible to change facing direction.
  * Shooting arrows left and right when next to a south wall no longer triggers collision.
  * Goomba can no longer be attacked after it has already been stomped.
  * Hardhat Beetle is affected by knockback effect of piece of power and red tunic.

## **v1.1.3**
### **The Languages Update:**
  * Implement Spanish Language. Thanks to [Álcam](https://www.youtube.com/@Alcam211) (creator), [IPeluchito](https://github.com/IPeluchito) (assistance), [orderorder](https://github.com/orderorder) (suggestions).
  * Implement Italian Language. Thanks to [Ryomare123](https://itch.io/profile/ryomare123) (creator), [Ryunam](https://github.com/Ryunam) (assistance).
  * Implement Russian language. Thanks to [Dima353](https://github.com/Dima353) (creator), plus font files.
  * Implement Portuguese language. Thanks to [altiereslima](https://github.com/altiereslima) (creator), plus font files.
  * Fixed muting audio when window is out of focus if option is enabled.
  * Shadow toggle in graphics options. Note that it still needs work as it removes all shadows.
  * Playtime is now tracked and visible on file selection. Thanks to [LouisSF](https://github.com/LouisSF).  
  
## **v1.1.2:**
### **An Emergency Update:**
 * Don't apply smaller collision scale to holes already smaller than 16x16.

## **v1.1.1:**
### **A Minor Update:**
 * Reimplement the option to unlock FPS.
 * Fix alligator can throw with unlocked FPS (credit @buttcheeks69).
 * Fix incorrectly calculated custom text height for confirm button (credit @squiddingme).
 * More accurate timing when picking up sword on beach (credit: @buttcheeks69).
 * Show key counter on HUD when in dungeons (credit: @squiddingme)
 * Custom sprite for key counter.
 * Add option to display items in the bottom right (credit: @squiddingme).
 * Pay the Shop Keeper to no longer be called "Thief" after stealing.
 * Reduced collision size / pull strength of holes to more closely match the original game.

## **v1.1.0:**
### **General:**
 * Saves and settings files are now located in "..\AppData\Local\Zelda_LA".
 * Intro and title screen reworked to better match original game.
 * Always show the title screen even when skipping intro video.
 * Fix crashing and many other issues when starting in "Fullscreen" with "Borderless" disabled.
 * Keyboard controls will now properly save.
 * Swimming sound effect when in deep water was added.
 * Low hearts will play the beeping sound.
 * Photographs are now colorized.
 * Save files reference hearts/health as "maxHearts/maxHealth" instead of "Hearth".
 * New version compatible with old save files but not vice versa.
 
 ### **User Interface:**
 * Swapped the Start and Select buttons from the original release.
 * Quit button on the main menu after the title screen.
 * Restored commented out Audio Settings page and move volume controls there.
 * Restored commented out UI scale code and added it to Graphics Settings page.
 * Added option to mute sound when window is not active in Audio Settings page.
 * Rename "Back to Game" button text to "Return to Game".
 * Added "Save & Quit" button on the in-game pause menu.
 * Game scale can be adjusted with LT and RT buttons.
 * Low hearts beep can be disabled from game settings menu.
 * Screen shake when taking damage can be disable from game settings menu.
 
 ### **Items:**
 * Items can be assigned to two additional buttons: LB and RB.
 * Inventory cursor sound was added.
 * Items can be used when pushing against objects.
 * Cooldown of the sword has been removed.
 * The sword can be charged while using the shield.
 * The sword can be charged while jumping off cliffs.
 * The sword can be charged while underwater in 2D.
 * The shield can be used while charging the sword.
 * The shield plays the proper sound effect when blocking.
 * Bombs will appear in the shop after buying the shovel.
 * Bomb-arrows now deal full damage to the initial target.
 * Pegasus Boots directional influence has been reworked.
 * Bracelets pick up Flying Rooster or Smasher Ball instantly.
 * Piece of Power and Guardian Acorn can be collected with sword.
 * Piece of Power and Guardian Acorn proper sound and music delay.
 * Piece of Power and Guardian Acorn will not lose music in some dungeons.
 * Knockback effect (piece of power) of sword lvl 2 removed.
 * Knockback effect (piece of power) of red tunic restored.
 
 ### **Enemies:**
 * Stunned enemies no longer deal damage.
 * Enemies dying from burning no longer deal damage.
 * Goponga Swamp flowers can be killed with Lvl 2 sword + spin attack/piece of power/red tunic.
 * Blade Traps will now collide with pushed blocks.
 * Green Zol damage delay while spawning.
 * Shy Guy facing direction fixed while charging sword.
 * Pols Voice can be defeated with the ocarina.
 * Flying Tile sound loop now more accurate to original.
 * All bosses have had their damage field deactivated on death.
 * Lanmola body (worm in desert) can now deal damage.
 * Stone Hinox (color dungeon) AI fixed from deadlocking into "jump" state.
 * Armos Knight (south of Face Shrine) can be hit with sword spin.
 * Final boss Aghanim's Shadow sound effect for bursting projectile.
 * Final boss Ganon's Shadow can be damaged with spin attack.
 * Final boss Lanmola can be attacked with spin attack.
 * Final boss DethI can be killed with boomerang.
 
 ### **Overworld:**
 * Mabe/Animal Village music always takes priority over piece of power/acorn music.
 * Trendy game timings are closer to original game.
 * Owl conversations now properly freeze all enemies until he flies away.
 * Marin will teach Ballad of the Wind Fish after acquiring Ocarina.
 * Fixed Marin's message when attacking chickens to not default to sadistic comment.
 * When Marin sings to the walrus it now matches the duration of the original game.
 * Fixed rare issue in Dream Shrine where sleeping sprite would float past the bed.
 * Randomized sound effect frequency at Seashell Mansion when counting shells.
 * Richard's Villa maze sequence can no longer be cheated by jumping the holes.
 * Richard's castle photograph event fixed.
 * The ghost sequence must be respected: house by the bay  grave stone.
 * The ghost must be returned before his photo and sprinkling powder on grave.
 * The ghost return sequence now freezes the animations of nearby monsters.
 * Fixed getting the ghost photograph before returning him to the grave.
 * Increased the "grab" distance of the raft so it can't be clipped through.
 * Sign post maze now starts at the correct sign.
 * Flying rooster can no longer skip the flame trap on the way to Turtle Rock.
 * Fixed dying when holding onto the flying rooster.
 
 ### **Dungeons:**
 * Softlock fixed when picking up instruments.
 * Increased interaction range of dungeon teleporters.
 * Tail Cave (dungeon 1) block near trap is now pushable.
 * Face Shrine (dungeon 6) now references the correct key.
 * Face Shrine (dungeon 6) doors can only be opened with statues.
 * Face Shrine (dungeon 6) fixed spot where it was possible to get stuck in a wall.
 * Eagle Tower (dungeon 7) pull lever now works with small analog left/right values.
 * Eagle Tower (dungeon 7) when falling in holes Z-axis is remembered.
 * Turtle Rock (dungeon 8) fixed locked door that took a key but did not open.
 * Turtle Rock (dungeon 8) breakable wall was fixed (it couldn't be broken).
 * Play the secret discovery chime when taking the correct path in the egg.



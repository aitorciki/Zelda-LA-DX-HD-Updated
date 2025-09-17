# **The Legend of Zelda: Link's Awakening DX HD Changelog:**

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



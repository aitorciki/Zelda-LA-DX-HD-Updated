# LADXHD Patcher
The LADXHD Patcher is used to upgrade the v1.0.0 release to the latest version. When it is finished patching the game, any files that were updated are copied to the game's `Data\Backup` folder. The patcher uses `.vcdiff` patches using the original v1.0.0 files and the files from the latest version. While this is not the most ideal way to distribute the latest version of the game, it is done this way to protect this project which does not contain any of the game's original copyrighted assets.

## Information
The patcher started out simple. The first iteration to patch the game from v1.0.0 to v1.1.0 only patched a dozen or so files. Since then, it now patches over 400 files and can patch the game to seven different versions of the game, including: Windows DirectX, Windows OpenGL, Linux x86-64, Linux Arm64, MacOS x86-64, MacOS Arm64, and Android. The APK should support multiple Android versions including Arm, Arm64, x86, and x86_64 (the latter two being for emulators).

## Using the Patcher
Patching the game is as simple as copying it to the game folder and updating the game to the user's selected choice. It should be copied alongside `Link's Awakening DX HD.exe` or without the `.exe` on both Linux and MacOS. Android needs to have a new APK generated for it every time it is updated and reinstalled on the device. When the game is done patching, it will create a `Backup` folder in the `Data` folder. Do not delete this folder or it will not be possible to patch it later, and a fresh copy of v1.0.0 will be required to patch to the latest version.

## Building the Patcher
If you are interested in this, then I have probably retired from the project or met my end somehow. Building the patcher is fairly straightforward these days, as the entire process of building the game, launchers, and patcher is streamlined. 

Building the Patcher requires having the [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed.

As long as everything is in the default folders of this repo, then one only needs to double click the `~Publish.bat` script found in the `ladxhd_game_source_code` folder. This will build the entire project from start to finish.

With that said, it would still be helpful to know what goes into the patcher. Lets start with the `Resources` folder.
- **android_base.apk** : This is the APK that was built by Visual Studio when publishing, minus the assets. The publishing scripts will automatically move the updated version here, replacing the old version of this file.
- **rcodesign.exe** : This is used to code sign the MacOS versions. The local `Name.pem` file is not included in the repo and would need to be generated manually by whoever is interested in creating a patcher.
- **SDL2.dll** : This is needed for the Windows OpenGL build. When patching is done, it is copied to the output folder.
- **Icon.*** : Various Icon files for different versions of the games.
- **d3map** :  This is `dungeon_3.map` renamed. It is copied to `Data\Maps` folder after patching. There is a long story of why this needs to be done, the short version is this is a brute force method to make sure it's correct.
- **la.png** :  The image file used on the patcher window.
- **Info.plist.template** :  Used to bundle the MacOS version into an `.app` bundle.
- **beep.wav** : The sound effect when showing "Ok" or "Yes/No" windows.
- **success.wav** : The sound effect when patching is complete.
- **7zip.zip** : Contains the 7zip executable. Required to create Android APK.
- **android_buttons.zip** : Contains the on-screen buttons for Android. Copied into `Data\Buttons` after patching.
- **android_tools** : Contains tools required to create an APK including java, apksigner.jar, keystore.jks, and zipalign.exe.
- **launcher_*** : The various versions of the Launcher. Publishing it automatically copies it to here.
- **macos_*_files** : Library files specific to the MacOS builds. Publishing the game automatically copies it to here.
- **patches_*** : The various versions of the game patches. Generated from the PowerShell script `CreatePatches.ps1`.

Any of the files in this folder that need to be updated every single version are automatically created here during the publishing process when started from `~Publish.bat` in the game folder `ladxhd_game_source_code`. They should never need to be manually updated or changed unless inherent changes to the patcher are made.













# The Legend of Zelda: Links Awakening DX HD - PC Port

**NOTE: THIS REPOSITORY DOES NOT INCLUDE COPYRIGHTED GAME ASSETS!**

A few years back, an anonymous user posted a PC Port of Link's Awakening on itch.io built with MonoGame. It wasn't long before the game was taken down, fortunately the release contained the source code. This is a continuation of that PC Port but with the assets stripped away to avoid copyright issues. To use this fork, whether to play the game or build upon, it requires the user to provide the assets from the original v1.0.0 release. Some assets have been updated, but I have created tooling to make migration easier.

See the [changelog](https://github.com/BigheadSMZ/Zelda-LA-DX-HD-Updated/blob/main/CHANGELOG.md) for a list of changes from v1.0.0.

As of v1.1.0, the game is in a really good state and the "feel" is really close to the original game (but with all the enhancements of a PC port). There is still more that could be done, but I am taking a break from it for now.

## Patching v1.0.0 to v1.1.0

This is probably what most people would be here for. To download the latest update, there is a patcher on the [Releases](https://github.com/BigheadSMZ/Zelda-LA-DX-HD-Updated/releases) page. If you wish to build the game yourself, see **Personal Build / Publishing**.
- Find the v1.0.0 release originally from itch.io.
- If you can not find it, you can search for an "archive" of it.
- Download the patcher from the releases page.
- Drop it into the same folder as the v1.0.0 game.
- Open the patcher. Press the "Patch" button.
- When it is done, the patcher can be deleted.

Note that antivirus programs like Windows Defender may flag this patcher as a virus. It is NOT a virus, and the source code can be found in this repository. I am working on a malware submission to Microsoft but the submission site is currently not working (as usual).

## About This Repository

This secion explains the files and folders found in the base of this respository.
- **assets_original**: This is where the **"Content"** and **"Data"** folders from v1.0.0 should go. See **Updating Source Code Assets** section.
- **assets_patches**: Contains xdelta3 patches that are the difference of assets from v1.0.0 to the latest updates. Instead of updating assets, patches should be created.
- **ladxhd_game_source_code**: This is the source code for The Legend of Zelda: Link's Awakening DX HD minus the assets required to build it.
- **ladxhd_migrate_source_code**: This is the source code for the migration tool which uses the xdelta patches to update 1.0.0 assets or create patches from new assets.
- **ladxhd_patcher_source_code**: This is the source code for the patcher tool which is used to update the base game of v1.0.0 to the most recent version.
- **LADXHD_Migrater.exe**: This is the program that can be used to migrate v1.0.0 assets found in "assets_original" to the latest versions or create new patches.
- **Unblock-All-Files.ps1**: Visual Studio complains about files downloaded from the web. This script can be used to unblock all files automatically.

## Updating Source Code Assets

The latest source code can be downloaded from this repository. But, you will need to provide the assets from the original v1.0.0 release. It is very important to follow the instructions carefully as some assets have been updated.
- You will notice there is a folder in the base of this repository named **"assets_original"**. This is where the **"Content"** and **"Data"** folders go from the original v1.0.0 release.
- Note that there is two versions of these folders, and you need to provide the correct ones.
- Assuming you have unzipped the original release, there should be two folders: **"Content"** and **"Data"**, an executable: **"Link's Awakening DX HD.exe"**, and a 7-Zip of the source: **"source.7z"**.
- Copy the **"Data"** folder from the original v1.0.0 game folder to the **"assets_original"** folder.
- Unzip the **"source.7z"** file from the original v1.0.0 release.
- Copy the **"Content"** folder from the original v1.0.0 source code folder to the **"assets_original"** folder.
- Open the **"LADXHD_Migrater.exe"** tool that is provided.
- Click the button **"Migrate Assets From v1.0.0"** and wait for it to finish.
- This will create new **"Content"** and **"Data"** folders in the **"ladxhd_game_source_code"** folder.
- And you are done. From here you can build the game or work on the code.
- The original **"Content/Data"** folders should be kept in **"assets_original"** so future patches can be created from new modifications.

Again, make sure you are grabbing the correct Content and Data folders. The "Data" folder should come from the <ins>game folder</ins>, and the "Content" folder should come from the <ins>source .7z file</ins>. While it is possible the original assets would work, there have been bugs fixed and issues addressed in some of them. The patches in **"assets_patches"** never need to be interacted with directly, as the migration tool can handle both directions: updating 1.0.0 assets, and creating new patches for asset updates.

## Contributing Prerequisites

If you wish to work on the code in this repository.
- Basic knowledge of C# .NET and Visual Studio.
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
    - Make sure to select `.NET desktop development` components in the visual studio installer.

## Contributing Assets

Do not make pull requests providing the assets directly, instead create xdelta patches using the **LADXHD Migrater** tool provided. These patches can then be applied to the original assets to update them.

See the wiki page on [contributing to this project](https://github.com/BigheadSMZ/Zelda-LA-DX-HD-Updated/wiki/Contributing-to-this-project) for more information.

## Build Instructions

If you wish to build the code in this repository.
- Clone or Download this repository: green `Code` Button > `Download ZIP`
- The game's source code is in **"ladxhd_game_source_code"** folder
- Follow the steps in **Updating Source Code Assets**
- Run the PowerShell script "Unblock-All-Files.ps1".
  - -OR- Go to the folder `ladxhd_game_source_code\.config` you will see `dotnet-tools.json`.
  - -AND- Right click, go to properties, check `Unblock`.
- Open ProjectZ.sln
- Build/run like any normal C# program

## Personal Build / Publishing

To create a personal build, follow the steps below:
- Download and install [.NET v6.0.428 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.428-windows-x64-installer).
- Clone or Download this repository: green `Code` Button > `Download ZIP`
- Unzip the repository and open up the unzipped folder.
- Follow the steps in **Updating Source Code Assets**
- Run the PowerShell script "Unblock-All-Files.ps1".
  - -OR- Go to the folder `ladxhd_game_source_code\.config` you will see `dotnet-tools.json`.
  - -AND- Right click, go to properties, check `Unblock`.
- Run the `ladxhd_game_source_code\publish.bat` script to build the game.
- Alternatively, the **"LADXHD_Migrater.exe"** tool can now build the game.
- When done, the build will be in the `Publish` folder.

## Build Troubleshooting

If you experience the error **The command “dotnet tool restore” exited with code 1** then make sure the file **.config\dotnet-tools.json** isn't blocked. 

- To unblock all files in one go, run the included PowerShell script **"Unblock-All-Files.ps1"**.
- To unblock a single file: Right click, go to Properties, check Unblock, and click OK.

## About This Fork

I am a terrible programmer, but I have a love for this game. A ton of forks popped up, some with fixes, but nowhere were they all centralized. This fork attempted to find and implement all the various fixes and improvements spread across the other various forks. Once that was done, I started tackling the issues from the repository this was cloned from. And after that was done, I worked on anything else I could find that would make the game feel more like the original game.

Feel free to commit any potential fixes as a PR. There are no coding guidelines and any style is welcome as long as the code either fixes something broken or makes the game behave closer to the original. But do try to at least keep it neat.

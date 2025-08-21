#========================================================================================================================================
# CONFIGURATION
#========================================================================================================================================

$OldGamePath = "C:\Users\Bighead\Desktop\original"
$NewGamePath = "C:\Users\Bighead\Desktop\updated"

#========================================================================================================================================
# SETUP XDELTA & OUTPUTS
#========================================================================================================================================

$BaseFolder  = Split-Path $script:MyInvocation.MyCommand.Path
$XDelta      = Join-Path $BaseFolder "xdelta3.exe"
$PatchFolder = Join-Path $BaseFolder "patches"

#========================================================================================================================================
# VERIFICATION
#========================================================================================================================================

if (!(Test-Path -LiteralPath $OldGamePath)) 
{
    Write-Host "Invalid path for original game (OldGamePath)."
    Write-Host ""
    Write-Host "Press any key to close this window."
    [void][System.Console]::ReadKey()
    Exit
}
if (!(Test-Path -LiteralPath $NewGamePath)) 
{
    Write-Host "Invalid path for updated game (NewGamePath)."
    Write-Host ""
    Write-Host "Press any key to close this window."
    [void][System.Console]::ReadKey()
    Exit
}

#========================================================================================================================================
# CREATE PATCHES FOLDER
#========================================================================================================================================

if (!(Test-Path -LiteralPath $PatchFolder)) 
{
    New-Item -Path $PatchFolder -ItemType Directory | Out-Null
}

#========================================================================================================================================
# PATHS TO FILES (DEFINE ONCE)
#========================================================================================================================================

$files = @(
    "Link's Awakening DX HD.exe",
    "Content\Fonts\credits font.xnb",
    "Content\Fonts\credits header font.xnb",
    "Data\musicOverworld.data",
    "Data\scripts.zScript",
    "Data\Intro\intro.png",
    "Data\Intro\intro.atlas",
    "Data\Languages\eng.lng",
    "Data\Languages\dialog_eng.lng",
    "Data\Maps\overworld.map",
    "Data\Maps\overworld.map.data",
    "Data\Maps\dreamShrine01.map",
    "Data\Maps\dreamShrine01.map.data",
    "Data\Maps\egg_lower_floor.map",
    "Data\Maps\egg_lower_floor.map.data",
    "Data\Maps\dungeon1.map",
    "Data\Maps\dungeon1.map.data",
    "Data\Maps\dungeon2_2d_2.map",
    "Data\Maps\dungeon2_2d_2.map.data",
    "Data\Maps\dungeon5_2d_1.map",
    "Data\Maps\dungeon5_2d_1.map.data",
    "Data\Maps\dungeon5_2d_2.map",
    "Data\Maps\dungeon5_2d_2.map.data",
    "Data\Maps\dungeon5_2d_3.map",
    "Data\Maps\dungeon5_2d_3.map.data",
    "Data\Maps\dungeon5_2d_4.map",
    "Data\Maps\dungeon5_2d_4.map.data",
    "Data\Maps\dungeon6.map",
    "Data\Maps\dungeon6.map.data",
    "Data\Maps\dungeon6_2d_1.map",
    "Data\Maps\dungeon6_2d_1.map.data",
    "Data\Maps\dungeon6_2d_2.map",
    "Data\Maps\dungeon6_2d_2.map.data",
    "Data\Maps\dungeon8.map",
    "Data\Maps\dungeon8.map.data",
    "Data\Maps\dungeon8_2d_1.map",
    "Data\Maps\dungeon8_2d_1.map.data",
    "Data\Maps\dungeon8_2d_2.map",
    "Data\Maps\dungeon8_2d_2.map.data",
    "Data\Maps\dungeon8_2d_3.map",
    "Data\Maps\dungeon8_2d_3.map.data",
    "Data\Maps\dungeon8_2d_4.map",
    "Data\Maps\dungeon8_2d_4.map.data",
    "Data\Maps\dungeon8_2d_5.map",
    "Data\Maps\dungeon8_2d_5.map.data",
    "Data\Maps\dungeon8_2d_6.map",
    "Data\Maps\dungeon8_2d_6.map.data",
    "Data\Photo Mode\photos.png"
)

#========================================================================================================================================
# GENERATE PATCHES
#========================================================================================================================================

foreach ($file in $files) 
{
    $src   = Join-Path $OldGamePath $file
    $tgt   = Join-Path $NewGamePath $file
    $patch = Join-Path $PatchFolder ((Split-Path $file -Leaf) + ".xdelta")

    Write-Host "Generating patch for: $file"
    & $XDelta -e -s $src $tgt $patch
}

Write-Host ""
Write-Host 'Patches go into the "Resources\<version> Patches" folder.'
Write-Host 'Old patches should be deleted out of "Resources.resx" in Visual Studio.'
Write-Host 'Newly generated patches should then be imported into "Resources.resx".'
Write-Host ""
Write-Host "Press any key to close this window."
[void][System.Console]::ReadKey()
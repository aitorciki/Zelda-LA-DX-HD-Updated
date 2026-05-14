@echo off
setlocal enabledelayedexpansion

for %%I in ("%~dp0.") do set "Root=%%~fI"
cd /d "%Root%"
Title LADXHD: Patcher Publish Script

::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────
:: Game Version
::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────

set GameVersion=1.8.1

::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────
:: Clean Previous Builds
::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────

if exist "%~dp0~Publish" (
    echo Cleaning previous builds...
    rd /s /q "%~dp0~Publish"
)

::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────
:: Publish all Builds
::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────

echo Building Windows x64...
dotnet publish LADXHD_Patcher.csproj -r win-x64 /p:PublishProfile=Windows
if %errorlevel% neq 0 ( echo Windows build failed! & pause & exit /b 1 )

echo Building Linux x64...
dotnet publish LADXHD_Patcher.csproj -r linux-x64 /p:PublishProfile=Linux-x64
if %errorlevel% neq 0 ( echo Linux x64 build failed! & pause & exit /b 1 )

echo Building Linux Arm64...
dotnet publish LADXHD_Patcher.csproj -r linux-arm64 /p:PublishProfile=Linux-arm64
if %errorlevel% neq 0 ( echo Linux Arm64 build failed! & pause & exit /b 1 )

echo Building MacOS x64...
dotnet publish LADXHD_Patcher.csproj -r osx-x64 /p:PublishProfile=macOS-x64
if %errorlevel% neq 0 ( echo MacOS x64 build failed! & pause & exit /b 1 )

echo Building MacOS Arm64...
dotnet publish LADXHD_Patcher.csproj -r osx-arm64 /p:PublishProfile=macOS-arm64
if %errorlevel% neq 0 ( echo MacOS Arm64 build failed! & pause & exit /b 1 )

::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────
:: Rename output "Launcher" executables to "LADXHD.Patcher.v#.#.#".
::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────

echo.
echo Renaming output executables...
if exist "%Root%\~Publish\Windows\Patcher.exe" (
    echo Renaming: "%Root%\~Publish\Windows\Patcher.exe" to "LADXHD.Patcher.v%GameVersion%.exe"
    ren "%Root%\~Publish\Windows\Patcher.exe" "LADXHD.Patcher.v%GameVersion%.exe"
)
if exist "%Root%\~Publish\Linux-x64\Patcher" (
    echo Renaming: "%Root%\~Publish\Linux-x64\Patcher" to "LADXHD.Patcher.v%GameVersion%"
    ren "%Root%\~Publish\Linux-x64\Patcher" "LADXHD.Patcher.v%GameVersion%"
)
if exist "%Root%\~Publish\Linux-arm64\Patcher" (
    echo Renaming: "%Root%\~Publish\Linux-arm64\Patcher" to "LADXHD.Patcher.v%GameVersion%"
    ren "%Root%\~Publish\Linux-arm64\Patcher" "LADXHD.Patcher.v%GameVersion%"
)
if exist "%Root%\~Publish\macOS-x64\Patcher.app" (
    echo Renaming: "%Root%\~Publish\macOS-x64\Patcher.app" to "LADXHD.Patcher.v%GameVersion%.app"
    ren "%Root%\~Publish\macOS-x64\Patcher.app" "LADXHD.Patcher.v%GameVersion%.app"
)
if exist "%Root%\~Publish\macOS-arm64\Patcher.app" (
    echo Renaming: "%Root%\~Publish\macOS-arm64\Patcher.app" to "LADXHD.Patcher.v%GameVersion%.app"
    ren "%Root%\~Publish\macOS-arm64\Patcher.app" "LADXHD.Patcher.v%GameVersion%.app"
)

::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────
:: Clean up unnecessary files
::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────

echo.
echo Cleaning up junk files...
for /r "%~dp0~Publish" %%f in (nfd.lib nfd.pdb) do (
  if exist "%%f" (
    echo Deleting: %%f
    del "%%f"
  )
)

::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────
:: Sign the MacOS Launcher
::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────
:: Uses a personal signing key. Users building for MacOS would need to generate their own key using "rcodesign.exe". 
:: This can be done with command: rcodesign generate-self-signed-certificate --person-name NAME > \path\to\NAME.pem

set CodeSignApp="%Root%\publish\rcodesign.exe"
set CodeSignKey="%USERPROFILE%\LADXHD\Bighead.pem"

echo.
echo Signing MacOS-x64 executable...
%CodeSignApp% sign --pem-source %CodeSignKey% "%Root%\~Publish\macOS-x64\LADXHD.Patcher.v%GameVersion%.app\Contents\MacOS\Patcher"
copy "%Root%\publish\fix-macos-permissions.command" "%Root%\~Publish\macOS-x64\fix-permissions.command"

echo.
echo Signing MacOS-Arm64 executable...
%CodeSignApp% sign --pem-source %CodeSignKey% "%Root%\~Publish\macOS-arm64\LADXHD.Patcher.v%GameVersion%.app\Contents\MacOS\Patcher"
copy "%Root%\publish\fix-macos-permissions.command" "%Root%\~Publish\macOS-arm64\fix-permissions.command"

::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────
:: Finish
::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────

echo.
echo Done! Builds can be found in the Publish folder.
pause >nul
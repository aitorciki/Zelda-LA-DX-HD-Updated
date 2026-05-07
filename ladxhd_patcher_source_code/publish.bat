@echo off
cd /d "%~dp0"
Title LADXHD: Patcher Publish Script

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
:: Finish
::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────

echo.
echo Done! Builds can be found in the Publish folder.
pause >nul
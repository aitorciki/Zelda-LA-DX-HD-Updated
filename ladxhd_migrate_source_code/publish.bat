@echo off
setlocal enabledelayedexpansion

cd /d "%~dp0"
Title LADXHD: Migration Tool Publish Script

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
dotnet publish LADXHD_Migrater.csproj -r win-x64 /p:PublishProfile=Windows
if %errorlevel% neq 0 ( echo Windows build failed! & pause & exit /b 1 )

::echo Building Linux x64...
::dotnet publish LADXHD_Migrater.csproj -r linux-x64 /p:PublishProfile=Linux-x64
::if %errorlevel% neq 0 ( echo Linux x64 build failed! & pause & exit /b 1 )

::echo Building Linux Arm64...
::dotnet publish LADXHD_Migrater.csproj -r linux-arm64 /p:PublishProfile=Linux-arm64
::if %errorlevel% neq 0 ( echo Linux Arm64 build failed! & pause & exit /b 1 )

::echo Building MacOS x64...
::dotnet publish LADXHD_Migrater.csproj -r osx-x64 /p:PublishProfile=macOS-x64
::if %errorlevel% neq 0 ( echo MacOS x64 build failed! & pause & exit /b 1 )

::echo Building MacOS Arm64...
::dotnet publish LADXHD_Migrater.csproj -r osx-arm64 /p:PublishProfile=macOS-arm64
::if %errorlevel% neq 0 ( echo MacOS Arm64 build failed! & pause & exit /b 1 )

::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────
:: Rename output "Migrater" executables to "LADXHD_Migrater".
::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────

echo.
echo Renaming output executables...
cd %cd%\~Publish
for /r %%f in (Migrater.*) do (
    if /I "%%~nf"=="Migrater" (
        echo Renaming: "%%f" to "LADXHD_Migrater%%~xf"
        ren "%%f" "LADXHD_Migrater%%~xf"
    )
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
:: Finish
::───────────────────────────────────────────────────────────────────────────────────────────────────────────────────

echo.
echo Done! Builds can be found in the Publish folder.
pause >nul
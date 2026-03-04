@echo off
cd /d "%~dp0"

echo Building Windows DirectX...
dotnet publish ProjectZ.Desktop\ProjectZ.Desktop.csproj -c Release -f net8.0-windows -r win-x64 -p:PublishProfile=FolderProfile_DX
if %errorlevel% neq 0 ( echo DX build failed! & pause & exit /b 1 )

echo Building Windows OpenGL...
dotnet publish ProjectZ.Desktop\ProjectZ.Desktop.csproj -c Release -f net8.0 -r win-x64 -p:PublishProfile=FolderProfile_GL
if %errorlevel% neq 0 ( echo GL build failed! & pause & exit /b 1 )

echo Done! Builds are in the Publish folder.
pause
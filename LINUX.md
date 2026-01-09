# 🐧 Running on Linux with Proton

## ⚡ Quick Start (Recommended)

Use the automated installer for a hassle-free setup:

[zladxhd-installer](https://github.com/jslay88/zladxhd-installer)

---

## 🔧 Manual Setup

If you prefer to set things up yourself, follow these steps:

### 1. Install Protontricks

Follow the [official installation guide](https://github.com/Matoking/protontricks?tab=readme-ov-file#installation).

<details>
<summary>⚠️ <strong>Flatpak users: You MUST set up aliases!</strong></summary>

```bash
echo "alias protontricks='flatpak run com.github.Matoking.protontricks'" >> ~/.bashrc
echo "alias protontricks-launch='flatpak run --command=protontricks-launch com.github.Matoking.protontricks'" >> ~/.bashrc
source ~/.bashrc
```

</details>

### 2. Extract the Game

Extract your game archive to a location of your choice:
```
~/Games/LADXHD/
```

### 3. Add to Steam

1. Open Steam → **Games** → **Add a Non-Steam Game to My Library**
2. Browse to and select `Link's Awakening DX HD.exe`

### 4. Configure Proton

1. Right-click the game in your Steam Library → **Properties**
2. Go to **Compatibility** tab
3. Check **Force the use of a specific Steam Play compatibility tool**
4. Select **Proton Experimental** (or your preferred version)

### 5. Create the Wine Prefix

1. Launch the game from Steam
2. You'll see an error about missing .NET - click **No** to close it
3. Close the game

### 6. Install .NET Runtime

Run this command to install the required .NET Desktop Runtime:

```bash
protontricks $(protontricks -l | grep -oE "Link's Awakening.*\(([0-9]+)\)" | grep -oE "[0-9]+") -q dotnetdesktop6
```

### 7. Apply the HD Patch

Navigate to your game folder and run the patcher:

```bash
cd ~/Games/LADXHD
protontricks-launch --appid $(protontricks -l | grep -oE "Link's Awakening.*\(([0-9]+)\)" | grep -oE "[0-9]+") LADXHD.Patcher.v1.5.2b.exe
```

<details>
<summary>🤖 <strong>Silent Mode (for scripts/automation)</strong></summary>

The patcher supports silent mode for automated installations:

```bash
protontricks-launch --appid $(protontricks -l | grep -oE "Link's Awakening.*\(([0-9]+)\)" | grep -oE "[0-9]+") LADXHD.Patcher.v1.5.2b.exe --silent
```

**Options:**
| Flag | Description |
|------|-------------|
| `--silent`, `-s` | Run without GUI prompts |
| `--help`, `-h` | Show help message |

**Exit codes:**
| Code | Meaning |
|------|---------|
| 0 | Success |
| 1 | Game executable not found |
| 2 | Patching failed |

</details>

### 8. Play! 🎮

Launch the game through Steam and enjoy!

---

## ⚠️ Known Issues

| Issue | Workaround |
|-------|------------|
| Window resizing on KDE | Drag window borders manually |

---

## 💾 Save Data Location

Your saves and settings are stored in the Wine prefix:

```bash
~/.steam/steam/steamapps/compatdata/<APP_ID>/pfx/drive_c/users/steamuser/AppData/Local/Zelda_LA
```

To open the folder directly:

```bash
cd ~/.steam/steam/steamapps/compatdata/$(protontricks -l | grep -oE "Link's Awakening.*\(([0-9]+)\)" | grep -oE "[0-9]+")/pfx/drive_c/users/steamuser/AppData/Local/Zelda_LA
```

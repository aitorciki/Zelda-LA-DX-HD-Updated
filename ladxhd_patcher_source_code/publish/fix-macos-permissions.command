#!/usr/bin/env bash

cd "$(dirname "$0")" || exit

cat <<EOF
----------------------------------------------
Link's Awakening DX HD - macOS Permissions Fix
----------------------------------------------

EOF

PATCHER_APP=$(find . -name "LADXHD.Patcher.v*.app" 2>/dev/null | head -n 1)

if [ -z "$PATCHER_APP" ]; then
    echo "Error: Could not find a file matching 'LADXHD.Patcher.v*.app' in this folder."
    exit 1
fi

echo "Found: $PATCHER_APP"

echo "Removing quarantine flag..."
xattr -dr com.apple.quarantine "$PATCHER_APP" 2>/dev/null

echo "Setting execution permissions..."
chmod +x "$PATCHER_APP/Contents/MacOS/Patcher"

printf "\nDone! You can now run the patcher.\n\n"

read -rp "Press Enter to exit..."

#!/bin/bash
cd "$(dirname "$0")"

echo "------------------------------------------------------"
echo "Link's Awakening DX HD - macOS Permission Fix"
echo "------------------------------------------------------"

PATCHER_FILE=$(ls LADXHD.Patcher.v* 2>/dev/null | head -n 1)

if [ -z "$PATCHER_FILE" ]; then
    echo "Error: Could not find a file starting with 'LADXHD.Patcher.v' in this folder."
    read -p "Press Enter to exit..."
    exit 1
fi

echo "Found: $PATCHER_FILE"
echo "Removing quarantine flags and setting execution permissions..."

xattr -d com.apple.quarantine "$PATCHER_FILE" *.dylib 2>/dev/null
chmod +x "$PATCHER_FILE"

echo "Done! You can now close this window and run the patcher."
echo "------------------------------------------------------"
read -p "Press Enter to exit..."
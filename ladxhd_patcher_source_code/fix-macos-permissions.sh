#!/usr/bin/env bash

cd "$(dirname "$0")" || exit

cat <<EOF
----------------------------------------------
Link's Awakening DX HD - macOS Permissions Fix
----------------------------------------------
EOF

PATCHER_FILE=$(find . -name "LADXHD.Patcher.v*" 2>/dev/null | head -n 1)

if [ -z "$PATCHER_FILE" ]; then
    echo "Error: Could not find a file starting with 'LADXHD.Patcher.v' in this folder."
    exit 1
fi

echo "Found: $PATCHER_FILE"
echo "Removing quarantine flag and setting execution permissions..."

xattr -d com.apple.quarantine "$PATCHER_FILE" ./*.dylib 2>/dev/null
chmod +x "$PATCHER_FILE"

echo "Done! You can now run the patcher."
read -rp "Press Enter to exit..."

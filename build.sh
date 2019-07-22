#!/bin/bash

EDITOR="$HOME/Unity/Hub/Editor/2019.1.10f1/Editor/Unity"
BUILD_DIR="/tmp/build-unity"

rm -rf "$BUILD_DIR"

mkdir -p "$BUILD_DIR/Linux"
mkdir -p "$BUILD_DIR/macOS"
mkdir -p "$BUILD_DIR/Windows"

"$EDITOR" -buildLinuxUniversalPlayer "$BUILD_DIR/Linux/SEGMent"   -projectPath "$PWD" -batchmode -quit
"$EDITOR" -buildOSXUniversalPlayer   "$BUILD_DIR/macOS/SEGMent"   -projectPath "$PWD" -batchmode -quit 
"$EDITOR" -buildWindows64Player      "$BUILD_DIR/Windows/SEGMent" -projectPath "$PWD" -batchmode -quit


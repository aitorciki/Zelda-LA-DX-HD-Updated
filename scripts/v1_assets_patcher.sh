#!/usr/bin/env bash

SCRIPT_DIR="$(dirname "$(realpath "${BASH_SOURCE[0]}")")"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"

SOURCE_DIR="${PROJECT_DIR}/assets_original"
PATCHES_DIR="${PROJECT_DIR}/assets_patches"
OUTPUT_DIR="${PROJECT_DIR}/ladxhd_game_source_code"
TMP_DIR=$(mktemp -d 2>/dev/null || mktemp -d -t 'ladxhd')

cleanup() {
    rm -rf "$TMP_DIR"
}

trap cleanup EXIT

usage() {
    echo "Usage: $0 [-p] [-c] [-u] [-v]"
    echo "  -p    patch v1 files and copy to source directory"
    echo "  -c    create patches from source directory changes into patches directory"
    echo "  -u    transform text files to unix format after patching into source"
    echo "  -v    verbose"
    exit 1
}

check_binary() {
    local binary="$1"

    command -v "$binary" >/dev/null 2>&1 || {
        echo >&2 "Error: ${binary} is required but not installed. Aborting."
        exit 1
    }
}

hash_delta() {
    xdelta3 printhdr "$1" | awk -F': +' '/adler32 checksum/ { print $2 }'
}

compare_deltas() {
    local old="$1"
    local new="$2"
    local old_hash
    local new_hash

    if [[ -f "$old" && -f "$new" ]]; then
        old_hash=$(hash_delta "$old")
        new_hash=$(hash_delta "$new")

        if [[ "$old_hash" == "$new_hash" ]]; then
            return 0
        fi
    fi

    return 1
}

patch() {
    local old="$1"
    local new="$2"
    local delta="$3"

    if $verbose; then
        printf "\n------ Patching %s ------\n" "${old#"$SOURCE_DIR"}"
    fi

    if [[ -f "$delta" ]]; then
        if $verbose; then
            echo "xdelta3 -d -f -s \"$old\" \"$delta\" \"$new\""
        fi
        xdelta3 -d -f -s "$old" "$delta" "$new"
    else
        if $verbose; then
            echo "cp \"$old\" \"$new\""
        fi
        cp "$old" "$new"
    fi

    if $to_unix; then
        if $verbose; then
            dos2unix "$new"
        else
            dos2unix -q "$new"
        fi
    fi
}

patch_multi() {
    local output_dir
    local delta
    local old="$1"
    shift

    output_dir="$(dirname "${OUTPUT_DIR}/${old#"$SOURCE_DIR"}")"

    for new in "$@"; do
        delta="${PATCHES_DIR}/${new}.xdelta"
        if [[ -f "$delta" ]]; then
            patch "$old" "${output_dir}/${new}" "$delta"
        fi
    done
}

create() {
    local old="$1"
    local new="$2"
    local delta="$3"
    local tmp
    tmp="${TMP_DIR}/$(basename "$delta")"

    if [[ ! -f "$old" || ! -f "$new" ]]; then
        return 1
    fi

    if $verbose; then
        printf "\n------ Creating patch for %s ------\n" "${old#"$SOURCE_DIR"}"
    fi

    if cmp -s "$old" "$new"; then
        if $verbose; then
            echo "Unchanged, skipping."
        fi
        return 1
    else
        if $verbose; then
            echo "xdelta3 -f -s \"${old}\" \"${new}\" \"${delta}\""
        fi
        # create a temporary delta and only keep it if it differs from an existing one
        xdelta3 -f -s "$old" "$new" "$tmp"
        if ! compare_deltas "$delta" "$tmp"; then
            mv "$tmp" "$delta"
        else
            if $verbose; then
                echo "No changes from existing delta, skipping."
            fi
        fi
    fi
}

create_multi() {
    local new_dir
    local delta
    local old="$1"
    shift

    new_dir="$(dirname "${OUTPUT_DIR}/${old#"$SOURCE_DIR"}")"

    for new in "$@"; do
        delta="${PATCHES_DIR}/${new}.xdelta"
        create "$old" "${new_dir}/${new}" "$delta"
    done
}

main() {
    if [[ -z "$1" ]]; then
        echo "Error: You must provide either -p (patch) or -c (create patches)."
        usage
    fi

    local action="$1"
    local action_multi="${action}_multi"

    check_binary "xdelta3"
    if $to_unix; then
        check_binary "dos2unix"
    fi

    find "${SOURCE_DIR}/Data" "${SOURCE_DIR}/Content" -type f -print0 | while IFS= read -r -d '' old; do
        new="${OUTPUT_DIR}/${old#"$SOURCE_DIR"}"
        mkdir -p "$(dirname "$new")"

        delta="${PATCHES_DIR}/$(basename "$old").xdelta"
        $action "$old" "$new" "$delta"

        # some origin files map to multiple output files
        case "$(basename "$old")" in
        "eng.lng")
            $action_multi "$old" "chn.lng" "deu.lng" "esp.lng" "fre.lng" "ind.lng" "ita.lng" "por.lng" "rus.lng"
            ;;
        "dialog_eng.lng")
            $action_multi "$old" "dialog_chn.lng" "dialog_deu.lng" "dialog_esp.lng" "dialog_fre.lng" "dialog_ind.lng" "dialog_ita.lng" "dialog_por.lng" "dialog_rus.lng"
            ;;
        "smallFont.png")
            $action_multi "$old" "smallFont_redux.png" "smallFont_vwf.png" "smallFont_vwf_redux.png" "smallFont_chn_0.png" "smallFont_chn_redux_0.png"
            ;;
        "menuBackground.png")
            $action_multi "$old" "menuBackgroundB.png" "menuBackgroundC.png" "sgb_border.png"
            ;;
        "link0.png")
            $action_multi "$old" "link1.png"
            ;;
        "npcs.png")
            $action_multi "$old" "npcs_redux.png"
            ;;
        "items.png")
            $action_multi "$old" "items_chn.png" "items_deu.png" "items_esp.png" "items_fre.png" "items_ind.png" "items_ita.png" "items_por.png" "items_rus.png" "items_redux.png" "items_redux_chn.png" "items_redux_deu.png" "items_redux_esp.png" "items_redux_fre.png" "items_redux_ind.png" "items_redux_ita.png" "items_redux_por.png" "items_redux_rus.png"
            ;;
        "intro.png")
            $action_multi "$old" "intro_chn.png" "intro_deu.png" "intro_esp.png" "intro_fre.png" "intro_ind.png" "intro_ita.png" "intro_por.png" "intro_rus.png"
            ;;
        "intro.atlas")
            $action_multi "$old" "intro_chn.atlas"
            ;;
        "minimap.png")
            $action_multi "$old" "minimap_chn.png" "minimap_deu.png" "minimap_esp.png" "minimap_fre.png" "minimap_ind.png" "minimap_ita.png" "minimap_por.png" "minimap_rus.png"
            ;;
        "objects.png")
            $action_multi "$old" "objects_chn.png" "objects_deu.png" "objects_esp.png" "objects_fre.png" "objects_ind.png" "objects_ita.png" "objects_por.png" "objects_rus.png"
            ;;
        "photos.png")
            $action_multi "$old" "photos_chn.png" "photos_deu.png" "photos_esp.png" "photos_fre.png" "photos_ind.png" "photos_ita.png" "photos_por.png" "photos_rus.png" "photos_redux.png" "photos_redux_chn.png" "photos_redux_deu.png" "photos_redux_esp.png" "photos_redux_fre.png" "photos_redux_ind.png" "photos_redux_ita.png" "photos_redux_por.png" "photos_redux_rus.png"
            ;;
        "ui.png")
            $action_multi "$old" "ui_chn.png" "ui_deu.png" "ui_esp.png" "ui_fre.png" "ui_ind.png" "ui_ita.png" "ui_por.png" "ui_rus.png"
            ;;
        "musicOverworld.data")
            $action_multi "$old" "musicOverworldClassic.data"
            ;;
        "dungeon3_1.map")
            $action_multi "$old" "dungeon3.map"
            ;;
        "dungeon3_1.map.data")
            $action_multi "$old" "dungeon3.map.data"
            ;;
        "BowWow.ani")
            $action_multi "$old" "bowwow_water.ani"
            ;;
        "mapPlayer.ani")
            $action_multi "$old" "mapDungeon.ani" "mapManboPond.ani"
            ;;
        esac
    done
}

action_patch=false
action_create=false
to_unix=false
verbose=false

while getopts "pcuv" opt; do
    case "$opt" in
    p) action_patch=true ;;
    c) action_create=true ;;
    u) to_unix=true ;;
    v) verbose=true ;;
    *) usage ;;
    esac
done

if [[ "$action_patch" == false && "$action_create" == false ]]; then
    echo "Error: You must provide either -p (patch) or -c (create patches)."
    usage
fi

if [[ "$action_patch" == true && "$action_create" == true ]]; then
    echo "Error: Options -p and -c cannot be used together."
    usage
fi

if $action_patch; then
    main "patch"
fi

if $action_create; then
    main "create"
fi

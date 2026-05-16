using System;
using System.IO;
using ProjectZ.Base;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.SaveLoad
{
    internal static class SharedSaveSync
    {
        public static void SyncFromSharedIfEnabled()
        {
        #if ANDROID
            if (!GameSettings.SharedStorage)
                return;
            if (!AndroidStorage.HasAllFilesAccess())
                return;
            try
            {
                SyncFromShared();
            }
            catch { }
        #endif
        }

    #if ANDROID
        private static void SyncFromShared()
        {
            var sharedSaveDir = AndroidStorage.GetSharedSavePath();
            var scopedSaveDir = Path.Combine(Values.UserDataRoot, "SaveFiles");

            // Nothing to sync from if shared doesn't exist yet.
            if (!Directory.Exists(sharedSaveDir))
                return;

            // Ensure scoped exists before we try to copy into it.
            Directory.CreateDirectory(scopedSaveDir);

            for (var slot = 0; slot < SaveStateManager.SaveCount; slot++)
                SyncSlot(slot, sharedSaveDir, scopedSaveDir);
        }

        private static void SyncSlot(int slot, string sharedDir, string scopedDir)
        {
            var sharedSave     = Path.Combine(sharedDir, SaveGameSaveLoad.SaveFileName + slot);
            var sharedSaveGame = Path.Combine(sharedDir, SaveGameSaveLoad.SaveFileNameGame + slot);
            var scopedSave     = Path.Combine(scopedDir, SaveGameSaveLoad.SaveFileName + slot);
            var scopedSaveGame = Path.Combine(scopedDir, SaveGameSaveLoad.SaveFileNameGame + slot);

            // If shared doesn't have a complete pair, there's nothing to sync.
            if (!File.Exists(sharedSave) || !File.Exists(sharedSaveGame))
                return;

            // Determine if shared is newer than scoped.
            var sharedTime = MinWriteTime(sharedSave, sharedSaveGame);

            // If scoped is missing the pair entirely, shared wins unconditionally.
            if (!File.Exists(scopedSave) || !File.Exists(scopedSaveGame))
            {
                CopyPair(sharedSave, sharedSaveGame, scopedSave, scopedSaveGame);
                return;
            }

            // Both sides have the pair; compare timestamps.
            var scopedTime = MaxWriteTime(scopedSave, scopedSaveGame);

            // This avoids needlessly re-copying when both sides are identical.
            if (sharedTime > scopedTime)
                CopyPair(sharedSave, sharedSaveGame, scopedSave, scopedSaveGame);
            
        }

        private static void CopyPair(string srcSave, string srcSaveGame,
                                     string dstSave, string dstSaveGame)
        {
            // Copy through temp files so a mid-copy failure can't leave a
            // half-written destination. Same pattern as SaveManager.Save.
            var tmpSave     = dstSave + ".tmp";
            var tmpSaveGame = dstSaveGame + ".tmp";

            File.Copy(srcSave,     tmpSave,     overwrite: true);
            File.Copy(srcSaveGame, tmpSaveGame, overwrite: true);

            if (File.Exists(dstSave))     File.Delete(dstSave);
            if (File.Exists(dstSaveGame)) File.Delete(dstSaveGame);

            File.Move(tmpSave,     dstSave);
            File.Move(tmpSaveGame, dstSaveGame);
        }

        private static DateTime MinWriteTime(string a, string b)
        {
            var ta = File.GetLastWriteTimeUtc(a);
            var tb = File.GetLastWriteTimeUtc(b);
            return ta < tb ? ta : tb;
        }

        private static DateTime MaxWriteTime(string a, string b)
        {
            var ta = File.GetLastWriteTimeUtc(a);
            var tb = File.GetLastWriteTimeUtc(b);
            return ta > tb ? ta : tb;
        }
    #endif
    }
}
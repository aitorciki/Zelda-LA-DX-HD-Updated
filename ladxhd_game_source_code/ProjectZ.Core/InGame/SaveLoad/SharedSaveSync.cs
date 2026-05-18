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
            var sharedSaveExists     = File.Exists(sharedSave);
            var sharedSaveGameExists = File.Exists(sharedSaveGame);
            var scopedSaveExists     = File.Exists(scopedSave);
            var scopedSaveGameExists = File.Exists(scopedSaveGame);

            if (!sharedSaveExists || !sharedSaveGameExists)
                return;

            // Determine if shared is newer than scoped.
            var sharedTime = MinWriteTime(sharedSave, sharedSaveGame);

            // If scoped is missing the pair entirely, shared wins unconditionally.
            if (!scopedSaveExists || !scopedSaveGameExists)
            {
                TryCopyPair(sharedSave, sharedSaveGame, scopedSave, scopedSaveGame);
                return;
            }
            // Both sides have the pair; compare timestamps.
            var scopedTime = MaxWriteTime(scopedSave, scopedSaveGame);

            // This avoids needlessly re-copying when both sides are identical.
            if (sharedTime > scopedTime)
                TryCopyPair(sharedSave, sharedSaveGame, scopedSave, scopedSaveGame);
        }

        private static void TryCopyPair(string srcSave, string srcSaveGame, string dstSave, string dstSaveGame)
        {
            try { CopyPair(srcSave, srcSaveGame, dstSave, dstSaveGame); }
            catch { }
        }

        private static void CopyPair(string srcSave, string srcSaveGame, string dstSave, string dstSaveGame)
        {
            // Uses Java.IO instead of System.IO.File.Copy because on Samsung Android 16+
            // (One UI 8.5), .NET's libc-level filesystem calls into "/Android/data/<pkg>/"
            // can be silently killed by SELinux policy when the process holds
            // MANAGE_EXTERNAL_STORAGE. Routing through Android's framework I/O channel
            // is treated as app-internal access and is allowed. 
            // Verified on Z Fold 7 (issue #843).
            JavaStreamCopy(srcSave, dstSave);
            JavaStreamCopy(srcSaveGame, dstSaveGame);
        }

        private static void JavaStreamCopy(string srcPath, string dstPath)
        {
            using var input = new Java.IO.FileInputStream(srcPath);
            using var output = new Java.IO.FileOutputStream(dstPath);

            var buffer = new byte[8192];
            int read;
            while ((read = input.Read(buffer)) > 0)
                output.Write(buffer, 0, read);
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
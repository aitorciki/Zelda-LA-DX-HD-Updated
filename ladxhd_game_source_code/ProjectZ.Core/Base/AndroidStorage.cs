#if ANDROID
using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Provider;
using AndroidNet = Android.Net;
using AndroidEnv = Android.OS.Environment;

namespace ProjectZ.Base
{
    public static class AndroidStorage
    {
        private const string ExternalRootName = "LADXHD";
        private const string ExternalSavesSubfolder = "SaveFiles";

        public static bool HasAllFilesAccess()
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(30))
                return AndroidEnv.IsExternalStorageManager;
            return Application.Context.CheckSelfPermission(
                global::Android.Manifest.Permission.WriteExternalStorage) == Permission.Granted;
        }

        public static void RequestAllFilesAccess()
        {
            try
            {
                Intent intent;

                if (OperatingSystem.IsAndroidVersionAtLeast(30))
                {
                    // Direct deep-link to the "All files access" toggle for this app.
                    intent = new Intent(Settings.ActionManageAppAllFilesAccessPermission);
                    intent.SetData(AndroidNet.Uri.Parse("package:" + Application.Context.PackageName));
                }
                else
                {
                    intent = new Intent(Settings.ActionApplicationDetailsSettings);
                    intent.SetData(AndroidNet.Uri.Parse("package:" + Application.Context.PackageName));
                }

                intent.AddFlags(ActivityFlags.NewTask);
                Application.Context.StartActivity(intent);
            }
            catch { }
        }

        public static string GetSharedRootPath()
        {
            return Path.Combine( AndroidEnv.ExternalStorageDirectory.AbsolutePath, ExternalRootName);
        }

        public static string GetSharedSavePath()
        {
            return Path.Combine(GetSharedRootPath(), ExternalSavesSubfolder);
        }
    }
}
#endif
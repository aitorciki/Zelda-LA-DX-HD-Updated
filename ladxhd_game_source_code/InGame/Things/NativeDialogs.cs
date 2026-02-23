using System;
using System.Runtime.InteropServices;

namespace ProjectZ.InGame.Things
{
    internal static class NativeDialogs
    {
        // SDL_ShowSimpleMessageBox flags
        private const uint SDL_MESSAGEBOX_ERROR = 0x00000010;
        private const uint SDL_MESSAGEBOX_WARNING = 0x00000020;
        private const uint SDL_MESSAGEBOX_INFORMATION = 0x00000040;

        [DllImport(
            "SDL2",
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "SDL_ShowSimpleMessageBox"
        )]
        private static extern int SDL_ShowSimpleMessageBox(
            uint flags,
            string title,
            string message,
            IntPtr window
        );

        public static void ShowError(string title, string message)
        {
            try
            {
                SDL_ShowSimpleMessageBox(SDL_MESSAGEBOX_ERROR, title, message, IntPtr.Zero);
            }
            catch
            {
                // Fallback to console if SDL2 is not yet loaded or unavailable
                Console.WriteLine($"[ERROR] {title}: {message}");
            }
        }
    }
}

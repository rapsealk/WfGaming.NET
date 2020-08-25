using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WfGaming.Utils
{
    class KeyHook
    {
        // https://docs.microsoft.com/en-us/previous-versions/windows/desktop/legacy/ms644985(v=vs.85)
        private delegate IntPtr LowLevelKeyProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;

        private static LowLevelKeyProc keyboardProc = KeyboardHookProc;

        private static IntPtr keyHook = IntPtr.Zero;

        private static Queue<char> texts = new Queue<char>();

        public static void InstallHook()
        {
            if (keyHook == IntPtr.Zero)
            {
                using (Process process = Process.GetCurrentProcess())
                {
                    using (ProcessModule currentModule = process.MainModule)
                    {
                        keyHook = SetWindowsHookEx(WH_KEYBOARD_LL, keyboardProc, GetModuleHandle(currentModule.ModuleName), 0);
                    }
                }
            }
        }

        public static void UninstallHook()
        {
            UnhookWindowsHookEx(keyHook);
        }

        private static IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (int)wParam == WM_KEYDOWN)
            {
                texts.Enqueue(Convert.ToChar(Marshal.ReadInt32(lParam)));
                //return (IntPtr)1; // Ignore keyboard input
            }
            return CallNextHookEx(keyHook, nCode, (int)wParam, lParam);
        }

        string ttmp = string.Empty;

        public void DigestQueue()
        {
            while (true)
            {
                Thread.Sleep(100);
                if (texts.Count > 0)
                {
                    char character = texts.Dequeue();
                    ttmp += character;
                    Console.WriteLine($"Text: {ttmp}");
                }
            }
        }
    }
}

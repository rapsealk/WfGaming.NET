using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;

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

        private LowLevelKeyProc keyboardProc;

        private IntPtr keyHook = IntPtr.Zero;

        public void InstallHook()
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

        public void UninstallHook()
        {
            UnhookWindowsHookEx(keyHook);
        }

        private IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (int)wParam == WM_KEYDOWN)
            {
                queue.Enqueue(Convert.ToChar(Marshal.ReadInt32(lParam)));
                //return (IntPtr)1; // Ignore keyboard input
            }
            return CallNextHookEx(keyHook, nCode, (int)wParam, lParam);
        }

        private ConcurrentQueue<char> queue = new ConcurrentQueue<char>();

        private char _forwardKey;
        private char _turnKey;
        private char _fireKey;

        public char ForwardKey
        {
            get
            {
                char value = _forwardKey;
                _forwardKey = char.MinValue;
                return value;
            }
        }

        public char TurnKey
        {
            get
            {
                char value = _turnKey;
                _turnKey = char.MinValue;
                return value;
            }
        }

        public char FireKey
        {
            get
            {
                char value = _fireKey;
                _fireKey = char.MinValue;
                return value;
            }
        }

        public KeyHook()
        {
            this.keyboardProc = KeyboardHookProc;
        }

        public void DigestQueue()
        {
            while (true)
            {
                try
                {
                    char key = char.MinValue;
                    if (queue.TryDequeue(out key))
                    {
                        Console.WriteLine($"DigestQueue: [{key}/{(int)key}]");
                        if (key == 'W' || key == 'S')
                        {
                            _forwardKey = key;
                        }
                        else if (key == 'Q' || key == 'E')
                        {
                            _turnKey = key;
                        }
                        else if (key == ' ')
                        {
                            _fireKey = key;
                        }
                        Console.WriteLine($"- Forward: [{_forwardKey}], Turn: [{_turnKey}], Fire: [{_fireKey}]");
                    }
                }
                catch (System.Threading.ThreadInterruptedException e)
                {
                    Console.WriteLine("KeyHook.ThreadInterruptedException");
                    break;
                }
            }
            Console.WriteLine("KeyHook.DigestQueue.Termintaed");
        }
    }
}

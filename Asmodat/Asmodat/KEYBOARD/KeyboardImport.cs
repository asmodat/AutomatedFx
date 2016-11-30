using System;

using System.IO;

using System.Drawing;
using System.Drawing.Imaging;

using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Globalization;

using System.Windows.Forms;

namespace Asmodat
{

    public partial class Keyboard
    {
        
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int keyCode);

        [DllImport("user32.dll")]
        public static extern short GetKeyState(byte keyCode);

        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] lpKeyState);


        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "SendInput", SetLastError = true)]
        public static extern uint SendInput( uint nInputs,ref INPUT pInput, int cbSize);


        [StructLayout(LayoutKind.Explicit, Size = 28)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public uint type;
            [FieldOffset(4)]
            public KEYBOARDINPUT ki;
        }

        public struct KEYBOARDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public ushort dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        public enum Win32
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2,
        }



        public enum dwFlgsSI
        {
            KEYDOWN = 0x0000,
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            UNICODE = 0x0004,
            SCANCODE = 0x0008,
        }

        public enum dwFlags
        {
            KE_SysKeyDown = 0x01,
            KE_SysKeyUp = 0x02,
            WM_KeyDown = 0x100,
            WM_KeyUp = 0x101,
            WM_SysKeyDown = 0x104,
            WM_SysKeyUp = 0x105
        }


    }

    
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotkeyWidget {
    // https://stackoverflow.com/questions/12805345/send-combination-of-keystrokes-to-background-window

    public class SendMessage {
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);


        public static void sendKeystroke(string processName, List<Keys> key_modifiers, Keys key) {
            const uint WM_KEYDOWN = 0x100;
            const uint WM_KEYUP = 0x0101;

            IntPtr hWnd;
            Process[] processList = Process.GetProcesses();

            foreach(Process P in processList) {
                if(P.ProcessName.Equals(processName)) {
                    hWnd = P.MainWindowHandle;
                    IntPtr editx = hWnd;
                    //IntPtr editx = FindWindowEx(hWnd, IntPtr.Zero, "Edit", null);
                    foreach(Keys mod_key in key_modifiers) {
                        PostMessage(editx, WM_KEYDOWN, (IntPtr)(mod_key), IntPtr.Zero);
                    }
                    PostMessage(editx, WM_KEYDOWN, (IntPtr)(key), IntPtr.Zero);
                    System.Threading.Thread.Sleep(1);
                    PostMessage(editx, WM_KEYUP, (IntPtr)(key), IntPtr.Zero);
                    foreach(Keys mod_key in key_modifiers) {
                        PostMessage(editx, WM_KEYUP, (IntPtr)(mod_key), IntPtr.Zero);
                    }
                }
            }
        }

    }
}

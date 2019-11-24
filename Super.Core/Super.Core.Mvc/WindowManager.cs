using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Super.Core.Mvc
{
    public class WindowManager
    {
        public static void Hide()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ShowWindow(GetConsoleWindow(), SW_HIDE);
            }
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
    }
}

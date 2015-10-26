namespace SBL.WPF.Controls.Win32
{
    using System;
    using System.Runtime.InteropServices;

    public static class WinAPI
    {
        [DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        public static extern IntPtr GetClassLong32(IntPtr hwnd, SetClassLongParam nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        public static extern IntPtr GetClassLong64(IntPtr hwnd, SetClassLongParam nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosParam flags);

        public static WindowStyles GetWindowLongStyle(this IntPtr hWnd)
        {
            return (WindowStyles)GetWindowLong(hWnd, (int)SetWindowLongParam.GWL_STYLE);
        }

        public static WindowStylesEx GetWindowLongExStyle(this IntPtr hWnd)
        {
            return (WindowStylesEx)GetWindowLong(hWnd, (int)SetWindowLongParam.GWL_EXSTYLE);
        }

        public static WindowStyles SetWindowLongStyle(this IntPtr hWnd, WindowStyles dwNewLong)
        {
            return (WindowStyles)SetWindowLong(hWnd, (int)SetWindowLongParam.GWL_STYLE, (int)dwNewLong);
        }

        public static WindowStylesEx SetWindowLongExStyle(this IntPtr hWnd, WindowStylesEx dwNewLong)
        {
            return (WindowStylesEx)SetWindowLong(hWnd, (int)SetWindowLongParam.GWL_EXSTYLE, (int)dwNewLong);
        }

        public static ClassStyles GetClassLong(this IntPtr hwnd, SetClassLongParam flags)
        {
            return IntPtr.Size == 8 ?
                (ClassStyles)GetClassLong64(hwnd, flags) :
                (ClassStyles)GetClassLong32(hwnd, flags);
        }

    }
}
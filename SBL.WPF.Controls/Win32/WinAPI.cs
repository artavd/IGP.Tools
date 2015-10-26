using System.Windows;

namespace SBL.WPF.Controls.Win32
{
    using System;
    using System.Runtime.InteropServices;

    public static class WinAPI
    {
        [DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
        private static extern int GetWindowLong(this IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(this IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        public static extern IntPtr GetClassLong32(this IntPtr hwnd, SetClassLongParam nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        public static extern IntPtr GetClassLong64(this IntPtr hwnd, SetClassLongParam nIndex);

        [DllImport("user32.dll", EntryPoint = "SetClassLong")]
        public static extern IntPtr SetClassLong32(IntPtr hWnd, SetClassLongParam nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetClassLongPtr")]
        public static extern IntPtr SetClassLong64(IntPtr hWnd, SetClassLongParam nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(this IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosParam flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(this IntPtr hwnd, WindowsMessages msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(this IntPtr hWnd, WindowsMessages msg, IntPtr wParam, IntPtr lParam);

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

        public static ClassStyles SetClassLong(this IntPtr hwnd, SetClassLongParam flags, ClassStyles dwLong)
        {
            return IntPtr.Size == 8 ?
                (ClassStyles)SetClassLong64(hwnd, flags, (IntPtr)dwLong) :
                (ClassStyles)SetClassLong32(hwnd, flags, (IntPtr)dwLong);
        }

        public static Point ToPoint(this IntPtr dword)
        {
            return new Point((short)((uint)dword & 0xffff), (short)((uint)dword >> 16));
        }
    }
}
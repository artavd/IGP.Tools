// ReSharper disable InconsistentNaming
namespace SBL.WPF.Controls.Win32
{
    public enum SetClassLongParam
    {
        /// <summary>
        /// Retrieves an ATOM value that uniquely identifies the window class. This is the same atom that the RegisterClassEx function returns.
        /// </summary>
        GCW_ATOM = -32,

        /// <summary>
        /// Retrieves the size, in bytes, of the extra memory associated with the class.
        /// </summary>
        GCL_CBCLSEXTRA = -20,

        /// <summary>
        /// Retrieves the size, in bytes, of the extra window memory associated with each window in the class. For information on how to access this memory, see GetWindowLong.
        /// </summary>
        GCL_CBWNDEXTRA = -18,

        /// <summary>
        /// Retrieves a handle to the background brush associated with the class.
        /// </summary>
        GCL_HBRBACKGROUND = -10,

        /// <summary>
        /// Retrieves a handle to the cursor associated with the class.
        /// </summary>
        GCL_HCURSOR = -12,

        /// <summary>
        /// Retrieves a handle to the icon associated with the class.
        /// </summary>
        GCL_HICON = -14,

        /// <summary>
        /// Retrieves a handle to the small icon associated with the class.
        /// </summary>
        GCL_HICONSM = -34,

        /// <summary>
        /// Retrieves a handle to the module that registered the class.
        /// </summary>
        GCL_HMODULE = -16,

        /// <summary>
        /// Retrieves the address of the menu name string. The string identifies the menu resource associated with the class.
        /// </summary>
        GCL_MENUNAME = -8,

        /// <summary>
        /// Retrieves the window-class style bits.
        /// </summary>
        GCL_STYLE = -26,

        /// <summary>
        /// Retrieves the address of the window procedure, or a handle representing the address of the window procedure.You must use the CallWindowProc function to call the window procedure.
        /// </summary>
        GCL_WNDPROC = -24,
    }
}
using System.Runtime.InteropServices;
using System.Text;

namespace ImeEdgeBar;

/// <summary>
/// P/Invoke declarations for Windows IME and user32 APIs.
/// </summary>
internal static class NativeMethods
{
    /// <summary>Message sent to an IME window to control it.</summary>
    internal const int WM_IME_CONTROL = 0x0283;

    /// <summary>Subcommand: get the open/close status of the IME.</summary>
    internal const int IMC_GETOPENSTATUS = 0x0005;

    /// <summary>Posted to a window when a registered hotkey is pressed.</summary>
    internal const int WM_HOTKEY = 0x0312;

    /// <summary>Modifier flag for RegisterHotKey: Control key.</summary>
    internal const uint MOD_CONTROL = 0x0002;

    /// <summary>Modifier flag for RegisterHotKey: Alt key.</summary>
    internal const uint MOD_ALT = 0x0001;

    /// <summary>Returns the handle to the foreground window.</summary>
    [DllImport("user32.dll")]
    internal static extern IntPtr GetForegroundWindow();

    /// <summary>Returns the default IME window handle associated with the specified window.</summary>
    [DllImport("imm32.dll")]
    internal static extern IntPtr ImmGetDefaultIMEWnd(IntPtr hWnd);

    /// <summary>Sends the specified message to a window.</summary>
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    /// <summary>Destroys an icon and frees any memory it occupied.</summary>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool DestroyIcon(IntPtr hIcon);

    /// <summary>Retrieves the identifier of the thread and process that created the specified window.</summary>
    [DllImport("user32.dll")]
    internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    /// <summary>Retrieves the name of the class to which the specified window belongs.</summary>
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    /// <summary>Registers a system-wide hotkey.</summary>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    /// <summary>Removes a hotkey registration.</summary>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
}

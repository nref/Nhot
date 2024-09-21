using System.Runtime.InteropServices;

namespace Nhot;

public static class User32
{
    public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr CreateWindowEx(
        uint dwExStyle,
        [MarshalAs(UnmanagedType.LPStr)] string lpClassName,
        [MarshalAs(UnmanagedType.LPStr)] string lpWindowName,
        uint dwStyle,
        int x,
        int y,
        int nWidth,
        int nHeight,
        IntPtr hWndParent,
        IntPtr hMenu,
        IntPtr hInstance,
        IntPtr lpParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool DestroyWindow(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern ushort RegisterClassEx(ref WndClassEx lpwcx);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern ushort UnregisterClass([MarshalAs(UnmanagedType.LPStr)] string lpClassName, IntPtr hInstance);

    [DllImport("user32.dll")]
    public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("user32.dll")]
    public static extern bool GetMessage(out Msg lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll")]
    public static extern bool TranslateMessage(ref Msg lpMsg);

    [DllImport("user32.dll")]
    public static extern IntPtr DispatchMessage(ref Msg lpMsg);

}

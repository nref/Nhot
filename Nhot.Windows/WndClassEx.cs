using System.Runtime.InteropServices;

namespace Nhot;

[StructLayout(LayoutKind.Sequential)]
public struct WndClassEx
{
    public uint cbSize;
    public uint style;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public User32.WndProc lpfnWndProc;
    public int cbClsExtra;
    public int cbWndExtra;
    public IntPtr hInstance;
    public IntPtr hIcon;
    public IntPtr hCursor;
    public IntPtr hbrBackground;
    [MarshalAs(UnmanagedType.LPStr)]
    public string lpszMenuName;
    [MarshalAs(UnmanagedType.LPStr)]
    public string lpszClassName;
    public IntPtr hIconSm;
}

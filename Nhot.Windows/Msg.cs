using System.Runtime.InteropServices;

namespace Nhot;

[StructLayout(LayoutKind.Sequential)]
public struct Msg
{
    public IntPtr hwnd;
    public uint message;
    public IntPtr wParam;
    public IntPtr lParam;
    public uint time;
    public Point pt;
}

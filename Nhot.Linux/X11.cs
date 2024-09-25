using System.Runtime.InteropServices;

public static class X11
{
    private const string X11Lib = "libX11.so.6";

    [DllImport(X11Lib)]
    public static extern nint XOpenDisplay(nint display);

    [DllImport(X11Lib)]
    public static extern int XDefaultRootWindow(nint display);

    [DllImport(X11Lib)]
    public static extern int XGrabKey(nint display, int keycode, uint modifiers, nint window, bool ownerEvents, int pointerMode, int keyboardMode);

    [DllImport(X11Lib)]
    public static extern int XUngrabKey(nint display, int keycode, uint modifiers, nint window);
    
    [DllImport(X11Lib)]
    public static extern int XKeysymToKeycode(nint display, nint keysym);

    [DllImport(X11Lib)]
    public static extern int XNextEvent(nint display, ref XEvent xevent);

    [DllImport(X11Lib)]
    public static extern int XSelectInput(nint display, nint window, long eventMask);
    
    [DllImport(X11Lib)]
    public static extern int XCloseDisplay(nint display);
    
    public delegate int XErrorHandler(IntPtr display, ref XErrorEvent errorEvent);

    [DllImport("libX11.so.6")]
    public static extern XErrorHandler XSetErrorHandler(XErrorHandler handler);

    [StructLayout(LayoutKind.Sequential)]
    public struct XEvent
    {
        public Event type;
        public XKeyEvent keyEvent;  // This will overlap with the rest of the event data
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XKeyEvent
    {
        public ulong serial;
        public bool send_event;
        public IntPtr display;
        public IntPtr window;
        public IntPtr root;
        public IntPtr subwindow;
        public ulong time;
        public int x, y;
        public int x_root, y_root;
        public uint state;
        public uint keycode;
        public bool same_screen;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct XErrorEvent
    {
        public int type;
        public IntPtr display;
        public ulong serial;
        public byte error_code;
        public byte request_code;
        public byte minor_code;
        public IntPtr resourceid;
    }

    public enum Event
    {
        KeyPress = 2,
        KeyRelease = 3,
    }

    public enum Modifier : uint
    {
        ModShift = 1 << 0,
        ModCtrl = 1 << 2,
        Mod1 = 1 << 3,
        Mod2 = 1 << 4,
        Mod3 = 1 << 5,
        Mod4 = 1 << 6,
        Mod5 = 1 << 7,
    }
    
    public const int KeyPressMask = 1;
    public const int GrabModeAsync = 1;
}
using System.Runtime.InteropServices;
using Nhot.Shared;

namespace Nhot.Linux;

public class X11HotkeyService : IHotkeyService
{
    // More keys at https://github.com/golang-design/hotkey/blob/main/hotkey_linux.go
    private const int SpaceKey = 0x0020; // The keycode for the space key

    public void Run(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            // var mod = (uint)X11.Modifier.ModShift | (uint)X11.Modifier.ModCtrl;
            var mod = (uint)X11.Modifier.ModCtrl;
            WaitHotkey(mod, SpaceKey, ct);
        }
    }
    
    private static bool WaitHotkey(uint mod, int key, CancellationToken ct)
    {
        if (!TryOpenDisplay(out nint display))
        {
            return false;
        }

        int keycode = X11.XKeysymToKeycode(display, key);
        nint rootWindow = X11.XDefaultRootWindow(display);

        X11.XErrorHandler previousHandler = X11.XSetErrorHandler(HandleError);
        
        X11.XGrabKey(display, keycode, mod, rootWindow, false, X11.GrabModeAsync, X11.GrabModeAsync);
        X11.XSelectInput(display, rootWindow, X11.KeyPressMask);

        var xev = new X11.XEvent();
        
        while (!ct.IsCancellationRequested)
        {
            X11.XNextEvent(display, ref xev);

            switch (xev.type)
            {
                case X11.Event.KeyPress:
                    HandleHotKeyPressed(xev.keyEvent.keycode);
                    continue;
                case X11.Event.KeyRelease:
                    //HandleHotKeyReleased(); // TODO
                    X11.XUngrabKey(display, keycode, mod, rootWindow);
                    X11.XCloseDisplay(display);
                    return true;
            }
        }
        
        return true;
    }

    private static bool TryOpenDisplay(out nint display)
    {
        display = nint.Zero;
        const int maxTries = 10;
        foreach (var _ in Enumerable.Range(0, maxTries))
        {
            display = X11.XOpenDisplay(nint.Zero);
            if (display != nint.Zero)
                return true;
        }

        return false;
    }
    
    private static int HandleError(IntPtr display, ref X11.XErrorEvent errorEvent)
    {
        if (errorEvent is { error_code: 10, request_code: 33 }) // BadAccess and X_GrabKey
        {
            Console.WriteLine("Error: Key combination already grabbed by another client.");
            return 0;
        }

        Console.WriteLine($"X11 Error: type={errorEvent.type}, " +
                          $"error_code={errorEvent.error_code}, " +
                          $"request_code={errorEvent.request_code}, " +
                          $"minor_code={errorEvent.minor_code}");
        return 0;
    }
    
    private static void HandleHotKeyPressed(uint keycode)
    {
        Console.WriteLine($"Hotkey was pressed: {keycode}");
    }
}
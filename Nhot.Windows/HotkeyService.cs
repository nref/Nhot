using System.Runtime.InteropServices;

namespace Nhot;

public class HotkeyService(VirtualKey Key, params KeyModifier[] ModifierKeys)
{
    private const int WM_HOTKEY = 0x0312;
    private const int HOTKEY_ID = 1;
    private string _className = "WhotClass";
    private nint _hWnd;

    private static nint WindowProc(nint hWnd, uint msg, nint wParam, nint lParam)
    {
        if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
        {
            HandleHotKeyPressed();
        }
        return User32.DefWindowProc(hWnd, msg, wParam, lParam);
    }

    public void Run(CancellationToken ct)
    {
        try
        {
            if (!InitializeClass()) { return; }
            if (!TryCreateWindow(out _hWnd)) { return; }
            if (!RegisterHotkey()) { return; }

            RunCore(ct);
        }
        finally
        {
            Cleanup();
        }
    }

    private static void RunCore(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested && User32.GetMessage(out Msg msg, nint.Zero, 0, 0))
        {
            User32.TranslateMessage(ref msg);
            User32.DispatchMessage(ref msg);
        }
    }

    private bool InitializeClass()
    {
        WndClassEx wndClass = new()
        {
            cbSize = (uint)Marshal.SizeOf(typeof(WndClassEx)),
            style = 0,
            lpfnWndProc = new User32.WndProc(WindowProc),
            cbClsExtra = 0,
            cbWndExtra = 0,
            hInstance = nint.Zero,
            hIcon = nint.Zero,
            hCursor = nint.Zero,
            hbrBackground = nint.Zero,
            lpszMenuName = null!,
            lpszClassName = _className,
            hIconSm = nint.Zero
        };

        bool ok = User32.RegisterClassEx(ref wndClass) != 0;

        if (!ok)
        {
            Console.WriteLine("Failed to register window class.");
        }

        return ok;
    }

    private bool TryCreateWindow(out nint hWnd)
    {
        hWnd = User32.CreateWindowEx(
            0,
            _className,
            _className,
            0,
            0, 0, 0, 0,
            nint.Zero,
            nint.Zero,
            nint.Zero,
            nint.Zero);

        bool ok = hWnd != nint.Zero;

        if (!ok)
        {
            Console.WriteLine("Failed to create window.");
        }

        return ok;
    }

    private bool RegisterHotkey()
    {
        // bitwise or all the modifier keys
        bool ok = User32.RegisterHotKey(_hWnd, HOTKEY_ID, ModifierKeys.Flatten(), (uint)Key);

        if (!ok)
        {
            Console.WriteLine("Failed to register hotkey.");
        }

        return ok;
    }

    private void Cleanup()
    {
        User32.UnregisterHotKey(_hWnd, HOTKEY_ID);
        User32.DestroyWindow(_hWnd);
        User32.UnregisterClass(_className, nint.Zero);
    }

    private static void HandleHotKeyPressed()
    {
        Console.WriteLine("Hotkey was pressed!");
    }
}

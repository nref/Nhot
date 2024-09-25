using System.Collections.Concurrent;
using SharpHook;
using SharpHook.Native;

namespace Nhot.Shared;

public class SharpHookHotkeyService : IHotkeyService
{ 
    private readonly ConcurrentDictionary<KeyCode, KeyCode> _keys = new();
    
    public void Run(CancellationToken ct)
    {
        var hook = new TaskPoolGlobalHook();
        hook.KeyPressed += HandleKeyPressed;
        hook.KeyReleased += HandleKeyReleased;
        
        hook.Run();
    }

    private void HandleKeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        if (!_keys.TryAdd(e.Data.KeyCode, e.Data.KeyCode))
        {
            return;
        }
        
        PrintKeys();
    }
    
    private void HandleKeyReleased(object? sender, KeyboardHookEventArgs e)
    {
        if (!_keys.TryRemove(e.Data.KeyCode, out _))
        {
            return;
        }
        
        PrintKeys();
    }

    private void PrintKeys()
    {
        Console.WriteLine($"Keys: {string.Join("+", _keys.Keys)}");
    }
}
using Nhot.Shared;

namespace Nhot.Linux;

class Program
{
    enum Backend
    {
        X11,
        SharpHook
    }
    
    static void Main(string[] args)
    {
        Backend backend = ParseBackend(args);
        
        IHotkeyService service = backend switch
        {
            Backend.SharpHook => new SharpHookHotkeyService(),
            _ => new X11HotkeyService(),
        };
        
        CancellationTokenSource cts = new();
        _ = Task.Run(() => service.Run(cts.Token), cts.Token);

        Console.WriteLine("Waiting for hotkey. Press Enter to exit.");
        Console.ReadLine();

        cts.Cancel();
    }

    private static Backend ParseBackend(string[] args) =>
        args switch
        {
            _ when args.Length == 0 => Backend.SharpHook,
            _ when args[0] == "sharphook" => Backend.SharpHook,
            _ when args[0] == "x11" => Backend.X11,
            _ => Backend.SharpHook,
        };
}
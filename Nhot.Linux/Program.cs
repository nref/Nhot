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
        var backend = args switch
        {
            _ when args.Length == 0 => Backend.SharpHook,
            _ when args[0] == "sharphook" => Backend.SharpHook,
            _ when args[0] == "x11" => Backend.X11,
            _ => Backend.SharpHook,
        };
        
        CancellationTokenSource cts = new();

        if (backend == Backend.SharpHook)
        {
            _ = Task.Run(() =>
                new SharpHookHotkeyService().Run(cts.Token), cts.Token);
        }
        else
        {
            _ = Task.Run(() =>
                new X11HotkeyService().Run(cts.Token), cts.Token);
        }

        Console.WriteLine("Waiting for hotkey. Press Enter to exit.");
        Console.ReadLine();

        cts.Cancel();
    }
}
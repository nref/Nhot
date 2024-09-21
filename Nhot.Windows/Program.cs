using Nhot;

CancellationTokenSource cts = new();

_ = Task.Run(() => 
    new WindowsHotkeyService(VirtualKey.Space, KeyModifier.Alt, KeyModifier.Ctrl)
        .Run(cts.Token), cts.Token);

Console.WriteLine("Waiting for hotkey. Press Enter to exit.");
Console.ReadLine();
cts.Cancel();
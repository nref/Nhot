using Nhot.Shared;

Console.WriteLine("Running...");

// Only macOS requires running on main thread
new SharpHookHotkeyService().Run(default);

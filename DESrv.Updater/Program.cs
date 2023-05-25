namespace Blusutils.DESrv.Updater;
internal class Program {
    private static void Main(string[] args) {
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Updater starting...");
        Console.ForegroundColor= ConsoleColor.White;
        Console.WriteLine("Checking versions...");
        var vers = Updater.CheckVersion(Versions.Versions.DESrvVersion);
        if (vers is null) {
            Console.WriteLine("You have the latest version of DESrv installed, no update is required!");
            Console.ForegroundColor = color;
            Environment.Exit(0);
        }
        Console.ForegroundColor = ConsoleColor.Cyan; 
        Console.WriteLine($"New version of DESrv is available: v{vers}...");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Downloading update...");
        try {
            Updater.Update();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Succefully downloaded!");
            Console.ForegroundColor = color;
        } catch (PlatformNotSupportedException) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: your platform is currently not supported!");
        } catch (HttpRequestException) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: failed to fetch update!");
        }
    }
}
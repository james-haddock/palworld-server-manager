using System;
using static SteamCmdService;
using System.IO;

public class Program
{
    private readonly IProcessManager processManager;
    private readonly IConsole console;
    private readonly VCRedistChecker vcRedistChecker;
    private readonly DirectXChecker directXChecker;

    public Program(IProcessManager processManager, IConsole console)
    {
        this.processManager = processManager;
        this.console = console;
        this.vcRedistChecker = new VCRedistChecker();
        this.directXChecker = new DirectXChecker();
    }

    public async Task InstallPalworldServer()
    {
        try
        {
            console.WriteLine("Starting the installer...");
            Log("Starting the installer...");

            console.WriteLine("Would you like to proceed with the installation? (y/n)");
            string proceedInput = console.ReadLine();
            if (proceedInput.ToLower() != "y")
            {
                console.WriteLine("Installation cancelled by user.");
                Log("Installation cancelled by user.");
                return;
            }

            await SteamCmdService.DownloadSteamCmd();

            processManager.StartProcess("steamcmd/steamcmd.exe");
            processManager.SendCommand("steamcmd +login anonymous +app_update 2394010 validate +quit");

            if (!vcRedistChecker.IsVCRedistInstalled())
            {
                console.WriteLine("Microsoft Visual C++ Runtime is not installed. Would you like to install it? (y/n)");
                string input = console.ReadLine();
                if (input.ToLower() == "y")
                {
                    var vcRedistPath = await ((InstallSteamApp)processManager).DownloadFile("https://download.microsoft.com/download/.../vc_redist.x64.exe");
                    ((InstallSteamApp)processManager).RunExecutable(vcRedistPath);
                    Log("Microsoft Visual C++ Runtime installed.");
                }
            }
            else
            {
                Log("Microsoft Visual C++ Runtime is already installed.");
            }

            if (!directXChecker.IsDirectXInstalled())
            {
                console.WriteLine("DirectX Runtime is not installed. Would you like to install it? (y/n)");
                string input = console.ReadLine();
                if (input.ToLower() == "y")
                {
                    var directXPath = await ((InstallSteamApp)processManager).DownloadFile("https://download.microsoft.com/download/.../dxwebsetup.exe");
                    ((InstallSteamApp)processManager).RunExecutable(directXPath);
                    Log("DirectX Runtime installed.");
                }
            }
            else
            {
                Log("DirectX Runtime is already installed.");
            }

            console.WriteLine("Installation complete.");
            Log("Installation complete.");
        }
        catch (Exception ex)
        {
            console.WriteLine($"An error occurred: {ex.Message}");
            Log($"An error occurred: {ex.Message}");
        }
    }

    private void Log(string message)
    {
        using (StreamWriter writer = new StreamWriter("log.txt", true))
        {
            writer.WriteLine($"[{DateTime.Now}] {message}");
        }
    }

    static void Main(string[] args)
    {
        IProcessManager processManager = new InstallSteamApp();
        IConsole console = new ConsoleWrapper();

        var program = new Program(processManager, console);
        program.InstallPalworldServer().GetAwaiter().GetResult();
    }
}
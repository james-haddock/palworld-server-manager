using System;
using static SteamCmdService;
using System.IO;
using System.Threading.Tasks;

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

            console.WriteLine("Would you like to download SteamCMD? (y/n)");
            string installSteamCMD = console.ReadLine();
            if (installSteamCMD.ToLower() == "y")
            {
                console.WriteLine("Downloading SteamCMD.");
                string steamDownload = await SteamCmdService.DownloadSteamCmd();
                console.WriteLine(steamDownload);
            }


            console.WriteLine("Would you like install Palworld Server? (y/n)");
            string palworldInstall = console.ReadLine();
            if (palworldInstall.ToLower() == "y")
            {
            console.WriteLine("Installing latest version of Palworld Server");
            processManager.StartProcess("steamcmd/steamcmd.exe", "+login anonymous +app_update 2394010 validate +quit");
            }

            if (!vcRedistChecker.IsVCRedistInstalled())
            {
                console.WriteLine("Microsoft Visual C++ Runtime is not installed. Would you like to install it? (y/n)");
                string input = console.ReadLine();
                if (input.ToLower() == "y")
                {
                    var vcRedistPath = await ((InstallSteamApp)processManager).DownloadFile("https://aka.ms/vs/17/release/vc_redist.x64.exe");
                    ((InstallSteamApp)processManager).RunExecutable(vcRedistPath);
                console.WriteLine("Once Visual C++ Runtime installation is complete press any button to continue.");
                console.ReadKey();
                }
            }
            else
            {
                Log("Microsoft Visual C++ Runtime is already installed.");
                console.WriteLine("Microsoft Visual C++ Runtime is already installed.");
            }

            string installDirectX;
            console.WriteLine("DirectX 9 Runtime needed to run Palworld Server. Would you like to install it? (y/n)");
            installDirectX = console.ReadLine();
            if (installDirectX.ToLower() == "y")
            {
                var directXPath = await ((InstallSteamApp)processManager).DownloadFile("https://download.microsoft.com/download/1/7/1/1718CCC4-6315-4D8E-9543-8E28A4E18C4C/dxwebsetup.exe");
                ((InstallSteamApp)processManager).RunExecutable(directXPath);
                console.WriteLine("Once DirectX 9 Runtime installation is complete press any button to continue.");
                console.ReadKey();
            }
            else
            {
                Log("DirectX 9 Runtime installation skipped");
                console.WriteLine("DirectX 9 Runtime installation skipped");
            }

            console.WriteLine("Installation complete.");
            Log("Installation complete.");
            console.WriteLine("Press any key to exit.");
            console.ReadKey();
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
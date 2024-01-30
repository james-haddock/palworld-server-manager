using System.Diagnostics;
using System.Threading.Tasks;


public class SteamCmdService
{
public async Task<string> DownloadSteamCmd()
{
    var startInfo = new ProcessStartInfo
    {
        FileName = "powershell.exe",
        Arguments = $"-Command \"& {{Invoke-WebRequest -Uri 'https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip' -OutFile 'steamcmd.zip'; Expand-Archive -Path 'steamcmd.zip' -DestinationPath '.'}}\"",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true,
    };

    using var process = Process.Start(startInfo);
    if (process != null)
    {
        using var reader = process.StandardOutput;
        string result = await reader.ReadToEndAsync();
        return result;
    }

    return "Error starting process";
}
}

using System.Net.Http;
using System.IO.Compression;

public class SteamCmdService
{
    public async Task<string> DownloadSteamCmd()
    {
        var httpClient = new HttpClient();
        var data = await httpClient.GetByteArrayAsync("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip");

        Console.WriteLine("Downloaded data length: " + data.Length);

        Directory.CreateDirectory("steamcmd");
        await File.WriteAllBytesAsync("steamcmd/steamcmd.zip", data);

        if (File.Exists("steamcmd/steamcmd.exe"))
        {
            throw new InvalidOperationException("SteamCMD is already installed.");
        }

        ZipFile.ExtractToDirectory("steamcmd/steamcmd.zip", "steamcmd");

        File.Delete("steamcmd/steamcmd.zip");

        return "Download and extraction complete";
    }
}
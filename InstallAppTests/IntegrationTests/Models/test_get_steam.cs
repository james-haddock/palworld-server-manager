using Xunit;
using System;
using System.IO;
using System.Threading.Tasks;

public class SteamCmdServiceTests : IDisposable
{

    [Fact]
    public async Task TestDownloadSteamCmd()
    {
        var service = new SteamCmdService();

        await service.DownloadSteamCmd();

        Assert.True(File.Exists("steamcmd/steamcmd.exe"));

        Assert.False(File.Exists("steamcmd/steamcmd.zip"));
    }

    public void Dispose()
    {
        if (Directory.Exists("steamcmd"))
        {
            Directory.Delete("steamcmd", true);
        }
    }
}
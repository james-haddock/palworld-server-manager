using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
public class ServerStatusCheckerTests
{
    private ServerStatusChecker _checker;
    private Mock<RCONService> _mockRconService;
    private Mock<ILogger<ServerControlService>> _mockLogger;

    [SetUp]
    public void SetUp()
    {
        _mockRconService = new Mock<RCONService>();
        _mockLogger = new Mock<ILogger<ServerControlService>>();
        _checker = new ServerStatusChecker(_mockRconService.Object, _mockLogger.Object);
    }

    [Test]
    public void TestServerStatus_WhenServerIsOnline()
    {
        _mockRconService.Setup(m => m.SendServerCommand("Info")).Returns(Task.FromResult("Server is online."));
        Thread.Sleep(1000); 
        Assert.AreEqual("Online", _checker.ServerStatus);
    }

    [Test]
    public void TestServerStatus_WhenServerIsOffline()
    {
        _mockRconService.Setup(m => m.SendServerCommand("Info")).Returns(Task.FromResult<string>(null));
        Thread.Sleep(1000);
        Assert.AreEqual("Offline", _checker.ServerStatus);
    }

    [Test]
    public void TestServerInfo_WhenServerIsOnline()
    {
        _mockRconService.Setup(m => m.SendServerCommand("Info")).Returns(Task.FromResult("Server info."));
        Thread.Sleep(1000); 
        Assert.AreEqual("Server info.", _checker.ServerInfo);
    }

    [Test]
    public void TestServerInfo_WhenServerIsOffline()
    {
        _mockRconService.Setup(m => m.SendServerCommand("Info")).Returns(Task.FromResult<string>(null));
        Thread.Sleep(1000); 
        Assert.AreEqual("Server info is not available.", _checker.ServerInfo);
    }

    [TearDown]
    public void TearDown()
    {
        _checker.StopChecking();
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Open.Nat;

public class UPnPPortForwarder
{
    private readonly NatDevice _natDevice;

    public UPnPPortForwarder()
    {
        var discoverer = new NatDiscoverer();
        var cts = new CancellationTokenSource();
        _natDevice = discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts).Result;
    }

    public async Task SetupPortForwarding(int internalPort, int externalPort)
    {
        if (_natDevice != null)
        {
            var externalIPAddress = await _natDevice.GetExternalIPAsync();

            if (externalIPAddress != null)
            {
                var mapping = new Mapping(Protocol.Tcp, internalPort, externalPort);
                await _natDevice.CreatePortMapAsync(mapping);

                Console.WriteLine($"Port forwarding set up successfully:");
                Console.WriteLine($"External IP: {externalIPAddress}");
                Console.WriteLine($"Internal Port: {internalPort}");
                Console.WriteLine($"External Port: {externalPort}");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Unable to retrieve external IP address.");
                                Console.ReadKey();
            }
        }
        else
        {
            Console.WriteLine("No UPnP-enabled device found on the network.");
                            Console.ReadKey();
        }
    }
}
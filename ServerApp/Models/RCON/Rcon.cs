using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class RCONService
    {
    private string _ipAddress;

    private string _port;
    private string _password;
    public string rconFilePath { get; set; }
    public string timeout { get; set; }

        public RCONService (string ipAddress, string port, string password, string rconPath, string timeoutDuration="10s"){
            _ipAddress = ipAddress;
            _password = password;
            timeout = timeoutDuration;
            rconFilePath = rconPath;
            _port = port;
        }

    public async Task<string> SendServerCommand(string command)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = rconFilePath,
                Arguments = $" -a {_ipAddress}:{_port} -p {_password} command {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        try
        {
            process.Start();

            string result = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"Error: {error}");
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }
    }
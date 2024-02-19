using System.Diagnostics;
using Microsoft.Extensions.Logging;

public class NginxLinux : IDisposable
{
    private readonly ILogger<Nginx> _logger;
    private Process? _nginxProcess;

    public NginxLinux(ILogger<Nginx> logger)
    {
        _logger = logger;

        var startInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = "-c \"sudo service nginx start\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        _nginxProcess = new Process { StartInfo = startInfo };

        _nginxProcess.OutputDataReceived += (sender, e) => _logger.LogInformation("NGINX output: {0}", e.Data);
        _nginxProcess.ErrorDataReceived += (sender, e) => _logger.LogError("NGINX error: {0}", e.Data);

        if (_nginxProcess.Start())
        {
            _logger.LogInformation("Started NGINX process.");
            _nginxProcess.BeginOutputReadLine();
            _nginxProcess.BeginErrorReadLine();
        }
        else
        {
            _logger.LogError("Failed to start NGINX process.");
        }
    }

    public void Dispose()
    {
        var stopInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = "-c \"sudo service nginx stop\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using var stopProcess = new Process { StartInfo = stopInfo };
        if (stopProcess.Start())
        {
            _logger.LogInformation("Stopped NGINX process.");
        }
        else
        {
            _logger.LogError("Failed to stop NGINX process.");
        }
    }
}
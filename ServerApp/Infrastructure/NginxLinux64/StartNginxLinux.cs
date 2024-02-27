using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.IO;

public class NginxLinux : IDisposable
{
    private readonly ILogger<Nginx> _logger;
    private Process? _nginxProcess;

    public NginxLinux(ILogger<Nginx> logger)
    {
        _logger = logger;

        var projectDirectory = System.IO.Directory.GetCurrentDirectory();
        var nginxConfPath = System.IO.Path.Combine(projectDirectory, "Infrastructure/NginxLinux64/nginx.conf");
        // var nginxLogPath = System.IO.Path.Combine(projectDirectory, "Infrastructure/NginxLinux64/error.log");

        var startInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"sudo nginx -p {projectDirectory} -c {nginxConfPath}\"",
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
            Arguments = "-c \"sudo nginx -s quit\"",
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

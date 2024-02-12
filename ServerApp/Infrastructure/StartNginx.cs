using System.Diagnostics;
using Microsoft.Extensions.Logging;

public class Nginx : IDisposable
{
    private readonly ILogger<Nginx> _logger;
    private Process? _nginxProcess;
    private int? _nginxProcessId;

    public Nginx(ILogger<Nginx> logger, string nginxPath = "./Infrastructure/nginx-1.24.0/nginx.exe")
    {
        _logger = logger;

        var startInfo = new ProcessStartInfo
        {
            FileName = nginxPath,
            WorkingDirectory = System.IO.Path.GetDirectoryName(nginxPath),
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
            _nginxProcessId = _nginxProcess.Id;
            _logger.LogInformation($"Started NGINX process with ID {_nginxProcess.Id}.");
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
        if (_nginxProcessId.HasValue)
        {
            var processToKill = Process.GetProcessById(_nginxProcessId.Value);
            if (processToKill != null && !processToKill.HasExited)
            {
                processToKill.Kill(true);
                processToKill.WaitForExit();
                _logger.LogInformation("Stopped NGINX process.");
            }
        }
    }
}
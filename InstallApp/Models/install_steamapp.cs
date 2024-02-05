using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

public class InstallSteamApp : IProcessManager
{
    private Process? process;
    private static readonly HttpClient httpClient = new HttpClient();

    public void StartProcess(string exePath, string arguments = "")
    {
        if (!File.Exists(exePath))
        {
            throw new FileNotFoundException($"The file {exePath} does not exist.");
        }

        process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        if (!process.Start())
        {
            throw new InvalidOperationException("Failed to start the process.");
        }

        Task.Run(() =>
        {
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }
        });
        process.WaitForExit();
    }

    public void SendCommand(string command)
    {
        if (process == null || process.HasExited)
        {
            throw new InvalidOperationException("Process not started or has already exited.");
        }

        process.StandardInput.WriteLine(command);
    }
    
    public async Task<string> DownloadFile(string url)
    {
        var fileName = System.IO.Path.GetFileName(new Uri(url).LocalPath);
        var localPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), fileName);

        using (var response = await httpClient.GetAsync(url))
        {
            response.EnsureSuccessStatusCode();
            await using (var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await response.Content.CopyToAsync(fileStream);
            }
        }

        return localPath;
    }

    public void RunExecutable(string filePath, bool noWindow=true)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file {filePath} does not exist.");
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true,
                CreateNoWindow = noWindow,
            }
        };

        if (!process.Start())
        {
            throw new InvalidOperationException("Failed to start the process.");
        }

    }
    
}
using System.Diagnostics;

public class Nginx
{
    Process ?nginxProcess;
    public Nginx(string nginxPath="Infrastructure/nginx-1.24.0/nginx.exe")
    {
        string _nginxPath = nginxPath;
        var startInfo = new ProcessStartInfo
        {
            FileName = _nginxPath,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };
        nginxProcess = Process.Start(startInfo);
    }
}
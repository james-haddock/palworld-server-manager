public interface IProcessManager
{
    void StartProcess(string processPath, string arguments);
    void SendCommand(string command);
    Task<string> DownloadFile(string url);
    void RunExecutable(string filePath, bool noWindow);
}
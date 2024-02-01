public interface IProcessManager
{
    void StartProcess(string processPath);
    void SendCommand(string command);
    Task<string> DownloadFile(string url);
    void RunExecutable(string filePath);
}
using Microsoft.Win32;

public class DirectXChecker
{
    public bool IsDirectXInstalled()
    {
        using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\DirectX"))
        {
            return key != null;
        }
    }
}
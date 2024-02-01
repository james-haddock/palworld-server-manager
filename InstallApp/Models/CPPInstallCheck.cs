using Microsoft.Win32;

public class VCRedistChecker
{
    public bool IsVCRedistInstalled()
    {
        using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\Installer\Products"))
        {
            if (key != null)
            {
                foreach (string subkeyName in key.GetSubKeyNames())
                {
                    using (var subkey = key.OpenSubKey(subkeyName))
                    {
                        if (subkey != null)
                        {
                            var productName = subkey.GetValue("ProductName") as string;
                            if (productName != null && productName.StartsWith("Microsoft Visual C++"))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
}
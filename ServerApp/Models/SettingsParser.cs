using System.IO.Abstractions;

public class IniSettingsService
{
    private readonly string _filePath;
    private readonly IFileSystem _fileSystem;
    private List<KeyValuePair<string, string>> _settings;

    public IniSettingsService(string filePath, IFileSystem fileSystem)
    {
        _filePath = filePath;
        _fileSystem = fileSystem;
        _settings = ParseIniFile();
    }

    private List<KeyValuePair<string, string>> ParseIniFile()
    {
        var lines = _fileSystem.File.ReadAllLines(_filePath);
        var settingsLine = lines.FirstOrDefault(line => line.StartsWith("OptionSettings="));
        if (settingsLine == null)
        {
            throw new Exception("OptionSettings not found in the INI file.");
        }

        var settingsString = settingsLine.Substring("OptionSettings=(".Length, settingsLine.Length - "OptionSettings=(".Length - 1);
        var settingsPairs = settingsString.Split(',');

        var settings = new List<KeyValuePair<string, string>>();
        foreach (var pair in settingsPairs)
        {
            var keyValue = pair.Split('=');
            if (keyValue.Length != 2)
            {
                throw new Exception($"Invalid key-value pair: {pair}");
            }

            settings.Add(new KeyValuePair<string, string>(keyValue[0], keyValue[1]));
        }

        return settings;
    }

    public List<KeyValuePair<string, string>> GetServerSettings()
    {
        return new List<KeyValuePair<string, string>>(_settings);
    }

    public string GetSetting(string key)
    {
        var setting = _settings.FirstOrDefault(kv => kv.Key == key);
        if (setting.Equals(default(KeyValuePair<string, string>)))
        {
            throw new Exception($"Key not found: {key}");
        }

        return setting.Value;
    }

    public void UpdateSetting(string key, string value)
    {
        var index = _settings.FindIndex(kv => kv.Key == key);
        if (index == -1)
        {
            throw new Exception($"Key not found: {key}");
        }

        _settings[index] = new KeyValuePair<string, string>(key, value);
    }

    public void SaveSettings()
    {
        var settingsString = string.Join(",", _settings.Select(kv => $"{kv.Key}={kv.Value}"));
        var settingsLine = $"OptionSettings=({settingsString})";

        var lines = _fileSystem.File.ReadAllLines(_filePath);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("OptionSettings="))
            {
                lines[i] = settingsLine;
                break;
            }
        }

        _fileSystem.File.WriteAllLines(_filePath, lines);
    }
}

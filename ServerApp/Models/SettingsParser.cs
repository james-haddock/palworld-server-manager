using IniParser;
using IniParser.Model;

public class SettingsParser
{
    private IniData _data;

    public SettingsParser(string settingsPath)
    {
        var parser = new FileIniDataParser();
        _data = parser.ReadFile(settingsPath);
    }

    public string GetSetting(string section, string key)
    {
        return _data[section][key];
    }

    public void SetSetting(string section, string key, string value)
    {
        _data[section][key] = value;
    }

    public IniData GetAllSettings()
    {
        return _data;
    }

    public void UpdateAllSettings(IniData newSettings)
    {
        _data = newSettings;
    }

    public void SaveSettings(string settingsPath)
    {
        var parser = new FileIniDataParser();
        parser.WriteFile(settingsPath, _data);
    }
}
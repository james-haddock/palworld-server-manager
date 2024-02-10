[ExtendObjectType("Mutation")]
public class ServerSettingsMutation
{
    private readonly IniSettingsService _iniSettingsService;

    public ServerSettingsMutation(IniSettingsService iniSettingsService)
    {
        _iniSettingsService = iniSettingsService;
    }

    public bool UpdateServerSettings(string key, string value)
    {
        _iniSettingsService.UpdateSetting(key, value);
        _iniSettingsService.SaveSettings();
        return true;
    }
}
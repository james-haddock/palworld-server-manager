[ExtendObjectType("Query")]

public class ServerSettingsQuery
{
    private readonly ILogger<ServerSettingsQuery> _logger;
    private readonly IniSettingsService _iniSettingsService;

    public ServerSettingsQuery(ILogger<ServerSettingsQuery> logger, IniSettingsService iniSettingsService)
    {
        _logger = logger;
        _iniSettingsService = iniSettingsService;
    }

    public List<KeyValuePair<string, string>> getAllSettings()
    {
        try
        {
            return _iniSettingsService.GetServerSettings();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting server settings");
            throw;
        }
    }

    public KeyValuePair<string, string> getSetting(string key)
    {
        try
        {
            string value = _iniSettingsService.GetSetting(key);
            return new KeyValuePair<string, string>(key, value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting the server setting with key {key}");
            throw;
        }
    }
}

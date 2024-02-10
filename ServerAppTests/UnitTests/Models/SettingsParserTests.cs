using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

public class IniSettingsServiceTests
{
    [Fact]
    public void GetServerSettings_ReturnsSettings()
    {
        var mockFileData = new MockFileData("OptionSettings=(key1=value1,key2=value2)");
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"C:\test.ini", mockFileData }
        });
        var service = new IniSettingsService(@"C:\test.ini", mockFileSystem);

        var settings = service.GetServerSettings();

        Assert.Equal(2, settings.Count);
        Assert.Contains(new KeyValuePair<string, string>("key1", "value1"), settings);
        Assert.Contains(new KeyValuePair<string, string>("key2", "value2"), settings);
    }

    [Fact]
    public void UpdateSetting_UpdatesSetting()
    {
        var mockFileData = new MockFileData("OptionSettings=(key1=value1,key2=value2)");
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"C:\test.ini", mockFileData }
        });
        var service = new IniSettingsService(@"C:\test.ini", mockFileSystem);

        service.UpdateSetting("key1", "updatedValue");

        var settings = service.GetServerSettings();
        Assert.Contains(new KeyValuePair<string, string>("key1", "updatedValue"), settings);
    }

    [Fact]
    public void ParseIniFile_WhenFileIsValid_ReturnsSettings()
    {
        var mockFileData = new MockFileData("OptionSettings=(key1=value1,key2=value2)");
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"C:\test.ini", mockFileData }
        });
        var service = new IniSettingsService(@"C:\test.ini", mockFileSystem);

        var settings = service.GetServerSettings();

        Assert.Equal(2, settings.Count);
        Assert.Contains(new KeyValuePair<string, string>("key1", "value1"), settings);
        Assert.Contains(new KeyValuePair<string, string>("key2", "value2"), settings);
    }

    [Fact]
    public void UpdateSetting_WhenKeyExists_UpdatesValue()
    {
        var mockFileData = new MockFileData("OptionSettings=(key1=value1,key2=value2)");
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"C:\test.ini", mockFileData }
        });
        var service = new IniSettingsService(@"C:\test.ini", mockFileSystem);

        service.UpdateSetting("key1", "updatedValue");

        var settings = service.GetServerSettings();
        Assert.Contains(new KeyValuePair<string, string>("key1", "updatedValue"), settings);
    }

    [Fact]
    public void SaveSettings_UpdatesFile()
    {
        var mockFileData = new MockFileData("OptionSettings=(key1=value1,key2=value2)");
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"C:\test.ini", mockFileData }
        });
        var service = new IniSettingsService(@"C:\test.ini", mockFileSystem);

        service.UpdateSetting("key1", "updatedValue");
        service.SaveSettings();

        var fileData = mockFileSystem.GetFile(@"C:\test.ini").TextContents;
        Assert.Contains("key1=updatedValue", fileData);
    }
}
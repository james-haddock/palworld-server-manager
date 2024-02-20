using System;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string os = Environment.OSVersion.Platform.ToString().ToLower();

string rconIp = configuration[$"{os}:RCON_IP"];
string rconPort = configuration[$"{os}:RCON_PORT;"];
string rconPassword = configuration[$"{os}:RCON_PASSWORD;"];
string rconPath = configuration[$"{os}:RCON_PATH;"];

if (os == "linux")
{
    builder.Services.AddSingleton<Nginx>();
}
else
{
    builder.Services.AddSingleton<NginxLinux>();
}

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services
    .AddSingleton<IFileSystem, FileSystem>()
    .AddSingleton<IniSettingsService>(sp =>
        new IniSettingsService("./DefaultPalWorldSettings.ini", sp.GetRequiredService<IFileSystem>()))
    .AddSingleton<ServerControlService>()
    .AddSingleton<ServerStatusChecker>()
    .AddSingleton<RCONService>(sp =>
        new RCONService(rconIp, rconPort, rconPassword, rconPath))
    .AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
        .AddTypeExtension<ServerSettingsQuery>()
        .AddTypeExtension<ServerStatusQuery>()
    .AddMutationType(d => d.Name("Mutation"))
        .AddTypeExtension<ServerSettingsMutation>()
        .AddTypeExtension<ServerControlMutation>()
        .AddTypeExtension<MutationRCON>()
    .AddType<ServerSetting>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapGraphQL();

var nginx = app.Services.GetRequiredService<Nginx>();

app.Run();

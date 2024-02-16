using System.IO.Abstractions;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);



builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services
    .AddSingleton<IFileSystem, FileSystem>()
    .AddSingleton<IniSettingsService>(sp =>
        new IniSettingsService("./DefaultPalWorldSettings.ini", sp.GetRequiredService<IFileSystem>()))
    .AddSingleton<ServerControlService>()
    .AddSingleton<ServerStatusChecker>()
    .AddSingleton<RCONService>(sp =>
        new RCONService("localhost", "25575", "test0303", "./Models/RCON/Console/rcon.exe"))
    .AddSingleton<Nginx>()
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
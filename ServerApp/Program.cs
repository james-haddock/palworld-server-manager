using System.IO.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services
    .AddSingleton<IFileSystem, FileSystem>()
    .AddSingleton<IniSettingsService>(sp => 
        new IniSettingsService("./DefaultPalWorldSettings.ini", sp.GetRequiredService<IFileSystem>()))
    .AddSingleton<ServerControlService>()
    .AddSingleton<RCONService>(sp => 
        new RCONService("localhost", "25575", "test0908", "/ServerApp/Models/RCON/Console/rcon.exe"))
    .AddGraphQLServer()
    .AddQueryType<ServerSettingsQuery>()
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

app.Run();
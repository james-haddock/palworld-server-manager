using System.IO.Abstractions;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string os = Environment.OSVersion.Platform.ToString().ToLower();

string rconIp = configuration["RCON:IpAddress"] ?? throw new Exception("RCON:IpAddress is not set in configuration");
string rconPort = configuration["RCON:Port"] ?? throw new Exception("RCON:Port is not set in configuration");
string rconPassword = configuration["RCON:Password"] ?? throw new Exception("RCON:Password is not set in configuration");
string rconPath = configuration["RCON:Path"] ?? throw new Exception("RCON:Path is not set in configuration");

if (os == "linux")
{
    builder.Services.AddSingleton<NginxLinux>();
}
else
{
    builder.Services.AddSingleton<Nginx>();
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
        new RCONService(rconIp, rconPort, rconPassword, rconPath));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
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
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.MapGraphQL();

var nginx = app.Services.GetRequiredService<Nginx>();

app.Run();

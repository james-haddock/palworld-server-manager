using System.IO.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string os = Environment.OSVersion.Platform.ToString().ToLower() switch
{
    "win32nt" => "windows",
    "unix" => "linux",
    _ => throw new Exception("Unsupported OS platform"),
};

string rconIp = configuration[$"{os}:RCON_IP"] ?? throw new Exception("RCON:IpAddress is not set in configuration");
string rconPort = configuration[$"{os}:RCON_PORT"] ?? throw new Exception("RCON:Port is not set in configuration");
string rconPassword = configuration[$"{os}:RCON_PASSWORD"] ?? throw new Exception("RCON:Password is not set in configuration");
string rconPath = configuration[$"{os}:RCON_PATH"] ?? throw new Exception("RCON:Path is not set in configuration");
string serverExecutablePath = configuration[$"{os}:SERVER_PATH"] ?? throw new Exception("SERVER_PATH is not set in configuration");

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
        .AddSingleton<ServerControlService>(sp =>
              new ServerControlService(serverExecutablePath, sp.GetRequiredService<RCONService>(), sp.GetRequiredService<ILogger<ServerControlService>>()))
    .AddSingleton<ServerStatusChecker>()
    .AddSingleton<RCONService>(sp =>
        new RCONService(rconIp, rconPort, rconPassword, rconPath));

builder.Services
    .AddDbContext<AppDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection")))
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IAuthService, AuthService>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
        .AddTypeExtension<ServerSettingsQuery>()
        .AddTypeExtension<ServerStatusQuery>()
    .AddMutationType(d => d.Name("Mutation"))
        .AddTypeExtension<ServerSettingsMutation>()
        .AddTypeExtension<ServerControlMutation>()
        .AddTypeExtension<MutationRCON>()
        .AddTypeExtension<UserAuthMutation>()
    .AddType<ServerSetting>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// app.UseAuthentication();
// app.UseAuthorization();
app.UseRouting();
app.MapGraphQL();


if (os == "linux")
{
    var nginx = app.Services.GetRequiredService<NginxLinux>();
}
else
{
    var nginx = app.Services.GetRequiredService<Nginx>();
}

app.Run();

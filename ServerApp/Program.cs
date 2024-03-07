using System.IO.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

builder.Services.AddControllersWithViews();
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "wwwroot";
});

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();

    dbContext.Database.EnsureCreated();

    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }

    await SeedData.Initialize(services);
}


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.MapGraphQL();

app.UseStaticFiles();
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "wwwroot";
});

app.MapFallbackToFile("index.html");

app.Run();

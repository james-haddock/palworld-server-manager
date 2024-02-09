using System.IO.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services
    .AddSingleton<IFileSystem, FileSystem>()
    .AddSingleton<IniSettingsService>(sp => 
        new IniSettingsService("./DefaultPalWorldSettings.ini", sp.GetRequiredService<IFileSystem>()))
    .AddGraphQLServer()
    .AddQueryType<ServerSettingsQuery>()
    .AddMutationType(d => d.Name("Mutation"))
        .AddTypeExtension<ServerSettingsMutation>()
        .AddTypeExtension<ServerControlMutation>()
    .AddType<ServerSetting>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.Run();
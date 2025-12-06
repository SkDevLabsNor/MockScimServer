using MockScimServer.Api.Metadata;
using MockScimServer.Api.Users;
using MockScimServer.Application.Services;
using MockScimServer.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserRepository>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var path = config.GetValue<string>("UsersFile") ?? "users.json";
    return new FileUserRepository(path);
});

builder.Services.AddScoped<UserService>();

var app = builder.Build();

app.MapMetadataEndpoints();
app.MapUserEndpoints();

app.Run();

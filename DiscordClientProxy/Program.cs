using DiscordClientProxy;
using DiscordClientProxy.StartupTasks;

await StartupTasks.Run();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});
builder.Services.AddControllers();

var app = builder.Build();
app.UseCors();
app.MapControllers();

if (Configuration.Instance.Debug.ClientEnvProxyUrl != null)
    app.MapGet("/api/_fosscord/v1/global_env", async context =>
    {
        var client = new HttpClient();
        var response = await client.GetAsync(Configuration.Instance.Debug.ClientEnvProxyUrl);
        var content = await response.Content.ReadAsStringAsync();
        await context.Response.WriteAsync(content);
    });

app.Run();
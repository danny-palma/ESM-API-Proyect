using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata.Ecma335;
using System.Timers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

ESM_API_SERVER.Globales.UpdateCache();
System.Timers.Timer TimerCache = new System.Timers.Timer(7200000);
TimerCache.Elapsed += ESM_API_SERVER.Globales.UpdateCache;
TimerCache.AutoReset = true;
TimerCache.Enabled = true;

app.Run();

namespace ESM_API_SERVER
{
    public static class Globales
    {
        public static VersionsInformation[] VersionArrayCache = Array.Empty<VersionsInformation>();
        public static IConfigurationRoot Configuration = new ConfigurationBuilder()
            .AddJsonFile($"{Environment.GetEnvironmentVariable("HOMEPATH")}\\Versions-ESM.json", false, true).Build();

        public static void UpdateCache(Object? source = null, ElapsedEventArgs? e = null)
        {
            Console.WriteLine($"{DateTime.Now}: Cached!");
            VersionArrayCache = Configuration.GetSection("Versions").Get<VersionsInformation[]>();

            for (int i = 0; i < VersionArrayCache.Length; i++)
            {
                var StreamRead = new StreamReader(VersionArrayCache[i].CodePowershell);
                VersionArrayCache[i].CodePowershell = StreamRead.ReadToEnd();
                StreamRead.Close();
            }
            return;
        }
    }
}
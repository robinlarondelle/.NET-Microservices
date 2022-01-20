using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.gRPC;
using PlatformService.SyncDataServices.HTTP;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment env = builder.Environment;

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddGrpc();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (env.IsProduction())
    {
        Console.WriteLine("--> Running in Production mode. Connecting to MSSQL");
        options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformServiceSQLServer"));
    }
    else
    {
        Console.WriteLine("--> Running in Development mode. Starting InMem Database");
        options.UseInMemoryDatabase("InMem");
    }
});
// DI

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

// Configuration
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/protos/platforms.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
});

DemoDb.PrepPopulation(app, env.IsProduction());

app.Run(); 
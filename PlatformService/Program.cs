using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.HTTP;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("InMem");
});
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

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

DemoDb.PrepPopulation(app);

Console.WriteLine($"Command Service endpoint: {builder.Configuration["CommandService"]}");

app.Run();
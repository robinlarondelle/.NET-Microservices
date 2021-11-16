using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Datal;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();  
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("InMem");
});
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

// Configuration
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

DemoDb.PrepPopulation(app);

app.Run();

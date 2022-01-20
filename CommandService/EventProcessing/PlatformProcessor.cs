using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dto;
using CommandService.Models;

namespace CommandService.EventProcessing;

public class PlatformProcessor : IPlatformProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public PlatformProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }
    
    public void AddPlatform(string platformPublishedMessage)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetService<ICommandRepository>();
        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

        try
        {
            var platform = _mapper.Map<Platform>(platformPublishedDto);

            if (repo!.PlatformExists(platform.Id))
            {
                Console.WriteLine($"--> Platform {platform.Name} already exists");
                return;
            }
            
            repo.CreatePlatform(platform);
            repo.SaveChanges();
            
            Console.WriteLine($"--> Created Platform {platform.Name}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not add platform to Database: {e.Message}");
        }
    }
}
using System.Text;
using System.Text.Json;
using PlatformService.DTO;

namespace PlatformService.SyncDataServices.HTTP;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public HttpCommandDataClient(
        HttpClient client,
        IConfiguration configuration)
    {
        _client = client;
        _configuration = configuration;
    }
    
    public async Task SendPlatformToCommand(PlatformReadDto platformReadDto)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platformReadDto),
            Encoding.UTF8,
            "application/json"
        );

        HttpResponseMessage response = await _client.PostAsync(_configuration["CommandService"], httpContent);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("sync post to command service was OK");
        }
        else
        {
            Console.WriteLine("sync post to command service was not OK");
            Console.Write(response);
        }
    }
}
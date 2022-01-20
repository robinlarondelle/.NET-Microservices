using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.SyncDataServices.gRPC;

public class PlatformDataClient : IPlatformDataClient
{
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public PlatformDataClient(IConfiguration config, IMapper mapper)
    {
        _config = config;
        _mapper = mapper;
    }
    
    public IEnumerable<Platform>? ReturnAllPlatforms()
    {
        Console.WriteLine($"--> Calling gRPC Service {_config["GrpcPlatform"]}");
        GrpcChannel channel = GrpcChannel.ForAddress(_config["GrpcPlatform"]);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();

        try
        {
            var reply = client.GetAllPlatforms(request);
            return _mapper.Map<IEnumerable<Platform>>(reply);
        }
        catch (Exception e)
        {
            Console.WriteLine("--> Could not call gRPC server");
            Console.WriteLine(e);
            return null;
        }
    }
}
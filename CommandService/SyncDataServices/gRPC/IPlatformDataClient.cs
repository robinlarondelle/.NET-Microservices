using CommandService.Models;

namespace CommandService.SyncDataServices.gRPC;

public interface IPlatformDataClient
{
    IEnumerable<Platform>? ReturnAllPlatforms();
}
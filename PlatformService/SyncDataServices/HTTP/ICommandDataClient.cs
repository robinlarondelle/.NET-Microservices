using PlatformService.DTO;

namespace PlatformService.SyncDataServices.HTTP;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto platformReadDto);
}
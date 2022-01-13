using AutoMapper;
using CommandService.Dto;
using CommandService.Models;

namespace CommandService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        // Source -> Target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandWriteDto, Command>();
        CreateMap<Command, CommandReadDto>();
    }
}
using AutoMapper;
using CommandService.Dto;
using CommandService.Models;
using PlatformService;

namespace CommandService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        // Source -> Target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandWriteDto, Command>();
        CreateMap<Command, CommandReadDto>();
        
        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(dest => 
                dest.ExternalId, opt => 
                    opt.MapFrom(src => src.Id));
        
        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(dest => dest.ExternalId, opt =>
                opt.MapFrom(src => src.PlatformId));
    }
}
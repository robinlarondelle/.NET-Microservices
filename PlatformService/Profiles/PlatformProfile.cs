using AutoMapper;
using PlatformService.DTO;
using PlatformService.Models;

namespace PlatformService.Profiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        // source -> target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformWriteDto, Platform>();
        CreateMap<PlatformReadDto, PlatformPublishedDto>();
        CreateMap<Platform, GrpcPlatformModel>()
            .ForMember(dest => dest.PlatformId, opt =>
                opt.MapFrom(src => src.Id));
    }
}
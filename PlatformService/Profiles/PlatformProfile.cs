using AutoMapper;
using PlatformService.DTO;
using PlatformService.Models;

namespace PlatformService.Profiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        // source -> target
        CreateMap<Platform, PlatformReadDTO>();
        CreateMap<PlatformWriteDTO, Platform>();
    }
}
using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.SyncDataServices.gRPC;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepository _repo;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var response = new PlatformResponse();
        List<Platform> platforms = _repo.GetAllPlatforms().ToList();

        platforms.ForEach(p => response.Platforms.Add(_mapper.Map<GrpcPlatformModel>(p)));

        return Task.FromResult(response);
    }
}
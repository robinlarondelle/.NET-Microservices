using AutoMapper;
using CommandService.Data;
using CommandService.Dto;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformController : ControllerBase
{
    private readonly ICommandRepository _repo;
    private readonly IMapper _mapper;

    public PlatformController(ICommandRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("--> Getting all platforms from CommandService");
        IEnumerable<Platform> platforms = _repo.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }
    
    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("request received");
        return Ok("request received");
    }
}
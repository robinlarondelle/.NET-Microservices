using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTO;
using PlatformService.Models;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformController : ControllerBase
{
    private readonly IPlatformRepository _repo;
    private readonly IMapper _mapper;

    public PlatformController(IPlatformRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        IEnumerable<Platform> platforms = _repo.GetAllPlatforms();

        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    [HttpGet("{id:int}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        Platform? platform = _repo.GetPlatformById(id);

        if (platform is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PlatformReadDto>(platform));
    }

    [HttpPost]
    public ActionResult<PlatformReadDto> CreatePlatform(PlatformWriteDto platformToCreate)
    {
        var platform = _mapper.Map<Platform>(platformToCreate);

        _repo.CreatePlatform(platform);
        _repo.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
        return CreatedAtRoute(nameof(GetPlatformById), new { platformReadDto.Id }, platformReadDto);
    }
}
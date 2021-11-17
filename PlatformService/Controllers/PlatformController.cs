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
    public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms()
    {
        IEnumerable<Platform> platforms = _repo.GetAllPlatforms();

        return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platforms));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDTO> GetPlatformById(int id)
    {
        Platform? platform = _repo.GetPlatformById(id);

        if (platform is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PlatformReadDTO>(platform));
    }

    [HttpPost]
    public ActionResult<PlatformReadDTO> CreatePlatform(PlatformWriteDTO platformToCreate)
    {
        Platform platform = _mapper.Map<Platform>(platformToCreate);

        _repo.CreatePlatform(platform);
        _repo.SaveChanges();

        PlatformReadDTO platformReadDTO = _mapper.Map<PlatformReadDTO>(platform);
        return CreatedAtRoute(nameof(GetPlatformById), new { platformReadDTO.Id }, platformReadDTO);
    }
}
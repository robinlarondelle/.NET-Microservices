using AutoMapper;
using CommandService.Data;
using CommandService.Dto;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/{platformId}/[controller]")]
[ApiController]
public class CommandController : ControllerBase
{
    private readonly ICommandRepository _repo;
    private readonly IMapper _mapper;

    public CommandController(ICommandRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    
    [HttpGet("{commandId:int}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine("--> Getting a single Command from a Platform");
        
        if (!_repo.PlatformExists(platformId))
        {
            return NotFound();
        }

        Command? command = _repo.GetCommand(platformId, commandId);

        if (command is null)
        {
            return NotFound();
        }
        
        return Ok(_mapper.Map<CommandReadDto>(command));
    }

    [HttpGet]
    public ActionResult<IEnumerable<Command>> GetAllCommandsForPlatform(int platformId)
    {
        Console.WriteLine("--> Getting all Commands of a Platform");
        
        if (!_repo.PlatformExists(platformId))
        {
            return NotFound();
        }

        IEnumerable<Command> commands = _repo.GetCommandsForPlatform(platformId);
        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
   }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandWriteDto commandToCreate)
    {
        Console.WriteLine("--> Creating new Command for Platform");
        
        if (!_repo.PlatformExists(platformId))
        {
            return NotFound();
        }

        var command = _mapper.Map<Command>(commandToCreate);
        
        _repo.CreateCommand(platformId, command);
        _repo.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);
        
        return CreatedAtRoute(
            nameof(GetCommandForPlatform), 
            new
                {
                    platformId,
                    commandId = commandReadDto.Id
                },
            commandReadDto);
    }
}
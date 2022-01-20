using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dto;
using CommandService.Models;

namespace CommandService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IPlatformProcessor _platformPublishedProcessor;

    public EventProcessor(IPlatformProcessor platformPublishedProcessor)
    {
        _platformPublishedProcessor = platformPublishedProcessor;
    }
    
    public void ProcessEvent(string message)
    {
        EventType eventType = GetEventType(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                _platformPublishedProcessor.AddPlatform(message);
                break;
            case EventType.Undetermined:
            default:
                Console.WriteLine("--> EventType not supported");
                break;
        }
    }

    private static EventType GetEventType(string message)
    {
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(message); // fetches message.event
        Console.WriteLine($"--> Received {eventType!.Event} EventType");

        return eventType!.Event switch
        {
            "Platform_Published" => EventType.PlatformPublished,
            _ => EventType.Undetermined
        };
    }
}
namespace CommandService.EventProcessing;

public interface IPlatformProcessor
{
    void AddPlatform(string platformPublishedMessage);
}
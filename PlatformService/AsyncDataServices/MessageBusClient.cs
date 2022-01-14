using System.Text;
using System.Text.Json;
using PlatformService.DTO;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _config;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _exchange;

    public MessageBusClient(IConfiguration config)
    {
        _config = config;
        _exchange = _config["RabbitMqExchange"];
        
        var factory = new ConnectionFactory()
        {
            HostName = _config["RabbitMqHost"],
            Port = int.Parse(_config["RabbitMqPort"])
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_exchange, ExchangeType.Fanout);
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            
            Console.WriteLine("--> Connected to Rabbit MQ");
        }
        catch (Exception e)
        {
            Console.WriteLine("--> Something went wrong when creating connection to Rabbit MQ");
            Console.WriteLine(e);
            throw;
        }
        
    }

    private static void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> Connection to Rabbit MQ Shutdown");
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        var message = JsonSerializer.Serialize(platformPublishedDto);

        if (_connection.IsOpen)
        {
            SendMessage(message);
        }
        
    }

    public void Dispose()
    {
        if (!_channel.IsOpen) return;
        _channel.Close();
        _connection.Close();
    }
    
    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(
            _exchange,
            routingKey: "",
            basicProperties:null,
            body);
        Console.WriteLine("--> Send message");
    }

}
using System.Text;
using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly IEventProcessor _eventProcessor;
    private IConnection? _connection;
    private IModel? _channel;
    private string? _queueName;

    public MessageBusSubscriber(IConfiguration config, IEventProcessor eventProcessor)
    {
        _config = config;
        _eventProcessor = eventProcessor;
        
        InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _config["RabbitMqHost"],
            Port = int.Parse(_config["RabbitMqPort"])
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: _config["RabbitMqExchange"], type: ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        
        _channel.QueueBind(
            _queueName,
            exchange: _config["RabbitMqExchange"],
            routingKey: "");
        
        Console.WriteLine($"--> Connected to RabbitMQ");

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

    }

    private static void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> Connection Shutdown");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (moduleHandle, ea) =>
        {
            Console.WriteLine("--> Event received");
            
            ReadOnlyMemory<byte> body = ea.Body;
            string notificationMessage = Encoding.UTF8.GetString(body.ToArray());
            _eventProcessor.ProcessEvent(notificationMessage);
        };

        _channel.BasicConsume(_queueName, autoAck: true, consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        if (_channel is { IsOpen: true })
        {
            _channel.Close();
            _connection?.Close();
        }
        
        base.Dispose();
    }
}
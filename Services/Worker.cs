using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace bookinghandlerAPI.Services;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IConnection _connection;
    private IBookingRepository _repository;
    public Worker(ILogger<Worker> logger, IBookingRepository repository, IConfiguration configuration)
    {
        _logger = logger;
        _repository = repository;

        var mqhost = configuration["TaxaBookingBrokerHost"];

        //var factory = new ConnectionFactory() { HostName = "localhost" };
        //var factory = new ConnectionFactory() { HostName = "172.17.0.2" };
        var factory = new ConnectionFactory() { HostName = mqhost };
        _connection = factory.CreateConnection();
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _connection.CreateModel();
        channel.QueueDeclare(queue: "hello",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
        
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            Booking? booking = JsonSerializer.Deserialize<Booking>(message);

            if(booking != null)
            {
                _repository.Put(booking);

                Console.WriteLine(" [x] Received {0}", message);
            }
        };

        channel.BasicConsume(queue: "hello",
                                autoAck: true,
                                consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await Task.Delay(1000, stoppingToken);
        }
    }
}

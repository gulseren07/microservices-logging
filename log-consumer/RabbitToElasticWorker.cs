using System.Text;
using System.Text.Json;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace log_consumer
{
    public class RabbitToElasticWorker : BackgroundService
    {
        private readonly ElasticsearchClient _elastic;

        public RabbitToElasticWorker(ElasticsearchClient elastic)
        {
            _elastic = elastic;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "service-a.logs",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var log = JsonSerializer.Deserialize<object>(message);

                await _elastic.IndexAsync(log, "service-logs");

                Console.WriteLine($"Log indexed: {message}");
            };

            channel.BasicConsume(
                queue: "service-a.logs",
                autoAck: true,
                consumer: consumer
            );

            return Task.CompletedTask;
        }
    }
}
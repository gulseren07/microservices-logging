using Elastic.Clients.Elasticsearch;
using log_consumer;

var builder = WebApplication.CreateBuilder(args);

// Elasticsearch bağlantısı
var esUrl = builder.Configuration["Elasticsearch:Url"];
builder.Services.AddSingleton(new ElasticsearchClient(new Uri(esUrl)));

// RabbitMQ consumer worker
builder.Services.AddHostedService<RabbitToElasticWorker>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => "log-consumer running");

app.Run();
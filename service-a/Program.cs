using Serilog;
using Serilog.Sinks.RabbitMQ;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.RabbitMQ((clientConfiguration, sinkConfiguration) =>
    {
        // RabbitMQ bağlantı ayarları -> clientConfiguration
        clientConfiguration.Username = "guest";
        clientConfiguration.Password = "guest";
        clientConfiguration.Hostnames = new[] { "localhost" };
        clientConfiguration.Port = 5672;

        // Mesajın nereye gideceği -> Exchange / routing key
        clientConfiguration.Exchange = "logs.topic";
        clientConfiguration.ExchangeType = "topic";
        clientConfiguration.RouteKey = "servicea";
    })
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

Log.Information("Service A started successfully");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
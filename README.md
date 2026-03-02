# Microservices Logging System

This project demonstrates a distributed logging architecture using microservices.

## Technologies Used

- ASP.NET Core (.NET 8)
- Serilog
- RabbitMQ
- Elasticsearch
- Kibana
- Docker

## Architecture

The system contains three services:

### service-a
Produces logs using Serilog and sends them to RabbitMQ.

### service-b
Another microservice that generates logs.

### log-consumer
Consumes logs from RabbitMQ and sends them to Elasticsearch.

## Logging Flow

Service A / Service B  
↓  
Serilog  
↓  
RabbitMQ  
↓  
Log Consumer  
↓  
Elasticsearch  
↓  
Kibana

## Running the Project

1. Start Docker containers (RabbitMQ, Elasticsearch, Kibana)
2. Run service-a
3. Run service-b
4. Run log-consumer

Kibana:
http://localhost:5601

RabbitMQ:
http://localhost:15672

Elasticsearch:
http://localhost:9200

## Author

Gülseren Tartuk

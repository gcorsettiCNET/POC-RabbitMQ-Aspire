using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.TestAspireApp_ApiService>("apiservice");

var rabbitmq = builder.AddRabbitMQ("rabbitmq").WithManagementPlugin();

builder.AddProject<Projects.AsprieTemplate_RabbitMQConsumer>("rabbitmqconsumer")
       .WithReference(rabbitmq);

builder.AddProject<Projects.TestAspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();

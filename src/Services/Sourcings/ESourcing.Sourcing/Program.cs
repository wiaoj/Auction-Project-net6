using ESourcing.Sourcing.Data;
using ESourcing.Sourcing.Data.Abstracts;
using ESourcing.Sourcing.Repositories;
using ESourcing.Sourcing.Repositories.Abstracts;
using ESourcing.Sourcing.Settings;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Producer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

#region Swagger Dependencies
builder.Services.AddSwaggerGen(s => {
    s.SwaggerDoc("v1",
        new OpenApiInfo {
            Title = "Esourcing.Sourcing",
            Version = "v1"
        });
});
#endregion

#region Configuration Dependencies
builder.Services.Configure<SourcingDatabaseSettings>(builder.Configuration.GetSection(nameof(SourcingDatabaseSettings)));

builder.Services.AddSingleton<ISourcingDatabaseSettings>(serviceProvider =>
serviceProvider.GetRequiredService<IOptions<SourcingDatabaseSettings>>().Value);
#endregion

#region Project Dependencies
builder.Services.AddTransient<ISourcingContext, SourcingContext>();
builder.Services.AddTransient<IAuctionRepository, AuctionRepository>();
builder.Services.AddTransient<IBidRepository, BidRepository>();

builder.Services.AddAutoMapper(typeof(Program));
#endregion

#region EventBus
builder.Services.AddSingleton<IRabbitMQPersistentConnection>(serviceProvider => {
    var logger = serviceProvider.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

    var factory = new ConnectionFactory() {
        HostName = builder.Configuration["EventBus:HostName"]
    };

    if(String.IsNullOrWhiteSpace(builder.Configuration["EventBus:UserName"]) is false) {
        factory.UserName = builder.Configuration["EventBus:UserName"];
    }

    if(String.IsNullOrWhiteSpace(builder.Configuration["EventBus:Password"]) is false) {
        factory.UserName = builder.Configuration["EventBus:Password"];
    }

    var retryCount = 5;
    if(String.IsNullOrWhiteSpace(builder.Configuration["EventBus:RetryCount"]) is false) {
        retryCount = int.Parse(builder.Configuration["EventBus:RetryCount"]);
    }

    return new DefaultRabbitMQPersistentConnection(factory, retryCount, logger);

});

builder.Services.AddSingleton<EventBusRabbitMQProducer>();
#endregion

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

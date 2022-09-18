using ESourcing.Order.Extensions;
using Microsoft.OpenApi.Models;
using Orders.Infrastructure;
using Orders.Application;

var builder = WebApplication.CreateBuilder(args);
//var x = MigrationManager.MigrateDatabase();
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

#region Swagger Dependencies
builder.Services.AddSwaggerGen(s => {
    s.SwaggerDoc("v1",
        new OpenApiInfo {
            Title = "Order API",
            Version = "v1"
        });
});
#endregion



builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
//builder.Services.MigrateDatabase();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.MigrateDatabase();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

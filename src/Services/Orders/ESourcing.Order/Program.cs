using ESourcing.Order.Extensions;
using Microsoft.OpenApi.Models;
using Orders.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
//var x = MigrationManager.MigrateDatabase();
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

#region Swagger Dependencies
builder.Services.AddSwaggerGen(s => {
    s.SwaggerDoc("v1",
        new OpenApiInfo {
            Title = "Esourcing.Order",
            Version = "v1"
        });
});
#endregion



builder.Services.AddInfrastructureServices(builder.Configuration);
//builder.Services.MigrateDatabase();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.MigrateDatabase();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

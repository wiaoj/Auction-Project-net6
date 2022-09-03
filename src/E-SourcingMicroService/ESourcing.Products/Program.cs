using ESourcing.Products.Data;
using ESourcing.Products.Data.Abstracts;
using ESourcing.Products.Respositories;
using ESourcing.Products.Respositories.Abstracts;
using ESourcing.Products.Settings;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyHeader().AllowAnyOrigin()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Swagger Dependencies
builder.Services.AddSwaggerGen(s => {
    s.SwaggerDoc("v1",
        new OpenApiInfo {
            Title = "Esourcing.Products",
            Version = "v1"
        });
});
#endregion

#region Configuration Dependencies
builder.Services.Configure<ProductDatabaseSettings>(builder.Configuration.GetSection(nameof(ProductDatabaseSettings)));

builder.Services.AddSingleton<IProductDatabaseSettings>(serviceProvider =>
serviceProvider.GetRequiredService<IOptions<ProductDatabaseSettings>>().Value);
#endregion

#region Project Dependencies
builder.Services.AddTransient<IProductContext, ProductContext>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ESourcing.Products v1"));
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

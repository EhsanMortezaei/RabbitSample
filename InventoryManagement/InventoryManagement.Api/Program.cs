using InventoryManagement.Application.Interface;
using InventoryManagement.Domain.Interface;
using InventoryManagement.Infrastructure;
using InventoryManagement.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderManagementDb")));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IMessageBus, RabbitMqMessageBus>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();  


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var consumer = new RabbitMqConsumer();
consumer.StartConsuming();
app.Run();

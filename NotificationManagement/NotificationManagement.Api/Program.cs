using Microsoft.EntityFrameworkCore;
using NotificationManagement.Application.Interface;
using NotificationManagement.Infrastructure;
using NotificationManagement.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderManagementDb")));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IMessageBus, RabbitMqMessageBus>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var consumer = new NotificationConsumer();
consumer.StartConsuming();

app.Run();

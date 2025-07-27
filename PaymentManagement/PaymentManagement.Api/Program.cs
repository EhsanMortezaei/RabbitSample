using Microsoft.EntityFrameworkCore;
using PaymentManagement.Application.Interface;
using PaymentManagement.Infrastructure;
using PaymentManagement.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PaymentDbContext>(options =>
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

var consumer = new PaymentConsumer();
consumer.StartConsuming();

app.Run();

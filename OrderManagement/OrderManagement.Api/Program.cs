using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.CreateOrder;
using OrderManagement.Application.Interface;
using OrderManagement.Domain.Interface;
using OrderManagement.Infrastructure;
using OrderManagement.Infrastructure.Commands.Orders;
using OrderManagement.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderManagementDb")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMessageBus, RabbitMqMessageBus>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommandHandler).Assembly);
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



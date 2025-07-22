using Microsoft.EntityFrameworkCore;
using NotificationManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderManagementDb")));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

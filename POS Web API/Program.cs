using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POS_API.Services;
using Microsoft.AspNetCore.SignalR; // Add SignalR namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services
builder.Services.AddScoped<IServiceProduct, ServiceProducts>();
builder.Services.AddScoped<IServiceTransaction, ServiceTransaction>();
builder.Services.AddScoped<IServiceCounter, ServiceCounter>();
builder.Services.AddLogging();

// Configure DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Add SignalR services
builder.Services.AddSignalR(); // Add SignalR services here

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// Map SignalR hubs
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub"); // Map the SignalR hub

app.Run();

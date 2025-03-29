using FLearning.NotificationService;
using FLearning.NotificationService.Data;
using FLearning.NotificationService.Repositories;
using FLearning.NotificationService.Services;
using Microsoft.EntityFrameworkCore;
using SendGrid;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NotificationDB")));

// Đăng ký repositories
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// Đăng ký services
builder.Services.AddScoped<INotificationService, NotificationService>();

// Đăng ký SendGrid
builder.Services.AddSingleton(provider =>
    new SendGridClient(builder.Configuration["SendGrid:ApiKey"]));

// Đăng ký background service
builder.Services.AddHostedService<NotificationBackgroundService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Tự động tạo database khi chạy trong môi trường development
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
        db.Database.Migrate();
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

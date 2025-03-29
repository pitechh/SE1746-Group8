using FLearning.ContentService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Thêm CORS (cho dev)
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 2. Đăng ký DbContext sử dụng SQL Server
string? connString = builder.Configuration.GetConnectionString("ContentDB");
builder.Services.AddDbContext<ContentDbContext>(options =>
{
    options.UseSqlServer(connString);
});

// 3. Đăng ký Controllers và Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Content API", Version = "v1" });
    // Nếu cần, khai báo server URL thủ công:
    // c.AddServer(new OpenApiServer { Url = "https://localhost:YOUR_HTTPS_PORT" });
});

var app = builder.Build();

// 4. Cấu hình Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Cho phép truy cập file tĩnh (để hiển thị file media từ wwwroot)
app.UseStaticFiles();

// Sử dụng CORS (đặt trước MapControllers)
app.UseCors("DevCors");

app.UseAuthorization();
app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using SalesPointServices.Data;
using SalesPointServices.Services.RabbitMQ;
using SalesPointServices.Services.Core;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Добавляем InMemory Database для хранения данных о точках продаж
builder.Services.AddDbContext<SalesPointDbContext>(options =>
    options.UseInMemoryDatabase("SalesPointDB"));

// Регистрируем сервисы
builder.Services.AddSingleton<ProductPriceRequester>();
builder.Services.AddScoped<ISalesPointService, SalesPointService>();

// Добавляем поддержку контроллеров
builder.Services.AddControllers();

// Настройка Swagger для автоматической документации API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SalesPoint API",
        Version = "v1",
        Description = "API для управления магазинами и продажами",
        Contact = new OpenApiContact
        {
            Name = "Support",
            Email = "support@example.com"
        }
    });

    // Подключаем XML-документацию
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Заполняем базу данных начальными данными при старте приложения
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SalesPointDbContext>();
    DataSeeder.SeedDatabase(dbContext);
}

// Включаем Swagger в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Настройка HTTPS и маршрутов API
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

using Microsoft.EntityFrameworkCore;
using ProductServices.Data;
using ProductServices.Services.RabbitMQ;
using ProductServices.Services.Interfaces;
using ProductServices.Services.Core;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Добавляем InMemory Database для хранения данных о продуктах
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseInMemoryDatabase("ProductDb"));

// Регистрируем сервис управления продуктами (DI)
builder.Services.AddScoped<IProductService, ProductService>();

// Добавляем обработчик сообщений RabbitMQ
builder.Services.AddHostedService<ProductMessageConsumer>();

// Добавляем поддержку контроллеров
builder.Services.AddControllers();

// Настройка Swagger для автоматической документации API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product API",
        Version = "v1",
        Description = "API для управления товарами",
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
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
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
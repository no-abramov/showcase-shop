using Microsoft.EntityFrameworkCore;
using ProductServices.Data;
using ProductServices.Services.RabbitMQ;
using ProductServices.Services.Interfaces;
using ProductServices.Services.Core;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ��������� InMemory Database ��� �������� ������ � ���������
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseInMemoryDatabase("ProductDb"));

// ������������ ������ ���������� ���������� (DI)
builder.Services.AddScoped<IProductService, ProductService>();

// ��������� ���������� ��������� RabbitMQ
builder.Services.AddHostedService<ProductMessageConsumer>();

// ��������� ��������� ������������
builder.Services.AddControllers();

// ��������� Swagger ��� �������������� ������������ API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product API",
        Version = "v1",
        Description = "API ��� ���������� ��������",
        Contact = new OpenApiContact
        {
            Name = "Support",
            Email = "support@example.com"
        }
    });

    // ���������� XML-������������
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// ��������� ���� ������ ���������� ������� ��� ������ ����������
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    DataSeeder.SeedDatabase(dbContext);
}

// �������� Swagger � ������ ����������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ��������� HTTPS � ��������� API
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
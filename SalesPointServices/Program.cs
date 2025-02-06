using Microsoft.EntityFrameworkCore;
using SalesPointServices.Data;
using SalesPointServices.Services.RabbitMQ;
using SalesPointServices.Services.Core;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ��������� InMemory Database ��� �������� ������ � ������ ������
builder.Services.AddDbContext<SalesPointDbContext>(options =>
    options.UseInMemoryDatabase("SalesPointDB"));

// ������������ �������
builder.Services.AddSingleton<ProductPriceRequester>();
builder.Services.AddScoped<ISalesPointService, SalesPointService>();

// ��������� ��������� ������������
builder.Services.AddControllers();

// ��������� Swagger ��� �������������� ������������ API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SalesPoint API",
        Version = "v1",
        Description = "API ��� ���������� ���������� � ���������",
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
    var dbContext = scope.ServiceProvider.GetRequiredService<SalesPointDbContext>();
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

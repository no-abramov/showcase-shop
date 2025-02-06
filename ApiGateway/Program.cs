using Microsoft.AspNetCore.Diagnostics;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ��������� ��������� Ocelot
builder.Services.AddOcelot();
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

// ��������� ������������ Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

var app = builder.Build();

// ���������� ���������� ������
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error != null)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(exceptionHandlerPathFeature.Error, "������ � Ocelot API Gateway");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("��������� ���������� ������ �������");
        }
    });
});

// ����������� (statusCode 400+) ��������
app.Use(async (context, next) =>
{
    await next();

    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var statusCode = context.Response.StatusCode;

    switch (statusCode)
    {
        case 400:
            logger.LogWarning($"������ 400 (Bad Request): {context.Request.Method} {context.Request.Path}");
            break;
        case 401:
            logger.LogWarning($"������ 401 (Unauthorized): {context.Request.Method} {context.Request.Path}");
            break;
        case 403:
            logger.LogWarning($"������ 403 (Forbidden): {context.Request.Method} {context.Request.Path}");
            break;
        case 404:
            logger.LogWarning($"������ 404 (Not Found): {context.Request.Method} {context.Request.Path}");
            break;
        case 500:
            logger.LogError($"������ 500 (Internal Server Error): {context.Request.Method} {context.Request.Path}");
            break;
        default:
            if (statusCode >= 400)
            {
                logger.LogWarning($"������ {statusCode}: {context.Request.Method} {context.Request.Path}");
            }
            break;
    }

    // ��������� 502 Bad Gateway
    if (statusCode == 502)
    {
        logger.LogError($"������ 502 (Bad Gateway): ������ ���������� {context.Request.Method} {context.Request.Path}");

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\": \"������ �������� ����������. ��������� ������� �����.\"}");
    }
});

// ���������� Ocelot
await app.UseOcelot();

app.Run();